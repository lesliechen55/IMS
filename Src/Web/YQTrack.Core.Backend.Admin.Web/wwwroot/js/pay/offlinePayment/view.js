layui.use(['layer', 'jquery', 'form'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;
    layui.layer.photos({
        photos: '#imgDiv',
        shadeClose: false,
        closeBtn: 2,
        anim: 5 //0-6的选择，指定弹出图片动画类型，默认随机（请注意，3.0之前的版本用shift参数）
    });
    $('#pass').click(function () {

        // 检查计算金额和交易金额是否一致否则弹出Confirm提示
        let amount = $('#amount').val();
        let calculateAmount = $('#calculateAmount').val();

        if (amount !== calculateAmount) {
            layer.confirm('系统检测到当前交易金额与计算金额不一致, 请仔细核对之后再次确认?', { icon: 3, title: '警告' }, function(index) {
                openPassView();
                layer.close(index);
            });
            return false;
        }
        openPassView();
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });

    function openPassView() {
        layer.open({
            type: 2,
            title: "线下交易-审核通过",
            content: '/Pay/OfflinePayment/Pass?iframeName=' + window.name + '&id=' + $('#hiId').val(),
            skin: 'layui-layer-lan',
            area: ['400px', '250px'],
            offset: 'auto',
            shade: 0.3,
            maxmin: true
        });
    }

    $('#reject').click(function () {
        layer.open({
            type: 2,
            title: "线下交易-驳回",
            content: '/Pay/OfflinePayment/Reject?iframeName=' + window.name + '&id=' + $('#hiId').val(),
            skin: 'layui-layer-lan',
            area: ['500px', '350px'],
            offset: 'auto',
            shade: 0.3,
            maxmin: true
        });
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });
});