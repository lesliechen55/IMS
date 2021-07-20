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
        url: '/Pay/ProductCategory/GetPageData/',
        height: "full-" + objWhere.top,
        cellMinWidth: 90,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: "productCategoryId", title: '分类ID', hide: true },
            { field: 'name', title: '分类名称' },
            { field: 'code', title: '分类编码' },
            { field: 'description', title: '描述' },
            { field: 'productCount', title: '商品数量', width: 100 },
            { field: 'createAt', title: '创建时间', width: 160 },
            { field: 'updateAt', title: '更新时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            name: objWhere.name
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    table.on('toolbar(table)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加商品分类',
                    content: '/Pay/ProductCategory/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['500px', '350px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'edit':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个商品分类');
                    return;
                }
                var title = checkStatus.data[0].name;
                var id = checkStatus.data[0].productCategoryId;
                layer.open({
                    type: 2,
                    title: '编辑商品分类 ' + title,
                    content: '/Pay/ProductCategory/Edit?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['500px', '350px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
        }
    });

    $("#search").on("click", function () {
        var name = $('#name').val();
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    name: name
                }
            });
        return true;
    });
});