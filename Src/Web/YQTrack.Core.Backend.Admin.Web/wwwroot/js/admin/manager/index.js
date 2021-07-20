layui.use(['form', 'layer', 'table', 'laytpl', 'jquery'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var objWhere = top.initFilter(window);

    table.render({
        elem: '#managerTable',
        method: 'post',
        url: '/Admin/Manager/GetPageData/',
        height: "full-" + objWhere.top,
        cellMinWidth: 90,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: "id", title: '管理员ID', hide: true },
            { field: 'account', title: '账号' },
            { field: 'nickName', title: '姓名' },
            { field: 'isLock', title: '是否启用', templet: '#isLock', width: 100 },
            { field: 'avatar', title: '头像', width: 70, templet: '#avatar' },
            { field: 'email', title: '邮箱', width: 160 },
            { field: 'remark', title: '备注' },
            { field: 'createdTime', title: '创建时间', width: 160 },
            { field: 'lastLoginTime', title: '最后登陆时间', width: 160 },
            { field: 'updatedTime', title: '更新时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            nickName: objWhere.nickName,
            account: objWhere.account
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    table.on('toolbar(managerTable)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加用户',
                    content: '/Admin/Manager/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['500px', '580px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'update':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个用户');
                    return;
                }
                var title = checkStatus.data[0].nickName;
                var id = checkStatus.data[0].id;
                layer.open({
                    type: 2,
                    title: '编辑用户 ' + title,
                    content: '/Admin/Manager/Edit?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['500px', '450px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'set':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个用户');
                    return;
                }
                var name = checkStatus.data[0].nickName;
                var mid = checkStatus.data[0].id;
                layer.open({
                    type: 2,
                    title: '设置管理员角色 ' + name,
                    content: '/Admin/Manager/GetRoleList?userId=' + mid,
                    skin: 'layui-layer-lan',
                    area: ['700px', '600px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'delete':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个用户');
                    return;
                }
                var nickName = checkStatus.data[0].nickName;
                var delId = checkStatus.data[0].id;
                layer.confirm('您确定要删除 ' + nickName + ' 用户吗?', { icon: 3, title: '警告' }, function (index) {
                    $.post('/Admin/Manager/Delete', { id: delId }, function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.msg('操作成功');
                        table.reload('managerTable');
                        layer.close(index);
                        return false;
                    });
                });
                break;
        }
    });

    $("#search").on("click", function () {
        var nickName = $('#nickName').val();
        var account = $('#account').val();
        table.reload('managerTable',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    nickName: nickName,
                    account: account
                }
            });
        return true;
    });

});