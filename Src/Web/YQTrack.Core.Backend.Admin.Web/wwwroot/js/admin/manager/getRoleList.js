layui.use(['layer', 'jquery', 'form', 'table'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        table = layui.table;

    var id = $('#id').val();

    table.render({
        id: 'table',
        elem: '#table',
        method: 'post',
        height: 400,
        url: '/Admin/Manager/GetRoleData?userId=' + id,
        page: false,
        cellMinWidth: 90,
        cols: [[
            { type: 'numbers' },
            { type: 'checkbox' },
            { field: "id", title: '角色ID' },
            { field: 'name', title: '角色名' },
            { field: 'remark', title: '备注' }
        ]]
    });

    $('#submit').click(function () {
        var checkStatus = table.checkStatus('table'); //idTest 即为基础参数 id 对应的值
        console.log(checkStatus.data); //获取选中行的数据
        var roleIdList = [];
        for (var i = 0; i < checkStatus.data.length; i++) {
            roleIdList.push(checkStatus.data[i].id);
        }
        if (roleIdList.length === 0) {
            layer.alert('必须至少选择一个角色', { icon: 5, title: '错误' });
            return false;
        }
        $.post('/Admin/Manager/SetRoleList', { id: id, roleIdList: roleIdList }, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功');
            $('#close').click();
            return false;
        });
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });

    $('#close').click(function () {
        //当你在iframe页面关闭自身时
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭
    });

});