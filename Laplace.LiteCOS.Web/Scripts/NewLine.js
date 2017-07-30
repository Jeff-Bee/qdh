$(function () {
    $("#NL_ReceiveType").combobox({
        url: '/BasicInfo/GetTerminal',
        valueField: 'id',
        textField: 'text',
    });
    $("#NL_ReceiveType").combobox("setValue", 0);
    $("#NL_PushedStartTime").datebox({
        currentText: '今天',
        formatter: function (date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return y + "-" + m + "-" + d;

        },
        onSelect: function (data) {

        },

    });
    var time = new Date();
    $("#NL_PushedStartTime").datebox("setValue", time.toLocaleTimeString());
    $("#NL_PushedEndTime").datebox({
        currentText: '今天',
        formatter: function (date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return y + "-" + m + "-" + d;

        },
        onSelect: function (data) {

        },

    });
    $("#NL_PushedEndTime").datebox("setValue", '2100-1-1');


    $("#NL_ConnectionProduct").combobox({
        url: '/BasicInfo/GetPIPList',
        textField: 'ProductFullName',
        valueField: 'ProductId',
        editable:false,
        hasDownArrow: false,
        icons: [{
            iconCls: 'combo-more',
            handler: function () {
                NL_SelectProduct();
            }
        }]
    });

    $("#NL_Search").combobox({
        url: '/BasicInfo/SearchAreaCondition',
        valueField: 'id',
        textField: 'text',
        width: 100,
    });
    $("#NL_Search").combobox("setValue", 0);

    $("#NL_List").datagrid({
        url: '/BasicInfo/GetNLModelList',
        singleSelect: true,
        rownumbers: true,
        striped: true,
        loadMsg: '数据加载中。。',
        remoteSort: false,
        fitColumns: true,
        fixed: true,
        pagination: true,
        selectOnCheck: false,//选择行则选择checkbox  
        checkOnSelect: false,//选择checkbox则选择行  
        columns: [[
            {
                title: '发布<input type=\"checkbox\" id=\"NL_ck_all\" value="" style="vertical-align:middle" checked=\"checked\" />',
                field: 'IsUsed',
                width: 40,
                align: 'right',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        if (row.IsUsed != undefined && row.IsUsed == "1") {
                            $("#NL_SelectRmark").val(row.Title);
                            return "<input type=\"checkbox\" class=\"NL_ck_item\" value=\"" + row.Id + "\" onclick=\"NL_UpdateIsUsed("+row.Id+")\"  checked=\"checked\" />";
                        }
                        else {
                            return "<input type=\"checkbox\" class=\"NL_ck_item\" value=\"" + row.Id + "\" onclick=\"NL_UpdateIsUsed("+row.Id+")\" />";
                        }
                    }
                }
            },
            {
                field: 'Title',
                title: '标题',
                width: 100,
               
            },
            {
                field: 'Content',
                title: '内容',
                width: 120,
            },
             {
                 field: 'Notes',
                 title: '关联商品',
                 width: 80,
                 formatter: function (value) {
                     if (value != undefined && value != "") {
                         return value;
                     }
                     else if (value != undefined) {
                         return "--";
                     }
                 }
             },
            {
                field: 'Link',
                title: '关联链接',
                width: 80,
                formatter: function (value) {
                    if (value != undefined && value != "") {
                        return value;
                    }
                    else if (value != undefined) {
                        return "--";
                    }
                }

            },
            {
                field: 'RemainTime',
                title: '自动关闭时间',
                width: 40,
                formatter: function (value) {
                    
                    if (value != undefined && value != "") {
                        return value;
                    }
                    else if (value != undefined) {
                        return "--";
                    }
                }

            },

        ]],
        onLoadSuccess: function () {
            NL_FillPiRows();
            $("#NL_SelectRmark").val("");
            CheckSelected();
        },
        onSelect: function (rowIndex, rowData) {
            if (rowData != null && rowData.Title != undefined) {
                $("#NL_SelectRmark").val(rowData.Title);
            }
        },
        //onUncheck: function (rowIndex, rowData) {
        //if (rowData != null && rowData.Id != undefined)
        //{
        //    NL_UpdateIsUsed(rowData.Id);
        //}
        //}
    });
    //促销信息全选
    $("#NL_ck_all").click(function () {
        if ($("#NL_ck_all").prop("checked")) {
            $(".NL_ck_item").each(function (i, e) {
                if ($(this).val() != undefined) {
                    if (!$(this).prop("checked")) {
                        $(this).prop("checked", true);
                        NL_UpdateIsUsed($(this).val());
                    }

                }
            });
        }
        else {
            $(".NL_ck_item").each(function (i, e) {
                if ($(this).val() != undefined) {
                    if ($(this).prop("checked")) {
                        $(this).prop("checked", false);
                        NL_UpdateIsUsed($(this).val());
                    }
                }
            });
        }
    });

    NLChangeSize();
    
})

