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
        url: '/Pay/ProductSku/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'productSkuId', title: 'SKU-ID', hide: true },
            { field: 'name', title: 'SKU名称' },
            { field: 'code', title: 'SKU编码' },
            { field: 'description', title: 'SKU描述' },
            { field: 'isInternalUse', title: '内部使用', width: 90 },
            { field: 'active', title: '是否启用', event: 'active', templet: '#active', width: 100 },
            { field: 'productName', title: '商品名称' },
            { field: 'productCategoryName', title: '商品分类' },
            { field: 'memberLevel', title: '会员类型', width: 180 },
            { field: 'skuType', title: '类型', width: 100 },
            { field: 'priceCount', title: '价格数量', width: 100 },
            { field: 'createAt', title: '创建时间', width: 160 },
            { field: 'updateAt', title: '更新时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            name: objWhere.name,
            code: objWhere.code,
            desc: objWhere.desc,
            productType: objWhere.productType
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    form.on('switch(active)', function (data) {
        var productSkuId = data.elem.value;
        var title = !data.elem.checked ? '您确定要禁用当前SKU吗' : '您确定要启用当前SKU吗';
        var x = data.elem.checked;
        layer.open({
            content: title
            , icon: 3
            , btn: ['确定', '取消']
            , yes: function (index, layero) {
                $.post('/Pay/ProductSku/ChangeStatus', { skuId: productSkuId, enable: x },
                    function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            data.elem.checked = !x;
                            form.render('checkbox');
                            return;
                        } else {
                            data.elem.checked = x;
                        }
                        form.render('checkbox');
                        layer.msg('操作成功!', { icon: 6 });
                        layer.close(index);
                    });
            }
            , btn2: function (index, layero) {
                data.elem.checked = !x;
                form.render('checkbox');
                layer.close(index);
            }
            , cancel: function () {
                data.elem.checked = !x;
                form.render('checkbox');
            }
        });
        return false;
    });

    table.on('toolbar(table)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加商品SKU',
                    content: '/Pay/ProductSku/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['900px', '490px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
            case 'edit':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个商品');
                    return;
                }
                var name = checkStatus.data[0].name;
                var id = checkStatus.data[0].productSkuId;
                layer.open({
                    type: 2,
                    title: '修改商品SKU ' + name,
                    content: '/Pay/ProductSku/Edit?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['1000px', '900px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
        }
    });

    $("#search").on("click", function () {
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    name: $('#name').val(),
                    code: $('#code').val(),
                    desc: $('#desc').val(),
                    productType: $('#productType').val()
                }
            });
        return true;
    });

});