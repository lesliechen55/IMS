layui.use(['layer', 'jquery', 'form', 'element', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table,
        element = layui.element;

    element.on('tab(tabFilter)', function (data) {
        if (data.index === 0) {
            table.reload('tablePrice');
        }
        if (data.index === 1) {
            table.reload('tableBusiness');
        }
    });

    const skuId = $('#productSkuId').val();
    const enable = $('#active').val() === 'true' ? true : false;

    form.on('switch(isInternalUse)', function (data) {
        var value = data.elem.value;
        if (value === 'true') {
            data.elem.value = 'false';
            $(data.elem).removeAttr('checked');
        } else {
            data.elem.value = 'true';
        }
    });

    form.on('submit(form)', function (data) {
        let isInternalUse = $('input[name=isInternalUse]').val();
        if (isInternalUse === 'true' && data.field.skuType !== '2' && data.field.skuType !== '3') {
            layer.alert('操作失败:只能SKU类型为邮件数或者查询数才能启用内部使用开关!', { title: '警告', icon: 5 });
            return false;
        }
        data.field.isInternalUse = isInternalUse;
        $.post('/Pay/ProductSku/Edit', data.field, function (res) {
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

    table.render({
        elem: '#tablePrice',
        method: 'post',
        url: '/Pay/ProductSku/GetPriceList?skuId=' + skuId,
        page: false,
        height: 330,
        cellMinWidth: 60,
        toolbar: '#toolbarPrice',
        defaultToolbar: [],
        limits: [10, 15, 20, 25],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'productSkuPriceId', title: 'SkuPriceId', hide: true },
            { field: 'platformType', title: '平台类型' },
            { field: 'currencyType', title: '货币类型' },
            { field: 'saleUnitPrice', title: '销售价' },
            { field: 'amount', title: '实付价' },
            { field: 'description', title: '描述' },
            { field: 'createAt', title: '创建时间', minWidth: 180 },
            { field: 'updateAt', title: '更新时间', minWidth: 180 }
        ]]
    });

    table.on('toolbar(tablePrice)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加SKU价格信息',
                    content: '/Pay/ProductSku/AddPrice?iframeName=' + window.name + '&id=' + skuId + '&tableId=tablePrice',
                    skin: 'layui-layer-lan',
                    area: ['900px', '420px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true,
                    end: function () {
                        table.reload('tablePrice');
                    }
                });
                break;
            case 'delete':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个SKU价格信息');
                    return;
                }
                var id = checkStatus.data[0].productSkuPriceId;
                layer.confirm('您确定要删除当条价格信息吗?', { icon: 3, title: '提示' }, function (index) {
                    $.post('/Pay/ProductSku/DeletePrice', { skuId: skuId, priceId: id }, function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.msg('操作成功');
                        table.reload('tablePrice');
                        layer.close(index);
                        return false;
                    });
                });
                break;
        }
    });

    table.render({
        elem: '#tableBusiness',
        method: 'post',
        url: '/Pay/ProductSku/GetBusinessList?skuId=' + skuId,
        page: false,
        height: 330,
        cellMinWidth: 60,
        toolbar: '#toolbarBusiness',
        defaultToolbar: [],
        limits: [10, 15, 20, 25],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'businessCtrlType', title: '业务类型' },
            { field: 'consumeType', title: '消费类型' },
            {
                field: 'renew', title: '是否续费', templet: function (d) {
                    return '<input type="checkbox" lay-skin="switch" lay-text="ON|OFF" ' +
                        (d.renew ? 'checked' : '') + ' disabled>';
                }
            },
            { field: 'validity', title: '有效期(月)' },
            { field: 'quantity', title: '业务数量' }
        ]]
    });

    table.on('toolbar(tableBusiness)', function (obj) {
        if (enable) {
            layer.msg('SKU启用之后不能添加或者修改业务信息', { icon: 5 });
            return;
        }
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加SKU业务控制信息',
                    content: '/Pay/ProductSku/AddBusiness?iframeName=' + window.name + '&id=' + skuId,
                    skin: 'layui-layer-lan',
                    area: ['900px', '420px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true,
                    end: function () {
                        table.reload('tableBusiness');
                    }
                });
                break;
            case 'delete':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个SKU业务控制信息');
                    return;
                }
                var businessCtrlType = checkStatus.data[0].businessCtrlType;
                layer.confirm('您确定要删除当条业务信息吗?', { icon: 3, title: '提示' }, function (index) {
                    $.post('/Pay/ProductSku/DeleteBusiness', { skuId: skuId, businessCtrlType: businessCtrlType }, function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.msg('操作成功');
                        table.reload('tableBusiness');
                        layer.close(index);
                        return false;
                    });
                });
                break;
        }
    });

});