//更新促销信息的启用状态
function NL_UpdateIsUsed(Id) {
    $.ajax({
        url: '/BasicInfo/NL_UpdateIsUsed',
        type: 'POST',
        data: { id: Id },
        success: function (data) {
            if (data != "1") {
                $.messager.alert("警告信息", "更改启用状态失败!", "info");
            }
            else {
                CheckSelected();
            }
        }
    });
}

//判断是否全部选中
function CheckSelected()
{
    if ($(".NL_ck_item").length == $(".NL_ck_item:checked").length) {
        if (!$("#NL_ck_all").prop("checked")) {
            $("#NL_ck_all").prop("checked", true);
        }
    }
    else {
        if ($("#NL_ck_all").prop("checked")) {
            $("#NL_ck_all").prop("checked", false);
        }
    }
}

// 当不够十行时补充十行空数据
function NL_FillPiRows() {
    var data = $('#NL_List').datagrid('getData');
    var pageopt = $('#NL_List').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#NL_List').datagrid('appendRow', {});
        }
    }
}

//打开新增对话框
function AddOrEditNL() {
    $("#addNL").show();
    ResetNLForm();
    OpenNLDialog();
}

//打开新增/编辑对话框
function OpenNLDialog() {
    $("#NL_AddInfo").dialog({
        title: '图片轮播',
        width: 400,
        height: 475,
        modal: true
    });
}

//编辑/修改  type=0:  编辑  type=1:修改
function EditFormNL(type) {
    ResetNLForm();
    var rows = $("#NL_List").datagrid("getSelections");
    if (rows != null && rows.length == 1 && rows[0].ProductId != undefined) {
        var row = rows[0];
        $("#NL_Title").val(row.Title);
        $("#NL_content").val(row.Content);
        
        if (row.Picture != undefined && row.Picture != "") {
            $("#NL_picId1").val(row.Picture);
        }
        if (row.PictureUrl != undefined && row.PictureUrl != "") {
            $("#NL_picUrl1").attr("src", row.PictureUrl);
        }
        if (row.Id != undefined && row.Id != "") {
            if (type == 1) {
                $("#NL_hid_Id").val(row.Id);
                $("#addNL").hide();
            }
            else {
                $("#NL_hid_Id").val("");
                $("#addNL").show();
            }
        }
        else {
            $("#NL_hid_Id").val("");
        }

        if (row.ProductId != undefined && row.ProductId!="")
        {
            $("#NL_ConnectionProduct").combobox("setValue", row.ProductId);
            $("#nl_productType").prop("checked",true);
        }
        else if (row.Link != undefined && row.Link != "") {
            $("#NL_Url").val(row.Link);
            $("#nl_linkType").prop("checked", true);
        }
        else if (row.RemainTime != undefined && row.RemainTime != "0") {
            $("#nl_closeTime").val(row.RemainTime);
            $("#nl_autoColse").prop("checked", true);
        }

        $("#NL_ReceiveType").combobox("setValue", row.Receiver);
        
      
        $("#NL_PushedStartTime").datebox("setValue", row.StartTime);
        $("#NL_PushedEndTime").datebox("setValue", row.EndTime);
        $("#NL_Notes").val(row.Notes);

        if (row.Config != undefined && row.Config != "") {
            $("#NL_picUrl1").attr("src", row.Config);
            $("#NL_picSrc1").val(row.Config);
            $("#NL_picId1").val(row.LMan);
        }

        OpenNLDialog();
    }
    else if (rows.length > 1) {
        $.messager.alert("提示信息", "请选择一条图片轮播信息", "info");
    }
    else {
        $.messager.alert("提示信息", "请先选择一条图片轮播信息", "info");
    }
}

