layui.config({
    base: '/module/'
}).extend({
    autocomplete: 'autoComplete/autocomplete'
}).use(['form', 'layer', 'table', 'laytpl', 'autocomplete'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table,
        autocomplete = layui.autocomplete;

    var objWhere = top.initFilter(window);
    autocomplete.render({
        elem: $('#userName')[0],
        url: '/TrackApi/Analyze/GetAutoCompleteInfo',
        cache: false,
        template_val: '{{d.userName}}',
        template_txt: '{{d.userName}} <span class=\'layui-badge layui-bg-gray\'>{{d.email}}</span>',
        onselect: function (resp) {
        }
    });

    var demo = echarts.init(document.getElementById('demo'), 'shine');

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
        if ($("#demo>div").length > 1) {
            $("#demo>div").eq(2).css("display", "none");
        }
        loadChart();
        top.changeUrlParam(window);
    });

    function loadChart() {
        demo.showLoading();
        $.post('/TrackApi/Analyze/RegisterAnalyze', {
            chartDateType: $('#chartDateType').val(),
            userNo: $('#userNo').val(),
            userName: $('#userName').val()
        }, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                demo.hideLoading();
                return;
            }

            let data = res.data;

            var demoOption = $.extend({}, commonOption, {
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

            demo.hideLoading();

            reloadChat(demo, demoOption);

        });
    }

    function reloadChat(myChart, option) {
        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option, true);
    }

});