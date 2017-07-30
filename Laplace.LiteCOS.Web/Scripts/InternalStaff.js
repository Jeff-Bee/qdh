$(function () {
    $("#internalUser").datagrid({
        url: '/BasicInfo/GetInternalUserList',
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
                field: 'EmployeeCode',
                title: '职员编号',
                width:100,
            },
            {
                field: 'EmployeeFullName',
                title: '职员全名',
                width: 100,
            },
            {
                field: 'MobilePhone',
                title: '绑定手机',
                width: 100,
            },
            {
                field: 'Notes',
                title: '备注',
                width: 100,
            },
        ]],
        onLoadSuccess: function () {
            FillInterRows();
        }
    });

    $("#InterdepId").combobox({
        url: '/BasicInfo/GetDepList',
        valueField: 'DeptId',
        textField: 'DeptFullName',
        height:22,

    });

    $("#isInterasc").click(function () {
        if ($("#isInterasc").prop("checked")) {
            GetEmployeeCode();
        }
    });

    //自适应高度
    EmployeeChangeSize();
});


//当不够十行时补充十行空数据
function FillInterRows() {
    var data = $('#internalUser').datagrid('getData');
    var pageopt = $('#internalUser').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#internalUser').datagrid('appendRow', {});
        }
    }
}

//新增用户信息
function addUser()
{
    ResetInterForm();
    $("#isInterasc").prop("checked", true);
    GetEmployeeCode();
    $("#addUser").dialog({
        title: '内部职员-信息框',
        width: 390,
        height: 220,
        modal: true
    });
}

//关闭新增
function closeInterAdd() {
    $("#addUser").dialog("close");
}

//新增保存,type:0  新增并关闭,1:保存并新增
function addInterSave(type)
{
    if ($("#Interfullname").val() == "")
    {
        $.messager.alert("提示信息", "职员全名不能为空", "info");
        $("#Interfullname").focus();
        return false;
    }
    else if ($("#Intercode").val() == "") {
        $.messager.alert("提示信息", "职员编号不能为空", "info");
        $("#Intercode").focus();
        return false;
    }
    else if ($("#InterdepId").combobox("getValue") == "") {
        $.messager.alert("提示信息", "部门不能为空", "info");
        //$("#InterdepId").focus();
        return false;
    }
    else if ($("#uphone").val() == "") {
        $.messager.alert("提示信息", "绑定手机不能为空", "info");
        $("#Interphone").focus();
        return false;
    }
    else {
        var json = "{";
        json += "\"EmployeeId\":\"" + ($("#hid_EmployeeId").val() == "" ? "0" : $("#hid_EmployeeId").val()) + "\",\"EmployeeCode\":\"" + $("#Intercode").val() + "\",\"EmployeeFullName\":\"" + $("#Interfullname").val() + "\",\"MobilePhone\":\"" + $("#Interphone").val() + "\",";
        json += "\"Notes\":\"" + $("#Interdemo").val() + "\",\"DeptId\":\"" + $("#InterdepId").combobox("getValue") + "\"";
        json += "}";
        $.ajax({
            url: '/BasicInfo/ISSave',
            type: 'POST',
            data: { str: json },
            success: function (data) {
                if (data == "1")
                {
                    if (type == 0)
                    {
                        $.messager.show({
                            title: '提示信息',
                            msg: '保存成功'
                        });
                        //$.messager.alert("提示信息", "保存成功", "info");
                        ResetInterForm();
                        closeInterAdd();
                        $("#internalUser").datagrid("reload");
                    }
                    else if (type == 1) {
                        $.messager.show({
                            title: '提示信息',
                            msg: '保存成功'
                        });
                        //$.messager.alert("提示信息", "保存成功", "info");
                        ResetInterForm();
                        GetEmployeeCode();
                        $("#internalUser").datagrid("reload");
                    }
                    
                }
                else if (data == "0")
                {
                    $.messager.alert("提示信息", "code重复", "info");
                }
                else if (data == "3")
                {
                    $.messager.alert("提示信息", "保存失败,请刷新后重试", "info");
                }
                else if (data == "2")
                {
                    $.messager.alert("提示信息", "获取参数出错,请刷新后重试", "info");
                }
            }
        });
    }
}

//重置新增用户表单
function ResetInterForm()
{
    $("#Interfullname").val("");
    $("#hid_EmployeeId").val("");
    $("#Intercode").val("");
    $("#Interphone").val("");
    $("#Interdemo").val("");
    $("input[name=isInterasc]").prop("checked", true);
    $("#InterdepId").combobox("setValue", '');
    
}

//编辑用户信息,type=1,修改,0,复制新增
function editInterForm(type)
{
    var row = $("#internalUser").datagrid("getSelected");
    if (row != null) {
        if(row.EmployeeCode==null)
        {
            return;
        }
        if (type == 1) {
            $("#addAndSave").hide();
            $("#hid_EmployeeId").val(row.EmployeeId);
            $("input[name=Intercode]").val(row.EmployeeCode);
            $("#isInterasc").prop("checked", true);
        }
        else {
            $("#addAndSave").show();
            $("#hid_EmployeeId").val("");
            $("input[name=Intercode]").val("");
            $("#isInterasc").prop("checked", true);
            GetEmployeeCode();
        }
        $("#InterdepId").combobox("setValue", row.DeptId);
        $("input[name=Interfullname]").val(row.EmployeeFullName);
       
        $("input[name=Interphone]").val(row.MobilePhone);
        $("input[name=Interdemo]").val(row.Notes);
        $("#addUser").dialog({
            title: '内部职员-信息框',
            width: 390,
            height: 250,
            modal: true
        });
    }
    else {
        $.messager.alert("提示信息","请先选择一行数据","info");
    }
}

//删除员工信息
function DelInterRow()
{
    var row = $("#internalUser").datagrid("getSelected");
    if (row != null) {
        if (row.EmployeeCode == null) {
            return;
        }
        $.messager.confirm("提醒", "您确定要删除当前行么", function (flag) {
            if (flag)
            {
                $.ajax({
                    url: '/BasicInfo/DelInternalStaff',
                    type: 'POST',
                    data: { id: row.EmployeeId },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示信息',
                                msg: '删除成功!'
                            });
                           
                            $("#internalUser").datagrid("reload");
                        }
                        else if (data == "0") {
                            $.messager.alert("提示信息", "删除失败!", "info");
                        }
                        else {
                            $.messager.alert("提示信息", "接受参数出错，请刷新重试", "info");
                        }
                    }
                });
            }
        });
       
    }
    else {
        $.messager.alert("提示信息", "请选择要删除的行", "info");
    }
}

//获取code
function GetEmployeeCode()
{
    $.ajax({
        url: '/BasicInfo/GetEmployeeCode',
        type: 'POST',
        success: function (data) {
            $("#Intercode").val(data);
        }
    });
}


function EmployeeChangeSize() {
    var height = document.documentElement.clientHeight - 190;
    if (height > 400) {
        $("#Employee_List").height(height);
    }

}