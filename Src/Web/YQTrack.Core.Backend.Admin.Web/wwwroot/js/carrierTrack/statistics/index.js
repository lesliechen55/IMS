layui.use(['laydate', 'form', 'layer', 'table', 'laytpl'], function () {

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
        min: '2019-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#endTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2019-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    // 基于准备好的dom，初始化echarts实例
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

    $("#export").on("click", function () {

        var url = $(this).prop('href') + '?';

        var data = $("form").serialize().split("&");
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                url = url + data[key].split("=")[0] + '=' + decodeURIComponent(data[key].split("=")[1]);
                url = url + '&';
            }
        }
        url = url.slice(0, -1);

        $.fileDownload(url)
            .done(function (result) {
                layer.msg('文件下载成功请保存到您的电脑上面!', { icon: 1 });
            })
            .fail(function (failHtml) {
                layer.open({
                    title: '文件下载失败',
                    content: failHtml
                });
            });
        return false; //this is critical to stop the click event which will trigger a normal file download 
    });

    function loadChart() {
        demo.showLoading();
        $.post('/CarrierTrack/Statistics/TrackAnalysis', {
            chartDateType: $('#chartDateType').val(),
            email: $('#email').val(),
            enable: $('#userStatus').val()
        }, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
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
        myChart.clear();
        myChart.setOption(option);
    }

});