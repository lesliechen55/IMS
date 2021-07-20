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
        url: '/Admin/Home/GetLoginLogPageData/',
        height: "full-" + objWhere.top,
        cellMinWidth: 90,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'account', title: '账号' },
            { field: 'nickName', title: '昵称' },
            { field: 'ip', title: 'IP地址', width: 120 },
            { field: 'platform', title: '登陆平台', width: 90 },
            { field: 'userAgent', title: '用户代理' },
            { field: 'loginTime', title: '登陆时间', width: 160 },
            { field: 'createdTime', title: '创建时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            platform: objWhere.platform,
            nickName: objWhere.nickName,
            account: objWhere.account
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    $("#search").on("click", function () {
        var nickName = window.hasSuperRole ? $('#nickName').val() : '';
        var account = window.hasSuperRole ? $('#account').val() : '';
        var platform = $('#platform').val();
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    platform: platform,
                    nickName: nickName,
                    account: account
                }
            });
        return true;
    });

});