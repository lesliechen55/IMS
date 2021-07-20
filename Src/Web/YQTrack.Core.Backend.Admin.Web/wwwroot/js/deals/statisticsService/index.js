layui.use(['element', 'laydate', 'form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var laydate = layui.laydate;

    var element = layui.element;

    var startDateTime = new Date();
    startDateTime = startDateTime.setDate(startDateTime.getDate() - 90);
    startDateTime = new Date(startDateTime);

    //执行一个laydate实例
    laydate.render({
        elem: '#startTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2015-1-1',
        max: '2050-12-31',
        lang: 'cn',
        theme: 'molv',
        value: startDateTime,
        calendar: true,
        done: function (value, date, endDate) {
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
    laydate.render({
        elem: '#endTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2015-1-1',
        max: '2050-12-31',
        lang: 'cn',
        theme: 'molv',
        value: new Date(),
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

    // 基于准备好的dom，初始化echarts实例
    var statisticsAllChartFromClickRate = echarts.init(document.getElementById('statisticsAllChartFromClickRate'), 'shine');
    var statisticsAllChartFromTransactionRate = echarts.init(document.getElementById('statisticsAllChartFromTransactionRate'), 'shine');
    var statisticsAllChartFromPaymentRate = echarts.init(document.getElementById('statisticsAllChartFromPaymentRate'), 'shine');
    var statisticsAllChartFromConversionRate = echarts.init(document.getElementById('statisticsAllChartFromConversionRate'), 'shine');
    var statisticsAllChartFromECPCRate = echarts.init(document.getElementById('statisticsAllChartFromECPCRate'), 'shine');
    var statisticsAllChartFromAllRate = echarts.init(document.getElementById('statisticsAllChartFromAllRate'), 'shine');


    // 指定图表的配置项和数据
    var commonOption = {
        title: {
            text: ''
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'cross',
                crossStyle: {
                    color: '#999'
                }
            }
        },
        toolbox: {
            feature: {
                dataView: { show: true, readOnly: false },
                magicType: { show: true, type: ['line', 'bar'] },
                restore: { show: true },
                saveAsImage: { show: true }
            }
        },
        legend: {
            data: []
        },
        xAxis: [
            {
                type: 'category',
                data: [],
                axisPointer: {
                    type: 'shadow'
                }
            }
        ],
        yAxis: [
            {
                type: 'value',
                name: '数量',
                min: 0
            }
        ],
        series: []
    };

    table.render({
        elem: '#tableStatisticsAll',
        method: 'post',
        url: '/Deals/StatisticsService/GetStatisticsContrastData',
        page: false,
        cellMinWidth: 60,
        cols: [[
            { type: 'numbers' },
            {
                field: 'clickCount', title: '点击',
                templet: function (d) { return getColor(d.clickCount, d.clickRate); }
            },
            {
                field: 'transactionCount', title: '交易',
                templet: function (d) { return getColor(d.transactionCount, d.transactionRate); }
            },
            {
                field: 'paymentCount', title: '佣金',
                templet: function (d) { return getColor(d.paymentCount, d.paymentRate); }
            },
            {
                field: 'conversion', title: '转化率',
                templet: function (d) { return getColor(d.conversion, d.conversionRate); }
            },
            {
                field: 'ecpc', title: 'ECPC',
                templet: function (d) { return getColor(d.ecpc, d.ecpcRate); }
            },
            { field: 'statisticsDate', title: '统计时间' }
        ]],
        where: {
            startTime: $('#startTime').val(),
            endTime: $('#endTime').val()
        }
    });

    table.render({
        elem: '#tableStartMer',
        method: 'post',
        url: '/Deals/StatisticsService/GetStatisticsMerStartData',
        page: false,
        cellMinWidth: 60,
        cols: [[
            { type: 'numbers' },
            { field: 'statisticsDate', title: '统计时间' },
            { field: 'prioritys', title: '顺序', width: 70 },
            { field: 'yqMerchantLibraryId', title: '广告商' },
            { field: 'name', title: '广告商名称' },
            { field: 'clickCount', title: '点击', width: 70 },
            { field: 'transactionCount', title: '转化', width: 70 },
            { field: 'paymentCount', title: '佣金', width: 70 },
            { field: 'conversion', title: '转化率', width: 80 },
            { field: 'ecpc', title: 'ECPC', width: 70 }
        ]],
        where: {
            startTime: $('#startTime').val(),
            endTime: $('#endTime').val()
        }
    });

    table.render({
        elem: '#tableEndMer',
        method: 'post',
        url: '/Deals/StatisticsService/GetStatisticsMerEndData',
        page: false,
        cellMinWidth: 60,
        cols: [[
            { type: 'numbers' },
            { field: 'statisticsDate', title: '统计时间' },
            { field: 'prioritys', title: '顺序', width: 70 },
            { field: 'yqMerchantLibraryId', title: '广告商' },
            { field: 'name', title: '广告商名称' },
            { field: 'clickCount', title: '点击', width: 70 },
            { field: 'transactionCount', title: '转化', width: 70 },
            { field: 'paymentCount', title: '佣金', width: 70 },
            { field: 'conversion', title: '转化率', width: 80 },
            { field: 'ecpc', title: 'ECPC', width: 70 }
        ]],
        where: {
            startTime: $('#startTime').val(),
            endTime: $('#endTime').val()
        }
    });


    loadChart();

    $("#search").on("click", function () {

        loadChart();

        table.reload('tableStatisticsAll',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    startTime: $('#startTime').val(),
                    endTime: $('#endTime').val()
                }
            });

        table.reload('tableStartMer',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    startTime: $('#startTime').val(),
                    endTime: $('#endTime').val()
                }
            });

        table.reload('tableEndMer',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    startTime: $('#startTime').val(),
                    endTime: $('#endTime').val()
                }
            });
        return true;
    });

    function getColor(d1, d2) {
        if (d2 < 0) {
            return d1 + '(' + '<span style="color: #00cc00;">' + d2 + '%</span>) ';
        }
        else {
            return d1 + '(' + '<span style="color: #cc0000;">' + d2 + '%</span>) ';
        }
    }


    function loadChart() {
        statisticsAllChartFromClickRate.showLoading();
        $.post('/Deals/StatisticsService/GetStatisticsListData', {
            startTime: $('#startTime').val(),
            endTime: $('#endTime').val()
        }, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return;
            }
            let data = res.data;

            var statisticsAllChartFromClickRateOption = $.extend({}, commonOption, {
                title: {
                    text: data.clickRate.title
                },
                legend: {
                    data: $.map(data.clickRate.series, function (e) {
                        return e.name;
                    })
                },
                xAxis: {
                    data: data.clickRate.xAxisData
                },
                series: data.clickRate.series
            });

            var statisticsAllChartFromTransactionRateOption = $.extend({}, commonOption, {
                title: {
                    text: data.transactionRate.title
                },
                legend: {
                    data: $.map(data.transactionRate.series, function (e) {
                        return e.name;
                    })
                },
                xAxis: {
                    data: data.transactionRate.xAxisData
                },
                series: data.transactionRate.series
            });

            var statisticsAllChartFromPaymentRateOption = $.extend({}, commonOption, {
                title: {
                    text: data.paymentRate.title
                },
                legend: {
                    data: $.map(data.paymentRate.series, function (e) {
                        return e.name;
                    })
                },
                xAxis: {
                    data: data.paymentRate.xAxisData
                },
                series: data.paymentRate.series
            });

            var statisticsAllChartFromConversionRateOption = $.extend({}, commonOption, {
                title: {
                    text: data.conversionRate.title
                },
                legend: {
                    data: $.map(data.conversionRate.series, function (e) {
                        return e.name;
                    })
                },
                xAxis: {
                    data: data.conversionRate.xAxisData
                },
                series: data.conversionRate.series
            });

            var statisticsAllChartFromECPCRateOption = $.extend({}, commonOption, {
                title: {
                    text: data.ecpcRate.title
                },
                legend: {
                    data: $.map(data.ecpcRate.series, function (e) {
                        return e.name;
                    })
                },
                xAxis: {
                    data: data.ecpcRate.xAxisData
                },
                series: data.ecpcRate.series
            });

            var statisticsAllChartFromAllRateOption = $.extend({}, commonOption, {
                title: {
                    text: data.allRate.title
                },
                legend: {
                    data: $.map(data.allRate.series, function (e) {
                        return e.name;
                    })
                },
                xAxis: {
                    data: data.allRate.xAxisData
                },
                series: data.allRate.series
            });

            statisticsAllChartFromClickRate.hideLoading();
            statisticsAllChartFromTransactionRate.hideLoading();
            statisticsAllChartFromPaymentRate.hideLoading();
            statisticsAllChartFromConversionRate.hideLoading();
            statisticsAllChartFromECPCRate.hideLoading();
            statisticsAllChartFromAllRate.hideLoading();

            reloadChat(statisticsAllChartFromClickRate, statisticsAllChartFromClickRateOption);
            reloadChat(statisticsAllChartFromTransactionRate, statisticsAllChartFromTransactionRateOption);
            reloadChat(statisticsAllChartFromPaymentRate, statisticsAllChartFromPaymentRateOption);
            reloadChat(statisticsAllChartFromConversionRate, statisticsAllChartFromConversionRateOption);
            reloadChat(statisticsAllChartFromECPCRate, statisticsAllChartFromECPCRateOption);
            reloadChat(statisticsAllChartFromAllRate, statisticsAllChartFromAllRateOption);

        });
    }

    function reloadChat(myChart, option) {
        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
    }

});