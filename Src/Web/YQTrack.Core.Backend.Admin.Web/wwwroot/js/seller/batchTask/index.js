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
        url: '/Seller/BatchTask/GetPageData',
        height: "full-" + (objWhere.top + 11),
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'batchId', title: '任务ID', width: 180 },
            { field: 'taskType', title: '任务类型' },
            { field: 'total', title: '处理总数', width: 120, align: 'right' },
            { field: 'success', title: '成功总数', width: 120, align: 'right' },
            { field: 'error', title: '错误总数', width: 120, align: 'right' },
            { field: 'taskStartTime', title: '任务开始时间', width: 160 },
            { field: 'taskEndTime', title: '任务结束时间', width: 160 },
            { field: 'taskStatus', title: '任务状态', width: 90},
            { field: 'createTime', title: '任务创建时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            userRoute: objWhere.userRoute,
            taskType: objWhere.taskType,
            taskStatus: objWhere.taskStatus
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
                    userRoute: object.userRoute,
                    taskType: object.taskType,
                    taskStatus: object.taskStatus
                }
            });
        return true;
    });
});