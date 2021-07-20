layui.config({
    base: '/module/'
}).extend({
    treeGrid: 'authtree'
}).use(['layer', 'authtree', 'jquery', 'form', 'code', 'laytpl'], function () {

    var form = layui.form,
        $ = layui.jquery;
    var layer = parent.layer === undefined ? layui.layer : top.layer;
    var laytpl = layui.laytpl;
    var authtree = layui.authtree;

    // 自定义表单验证
    form.verify({
        fullName: function (value, item) {
            if ($('#multiPower').prop('checked') && value == '') {
                return '请输入唯一的权限代码';
            }
        }
    });
    $('input[name=fullName]').focus(function () {
        if (!$('#multiPower').prop('checked')) {
            var areaName = $('input[name=areaName]').val();
            var controllerName = $('input[name=controllerName]').val();
            var actionName = $('input[name=actionName]').val();
            $(this).val(areaName + '_' + controllerName + '_' + actionName);
        }
    });
    $('input[name=url]').focus(function () {
        var areaName = $('input[name=areaName]').val();
        var controllerName = $('input[name=controllerName]').val();
        var actionName = $('input[name=actionName]').val();
        var permissionCode = '';
        if ($('#multiPower').prop('checked')) {
            permissionCode = '?permissionCode=' + $('input[name=fullName]').val();
        }
        $(this).val('/' + areaName + '/' + controllerName + '/' + actionName + permissionCode);
    });

    $.ajax({
        url: '/Admin/Permission/GetAll',
        method: 'get',
        dataType: 'json',
        success: function (res) {
            var sourceParentId = parseInt($('#parentId').val());
            console.log(sourceParentId);
            var trees = authtree.listConvert(res.data, {
                primaryKey: 'id'
                , startPid: -1
                , parentKey: 'parentId'
                , nameKey: 'name'
                , valueKey: 'id'
                , checkedKey: [sourceParentId]
                , disabledKey: []
            });
            console.log(trees);
            var selectList = authtree.treeConvertSelect(trees, {
                childKey: 'list'
            });
            console.log(selectList);
            // 渲染单选框
            var string = laytpl($('#LAY-auth-tree-convert-select').html()).render({
                // 为了 layFilter 的唯一性，使用模板渲染的方式传递
                layFilter: 'LAY-auth-tree-convert-select-input',
                list: selectList,
                code: JSON.stringify(res, null, 2)
            });
            $('#LAY-auth-tree-convert-select-dom').html(string);
            form.render('select');
            // 使用form.on('select(LAY-FILTER)')监听选中
            form.on('select(LAY-auth-tree-convert-select-input)', function (data) {
                console.log('选中信息', data);
            });
        }
    });

    var menuType = parseInt($('#menuType').val());

    $('select').val(menuType);

    form.on('submit(formEdit)', function (data) {
        if (data.field.parentId === '-1') {
            data.field.parentId = '';
        }
        $.post('/Admin/Permission/Edit', data.field, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功', { icon: 6 });
            // 刷新父级页面
            top.refreshParent();
            $('#close').click();
            return false;
        });
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });

    $('#close').click(function () {
        //当你在iframe页面关闭自身时
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭
    });

});