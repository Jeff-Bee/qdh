$(function () {
    $("#PI_ReceiveType").combobox({
        url: '/BasicInfo/GetTerminal',
        valueField: 'id',
        textField: 'text',
    });
    $("#PI_ReceiveType").combobox("setValue", 0);
    $("#PI_PushedStartTime").datebox({
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
    $("#PI_PushedStartTime").datebox("setValue", time.toLocaleTimeString());
    $("#PI_PushedEndTime").datebox({
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
    $("#PI_PushedEndTime").datebox("setValue", '2100-1-1');

    $("#PI_ConnectionProduct").combobox({
        url: '/BasicInfo/GetPIPList',
        textField: 'ProductFullName',
        valueField: 'ProductId',
        editable: false,
        hasDownArrow: false,
        icons: [{
            iconCls: 'combo-more',
            handler: function () {
                PI_SelectProduct();
            }
        }]
    });

    $("#PI_Search").combobox({
        url: '/BasicInfo/SearchAreaCondition',
        valueField: 'id',
        textField: 'text',
        width: 100,
    });
    $("#PI_Search").combobox("setValue", 0);

    $("#CXList").datagrid({
        url: '/BasicInfo/GetPIModelList',
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
                title: '选中',
                field: 'IsUsed',
                width: 30,
                align: 'right',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        if (row.IsUsed != undefined && row.IsUsed == "1") {
                            $("#PI_SelectRmark").val(row.Title);
                            return "<input type=\"radio\" name=\"rd_item\" class=\"pi_ck_item\" value=\"" + row.AdsId + "\" onclick=\"UpdateIsUsed($(this),'" + row.Title + "')\"  checked=\"checked\" />";
                        }
                        else {
                            return "<input type=\"radio\" name=\"rd_item\" class=\"pi_ck_item\" value=\"" + row.AdsId + "\" onclick=\"UpdateIsUsed($(this),'" + row.Title + "')\" />";
                        }
                    }
                }
            },
            {
                field: 'Title',
                title: '标题',
                width: 120,
               
            },
            {
                field: 'Content',
                title: '内容',
                width: 180,
            },
             {
                 field: 'Notes',
                 title: '关联商品',
                 width: 150,
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
                width: 100,
                formatter: function (value) {
                    if (value != undefined && value != "")
                    {
                        return value;
                    }
                    else if (value != undefined)
                    {
                        return "--";
                    }
                }
            },
            //{
            //    field: 'Receiver',
            //    title: '接收终端',
            //    width: 50,
            //    formatter: function (value) {
            //        if (value != undefined) {
            //            switch (value) {
            //                default:
            //                case 0:
            //                    return "全部";
            //                    break;
            //                case 1:
            //                    return "微信";
            //                    break;
            //                case 2:
            //                    return "手机";
            //                    break;
            //                case 3:
            //                    return "APP";
            //                    break;
            //            }
            //        }
            //    }
            //},
            //{
            //    field: 'PopTime',
            //    title: '弹出时间',
            //    width: 50,

            //},
            {
                field: 'RemainTime',
                title: '自动关闭时间',
                width: 50,
                formatter: function (value) {
                    if (value != undefined && value != "" && value != "0") {
                        return value;
                    }
                    else if (value != undefined) {
                        return "--";
                    }
                }
            },
             {
                 field: 'StartTime',
                 title: '开始时间',
                 width: 100,
                 formatter: function (value) {
                     if (value != undefined) {
                         return GetDate(value);
                     }
                 }
             },
              {
                  field: 'EndTime',
                  title: '结束时间',
                  width: 100,
                  formatter: function (value) {
                      if (value != undefined) {
                          return GetDate(value);
                      }
                  }
              },

        ]],
        onLoadSuccess: function () {
            PI_FillPiRows();
        },
        //onCheck: function (rowIndex, rowData) {
        //    if (rowData != null && rowData.AdsId != undefined)
        //    {
        //        var type = $(".pi_ck_item:eq(" + rowIndex + ")").prop("checked") ? 1 : 0;
        //        UpdateIsUsed(rowData.AdsId, type);
        //    }
        //},
        //onUncheck: function (rowIndex, rowData) {
        //if (rowData != null && rowData.AdsId != undefined)
        //{
        //    var type = $(".pi_ck_item:eq(" + rowIndex + ")").prop("checked") ? 1 : 0;
        //    UpdateIsUsed(rowData.AdsId, type);
        //}
    //}
    });
    //消息推送全选
    $("#pi_ck_all").click(function () {
        if ($("#pi_ck_all").prop("checked")) {
            $("#pi_ck_all").prop("checked", true);
            $(".pi_ck_item").each(function (i, e) {
                if ($(this).val() != undefined) {
                    $(this).prop("checked", true);
                }
            });
        }
        else {
            $(".pi_ck_item").each(function (i, e) {
                $(this).prop("checked", false);
            });
        }
    });


    PIChangeSize();
})

