
/*  定义全局参数*/
var isfirstload = true;
var rowInd = undefined;
$(function () {

    //绑定快速查询
    $("#searchType").combobox({
        url: '/BasicInfo/GetSearchTypeList',
        valueField: 'id',
        textField: 'text',
        onSelect: function (res) {

        },
    });
    $("#searchType").combobox("select", 0);

    //绑定过滤条件
    $("#glSelect").combobox({
        url: '/BasicInfo/GetGLList',
        valueField: 'id',
        textField: 'text',
        onSelect: function (res) {
            searchList(res.id);
        },
    });
    $("#glSelect").combobox("select", -1);
    GetProductType();
    SearchList();
    searchList();
    $("#treeEdit").click(function (event) {
        event = event || window.event;
        event.stopPropagation();
    });

    //点击层外，隐藏这个层。由于层内的事件停止了冒泡，所以不会触发这个事件    
    $(document).click(function (e) {

        $("#treeEdit").hide();

    });

    //$("#pType").etree("expandAll");
    //从库中选择商品-搜索条件
    $("#pinfo_SelectProduct_Search").combobox({
        url: '/BasicInfo/SearchAreaCondition',
        valueField: 'id',
        textField: 'text',
        width: 100,
    });
    $("#pinfo_SelectProduct_Search").combobox("setValue", 0);


    ChangeSize();
});

//从库中选择商品
function pinfo_SelectProduct() {
    //选择商品对话框
    $("#pinfo_SelectProduct").dialog({
        title: '选择关联商品',
        width: 415,
        height: 455,
        modal: true
    });
    $("#pinfo_SelectProduct_List").datagrid({
        url: '/BasicInfo/GetPIProductList',
        singleSelect: true,
        rownumbers: true,
        striped: true,
        loadMsg: '数据加载中。。',
        remoteSort: false,
        fitColumns: true,
        fixed: true,
        pagination: true,
        columns: [[
            {
                field: 'ProductCode',
                title: '商品编号',
                width: 100,
            },
            {
                field: 'ProductFullName',
                title: '商品全名',
                width: 100,
            },
            {
                field: 'ProductShortName',
                title: '商品简名',
                width: 100,
            },
            {
                field: 'Notes',
                title: '备注',
                width: 100,
            },
        ]],
        onLoadSuccess: function () {
            pinfo_SelectProduct_FillInterRows();
        }
    });
    pinfo_SelectProduct_Search();
}
//从库中选择商品-填充数据到十行
function pinfo_SelectProduct_FillInterRows() {
    var data = $('#pinfo_SelectProduct_List').datagrid('getData');
    var pageopt = $('#pinfo_SelectProduct_List').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#pinfo_SelectProduct_List').datagrid('appendRow', {});
        }
    }
}

//从库中选择商品-查询
function pinfo_SelectProduct_Search()
{
    $("#pinfo_SelectProduct_List").datagrid("load", {
        type: $("#pinfo_SelectProduct_Search").combobox("getValue"),
        content: $("#pinfo_SelectProduct_Content").val()
    });
}

//关闭选择商品对话框
function pinfo_SelectProduct_Close() {
    $("#pinfo_SelectProduct").dialog("close");
}

//选择商品-选中
function pinfo_SProduct() {
    var row = $("#pinfo_SelectProduct_List").datagrid("getSelected");
    if (row != null && row.ProductId != undefined && row.ProductId != "") {

        EditProduct(row.ProductId, row.ClassId, 0);
        pinfo_SelectProduct_Close();
    }
    else {
        $.messager.alert("提示信息", "请先选择一条商品", "info");
    }
}

