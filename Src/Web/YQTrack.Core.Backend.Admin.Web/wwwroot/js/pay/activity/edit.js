layui.use(['layer', 'jquery', 'form', 'element', 'table', 'laytpl', 'laydate', 'formSelects'], function () {

    var laydate = layui.laydate;
    var table = layui.table;
    var formSelects = layui.formSelects;

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

    laydate.render({
        elem: '#startTime'
    });
    laydate.render({
        elem: '#endTime'
    });


    let hRules = JSON.parse($("#hRules").val());
    layer.rules = hRules;
    let hProductId = $("#hProductId").val();

    let isSelect = 0;
    let radioType = 0;

    $.ajax({
        url: "/pay/activity/getproducts",
        data: {},
        dataType: "json",
        success: function (resultData) {
            $("#productId").empty();
            if (resultData.success) {
                $("#productId").append(new Option("请选择", ""));
                $.each(resultData.data, function (index, item) {
                    if (index == hProductId) {
                        $('#productId').append(new Option(item, index, false, true));
                    } else {

                        $('#productId').append(new Option(item, index));
                    }
                });
            } else {
                $("#productId").append(new Option("暂无数据", ""));
            }
            layui.form.render("select");

            var skus = JSON.parse($("#hSkuCodes").val());

            var skuNew = [];//数据改造,为了能显示选中
            $.each(skus, function (index, item) {
                var item = { name: item, value: index + 1 };
                skuNew.push(item);
            });

            formSelects.data('skuCodes', 'local', {
                arr: skuNew
            });

            //获取数组对象中value属性值的集合
            var valueList = Array.from(skuNew, ({ value }) => value);//压缩时候会出错

            //设置选中
            formSelects.value('skuCodes', valueList);
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
                isSelect = 1;
            }
        })
    });

    form.on('submit(form)', function (data) {

        data.field.rules = layer.rules;
        data.field.internalUse = $('input[name=internalUse]').val();

        var tt = formSelects.value('skuCodes', 'value');//获取选中的值
        if (isSelect == 1) {
            data.field.skuCodes = Array.from(tt, ({ value }) => value);//如果重新选择了就是value,没有重新选择就是name
        } else {
            data.field.skuCodes = Array.from(tt, ({ name }) => name);
        }

        data.field.term = radioType;

        if (radioType == 1) {//优惠领取x天
            var day = $("input[name='days']").val();//不能为空必须是大于0的数字

            if (/^[1-9]\d*$/.test(day) == false) {
                layer.alert('请输入大于0的数字', { title: '错误', icon: 5 });
                $("input[name='days']").focus();
                return false;
             
            }
            data.field.term = day;
        }
        //console.log(data.field); return false;

        $.post('/pay/activity/edit', data.field, function (res) {
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


    //显示活动规则table
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
        } else {
            layer.alert('请选择一行数据', { title: '错误', icon: 5 });
            return;
        }
    });

    form.on('switch(locked)', function (data) {
        var value = data.elem.value;
        if (value === 'true') {
            data.elem.value = 'false';
            $(data.elem).removeAttr('checked');
        } else {
            data.elem.value = 'true';
        }
    });

    form.on('radio(term)', function (data) {
        radioType = data.value;
        if (data.value != '1') {
            $("input[name='days']").val('');
            $("input[name='days']").hide();
        }
        else {  //优惠领取之日后
            $("input[name='days']").show();
            $("input[name='days']").focus();
        }
    });

    let discountType = $("select[name='discountType']").val();
    if (discountType == "1") {
        $("#businessType").get(0).selectedIndex = 0;
        $("#businessType").attr("disabled", "disabled");
    }

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

    function getAttrs(array, attr) {
        var arr = array.map((item) => {
            return item[attr];
        })
        return arr;
    }

});