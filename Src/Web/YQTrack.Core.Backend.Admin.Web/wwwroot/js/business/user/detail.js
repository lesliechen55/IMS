layui.use(['layer', 'form', 'table', 'element'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        table = layui.table;
    var element = layui.element;
    table.init('info', {
    });
    table.init('device', {
    });
    table.init('payment', {
    });
    //亲测下面的代码有效果，会让表格自适应宽度，但是会报错误，目测是layui的bug，还是不加了
    //window.onresize = function () {
    //    table.reload('info', {});
    //    table.reload('device', {});
    //    table.reload('payment', {});
    //    //table.resize('info');
    //    //table.resize('device');
    //    //table.resize('payment');
    //}

    $('#more').click(function () {
        window.top.addMyTab('/pay/payment/index?paymentStatus=200,501&userId=' + $('#userId').val(), "支付记录", 'paycenter', '/pay/payment/index');
    });
    $('#shop').click(function () {
        var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
        window.top.addMyTab('/seller/usershop/index?userRoute=' + $('#userRoute').val(), "用户店铺", curmenu.topMenuName, curmenu.leftMenuName);
    });
    $('#task').click(function () {
        var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
        window.top.addMyTab('/seller/batchtask/index?userRoute=' + $('#userRoute').val(), "大批量任务", curmenu.topMenuName, curmenu.leftMenuName);
    });
});