var isEdit = false;
//绑定商品类别
function GetProductType() {
    $("#pType").etree({
        url: '/BasicInfo/GetProductTypes',
        id: 'ptype',
        lines: true,
        dataType: 'text',
        dnd: true,
        onDblClick: function (node) {
            e.preventDefault();
        },
        onClick: function (node) {
            var ptype = "\\";
            var list = [];
            if (node.id == 0) {
                return;
            }
            var parentNode = $("#pType").etree("getParent", node.target);
            var pparentNode;
            var ppparentNode;
            if (parentNode != null && parentNode.id) {
                pparentNode = $("#pType").etree("getParent", parentNode.target);
                if (pparentNode != null && pparentNode.id != 0) {
                    ppparentNode = $("#pType").etree("getParent", pparentNode.target);
                }

            }
            if (ppparentNode != null && ppparentNode.id != 0) {
                ptype += ppparentNode.text + "\\";
            }
            if (pparentNode != null && pparentNode.id != 0) {
                ptype += pparentNode.text + "\\";
            }
            if (parentNode != null && parentNode.id != 0) {
                ptype += parentNode.text + "\\";
            }
            ptype += node.text + "\\";
            $("#productType").text(ptype);
            $("#editPType").text(ptype);
            $("#hid_pType").val(node.id);
            searchList();

        },
        onSelect: function (node) {
            var ptype = "\\";
            var list = [];
            if (node.id == 0) {
                return;
            }
            var parentNode = $("#pType").etree("getParent", node.target);
            var pparentNode;
            var ppparentNode;
            if (parentNode != null && parentNode.id) {
                pparentNode = $("#pType").etree("getParent", parentNode.target);
                if (pparentNode != null && pparentNode.id != 0) {
                    ppparentNode = $("#pType").etree("getParent", pparentNode.target);
                }

            }
            if (ppparentNode != null && ppparentNode.id != 0) {
                ptype += ppparentNode.text + "\\";
            }
            if (pparentNode != null && pparentNode.id != 0) {
                ptype += pparentNode.text + "\\";
            }
            if (parentNode != null && parentNode.id != 0) {
                ptype += parentNode.text + "\\";
            }
            ptype += node.text + "\\";
            $("#productType").text(ptype);
            $("#editPType").text(ptype);
            $("#hid_pType").val(node.id);
            searchList();
        },
        onContextMenu: function (e, node) {
            console.log(node);
            e.preventDefault();
            if (node.id == 0)//如果是0则是没有分类,添加的第一个分类
            {
                $("input[name=nodetype]:eq(1)").prop("checked", true);
                $("input[name=nodetype]:eq(0)").attr("disabled", "disabled");
            }
            else {
                $("input[name=nodetype]:eq(0)").attr("disabled", false);
            }
            $("#pType").etree("expand", node.target);
            $("#pType").etree("select", node.target);
            $("#treeEdit").css("display", "block");
            $("#treeEdit").css("top", e.pageY + "px");
            $("#treeEdit").css("left", e.pageX + "px");
        },
        updateUrl: '/BasicInfo/TypeEdit',
        dndUrl: '/BasicInfo/TypeDnd',
    });

}

//当不够十行时补充十行空数据
function FillProductRows() {
    var data = $('#slist').datagrid('getData');
    var pageopt = $('#slist').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#slist').datagrid('appendRow', {});
        }
    }
}

//操作
function operator(value, rowData, rowIndex) {
    if (value == undefined || value == "") {
        return "";
    }
    return '<a href="#" class="easyui-linkbotton" style=\"color:blue;margin-left:5px;\" onclick="form_edit(\'' + rowData.ProductId + '\',\'' + rowData.ClassId + '\')">编辑</a><a href="#"  style=\"color:blue;margin-left:10px;\" class="easyui-linkbotton" onclick="form_del(\'' + rowData.ProductId + '\')">删除</a>';
}

//搜索+渲染dagarid
function SearchList() {
    /*  动态拼接column   */
    var json = new Array();
    $.ajax({
        url: '/BasicInfo/GetColumnConfig',
        type: 'POST',
        async: false,
        success: function (data) {
            var data = eval("(" + data + ")");
            var column1 = {};

            column1["field"] = "";
            column1["width"] = 30;
            column1["checkbox"] = true;
            json.push(column1);
            column1 = {};
            column1["title"] = "操作";
            column1["field"] = "ProductId";
            column1["width"] = 80;
            column1["formatter"] = operator;
            json.push(column1);

            $.each(data, function (i, field) {
                var column = {};
                if (field.FieldName != "ProductId") {
                    if (!field.Visible) {
                        column["hidden"] = true;
                    }
                    column["title"] = field.DisplayName;
                    column["field"] = field.FieldName;
                    column["sortable"] = true;
                    column["width"] = 100;
                    json.push(column);
                }

            })
        }
    });
    /* 渲染表格 */
    $("#slist").datagrid({
        url: '/BasicInfo/GetList',
        rownumbers: true,
        loadMsg: '数据加载中。。',
        striped: true,
        remoteSort: false,
        pagination: true,
        //singleSelect: true,
        fixed: true,
        fitColumns: true,
        columns: [json],
        onHeaderContextMenu: function (e, field) {
            e.preventDefault();
            $("#lpz").menu('show', {
                left: e.pageX,
                top: e.pageY,
            });
        },
        onLoadSuccess: function () {
            FillProductRows();
        }

    });


}

