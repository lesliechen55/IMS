layui.use(['layer', 'jquery', 'form', 'table'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        table = layui.table;

    var messageId = $('#messageId').val();

    table.render({
        id: 'table',
        elem: '#table',
        method: 'post',
        height: 500,
        url: '/Business/Email/GetDeliveryData?messageId=' + messageId,
        page: false,
        cellMinWidth: 90,
        cols: [[
            { field: "providerType", title: '发送渠道', width: 130 },
            { field: 'reportType', title: '报告类型', width: 100 },
            { field: 'reportContent', title: '报告内容' },
            { field: 'createTime', title: '创建时间', width: 160 }
        ]]
    });
});