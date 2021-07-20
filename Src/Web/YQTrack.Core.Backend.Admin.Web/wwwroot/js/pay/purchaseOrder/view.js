layui.use(['form', 'layer', 'table', 'laytpl', 'jquery'], function () {

    var layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    table.init('detail', {
        //height: "full-860",
        cellMinWidth: 60
    });
    //table.init('product', {
    //    //height: "full-860",
    //    cellMinWidth: 60
    //});
    //监听工具条
    table.on('tool(detail)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var data = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象
        if (layEvent === 'view') {
            var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
            window.top.addMyTab('/pay/payment/view?id=' + obj.data.purchaseOrderId, "支付详情", curmenu.topMenuName, curmenu.leftMenuName);
        }
    });

    $('#okPresent').click(function () {
        let skuId = parseInt($('#productSku').val());
        let quantity = parseInt($('#quantity').val());
        layer.confirm('您确定要赠与 ' + window.userEmail + ' 用户所选商品吗?', { icon: 3, title: '警告' }, function (index) {
            $.post('/Pay/PurchaseOrder/Present', { orderId: window.orderId, skuId: skuId, quantity: quantity }, function (res) {
                if (!res.success) {
                    layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                    return false;
                }
                layer.msg('操作成功');
                layer.open({
                    title: '赠送充值成功'
                    , content: '赠送订单号:' + res.data
                });
                layer.close(index);
                return false;
            });
        });
        return false;
    });

});