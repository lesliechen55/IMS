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
        elem: '#startTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2019-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#endTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2019-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    var carrierTable = table.render({
        elem: '#table',
        method: 'post',
        url: null,
        height: "full-" + objWhere.top,
        cellMinWidth: 90,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'email', title: '邮箱', width: 300 },
            { field: 'description', title: '描述', width: 150 },
            { field: 'detail', title: '操作详情' },
            { field: 'createTime', title: '创建时间', width: 160 }
        ]],
        parseData: function (res) {
            if (!res.success) {
                return {
                    "code": 0, //解析接口状态
                    "msg": res.msg, //解析提示文本
                    "count": 0, //解析数据长度
                    "data": [], //解析数据列表
                    "success": false
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
            email: objWhere.userEmail
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
            if (res && res.success !== undefined && !res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
            }
        }
    });

    $("#search").on("click", function () {
        var email = $('#userEmail').val();

        if ($.trim(email).length <= 0) {
            layer.alert('请填写用户邮箱', { title: '错误', icon: 5 });
            return false;
        }
        carrierTable.config.url = '/CarrierTrack/Statistics/GetUserMarkLogPageData';
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    email: email
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