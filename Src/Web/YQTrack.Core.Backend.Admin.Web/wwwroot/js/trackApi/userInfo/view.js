layui.use(['layer', 'jquery', 'table', 'form', 'laydate'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        table = layui.table;

    //执行一个laydate实例
    layui.laydate.render({
        elem: '#invoiceStartTime', //指定元素
        type: 'month',
        format: 'yyyy-MM',
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
            var endTime = $('#invoiceEndTime').val();
            if (endTime && value) {
                if (value > endTime) {
                    layer.alert('开始日期不能大于结束日期');
                    $('#invoiceStartTime').val("");
                }
            }
        }
    });
    //执行一个laydate实例
    layui.laydate.render({
        elem: '#invoiceEndTime', //指定元素
        type: 'month',
        format: 'yyyy-MM',
        min: '2015-1-1',
        max: '2050-12-31',
        lang: 'cn',
        theme: 'molv',
        //range: '~',//或 range: '~' 来自定义分割字符
        calendar: true,
        done: function (value, date, endDate) {
            var startTime = $('#invoiceStartTime').val();
            if (startTime && value) {
                if (value < startTime) {
                    layer.alert('开始日期不能大于结束日期');
                    $('#invoiceEndTime').val("");
                }
            }
        }
    });
    //执行一个laydate实例
    layui.laydate.render({
        elem: '#trackCountStartTime', //指定元素
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
            var endTime = $('#trackCountEndTime').val();
            if (endTime && value) {
                if (value > endTime) {
                    layer.alert('开始日期不能大于结束日期');
                    $('#trackCountStartTime').val("");
                }
            }
        }
    });
    //执行一个laydate实例
    layui.laydate.render({
        elem: '#trackCountEndTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2015-1-1',
        max: '2050-12-31',
        lang: 'cn',
        theme: 'molv',
        //range: '~',//或 range: '~' 来自定义分割字符
        calendar: true,
        done: function (value, date, endDate) {
            var startTime = $('#trackCountStartTime').val();
            if (startTime && value) {
                if (value < startTime) {
                    layer.alert('开始日期不能大于结束日期');
                    $('#trackCountEndTime').val("");
                }
            }
        }
    });
    var data = $("form").serialize().split("&");
    var object = {};
    for (var key in data) {
        if (data.hasOwnProperty(key)) {
            object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
        }
    }
    table.render({
        elem: '#invoice',
        method: 'post',
        url: '/TrackApi/UserInfo/GetUserInvoiceListData',
        height: "300",
        where: {
            userId: object.userId,
            startTime: object.invoiceStartTime,
            endTime: object.invoiceEndTime
        },
        cols: [[
            {
                field: 'invoiceId', title: '账单编号', templet: function (d) {
                    return '<a class="layui-table-link" href="/trackapi/userinfo/invoicepdf?id=' + d.invoiceId + '" target="_blank">' + d.invoiceId + '</a>';
                }
            },
            { field: 'totalRequest', title: '请求量' },
            {
                field: 'amount', title: '金额'
            }
        ]]
    });
    $("#invoiceSearch").on("click", function () {
        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            }
        }
        table.reload('invoice',
            {
                where: {
                    userId: object.userId,
                    startTime: object.invoiceStartTime,
                    endTime: object.invoiceEndTime
                }
            });
        return true;
    });
    table.render({
        elem: '#trackCount',
        method: 'post',
        url: '/TrackApi/UserInfo/GetTrackCountListData',
        height: "300",
        where: {
            userId: object.userId,
            startTime: object.trackCountStartTime,
            endTime: object.trackCountEndTime
        },
        cols: [[
            { field: 'date', title: '日期' },
            { field: 'count', title: '请求量' }
        ]]
    });

    $("#trackCountSearch").on("click", function () {
        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            }
        }
        table.reload('trackCount',
            {
                where: {
                    userId: object.userId,
                    startTime: object.trackCountStartTime,
                    endTime: object.trackCountEndTime
                }
            });
        return true;
    });
    form.on('submit(order)', function (data) {
        window.top.addMyTab('/pay/purchaseorder/index?serviceType=4&userId=' + $('#hiUserId').val(), "查看购买记录", 'paycenter', '/pay/purchaseOrder/index');
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });
    form.on('submit(config)', function (data) {
        var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
        window.top.addMyTab('/trackapi/userinfo/viewconfig?id=' + $('#hiUserNo').val(), "查看用户配置", curmenu.topMenuName, curmenu.leftMenuName);
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });
});