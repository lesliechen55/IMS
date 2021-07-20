layui.config({
    base: '/module/'
}).extend({
    treeGrid: 'authtree'
}).use(['jquery', 'authtree', 'layer', 'form'], function () {

    var $ = layui.jquery;
    var authtree = layui.authtree;
    var form = layui.form;
    var layer = parent.layer === undefined ? layui.layer : top.layer;

    var id = $('#id').val();

    var treeId = '#permissionTree';

    $.ajax({
        method: 'post',
        url: '/Admin/Role/QueryPermissionList?roleId=' + id,
        success: function (res) {
            var trees = authtree.listConvert(res.data, {
                primaryKey: 'id'
                , startPid: 0
                , parentKey: 'pid'
                , nameKey: 'name'
                , valueKey: 'id'
                , checkedKey: res.checkedIdList
            });
            authtree.render(treeId, trees, {
                inputname: 'authids[]',
                openchecked: true,
                openall: true,
                layfilter: 'lay-check-auth',
                autowidth: true
            });
        }
    });

    $('#checkAll').click(function () {
        authtree.checkAll(treeId);
    });

    $('#uncheckAll').click(function () {
        authtree.uncheckAll(treeId);
    });

    $('#showAll').click(function () {
        authtree.showAll(treeId);
    });

    $('#closeAll').click(function () {
        authtree.closeAll(treeId);
    });

    $('#getMaxDept').click(function () {
        layer.alert('树' + treeId + '的最大深度为：' + authtree.getMaxDept(treeId));
    });

    $('#getCheckedNodes').click(function () {
        // 获取所有节点
        var all = authtree.getAll(treeId);
        // 获取所有已选中节点
        var checked = authtree.getChecked(treeId);
        // 获取所有未选中节点
        var notchecked = authtree.getNotChecked(treeId);
        // 获取选中的叶子节点
        var leaf = authtree.getLeaf(treeId);
        // 获取最新选中
        var lastChecked = authtree.getLastChecked(treeId);
        // 获取最新取消
        var lastNotChecked = authtree.getLastNotChecked(treeId);
        console.log(
            'all', all, "\n",
            'checked', checked, "\n",
            'notchecked', notchecked, "\n",
            'leaf', leaf, "\n",
            'lastChecked', lastChecked, "\n",
            'lastNotChecked', lastNotChecked, "\n"
        );
        layer.alert('当前选择的权限ID集合: ' + checked);
    });

    $('#close').click(function () {
        //当你在iframe页面关闭自身时
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭
    });

    $('#submit').click(function () {
        var authids = authtree.getChecked(treeId);
        $.post('/Admin/Role/SetPermissionList', { id: id, permissionIdList: authids }, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功');
            $('#close').click();
            return false;
        });
        return false;
    });

});