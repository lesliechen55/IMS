layui.use(['form', 'layer', 'table', 'laytpl', 'laydate'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var objWhere = top.initFilter(window);
    var laydate = layui.laydate;

    laydate.render({
        elem: '#startDate', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#endDate', //指定元素
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
        url: '/Admin/Home/GetOperationLogPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 90,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'account', title: '操作员账号' },
            { field: 'nickName', title: '操作员昵称' },
            { field: 'ip', title: '操作员IP地址', width: 120 },
            { field: 'method', title: '操作方法' },
            { field: 'parameter', title: '操作方法参数' },
            { field: 'desc', title: '操作描述' },
            { field: 'operationType', title: '操作类型', width: 100 },
            { field: 'createdTime', title: '创建时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            account: window.hasSuperRole ? objWhere.account : '',
            method: objWhere.method,
            desc: objWhere.desc,
            operationType: objWhere.operationType,
            startDate: objWhere.startDate,
            endDate: objWhere.endDate
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
            }
        },
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
                    account: window.hasSuperRole ? object.account : '',
                    method: object.method,
                    desc: object.desc,
                    operationType: object.operationType,
                    startDate: object.startDate,
                    endDate: object.endDate
                }
            });
        return true;
    });

});