//刷新操作
function reloadList() {
    $("#slist").datagrid("reload");
}

//搜索
function searchList(status) {
    $("#slist").datagrid("load", {
        searchType: $("#searchType").combobox("getValue"),
        type: $("#hid_pType").val(),
        searchContent: $("input[name=scontent]").val(),
        status: status == undefined ? $("#glSelect").val() : status
    });
    //$("#slist").datagrid("load");
}



//编辑
function form_edit(row, pType) {
    EditProduct(row, pType, 1);
}

//编辑
function editProductForm() {
    var row = $("#slist").datagrid("getSelections");
    if (row != null && row.length == 1) {
        if (row[0].ProductCode == undefined) {
            $.messager.alert("提示信息", "请选择一行商品数据", "info");
            return;
        }
        EditProduct(row[0].ProductId, row[0].ClassId, 1);
    }
    else {
        $.messager.alert("提示信息", "请选择一行商品数据", "info");
    }

}

//删除单条
function form_del(row) {

    row_del(row);
    //$.messager.confirm("提醒信息", "确定要删除该商品信息吗?", function (flag) {
    //    if (flag) {
    //        $.ajax({
    //            url: '/BasicInfo/DelProduct',
    //            data: { ids: row },
    //            success: function (data) {
    //                if (data == "1") {
    //                    $.messager.alert("提示信息", "刪除成功!", "info");
    //                    $("#slist").datagrid("reload");
    //                }
    //                else {
    //                    $.messager.alert("提示信息", "刪除失败,请稍后重试!", "info");
    //                    $("#slist").datagrid("reload");
    //                }
    //            },
    //        });
    //    }
    //});
}

//空白新增
function addNewRow() {
    if ($("#hid_pType").val() == "") {
        $.messager.alert("警告", "请先选择左侧的商品类型再添加本类型的商品", "info");
        return false;
    }
    productEdit();
}

//复制新增
function editAddNewRow() {
    var row = $("#slist").datagrid("getSelections");
    if (row != null && row.length == 1) {
        if (row[0].ProductCode == undefined) {
            $.messager.alert("提醒", "请选择要复制的行", "info");
            return;
        }
        EditProduct(row[0].ProductId, row[0].ClassId, 0);
    }
    else {
        $.messager.alert("提醒", "请选择一行", "info");
    }
}

//删除一条
function row_del(Pids) {
    var ids = [];
    var row = $("#slist").datagrid("getSelections");
    if (row.length > 0) {
        if (row[0].ProductCode == undefined) {
            return;
        }
        $.messager.confirm("提醒信息", "确定要删除吗?", function (flag) {
            if (flag) {
                if (Pids == undefined) {
                    for (var i = 0; i < row.length; i++) {
                        ids.push(row[i].ProductId);
                    }
                }
                else {
                    ids.push(Pids);
                }
                $.ajax({
                    url: '/BasicInfo/DelCheck',
                    anysc: false,
                    data: { ids: ids.join(",") },
                    success: function (data) {
                        if (data == "0") {
                            $.messager.alert("提示信息", "刪除失败,请稍后重试!", "info");
                            $("#slist").datagrid("reload");
                        }
                        else if (data == "") {
                            $.ajax({
                                url: '/BasicInfo/DelProduct',
                                data: { ids: ids.join(",") },
                                success: function (data) {
                                    if (data == "1") {
                                        $.messager.alert("提示信息", "刪除成功!", "info");
                                        $("#slist").datagrid("reload");
                                    }
                                    else {
                                        $.messager.alert("提示信息", "刪除失败,请稍后重试!", "info");
                                        $("#slist").datagrid("reload");
                                    }
                                },
                            });
                        }
                        else {
                            $.messager.confirm("提醒信息", data + "商品已经上线，是否确定删除？", function (flag) {
                                if (flag) {
                                    $.ajax({
                                        url: '/BasicInfo/DelProduct',
                                        data: { ids: ids.join(",") },
                                        success: function (data) {
                                            if (data == "1") {
                                                $.messager.alert("提示信息", "刪除成功!", "info");
                                                $("#slist").datagrid("reload");
                                            }
                                            else {
                                                $.messager.alert("提示信息", "刪除失败,请稍后重试!", "info");
                                                $("#slist").datagrid("reload");
                                            }
                                        },
                                    });
                                }
                            });
                        }
                    },
                })




            }
        });
    }
    else {
        $.messager.alert("提示", "至少选择一项删除", "info");
    }
}

//隐藏一列
function hideRow() {
    $("#slist").datagrid("hideColumn", "text");
}

