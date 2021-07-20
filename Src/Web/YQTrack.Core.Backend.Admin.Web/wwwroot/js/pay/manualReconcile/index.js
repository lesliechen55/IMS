layui.use(['form', 'layer', 'table'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var objWhere = top.initFilter(window);
    table.render({
        elem: '#table',
        method: 'post',
        url: '/Pay/ManualReconcile/GetPageData',
        page: true,
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        limits: [10, 15, 20, 25],
        cols: [[
            { type: 'numbers' },
            //{ type: 'radio' },
            { field: 'fileName', title: '文件名', minWidth: 300 },
            { field: 'fileMd5', title: '文件MD5', minWidth: 350 },
            { field: 'year', title: '年份' },
            { field: 'month', title: '月份' },
            { field: 'orderCount', title: '订单数量' },
            { field: 'remark', title: '备注' },
            { field: 'createdTime', title: '创建时间', minWidth: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            year: objWhere.year,
            month: objWhere.month
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

    table.on('toolbar(table)', function (obj) {
        switch (obj.event) {
            case 'import':
                //执行实例
                layer.open({
                    type: 2,
                    title: '导入信用卡对账单',
                    content: '/Pay/ManualReconcile/ImportGlocash?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['430px', '380px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
        }
    });

    $("#search").on("click", function () {

        var year = $('#year').val();
        var month = $('#month').val();

        table.reload('table',
            {
                where: {
                    year: year,
                    month: month
                }
            });
        return true;
    });

});