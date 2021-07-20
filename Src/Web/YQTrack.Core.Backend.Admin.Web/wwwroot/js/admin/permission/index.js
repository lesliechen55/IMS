// 项目参考使用地址:https://gitee.com/whvse/treetable-lay
layui.config({
    base: '/module/'
}).extend({
    treetable: 'treetable-lay/treetable'
}).use(['form', 'layer', 'table', 'treetable', 'laytpl'], function () {
    var $ = layui.jquery,
        layer = parent.layer === undefined ? layui.layer : top.layer;
    var table = layui.table;
    var treetable = layui.treetable;
    treetable.render({
        id: 'permissionTable',
        method: 'get',
        toolbar: '#toolbar',
        treeColIndex: 4,
        treeSpid: -1,
        treeIdName: 'id',
        treePidName: 'parentId',
        treeDefaultClose: false,
        treeLinkage: true,
        elem: '#permissionTable',
        url: '/Admin/Permission/GetAll',
        page: false,
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'id', title: '权限ID', hide: true },
            { field: 'parentId', title: '父级ID', hide: true },
            { field: 'name', title: '名字', minWidth: 200 },
            { field: 'areaName', title: '区域', minWidth: 100 },
            { field: 'controllerName', title: '控制器', minWidth: 100 },
            { field: 'actionName', title: '行为', minWidth: 100 },
            { field: 'fullName', title: '权限标识', minWidth: 180 },
            { field: 'url', title: '菜单url', minWidth: 180 },
            { field: 'sort', align: 'left', title: '排序号', width: 80 },
            { field: 'remark', title: '备注说明' },
            { field: 'icon', title: '图标', align: 'left', width: 110 },
            { field: 'menuTypeDesc', title: '菜单类型', minWidth: 100 },
            { field: 'topMenuKey', title: '顶级菜单Key', minWidth: 120 },
            { field: 'createdTime', title: '创建时间', minWidth: 100, hide: true },
            { field: 'updatedTime', title: '更新时间', minWidth: 100, hide: true }
        ]],
        done: function () {
            layer.closeAll('loading');
        }
    });

    //监听事件
    table.on('toolbar(permissionTable)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加权限',
                    content: '/Admin/Permission/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['660px', '880px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'update':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请选择一个您所需要编辑的项');
                    return false;
                }
                var title = checkStatus.data[0].name;
                var id = checkStatus.data[0].id;
                layer.open({
                    type: 2,
                    title: '编辑权限-' + title,
                    content: '/Admin/Permission/Edit?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['660px', '880px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'field':
                //if (checkStatus.data.length !== 1) {
                //    layer.msg('请选择一个您所需要编辑的项');
                //    return false;
                //}
                //var title = checkStatus.data[0].name;
                //var id = checkStatus.data[0].id;
                var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
                window.top.addMyTab('/business/esfield/index', "ES字段管理", curmenu.topMenuName, curmenu.leftMenuName);
                break;
            case 'dashboard':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请选择一个您所需要编辑的项');
                    return false;
                }
                var title = checkStatus.data[0].name;
                var id = checkStatus.data[0].id;
                layer.open({
                    type: 2,
                    title: '配置Dashboard-' + title,
                    content: '/Business/ESDashboard/Set?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['1100px', '800px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
        }
    });
});