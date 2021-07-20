layui.use(['layer', 'jquery', 'form', 'upload'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

    var upload = layui.upload;
    //执行实例
    var uploader = upload.render({
        elem: '#upload' //绑定元素
        , url: '/Account/UpdateAvatar' //上传接口
        , method: 'post'
        , accept: 'images'
        , auto: false
        , bindAction: '#submit'
        , size: 2048 // 限制2M
        , multiple: false
        , number: 1
        , field: 'formFile'
        , drag: false
        , acceptMime: 'image/jpg, image/png, image/jpeg'
        , done: function (res, index, upload) {
            //上传完毕回调
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功,可以刷新界面查看新头像喔');
            $('#close').click();
            return false;
        }
        , error: function (index, upload) {
            //请求异常回调
        }
        , choose: function (obj) {
            //选择文件的回调

            //预读本地文件，如果是多文件，则会遍历。(不支持ie8/9)
            obj.preview(function (index, file, result) {
                console.log(index); //得到文件索引
                console.log(file); //得到文件对象
                console.log(result); //得到文件base64编码，比如图片
                $('#img').prop('src', result);
            });
        }
        , allDone: function (obj) {
            //当文件全部被提交后，才触发
        }
        , before: function (obj) {
            //文件提交上传前的回调
        }
    });

    $('#updateNickName').click(function () {
        var val = $('input[name=nickName]').val().trim();
        if (val.length <= 0 || val.length > 16) {
            layer.msg('昵称不能为空且不能超过16个字符');
            return;
        }
        $.post('/Account/UpdateNickName', { nickName: val }, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功');
            return false;
        });
    });

    $('#close').click(function () {
        //当你在iframe页面关闭自身时
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭
    });

});