layui.use(['layer', 'jquery', 'form'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;
    form.on('select(sendType)', function (data) {
        if (data.value == 1) {
            $('#sendInfo .layui-form-label').text('邮箱地址');
            $('#sendInfo .layui-input').attr('placeholder', '请输入邮箱地址');
            $('#sendInfo .layui-input').val($('#invoiceEmail').val());
        }
        else {
            $('#sendInfo .layui-form-label').text('快递信息');
            $('#sendInfo .layui-input').attr('placeholder', '例如：顺丰 SF123456');
            $('#sendInfo .layui-input').val('');
        }
    });
    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    form.on('submit(form)', function (data) {
        $.post('/Pay/InvoiceApply/Pass', data.field, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功', { icon: 6 });
            // 刷新父级页面
            top.refreshParent();
            parent.layer.close(index);
        });
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });

    $('#close').click(function () {
        //当你在iframe页面关闭自身时
        parent.layer.close(index); //再执行关闭
    });

});