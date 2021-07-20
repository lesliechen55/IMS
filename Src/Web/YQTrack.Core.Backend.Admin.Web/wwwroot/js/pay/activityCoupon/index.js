layui.use(['layer', 'jquery', 'table', 'form', 'laydate'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var laydate = layui.laydate;
    laydate.render({
        elem: '#startTime'
    });
    laydate.render({
        elem: '#endTime'
    });

    var objWhere = top.initFilter(window);
    if (objWhere.activityName != 'undefined') {
        $("#activityName").val('');
        objWhere.activityName = '';//刷新页面清空输入框
    }

    table.render({
        elem: '#table',
        method: 'post',
        url: '/Pay/ActivityCoupon/GetPageData',
        cellMinWidth: 60,
        checkStatus: false,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        height: "full-" + objWhere.top,
        cols: [[
            {
                field: 'activityCouponId', title: '优惠券ID', width: 180
            },
            {
                field: 'activityName', title: '活动名称', width: 180
            },
            {
                field: 'purchaseOrderId', title: '订单编号', templet: function (d) {
                    if (d.purchaseOrderId == null) {
                        return '';
                    }
                    else {
                        return '<a class="layui-table-link" lay-event="view" lay-filter="view">' + d.purchaseOrderId + '</a>';
                    }
                }, width: 180
            },
            {
                field: 'userId', title: '用户ID', templet: function (d) {
                    if (d.userId == '0') {
                        return d.userId;
                    }
                    else {
                        return '<a class="layui-table-link" lay-event="viewUser" lay-filter="viewUser">' + d.userId + '</a>';
                    }
                }, width: 200
            },
            { field: 'email', title: '用户Email', width: 180 },
            {
                field: 'startTime', title: '开始时间', templet: function (d) {
                    return layui.util.toDateString(d.startTime, 'yyyy-MM-dd')
                }
            },
            {
                field: 'endTime', title: '结束时间', templet: function (d) {
                    return layui.util.toDateString(d.endTime, 'yyyy-MM-dd')
                }
            },
            {
                field: 'status', title: '状态', templet: function (d) {
                    if (d.status == 1) {
                        return "未使用";
                    } else if (d.status == 2) {
                        return "已使用";
                    } else if (d.status == 0) {
                        return "None";
                    }
                }
            },
            { field: 'rule', title: '享受的优惠', width: 230 },
            { field: 'actualDiscount', title: '优惠金额' },
            {
                field: 'source', title: '来源', templet: function (d) {
                    switch (d.source) {
                        case "0":
                        case "":
                            return "None";
                            break;
                        case "1":
                            return "Email";
                            break;
                        case "2":
                            return "Twitter";
                            break;
                        case "3":
                            return "Facebook";
                            break;
                        case "4":
                            return "Weibo";
                            break;
                        case "5":
                            return "QQ";
                            break;
                        case "6":
                            return "Weixin";
                            break;

                        default:
                            return d.source;
                            break;
                    }
                }
            }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        }
        ,
        where: {
            //activityId: objWhere.activityId,
            activityName: objWhere.activityName,
            purchaseOrderId: objWhere.purchaseOrderId,
            status: objWhere.status,
            userEmail: objWhere.userEmail,
            userId: objWhere.userId
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
        var params = get_parameters();
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    //activityId: params.activityId,
                    activityName: params.activityName,
                    purchaseOrderId: params.purchaseOrderId,
                    status: params.status,
                    userEmail: params.userEmail,
                    userId: params.userId
                }
            });
        return true;
    });

    //监听工具条
    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
        switch (obj.event) {
            case 'view'://查看
                window.top.addMyTab('/pay/purchaseorder/viewproduct?id=' + obj.data.purchaseOrderId, "订单详情", curmenu.topMenuName, curmenu.leftMenuName);
                break;
            case 'viewUser'://查看用户
                window.top.addMyTab('/business/user/detail?userId=' + obj.data.userId, '用户详情', 'systemcenter', curmenu.leftMenuName);
                break;
        }
    });

    function get_parameters() {
        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]).replace(/\+/g, " ");
            }
        }
        return object;
    };
});