//更新消息推送的启用状态
function UpdateIsUsed(obj,title)
{
    $("#PI_SelectRmark").val(title);
    var adsId = obj.val();
    $.ajax({
        url: '/BasicInfo/PI_UpdateIsUsed',
        type: 'POST',
        data: { id: adsId },
        success: function (data) {
            if (data != "1")
            {
                $.messager.alert("警告信息","更改启用状态失败!","info");
            }
        }
    });
}

// 当不够十行时补充十行空数据
function PI_FillPiRows() {
    var data = $('#CXList').datagrid('getData');
    var pageopt = $('#CXList').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#CXList').datagrid('appendRow', {});
        }
    }
}

//打开新增对话框
function AddOrEditPI() {
    $("#addPI").show();
    ResetForm();
    OpenPIDialog();
}

//打开新增/编辑对话框
function OpenPIDialog() {
    $("#PI_AddInfo").dialog({
        title: '消息推送',
        width: 415,
        height: 510,
        modal: true
    });
}

//编辑/修改  type=0:  编辑  type=1:修改
function EditFormPI(type) {
    ResetForm();
    var rows = $("#CXList").datagrid("getSelections");
    if (rows != null && rows.length == 1 && rows[0].ProductId != undefined) {
        var row = rows[0];
        $("#PI_Title").val(row.Title);
        $("#PI_content").val(row.Content);
        
        if (row.Picture != undefined && row.Picture != "") {
            $("#pi_picId1").val(row.Picture);
        }
        if (row.PictureUrl != undefined && row.PictureUrl != "") {
            $("#pi_picUrl1").attr("src", row.PictureUrl);
        }
        if (row.AdsId != undefined && row.AdsId != "") {
            if (type == 1) {
                $("#pi_hid_AdsId").val(row.AdsId);
                $("#addPI").hide();
            }
            else {
                $("#pi_hid_AdsId").val("");
                $("#addPI").show();
            }
        }
        else {
            $("#pi_hid_AdsId").val("");
        }

        $("#PI_ReceiveType").combobox("setValue", row.Receiver);
        if (row.ProductId != undefined&&row.ProductId != 0)
        {
            $("#pi_productType").prop("checked", true);
            $("#PI_ConnectionProduct").combobox("setValue", row.ProductId);
        }
        else if (row.Link != undefined && row.Link != "")
        {
            $("#pi_linkType").prop("checked", true);
            $("#PI_Url").val(row.Link);
        }
        else if (row.RemainTime != undefined && row.RemainTime != "0") {
            $("#pi_autoColse").prop("checked", true);
            $("#PI_StaySpan").val(row.RemainTime);
        }

       
        $("#PI_AlertSpan").val(row.PopTime);
        
        $("#PI_PushedStartTime").datebox("setValue", row.StartTime);
        $("#PI_PushedEndTime").datebox("setValue", row.EndTime);
        $("#pi_Notes").val(row.Notes);



        if (row.Config!=undefined&& row.Config != "") {
            $("#pi_picUrl1").attr("src", row.Config);
            $("#pi_picSrc1").val(row.Config);
            $("#pi_picId1").val(row.LMan);
        }

        OpenPIDialog();
    }
    else if (rows.length > 1) {
        $.messager.alert("提示信息", "请选择一条消息推送", "info");
    }
    else {
        $.messager.alert("提示信息", "请先选择一条消息推送", "info");
    }
}

