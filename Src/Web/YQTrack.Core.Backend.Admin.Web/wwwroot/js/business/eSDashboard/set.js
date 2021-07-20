layui.config({
    base: '/module/'
}).extend({
    xmSelect: 'xm-select'
}).use(['layer', 'jquery', 'form', 'xmSelect', 'laydate'], function () {

    var form = layui.form,
        $ = layui.jquery;
    var layer = parent.layer === undefined ? layui.layer : top.layer;

    var xmSelect = layui.xmSelect;
    initFieldData();
    //解析src
    $('#dashboardSrc').blur(function () {
        initFieldData();
    });
    function initFieldData() {
        var src = $('#dashboardSrc').val();
        if (src == '') {
            return;
        }
        //解码
        src = decodeURIComponent(src);
        var eSFields = [];
        //正则取字段
        var reg = /_a=\(filters:!\(([\s\S]*?)\),linked:/;
        reg.test(src);
        var src1 = RegExp.$1;
        if (src1 != '') {
            var reg1 = /\('\$state':\(store:appState\),[\s\S]*?\)(?:,query|,range)/g;
            var res = src1.match(reg1);

            var regKey = /key:'?([\s\S]*?)'?,/;
            var regValue = /value:'?([\s\S]*?)'?\)/;
            var regValue1 = /params:\(query:!([\s\S]*?)\)/;
            var regDisabled = /disabled:!([\s\S]*?),/;
            res.forEach(function (item, index) {
                var field = {};
                field.category = '';
                field.type = -1;//类型不能确定，需手动配置
                field.fieldName = regKey.exec(item)[1];
                field.isValue = false;
                field.required = false;
                //disabled为!f则有默认值
                var disable = regDisabled.exec(item)[1];
                if (disable == 'f') {
                    var val = regValue.exec(item);
                    if (val) {
                        val = val[1];
                    }
                    else {
                        val = regValue1.exec(item)[1];
                    }
                    field.defaultValue = val.replace('-∞', '').replace('+∞', '').replace('-Infinity', '').replace('Infinity', '').split('+to+').join(',');
                }
                else {
                    field.defaultValue = '';
                }
                field = bindSavedData(field);
                eSFields.push(field);
            })
        }
        //全文搜索字段 
        var regFulltext = /linked:!(?:f|t),query:\(query_string:\((?:analyze_wildcard:!t,)?query:'([\s\S]*?)'\)\)/
        if (regFulltext.test(src)) {
            var fullFieldText = regFulltext.exec(src)[1];
            if (fullFieldText) {
                var arrFullField = fullFieldText.split(' ');
                arrFullField.forEach(function (item, index) {
                    var fulltext = { id: 0, type: 3 };
                    var arrFull = item.split(':');
                    if (arrFull.length > 1) {
                        fulltext.fieldName = arrFull[0];
                        fulltext.defaultValue = arrFull[1];

                        fulltext = bindSavedData(fulltext);
                        eSFields.push(fulltext);
                    }
                });
            }
        }
        renderFieldsTable(eSFields);
    }
    //绑定已存储信息
    function bindSavedData(field) {
        if ($('#hiFields').val() != '') {
            var jsonFields = JSON.parse($('#hiFields').val());
            for (var i = 0; i < jsonFields.length; i++) {
                if (jsonFields[i].fieldName == field.fieldName) {
                    jsonFields[i].defaultValue = jsonFields[i].defaultValue == null ? "" : jsonFields[i].defaultValue;
                    return jsonFields[i];
                }
            }
        }
        return field;
    }
    //渲染字段列表
    function renderFieldsTable(eSFields) {
        $('#tbody').html('');
        eSFields.forEach(function (item, index) {
            var tr = $(`<tr>
                            <td>
                                <input type="hidden" class="hiType" value="`+ item.type + `">
                                <input type="hidden" class="hiCategory" value="`+ item.category + `">
                                <input type="hidden" class="hiIsValue" value="`+ item.isValue + `">
                                <input type="hidden" class="hiDefaultValue" value="`+ item.defaultValue + `">
                                <input type="text" autocomplete="off" class="layui-input fieldName" value="`+ item.fieldName + `" disabled>
                            </td>
                            <td>
                               `+ eleType + `
                            </td>
                            <td>
                               <div>`+ categories + `</div>
                            </td>
                            <td>
                                <div><input type="checkbox" lay-skin="switch" class="isValue" style="display:none" lay-text="是|否" `+ (item.isValue ? 'checked' : '') + ` /></div>
                            </td>
                            <td>
                                <div><input type="checkbox" lay-skin="switch" class="required" lay-text="是|否" `+ (item.required ? 'checked' : '') + ` /></div>
                            </td>
                            <td class="tdDefault">
                                
                            </td>
                        </tr>`);
            $('#tbody').append(tr);
        });
        $('#tbody').find('tr').each(function () {
            renderElem($(this));
            renderSelect($(this));
        });
        form.render();
        form.on('select(type)', function (data) {
            var tr = $(this).parents('tr');
            tr.find('.hiType').val(data.elem.value);

            if (data.elem.value != '') {
                tr.find('.type').next().find('input').css('border-color', 'rgb(195, 195, 195)');
            }
            renderElem(tr);
        });
        form.on('select(category)', function (data) {
            var tr = $(this).parents('tr');
            tr.find('.hiCategory').val(data.elem.value);
            tr.find('.category').val(data.elem.value);
            if (data.elem.value != '') {
                tr.find('.category').next().find('input').css('border-color', 'rgb(195, 195, 195)');
                renderSelect(tr);
            }
        });
    }
    function renderElem(tr) {
        var obj = {
            fieldName: tr.find('.fieldName').val(),
            type: tr.find('.hiType').val(),
            category: tr.find('.hiCategory').val(),
            defaultValue: tr.find('.hiDefaultValue').val()
        };
        tr.find('.tdDefault').html(getDefaultValueHtml(obj));
        tr.find('.type').val(obj.type);
        tr.find('.category').val(obj.category);
        if (obj.type == '0') {
            tr.find('.category').parent().css('visibility', 'visible');
            tr.find('.isValue').parent().css('visibility', 'visible');
        }
        else {
            tr.find('.category').parent().css('visibility', 'hidden');
            tr.find('.isValue').parent().css('visibility', 'hidden');
        }
        if (obj.type == '4') {
            //执行一个laydate实例
            layui.laydate.render({
                elem: tr.find('.startValue')[0], //指定元素
                type: 'date',
                format: 'yyyy-MM-dd',
                min: '2015-1-1',
                max: '2050-12-31',
                lang: 'cn',
                theme: 'molv',
                //range: '~',//或 range: '~' 来自定义分割字符
                calendar: true,
                done: function (value, date, endDate) {
                    //console.log(value); //得到日期生成的值，如：2017-08-18
                    //console.log(date); //得到日期时间对象：{year: 2017, month: 8, date: 18, hours: 0, minutes: 0, seconds: 0}
                    //console.log(endDate); //得结束的日期时间对象，开启范围选择（range: true）才会返回。对象成员同上。
                }
            });
            //执行一个laydate实例
            layui.laydate.render({
                elem: tr.find('.endValue')[0], //指定元素
                type: 'date',
                format: 'yyyy-MM-dd',
                min: '2015-1-1',
                max: '2050-12-31',
                lang: 'cn',
                theme: 'molv',
                //range: '~',//或 range: '~' 来自定义分割字符
                calendar: true
            });
        }
        else if (obj.type == '5') {
            form.render();
        }
    }
    function renderSelect(tr) {
        var obj = {
            type: tr.find('.hiType').val(),
            category: tr.find('.category').val(),
            isValue: tr.find('.hiIsValue').val(),
            defaultValue: tr.find('.hiDefaultValue').val()
        };
        if (obj.type == '0') {
            tr.find('.tdDefault').html('<div></div>');
            var defaultXmSelect = xmSelect.render({
                el: tr.find('.tdDefault div')[0],
                toolbar: { show: true },
                filterable: true,
                data: [],
                autoRow: false
            })
            updateSelect(obj, defaultXmSelect);
        }
    }

    //获取默认值内容
    function getDefaultValueHtml(item) {
        var defaultHtml = '';
        var defaultVal = item.defaultValue;
        switch (parseInt(item.type)) {
            case 0:
                defaultHtml = '<div></div>';
                break;
            case 1:
                var vals = ['', ''];
                if (defaultVal != '') {
                    vals = defaultVal.split(',');
                    if (vals.length == 1) {
                        vals = [vals[0], ""];
                    }
                }
                defaultHtml = `<div class="layui-input-inline" style="width:120px;">
                                    <input type="number" class="layui-input startValue" value="`+ vals[0] + `">
                                </div>
                                <div class="layui-input-inline" style="width:5px">
                                    <label class="layui-form-label-col">-</label>
                                </div>
                                <div class="layui-input-inline" style="width:120px;margin-right:0">
                                    <input type="number" class="layui-input endValue" value="`+ vals[1] + `">
                                </div>`;
                break;
            case 2: case 3:
                defaultHtml = `<input type="text" placeholder="请输入默认值（选填）" autocomplete="off" class="layui-input defaultValue" value="` + defaultVal + `">`;
                break;
            case 4:
                var vals = ['', ''];
                if (defaultVal != '') {
                    vals = defaultVal.split(',');
                    if (vals.length == 1) {
                        vals = [vals[0], ""];
                    }
                }
                defaultHtml = `<div class="layui-input-inline" style="width:120px">
                                    <input type="text" class="layui-input startValue" value="`+ vals[0] + `">
                                </div>
                                <div class="layui-input-inline" style="width:5px">
                                    <label class="layui-form-label-col">-</label>
                                </div>
                                <div class="layui-input-inline" style="width:120px;margin-right:0">
                                    <input type="text" class="layui-input endValue" value="`+ vals[1] + `">
                                </div>`;

                break;
            case 5:
                defaultHtml = `<div class="layui-input-inline">
                                 <input type="checkbox" lay-skin="switch" class="defaultValue" lay-text="是|否" ` + (defaultVal == 't' ? 'checked' : '') + ` />
                              </div>`;
                break;
            default:
        }
        return defaultHtml;
    }
    //重新渲染枚举多选下拉
    function updateSelect(item, defaultXmSelect) {
        var arrData = [];
        var defaultVals = item.defaultValue.split(',');
        $.ajax({
            url: '/Business/ESDashboard/GetDataByCategory?category=' + item.category,
            method: 'get',
            dataType: 'json',
            success: function (res) {
                res.data.forEach(function (item, index) {
                    var obj = {};
                    obj.name = item.name;
                    obj.value = item.isValue == 'true' ? item.value : item.name;
                    if ($.inArray(item.value, defaultVals) >= 0) {
                        obj.selected = true;
                    }
                    arrData.push(obj);
                });
                defaultXmSelect.update({
                    data: arrData
                });
            }
        });
    }

    form.on('submit(form)', function (data) {
        var postData = {
            permissionId: data.field.permissionId,
            dashboardSrc: data.field.dashboardSrc,
            username: data.field.username,
            password: data.field.password,
            maxDateRange: data.field.maxDateRange,
            fieldsConfig: []
        };
        var trs = $('#tbody').find('tr');
        for (var i = 0; i < trs.length; i++) {
            var eSField = {
                fieldName: $(trs[i]).find('.fieldName').val(),
                category: $(trs[i]).find('.category').val(),
                type: $(trs[i]).find('.type').val(),
                isValue: $(trs[i]).find('.isValue').prop('checked'),
                required: $(trs[i]).find('.required').prop('checked')
            };
            if (eSField.type == '' || eSField.type == null) {
                var elem = $(trs[i]).find('.type').next().find('input');
                //elem.focus();
                elem.css('border-color', '#FF5722');
                layer.msg('请选择字段类型', { icon: 5 });
                return false;
            }
            if (eSField.type == '0' && (eSField.category == '' || eSField.category == null)) {
                var elem = $(trs[i]).find('.category').next().find('input');
                elem.css('border-color', '#FF5722');
                layer.msg('请选择枚举类别', { icon: 5 });
                return false;
            }
            switch (parseInt(eSField.type)) {
                case 0:
                    eSField.defaultValue = $(trs[i]).find('.xm-select-default').val();//defaultValue.getValue('valueStr');
                    break;
                case 1: case 4:
                    eSField.defaultValue = $(trs[i]).find('.startValue').val() + ',' + $(trs[i]).find('.endValue').val();
                    break;
                case 5:
                    eSField.defaultValue = $(trs[i]).find('.defaultValue')[0].checked ? 't' : 'f';
                    break;
                default:
                    eSField.defaultValue = $(trs[i]).find('.defaultValue').val();
            }
            postData.fieldsConfig.push(eSField);
        }

        $.post('/Business/ESDashboard/Set', postData, function (res) {
            if (!res.success) {
                layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                return false;
            }
            layer.msg('操作成功', { icon: 6 });

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