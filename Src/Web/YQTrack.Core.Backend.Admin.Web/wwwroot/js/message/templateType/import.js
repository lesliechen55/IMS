layui.use(['layer', 'jquery', 'form', 'upload'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        upload = layui.upload;

    var uploader = upload.render({
        elem: '#upload' //绑定元素
        , url: '/Message/TemplateType/Import' //上传接口
        , method: 'post'
        , accept: 'json,js'
        , auto: false
        , bindAction: '#submit'
        , data: {
            language: function () {
                return $('#language').val();
            }
        }
        , size: 2048 // 限制2M
        , multiple: false
        , number: 1
        , field: 'formFile'
        , drag: false
        , exts: "json|js"
        , acceptMime: 'application/x-javascript,text/javascript,application/json'
        , done: function (res, index, upload) {
            //上传完毕回调
            layer.closeAll('loading');
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            //var m = layer.msg('导入成功，生成预览');
            layer.open({
                type: 2,
                title: "预览 " + res.data.templateName,
                content: '/Message/TemplateType/TemplatePreview?id=' + res.data.templateId + '&loadInCache=' + true,
                skin: 'layui-layer-lan',
                area: ['900px', '820px'],
                offset: 'auto',
                shade: 0.3,
                maxmin: true,
                btn: ['保存', '关闭'],
                yes: function (index, layero) {
                    //layer.close(m);
                    //按钮【按钮一】的回调
                    $.get('/Message/Template/Add', { id: res.data.templateId }, function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.msg('操作成功');
                        layer.close(index);
                        return false;
                    });
                },
                btn2: function (index, layero) {
                    //按钮【按钮二】的回调
                    $.get('/Message/Template/RealDelete', { id: res.data.templateId }, function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.close(index);
                        return false;
                    });
                    //return false 开启该代码可禁止点击该按钮关闭
                },
                cancel: function () {
                    //右上角关闭回调
                    $.get('/Message/Template/RealDelete', { id: res.data.templateId }, function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.close(index);
                        return false;
                    });
                    //return false 开启该代码可禁止点击该按钮关闭
                }
            });
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
                $('#lbFileName').text(file.name)
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
            layer.alert("请上传词条文件", { icon: 5, anim: 6 });
        }
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });
    $('#close').click(function () {
        //当你在iframe页面关闭自身时
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭
    });

});