//删除
function DeletePI()
{
    $.messager.confirm("提示信息", "您确定要删除该消息推送么?", function (flag) {
        if (flag)
        {
            var row = $("#CXList").datagrid("getSelections");
            if (row != null && row.length == 1 && row[0].AdsId != undefined) {
                $.ajax({
                    url: '/BasicInfo/PI_DelSave',
                    type: 'POST',
                    data: { id: row[0].AdsId, },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '删除成功'
                            });
                            $("#CXList").datagrid("reload");
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
                $.messager.alert("提示信息", "请选择一条消息推送", "info");
            }
            else {
                $.messager.alert("提示信息", "请先选择一条消息推送", "info");
            }
        }
    });
    
}

//关闭新增对话框
function addPIClose() {
    $("#PI_AddInfo").dialog("close");
}

//选择商品对话框
function PI_SelectProduct() {
    $("#PI_SelectProduct").dialog({
        title: '选择关联商品',
        width: 415,
        height: 455,
        modal: true
    });
    $("#PI_SelectProduct_list").datagrid({
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
            PI_FillInterRows();
        }
    });
    PI_searchProduct();
}



//当不够十行时补充十行空数据
function PI_FillInterRows() {
    var data = $('#PI_SelectProduct_list').datagrid('getData');
    var pageopt = $('#PI_SelectProduct_list').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#PI_SelectProduct_list').datagrid('appendRow', {});
        }
    }
}

//关闭选择商品对话框
function PI_SelectProduct_Close() {
    $("#PI_SelectProduct").dialog("close");
}

//选择商品查询
function PI_searchProduct() {
    $("#PI_SelectProduct_list").datagrid("load", {
        type: $("#PI_Search").combobox("getValue"),
        content: $("#PI_Content").val()
    });
}
//选择商品-选中
function PI_SProduct() {
    var row = $("#PI_SelectProduct_list").datagrid("getSelected");
    if (row != null && row.ProductId != undefined && row.ProductId != "") {
        $("#PI_ConnectionProduct").combobox("setValue", row.ProductId);
        PI_SelectProduct_Close();
    }
    else {
        $.messager.alert("提示信息", "请先选择一条商品", "info");
    }
}

