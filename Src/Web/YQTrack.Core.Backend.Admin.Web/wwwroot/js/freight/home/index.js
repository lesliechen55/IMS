layui.use(['form', 'layer', 'table', 'laytpl'], function () {

    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    //icon动画
    $(".panel a").hover(function () {
        $(this).find(".layui-anim").addClass("layui-anim-scaleSpring");
    }, function () {
        $(this).find(".layui-anim").removeClass("layui-anim-scaleSpring");
    });

    $('#export1').click(function () {
        $.fileDownload($(this).prop('href'))
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

    $('#export2').click(function () {
        $.fileDownload($(this).prop('href'))
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

    $('#export3').click(function () {
        $.fileDownload($(this).prop('href'))
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