layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    var objWhere = top.initFilter(window);

    table.render({
        elem: '#table',
        method: 'post',
        url: '/Business/User/GetUserFeedbackPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'checkbox' },
            { field: 'feedbackId', title: '反馈ID', hide: true },
            { field: 'createTime', title: '反馈时间', width: 160 },
            { field: 'userId', title: '用户ID', width: 180 },
            { field: 'email', title: '邮箱' },
            { field: 'feedback', title: '内容' },
            { field: 'state', title: '状态', width: 80 },
            { field: 'replyTime', title: '回复时间', width: 160 },
            { field: 'replyContent', title: '回复内容' },
            { field: 'replyUserId', title: '回复人', width: 180 }
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
            userId: objWhere.userId,
            email: objWhere.email,
            content: objWhere.content
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

    table.on('toolbar(table)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        if (checkStatus.data.length < 1) {
            layer.msg('请先选择需要回复的反馈意见');
            return;
        }
        var ids = [];
        var hasHandled = false;
        checkStatus.data.forEach(function (item, index) {
            if (item.state == '已回复') {
                hasHandled = true;
                return;
            }
            else {
                ids.push(item.feedbackId);
            }
        })
        if (hasHandled) {
            layer.msg('选中的项包含已经处理过的反馈信息！');
            return;
        }
        switch (obj.event) {
            case 'reply':
                layer.open({
                    type: 2,
                    title: "回复用户反馈信息：",
                    content: '/Business/User/Reply?iframeName=' + window.name + '&feedBackIds=' + ids.join(','),
                    skin: 'layui-layer-lan',
                    area: ['500px', '300px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
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
                    userId: object.userId,
                    email: object.email,
                    content: object.content
                }
            });
        return true;
    });


});