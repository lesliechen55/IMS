layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var formSelects = layui.formSelects;

    top.initFilter(window, formSelects);
    // 基于准备好的dom，初始化echarts实例
    var orderChart = echarts.init(document.getElementById('orderChart'), 'shine');
    //获取Y坐标名称（加币种显示）
    function getY() {
        var yName = '金额';
        //如果不是按照币种统计
        if ($('#orderAnalysisType').val() != 3) {
            if ($('#currencyTypeSel').val() == 2) {
                if ($('#exchangeRate').val() == '') {
                    yName = '金额($)';
                }
                else {
                    yName = '金额(￥)';
                }
            }
            else if ($('#currencyTypeSel').val() == 1) {
                yName = '金额(￥)';
            }
            else {
                if ($('#exchangeRate').val() != '') {
                    yName = '金额(￥)';
                }
            }
        }
        return yName;
    }
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
                name: getY(),
                min: 0
            }
        ],
        series: []
    };
    loadChart();

    $("#search").on("click", function () {
        //如果数据视图是打开状态，关闭数据视图
        if ($("#orderChart>div").length > 1) {
            $("#orderChart>div").eq(2).css("display", "none");
        }
        loadChart();
        top.changeUrlParam(window);
    });

    function loadChart() {
        orderChart.showLoading();
        $.post('/Pay/Chart/OrderAmountAnalysis',
            {
                orderAnalysisType: $('#orderAnalysisType').val(),
                chartDateType: $('#chartDateType').val(),
                email: $('#email').val(),
                serviceType: formSelects.value('serviceTypeSel', 'val'),
                currencyType: $('#currencyTypeSel').val(),
                exchangeRate: $('#exchangeRate').val(),
                platformType: formSelects.value('platformTypeSel', 'val'),
                orderStatus: formSelects.value('orderStatusSel', 'val')
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
                    yAxis: [
                        {
                            type: 'value',
                            name: getY(),
                            min: 0
                        }
                    ],
                    series: data.series
                });

                orderChart.hideLoading();
                reloadChat(orderChart, chartOption);
            });
    }

    function reloadChat(myChart, option) {
        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option, true);
    }

});