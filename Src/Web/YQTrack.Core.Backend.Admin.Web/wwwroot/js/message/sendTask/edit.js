layui.use(['layer', 'jquery', 'form'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

    form.on('radio(sendType)', function (data) {
        if (data.value == '1') {
            $('#divDetail').hide();
            $('#divRole').show();
        }
        else {
            $('#divRole').hide();
            $('#divDetail').show();
        }
        //console.log(data.elem); //得到radio原始DOM对象
        //console.log(data.value); //被点击的radio的value值
    });

    form.on('submit(form)', function (data) {
        if ($('input[name=sendType]:checked').val() == 0) {
            if (!$('#objDetails').val()) {
                var msg = $('#objDetails').parent().prev().text() + '不能为空';
                layer.msg(msg, { icon: 5, anim: 6 });
                return false;
            }
        }
        $.post('/Message/SendTask/Edit', data.field, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            if (data.field.sendAction == 0 || data.field.sendAction == undefined) {
                layer.msg('已保存为草稿');
            }
            else {
                layer.msg('操作成功', { icon: 6 });
            }
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