//图片上传
function pi_savePic(obj, index) {
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
    $("#pi_picId1").val("");
    $("#pi_imgform" + index).ajaxSubmit({
        loadingMsg: '正在上传...',
        dataType: 'text', //数据格式为text
        success: function (data) {
            var json = eval("(" + data + ")");
            if (json.success == "1") {
                $("#pi_picUrl" + index).attr("src", json.src);
                $("input[name=pi_picSrc" + index + "]").val(json.src);
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
function pi_delPicture(index) {
    if ($("#pi_picSrc1").val() == "") {
        return;
    }
    if ($("#pi_picId1").val() != "") {
        $("#pi_picUrl" + index).attr("src", "");
        $("#pi_picId1").val("");
        $("input[name=pi_picSrc" + index + "]").val("");
        $("input[name=pi_file" + index + "]").val("");
        return;
    }
    if ($("input[name=pi_picUrl" + index + "]").val() != "") {
        $.post("/BasicInfo/DelPic", { name: $("input[name=pi_picSrc" + index + "]").val() }, function (data) {
            if (data != 1) {
                $.messager.alert("警告", "图片删除出错，请重试!", "warning");
            }
            else {
                $("#pi_picUrl" + index).attr("src", "");
                $("#pi_picId1").val("");
                $("input[name=pi_picSrc" + index + "]").val("");
                $("input[name=pi_file" + index + "]").val("");
            }
        });
    }

}

//新增/编辑消息推送保存,type=1  保存并新增,type=0  保存并关闭
function PI_AddSave(type) {
    if ($("#pi_picSrc1").val() == "") {
        $.messager.alert("提示信息", "图片不能为空", "info");
        return;
    }
    else if ($("#PI_Title").val() == "") {
        $.messager.alert("提示信息", "标题不能为空", "info");
        return;
    }
    else if ($("#PI_content").val() == "") {
        $.messager.alert("提示信息", "内容不能为空", "info");
        return;
    }
    else if ($("#pi_linkType").prop("checked") && $("#PI_Url").val() == "") {
        $.messager.alert("提示信息", "关联链接不能为空", "info");
        return;
    }
    else if ($("#pi_productType").prop("checked") && $("#PI_ConnectionProduct").combobox("getValue") == "") {
        $.messager.alert("提示信息", "关联商品不能为空", "info");
        return;
    }
    else if ($("#pi_autoColse").prop("checked") && $("#PI_StaySpan").numberspinner("getValue") == "") {
        $.messager.alert("提示信息", "自动关闭时间不能为空", "info");
        return;
    }
    else {
        var json = "{";
        json += "\"Title\":\"" + $("#PI_Title").val() + "\",\"Content\":\"" + $("#PI_content").val() + "\",\"Link\":\"" + $("#PI_Url").val() + "\",\"Receiver\":\"" + $("#PI_ReceiveType").combobox("getValue") + "\",";
        json += "\"PopTime\":\"" + $("#PI_AlertSpan").val() + "\",\"StartTime\":\"" + $("#PI_PushedStartTime").datebox("getValue") + "\",\"EndTime\":\"" + $("#PI_PushedEndTime").datebox("getValue") + "\",";
        if ($("#pi_linkType").prop("checked"))
        {
            json += "\"Link\":\"" + $("#PI_Url").val() + "\",";
        }
        if ($("#pi_productType").prop("checked")) {
            json += "\"ProductId\":\"" + $("#PI_ConnectionProduct").combobox("getValue") + "\",";
        }
        if ($("#pi_autoColse").prop("checked")) {
            json += "\"RemainTime\":\"" + $("#PI_StaySpan").numberspinner("getValue") + "\",";
        }

        json += "\"Notes\":\"" + $("#pi_Notes").val() + "\",\"Picture\":\"" + ($("#pi_picId1").val() == "" ? 0 : $("#pi_picId1").val()) + "\",\"AdsId\":\"" + ($("#pi_hid_AdsId").val() == "" ? 0 : $("#pi_hid_AdsId").val()) + "\"";
        json += "}";
        console.log(json);
        $.ajax({
            url: '/BasicInfo/PI_AddSave',
            type: 'POST',
            data: { str: json, picSrc: $("#pi_picSrc1").val() },
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
                        addPIClose();
                        ResetForm();
                        $("#addPI").show();
                    }
                    $("#CXList").datagrid("reload");

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
function ResetForm() {
    $("#pi_hid_AdsId").val("");
    $("#PI_Title").val("");
    $("#PI_content").val("");
    $("#PI_Url").val("");
    var time = new Date();
    $("#PI_ReceiveType").combobox("setValue", 0);
    $("#PI_ConnectionProduct").combobox("setValue", "");
    $("#PI_AlertSpan").val(10);
    $("#PI_StaySpan").val(5);
    $("#PI_PushedStartTime").datebox("setValue", time.toLocaleTimeString());
    $("#PI_PushedEndTime").datebox("setValue", '2100-1-1');
    $("#pi_Notes").val("");
    //图片重置
    $("#pi_picUrl1").attr("src", "");
    $("#pi_picSrc1").val("");
    $("#pi_picId1").val("");
    $("input[name=pi_file1]").val("");
}

function PIChangeSize() {
    var height = document.documentElement.clientHeight - 255;
    if (height > 400) {
        $("#PI_CXList").height(height);
    }

}

