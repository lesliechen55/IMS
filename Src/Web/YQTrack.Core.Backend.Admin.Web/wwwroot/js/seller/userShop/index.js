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
        url: '/Seller/UserShop/GetPageData',
        height: "full-" + (objWhere.top + 11),
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            {
                field: 'shopId', title: '店铺ID', width: 180, templet: function (d) {
                    return '<a class="layui-table-link" lay-event="viewUpload" lay-filter="viewUpload" value="' + d.shopId + '">' + d.shopId + '</a>';
                }
            },
            { field: 'shopName', title: '店铺名称' },
            { field: 'shopAlias', title: '店铺标签' },
            { field: 'platformType', title: '平台类型' },
            { field: 'platformUID', title: '平台编号', width: 100, hide: true },
            { field: 'lastSyncNum', title: '最新同步数量', width: 120, align: 'right' },
            { field: 'lastSyncTime', title: '最新同步时间', width: 160 },
            { field: 'nextSyncTime', title: '下次同步时间', width: 160 },
            { field: 'state', title: '店铺状态', width: 90 },
            { field: 'createTime', title: '店铺创建时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            userRoute: objWhere.userRoute,
            platformType: objWhere.platformType,
            shopName: objWhere.shopName,
            state: objWhere.state
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    $("#search").on("click", function () {
        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]).replace(/\+/g, " ");
            }
        }
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    userRoute: object.userRoute,
                    platformType: object.platformType,
                    shopName: object.shopName,
                    state: object.state
                }
            });
        return true;
    });

    //监听工具条
    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var data = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象
        switch (layEvent) {
            case 'viewUpload'://查看上传记录
                layer.open({
                    type: 2,
                    title: '店铺导入记录-' + data.shopName,
                    content: '/seller/usershop/trackuploadrecord?shopId=' + data.shopId + '&userRoute=' + encodeURIComponent($('#userRoute').val()),
                    skin: 'layui-layer-lan',
                    area: ['800px', '700px'],
                    offset: 'auto',
                    shade: false,
                    maxmin: true
                });
                break;
        }
    });
});