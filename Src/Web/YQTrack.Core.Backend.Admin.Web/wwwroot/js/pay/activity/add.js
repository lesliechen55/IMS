layui.use(['layer', 'laydate', 'jquery', 'table', 'form', 'laytpl', 'formSelects'], function () {
    var laydate = layui.laydate;
    var table = layui.table;
    var formSelects = layui.formSelects;
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;
    layer.rules = [];

    laydate.render({
        elem: '#startTime'
    });
    laydate.render({
        elem: '#endTime'
    });

    form.on('submit(form)', function (data) {
        data.field.rules = layer.rules;
        data.field.internalUse = data.field.internalUse == 'on';
        data.field.skuCodes = data.field.skuCodes ? data.field.skuCodes.split(',') : [];

        $(this).addClass("layui-btn-disabled").attr("disabled", 'disabled');//防止重复提交

        $.post('/pay/activity/add', data.field, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                $(".layui-btn-disabled").removeClass("layui-btn-disabled").removeAttr("disabled");
                return false;
            }
            layer.msg('操作成功', { icon: 6 });
            // 刷新父级页面
            $('#close').click();
            top.refreshParent();
            return true;
        }, 'json');
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });

    form.on('radio(term)', function (data) {
        if (data.value != '1') {
            $("input[name='days']").val('');
            $("input[name='days']").hide();
        }
        else {  //优惠领取之日后
            $("input[name='days']").show();
            $("input[name='days']").focus();
        }
    });


    $('#close').click(function () {
        //当你在iframe页面关闭自身时
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭
    });

    $.ajax({
        url: "/pay/activity/getproducts",
        data: {},
        dataType: "json",
        success: function (resultData) {
            $("#productId").empty();
            if (resultData.success) {
                $("#productId").append(new Option("请选择", ""));
                $.each(resultData.data, function (index, item) {
                    $('#productId').append(new Option(item, index));
                });
            } else {
                $("#productId").append(new Option("暂无数据", ""));
            }

            layui.form.render("select");
        }
    });

    //级联子项目-可用产品
    form.on('select(productIdfilter)', function (data) {
        var value = data.value;  //select选中的值
        $.ajax({
            url: "/pay/activity/getskus",
            data: { productId: value },
            dataType: "json",
            success: function (resultData) {
                data = [];
                if (resultData.success) {
                    $.each(resultData.data, function (index, item) {
                        var item = { name: item, value: index };
                        data.push(item);
                    });
                };
                formSelects.data('skuCodes', 'local', { arr: data });
            }
        })
    });


    form.on('select(discountTypefilter)', function (data) {
        var value = data.value;  //select选中的值
        //金额优惠 不需要设置业务类型
        if (value == "1") {
            $("#businessType").get(0).selectedIndex = 0;
            $("#businessType").attr("disabled", "disabled");
        } else {
            $("#businessType").removeAttr("selected");
            $("#businessType").removeAttr("disabled");
        }
        form.render();
    });

    $("#addRule").click(function () {
        var idx = layer.open({
            type: 2,
            title: '添加规则',
            content: '/pay/activity/addrule?iframeName=' + window.name,
            skin: 'layui-layer-lan',
            area: ['400px', '300px'],
            offset: 'auto',
            btn: ['确定', '取消'],
            shade: 0.3,
            yes: function (index, layero) {
                var body = layer.getChildFrame("body", index);
                var tag = body.find("#currency:checked").val();
                var currency = 0;
                switch (tag) {
                    case "1":
                        tag = "人民币";
                        currency = 1;
                        break;
                    case "2":
                        tag = "美元";
                        currency = 2;
                        break;
                    default:
                        tag = "未知";
                        currency = 0;
                        break;
                }
                //var ruleItem = {
                //    id: layer.rules.length + 1,
                //    currency: currency,
                //    tag: tag,
                //    threshold: body.find("#threshold").val(),
                //    discount: body.find("#discount").val()
                //};

                var ruleItem = {
                    id: layer.rules.length + 1,
                    c: currency,
                    tag: tag,
                    t: body.find("#threshold").val(),
                    d: body.find("#discount").val()
                };

                layer.rules.push(ruleItem);
                table.reload("activityRule", {
                    data: layer.rules,
                })
                layer.close(index);
            }
        });
    });

    $("#delRule").on("click", function () {
        var checkStatus = table.checkStatus('activityRule');
        if (checkStatus.data.length > 0) {
            cbList = layer.rules;
            for (var k = 0; k < checkStatus.data.length; k++) {
                var _delId = checkStatus.data[k].id;
                for (var i = 0; i < cbList.length; i++) {
                    var _id = cbList[i].id;
                    if (_id == _delId) {
                        cbList.splice(i, 1);
                        break;
                    }
                }
            }
            table.reload("activityRule", {
                data: layer.rules,
            })
        }
    });

    table.render({
        elem: '#activityRule',
        data: layer.rules,
        cols: [[
            { type: 'checkbox' },
            //{ field: 'id', title: '顺序' },
            //{ field: 'tag', title: '货币' },
            //{ field: 'threshold', title: '阈值' },
            //{ field: 'discount', title: '优惠' }
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
