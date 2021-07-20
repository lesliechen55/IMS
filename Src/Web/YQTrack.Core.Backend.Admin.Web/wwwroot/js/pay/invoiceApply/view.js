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
        layer.open({
            type: 2,
            title: "申请开票-审核通过",
            content: '/Pay/InvoiceApply/Pass?iframeName=' + window.name + '&id=' + $('#hiId').val() + '&invoiceEmail=' + $('#hiEmail').val(),
            skin: 'layui-layer-lan',
            area: ['500px', '380px'],
            offset: 'auto',
            shade: 0.3,
            maxmin: true
        });
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });
    $('#reject').click(function () {
        layer.open({
            type: 2,
            title: "申请开票-驳回",
            content: '/Pay/InvoiceApply/Reject?iframeName=' + window.name + '&id=' + $('#hiId').val(),
            skin: 'layui-layer-lan',
            area: ['500px', '350px'],
            offset: 'auto',
            shade: 0.3,
            maxmin: true
        });
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });
});