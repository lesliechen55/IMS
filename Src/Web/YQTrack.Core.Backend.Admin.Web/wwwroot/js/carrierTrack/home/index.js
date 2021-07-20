layui.use(['form', 'layer', 'table', 'laytpl', 'jquery'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var objWhere = top.initFilter(window);

    var carrierTable = table.render({
        elem: '#table',
        method: 'post',
        url: '/CarrierTrack/Home/GetPageData/',
        height: "full-" + objWhere.top,
        cellMinWidth: 90,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: "controlId", title: '控制ID', hide: true },
            { field: "userId", title: '用户ID' },
            {
                field: 'userId', title: '用户ID', templet: function (d) {
                    return '<a class="layui-table-link" lay-event="viewUser" lay-filter="viewUser">' + d.userId + '</a>';
                }, width: 180
            },
            { field: 'email', title: '用户邮箱' },
            { field: 'enable', title: '是否启用', templet: '#enable', width: 100 },
            { field: 'importTodayLimit', title: '当日导入限制' },
            { field: 'exportTimeLimit', title: '单次导出限制' },
            { field: 'offlineDay', title: '离线天数' },
            { field: 'createTime', title: '创建时间', width: 160 },
            { field: 'updateTime', title: '更新时间', width: 160 }
        ]],
        where: {
            email: objWhere.email,
            enable: objWhere.userStatus,
            offlineDay: objWhere.offlineDay
        },
        done: function (res, curr, count) {
            top.changeUrlParam(window);
        }//,
        //parseData: function (res) {
        //    if (!res.success) {
        //        return {
        //            "code": 0, //解析接口状态
        //            "msg": res.msg, //解析提示文本
        //            "count": 0, //解析数据长度
        //            "data": [], //解析数据列表
        //            "success": false
        //        };
        //    }
        //    return res;
        //},
        //done: function (res, curr, count) {
        //    if (res && res.success !== undefined && !res.success) {
        //        layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
        //    }
        //}
    });

    table.on('toolbar(table)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加用户资料',
                    content: '/CarrierTrack/Home/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['520px', '360px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
            case 'update':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个用户');
                    return;
                }
                var title = checkStatus.data[0].email;
                var id = checkStatus.data[0].controlId;
                var userId = checkStatus.data[0].userId;
                layer.open({
                    type: 2,
                    title: '编辑用户 ' + title,
                    content: '/CarrierTrack/Home/Edit?iframeName=' + window.name + '&id=' + id + '&userId=' + userId,
                    skin: 'layui-layer-lan',
                    area: ['520px', '400px'],
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
        var email = $('#email').val();
        //if ($.trim(email).length <= 0) {
        //    layer.alert('请填写用户邮箱', { title: '错误', icon: 5 });
        //    return false;
        //}
        var userStatus = $('#userStatus').val();
        var offlineDay = $('#offlineDay').val();
        if (offlineDay !== '') {
            if (parseInt(offlineDay) < 0) {
                layer.alert('离线天数不能小于0,请重新填写', { title: '错误', icon: 5 });
                return false;
            }
        }
        //carrierTable.config.url = '/CarrierTrack/Home/GetPageData/';
        table.reload('table',
            {
                //page: {
                //    curr: 1 // 重新从第 1 页开始
                //},
                where: {
                    email: email,
                    enable: userStatus,
                    offlineDay: offlineDay
                }
            });
        return true;
    });

});