//加载和显示列配置列表
function Loadcr() {
    $("#cr").datagrid({
        url: '/BasicInfo/GetColumnConfig',
        rownumbers: true,
        loadMsg: '数据加载中。。',
        striped: true,
        remoteSort: false,
        singleSelect: true,
        columns: [[
            {
                field: 'Index',
                title: '',
                checkbox: true
            },
            {
                field: 'DefaultName',
                title: '列名',
                width: 100,
            },
            {
                field: 'DisplayName',
                title: '显示名称',
                width: 100,
            },
            {
                field: 'Visible',
                title: '状态',
                width: 100,
                formatter: function (row, data, index) {
                    if (row == "1") {
                        return "显示";
                    }
                    else {
                        return "隐藏";
                    }
                }
            }
        ]],
    });
}

//列配置
function deployColumn() {
    Loadcr();
    $("#columnRepair").dialog({
        title: '列配置',
        width: 550,
        height: 380,
        modal: true
    });
}

//关闭列配置弹框
function closeForm() {
    $("#columnRepair").dialog("close");
}

//列配置-显示切换
function changeShow() {
    var row = $("#cr").datagrid("getSelected");
    if (row == null) {
        $.messager.alert("提示信息", "请选择一行数据", "info");
        return;
    }
    $.ajax({
        url: '/BasicInfo/UpdateShowState',
        type: 'POST',
        async: false,
        data: { index: row.Index },
        success: function (data) {
            if (data != "") {
                $("#cr").datagrid("reload");
                SearchList();
            }
            else {
                $.messager.alert("提示信息", "显示切换操作出错,请刷新重试!", "info");
            }
        }
    });
    $("#cr").datagrid("reload");
    $("#slist").datagrid("reload");
}

//修改标题
function UpdateHeader() {
    var row = $("#cr").datagrid("getSelected");
    if (row == null) {
        $.messager.alert("提示信息", "请选择一行数据", "info");
        return;
    }
    $("input[name=newHeader]").val(row.DisplayName);
    $("#updateHeader").dialog({
        title: '修改列名称',
        width: 300,
        height: 100,
        modal: true
    });

}

//修改标题  确定
function UpdateHead() {
    if ($("input[name=newHeader]").val() == "") {
        $.messager.alert("提示信息", "新名字不能为空", "into");
        $("input[name=newHeader]").focus();
        return;
    }
    var row = $("#cr").datagrid("getSelected");
    $.ajax({
        url: '/BasicInfo/UpdateColumnName',
        async: false,
        type: 'POST',
        data: { index: row.Index, newName: $("input[name=newHeader]").val() },
        success: function (data) {
            if (data) {
                $("#updateHeader").dialog("close");
                $("input[name=newHeader]").val("");
                $("#cr").datagrid("reload");
                SearchList();
            }
            else {
                $.messager.alert("提示信息", "修改名称出错,请刷新重试!", "info");
            }
        }
    });


    //var row = $("#cr").datagrid("getSelected");
    //var obj = $("#slist").datagrid("getColumnOption", row.columnFiled);
    //obj.title = $("input[name=newHeader]").val();
    //$('#slist').datagrid();
    //$.messager.show({
    //    title: '提示信息',
    //    msg: '修改成功',
    //});
    //Loadcr();
    //$("#updateHeader").dialog("close");
}

//列配置-还原设置
function ResetFormColumn() {
    $.messager.confirm("提示信息", "您确定还原默认设置么?", function (flag) {
        if (flag) {
            $.ajax({
                url: '/BasicInfo/ResetColumnConfig',
                async: false,
                type: 'POST',
                success: function (data) {
                    if (data != "") {
                        $("#cr").datagrid("reload");
                        SearchList();
                    }
                    else {
                        $.messager.alert("提示信息", "还原默认设置失败，请刷新后重试", "info");
                    }
                }
            });
        }
    });

}

//列配置-上移，下移，type=0，下移，type=1上移
function UpDown(type) {
    var row = $("#cr").datagrid("getSelected");
    if (row == null) {
        $.messager.alert("提示信息", "请选择一行数据", "into");
        return;
    }
    if (type == 0)//向下移
    {
        if ($("#cr").datagrid("getRows").length <= (row.Index + 1)) {
            $.messager.alert("提示信息", "已经是最后一个了", "info");
            return;
        }
    }
    else {
        if (row.Index == 0) {
            $.messager.alert("提示信息", "已经是第一个了", "info");
            return;
        }
    }
    $.ajax({
        url: '/BasicInfo/UpdateColumnIndex',
        async: false,
        type: 'POST',
        data: { index: row.Index, stype: type },
        beforeSend: function () {

        },
        success: function (data) {
            if (data) {
                $("#cr").datagrid("reload");
                SearchList();
            }
            else {
                $.messager.alert("提示信息", "修改名称出错,请刷新重试!", "info");
            }
        }
    });
}