//删除
function DeleteNL() {
    $.messager.confirm("提示信息", "您确定要删除该图片轮播信息么?", function (flag) {
        if (flag) {
            var row = $("#NL_List").datagrid("getSelections");
            if (row != null && row.length == 1 && row[0].Id != undefined) {
                $.ajax({
                    url: '/BasicInfo/NL_DelSave',
                    type: 'POST',
                    data: { id: row[0].Id, },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '删除成功'
                            });
                            $("#NL_List").datagrid("reload");
                        }
                        else if (data == "0") {
                            $.messager.alert("提示信息", "删除失败", "info");
                        }
                        else {
                            $.messager.alert("提示信息", "获取参数错误", "info");
                        }
                    }
                });
            }
            else if (row.length > 1) {
                $.messager.alert("提示信息", "请选择一条图片轮播信息", "info");
            }
            else {
                $.messager.alert("提示信息", "请先选择一条图片轮播信息", "info");
            }
        }
    });

}

//关闭新增对话框
function addNLClose() {
    $("#NL_AddInfo").dialog("close");
}

//选择商品对话框
function NL_SelectProduct() {
    $("#NL_SelectProduct").dialog({
        title: '选择关联商品',
        width: 415,
        height: 455,
        modal: true
    });
    $("#NL_SelectProduct_list").datagrid({
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
            NL_FillInterRows();
        }
    });
    NL_searchProduct();

}



//当不够十行时补充十行空数据
function NL_FillInterRows() {
    var data = $('#NL_SelectProduct_list').datagrid('getData');
    var pageopt = $('#NL_SelectProduct_list').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#NL_SelectProduct_list').datagrid('appendRow', {});
        }
    }
}

//关闭选择商品对话框
function NL_SelectProduct_Close() {
    $("#NL_SelectProduct").dialog("close");
}

//选择商品查询
function NL_searchProduct() {
    $("#NL_SelectProduct_list").datagrid("load", {
        type: $("#NL_Search").combobox("getValue"),
        content: $("#NL_Content").val()
    });
}
//选择商品-选中
function NL_SProduct() {
    var row = $("#NL_SelectProduct_list").datagrid("getSelected");
    if (row != null && row.ProductId != undefined && row.ProductId != "") {
        $("#NL_ConnectionProduct").combobox("setValue", row.ProductId);
        NL_SelectProduct_Close();
    }
    else {
        $.messager.alert("提示信息", "请先选择一条商品", "info");
    }
}

