// 注册扩展方法
Date.prototype.format = function (format) {
    var date = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S+": this.getMilliseconds()
    };
    if (/(y+)/i.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
    }
    for (var k in date) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
                ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
        }
    }
    return format;
}

layui.use(['form', 'layer', 'table', 'laytpl', 'laydate'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var laydate = layui.laydate;
    //日期范围
    laydate.render({
        elem: '#submitDateRange'
        , range: '~'
        , type: 'date'
        , min: -30 //30天前
        , max: 1 //1天后
        , theme: 'molv'
        , calendar: true
    });

    // 获取一周的时间范围
    function getLastWeek() {
        let today = new Date();
        let lastWeek = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 7);
        return lastWeek.format('yyyy-MM-dd') + ' ~ ' + today.format('yyyy-MM-dd');
    }

    var objWhere = top.initFilter(window);

    // 如果时间范围控件没有值则设置默认值一周
    if ($('#submitDateRange').val().trim() === '') {
        let week = getLastWeek();
        $('#submitDateRange').val(week);
    }

    var tableIns = table.render({
        elem: '#table',
        method: 'post',
        url: null,
        data: [],
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { field: 'senderRecordId', title: '邮件主键', hide: true },
            { field: 'providerType', title: '发送渠道', width: 150 },
            { field: 'title', title: '邮件标题' },
            {
                field: 'typeInfo', title: '发送模板', width: 200, templet: function (d) {
                    if (d.businessEmailType === 'None') {
                        return d.messageEmailType;
                    }
                    return d.businessEmailType;
                }
            },
            { field: 'from', title: '发件人', width: 300 },
            { field: 'to', title: '收件人', width: 250 },
            { field: 'messageId', title: '邮件提交标识', hide: true },
            {
                field: 'submitInfo', title: '提交信息', width: 160, templet: function (d) {
                    // 非成功提交显示橘色
                    if (d.submitFailureStatus !== '成功') {
                        return d.submitTime + '<br/>' + '<p style="color:orange">' + d.submitFailureStatus + '</p>';
                    }
                    return d.submitTime + '<br/>' + d.submitFailureStatus;
                }
            },
            {
                field: 'deliveryInfo', title: '投递信息', width: 160, templet: function (d) {
                    if (d.deliveryFailureStatus === '' && d.deliveryReportedTime === null) {
                        return '';
                    }
                    let html = '';
                    // 有投递结果且非成功投递显示橘色且hint方式展示投递失败的错误原因
                    if (d.deliveryFailureStatus !== "成功" && d.deliveryFailureStatus !== "") {
                        html = (d.deliveryReportedTime === null ? '' : d.deliveryReportedTime) +
                            '<br/>' +
                            '<p title="' + d.deliveryFailureMessage + '" style="color:orange">' +
                            d.deliveryFailureStatus +
                            '</p>';
                    } else {
                        html = (d.deliveryReportedTime === null ? '' : d.deliveryReportedTime) +
                            '<br/>' + '<p>' +
                            d.deliveryFailureStatus + '</p>';
                    }
                    // 判断是否展示[投递详情]按钮
                    if (d.deliveryFailureStatus !== "" && d.messageId !== "") {
                        html += '<a class="layui-table-link" lay-event="detail">投递详情</a>';
                    }
                    return html;
                }
            },
            { field: 'deliveryFailureMessage', title: '投递信息报告', hide: true },
            {
                field: 'businessInfo', title: '业务信息', width: 230, templet: function (d) {
                    // 如果没有通知业务系统且邮件类型不是None显示警告表示该数据需要通知业务系统
                    if (d.businessNotifyConfirmed === "否" && d.businessEmailType !== "None") {
                        return '业务状态: ' + d.businessEmailFailureStatus + '<br/>' + '<p style="color:orange">业务确认: ' + d.businessNotifyConfirmed + '<p>' + '确认时间: ' + (d.businessNotifyConfirmTime === null ? '' : d.businessNotifyConfirmTime);
                    }

                    // 如果是不需要通知业务系统显示'不需确认'
                    if (d.businessNotifyConfirmed === "否" && d.businessEmailType === "None") {
                        return '业务状态: ' + d.businessEmailFailureStatus + '<br/>' + '业务确认: ' + '不需确认' + '<br/>' + '确认时间: ' + (d.businessNotifyConfirmTime === null ? '' : d.businessNotifyConfirmTime);
                    }

                    return '业务状态: ' + d.businessEmailFailureStatus + '<br/>' + '业务确认: ' + d.businessNotifyConfirmed + '<br/>' + '确认时间: ' + (d.businessNotifyConfirmTime === null ? '' : d.businessNotifyConfirmTime);
                }
            }
        ]],
        parseData: function (res) {
            if (!res.success) {
                return {
                    "code": 0, //解析接口状态
                    "msg": res.msg, //解析提示文本
                    "count": 0, //解析数据长度
                    "data": [], //解析数据列表
                    "success": false
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
            submitDateRange: objWhere.submitDateRange,
            to: objWhere.to,
            from: objWhere.from
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
            if (res && res.success !== undefined && !res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
            }
        }
    });

    table.on('tool(table)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        let messageId = obj.data.messageId;
        switch (obj.event) {
            case 'detail':
                layer.open({
                    type: 2,
                    title: '投递详情 ' + messageId,
                    content: '/Business/Email/Detail?messageId=' + messageId,
                    skin: 'layui-layer-lan',
                    area: ['800px', '580px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;
        }
    });

    $("#search").on("click", function () {

        if ($.trim($('#to').val()).length <= 0 || $.trim($('#submitDateRange').val()).length <= 0) {
            layer.alert('操作失败:搜索参数必须包含提交时间段和收件人邮箱', { title: '错误', icon: 5 });
            return false;
        }

        var data = $("form").serialize().split("&");
        var object = {};
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                object[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            }
        }

        object.submitDateRange = object.submitDateRange.replace(/\+/g, '');

        tableIns.config.url = '/Business/Email/GetEmailPageData';
        table.reload('table',
            {
                page: {
                    curr: 1 // 重新从第 1 页开始
                },
                where: {
                    submitDateRange: object.submitDateRange,
                    to: object.to,
                    from: object.from
                }
            });
        return true;
    });

    // 首次页面有条件则加载数据
    if ($.trim($('#submitDateRange').val()).length > 0 &&
        $.trim($('#to').val()).length > 0) {
        $("#search").click();
    }

});