layui.use(['layer', 'jquery', 'form', 'upload'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        upload = layui.upload;
    // 自定义表单验证
    form.verify({
        formFile: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!item.src) {
                if ($('#invoiceType').val() == 2) {
                    return '请上传一般纳税人证明';
                }
            }
        },
        notEmpty: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!value) {
                if ($('#invoiceType').val() == 2) {
                    return '必填项不能为空';
                }
            }
        }
    });

    $('#userEmail').blur(function () {
        if ($('#userEmail').val() == '') {
            $('#userEmail').css("border-color", "red");
            $('#userEmail').focus();
            layer.msg('用户邮箱不能为空!', { icon: 5, anim: 6 });
            return false;
        }
        else {
            $('#userEmail').css("border-color", "rgb(230, 230, 230)");
            $.get('/Pay/Invoice/GetUserId', { userEmail: $('#userEmail').val() }, function (res) {
                if (!res.success || !res.data) {
                    layer.msg('找不到该用户!', { icon: 5, anim: 6 });
                    $('#userEmail').focus();
                    return false;
                }
                $('#userEmail').css("border-color", "green");
                $('#userEmail').attr("userId", res.data);
            });
        }
    })

    var uploader = upload.render({
        elem: '#upload' //绑定元素
        , url: '/Pay/Invoice/UploadTaxImage' //上传接口
        , method: 'post'
        , accept: 'images'
        , auto: true
        , size: 2048 // 限制2M
        , multiple: false
        , number: 1
        , field: 'formFile'
        , data: {
            userId: function () { return $('#userEmail').attr("userId") }
        }
        , drag: false
        , exts: "jpg|jpeg|png"
        , acceptMime: 'image/jpg, image/png, image/jpeg'
        , done: function (res, index, upload) {
            //上传完毕回调
            layer.closeAll('loading');
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            $('#taxPayerCertificateUrl').val(res.data);
            return false;
        }
        , error: function (index, upload) {
            //请求异常回调
            layer.closeAll('loading');
        }
        , choose: function (obj) {
            //选择文件的回调

            //预读本地文件，如果是多文件，则会遍历。(不支持ie8/9)
            obj.preview(function (index, file, result) {
                $('#img').prop('src', result);
            });
        }
        , allDone: function (obj) {
            //当文件全部被提交后，才触发
        }
        , before: function (obj) {
            //文件提交上传前的回调
            if (!$('#userEmail').attr("userId")) {
                layer.msg('用户邮箱不能为空!', { icon: 5, anim: 6 });
                $('#userEmail').css("border-color", "red");
                $('#userEmail').focus();
            }
            layer.load();
        }
    });
    form.on('submit(form)', function (data) {
        //console.log(data.field)
        $.post('/Pay/Invoice/Add', data.field, function (res) {
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