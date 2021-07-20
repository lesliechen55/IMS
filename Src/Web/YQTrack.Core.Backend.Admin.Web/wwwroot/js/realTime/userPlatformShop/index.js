layui.use(['layer', 'jquery', 'table', 'form', 'laytpl'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var laydate = layui.laydate;

    var objWhere = top.initFilter(window);

    table.render({
        elem: '#table',
        method: 'post',
        url: '/RealTime/UserPlatformShop/GetData',
        cellMinWidth: 300,
        checkStatus: false,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        height: "full-" + objWhere.top,
        cols: [[
            { type: 'numbers' },
            {
                field: 'platformType', title: '平台类型'
            },
            { field: 'platformTypeName', title: '平台名称' },
            { field: 'sumCount', title: '平台用户总数量' }
        ]],
        where: {
            platformType: objWhere.sltPlatformType
        },
        done: function (res, curr, count) {
            top.changeUrlParam(window);
            if (res && res.success !== undefined && !res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
            }
        }
    });

    $("#search").on("click", function () {
        var params = get_parameters();
        table.reload('table',
            {
                where: {
                    platformType: params.sltPlatformType
                }
            });
        return true;
    });

    function get_parameters() {
        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]).replace(/\+/g, " ");
            }
        }
        return object;
    };
});