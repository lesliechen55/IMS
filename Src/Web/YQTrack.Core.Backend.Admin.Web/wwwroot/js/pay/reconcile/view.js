layui.use(['layer', 'table', 'laytpl'], function () {

    var layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    table.init('item', {
        height: "full-20",
        cellMinWidth: 60
    });
});