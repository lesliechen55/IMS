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
        elem: '#publishStartTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#publishEndTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#expireStartTime', //指定元素
        type: 'date',
        format: 'yyyy-MM-dd',
        min: '2018-1-1',
        max: '2025-12-31',
        lang: 'cn',
        theme: 'molv',
        calendar: true
    });

    laydate.render({
        elem: '#expireEndTime', //指定元素
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
        url: '/Freight/Channel/GetChannelPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'channelTitle', title: '渠道名称' },
            { field: 'channelId', title: '渠道ID' },
            { field: 'productType', title: '产品类型' },
            { field: 'minDay', title: '最小天数' },
            { field: 'maxDay', title: '最大天数' },
            { field: 'citys', title: '揽件城市' },
            { field: 'countrys', title: '派送国家' },
            { field: 'limitWeight', title: '限重(g)' },
            { field: 'operateCost', title: '操作费' },
            { field: 'firstWeight', title: '首重(g)' },
            { field: 'firstPrice', title: '首重价格' },
            { field: 'freightType', title: '报价类型' },
            { field: 'freightIntervals', title: '报价区间' },
            { field: 'companyName', title: '运输商' },
            { field: 'publishTime', title: '发布时间' },
            { field: 'state', title: '渠道状态' },
            { field: 'expireTime', title: '过期时间' },
            { field: 'validReportTimes', title: '有效举报次数' }
        ]],
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
        },
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            id: objWhere.id,
            name: objWhere.name,
            publishStartTime: objWhere.publishStartTime,
            publishEndTime: objWhere.publishEndTime,
            expireStartTime: objWhere.expireStartTime,
            expireEndTime: objWhere.expireEndTime,
            status: objWhere.status
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
            }
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
                    id: object.id,
                    name: object.name,
                    publishStartTime: object.publishStartTime,
                    publishEndTime: object.publishEndTime,
                    expireStartTime: object.expireStartTime,
                    expireEndTime: object.expireEndTime,
                    status: object.status
                }
            });
        return true;
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

});