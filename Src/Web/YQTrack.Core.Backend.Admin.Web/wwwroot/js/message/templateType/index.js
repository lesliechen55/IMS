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
        url: '/Message/TemplateType/GetPageData',
        height: "full-" + objWhere.top,
        cellMinWidth: 60,
        toolbar: '#toolbar',
        defaultToolbar: ['filter', 'exports', 'print'],
        cols: [[
            { type: 'numbers' },
            { type: 'radio' },
            //{ field: 'templateTypeId', title: '编号' },
            { field: 'templateNo', title: '模板编号', width: 260 },
            { field: 'templateCode', title: 'Code', width: 80 },
            { field: 'channelName', title: '发送通道', width: 88 },
            { field: 'templateName', title: '模板名称' },
            { field: 'templateDescribe', title: '模板描述' },
            { field: 'createTime', title: '创建时间', width: 160 },
            { field: 'updateTime', title: '更新时间', width: 160 }
        ]],
        page: {
            limit: objWhere.limit ? objWhere.limit : 10,
            limits: [10, 15, 20, 25],
            curr: objWhere.page ? objWhere.page : 1
        },
        where: {
            projectId: objWhere.projectId,
            channelId: objWhere.channelId,
            templateName: objWhere.templateName
        },
        done: function (res, curr, count) {
            $('#page').val(curr);
            $('#limit').val(this.limit);
            top.changeUrlParam(window);
        }
    });

    table.on('toolbar(table)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'viewTemplate':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个基础模板');
                    return;
                }
                var name = checkStatus.data[0].templateName;
                var id = checkStatus.data[0].templateTypeId;
                var curmenu = JSON.parse(window.top.sessionStorage.getItem("curmenu"));
                window.top.addMyTab('/message/template/index?templateTypeId=' + id, '语言模板-' + name, curmenu.topMenuName, curmenu.leftMenuName);
                break;
            case 'add':
                layer.open({
                    type: 2,
                    title: '添加基础模板',
                    content: '/Message/TemplateType/Add?iframeName=' + window.name,
                    skin: 'layui-layer-lan',
                    area: ['900px', '820px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
            case 'update':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个基础模板');
                    return;
                }
                var name = checkStatus.data[0].templateName;
                var id = checkStatus.data[0].templateTypeId;
                layer.open({
                    type: 2,
                    title: '修改 ' + name,
                    content: '/Message/TemplateType/Edit?iframeName=' + window.name + '&id=' + id,
                    skin: 'layui-layer-lan',
                    area: ['900px', '820px'],
                    offset: 'auto',
                    shade: 0.3,
                    maxmin: true
                });
                break;
            case 'preview':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个基础模板');
                    return;
                }
                var name = checkStatus.data[0].templateName;
                var id = checkStatus.data[0].templateTypeId;
                var code = checkStatus.data[0].templateCode;
                //$.get('/Message/TemplateType/Preview', { id: id }, function (res) {
                //    if (!res.success) {
                //        layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                //        return false;
                //    }
                if (code == 999999) {
                    layer.open({
                        type: 2,
                        title: "预览 " + name,
                        content: '/Message/TemplateType/Preview?id=' + id,
                        skin: 'layui-layer-lan',
                        area: ['900px', '820px'],
                        offset: 'auto',
                        shade: 0.3,
                        maxmin: true,
                        btn: ['发送', '关闭'],
                        yes: function (index, layero) {
                            //按钮【按钮一】的回调
                            layer.open({
                                type: 2,
                                title: '发送 ' + name,
                                content: '/Message/SendTask/Add?id=' + id + '&templateName=' + name + '&channel=' + checkStatus.data[0].channel,
                                skin: 'layui-layer-lan',
                                area: ['700px', '520px'],
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
                }
                else {
                    layer.open({
                        type: 2,
                        title: "预览 " + name,
                        content: '/Message/TemplateType/Preview?id=' + id,
                        skin: 'layui-layer-lan',
                        area: ['900px', '820px'],
                        offset: 'auto',
                        shade: 0.3,
                        maxmin: true
                    });
                }

                return false;
                //});
                break;
            case 'export':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个基础模板');
                    return;
                }
                var title = checkStatus.data[0].templateName;
                var id = checkStatus.data[0].templateTypeId;
                layer.load(3);
                var url = $(this).attr('href') + '?id=' + id;
                $.fileDownload(url, {
                    httpMethod: 'get',
                    //data: { id: id },//$("#exportForm").serialize()
                    successCallback: function (url) {
                        layer.closeAll();
                        layer.msg('文件下载成功请保存到您的电脑上面!', { icon: 1 });
                    },
                    failCallback: function (failHtml, url) {
                        layer.closeAll();
                        failHtml = JSON.parse(failHtml.replace(/<[^>]+>/g, ""));
                        layer.alert('操作失败:' + failHtml.msg, { title: '文件下载失败', icon: 5 });
                        //layer.open({
                        //    title: '文件下载失败',
                        //    content: failHtml,
                        //});
                    }
                });
                //$.fileDownload(url)
                //    .done(function (result) {
                //        //layer.closeAll('loading');
                //        layer.msg('文件下载成功请保存到您的电脑上面!', { icon: 1 });
                //    })
                //    .fail(function (failHtml) {
                //        //layer.closeAll('loading');
                //        layer.open({
                //            title: '文件下载失败',
                //            content: failHtml,
                //        });
                //    });
                return false; //this is critical to stop the click event which will trigger a normal file download 
                break;
            case 'import':
                //执行实例
                layer.open({
                    type: 2,
                    title: '导入语言词条',
                    content: '/Message/TemplateType/Import',
                    skin: 'layui-layer-lan',
                    area: ['500px', '420px'],
                    offset: 'auto',
                    shade: 0.3
                });
                break;

            case 'updateTemplate':
                if (checkStatus.data.length !== 1) {
                    layer.msg('请先选择有且仅有一个基础模板');
                    return;
                }
                var title = checkStatus.data[0].templateName;
                var id = checkStatus.data[0].templateTypeId;
                layer.confirm('确认要批量更新“' + title + '”的语言模板吗？', { icon: 3, title: '提示' }, function (index) {
                    $.get('/Message/TemplateType/UpdateTemplate', { id: id }, function (res) {
                        if (!res.success) {
                            layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                            return false;
                        }
                        layer.msg('操作成功');
                        layer.close(index);
                        return false;
                    });
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
                    projectId: object.projectId,
                    channelId: object.channelId,
                    templateName: object.templateName
                }
            });
        return true;
    });

});