layui.use(['form', 'layer'], function () {
    var form = layui.form,
        $ = layui.jquery;
    var layer = parent.layer === undefined ? layui.layer : top.layer;
    form.verify({
        reason: function (value, item) {
            if (value.length > 300) {
                return '下架原因不能超过300个字';
            }
        }
    });

    form.on('submit(formReject)', function (data) {
        $.post('/Freight/InquiryManage/Reject', data.field, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功', { icon: 6 });

            // 刷新父级页面
            $('#close').click();
            var frames = window.parent.frames;
            for (var i = 0; i < frames.length; i++) {
                console.log(frames[i].name);
                if (frames[i].name === window.iframeName) {
                    var search = frames[i].document.getElementById(window.invokeEle);
                    search.click();
                    break;
                }
            }
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