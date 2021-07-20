layui.use(['form', 'layer', 'jquery'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer;
    var $ = layui.jquery;

    // 自定义表单验证
    form.verify({
        account: function (value, item) { //value：表单的值、item：表单的DOM对象
            var length = value.length;
            if ($.trim(value).length !== length) {
                return '用户名错误,不能包含空格';
            }
            if (length < 1 || length > 16) {
                return '用户名长度错误,不能小于1位,不能大于16位';
            }
        },
        password: [
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,16}$/
            , '密码必须8到16位,最少8个字符至少1个大写字母，1个小写字母，1个数字和1个特殊字符'
        ],
        code: [
            /^[\d]{4,4}$/
            , '验证码必须是4位数字'
        ]
    });

    $('#imgCode img').click(function () {
        $(this).attr('src', '/home/GetValidateCode?v=' + new Date().getTime());
    });
    //获取returnUrl参数
    function getReturnUrl() {
        var href = window.location.href;
        var index = href.indexOf('ReturnUrl=');
        if (index == -1) {
            return "";
        }
        else {
            return href.substring(href.indexOf('ReturnUrl=') + 10);
        }
    }
    //登录按钮
    form.on("submit(login)", function (data) {
        var that = this;
        $(this).text("登录中...").attr("disabled", "disabled").addClass("layui-disabled");
        $.post('/home/login', { request: data.field }, function (result) {
            if (result.success) {
                var returnUrl = getReturnUrl();
                returnUrl = returnUrl == '' ? '/' : unescape(returnUrl);
                window.location.href = returnUrl;
            } else {
                $('#imgCode img').click();
                $(that).text('登录').removeAttr('disabled').removeClass('layui-disabled');
                layer.alert('登陆失败,错误原因如下:' + result.msg, { title: '错误', icon: 5 });
            }
        });
    });

    //表单输入效果
    $(".loginBody .input-item").click(function (e) {
        e.stopPropagation();
        $(this).addClass("layui-input-focus").find(".layui-input").focus();
    });
    $(".loginBody .layui-form-item .layui-input").focus(function () {
        $(this).parent().addClass("layui-input-focus");
    });
    $(".loginBody .layui-form-item .layui-input").blur(function () {
        $(this).parent().removeClass("layui-input-focus");
        if ($(this).val() !== '') {
            $(this).parent().addClass("layui-input-active");
        } else {
            $(this).parent().removeClass("layui-input-active");
        }
    });
});