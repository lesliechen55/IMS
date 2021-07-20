layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var objWhere = top.initFilter(window);
    table.render({
        elem: '#table',
        method: 'post',
        url: '/Pay/InvoiceApply/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'invoiceApplyId', title: '发票申请ID', hide: true, width: 180 },
            { field: 'companyName', title: '公司名称' },
            { field: 'createAt', title: '申请时间', width: 160 },
            { field: 'email', title: '注册邮箱' },
            { field: 'invoiceType', title: '发票类型', width: 130 },
            {
                field: 'amount', title: '总金额', templet: function (d) {
                    return d.currencyType + d.amount;
                }, width: 140
            },
            { field: 'status', title: '状态' },
            { field: 'handleTime', title: '审核时间', width: 160 },
            { field: 'sendInfo', title: '派送信息' },
            {
                field: 'operate', title: '操作', templet: function (d) {
                    return '<a class="layui-btn layui-btn layui-btn-xs" lay-event="view" lay-filter="view" value="' + d.invoiceApplyId + '">详情</a>';
                }, width: 68
            }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            userEmail: objWhere.userEmail,
            status: objWhere.status
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    $("#search").on("click", function () {
        var data = decodeURIComponent($("form").serialize(), true).split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = (data[key].split("=")[1]).replace(/\+/g, " ");
            }
        }
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    userEmail: object.userEmail,
                    status: object.status
                }
            });
        return true;
    });
    //监听工具条
    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        switch (obj.event) {
            case 'view'://查看
                var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
                window.top.addMyTab('/pay/invoiceapply/view?id=' + obj.data.invoiceApplyId, '申请开票详情', curmenu.topMenuName, curmenu.leftMenuName);
                break;
        }
    });

});