//新增，编辑商品信息
function productEdit() {
    if ($("#hid_pType").val() == "") {
        $.messager.alert("警告", "请先选择左侧的商品类型再添加本类型的商品", "info");
        return false;
    }
    ResetForm();
    $("#isProductasc").prop("checked", true);
    GetPTyoeCode($("#isProductasc"));
    $("#productInfo").dialog({
        title: '商品信息-信息框',
        width: 636,
        height: 600,
        modal: true
    });
}

//关闭新增/编辑商品信息
function closePInfo() {
    $("#productInfo").dialog("close");
}

//图片上传
function savePic(obj, index) {
    var imgPath = obj.val();
    if (imgPath == "") {
        return;
    }
    var strExtension = imgPath.substr(imgPath.lastIndexOf('.') + 1);
    if (strExtension != 'jpg' && strExtension != 'gif'
    && strExtension != 'png' && strExtension != 'bmp') {
        $.messager.alert("提示", "图片格式不正确", "info");
        obj.val("");
        return;
    }
    else if (obj[0].files[0].size > 1 * 1024 * 1024)//判断图片大小
    {
        $.messager.alert("提示", "图片大小不超过1M", "info");
        obj.val("");
        return;
    }
    $("#imgform" + index).ajaxSubmit({
        loadingMsg: '正在上传...',
        dataType: 'text', //数据格式为text
        success: function (data) {
            var json = eval("(" + data + ")");
            if (json.success == "1") {
                $("#picUrl" + index).attr("src", json.src);
                $("input[name=picSrc" + index + "]").val(json.src);
                $("input[name=picname" + index + "]").attr("disabled", false);
            }
            else {
                $.messager.alert("警告", "图片上传失败，请重试!", "warning");
            }
        },
        error: function () {
            $.messager.alert("警告", "图片上传过程出错，请重试!", "warning");
        }
    });
}


//删除图片
function delPicture(index) {
    if ($("input[name=picSrc" + index + "]").val() != "") {
        $.post("/BasicInfo/DelPic", { name: $("input[name=picSrc" + index + "]").val() }, function (data) {
            if (data != 1) {
                $.messager.alert("警告", "图片删除出错，请重试!", "warning");
            }
            else {
                $("#picUrl" + index).attr("src", "");
                $("input[name=picSrc" + index + "]").val("");
                $("input[name=file" + index + "]").val("");
                $("input[name=picname" + index + "]").val("");
                $("input[name=picname" + index + "]").attr("disabled", true);
            }
        });
    }

}

//编辑树操作-添加节点
function addNode() {
    var node = $("#pType").etree("getSelected");
    var parentNode = $("#pType").etree("getParent", node.target)
    var text = $("input[name=nName]").val();
    var type = $("input[name=nodetype]:checked").val();
    if (text != "" && $("input[name=nName]").validatebox("isValid")) {
        var data = $("#pType").etree("getChildren", node);
        if (data.length > 0)//有子节点
        {
            var intrue = true;
            for (var i = 0; i < data.length; i++) {
                if (data[i].text == text) {
                    intrue = false;
                    break;
                }
            }
            if (!intrue) {
                $("#errormsg").show();
                $("#errormsg").text("该节点名称已存在");
                $("input[name=nName]").val("");
                $("input[name=nName]").focus();
                return false;
            }
            else {
                $("#errormsg").hide();
            }
        }
        //访问服务器添加节点
        var isadd = true;
        var pid = '';
        if (type == "0") {
            pid = parentNode.id;
        }
        else {
            pid = node.id;
        }
        $.ajax({
            url: '/BasicInfo/TypeCreate',
            async: false,
            data: { parentId: pid, text: text },
            dataType: 'text',
            success: function (data) {
                if (parseInt(data) <= 0) {
                    isadd = false;
                }
                pid = data;
            }
        });
        if (isadd) {
            if (type == "0") {
                $("#pType").etree("insert", {
                    after: node.target,
                    data: {
                        id: pid,
                        text: text,
                        state: 'open'
                    }

                });
            }
            else {
                $("#pType").etree("append", {
                    parent: node.target,
                    data: {
                        id: pid,
                        text: text,
                        state: "open"
                    }

                });

                //$("#pType").etree("create");
            }
            $("#treeEdit").hide();
            $("input[name=nName]").val("");
        }
        else {
            $.messager.alert("警告", "添加节点失败,请重试!", "warning");
        }

    }
    else {
        $("input[name=nName]").focus();
    }
}
//编辑树操作-修改节点
function editNode() {
    var node = $("#pType").etree("getSelected");
    if (node != null) {
        $("#pType").etree("edit");
    }
    else {
        $.messager.alert("提醒", "请选择要修改的节点", "info");
    }

}
//编辑树操作-删除节点
function delNode() {
    var node = $("#pType").etree("getSelected");
    if (node != null) {
        $.ajax({
            url: '/BasicInfo/TypeDel',
            dataType: 'text',
            data: { id: node.id },
            success: function (data) {
                if (data == "1") {
                    $.messager.alert("警告", "删除出错,请重试!", "warning");
                }
                else {
                    $("#pType").etree("remove", node.target);
                }
            }
        });


    }
    else {
        $.messager.alert("提醒", "请选择要修改的节点", "info");
    }
}

