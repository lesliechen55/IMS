layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var objWhere = top.initFilter(window);

    table.render({
        elem: '#table',
        method: 'post',
        url: '/Seller/UserShop/GetTrackUploadRecord',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'filePath', title: '文件路径'},
            { field: 'rowTotal', title: '总行数', width: 90, align: 'right' },
            { field: 'successTotal', title: '成功行数', width: 90, align: 'right' },
            { field: 'errorTotal', title: '错误行数', width: 90, align: 'right' },
            { field: 'errorDetail', title: '错误详细信息'}
        ]],
        page: false,
        where: {
            shopId: objWhere.shopId,
            userRoute: objWhere.userRoute
        }
    });
});