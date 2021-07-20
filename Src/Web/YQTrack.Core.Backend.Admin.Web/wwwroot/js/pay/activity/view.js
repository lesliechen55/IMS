layui.use(['layer', 'jquery', 'form', 'element', 'table', 'laytpl', 'laydate', 'formSelects'], function () {

    var laydate = layui.laydate;
    var table = layui.table;
    var formSelects = layui.formSelects;

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

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
            //var valueList = Array.from(skuNew, ({ value }) => value);//压缩时候会出错
            var valueList = getAttrs(skuNew, "value");

            //设置选中
            formSelects.value('skuCodes', valueList);
        }
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

    form.on('switch(locked)', function (data) {
        var value = data.elem.value;
        if (value === 'true') {
            data.elem.value = 'false';
            $(data.elem).removeAttr('checked');
        } else {
            data.elem.value = 'true';
        }
    });

    function getAttrs(array, attr) {
        var arr = array.map((item) => {
            return item[attr];
        })
        return arr;
    }
});