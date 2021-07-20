layui.use(['layer', 'jquery', 'form'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

    // 自定义表单验证
    form.verify({
        oldPassword: function (value, item) {
            var password = $('input[name="newPassword"]').val();
            if (value === password) {
                return '新旧密码不能一致';
            }
        },
        password: function (value, item) {
            if (value.length > 0) {
                if (!/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,16}$/.test(value)) {
                    return '密码必须8到16位,最少8个字符至少1个大写字母，1个小写字母，1个数字和1个特殊字符';
                }
            }
        },
        newPassword: function (value, item) {
            var password = $('input[name="oldPassword"]').val();
            if (value === password) {
                return '新旧密码不能一致';
            }
        },
        confirmPassword: function (value, item) {
            var password = $('input[name="newPassword"]').val();
            if (value !== password) {
                return '请保持与新密码填写一致';
            }
        }
    });

    form.on('submit(form)', function (data) {
        $.post('/Account/ChangePassword', data.field, function (res) {
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