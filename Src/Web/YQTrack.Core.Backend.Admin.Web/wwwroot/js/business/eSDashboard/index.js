layui.config({
    base: '/module/'
}).extend({
    xmSelect: 'xm-select'
}).use(['layer', 'jquery', 'form', 'xmSelect', 'laydate'], function () {

    var form = layui.form,
        $ = layui.jquery;
    var layer = parent.layer === undefined ? layui.layer : top.layer;

    var xmSelect = layui.xmSelect;
    initFilter();

    function initFilter() {
        var src = $('#dashboardSrc').val();
        //解码
        src = decodeURIComponent(src).replace(/#([a-zA-Z0-9]{3,6}(?:,|\)))/g, '%23$1');

        //时间范围选中
        var regTime = /_g=[\s\S]*time:\(([\s\S]*?)\)\)/;
        var time = regTime.exec(src);
        $('#time').val(time[1]);
        $('#dashboardSrc').val(src);

        ////全文搜索字段选中
        //if ($('#fulltextKey').length > 0) {
        //    var regFulltext = /linked:!f,query:\(query_string:\((?:analyze_wildcard:!t,)?query:'([\s\S]*?)'\)\)/
        //    if (regFulltext.test(src)) {
        //        var fulltext = regFulltext.exec(src)[1];
        //        if (fulltext) {
        //            //只处理第一个全文搜索字段
        //            var i = fulltext.indexOf(' ');
        //            if (i > -1) {
        //                fulltext = fulltext.substring(0, i);
        //            }
        //            var arrFull = fulltext.split(':');
        //            if (arrFull.length > 1) {
        //                $('#fulltextKey').val(arrFull[0]);
        //                $('#fulltextValue').val(arrFull[1]);
        //            }
        //        }
        //    }
        //}

        form.render();
        renderFields();
    }
    //获取url所有参数值
    function getParameters() {
        var arrObj = [];
        // 取得url中?后面的字符
        var query = window.location.search.substring(1);
        // 把参数按&拆分成数组
        var param_arr = query.split("&");
        param_arr.forEach(function (item, index) {
            if (item == '') {
                param_arr.splice(index, 1);
            }
            else {
                var pair = item.split("=");
                if (pair[0] != 'permissionCode') {
                    pair[1] = decodeURIComponent(pair[1]).replace(/\+/g, " ");
                    arrObj.push({ key: pair[0], value: pair[1] });
                }
            }
        });
        return arrObj;
    }

    //渲染筛选字段
    function renderFields() {
        var jsonFields = JSON.parse($('#hiFields').val());
        $('#fields').html('');
        jsonFields.forEach(function (item, index) {
            var defaultHtml = `<div class="fieldItem">
                                <div class="layui-form-mid"> &nbsp;` + item.fieldName + `
                                    <input type="hidden" class="type" value="`+ item.type + `">
                                    <input type="hidden" class="fieldName" value="`+ item.fieldName + `">
                                    <input type="hidden" class="category" value="`+ item.category + `">
                                    <input type="hidden" class="required" value="`+ item.required + `">
                                    <input type="hidden" class="isValue" value="`+ item.isValue + `">
                                    <input type="hidden" class="hiDefaultValue" value="`+ item.defaultValue + `">
                                </div>`;
            var defaultVal = item.defaultValue == null ? "" : item.defaultValue;
            var id = item.fieldName.replace(/\./g, '-').replace(/@/g, '_');
            switch (parseInt(item.type)) {
                case 0:
                    defaultHtml += '<div class="layui-input-inline" style="width:150px;"><div class="xmSelect"></div></div>';
                    break;
                case 1:
                    var vals = ['', ''];
                    if (defaultVal != '') {
                        vals = defaultVal.split(',');
                    }
                    defaultHtml += `<div class="layui-input-inline" style="width:120px;">
                                        <input type="number" id="`+ id + `StartValue" name="` + id + `StartValue" class="layui-input startValue" value="` + vals[0] + `">
                                    </div>
                                    <div class="layui-input-inline" style="width:5px">
                                        <label class="layui-form-label-col">-</label>
                                    </div>
                                    <div class="layui-input-inline" style="width:120px;">
                                        <input type="number" id="`+ id + `EndValue" name="` + id + `EndValue" class="layui-input endValue" value="` + vals[1] + `">
                                    </div>`;
                    break;
                case 2: case 3:
                    defaultHtml += `<div class="layui-input-inline">
                                      <input type="text" placeholder="请输入`+ item.fieldName + `" autocomplete="off" class="layui-input defaultValue"  id="` + id + `" name="` + id + `" value="` + defaultVal + `">
                                   </div>`;
                    break;
                case 4:
                    var vals = ['', ''];
                    if (defaultVal != '') {
                        vals = defaultVal.split(',');
                    }
                    defaultHtml += `<div class="layui-input-inline" style="width:120px">
                                        <input type="text" class="layui-input startValue" id="`+ id + `StartValue" name="` + id + `StartValue" value="` + vals[0] + `">
                                    </div>
                                    <div class="layui-input-inline" style="width:5px">
                                        <label class="layui-form-label-col">-</label>
                                    </div>
                                    <div class="layui-input-inline" style="width:120px;">
                                        <input type="text" class="layui-input endValue" id="`+ id + `EndValue" name="` + id + `EndValue" value="` + vals[1] + `">
                                    </div>`;

                    break;
                case 5:
                    defaultHtml +=  `<div class="layui-input-inline">
                                        <input type="checkbox" lay-skin="switch" class="defaultValue" lay-text="是|否" ` + (defaultVal == 't' ? 'checked' : '') + ` />
                                    </div>`;
                    break;
                default:
            }
            defaultHtml += '</div>';
            $('#fields').append(defaultHtml);
        });

        var urlParams = getParameters();
        $('form').find('div.fieldItem').each(function () {
            renderElem($(this), urlParams);
        });

        //开始检索数据
        setTimeout(function () {
            urlParams.forEach(function (item, index) {
                if (item.key.indexOf('xm-') == -1) {
                    $('form').find('#' + item.key.replace(/\./g, '-').replace(/@/g, '_')).val(item.value);
                }
            });
            form.render();
            searchData();
        }, 500);
    }

    function renderElem(field, urlParams) {
        var obj = {
            fieldName: field.find('.fieldName').val(),
            category: field.find('.category').val(),
            isValue: field.find('.isValue').val(),
            type: field.find('.type').val(),
            defaultValue: field.find('.hiDefaultValue').val()
        };
        if (obj.type == "0") {
            var defaultXmSelect = xmSelect.render({
                el: field.find('div.xmSelect')[0],
                toolbar: { show: true },
                filterable: true,
                name: 'xm-' + obj.fieldName.replace(/\./g, '-').replace(/@/g, '_'),
                data: []
            })
            renderSelect(obj, defaultXmSelect, urlParams);
        }
        else if (obj.type == '4') {
            //执行一个laydate实例
            layui.laydate.render({
                elem: field.find('.startValue')[0], //指定元素
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
                elem: field.find('.endValue')[0], //指定元素
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

    }

    //重新渲染枚举多选下拉
    function renderSelect(item, defaultXmSelect, urlParams) {
        var arrData = [];
        var defaultVals = [];
        var urlItem = {};
        urlParams.forEach(function (item1, index) {
            if (item1.key == 'xm-' + item.fieldName) {
                urlItem = item1;
                return false;
            }
        });
        if (urlItem.key) {
            defaultVals = urlItem.value.split(',');
        }
        else {
            defaultVals = item.defaultValue.split(',');
        }
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
                    data: arrData,
                    autoRow: false,
                });
            }
        });
    }

    $("#search").click(function () {
        searchData();
    });

    function searchData() {

        var src = $('#dashboardSrc').val();
        if (src == '') {
            return false;
        }

        if ($('#time').val()) {
            var regTime = /_g=[\s\S]*time:\(([\s\S]*?)\)\)/;
            var time = regTime.exec(src);
            src = src.replace(time[0], time[0].replace(time[1], $('#time').val()));
        }

        //正则取字段
        var reg = /_a=\(filters:!\(([\s\S]*?)\),linked:/;
        reg.test(src);
        var src1 = RegExp.$1;
        var reg1 = /\('\$state':\(store:appState\),[\s\S]*?\)(?:,query|,range)/g;
        var res = src1.match(reg1);
        var regKey = /key:'?([\s\S]*?)'?,/;
        var regValue = /value:'?([\s\S]*?)'?\)/;
        var regValue1 = /params:\(query:!([\s\S]*?)\)/;

        var fulltextStr = '';

        var fieldsStr = '';
        var fields = $('#fields').find('div.fieldItem');

        if (fields.length == 0) {
            $('#fields').next('br').remove();
        }
        for (var i = 0; i < fields.length; i++) {
            var eSField = {
                fieldName: $(fields[i]).find('.fieldName').val(),
                category: $(fields[i]).find('.category').val(),
                required: $(fields[i]).find('.required').val(),
                isValue: $(fields[i]).find('.isValue').val(),
                type: $(fields[i]).find('.type').val()
            };
            switch (parseInt(eSField.type)) {
                case 0:
                    eSField.inputValue = $(fields[i]).find('.xm-select-default').val();
                    break;
                case 1: case 4:
                    eSField.inputValue = $(fields[i]).find('.startValue').val() + ',' + $(fields[i]).find('.endValue').val();
                    break;
                case 5:
                    eSField.inputValue = $(fields[i]).find('.defaultValue')[0].checked ? 't' : 'f';
                    break;
                default:
                    eSField.inputValue = $(fields[i]).find('.defaultValue').val();
            }
            if (eSField.inputValue.replace(',', '') == '') {
                if (eSField.required == 'true') {
                    layer.msg('必填项:"' + eSField.fieldName + '"不能为空', { icon: 5 });
                    return false;
                }
                else {
                    continue;
                }
            }
            //拼接字段
            res.forEach(function (item, index) {
                var fieldName = regKey.exec(item);
                var value = regValue.exec(item);
                if (!value) {
                    value = regValue1.exec(item);
                }
                if (eSField.fieldName == fieldName[1]) {
                    var valStr = '';
                    eSField.fieldName = eSField.fieldName.indexOf('@') > -1 ? ("'" + eSField.fieldName + "'") : eSField.fieldName;
                    switch (parseInt(eSField.type)) {
                        case 0:
                            fieldsStr += item.replace(value[0], value[0].replace(value[1], eSField.inputValue)).replace('params:!(' + value[1] + ')', 'params:!(' + eSField.inputValue + ')');
                            eSField.inputValue.split(',').forEach(function (item, index) {
                                valStr += '(match_phrase:(' + eSField.fieldName + ':' + item + ')),';
                            });
                            fieldsStr += ':(bool:(minimum_should_match:1,should:!(' + valStr.substr(0, valStr.length - 1) + ')))),';
                            break;
                        case 1:
                            var arrVal = eSField.inputValue.split(',');
                            valStr = (arrVal[0] == '' ? '-∞' : arrVal[0]) + ' to ' + (arrVal[1] == '' ? '+∞' : arrVal[1]);
                            fieldsStr += item.replace(value[0], value[0].replace(value[1], valStr));
                            fieldsStr += ':(' + eSField.fieldName + ':(gte:' + (arrVal[0] == '' ? '!n' : arrVal[0]) + ',lt:' + (arrVal[1] == '' ? '!n' : arrVal[1]) + '))),';
                            break;
                        case 4:
                            var arrVal = eSField.inputValue.split(',');
                            valStr = (arrVal[0] == '' ? '-Infinity' : arrVal[0]) + ' to ' + (arrVal[1] == '' ? '+Infinity' : arrVal[1]);
                            fieldsStr += item.replace(value[0], value[0].replace(value[1], valStr));
                            fieldsStr += ':(' + eSField.fieldName + ':(gte:' + (arrVal[0] == '' ? '!n' : ("'" + arrVal[0] + "'")) + ',lt:' + (arrVal[1] == '' ? '!n' : ("'" + arrVal[1] + "'")) + '))),';
                            break;
                        case 5:
                            fieldsStr += item.replace(value[0], value[0].replace(value[1], eSField.inputValue)).replace('params:!(' + value[1] + ')', 'params:!(' + eSField.inputValue + ')');
                            fieldsStr += ':(match_phrase:(' + eSField.fieldName + ':!' + eSField.inputValue + '))),';
                            break;
                        default:
                            fieldsStr += item.replace(value[0], value[0].replace(value[1], eSField.inputValue));
                            fieldsStr += ':(match:(' + eSField.fieldName + ':(query:' + eSField.inputValue + ',type:phrase)))),';
                    }
                }
            })

            //全文搜索字段
            if (eSField.inputValue != '' && eSField.type == '3') {
                fulltextStr += eSField.fieldName + ':' + eSField.inputValue + " AND ";
            }
        }//字段循环结束
        //替换字段筛选内容
        src = src.replace(src1, fieldsStr.substr(0, fieldsStr.length - 1));
        //替换全文搜索字段
        var regFulltext = /linked:!(?:f|t),query:\(([\s\S]*?)\),/;
        var fulltext = regFulltext.exec(src);;
        if (fulltext.length > 1) {
            if (fulltextStr != '') {
                if (fulltext.length > 1) {
                    fulltextStr = "query_string:\(" + (fulltext[1].indexOf('analyze_wildcard:!t,') > -1 ? "analyze_wildcard:!t," : "") + "query:'" + fulltextStr.substr(0, fulltextStr.length - 5) + "'\)";
                    src = src.replace(fulltext[0], fulltext[0].replace(fulltext[1], fulltextStr));
                }
            }
            else {
                src = src.replace(fulltext[0], fulltext[0].replace(fulltext[1], 'match_all:()'));
            }
        }
        top.changeUrlParam(window);
        $('iframe').attr('src', src);
    }

});