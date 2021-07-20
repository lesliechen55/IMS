layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    table.init('detail', {
        //height: "full-860",
        cellMinWidth: 60
    });
    table.init('log', {
        //height: "full-860",
        cellMinWidth: 60
        //cols: [[
        //    { field: 'action', title: '操作类型' },
        //    { field: 'request', title: '操作请求', width: 200 },
        //    { field: 'response', title: '操作响应', width: 200 },
        //    { field: 'userId', title: '操作用户', width: 200 },
        //    { field: 'clientIP', title: '客户IP', width: 170 },
        //    { field: 'success', title: '是否成功', width: 100 },
        //    { field: 'createAt', title: '创建时间', width: 180 }
        //]]
    });
    //监听工具条
    table.on('tool(detail)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var data = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象
        if (layEvent === 'refund') { //退款

            //do somehing
            layer.prompt({
                formType: 2,
                //value: '请输入原因',
                title: '确认要退款吗？',
                area: ['500px', '220px'] //自定义文本域宽高
            }, function (value, index, elem) {
                if (!value) {
                    layer.msg('请输入原因');
                    return false;
                }
                var load = layer.load({ time: 10 * 1000 });
                $.post('/Pay/Payment/Refund', { orderId: obj.data.orderId, reason: value }, function (res) {
                    //layer.alert('操作失败:' + JSON.stringify(res.data), { title: '错误', icon: 5 });
                    layer.close(load);
                    if (!res.success) {
                        layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                        return false;
                    }
                    layer.msg('操作成功');
                    layer.close(index);
                    window.location.reload();
                    return false;
                });
            });
        }
        else if (layEvent === 'viewOrder') { //查订单
            var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
            window.top.addMyTab('/pay/purchaseorder/viewproduct?id=' + obj.data.orderId, "订单 " + obj.data.orderName, curmenu.topMenuName, curmenu.leftMenuName);
        }
    });
});