//图片上传
function NL_savePic(obj, index) {
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
    $("#NL_picId1").val("");
    $("#NL_imgform" + index).ajaxSubmit({
        loadingMsg: '正在上传...',
        dataType: 'text', //数据格式为text
        success: function (data) {
            var json = eval("(" + data + ")");
            if (json.success == "1") {
                $("#NL_picUrl" + index).attr("src", json.src);
                $("input[name=NL_picSrc" + index + "]").val(json.src);
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
function NL_delPicture(index) {
    if ($("#NL_picSrc1").val() == "") {
        return;
    }
    if($("#NL_picId1").val()!="")
    {
        $("#NL_picUrl" + index).attr("src", "");
        $("#NL_picId1").val("");
        $("input[name=NL_picSrc" + index + "]").val("");
        $("input[name=NL_file" + index + "]").val("");
        return;
    }
    if ($("input[name=NL_picUrl" + index + "]").val() != "") {
        $.post("/BasicInfo/DelPic", { name: $("input[name=NL_picSrc" + index + "]").val() }, function (data) {
            if (data != 1) {
                $.messager.alert("警告", "图片删除出错，请重试!", "warning");
            }
            else {
                $("#NL_picUrl" + index).attr("src", "");
                $("#NL_picId1").val("");
                $("input[name=NL_picSrc" + index + "]").val("");
                $("input[name=NL_file" + index + "]").val("");
            }
        });
    }

}

//新增/编辑促销信息保存,type=1  保存并新增,type=0  保存并关闭
function NL_AddSave(type) {
    if ($("#NL_picSrc1").val() == "") {
        $.messager.alert("提示信息", "图片不能为空", "info");
        return;
    }
    else if ($("#NL_Title").val() == "") {
        $.messager.alert("提示信息", "标题不能为空", "info");
        return;
    }
    else if ($("#NL_content").val() == "") {
        $.messager.alert("提示信息", "内容不能为空", "info");
        return;
    }
    else if ($("#NL_picSrc1").val() == "") {
        $.messager.alert("提示信息", "图片不能为空", "info");
        return;
    }
    else if ($("#nl_linkType").prop("checked")&&$("#NL_Url").val() == "") {
        $.messager.alert("提示信息", "关联链接能为空", "info");
        return;
    }
    else if ($("#nl_productType").prop("checked")&&$("#NL_ConnectionProduct").combobox("getValue") == "") {
        $.messager.alert("提示信息", "关联商品能为空", "info");
        return;
    }
    else if ($("#nl_autoColse").prop("checked")&&$("#nl_closeTime").val() == "") {
        $.messager.alert("提示信息", "自动关闭时间不能为了", "info");
        return;
    }
    else {
        var json = "{";
        json += "\"Title\":\"" + $("#NL_Title").val() + "\",\"Content\":\"" + $("#NL_content").val() + "\",\"Receiver\":\"" + $("#NL_ReceiveType").combobox("getValue") + "\",";
        if ($("#nl_productType").prop("checked"))
        {
            json+="\"ProductId\":\"" + $("#NL_ConnectionProduct").combobox("getValue") + "\",";
        }
        if ($("#nl_autoColse").prop("checked")) {
            json += "\"RemainTime\":\"" + $("#nl_closeTime").numberspinner("getValue") + "\",";
        }
        if ($("#nl_linkType").prop("checked")) {
            json += "\"Link\":\"" + $("#NL_Url").val() + "\",";
        }
       
        json += "\"StartTime\":\"" + $("#NL_PushedStartTime").datebox("getValue") + "\",\"EndTime\":\"" + $("#NL_PushedEndTime").datebox("getValue") + "\",";
        json += "\"Notes\":\"" + $("#NL_Notes").val() + "\",\"Picture\":\"" + ($("#NL_picId1").val() == "" ? 0 : $("#NL_picId1").val()) + "\",\"Id\":\"" + ($("#NL_hid_Id").val() == "" ? 0 : $("#NL_hid_Id").val()) + "\"";
        json += "}";
        console.log(json);
        $.ajax({
            url: '/BasicInfo/NL_AddSave',
            type: 'POST',
            data: { str: json, picSrc: $("#NL_picSrc1").val() },
            success: function (data) {
                if (data == "1") {
                    $.messager.show({
                        title: '提示',
                        msg: '保存成功!',
                    });

                    if (type == 1) {//并新增
                        ResetForm();
                    }
                    else {//并关闭
                        addNLClose();
                        ResetNLForm();
                        $("#addNL").show();
                    }
                    $("#NL_List").datagrid("reload");
                   

                }
                else if (data == "0") {
                    $.messager.alert("提示信息", "保存失败!", "info");
                }
                else if (data == "2") {
                    $.messager.alert("提示信息", "标题重复!", "info");
                }
                else if (data == "3") {
                    $.messager.alert("提示信息", "图片上传失败!", "info");
                }
                else {
                    $.messager.alert("提示信息", "获取参数失败!", "info");
                }
            }
        });

    }
}

//重置表单
function ResetNLForm() {
    $("#NL_hid_AdsId").val("");
    $("#NL_Title").val("");
    $("#NL_content").val("");
    $("#NL_Url").val("");
    var time = new Date();
    $("#NL_ReceiveType").combobox("setValue", 0);
    $("#NL_ConnectionProduct").combobox("setValue", "");
    $("#NL_AlertSpan").val(10);
    $("#NL_StaySpan").val(5);
    $("#NL_PushedStartTime").datebox("setValue", time.toLocaleTimeString());
    $("#NL_PushedEndTime").datebox("setValue", '2100-1-1');
    $("#NL_Notes").val("");
    //图片重置
    $("#NL_picUrl1").attr("src", "");
    $("#NL_picSrc1").val("");
    $("#NL_picId1").val("");
    $("input[name=NL_file1]").val("");
}

function NLChangeSize() {
    var height = document.documentElement.clientHeight - 255;
    if (height > 400) {
        $("#NL_CXList").height(height);
    }

}