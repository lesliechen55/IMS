layui.use(['layer', 'jquery', 'form'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

    $('#pass').click(function () {
        layer.confirm('您是否确认通过审核?', { icon: 3, title: '提示' }, function (index) {
            $.post('/Freight/Company/Pass', { id: $('#id').val() }, function (res) {
                if (!res.success) {
                    layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                    return false;
                }

                layer.msg('操作成功', { icon: 6 });
                // 刷新页面
                location.reload();
                return false;
            });
            layer.close(index);
        });
    });

    $('#reject').click(function () {
        var id = $('#id').val();
        layer.prompt({
            formType: 2,
            value: '',
            title: '请填写驳回原因',
            area: ['450px', '300px'] //自定义文本域宽高
        }, function (value, index, elem) {
            if ($.trim(value).length <= 0) {
                layer.msg('请填写驳回原因');
                return;
            }
            $.post('/Freight/Company/Reject', { id: id, desc: value }, function (res) {
                if (!res.success) {
                    layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                    return false;
                }

                layer.msg('操作成功');
                // 刷新页面
                location.reload();
                return false;
            });
            layer.close(index);
        });
    });

});