layui.use(['layer', 'laydate', 'jquery', 'table', 'form', 'laytpl', 'formSelects'], function () {
    var laydate = layui.laydate;
    var table = layui.table;
    var formSelects = layui.formSelects;
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

    let hRules = JSON.parse($("#hRules").val());
   
    form.on('submit(form)', function (data) {
        
        data.field.userEmail = $("#email").val();
        layer.rules = [];

        var checkStatus = table.checkStatus('activityRule');
        if (checkStatus.data.length > 0) {
            for (var k = 0; k < checkStatus.data.length; k++) {
                layer.rules.push(checkStatus.data[k]);
            }
        }
        else {
            layer.alert('请至少勾选一个规则', { title: '错误', icon: 5 });
            return false;
        }

        data.field.rules = layer.rules;
        //console.log(data.field); return false;

        $(this).addClass("layui-btn-disabled").attr("disabled", 'disabled');//防止重复提交

        $.post('/pay/activity/addCoupon', data.field, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                $(".layui-btn-disabled").removeClass("layui-btn-disabled").removeAttr("disabled");
                return false;
            }
            layer.msg(res.msg, { icon: 6 });
            // 刷新父级页面
            $('#close').click();
            top.refreshParent();
            return true;
        }, 'json');
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });

    $('#close').click(function () {
        //当你在iframe页面关闭自身时
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭
    });

    table.render({
        elem: '#activityRule',
        data: hRules,
        cols: [[
            { type: 'checkbox' },
            { field: 'id', title: '顺序' },
            {
                field: 'c', title: '货币', templet: function (d) {
                    if (d.c == "0") {
                        return "未知";
                    } else if (d.c == "1") {
                        return "人民币";
                    } else if (d.c == "2") {
                        return "美元";
                    }
                }
            },
            { field: 't', title: '阈值' },
            { field: 'd', title: '优惠' }
        ]]
    });

});