//类别定位
function DixPosition() {
    var name = $("#positionname").val();
    if (name.trim() != "") {
        var id = '';
        $.ajax({
            url: '/BasicInfo/Position',
            data: { name: name },
            dataType: 'text',
            success: function (data) {
                if (data != '') {
                    var row = $("#pType").etree("find", data);
                    var parentNode = $("#pType").etree("getParent", row.target);
                    if (parentNode != null) {
                        $("#pType").etree("expand", parentNode.target);
                    }
                    $("#pType").etree("select", row.target);
                }
                else {
                    $.messager.alert("提示信息", "无法定位分类,请确认分类名或拼音码是否正确", "info");
                }
            }
        });

    }
}


//新增商品保存
function productAddSave(type) {
    if ($("#hid_pType").val() == "") {
        $.messager.alert("警告", "请先选择左侧的商品类型再添加本类型的商品", "info");
        return false;
    }
    var productid = ($("#hid_productId").val() == "" ? "0" : $("#hid_productId").val());
    var ptype = $("#hid_pType").val();
    var pname = $("input[name=pname]").val();
    var pid = $("input[name=pcode]").val();
    var pidasc = $("input[name=isProductasc]:checked") ? "1" : "0";
    var pformat = $("input[name=pformat]").val();
    var pxh = $("input[name=pxh]").val();
    var phome = $("input[name=phome]").val();
    var punit = $("input[name=punit]").val();
    var pbarcode = $("input[name=pbarcode]").val();
    var pkg = $("input[name=pkg]").val() == "" ? "0" : $("input[name=pkg]").val();
    if (!$("input[name=pname]").validatebox("isValid")) {
        $("input[name=pname]").focus();
        return false;
    }
    else if (!$("input[name=pcode]").validatebox("isValid")) {
        $("input[name=pcode]").focus();
        return false;
    }
    else if (!$("input[name=phome]").validatebox("isValid")) {
        $("input[name=phome]").focus();
        return false;
    }

    //预售价格
    var pprice1 = $("input[name=pprice1]").val() == "" ? "0" : $("input[name=pprice1]").val();
    var pprice2 = $("input[name=pprice2]").val() == "" ? "0" : $("input[name=pprice2]").val();
    var pprice3 = $("input[name=pprice3]").val() == "" ? "0" : $("input[name=pprice3]").val();
    var pprice4 = $("input[name=pprice4]").val() == "" ? "0" : $("input[name=pprice4]").val();
    var pprice5 = $("input[name=pprice5]").val() == "" ? "0" : $("input[name=pprice5]").val();
    var pprice6 = $("input[name=pprice6]").val() == "" ? "0" : $("input[name=pprice6]").val();
    var pprice7 = $("input[name=pprice7]").val() == "" ? "0" : $("input[name=pprice7]").val();
    var pprice8 = $("input[name=pprice8]").val() == "" ? "0" : $("input[name=pprice8]").val();
    //图片
    var picurl1 = $("input[name=picSrc1]").val();
    var picname1 = $("input[name=picname1]").val();
    var picurl2 = $("input[name=picSrc2]").val();
    var picname2 = $("input[name=picname2]").val();
    var picurl3 = $("input[name=picSrc3]").val();
    var picname3 = $("input[name=picname3]").val();
    //简介
    var brief = $("#brochure").val();
    var json = "{\"productInfo\":{\"ProductId\":\"" + productid + "\",\"PinyinCode\":\"" + $("input[name=pPym]").val() + "\",";
    json += "\"ClassId\":\"" + ptype + "\",\"ProductFullName\":\"" + pname + "\",\"ProductCode\":\"" + pid + "\",\"pidasc\":\"" + pidasc + "\",\"ProductSpec\":\"" + pformat + "\",\"ProductModel\":\"" + pxh + "\",";
    json += "\"Place\":\"" + phome + "\",\"ProductUnit\":\"" + punit + "\",\"BarCode\":\"" + pbarcode + "\",\"Weight\":\"" + pkg + "\",\"Price1\":\"" + pprice1 + "\",\"Price2\":\"" + pprice2 + "\",";
    json += "\"Price3\":\"" + pprice3 + "\",\"Price4\":\"" + pprice4 + "\",\"Price5\":\"" + pprice5 + "\",\"Price6\":\"" + pprice6 + "\",\"Price7\":\"" + pprice7 + "\",\"Price8\":\"" + pprice8 + "\",";
    json += "\"Summary\":\"" + brief + "\"";
    json += "}";
    json += ",\"productPic\":[";
    if (picurl1 != "" || picurl2 != "" || picurl3 != "") {
        if (picurl1 != "") {
            json += " {\"Format\":\"" + picurl1 + "\",\"Name\":\"" + picname1 + "\",\"PicId\":\"" + ($("input[name=picId1]").val() == "" ? "0" : $("input[name=picId1]").val()) + "\"},";
        }
        if (picurl2 != "") {
            json += " {\"Format\":\"" + picurl2 + "\",\"Name\":\"" + picname2 + "\",\"PicId\":\"" + ($("input[name=picId2]").val() == "" ? "0" : $("input[name=picId2]").val()) + "\"},";
        }
        if (picurl3 != "") {
            json += " {\"Format\":\"" + picurl3 + "\",\"Name\":\"" + picname3 + "\",\"PicId\":\"" + ($("input[name=picId3]").val() == "" ? "0" : $("input[name=picId3]").val()) + "\"},";
        }
        if (json.substring(json.length - 1, json.length) == ",") {
            json = json.substring(0, json.length - 1);
        }
    }
    json += "]";
    json += "}";
    $.ajax({
        url: '/BasicInfo/ProductAdd',
        type: 'POST',
        data: { json: json },
        success: function (data) {
            if (data == "1")//成功
            {
                $.messager.show({
                    title: '提示信息',
                    msg: '保存成功！'
                });
                if (type == 1)//新增并保存
                {
                    ResetForm();
                    GetPTyoeCode($("#isProductasc"));
                    //$.messager.alert("提示信息", "保存成功！", "info");
                }
                else if (type == 0)//保存并关闭
                {
                    ResetForm();
                    //$.messager.alert("提示信息", "保存成功！", "info");
                    $("#productInfo").dialog("close");
                }
                $("#slist").datagrid("reload");
            }
            else if (data == "0") {
                $.messager.alert("提示信息", "获取参数出错,请刷新页面重试", "info");
            }
            else if (data == "2") {
                $.messager.alert("提示信息", "图片保存失败,请重新上传图片", "info");
            }
            else if (data == "3") {
                $.messager.alert("提示信息", "商品信息保存失败，请重试", "info");
            }
            else if (data == "4") {
                $.messager.alert("提示信息", "商品名称已存在", "info");
            }
            else if (data == "5") {
                $.messager.alert("提示信息", "商品编号已存在", "info");
            }
        }
    });


}

