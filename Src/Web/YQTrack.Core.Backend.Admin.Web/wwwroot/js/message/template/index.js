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
        url: '/Message/Template/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            { field: 'language', title: '模板语言', width: 160 },
            { field: 'projectName', title: '项目名称', width: 88 },
            { field: 'channelName', title: '发送通道',width:88 },
            { field: 'templateName', title: '模板名称' },
            { field: 'templateTitle', title: '模板标题' }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            templateTypeId: objWhere.templateTypeId,
            language: objWhere.language
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    table.on('toolbar(table)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        if (checkStatus.data.length !== 1) {
            layer.msg('请先选择有且仅有一个语言模板');
            return;
        }
        var name = checkStatus.data[0].templateName;
        var id = checkStatus.data[0].templateId;
        switch (obj.event) {
            case 'delete':
                layer.confirm('确认要删除“' + checkStatus.data[0].language + '”模板吗？', { icon: 3, title: '提示' }, function (index) {
                    $.get('/Message/Template/Delete', { id: id }, function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.msg('操作成功');
                        $("#search").trigger('click');
                        layer.close(index);
                        return false;
                    });
                });
                break;
            case 'preview':
                //$.get('/Message/Template/Preview', { id: id }, function (res) {
                //    if (!res.success) {
                //        layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                //        return false;
                //    }

                layer.open({
                    type: 2,
                    title: "预览 " + name,
                    content: '/Message/TemplateType/TemplatePreview?id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['900px', '820px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true,
                    btn: ['测试发送', '关闭'],
                    yes: function (index, layero) {
                        //按钮【按钮一】的回调
                        layer.open({
                            type: 2,
                            title: '发送 ' + name,
                            content: '/Message/SendTask/SendTemplateTest?id=' + id + '&templateName=' + name + '&channel=' + checkStatus.data[0].channel,
                            skin: 'layui-layer-lan',
                            area: ['700px', '420px'],
                            offset: 'auto',
                            shade: 0.3,
                            maxmin: true
                        });
                    },
                    btn2: function (index, layero) {
                        //按钮【按钮二】的回调

                        //return false 开启该代码可禁止点击该按钮关闭
                    },
                    cancel: function () {
                        //右上角关闭回调

                        //return false 开启该代码可禁止点击该按钮关闭
                    }
                });
                return false;
                //});
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
                    templateTypeId: object.templateTypeId,
                    language: object.language
                }
            });
        return true;
    });

});