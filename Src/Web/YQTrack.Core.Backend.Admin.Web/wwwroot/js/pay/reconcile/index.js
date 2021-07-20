layui.use(['form', 'layer', 'table', 'laytpl', 'laydate'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table,
        formSelects = layui.formSelects;

    var objWhere = top.initFilter(window, formSelects);
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
        url: '/Pay/Reconcile/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'providerId', title: '支付商' },
            { field: 'sheetCount', title: '对账笔数' },
            { field: 'successCount', title: '成功笔数' },
            {
                field: 'failedCount', title: '失败笔数', templet: '#failedCountTpl'
            },
            { field: 'notExistCount', title: '不存在笔数' },
            { field: 'refundedCount', title: '退款笔数' },
            { field: 'totalCount', title: '总笔数' },
            { field: 'beginTime', title: '对账时间' }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            paymentProvider: formSelects.value('paymentProvider', 'val'),
            startTime: objWhere.startTime,
            endTime: objWhere.endTime
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
            layer.msg('请先选择有且仅有一个对账记录');
            return;
        }
        var id = checkStatus.data[0].reconcileId;
        var provider = checkStatus.data[0].providerId;
        var beginTime = checkStatus.data[0].beginTime;
        switch (obj.event) {
            case 'view':
                var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
                window.top.addMyTab('/pay/reconcile/view?id=' + id, "对账记录明细-" + provider + "【" + beginTime + "】", curmenu.topMenuName, curmenu.leftMenuName);
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
                    paymentProvider: formSelects.value('paymentProvider', 'val'),
                    startTime: object.startTime,
                    endTime: object.endTime
                }
            });
        return true;
    });

});