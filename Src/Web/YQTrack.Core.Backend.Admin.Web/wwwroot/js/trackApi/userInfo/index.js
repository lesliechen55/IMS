layui.use(['form', 'layer', 'table', 'laytpl', 'laydate'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var objWhere = top.initFilter(window);
    //执行一个laydate实例
    layui.laydate.render({
        elem: '#startTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2015-1-1',
        max: '2050-12-31',
        lang: 'cn',
        theme: 'molv',
        //range: '~',//或 range: '~' 来自定义分割字符
        calendar: true,
        done: function (value, date, endDate) {
            //console.log(value); //得到日期生成的值，如：2017-08-18
            //console.log(date); //得到日期时间对象：{year: 2017, month: 8, date: 18, hours: 0, minutes: 0, seconds: 0}
            //console.log(endDate); //得结束的日期时间对象，开启范围选择（range: true）才会返回。对象成员同上。
            var endTime = $('#endTime').val();
            if (endTime && value) {
                if (value > endTime) {
                    layer.alert('开始日期不能大于结束日期');
                    $('#startTime').val("");
                }
            }
        }
    });

    //执行一个laydate实例
    layui.laydate.render({
        elem: '#endTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2015-1-1',
        max: '2050-12-31',
        lang: 'cn',
        theme: 'molv',
        //range: '~',//或 range: '~' 来自定义分割字符
        calendar: true,
        done: function (value, date, endDate) {
            var startTime = $('#startTime').val();
            if (startTime && value) {
                if (value < startTime) {
                    layer.alert('开始日期不能大于结束日期');
                    $('#endTime').val("");
                }
            }
        }
    });

    table.render({
        elem: '#table',
        method: 'post',
        url: '/TrackApi/UserInfo/GetPageData',
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
                        return '<a class="layui-table-link" lay-event="viewUser" lay-filter="viewUser" value="' + d.userNo + '">' + d.userId + '</a>';
                    }
                }, width: 180
            },
            { field: 'userNo', title: '用户编号', width: 90 },
            { field: 'userName', title: '用户名称' },
            { field: 'email', title: '注册邮箱' },
            { field: 'contactEmail', title: '联系邮箱' },
            { field: 'scheduleFrequency', title: '调度频率', width: 90, align: 'center' },
            { field: 'giftQuota', title: '赠送配额', width: 90, align: 'right' },
            {
                field: 'remain', title: '剩余额度', width: 120, align: 'right', templet: function (d) {
                    return '<a class="layui-table-link" lay-event="view" lay-filter="view" value="' + d.userNo + '">' + d.remain + '</a>';
                }
            },
            { field: 'todayUsed', title: '当日使用', width: 90, align: 'right' },
            { field: 'contactName', title: '联系人', hide: true },
            { field: 'contactPhone', title: '联系电话', hide: true },
            {
                field: 'apiState', title: '接口授权', templet: function (d) {
                    return '<input type="checkbox" name="apiState" lay-skin="switch" lay-text="正常|停用" lay-filter="state" value="' + d.userNo +
                        (d.apiState == 1 ? '" checked' : '"') +
                        '>';
                }, width: 95
            },
            { field: 'createdTime', title: '注册时间', width: 160 },
            {
                field: 'operate', title: '操作', templet: function (d) {
                    var act = '';
                    if (d.userId == '0') {
                        act =
                            '<a class="layui-btn layui-btn layui-btn-xs" lay-event="reregister" lay-filter="reregister" value="' +
                            d.userNo +
                            '">重试</a>';
                    }
                    else {
                        act = '<a class="layui-btn layui-btn layui-btn-xs" lay-event="edit" lay-filter="edit" value="' + d.userNo + '">编辑</a>';
                    }
                    return act;
                }, width: 70
            }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            userId: objWhere.userId,
            userName: objWhere.userName,
            apiState: objWhere.apiState,
            startTime: objWhere.startTime,
            endTime: objWhere.endTime
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
                    userId: object.userId,
                    userName: object.userName,
                    apiState: object.apiState,
                    startTime: object.startTime,
                    endTime: object.endTime
                }
            });
        return true;
    });

    form.on('switch(state)', function (data) {
        var apiState = data.elem.checked ? 1 : 2;
        var userNo = data.elem.value;
        var title = apiState == 1 ? '您确定要启用该用户API访问授权吗？' : '您确定要停止该用户API访问授权吗？<br />停止后，该用户不可请求接口';
        layer.open({
            content: title
            , icon: 3
            , btn: ['确定', '取消']
            , yes: function (index, layero) {
                $.post('/TrackApi/UserInfo/ChangeStatus', { userNo: userNo, apiState: apiState },
                    function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            data.elem.checked = !data.elem.checked;
                            form.render('checkbox');
                        } else {
                            data.elem.checked = data.elem.checked;
                            layer.msg('操作成功');
                        }
                        layer.close(index);
                    });
            }
            , btn2: function (index, layero) {
                data.elem.checked = !data.elem.checked;
                form.render('checkbox');
                layer.close(index);
            }
            , cancel: function () {
                data.elem.checked = !data.elem.checked;
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
                    title: '添加用户',
                    content: '/TrackApi/UserInfo/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['800px', '930px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
        }
    });

    //监听工具条
    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var data = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象
        var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
        switch (layEvent) {
            case 'edit'://编辑
                //do somehing
                layer.open({
                    type: 2,
                    title: "修改 " + data.userName,
                    content: '/TrackApi/UserInfo/Edit?iframeName=' + window.name + '&id=' + data.userNo,
                    skin: 'layui-layer-lan',
                    area: ['800px', '930px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
            case 'reregister'://重新注册
                $.get('/TrackApi/UserInfo/Reregister', { userNo: data.userNo },
                    function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                        }
                        else {
                            layer.msg('操作成功');
                            location.reload();
                        }
                    });
                break;
            case 'view'://查看额度消耗
                window.top.addMyTab('/trackapi/userinfo/view?id=' + data.userNo, 'API额度消耗 ' + data.userName, curmenu.topMenuName, curmenu.leftMenuName);
                break;
            case 'viewUser'://查看用户详情
                window.top.addMyTab('/business/user/detail?userId=' + data.userId, '用户详情 ' + data.email, 'systemcenter', curmenu.leftMenuName);
                break;
        }
    });

});