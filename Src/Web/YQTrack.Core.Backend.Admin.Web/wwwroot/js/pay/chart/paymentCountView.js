layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var formSelects = layui.formSelects;

    top.initFilter(window, formSelects);
    // 基于准备好的dom，初始化echarts实例
    var paymentChart = echarts.init(document.getElementById('paymentChart'), 'shine');

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

    loadChart();

    $("#search").on("click", function () {
        if ($("#paymentChart>div").length > 1) {
            $("#paymentChart>div").eq(2).css("display", "none");
        }
        loadChart();
        top.changeUrlParam(window);
    });

    function loadChart() {
        paymentChart.showLoading();
        $.post('/Pay/Chart/PaymentCountAnalysis', {
            paymentAnalysisType: $('#paymentAnalysisType').val(),
            chartDateType: $('#chartDateType').val(),
            email: $('#email').val(),
            paymentProvider: formSelects.value('paymentProviderSel', 'val'),
            serviceType: formSelects.value('serviceTypeSel', 'val'),
            currencyType: $('#currencyTypeSel').val(),
            platformType: formSelects.value('platformTypeSel', 'val'),
            paymentStatus: formSelects.value('paymentStatusSel', 'val')
        }, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return;
            }
            let data = res.data;

            var chartOption = $.extend({}, commonOption, {
                title: {
                    text: data.title
                },
                legend: {
                    data: $.map(data.series, function (e) {
                        return e.name;
                    })
                },
                xAxis: {
                    data: data.xAxisData
                },
                series: data.series
            });

            paymentChart.hideLoading();
            reloadChat(paymentChart, chartOption);

        });
    }

    function reloadChat(myChart, option) {
        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option, true);
    }

});