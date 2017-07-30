$(function () {
    $("#Departmentinfo").datagrid({
        url: '/BasicInfo/GetDepList',
        rownumbers: true,
        loadMsg: '数据加载中。。',
        striped: true,
        remoteSort: false,
        pagination: true,
        singleSelect: true,
        fixed: true,
        fitColumns: true,
        columns: [[
            {
                field: 'DeptCode',
                title: '部门编号',
                width:100,
            },
             {
                 field: 'DeptFullName',
                 title: '部门名称',
                 width: 100,
             },
             {
                 field: 'Notes',
                 title: '备注',
                 width: 100,
             }
        ]],
        onLoadSuccess: function () {
            FillDeptRows();
        }
    });
    $("#isDepASC").click(function () {
        if ($("#isDepASC").prop("checked"))
        {
            GetDepCode();
        }
    });

    //自适应高度
    DepChangeSize();
});

//当不够十行时补充十行空数据
function FillDeptRows() {
    var data = $('#Departmentinfo').datagrid('getData');
    var pageopt = $('#Departmentinfo').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#Departmentinfo').datagrid('appendRow', {});
        }
    }
}

//新增部门
function addDepInfo()
{
    ReFlashDepInfo();
    $("#isDepASC").prop("checked", true);
    GetDepCode();
    $("#dep_add").dialog({
        title: '部门信息',
        height: 190,
        width: 310,
        modal: true
    });
}

//关闭弹框
function addDepClose()
{
    $("#dep_add").dialog("close");
}

//新增部门保存type:1 保存并新增,0保存并关闭
function addDepInfoSave(type)
{
    if ($("#depName").val() == "") {
        $.messager.alert("提示信息", "部门名称不能为空", "info");
        $("#depName").focus();
        return false;
    }
    else if ($("#depCode").val() == "") {
        $.messager.alert("提示信息", "部门编号不能为空", "info");
        $("#depCode").focus();
        return false;
    }
    else  {
        var json = "{";
        json += "\"DeptId\":\"" + ($("#hid_depId").val() == "" ? "0" : $("#hid_depId").val()) + "\",\"DeptCode\":\"" + ($("#depCode").val() == "" ? $("#hid_depCode").val() : $("#depCode").val()) + "\",\"DeptFullName\":\"" + $("#depName").val() + "\",\"Notes\":\"" + $("#depNotes").val() + "\"";
        json += "}";
        $.ajax({
            url: '/BasicInfo/AddDeptSave',
            type: 'POST',
            data: { str: json },
            success: function (data) {
                if (data == "1")
                {
                    $.messager.show({
                        title: '提示',
                        msg:'保存成功',
                    });
                    if (type == "1")//新增
                    {
                        ReFlashDepInfo();
                        GetDepCode();
                        $("#Departmentinfo").datagrid("reload");
                    }
                    else if (type == "0")//关闭
                    {
                        addDepClose();
                        ReFlashDepInfo();
                        $("#Departmentinfo").datagrid("reload");
                    }
                }
                else if (data == "2")
                {
                    $.messager.alert("提示信息","code重复","info");
                }
                else if (data == "0") {
                    $.messager.alert("提示信息", "保存失败,请刷新重试", "info");
                }
                else {
                    $.messager.alert("提示信息", "获取参数失败,请刷新重试", "info");
                }
            }
        });
    }
}

//重置新增表格
function ReFlashDepInfo()
{
    $("#hid_depId").val("");
    $("#depName").val("");
    $("#depCode").val("");
    $("#depNotes").val("");
    $("#isDepASC").prop("checked", true);
    $("#addDapt").show();
}

//编辑新增/修改,type=0,编辑新增,1修改
function editDepForm(type)
{
    ReFlashDepInfo();
    var row = $("#Departmentinfo").datagrid("getSelected");
    if (row != null) {
        if (row.DeptId == undefined)
        {
            return;
        }
        if (type == 1) {
            $("#hid_depId").val(row.DeptId);
            $("#depCode").val(row.DeptCode);
            $("#isDepASC").prop("checked", true);
            $("#addDapt").hide();
        }
        else {
            $("#isDepASC").prop("checked", true);
            GetDepCode();
            $("#addDapt").show();
        }
        $("#depName").val(row.DeptFullName);
        $("#depNotes").val(row.Notes);
        $("#dep_add").dialog({
            title: '部门信息',
            height: 190,
            width: 310,
            modal: true
        });
    }
    else {
        $.messager.alert("提示信息","请先选择一行","info");
    }
}

//删除一行
function delDepInfo()
{
    var row = $("#Departmentinfo").datagrid("getSelected");
    if (row != null) {
        if (row.DeptId == undefined) {
            return;
        }
        $.messager.confirm("确认信息", "是否确认删除", function (flag) {
            if (flag)
            {
                $.ajax({
                    url: "/BasicInfo/delDeptInfo",
                    data: { id: row.DeptId },
                    type: 'POST',
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '删除成功!',
                            });
                            $("#Departmentinfo").datagrid("reload");
                        }
                        else {
                            $.messager.alert("提示信息", "删除失败,请刷新重试", "info");
                        }
                    }
                });
            }
        });
       
    }
    else {
        $.messager.alert("提示信息", "请先选择一行", "info");
    }
}

//获取code
function GetDepCode()
{
    $.ajax({
        url: '/BasicInfo/GetDepCode',
        type: 'POST',
        success: function (data) {
            $("#depCode").val(data);
        }
    });
}

function DepChangeSize() {
    var height = document.documentElement.clientHeight - 190;
    if (height > 400) {
        $("#Dep_List").height(height);
    }

}