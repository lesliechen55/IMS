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
        url: '/Freight/Report/GetReportPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'id', title: '举报记录ID' },
            { field: 'channelTitle', title: '渠道名称' },
            { field: 'companyName', title: '公司名称' },
            { field: 'reasonType', title: '举报类型' },
            { field: 'detail', title: '举报详情' },
            { field: 'reportEmail', title: '举报人邮箱' },
            { field: 'reportTime', title: '举报时间' },
            { field: 'processStatus', title: '处理状态' },
            { field: 'processTime', title: '处理时间' },
            { field: 'processRemark', title: '备注' },
            { field: 'processDescription', title: '处理描述(用于邮件发送)' }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            channelName: objWhere.channelName,
            companyName: objWhere.companyName,
            processReportStatus: objWhere.processReportStatus
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
            layer.msg('请先选择有且仅有一条举报记录');
            return;
        }
        var title = checkStatus.data[0].channelTitle;
        var id = checkStatus.data[0].id;
        switch (obj.event) {
            case 'process':
                layer.open({
                    type: 2,
                    title: '处理渠道举报: ' + title,
                    content: '/Freight/Report/Process?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['550px', '400px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: false
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
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    channelName: object.channelName,
                    companyName: object.companyName,
                    processReportStatus: object.processReportStatus
                }
            });
        return true;
    });


});