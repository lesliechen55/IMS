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
        url: '/Pay/Invoice/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            {
                field: 'userId', title: '用户ID', templet: function (d) {
                    if (d.userId == '0') {
                        return d.userId;
                    }
                    else {
                        return '<a class="layui-table-link" lay-event="viewUser" lay-filter="viewUser">' + d.userId + '</a>';
                    }
                }, width: 180
            },
            { field: 'userEmail', title: '注册邮箱' },
            { field: 'invoiceType', title: '发票类型', width: 150 },
            { field: 'companyName', title: '公司名称' },
            { field: 'taxNo', title: '税号' },
            { field: 'contact', title: '联系人', width: 140 },
            { field: 'createAt', title: '创建时间', width: 160 },
            { field: 'updateAt', title: '更新时间', width: 160 },
            {
                field: 'operate', title: '操作', templet: function (d) {
                    return '<a class="layui-btn layui-btn layui-btn-xs" lay-event="view" lay-filter="view" value="' + d.invoiceId + '">详情</a><a class="layui-btn layui-btn layui-btn-xs" lay-event="edit" lay-filter="edit" value="' + d.invoiceId + '">编辑</a>';
                }, width: 110
            }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            userEmail: objWhere.userEmail,
            status: objWhere.status
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
                object[data[key].split("=")[0]] = (data[key].split("=")[1]).replace(/\+/g, " ");
            }
        }
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    userEmail: object.userEmail
                }
            });
        return true;
    });
    table.on('toolbar(table)', function (obj) {
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加发票资料',
                    content: '/Pay/Invoice/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['800px', '780px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
        }
    });
    //监听工具条
    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
        switch (obj.event) {
            case 'view'://查看
                window.top.addMyTab('/pay/invoice/view?id=' + obj.data.invoiceId, '查看发票资料', curmenu.topMenuName, curmenu.leftMenuName);
                break;
            case 'edit'://编辑
                layer.open({
                    type: 2,
                    title: "修改发票资料",
                    content: '/Pay/Invoice/Edit?iframeName=' + window.name + '&id=' + obj.data.invoiceId,
                    skin: 'layui-layer-lan',
                    area: ['800px', '780px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
            case 'viewUser'://查看用户
                window.top.addMyTab('/business/user/detail?userId=' + obj.data.userId, '用户详情 ' + obj.data.userEmail, 'systemcenter', curmenu.leftMenuName);
                break;
        }
    });

});