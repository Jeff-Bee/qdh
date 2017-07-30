$(function () {
    $("#AreaIndex").datagrid({
        url: '/BasicInfo/GetAreaIndexList',
        rownumbers: true,
        loadMsg: '数据加载中。。',
        striped: true,
        remoteSort: false,
        pagination: true,
        fixed: true,
        fitColumns: true,
        singleSelect: true,
        columns: [[
            {
                field: 'Code',
                title: '地区代码',
                width:100,
            },
            {
                field: 'FullName',
                title: '地区全名',
                width: 100,
            },
            {
                field: 'Notes',
                title: '备注',
                width: 100,
            }
           
        ]],
        onLoadSuccess: function () {
            FillAreaRows();
        }
    });

    //自适应高度
    AreaChangeSize();
});

//当不够十行时补充十行空数据
function FillAreaRows() {
    var data = $('#AreaIndex').datagrid('getData');
    var pageopt = $('#AreaIndex').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#AreaIndex').datagrid('appendRow', {});
        }
    }
}

//空白新增
function addAreaForm()
{
    ResetAreaForm();
    GetAreaCode();
    $("#addArea").dialog({
        title: '地区信息',
        width: 340,
        height: 200,
        modal: true
    });
}

//保存 type=1 保存并关闭,0保存并新增
function SaveAreaEdit(type)
{
    if ($("#Areafullname").val() == "")
    {
        $.messager.alert("提示信息", "地区名称不能为空", "info");
        $("#Areafullname").focus();
        return false;
    }
    else if ($("#Areacode").val() == "") {
        $.messager.alert("提示信息", "地区代码不能为空", "info");
        $("#Areacode").focus();
        return false;
    }
    else {
        var json = "{";
        json += "\"AreaId\":\"" + ($("#hid_AreaId").val() == "" ? "0" : $("#hid_AreaId").val()) + "\",\"Code\":\"" + $("#Areacode").val() + "\",\"FullName\":\"" + $("#Areafullname").val() + "\",\"Notes\":\"" + $("#Areademo").val() + "\"";
        json += "}";
        $.ajax({
            url: '/BasicInfo/SaveAreaEdit',
            type: 'POST',
            data: { str: json },
            success: function (data) {
                if (data == "1") {
                    $.messager.show({
                        title: '提示',
                        msg:'保存成功',
                    });
                    if (type == "1") {
                        closeAreaAdd();
                    }
                    else {
                        ResetAreaForm();
                        GetAreaCode();
                    }
                    $("#AreaIndex").datagrid("reload");
                }
                else if (data == "0")
                {
                    $.messager.alert("提示信息","保存失败,请刷新重试","info");
                }
                else if (data == "2") {
                    $.messager.alert("提示信息", "地区代码重复", "info");
                }
                else {
                    $.messager.alert("提示信息", "获取参数失败,请刷新重试", "info");
                }
            }
        });
    }
}

//重置新增表单
function ResetAreaForm()
{
    $("#hid_AreaId").val("");
    $("#Areafullname").val("");
    $("#Areacode").val("");
    $("#Areademo").val("");
    $("#isAreaasc").prop("checked", true);
    $("#addAndSaveArea").show();
}

//关闭新增对话框
function closeAreaAdd()
{
    $("#addArea").dialog("close");
}


//自动获取code
function GetAreaCode()
{
    $.ajax({
        url: '/BasicInfo/GetAreaCode',
        type: 'POST',
        async:false,
        success: function (data) {
            $("#Areacode").val(data);
        }
    });
}

//编辑/复制新增 type=1 编辑  0 复制新增
function editAreaForm(type)
{
    var row = $("#AreaIndex").datagrid("getSelected");
    if (row != null) {
        if (row.AreaId == undefined)
        {
            return;
        }
        $("#isAreaasc").prop("checked", true);
        if (type == "1") {
            $("#hid_AreaId").val(row.AreaId);
            $("#Areacode").val(row.Code);
            $("#addAndSaveArea").hide();
        }
        else {
            $("#hid_AreaId").val("");
            GetAreaCode();
            $("#addAndSaveArea").show();
        }
        $("#Areafullname").val(row.FullName);
        $("#Areademo").val(row.Notes);
        $("#addArea").dialog({
            title: '地区信息',
            width: 340,
            height: 200,
            modal: true
        });
    }
    else {
        $.messager.alert("提示信息","请先选择一行数据","info");
    }
}

//删除地区信息
function DelAreaRow()
{
    var row = $("#AreaIndex").datagrid("getSelected");
    if (row != null) {
        if (row.AreaId == undefined)
        {
            return;
        }
        $.messager.confirm("确认信息", "您确定要删除么？", function (flag) {
            if (flag)
            {
                $.ajax({
                    url: '/BasicInfo/DelArea',
                    type: 'POST',
                    data: { id: row.AreaId },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '删除成功',
                            });
                            $("#AreaIndex").datagrid("reload");
                        }
                        else if (data == "0") {
                            $.messager.alert("提示信息", "删除失败", "info");
                        }
                        else {
                            $.messager.alert("提示信息", "获取参数失败，请刷新重试", "info");
                        }
                    }
                });
            }
        });
       
    }
    else {
        $.messager.alert("提示信息","请先选择一行","info");
    }
}

function AreaChangeSize() {
    var height = document.documentElement.clientHeight - 190;
    if (height > 400) {
        $("#Area_List").height(height);
    }

}