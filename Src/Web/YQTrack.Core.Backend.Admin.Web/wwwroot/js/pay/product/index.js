layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table,
        formSelects = layui.formSelects;

    var objWhere = top.initFilter(window, formSelects);
    table.render({
        elem: '#table',
        method: 'post',
        url: '/Pay/Product/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'code', title: '商品编码' },
            { field: 'name', title: '商品名称' },
            { field: 'categoryName', title: '商品分类' },
            { field: 'description', title: '商品描述' },
            { field: 'serviceType', title: '服务类型' },
            { field: 'role', title: '适用角色' },
            { field: 'isSubscription', title: '是否订阅' },
            { field: 'skuCount', title: 'Sku数量' },
            {
                field: 'active', title: '是否启用', templet: function (d) {
                    return '<input type="checkbox" name="active" lay-skin="switch" lay-text="开启|关闭" skuCount="' + d.skuCount + '" lay-filter="active" value="' + d.productId +
                        (d.active ? '" checked' : '"') +
                        '>';
                }, width: 100
            },
            { field: 'createAt', title: '创建时间', width: 160 },
            { field: 'updateAt', title: '更新时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            productCategory: objWhere.productCategory,
            serviceType: formSelects.value('serviceType', 'val'),
            role: formSelects.value('role', 'val'),
            keyword: objWhere.keyword
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });
    $("#search").on("click", function () {
        var data = decodeURIComponent($("form").serialize(), true).split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = data[key].split("=")[1].replace(/\+/g, " ");
            }
        }
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    productCategory: object.productCategory,
                    serviceType: formSelects.value('serviceType', 'val'),
                    role: formSelects.value('role', 'val'),
                    keyword: object.keyword
                }
            });
        return true;
    });
    form.on('switch(active)', function (data) {
        var active = data.elem.checked;
        var skuCount = $(data.elem).attr('skuCount');
        if (active && skuCount == 0) {
            layer.alert('操作失败:没有SKU数据，不能启用', { title: '错误', icon: 5 });
            data.elem.checked = !active;
            form.render('checkbox');
            return false;
        }
        var productId = data.elem.value;
        var title = !active ? '您确定要禁用当前商品吗' : '您确定要启用当前商品吗';
        layer.open({
            content: title
            , icon: 3
            , btn: ['确定', '取消']
            , yes: function (index, layero) {
                $.post('/Pay/Product/ChangeStatus', { productId: productId, active: active },
                    function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            data.elem.checked = !active;
                            form.render('checkbox');
                        } else {
                            data.elem.checked = active;
                            layer.msg('操作成功');
                        }
                        layer.close(index);
                    });
            }
            , btn2: function (index, layero) {
                data.elem.checked = !active;
                form.render('checkbox');
                layer.close(index);
            }
            , cancel: function () {
                data.elem.checked = !active;
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
                    title: '添加商品',
                    content: '/Pay/Product/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['900px', '620px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
            case 'update':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个商品');
                    return;
                }
                var name = checkStatus.data[0].name;
                var id = checkStatus.data[0].productId;
                layer.open({
                    type: 2,
                    title: '修改 ' + name,
                    content: '/Pay/Product/Edit?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['900px', '620px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
        }
    });
});