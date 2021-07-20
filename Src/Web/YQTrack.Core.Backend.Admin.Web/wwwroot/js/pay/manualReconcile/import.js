layui.use(['layer', 'jquery', 'form', 'upload'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        upload = layui.upload;

    var uploader = upload.render({
        elem: '#upload' //绑定元素
        , url: '/Pay/ManualReconcile/ImportGlocash' //上传接口
        , method: 'post'
        , accept: 'json'
        , auto: false
        , bindAction: '#submit'
        , data: {
            remark: function () {
                return $('#remark').val();
            },
            year: function () {
                return $('#year').val();
            },
            month: function () {
                return $('#month').val();
            }
        }
        , size: 20480 // 限制20M
        , multiple: false
        , number: 1
        , field: 'formFile'
        , drag: false
        , exts: "json"
        , acceptMime: 'application/json'
        , done: function (res, index, upload) {
            //上传完毕回调
            layer.closeAll('loading');
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功');
            $('#close').click();
            // 刷新父级页面
            top.refreshParent();
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
                $('#lbFileName').text(file.name);
                //console.log(index); //得到文件索引
                //console.log(file); //得到文件对象
                //console.log(result); //得到文件base64编码，比如图片
            });
        }
        , allDone: function (obj) {
            //当文件全部被提交后，才触发
        }
        , before: function (obj) {
            //文件提交上传前的回调
            layer.load(3);
        }
    });

    form.on('submit(form)', function (data) {
        if (!$('#lbFileName').text()) {
            layer.alert("请先选择您要要上传的账单文件", { icon: 5, anim: 6 });
        }
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });

    $('#close').click(function () {
        //当你在iframe页面关闭自身时
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭
    });

});