layui.use(['form', 'layer', 'table'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var objWhere = top.initFilter(window);

    var tableIns = table.render({
        elem: '#table',
        method: 'post',
        url: null,
        data: [],
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'id', title: 'ID', width: 80, hide: true },
            { field: 'userId', title: '用户ID', width: 180 },
            { field: 'email', title: '邮箱' },
            { field: 'userRole', title: '角色', width: 120 },
            { field: 'nodeId', title: '节点'},
            { field: 'dbNo', title: '数据库编号'},
            { field: 'tableNo', title: '数据表编号'},
            { field: 'unRegisterTime', title: '注销时间', width: 160 },
            { field: 'completedTime', title: '完成时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            userId: objWhere.userId,
            email: objWhere.email
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

        if ($.trim($('#userId').val()).length <= 0 && $.trim($('#email').val()).length <= 0) {
            layer.alert('操作失败:搜索参数至少包含UserId或者Email其一', { title: '错误', icon: 5 });
            return false;
        }

        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            }
        }
        tableIns.config.url = '/Business/UserUnregister/GetPageData';
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    userId: object.userId,
                    email: object.email
                }
            });
        return true;
    });

    if ($.trim($('#userId').val()).length > 0 || $.trim($('#email').val()).length > 0) {
        $("#search").click();
    }

});