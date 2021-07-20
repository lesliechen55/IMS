layui.use(['form', 'layer', 'table', 'laytpl', 'laydate'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var objWhere = top.initFilter(window);

    var laydate = layui.laydate;

    //执行一个laydate实例
    laydate.render({
        elem: '#startTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#endTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    table.render({
        elem: '#table',
        method: 'post',
        url: '/Freight/InquiryManage/GetInquiryOrderStatusLogPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 50,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'orderId', title: '询价单ID', width: 180 },
            { field: 'from', title: '变更前', width: 100 },
            { field: 'to', title: '变更后', width: 100 },
            { field: 'desc', title: '描述信息' },
            { field: 'createTime', title: '创建时间', width: 160 }
        ]],
        parseData: function (res) {
            if (!res.success) {
                return {
                    "code": 0, //解析接口状态
                    "msg": res.msg, //解析提示文本
                    "count": 0, //解析数据长度
                    "data": [] //解析数据列表
                };
            }
            return res;
        },
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            orderId: objWhere.orderId,
            startTime: objWhere.startTime,
            endTime: objWhere.endTime
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
            }
        }
    });

    $("#search").on("click", function () {
        var orderId = $('#orderId').val();
        var startTime = $('#startTime').val();
        var endTime = $('#endTime').val();
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    orderId: orderId,
                    startTime: startTime,
                    endTime: endTime
                }
            });
        return true;
    });

});