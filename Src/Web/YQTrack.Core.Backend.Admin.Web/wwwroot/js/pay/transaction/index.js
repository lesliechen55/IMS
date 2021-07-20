layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var objWhere = top.initFilter(window);
    function renderJson() {
        var index = layer.load({ time: 10 * 1000 });
        $.post('/Pay/Transaction/Query',
            {
                paymentProvider: $('#paymentProvider').val(),
                orderId: $('#orderId').val(),
                tradeNo: $('#tradeNo').val()
            },
            function (res) {
                layer.close(index);
                if (!res.success) {
                    layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                    return;
                }
                var input = res.data;
                var options = {
                    collapsed: false,
                    rootCollapsable: true,
                    withQuotes: true,
                    withLinks: true
                };
                $('#json-renderer').jsonViewer(input, options);
            });
        return;
    }

    $("#search").on("click", function () {
        $('#json-renderer').empty();
        let orderId = $('#orderId').val();
        let tradeNo = $('#tradeNo').val();
        if ($.trim(orderId).length <= 0 && $.trim(tradeNo).length <= 0) {
            layer.msg('缺少必要查询参数orderId或者tradeNo');
            return;
        }
        renderJson();
        top.changeUrlParam(window);
    });
});