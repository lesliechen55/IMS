layui.use(['laydate', 'form', 'layer', 'table', 'laytpl'], function () {

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
            if (endTime) {
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
            if (startTime) {
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
        url: '/Message/SendTask/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'projectName', title: '项目名称', width: 100 },
            { field: 'channel', title: '发送通道', width: 100 },
            { field: 'templateName', title: '模板名称' },
            { field: 'remarks', title: '任务描述' },
            { field: 'stateName', title: '发送状态', width: 100 },
            { field: 'pushSucess', title: '成功数', width: 80 },
            { field: 'pushFail', title: '失败数', width: 80 },
            { field: 'createBy', title: '创建人', width: 100 },
            { field: 'createTime', title: '创建时间', width: 160 },
            { field: 'updateBy', title: '修改人', width: 100 },
            { field: 'updateTime', title: '更新时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            projectId: objWhere.projectId,
            channelId: objWhere.channelId,
            startTime: objWhere.startTime,
            endTime: objWhere.endTime,
            templateName: objWhere.templateName
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
            case 'update':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个基础模板');
                    return;
                }
                var title = checkStatus.data[0].remarks;
                var id = checkStatus.data[0].taskId;
                var state = checkStatus.data[0].state;
                if (state != 5) {
                    layer.alert(title + '处于“' + checkStatus.data[0].stateName + '”状态，不能修改。');
                    return false;
                }
                layer.open({
                    type: 2,
                    title: '修改 ' + title,
                    content: '/Message/SendTask/Edit?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['700px', '520px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
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
        //var date = object.updateTime.split('+~+');
        //console.log(date);
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    projectId: object.projectId,
                    channelId: object.channelId,
                    startTime: object.startTime,
                    endTime: object.endTime,
                    templateName: object.templateName
                }
            });
        return true;
    });

});