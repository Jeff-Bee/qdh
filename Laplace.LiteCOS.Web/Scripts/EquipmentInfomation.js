$(function () {
    $("#Equipmentinfo").datagrid({
        url: '/BasicInfo/GetEquipmentList',
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
                field: 'Name',
                title: '设备名称',
                width: 100,
            },
            {
                field: 'Code',
                title: '设备编号',
                width: 100,
            },
            {
                field: 'CarNumber',
                title: '车牌号',
                width: 100,
            },
            {
                field: 'Driver',
                title: '司机',
                width: 100,
            },
            {
                field: 'MobilePhone',
                title: '联系电话',
                width: 100,
            },
            {
                field: 'Notes',
                title: '备注',
                width: 100,
            }

        ]],
        onLoadSuccess: function () {
            FillProductRows();
        }
    });
    $("#isEquipASC").click(function () {
        if ($("#isEquipASC").prop("checked")) {
            GetEquipCode();
        }
    });

    //自适应高度
    EquipChangeSize();
});

//当不够十行时补充十行空数据
function FillProductRows() {
    var data = $('#Equipmentinfo').datagrid('getData');
    var pageopt = $('#Equipmentinfo').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#Equipmentinfo').datagrid('appendRow', {});
        }
    }
}


//空白新增
function addEquapInfo() {
    resetEquipForm();
    $("#isEquipASC").prop("checked", true);
    GetEquipCode();

    $("#equip_add").dialog({
        title: '设备信息框',
        width: 365,
        height: 270,
    });
}

//编辑新增/修改 type=0 复制新增,  1  修改
function editEquapForm(type) {
    var row = $("#Equipmentinfo").datagrid("getSelected");
    if (row != null) {
        if (row.CarId == undefined) {
            return;
        }
        resetEquipForm();
        if (type == 1) {
            $("#hid_equipId").val(row.CarId);
            $("#equipCode").val(row.Code);
            $("#isEquipASC").prop("checked", true);
            $("#addDepartment").hide();
        }
        else {
            $("#hid_equipId").val("");
            $("#isEquipASC").prop("checked", true);
            $("#equipCode").val("");
            GetEquipCode();
        }
        $("#equipName").val(row.Name);
        $("#equipCarNumber").val(row.CarNumber);
        $("#equipDriver").val(row.Driver);
        $("#equipPhone").val(row.MobilePhone);
        $("#equipNotes").val(row.Notes);

        $("#equip_add").dialog({
            title: '设备信息框',
            width: 365,
            height: 270,
        });
    }
    else {
        $.messager.alert("提示信息", "请先选择一行数据", "info");
    }
}

//删除一行数据
function delEquapInfo() {
    var row = $("#Equipmentinfo").datagrid("getSelected");
    if (row != null) {
        if (row.CarId == undefined) {
            return;
        }
        $.messager.confirm("确认信息", "您确认要删除当前数据么?", function (flag) {
            if (flag) {
                $.ajax({
                    url: '/BasicInfo/DelDeliveryCarInfo',
                    type: 'POST',
                    data: { id: row.CarId },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '删除成功',
                            });
                            $("#Equipmentinfo").datagrid("reload");
                        }
                        else if (data == "0") {
                            $.messager.alert("警告信息", "删除失败,请刷新重试", "info");
                        }
                        else {
                            $.messager.alert("警告信息", "获取参数失败,请刷新重试", "info");
                        }
                    }
                });
            }
        })

    }
    else {
        $.messager.alert("提示信息", "请先选择一行数据", "info");
    }
}

//新增/编辑保存  type=0 保存并关闭  type=1 保存并新增
function addEquapInfoSave(type) {
    if ($("#equipName").val() == "") {
        $.messager.alert("提示信息", "设备名称不能为空", "info");
        return false;
    }
    else if ($("#equipJYM").val() == "") {
        $.messager.alert("提示信息", "校验码不能为空", "info");
        return false;
    }
    else if ($("#equipCode").val() == "") {
        $.messager.alert("提示信息", "设备编号不能为空", "info");
        return false;
    }
    else {
        var json = "{";
        json += "\"CarId\":\"" + ($("#hid_equipId").val() == "" ? "0" : $("#hid_equipId").val()) + "\",\"Code\":\"" + $("#equipCode").val() + "\",\"Name\":\"" + $("#equipName").val() + "\",\"CarNumber\":\"" + $("#equipCarNumber").val() + "\",";
        json += "\"Driver\":\"" + $("#equipDriver").val() + "\",\"MobilePhone\":\"" + $("#equipPhone").val() + "\",\"Notes\":\"" + $("#equipNotes").val() + "\"";
        json += "}";
        $.ajax({
            url: '/BasicInfo/SaveEquitment',
            type: 'POST',
            data: { str: json, jym: $("#equipJYM").val() },
            success: function (data) {
                if (data == "1") {
                    $.messager.show({
                        title: '提示',
                        msg: '保存成功',
                    });
                    resetEquipForm();
                    $("#isEquipASC").prop("checked", true);
                    GetEquipCode();
                    if (type == 0) {
                        addEquapClose();
                    }
                    $("#Equipmentinfo").datagrid("reload");
                }
                else if (data == "0") {
                    $.messager.alert("提示信息", "保存失败,请刷新重试", "info");
                }
                else if (data == "3") {
                    $.messager.alert("提示信息", "校验码不正确", "info");

                }
                else if (data == "2") {
                    $.messager.alert("提示信息", "code重复", "info");
                }
                else {
                    $.messager.alert("提示信息", "参数获取失败,请刷新重试", "info");
                }
            }
        });


    }

}

//重置表单
function resetEquipForm() {
    $("#equip_add")[0].reset();
    $("#hid_equipId").val("");
    $("#addDepartment").show();

}

//关闭信息
function addEquapClose() {
    $("#equip_add").dialog("close");
}

//获取Code
function GetEquipCode() {
    $.ajax({
        url: '/BasicInfo/GetEquipmentCode',
        type: 'POST',
        success: function (data) {
            $("#equipCode").val(data);
        }
    });
}

function EquipChangeSize() {
    var height = document.documentElement.clientHeight - 190;
    if (height > 400) {
        $("#Equip_List").height(height);
    }

}