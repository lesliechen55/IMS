layui.use(['laydate', 'form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var objWhere = top.initFilter(window);
    var laydate = layui.laydate;

    laydate.render({
        elem: '#startTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#endTime', //指定元素
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
        url: '/Freight/Home/GetQuotePageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 50,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'quoteId', title: '报价ID', minWidth: 180},
            { field: 'quoteOrderNo', title: '竞价单号', minWidth: 175 },
            { field: 'orderId', title: '询价ID', hide: true },
            { field: 'inquiryOrderNo', title: '询价单号', minWidth: 175 },
            { field: 'status', title: '询价单状态', minWidth: 100 },
            { field: 'packageCity', title: '揽件城市' },
            { field: 'deliveryCountry', title: '派送国家' },
            { field: 'userId', title: '用户ID', hide: true },
            { field: 'companyId', title: '公司ID', hide: true },
            { field: 'companyName', title: '公司名称' },
            { field: 'content', title: '报价内容' },
            { field: 'remark', title: '备注' },
            { field: 'cancel', title: '是否撤销' },
            { field: 'cancelTime', title: '撤销时间' },
            { field: 'cancelReason', title: '撤销原因' },
            { field: 'viewed', title: 'Seller是否查看', minWidth: 130  },
            { field: 'createTime', title: '竞价时间' }
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
            quoteId: objWhere.quoteId,
            quoteNo: objWhere.quoteNo,
            inquiryNo: objWhere.inquiryNo,
            companyName: objWhere.companyName,
            startTime: objWhere.startTime,
            endTime: objWhere.endTime,
            inquiryStatus: objWhere.inquiryStatus,
            cancelStatus: objWhere.cancelStatus
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
                    quoteId: object.quoteId,
                    quoteNo: object.quoteNo,
                    inquiryNo: object.inquiryNo,
                    companyName: object.companyName,
                    startTime: object.startTime,
                    endTime: object.endTime,
                    inquiryStatus: object.inquiryStatus,
                    cancelStatus: object.cancelStatus
                }
            });
        return true;
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