var $, tab, dataStr, layer, topMenuName = '';
layui.config({
    base: "/js/"
}).extend({
    "bodyTab": "bodyTab"
});
layui.use(['bodyTab', 'form', 'element', 'layer', 'jquery'], function () {
    var form = layui.form,
        element = layui.element;
    $ = layui.$;
    layer = parent.layer === undefined ? layui.layer : top.layer;
    var leftMenuName = '', iframeSrc = '';
    var pointArr = [];
    tab = layui.bodyTab({
        openTabNum: 10,  //最大可打开窗口数量
        url: "/Home/GetMenuData" //获取菜单json地址 后台地址返回菜单数据
    });
    //根据锚点初始化路由信息
    function initRoute() {
        var href = window.location.href;
        var paramStr = '';
        if (href.indexOf('?') > -1) {
            var arr = href.split('?');
            href = arr[0];
            paramStr = '?' + arr[1];
        }
        var urlArray = href.split('#');
        if (urlArray.length > 1) {
            pointArr = urlArray[1].split('/');
            //去空
            pointArr.forEach(function (value, index) {
                if (value == '') {
                    pointArr.splice(index, 1);
                }
            });
            if (pointArr.length > 0) {
                topMenuName = pointArr[0];
                leftMenuName = '/' + pointArr.slice(1).join('/');
                iframeSrc = leftMenuName + paramStr;
            }
        }
    }
    initRoute();
    //通过顶部菜单获取左侧二三级菜单
    function getData(json) {
        $.getJSON(tab.tabConfig.url,
            function (data) {
                dataStr = data[json];
                //重新渲染左侧菜单
                tab.render();
                //根据锚点定位页面
                if (leftMenuName != '') {
                    if (leftMenuName == "/home/main") {
                        return;
                    }
                    var act = getUrlParam(iframeSrc, 'permissioncode');
                    if (act != '') {
                        leftMenuName += '?permissioncode=' + act;
                    };
                    //如果是左侧菜单页
                    var selectLeftMenu = $(".layui-nav .layui-nav-item a:not('.mobileTopLevelMenus .layui-nav-item a')[data-url='" + leftMenuName + "']");
                    var title = '';
                    if ($(selectLeftMenu).length > 0) {
                        title = $(selectLeftMenu).find("cite").text();
                    }
                    else {
                        //如果是详情页
                        leftMenuName = '/' + pointArr.slice(1, pointArr.length - 1).join('/') + '/index'
                        selectLeftMenu = $(".layui-nav .layui-nav-item a:not('.mobileTopLevelMenus .layui-nav-item a')[data-url='" + leftMenuName + "']");
                        if ($(selectLeftMenu).length > 0) {
                            if (window.sessionStorage.getItem("menu")) {
                                var menu = JSON.parse(window.sessionStorage.getItem("menu"));
                                menu.forEach(function (value, index) {
                                    if (value.href == iframeSrc) {
                                        title = value.title;
                                        return;
                                    }
                                });
                            }
                        }
                        else {//如果还是没有找到左侧菜单，查找缓存里是否存在当前tab
                            var name = iframeSrc.split('?')[0].split('/').join('');
                            if (window.sessionStorage.getItem("menu")) {
                                var menu = JSON.parse(window.sessionStorage.getItem("menu"));
                                menu.forEach(function (value, index) {
                                    if (value.name == name) {
                                        leftMenuName = value.leftMenuName;
                                        selectLeftMenu = $(".layui-nav .layui-nav-item a:not('.mobileTopLevelMenus .layui-nav-item a')[data-url='" + leftMenuName + "']");
                                        if (value.href == iframeSrc) {
                                            title = value.title;
                                        }
                                        return;
                                    }
                                });
                            }
                        }
                    }
                    if ($(selectLeftMenu).length > 0) {
                        if (title == '') {
                            title = $(selectLeftMenu).find("cite").text() + '详情';
                        }
                        addTab($('<a topMenuName="' + topMenuName + '" leftMenuName="' + leftMenuName + '" data-url="' + iframeSrc + '"><i class="seraph icon-look" data-icon="icon-look"></i><cite>' + title + '</cite></a>'));
                        $('body').removeClass('site-mobile');  //移动端点击菜单关闭菜单层
                        $(selectLeftMenu).parents('dl').parent().addClass("layui-nav-itemed").siblings().removeClass("layui-nav-itemed").removeClass("layui-this");
                        $(selectLeftMenu).parent().addClass("layui-this").siblings().removeClass("layui-this");
                    }
                }
                else {
                    leftMenuName = "/home/main";
                }
            });
    }

    //通过顶部菜单获取左侧菜单
    $(".topLevelMenus li,.mobileTopLevelMenus dd").click(function () {
        topMenuName = $(this).data("menu");
        $(".topLevelMenus li").eq($(this).index()).addClass("layui-this").siblings().removeClass("layui-this");
        $(".mobileTopLevelMenus dd").eq($(this).index()).addClass("layui-this").siblings().removeClass("layui-this");
        $(".layui-layout-admin").removeClass("showMenu");
        $("body").addClass("site-mobile");
        if (afterInit) {
            leftMenuName = '';
        }
        getData($(this).data("menu"));
        //渲染顶部窗口
        tab.tabMove();
    });

    //隐藏左侧导航
    $(".hideMenu").click(function () {
        if ($(".topLevelMenus li.layui-this a").data("url")) {
            layer.msg("此栏目状态下左侧菜单不可展开");  //主要为了避免左侧显示的内容与顶部菜单不匹配
            return false;
        }
        $(".layui-layout-admin").toggleClass("showMenu");
        //渲染顶部窗口
        tab.tabMove();
    });
    var afterInit = false;
    if (defaultKey !== null && defaultKey !== '') {
        initMyPage();
        var topMenu;
        if ($('*[mobile]').css('display') == 'none') {
            if (topMenuName == '') {
                topMenuName = defaultKey;
                topMenu = $(".topLevelMenus li[data-menu='" + defaultKey + "']");
            }
            else {
                topMenu = $(".topLevelMenus li[data-menu='" + topMenuName + "']");
                if ($(topMenu).length == 0) {
                    topMenu = $(".topLevelMenus li[data-menu='" + defaultKey + "']");
                }
            }
        }
        else {
            if (topMenuName == '') {
                topMenuName = defaultKey;
                topMenu = $(".mobileTopLevelMenus dd[data-menu='" + defaultKey + "']");
            }
            else {
                topMenu = $(".mobileTopLevelMenus dd[data-menu='" + topMenuName + "']");
                if ($(topMenu).length == 0) {
                    topMenu = $(".mobileTopLevelMenus dd[data-menu='" + defaultKey + "']");
                }
            }
        }
        if ($(topMenu).length > 0) {
            $(topMenu).trigger('click');
            afterInit = true;
        }
    }
    //添加我的首页会话缓存，初始化缓存窗口
    function initMyPage() {
        var menu = [];
        if (window.sessionStorage.getItem("menu") == null) {
            var curmenu = {
                icon: "icon-look",
                title: "我的首页",
                name: "homemain",
                href: "/home/main",
                layId: "",
                topMenuName: defaultKey,
                leftMenuName: "/home/main"
            }
            menu.push(curmenu);
            window.sessionStorage.setItem("menu", JSON.stringify(menu)); //打开的窗口
            window.sessionStorage.setItem("curmenu", JSON.stringify(curmenu));  //当前的窗口
        }
        else {
            //刷新后还原打开的窗口
            if (cacheStr == "true") {
                var curmenu = window.sessionStorage.getItem("curmenu");
                menu = JSON.parse(window.sessionStorage.getItem("menu"));
                if (topMenuName == '' || leftMenuName == '') {
                    var cur = [];
                    if (curmenu) {
                        cur = JSON.parse(curmenu);
                    }
                    else {
                        cur = menu[0];
                        curmenu = JSON.stringify(cur);
                        window.sessionStorage.setItem("curmenu", curmenu);  //当前的窗口
                    }
                    topMenuName = cur.topMenuName;
                    leftMenuName = cur.leftMenuName;
                    iframeSrc = cur.href;
                }
                var openTitle = '';
                for (var i = 1; i < menu.length; i++) {
                    openTitle = '';
                    if (menu[i].icon) {
                        if (menu[i].icon.split("-")[0] == 'icon') {
                            openTitle += '<i class="seraph ' + menu[i].icon + '"></i>';
                        } else {
                            openTitle += '<i class="layui-icon">' + menu[i].icon + '</i>';
                        }
                    }
                    openTitle += '<cite name="' + menu[i].name + '">' + menu[i].title + '</cite>';
                    openTitle += '<i class="layui-icon layui-unselect layui-tab-close" data-id="' + menu[i].layId + '">&#x1006;</i>';
                    element.tabAdd("bodyTab", {
                        title: openTitle,
                        content: "<iframe name='" + menu[i].name + "' data-id='" + menu[i].layId + "'></iframe>",
                        id: menu[i].layId
                    })
                    //定位到刷新前的窗口
                    if (curmenu) {
                        if (JSON.parse(curmenu).name == menu[i].name) {  //定位到刷新前的页面
                            element.tabChange("bodyTab", menu[i].layId);
                        }
                    } else {
                        element.tabChange("bodyTab", menu[menu.length - 1].layId);
                    }
                }
                //渲染顶部窗口
                tab.tabMove();
            } else {
                window.sessionStorage.removeItem("menu");
                window.sessionStorage.removeItem("curmenu");
            }
        }
    }
    //点击控制台logo链接，定位到我的首页
    $('a.logo').on('click', function () {
        var curmenu = JSON.parse(window.sessionStorage.getItem("menu"))[0];
        element.tabChange("bodyTab", "").init();
        window.sessionStorage.setItem("curmenu", JSON.stringify(curmenu));  //当前的窗口
        //渲染顶部窗口
        tab.tabMove();
        if (curmenu) {
            var href = window.location.href;
            if (href.indexOf('#') > -1) {
                href = href.split('#')[0];
            }
            window.location.href = href + '#/' + curmenu.topMenuName + curmenu.href;
            tab.changeRegresh($(this).index(), curmenu.href, changeRefreshStr);
            top.positionMenu(curmenu);
        }
    });
    //手机设备的简单适配
    $('.site-tree-mobile').on('click', function () {
        $('body').addClass('site-mobile');
    });
    $('.site-mobile-shade').on('click', function () {
        $('body').removeClass('site-mobile');
    });

    // 添加新窗口
    $("body").on("click", ".layui-nav .layui-nav-item a:not('.mobileTopLevelMenus .layui-nav-item a,.topLevelMenus .layui-nav-item a')", function () {
        //如果不存在子级(表示是功能界面可以打开Tab页面呈现)
        if ($(this).siblings().length === 0) {
            leftMenuName = $(this).attr('data-url');
            $(this).attr('topMenuName', topMenuName);
            $(this).attr('leftMenuName', leftMenuName);
            addTab($(this));
            $('body').removeClass('site-mobile');  //移动端点击菜单关闭菜单层
        }
        $(this).parents('dl').parent().addClass("layui-nav-itemed").siblings();
    });

    //清除缓存
    $(".clearCache").click(function () {
        window.sessionStorage.clear();
        window.localStorage.clear();
        var index = layer.msg('清除缓存中，请稍候', { icon: 16, time: false, shade: 0.8 });
        setTimeout(function () {
            layer.close(index);
            location.href = '/';
            layer.msg("缓存清除成功！");
        }, 1000);
        return false;
    })


    // 绑定个人资料和修改密码按钮
    $('#selfInfo').click(function () {
        var url = $(this).prop('href');
        layer.open({
            type: 2,
            title: '个人资料',
            content: url,
            skin: 'layui-layer-lan',
            area: ['500px', '450px'],
            offset: 'auto',
            shade: 0.3
        });
        return false;
    });
    $('#changePwd').click(function () {
        var url = $(this).prop('href');
        layer.open({
            type: 2,
            title: '修改密码',
            content: url,
            skin: 'layui-layer-lan',
            area: ['500px', '280px'],
            offset: 'auto',
            shade: 0.3
        });
        return false;
    });

    //定位顶部菜单和左侧菜单
    window.positionMenu = function (curmenu) {
        if (curmenu && curmenu.topMenuName != '' && curmenu.leftMenuName != '') {
            var pcTopMenu = $(".topLevelMenus li[data-menu='" + curmenu.topMenuName + "']");
            var mobTopMenu = $(".mobileTopLevelMenus dd[data-menu='" + curmenu.topMenuName + "']");
            if ($(pcTopMenu).length > 0) {
                $(pcTopMenu).addClass("layui-this").siblings().removeClass("layui-this");
            }
            if ($(mobTopMenu).length > 0) {
                $(mobTopMenu).addClass("layui-this").siblings().removeClass("layui-this");
            }
            $(".layui-layout-admin").removeClass("showMenu");
            $("body").addClass("site-mobile");
            $.getJSON(tab.tabConfig.url,
                function (data) {
                    dataStr = data[curmenu.topMenuName];
                    //重新渲染左侧菜单
                    tab.render();
                    //根据锚点定位页面
                    var selectLeftMenu = $(".layui-nav .layui-nav-item a:not('.mobileTopLevelMenus .layui-nav-item a')[data-url='" + curmenu.leftMenuName + "']");
                    $('body').removeClass('site-mobile');  //移动端点击菜单关闭菜单层
                    $(selectLeftMenu).parents('dl').parent().addClass("layui-nav-itemed").siblings().removeClass("layui-nav-itemed").removeClass("layui-this");
                    $(selectLeftMenu).parent().addClass("layui-this").siblings().removeClass("layui-this");
                });
        }
    }
});

//打开新窗口
function addTab(_this) {
    tab.tabAdd(_this);
}