layui.use(['layer', 'jquery', 'form'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;


    //根据DOM元素的id构造出一个编辑器
    var dataJsonEditor = CodeMirror.fromTextArea(document.getElementById("dataJson"), {
        mode: "application/json",    //实现json代码高亮
        lineNumbers: true,	//显示行号
        theme: "dracula",	//设置主题
        lineWrapping: true,	//代码折叠
        foldGutter: true,
        gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
        matchBrackets: true,	//括号匹配
        smartIndent: true        //自动缩进
        //readOnly: true,        //只读
    });
    //dataJsonEditor.setSize('800px', '350px');     //设置代码框的长宽
    //$("#dataJson").text(dataJson.getValue());

    //根据DOM元素的id构造出一个编辑器
    var templateTitleEditor = CodeMirror.fromTextArea(document.getElementById("templateTitle"), {
        mode: "text/html",    //实现html代码高亮
        //mode: "text/x-csharp",    //实现aspx代码高亮
        lineNumbers: true,	//显示行号
        theme: "dracula",	//设置主题
        lineWrapping: true,	//代码折叠
        foldGutter: true,
        gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
        matchBrackets: true,	//括号匹配
        smartIndent: true        //自动缩进
        //readOnly: true,        //只读
    });
    templateTitleEditor.setSize('100%', '50px');     //设置代码框的长宽

    //根据DOM元素的id构造出一个编辑器
    var templateBodyEditor = CodeMirror.fromTextArea(document.getElementById("templateBody"), {
        mode: "text/html",    //实现html代码高亮
        //mode: "text/x-csharp",    //实现aspx代码高亮
        lineNumbers: true,	//显示行号
        theme: "dracula",	//设置主题
        lineWrapping: true,	//代码折叠
        foldGutter: true,
        gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
        matchBrackets: true,	//括号匹配
        smartIndent: true        //自动缩进
        //readOnly: true,        //只读
    });
    templateBodyEditor.setSize('100%', '600px');     //设置代码框的长宽
    //console.log(templateBody.getValue());    //获取代码框的值

    //editor.setOption("readOnly", true);	//类似这种

    form.on('submit(form)', function (data) {
        var dataJson = dataJsonEditor.getValue();
        var templateTitle = templateTitleEditor.getValue();
        var templateBody = templateBodyEditor.getValue();
        if (!templateBody) {
            layer.msg("必填项不能为空", { icon: 5, anim: 6 });
            return false;
        }
        data.field.dataJson = dataJson;
        data.field.templateTitle = templateTitle;
        data.field.templateBody = templateBody;
        $.post('/Message/TemplateType/Add', data.field, function (res) {
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