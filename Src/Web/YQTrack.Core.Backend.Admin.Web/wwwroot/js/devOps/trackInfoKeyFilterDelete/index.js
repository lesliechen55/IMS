layui.config({
    base: '/module/'
}).extend({
    treetable: 'treetable-lay/treetable'
}).use(['form', 'layer', 'table', 'treetable', 'laytpl'], function () {
    var $ = layui.jquery,
        layer = parent.layer === undefined ? layui.layer : top.layer;
    var table = layui.table;

    var objWhere = top.initFilter(window);
    //批量删除
    $('#batchDel').click(function () {
        top.changeUrlParam(window);
        var filter = $.trim($('#filter').val());
        if (filter.length < 4) {
            layer.msg('缓存键匹配规则不能少于四个字符');
            return false;
        };
        layer.confirm('确认要删除模糊匹配“' + filter + '”的缓存吗？', { icon: 3, title: '提示' }, function (index) {
            layer.close(index);
            $('#batchDel').hide();
            $('#scanning').show();
            $('#cancel').show();
            $('#table tbody').html('');
            $.get('/DevOps/TrackInfoKeyFilterDelete/BatchDelete?filter=' + filter, function (res) {
                if (!res.success) {
                    layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                    return false;
                }
                layui.layer.msg('任务已启动');
                it = setInterval(getDeleteKeys, 1000);
            });
        });
        return false;
    })

    //取消批量删除
    $('#cancel').click(function () {
        var filter = $('#filter').val();
        layer.confirm('确认要取消删除模糊匹配“' + filter + '”的缓存吗？', { icon: 3, title: '提示' }, function (index) {
            layer.close(index);
            var l = layer.load(0, {
                shade: [0.3, '#fff']
            });//0.3透明度的白色背景
            $.get('/DevOps/TrackInfoKeyFilterDelete/CancelBatchDelete', function (res) {
                layer.close(l);
                if (!res.success) {
                    layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                    return false;
                }
                layui.layer.msg('操作成功');
            });
        });
        return false;
    })
    var it = null;
    if ($('#batchDel').css('display') == 'none') {
        it = setInterval(getDeleteKeys, 1000);
    }
    //搜索
    function getDeleteKeys() {
        var body = $('#table tbody');
        var key = '';
        if (body.html() != '') {
            key = body.find('td:first').text();
        }
        $.get('/DevOps/TrackInfoKeyFilterDelete/GetBatchDeleteState?key=' + key, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            res.data.item2.batchDeleteKeys.forEach(function (item, index) {
                body.prepend('<tr><td>' + item + '</td></tr>');
            })
            if (res.data.item1) {
                layer.alert('操作成功:' + res.data.item2.msg, { title: '提示', icon: 6 });
                clearInterval(it);
                $('#batchDel').show();
                $('#scanning').hide();
                $('#cancel').hide();
            }
        });
    }
});