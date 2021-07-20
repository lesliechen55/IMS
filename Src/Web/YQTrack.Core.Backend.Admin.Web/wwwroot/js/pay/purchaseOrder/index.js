layui.use(['form', 'layer', 'table', 'laytpl', 'laydate'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table,
        formSelects = layui.formSelects;

    var objWhere = top.initFilter(window, formSelects);
    //执行一个laydate实例
    layui.laydate.render({
        elem: '#startTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2015-1-1',
        max: '2050-12-31',
        lang: 'cn',
        theme: 'molv',
        //range: '~',//或 range: '~' 来自定义分割字符
        calendar: true,
        done: function (value, date, endDate) {
            //console.log(value); //得到日期生成的值，如：2017-08-18
            //console.log(date); //得到日期时间对象：{year: 2017, month: 8, date: 18, hours: 0, minutes: 0, seconds: 0}
            //console.log(endDate); //得结束的日期时间对象，开启范围选择（range: true）才会返回。对象成员同上。
            var endTime = $('#endTime').val();
            if (endTime && value) {
                if (value > endTime) {
                    layer.alert('开始日期不能大于结束日期');
                    $('#startTime').val("");
                }
            }
        }
    });
    //执行一个laydate实例
    layui.laydate.render({
        elem: '#endTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2015-1-1',
        max: '2050-12-31',
        lang: 'cn',
        theme: 'molv',
        //range: '~',//或 range: '~' 来自定义分割字符
        calendar: true,
        done: function (value, date, endDate) {
            var startTime = $('#startTime').val();
            if (startTime && value) {
                if (value < startTime) {
                    layer.alert('开始日期不能大于结束日期');
                    $('#endTime').val("");
                }
            }
        }
    });

    table.render({
        elem: '#table',
        method: 'post',
        url: '/Pay/PurchaseOrder/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            {
                field: 'purchaseOrderId', title: '订单编号', templet: function (d) {
                    return '<a class="layui-table-link" lay-event="view" lay-filter="view">' + d.purchaseOrderId + '</a>';
                }, width: 180
            },
            { field: 'name', title: '订单名称', width: 150 },
            { field: 'userPlatformType', title: '平台类型', width: 88 },
            { field: 'currencyType', title: '货币类型', width: 88 },
            { field: 'serviceType', title: '服务类型', width: 88 },
            //{ field: 'salePrice', title: '销售价', width: 88 },
            { field: 'paymentAmount', title: '实付价', width: 88 },
            {
                field: 'userId', title: '用户ID', templet: function (d) {
                    if (d.userId == '0') {
                        return d.userId;
                    }
                    else {
                        return '<a class="layui-table-link" lay-event="viewUser" lay-filter="viewUser">' + d.userId + '</a>';
                    }
                }, width: 180
            },
            { field: 'nickName', title: '用户名称', width: 160 },
            { field: 'providerId', title: '支付方式', width: 125 },
            { field: 'status', title: '状态', width: 100 },
            { field: 'conflict', title: '是否冲突', width: 88 },
            { field: 'createAt', title: '创建时间', width: 160 },
            { field: 'updateAt', title: '修改时间', width: 160 },
            { field: 'originalOrderId', title: '源订单号', width: 180, hide: true }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            orderId: objWhere.orderId,
            userId: objWhere.userId,
            email: objWhere.email,
            name: objWhere.name,
            platformType: formSelects.value('platformType', 'val'),
            currencyType: objWhere.currencyType,
            serviceType: formSelects.value('serviceType', 'val'),
            providerId: formSelects.value('providerId', 'val'),
            purchaseOrderStatus: formSelects.value('purchaseOrderStatus', 'val'),
            startTime: objWhere.startTime,
            endTime: objWhere.endTime,
            conflictOrder: objWhere.conflictOrder
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
    //监听工具条
    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
        switch (obj.event) {
            case 'view'://查看
                window.top.addMyTab('/pay/purchaseorder/viewproduct?id=' + obj.data.purchaseOrderId, "订单 " + obj.data.name, curmenu.topMenuName, curmenu.leftMenuName);
                break;
            case 'viewUser'://查看用户
                window.top.addMyTab('/business/user/detail?userId=' + obj.data.userId, '用户详情', 'systemcenter', curmenu.leftMenuName);
                break;
        }
    });
    $("#search").on("click", function () {
        var data = decodeURIComponent($("form").serialize(), true).split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = data[key].split("=")[1].replace(/\+/g, " ");
            }
        }
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    orderId: object.orderId,
                    userId: object.userId,
                    email: object.email,
                    name: object.name,
                    platformType: formSelects.value('platformType', 'val'),
                    currencyType: object.currencyType,
                    serviceType: formSelects.value('serviceType', 'val'),
                    providerId: formSelects.value('providerId', 'val'),
                    purchaseOrderStatus: formSelects.value('purchaseOrderStatus', 'val'),
                    startTime: object.startTime,
                    endTime: object.endTime,
                    conflictOrder: object.conflictOrder
                }
            });
        return true;
    });
});