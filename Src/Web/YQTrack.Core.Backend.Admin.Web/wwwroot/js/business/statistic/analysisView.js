layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var objWhere = top.initFilter(window);

    // 基于准备好的dom，初始化echarts实例
    var userChart = echarts.init(document.getElementById('main'), 'shine');

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
        //如果数据视图是打开状态，关闭数据视图
        if ($("#main>div").length > 1) {
            $("#main>div").eq(2).css("display", "none");
        }
        loadChart();
        top.changeUrlParam(window);
    });

    function loadChart() {
        userChart.showLoading();
        $.post('/Business/Statistic/GetAnalysisData',
            {
                analysisType: $('#analysisType').val(),
                chartDateType: $('#chartDateType').val()
            }, function (res) {
                if (!res.success) {
                    layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                    return false;
                }
                let data = res.data;

                var userOption = $.extend({}, commonOption, {
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

                userChart.hideLoading();
                userChart.setOption(userOption);

                return true;
            });
    }
});