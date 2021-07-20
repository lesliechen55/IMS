layui.use(['layer', 'jquery', 'form', 'table'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        table = layui.table;

    form.on('switch(renew)', function (data) {
        var value = data.elem.value;
        if (value === 'true') {
            data.elem.value = 'false';
            $(data.elem).removeAttr('checked');
        } else {
            data.elem.value = 'true';
        }
    });

    form.on('submit(form)', function (data) {
        $.post('/Pay/ProductSku/AddBusiness', data.field, function (res) {
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