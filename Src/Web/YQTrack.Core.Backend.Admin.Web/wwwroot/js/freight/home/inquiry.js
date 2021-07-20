layui.use(['laydate', 'form', 'layer', 'table', 'laytpl'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var objWhere = top.initFilter(window);

    var laydate = layui.laydate;

    //执行一个laydate实例
    laydate.render({
        elem: '#publishStartTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#publishEndTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#expireStartTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#expireEndTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    table.render({
        elem: '#table',
        method: 'post',
        url: '/Freight/Home/GetInquiryList/',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            {
                field: "id", title: '询价单ID', templet: '<div><a target="_blank" href="https://freight.17track.net/en/inqury-detail/{{d.id}}" class="layui-table-link">{{d.id}}</a></div>'
            },
            { field: 'title', title: '标题' },
            { field: 'inquiryNo', title: '询价单号', minWidth: 170 },
            { field: 'userId', title: '用户ID' },
            { field: 'userUniqueId', title: '用户短标识' },
            { field: 'packageCity', title: '揽件城市' },
            { field: 'deliveryCountry', title: '派送国家' },
            { field: 'publishDateTime', title: '发布时间', minWidth: 170 },
            { field: 'status', title: '审核状态' },
            { field: 'statusTime', title: '状态变更时间' },
            { field: 'processTime', title: '处理时间' },
            { field: 'LogisticsRequire', title: '物流要求' },
            { field: 'contactInfo', title: '联系人信息' },
            { field: 'quoterCount', title: '报价数量' },
            { field: 'viewerCount', title: '浏览次数' },
            { field: 'expireDate', title: '截止时间', minWidth: 170}
        ]],
        parseData: function (res) {
            if (!res.success) {
                return {
                    "code": 0, //解析接口状态
                    "msg": res.msg, //解析提示文本
                    "count": 0, //解析数据长度
                    "data": [] //解析数据列表
                };
            }
            return res;
        },
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            id: objWhere.id,
            title: objWhere.title,
            inquiryNo: objWhere.inquiryNo,
            status: objWhere.status,
            publisher: objWhere.publisher,
            publishStartTime: objWhere. publishStartTime,
            publishEndTime: objWhere.publishEndTime,
            expireStartTime: objWhere.expireStartTime,
            expireEndTime: objWhere.expireEndTime
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
            }
        }
    });

    //列表操作
    table.on('toolbar(inquiryTable)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        if (obj.event === 'reject') {
            if (checkStatus.data.length !== 1) {
                layer.msg('请先选择有且仅有一个询价单');
                return;
            }
            var data = checkStatus.data[0];
            if (data.status === '交易失败' || data.status === '管理员下架') {
                layer.msg('交易失败或者管理员下架状态,不能下架!!!');
                return;
            }

            if (data.status === '交易成功') {
                layer.confirm('此询价单处于"交易成功"状态,是否继续下架?', { icon: 3, title: '提示' }, function (index) {
                    openRejectDialog(data.inquiryNo, data.id, window.name, 'search');
                    layer.close(index);
                });
                return;
            }
            openRejectDialog(data.inquiryNo, data.id, window.name, 'search');
        }
    });

    function openRejectDialog(title, id, sourceIframeName, invokeElementId) {
        layer.open({
            type: 2,
            shade: 0.3,
            title: '下架-询价单号: ' + title,
            skin: 'layui-layer-lan', //加上边框
            area: ['500px', '360px'], //宽高
            content: '/Freight/InquiryManage/Index?orderId=' + id + '&sourceIframeName=' + sourceIframeName + '&invokeElementId=' + invokeElementId
        });
    }

    $("#search").on("click", function () {
        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            }
        }

        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    id: object.id,
                    title: object.title,
                    inquiryNo: object.inquiryNo,
                    status: object.status,
                    publisher: object.publisher,
                    publishStartTime: object.publishStartTime,
                    publishEndTime: object.publishEndTime,
                    expireStartTime: object.expireStartTime,
                    expireEndTime: object.expireEndTime
                }
            });
    });

    $("#export").on("click", function () {

        var url = $(this).prop('href') + '?';

        var data = $("form").serialize().split("&");
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                url = url + data[key].split("=")[0] + '=' + decodeURIComponent(data[key].split("=")[1]);
                url = url + '&';
            }
        }
        url = url.slice(0, -1);

        $.fileDownload(url)
            .done(function (result) {
                layer.msg('文件下载成功请保存到您的电脑上面!', { icon: 1 });
            })
            .fail(function (failHtml) {
                layer.open({
                    title: '文件下载失败',
                    content: failHtml
                });
            });
        return false; //this is critical to stop the click event which will trigger a normal file download 
    });
});