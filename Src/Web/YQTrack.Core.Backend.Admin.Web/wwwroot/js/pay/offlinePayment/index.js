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
        url: '/Pay/OfflinePayment/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'offlinePaymentId', title: '线下交易ID', hide: true, width: 180 },
            { field: 'createAt', title: '申请时间', width: 160 },
            { field: 'email', title: '注册邮箱' },
            {
                field: 'purchaseOrderIdList', title: '订单号', templet: function (d) {
                    var html = '';
                    for (let index in d.purchaseOrderIdList) {
                        html += '<a class="layui-table-link" lay-event="viewPurchaseOrder" lay-filter="viewPurchaseOrder" value="' + d.purchaseOrderIdList[index] + '">' + d.purchaseOrderIdList[index] + '</a><br>';
                    };
                    return html;
                }, width: 180, align: 'center'
            },
            {
                field: 'amount', title: '交易金额', templet: function (d) {
                    return d.currencyType + d.amount;
                }, width: 140
            },
            {
                field: 'calculateAmount', title: '计算金额', templet: function (d) {
                    return d.currencyType + d.calculateAmount;
                }, width: 140
            },
            { field: 'transferNo', title: '转账流水号' },
            //{ field: 'providerId', title: '支付方式' },
            { field: 'status', title: '状态' },
            { field: 'handleTime', title: '审核时间', width: 160, align: 'center' },
            {
                field: 'operate', title: '操作', templet: function (d) {
                    return '<a class="layui-btn layui-btn layui-btn-xs" lay-event="view" lay-filter="view" value="' + d.offlinePaymentId + '">详情</a>';
                }, width: 68
            }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            email: objWhere.email,
            status: objWhere.status
        },
        done: function (res, curr, count) {
            ////如果是异步请求数据方式，res即为你接口返回的信息。
            ////如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
            //console.log(res);
            ////得到当前页码
            //console.log(curr);
            ////得到当前页大小
            //console.log(res.data.length);
            ////得到数据总量
            //console.log(count);
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    $("#search").on("click", function () {
        var data = decodeURIComponent($("form").serialize(), true).split("&");
        var object = {};
        for (var key in data) {
            object[data[key].split("=")[0]] = (data[key].split("=")[1]).replace(/\+/g, " ");
        }
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    email: object.email,
                    status: object.status
                }
            });
        return true;
    });
    //监听工具条
    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
        switch (obj.event) {
            case 'view'://查看
                window.top.addMyTab('/pay/offlinepayment/view?id=' + obj.data.offlinePaymentId, '线下交易详情', curmenu.topMenuName, curmenu.leftMenuName);
                return false;
                break;
            case 'viewPurchaseOrder':
                window.top.addMyTab('/pay/purchaseorder/viewproduct?id=' + $(this).attr('value'), "订单详情", curmenu.topMenuName, curmenu.leftMenuName);//$(this).attr('value')
                return false;
                break;
        }
    });

});