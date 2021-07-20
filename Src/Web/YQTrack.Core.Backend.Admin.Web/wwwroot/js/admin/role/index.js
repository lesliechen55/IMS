layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var objWhere = top.initFilter(window);

    table.render({
        elem: '#roleTable',
        method: 'post',
        url: '/Admin/Role/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 90,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: "id", title: '角色ID', hide: true },
            { field: 'name', title: '角色名称' },
            { field: 'isActive', title: '是否启用', templet: '#isActive', width: 100 },
            { field: 'remark', title: '备注' },
            { field: 'createdTime', title: '创建时间', width: 160 },
            { field: 'updatedTime', title: '更新时间', width: 160 }
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
            name: objWhere.name
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

    table.on('toolbar(roleTable)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加角色',
                    content: '/Admin/Role/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['500px', '360px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'update':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个角色');
                    return;
                }
                var title = checkStatus.data[0].name;
                var roleId = checkStatus.data[0].id;
                layer.open({
                    type: 2,
                    title: '编辑角色 ' + title,
                    content: '/Admin/Role/Edit?roleId=' + roleId + '&iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['500px', '400px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'set':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个角色');
                    return;
                }
                var name = checkStatus.data[0].name;
                var id = checkStatus.data[0].id;
                layer.open({
                    type: 2,
                    title: '设置角色权限 ' + name,
                    content: '/Admin/Role/QueryPermissionList?roleId=' + id,
                    skin: 'layui-layer-lan',
                    area: ['1100px', '750px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'delete':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个用户');
                    return;
                }
                var roleName = checkStatus.data[0].name;
                var delId = checkStatus.data[0].id;
                layer.confirm('您确定要删除 ' + roleName + ' 角色吗?(注意:包括删除与用户与权限的关系)', { icon: 3, title: '警告' }, function (index) {
                    $.post('/Admin/Role/Delete', { id: delId }, function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.msg('操作成功');
                        table.reload('roleTable');
                        layer.close(index);
                        return false;
                    });
                });
                break;
        }
    });

    $("#search").on("click", function () {
        var name = $('#name').val();
        table.reload('roleTable',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    name: name
                }
            });
    });

});