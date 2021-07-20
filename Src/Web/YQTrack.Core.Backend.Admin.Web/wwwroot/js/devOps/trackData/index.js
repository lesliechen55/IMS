layui.use(['form', 'layer', 'table', 'laytpl'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    var objWhere = top.initFilter(window);
    if ($('#number').val() != '') {
        renderJson();
    }
    function renderJson() {
        var l = layer.load(0, {
            shade: [0.3, '#fff']
        });//0.3透明度的白色背景
        $.get('/DevOps/TrackData/GetJsonData?number=' + $('#number').val(),
            function (res) {
                layer.close(l);
                if (!res.success) {
                    layer.alert('查询失败:' + res.msg, { title: '提示', icon: 5 });
                    return;
                }
                var input = JSON.parse(res.data);
                var options = {
                    collapsed: false,
                    rootCollapsable: true,
                    withQuotes: true,
                    withLinks: true
                };
                $('#json-renderer').jsonViewer(input, options);
            });
    }

    $("#search").click(function () {
        $('#json-renderer').empty();
        let number = $('#number').val();
        if ($.trim(number).length <= 0) {
            layer.msg('缺少必要查询参数运输单号');
            return;
        }
        renderJson();
        top.changeUrlParam(window);
        return false;
    });
});