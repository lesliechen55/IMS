layui.config({
    base: '/module/'
}).extend({
    treetable: 'treetable-lay/treetable'
}).use(['form', 'layer', 'table', 'treetable', 'laytpl'], function () {
    var $ = layui.jquery,
        layer = parent.layer === undefined ? layui.layer : top.layer;
    var table = layui.table;

    var objWhere = top.initFilter(window);

    //监听事件
    table.on('toolbar(table)', function (obj) {
        switch (obj.event) {
            case 'delete':
                if (obj.config.data.length < 1) {
                    layer.msg('请查询您所需要删除的缓存');
                    return false;
                }
                layer.confirm('确认要删除查询出的缓存吗？', { icon: 3, title: '提示' }, function (index) {
                    layer.close(index);
                    var l = layer.load(0, {
                        shade: [0.3, '#fff']
                    });//0.3透明度的白色背景
                    var data = [];
                    obj.config.data.forEach(function (item, inx) {
                        if (item.key) {
                            data.push(item.key);
                        }
                    })
                    var keys = data.join(',');
                    $.post('/DevOps/trackInfoNumbersDelete/DeleteKeys', { keys: keys }, function (res) {
                        layer.close(l);
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.msg('操作成功');
                        $('#search').click();
                    });
                });
                break;
        }
        return false;
    });

    //搜索
    $('#search').click(function () {
        var trackNos = $('#areaTrackNos').val().replace(/\n/g, ',');
        if ($.trim(trackNos) == '') {
            layer.msg('请输入单号');
            return false;
        }
        $('#trackNos').val(trackNos);
        top.changeUrlParam(window);
        var l = layer.load(0, {
            shade: [0.3, '#fff']
        });//0.3透明度的白色背景
        layui.treetable.render({
            id: 'table',
            method: 'get',
            toolbar: '#toolbar',
            treeColIndex: 4,
            treeSpid: -1,
            treeIdName: 'id',
            treePidName: 'parentId',
            treeDefaultClose: false,
            treeLinkage: true,
            elem: '#table',
            url: '/DevOps/trackInfoNumbersDelete/GetListTrackCache?trackNos=' + trackNos,
            page: false,
            cols: [[
                { type: 'numbers' },
                { type: 'checkbox', hide: true },
                { field: 'id', title: 'ID', hide: true },
                { field: 'parentId', title: '父级ID', hide: true },
                { field: 'key', title: '缓存键', minWidth: 200 }
            ]],
            done: function (res) {
                layer.close(l);
                if (res.Count == -1) {
                    layer.alert('操作失败:未知的异常，请联系管理员', { title: '错误', icon: 5 });
                }
            }
        });
    })

    if ($('#trackNos').val() != '') {
        $('#areaTrackNos').val($('#trackNos').val().replace(/,/g, '\n'));
        $('#search').click();
    }
});