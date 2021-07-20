'use strict';

layui.use(['layer', 'jquery'], function () {
    const $ = layui.jquery;
    var layer = parent.layer === undefined ? layui.layer : top.layer;
    if ($) {
        $.imsPost = function (url, data, successFunc, btnId, timeout) {
            // 加载loading效果
            var index = layer.load({ time: timeout === undefined ? 3000 : timeout });
            $.ajax({
                type: "post",//请求方式
                timeout: timeout === undefined ? 3000 : timeout,//超时时间
                data: data,//请求发送数据
                contentType: "application/x-www-form-urlencoded",//发送信息至服务器时内容编码类型
                dataType: "json",//返回值类型
                url: url,//请求地址
                beforeSend: function () {
                    // 禁用按钮防止重复点击
                    if (btnId) {
                        $(btnId).attr("disabled", "disabled");
                    }
                },
                success: function (res) {
                    // 预处理错误消息情况
                    if (!res.success) {
                        layer.alert('操作失败:' + res.msg, { title: '错误', icon: 5 });
                        return;
                    }
                    successFunc(res.data);
                },
                complete: function () {
                    // 恢复按钮状态以及关闭loading层
                    if (btnId) {
                        $(btnId).removeAttr("disabled");
                    }
                    layer.close(index);
                },
                error: function (xhr, textStatus, errorThrown) {
                    // 处理异常错误情况
                    layer.alert('操作失败:' + errorThrown, { title: '错误', icon: 5 });
                }
            });
        };
    }
    window.addMyTab = function (url, title, topMenuName = '', leftMenuName = '') {
        let menuHtmlTemplate = '<a topMenuName="' + topMenuName + '" leftMenuName="' + leftMenuName + '" data-url="' + url + '"><i class="seraph icon-look" data-icon="icon-look"></i><cite>' + title + '</cite></a>';
        window.parent.addTab($(menuHtmlTemplate));
    };
    //获取url参数值
    window.getUrlParam = function (url, name) {
        // 取得url中?后面的字符
        var arr = url.split('?');
        if (arr.length < 2) {
            return "";
        }
        var query = arr[1];
        // 把参数按&拆分成数组
        var param_arr = query.split("&");
        for (var i = 0; i < param_arr.length; i++) {
            var pair = param_arr[i].split("=");
            if (pair[0] == name) {
                return pair[1];
            }
        }
        return "";
    }
    //获取所有url参数值
    window.getAllUrlParam = function () {
        var object = {};
        // 取得url中?后面的字符
        var arr = window.location.href.split('?');
        if (arr.length < 2) {
            return object;
        }
        var query = arr[1];
        // 把参数按&拆分成数组
        var param_arr = query.split("&");
        for (var key in param_arr) {
            object[param_arr[key].split("=")[0]] = decodeURIComponent(param_arr[key].split("=")[1]).replace(/\+/g, " ");
        }
        return object;
    }
    //根据url参数初始化筛选条件，并返回所有参数值
    window.initFilter = function (myWindow, formSelects) {
        var object = {};
        // 取得url中?后面的字符
        var query = myWindow.location.search.substring(1);
        // 把参数按&拆分成数组
        var param_arr = query.split("&");
        param_arr.forEach(function (value, index) {
            if (value == '') {
                param_arr.splice(index, 1);
            }
        });
        for (var i = 0; i < param_arr.length; i++) {
            var pair = param_arr[i].split("=");
            pair[1] = decodeURIComponent(pair[1]).replace(/\+/g, " ");
            var el = $(myWindow.document).find('#' + pair[0]);
            if ($(el).length > 0) {
                switch ($(el)[0].type) {
                    case 'select-multiple':
                        formSelects.value(pair[0], pair[1].split(','));
                        break;
                    default:
                        $(el).val(pair[1]);
                        break;
                }
            }
            object[pair[0]] = pair[1];
        }
        myWindow.layui.form.render();
        var table = $(myWindow.document).find('#table');
        if ($(table).length > 0) {
            object.top = $(table)[0].offsetTop + 10;
        }
        return object;
    }
    //根据表单内容改变url参数值
    window.changeUrlParam = function (myWindow) {
        var href = window.location.href;
        var actIndex = href.indexOf('?permissioncode=');
        var act = '';
        if (actIndex > -1) {
            var param = href.substring(actIndex + 1);
            var endIndex = param.indexOf('&');
            if (endIndex > -1) {
                act = param.substring(0, endIndex) + '&';
            }
            else {
                act = param + '&';
            }
            href = href.substring(0, actIndex);
        }
        else {
            var inx = href.indexOf('?');
            if (inx > -1) {
                href = href.substring(0, inx);
            }
        }
        window.location.href = href + '?' + act + decodeURIComponent($(myWindow.document).find("form").serialize());
        var curmenu = '';
        var menu = JSON.parse(window.sessionStorage.getItem("menu"));
        var index = window.parent.tab.getLayIndex(myWindow.name);
        if (index == -1) {
            return;
        }
        else {
            if (menu) {
                var urlArray = window.location.href.split('#');
                if (urlArray.length > 1) {
                    var pointArr = urlArray[1].split('/');
                    //去空
                    pointArr.forEach(function (value, index) {
                        if (value == '') {
                            pointArr.splice(index, 1);
                        }
                    });
                    if (pointArr.length > 0) {
                        menu[index].href = "/" + pointArr.slice(1).join('/');
                        curmenu = menu[index];
                    }
                }
            }
            window.sessionStorage.setItem("curmenu", JSON.stringify(curmenu));
            window.sessionStorage.setItem("menu", JSON.stringify(menu));
        }
    }
    //刷新父级页面方法
    window.refreshParent = function () {
        //这种情况下，父页面一定是当前缓存页
        if (window.sessionStorage.getItem("curmenu")) {
            var curmenu = JSON.parse(window.sessionStorage.getItem("curmenu"));
            window.frames[curmenu.name].location.href = curmenu.href;
        }
    }
});

