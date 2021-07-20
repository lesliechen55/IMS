layui.use(['layer', 'jquery', 'form'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

    //form.on('switch(active)', function (data) {
    //    var value = data.elem.value;
    //    if (value === 'true') {
    //        data.elem.value = 'false';
    //        $(data.elem).removeAttr('checked');
    //    } else {
    //        data.elem.value = 'true';
    //    }
    //});

    form.on('switch(isInternalUse)', function (data) {
        var value = data.elem.value;
        if (value === 'true') {
            data.elem.value = 'false';
            $(data.elem).removeAttr('checked');
        } else {
            data.elem.value = 'true';
        }
    });

    form.on('submit(form)', function (data) {
        let isInternalUse = $('input[name=isInternalUse]').val();
        if (isInternalUse === 'true' && data.field.skuType !== '2' && data.field.skuType !== '3') {
            layer.alert('操作失败:只能SKU类型为邮件数或者查询数才能启用内部使用开关!', { title: '警告', icon: 5 });
            return false;
        }
        data.field.isInternalUse = isInternalUse;
        $.post('/Pay/ProductSku/Add', data.field, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功', { icon: 6 });
            // 刷新父级页面
            top.refreshParent();
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