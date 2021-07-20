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
        url: '/Freight/Company/GetCompanyPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'companyId', title: '公司ID' },
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
            { field: 'companyName', title: '公司名称' },
            { field: 'contact', title: '联系人' },
            { field: 'email', title: '邮箱' },
            { field: 'mobile', title: '手机号码' },
            { field: 'area', title: '地区代码' },
            { field: 'address', title: '企业地址' },
            { field: 'code', title: '社会信用代码' },
            { field: 'url', title: '官网', templet: '<div><a target="_blank" href="{{d.url}}" class="layui-table-link">{{d.url}}</a></div>' },
            { field: 'checkDescHistory', title: '驳回原因(累积)' },
            { field: 'createTime', title: '创建时间' },
            { field: 'updateTime', title: '更新时间' },
            { field: 'checkState', title: '审核状态' },
            { field: 'channelValidReportTimes', title: '有效举报次数' }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            companyName: objWhere.companyName,
            email: objWhere.email,
            status: objWhere.status
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    table.on('toolbar(table)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        if (checkStatus.data.length !== 1) {
            layer.msg('请先选择有且仅有一个运输商');
            return;
        }
        var title = checkStatus.data[0].companyName;
        var id = checkStatus.data[0].companyId;
        switch (obj.event) {
            case 'viewCheck':
                var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
                window.top.addMyTab('/freight/company/viewcheck?iframeName=' + window.name + '&id=' + id, '查看审核运输商-' + title, curmenu.topMenuName, curmenu.leftMenuName);
                break;
            case 'update':
                layer.open({
                    type: 2,
                    title: '修改资料 ' + title,
                    content: '/Freight/Company/Edit?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['500px', '360px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
        }
    });
    //监听工具条
    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        switch (obj.event) {
            case 'viewUser'://查看用户
                window.top.addMyTab('/business/user/detail?userId=' + obj.data.userId, '用户详情', 'systemcenter', '/business/user/index');
                break;
        }
    });
    $("#search").on("click", function () {
        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            }
        }
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    companyName: object.companyName,
                    email: object.email,
                    status: object.status
                }
            });
        return true;
    });

});