//重置新增商品信息
function ResetForm() {
    $("input[name=pname]").val("");
    $("input[name=pcode]").val("");
    $("input[name=pformat]").val("");
    $("input[name=pxh]").val("");
    $("input[name=phome]").val("");
    $("input[name=punit]").val("");
    $("input[name=pbarcode]").val("");
    $("input[name=picSrc1]").val("");
    $("input[name=picname1]").val("");
    $("input[name=picSrc2]").val("");
    $("input[name=picname2]").val("");
    $("input[name=picSrc3]").val("");
    $("input[name=picname3]").val("");
    $("input[name=file1]").val("");
    $("input[name=file2]").val("");
    $("input[name=file3]").val("");
    $("#picUrl1").attr("src", "");
    $("#picUrl2").attr("src", "");
    $("#picUrl3").attr("src", "");
    $("#brochure").val("")

    $("#pkg").numberspinner("setValue", 1);

    $("#isProductasc").prop("checked", true);

    $("#hid_productId").val("");

    $("input[name=picId1]").val("");
    $("input[name=picId2]").val("");
    $("input[name=picId3]").val("");

    //重置预售价格
    $("#pprice1").numberspinner("setValue", 0);
    $("#pprice2").numberspinner("setValue", 0);
    $("#pprice3").numberspinner("setValue", 0);
    $("#pprice4").numberspinner("setValue", 0);
    $("#pprice5").numberspinner("setValue", 0);
    $("#pprice6").numberspinner("setValue", 0);
    $("#pprice7").numberspinner("setValue", 0);
    $("#pprice8").numberspinner("setValue", 0);

    $("#saveandadd").show();

    $("#pinfo_SelectProd").show();

}
//编辑/复制新增，type：1 编辑，0复制新增
function EditProduct(pid, pType, type) {
    if (pType != undefined && pType != null) {
        var row = $("#pType").etree("find", pType);
        var parentNode = $("#pType").etree("getParent", row.target);
        if (parentNode != null) {
            $("#pType").etree("expand", parentNode.target);
        }
        $("#pType").etree("select", row.target);
    }
    else {
        $.messager.alert("警告", "获取商品类型出错", "info");
        return false;
    }
    ResetForm();
    $.ajax({
        url: '/BasicInfo/GetProductInfo',
        type: 'POST',
        data: { id: pid },
        success: function (data) {
            var json = eval("(" + data + ")");
            var data = json.productInfo[0];
            var picInfo = json.pics;
            //基本信息
            if (type == 1) {
                $("#hid_productId").val(pid);
                $("input[name=pcode]").val(data.ProductCode);
                $("#isProductasc").prop("checked", true);
                $("#saveandadd").hide();
                $("#pinfo_SelectProd").hide();
            }
            else {
                $("#hid_productId").val("");
                $("input[name=pcode]").val("");
                $("#isProductasc").prop("checked", true);
                GetPTyoeCode($("#isProductasc"));
                $("#pinfo_SelectProd").show();
            }
            $("#editPType").text($("#productType").text());
            $("input[name=pname]").val(data.ProductFullName);
            $("input[name=pformat]").val(data.ProductSpec);
            $("input[name=pxh]").val(data.ProductModel);
            $("input[name=phome]").val(data.Place);
            $("input[name=punit]").val(data.ProductUnit);
            $("input[name=pbarcode]").val(data.BarCode);
            $("#brochure").val(data.Summary);
            $("input[name=pPym]").val(data.PinyinCode);

            $("#pkg").numberspinner("setValue", data.Weight);
            //售价
            $("#pprice1").numberspinner("setValue", data.Price1);
            $("#pprice2").numberspinner("setValue", data.Price2);
            $("#pprice3").numberspinner("setValue", data.Price3);
            $("#pprice4").numberspinner("setValue", data.Price4);
            $("#pprice5").numberspinner("setValue", data.Price5);
            $("#pprice6").numberspinner("setValue", data.Price6);
            $("#pprice7").numberspinner("setValue", data.Price7);
            $("#pprice8").numberspinner("setValue", data.Price8);
            //图片信息
            for (var i = 0; i < picInfo.length; i++) {

                $("input[name=picname" + (i + 1) + "]").attr("disabled", false);
                $("input[name=picname" + (i + 1) + "]").val(picInfo[i].picName);
                //$("input[name=file" + (i + 1) + "]").val(picInfo[i].picUrl);
                $("input[name=picSrc" + (i + 1) + "]").val(picInfo[i].picUrl);
                $("#picUrl" + (i + 1)).attr("src", picInfo[i].picUrl);
                if (type == 1) {
                    $("input[name=picId" + (i + 1) + "]").val(picInfo[i].picId);
                }
                else {
                    $("input[name=picId" + (i + 1) + "]").val("");
                }
            }
            $("#productInfo").dialog({
                title: '商品信息-信息框',
                width: 636,
                height: 600,
                modal: true
            });


        },
    });


}

//编号自增点击事件
function GetPTyoeCode(obj) {
    if (obj.is(":checked")) {
        $.ajax({
            url: '/BasicInfo/GetProductCode',
            type: 'POST',
            success: function (data) {
                if (data != "") {
                    $("input[name=pcode]").val(data);
                }
                else {
                    $.messager.alert("提醒", "自动获取商品code失败，请联系管理员", "info");
                }
            }
        });
    }
}


function ChangeSize() {
    var height = document.documentElement.clientHeight - 280;
    if (height > 305) {
        $("#pList").height(height);
        $("#ptyleList").height(height);
    }

}
