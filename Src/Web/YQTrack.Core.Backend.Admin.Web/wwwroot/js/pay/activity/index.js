layui.use(['layer', 'jquery', 'table', 'form', 'laydate', 'laytpl'], function () {
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
    table.on('toolbar(table)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加活动',
                    content: '/Pay/Activity/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['800px', '930px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
        }
    });

    //监听工具条
    table.on('tool(table)', function (obj) {
        switch (obj.event) {
            case 'update':
                var title = obj.data.cnName;
                var id = obj.data.activityId;
                
                layer.open({
                    type: 2,
                    title: '编辑活动 ' + title,
                    content: '/Pay/Activity/Edit?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['800px', '930px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;

            case 'updateStatus':
                var id = obj.data.activityId;
                var status = $(this).attr("attrStats");
                UpdateStatus(id, status);
                break;

            case 'addCoupon':
                var id = obj.data.activityId;
                var title = obj.data.cnName;
                layer.open({
                    type: 2,
                    title: '发放优惠券—' + title,
                    content: '/Pay/Activity/AddCoupon?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['500px', '400px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;

            case 'details':
                var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
                window.top.addMyTab('/Pay/Activity/view?id=' + obj.data.activityId, '查看活动详情', curmenu.topMenuName, curmenu.leftMenuName);
                break;
        }
    });

    table.render({
        elem: '#table',
        method: 'post',
        url: '/Pay/Activity/GetPageData',
        cellMinWidth: 60,
        checkStatus: false,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        height: "full-" + objWhere.top,
        cols: [[
            { type: 'numbers' },
            /*{ type: 'radio' },*/
            {
                field: 'cnName', title: '活动名称'
            },
            { field: 'enName', title: '英文名称' },
            { field: 'activityType', title: '活动类型', width: 100 },
            { field: 'discountType', title: '优惠类型', width: 100 },
            { field: 'couponMode', title: '优惠模式', width: 180 },
            {
                field: 'startTime', title: '开始时间', width: 120, templet: function (d) {
                    return layui.util.toDateString(d.startTime, 'yyyy-MM-dd')
                }
            },
            {
                field: 'endTime', title: '截至时间', width: 120, templet: function (d) {
                    return layui.util.toDateString(d.endTime, 'yyyy-MM-dd')
                }
            },
            {
                field: 'status', title: '状态', width: 100, templet: function (d) {
                    if (d.status == "未知") {
                        return '<a class="layui-btn layui-btn-primary layui-btn-xs">None</a>';//白色

                    } else if (d.status == "待审核") {
                        return '<a class="layui-btn layui-btn-xs">待审核</a>';//墨绿

                    } else if (d.status == "审核通过") {
                        return '<a class="layui-btn layui-btn-normal layui-btn-xs">审核通过</a>';//蓝色

                    } else if (d.status == "已上架") {
                        return '<a class="layui-btn layui-btn-warm layui-btn-xs">已上架</a>';//黄色

                    } else if (d.status == "已驳回") {
                        return '<a class="layui-btn layui-btn-danger layui-btn-xs">已驳回</a>';//橙色

                    } else {
                        //已结束和已下架
                        return '<a class="layui-btn layui-btn-disabled layui-btn-xs">' + d.status + '</a>';//禁用
                    }
                }
            },
            {
                field: 'internalUse', title: '内部使用', templet: function (d) {
                    return '<input type="radio" title="' + (d.internalUse ? '是' : '否') + (d.internalUse ? '" checked' : '"') + '>';
                }, width: 100
            },
            {
                field: 'operate', title: '操作', templet: function (d) {
                    var str = '<a class="layui-btn layui-btn-primary layui-btn-xs" lay-event="update" value="' + d.activityId + '">编辑</a>';
                    var viewStr = '<a class="layui-btn layui-btn-primary layui-btn-xs" lay-event="details" value="' + d.activityId + '">详情</a>';

                    if (d.status == "未知") {
                        str += '<button class="layui-btn layui-btn-xs" lay-event="updateStatus" attrStats="1">提交审核</button>';
                        return viewStr+str;
                    } else if (d.status == "待审核") {
                        //可以审核通过 也可以驳回
                        str += '<button class="layui-btn layui-btn-normal layui-btn-xs" lay-event="updateStatus" attrStats="2">通过</button><button class="layui-btn layui-btn-danger layui-btn-xs" lay-event="updateStatus" attrStats="6">驳回</button>';
                        return viewStr+str;
                    } else if (d.status == "审核通过") {
                        //可以上架也可以下架
                        return viewStr+'<button class="layui-btn layui-btn-warm layui-btn-xs" lay-event="updateStatus" attrStats="3">上架</button><button class="layui-btn layui-btn-primary layui-btn-xs" lay-event="updateStatus" attrStats="5">下架</button>';

                    } else if (d.status == "已上架") {
                        if (d.activityType == "通用活动") {  
                            return viewStr+'<button class="layui-btn layui-btn-primary layui-btn-xs" lay-event="updateStatus" attrStats="5">下架</button>';
                        }
                        //可以下架,可以发放优惠券
                        return viewStr+'<button class="layui-btn layui-btn-primary layui-btn-xs" lay-event="updateStatus" attrStats="5">下架</button><button class="layui-btn layui-btn-green layui-btn-sm" lay-event="addCoupon">发放优惠券</button>';

                    } else if (d.status == "已驳回") {
                        //可以回到初始状态
                        return viewStr+'<button class="layui-btn layui-btn-primary layui-btn-xs" lay-event="updateStatus" attrStats="0">重置</button>';
                    } else {
                        //已结束或已下架 不可以有任何操作
                        return viewStr;
                    }
                }, width: 220
            }

        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        }
        ,
        where: {
            keyword: objWhere.keyword,
            productId: objWhere.productId,
            activityType: objWhere.activityType,
            activityStatus: objWhere.activityStatus,
            startTime: objWhere.startTime,
            endTime: objWhere.endTime
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
                    keyword: params.keyword,
                    productId: params.productId,
                    activityType: params.activityType,
                    activityStatus: params.activityStatus,
                    startTime: params.startTime,
                    endTime: params.endTime
                }
            });
        return true;
    });

    function UpdateStatus(id, status) {
        $.ajax({
            url: "/pay/activity/changeStatus",
            method: 'post',
            data: { activityId: id, status: status },
            dataType: "json",
            success: function (resultData) {
                if (resultData.success) {
                    layer.msg('操作成功');
                    $(".layui-laypage-btn")[0].click();
                } else {
                    layer.msg('操作失败');
                }
            }
        })
    }

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