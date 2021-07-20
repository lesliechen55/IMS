layui.use(['form', 'layer', 'table', 'laytpl'], function () {

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
            { field: 'userId', title: '用户ID', width: 180 },
            { field: 'userRole', title: '角色', width: 120 },
            { field: 'nodeId', title: '节点', hide: true },
            { field: 'dbNo', title: '数据库编号', hide: true },
            { field: 'nickName', title: '昵称' },
            { field: 'email', title: '邮箱' },
            { field: 'source', title: '来源' },
            { field: 'state', title: '状态', width: 70 },
            { field: 'lastSignIn', title: '最后登录时间', width: 160 },
            { field: 'createTime', title: '创建时间', width: 160 },
            { field: 'language', title: '语言', width: 70 },
            { field: 'country', title: '国家', width: 120 },
            { field: 'isPay', title: '是否付费', hide: true },
            { field: 'source', title: '来源', hide: true },
            { field: 'operation', title: '操作', toolbar: '#operationBar', width: 120, minWidth: 120 }
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

    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var data = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象
        let userId = data.userId;
        if (layEvent === 'detail') { //查看
            var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
            window.top.addMyTab('/business/user/detail?userId=' + userId, '注册用户 ' + data.email, curmenu.topMenuName, curmenu.leftMenuName);
        }
        if (layEvent === 'delete') { //注销
            var clickEnable = true;
            layer.prompt({
                formType: 0,
                title: '请输入要注销用户"' + data.nickName + '"的注册邮箱：'
            }, function (value, index, elem) {
                if (clickEnable) {
                    clickEnable = false;
                }
                else {
                    return false;
                }
                if (value != data.email) {
                    layer.alert('操作失败:输入的邮箱不正确', { title: '错误', icon: 5 });
                    clickEnable = true;
                    return false;
                }
                $.post('/Business/User/DeleteUser', { userId: userId, email: value },
                    function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            clickEnable = true;
                        }
                        else {
                            layer.alert('操作成功', { icon: 6 });
                            layer.close(index);
                            $("#search").click();
                        }
                    });
            });
        }
    });

    $("#search").on("click", function () {

        if ($.trim($('#userId').val()).length <= 0 && $.trim($('#email').val()).length <= 0 && $.trim($('#gid').val()).length <= 0) {
            layer.alert('操作失败:搜索参数至少包含UserId(Gid)或者Email其一', { title: '错误', icon: 5 });
            return false;
        }

        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            }
        }
        tableIns.config.url = '/Business/User/GetUserPageData';
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