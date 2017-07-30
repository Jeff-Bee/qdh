$(function () {
    $("#suppInfo").datagrid({
        url: '/BasicInfo/GetSuppList',
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
                field: 'SupplierId',
                title: '供应商编号',
                width:100,
            },
            {
                field: 'Code',
                title: '供应商代码',
                width: 100,
            },
            {
                field: 'FullName',
                title: '单位名称',
                //width: '100px',
                width: 100,
            },
            {
                field: 'Phone',
                title: '电话',
                //width: '100px',
                width: 100,
            },
            {
                field: 'MobilePhone',
                title: '手机',
                //width: '100px',
                width: 100,
            },
            {
                field: 'ConstactPerson',
                title: '联系人',
                //width: '100px',
                width: 100,
            },
            {
                field: 'Address',
                title: '地址',
                //width: '100px',
                width: 100,
            },
             {
                 field: 'Bank',
                 title: '开户银行',
                 //width: '100px',
                 width: 100,
             }
        ]],
        onLoadSuccess: function () {
            FillRows();
        }
    });

    $("#suppArea").combobox({
        url: '/BasicInfo/GetAreaList',
        textField: 'text',
        valueField: 'id',
        editable:false,
        hasDownArrow: false,
        icons: [{
            iconCls: 'combo-more',
            handler: function () {
                CheckSupArea();
            }
        }]
    });
    $("#isSuppasc").click(function () {
        if ($("#isSuppasc").prop("checked"))
        {
            GetCode();
        }
    });
    $("#supAreaList").datagrid({
        url: '/BasicInfo/GetSupAreaList',
        rownumbers: true,
        loadMsg: '数据加载中。。',
        field:true,
        striped: true,
        remoteSort: false,
        singleSelect: true,
        columns: [[
            {
                field: 'Code',
                title: '地区编号',
                width:100,
            },
            {
                field: 'FullName',
                title: '地区名称',
                width: 100,
            }
        ]],
        onLoadSuccess: function () {
            FillSupAreaSearchRows();
        },
        onSelect: function (rowIndex, rowData) {
            if (rowData != null && rowData.AreaId != undefined) {
                $("#supAreaCheck").linkbutton("enable");
            }
            else {
                $("#supAreaCheck").linkbutton("disable");
            }
        }
    });

    $("#supAreaListSearch").combobox({
        url: '/BasicInfo/SearchAreaCondition',
        valueField: 'id',
        textField: 'text',
        width:100,
    });
    $("#supAreaListSearch").combobox("setValue",0);
    $("#supAreaCheck").linkbutton({
        disabled:true,
    });
    //自适应高度
    SuppChangeSize();
});

//当不够十行时补充十行空数据
function FillRows() {
    var data = $('#suppInfo').datagrid('getData');
    var pageopt = $('#suppInfo').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#suppInfo').datagrid('appendRow', {});
        }
    }
}

//当不够十行时补充十行空数据
function FillSupAreaSearchRows() {
    var data = $('#supAreaList').datagrid('getRows');
    if (data.length < 10) {
        for (var i = data.length; i < 10; i++) {
            $('#supAreaList').datagrid('appendRow', {});
        }
    }
}

//选中地区
function CheckSupArea()
{
    $("#GetAreaList").dialog({
        title: '地区选中框',
        width: 270,
        height: 400,
        modal: true
    });
}

//关闭所属地区选择
function CloseCheckSupArea()
{
    $("#GetAreaList").dialog("close");
}

//选择地区搜索
function searchSupArea() {
    $("#supAreaList").datagrid("load", {
        id: $("#supAreaListSearch").combobox("getValue"),
        text: $("#supAreaSearchContent").val(),
    });
}

//地区选择-选中操作
function SupAreaChecked()
{
    var row = $("#supAreaList").datagrid("getSelected");
    if (row != null) {
        $("#suppArea").combobox("setValue", row.AreaId);
        CloseCheckSupArea();
    }
}

//新增
function addSuppliers()
{
    resetForm();
    $("#isSuppasc").prop("checked",true);
    GetCode();
    $("#addsuppInfo").dialog({
        title: '供应商信息-信息框',
        width:415,
        height: 400,
        modal: true
    });
}

//保存.type=1 保存并新增,type=0  保存并关闭
function addSuppSave(type)
{
    if ($("#suppName").val() == "")
    {
        $.messager.alert("提示信息", "单位名称不能为空", "info");
        $("#suppName").focus();
        return false;
    }
    else if ($("#suppCode").val() == "") {
        $.messager.alert("提示信息", "单位编号不能为空", "info");
        $("#suppCode").focus();
        return false;
    }
    else if ($("#suppPhone").val() == "") {
        $.messager.alert("提示信息", "手机号不能为空", "info");
        $("#suppPhone").focus();
        return false;
    }
    else if ($("#suppArea").combobox("getValue") == "")
    {
        $.messager.alert("提示信息", "所属地区不能为空,如果还未定义地区,请先定义地区", "info");
        return false;
    }
    else if ($("#suppAdress").val() == "") {
        $.messager.alert("提示信息", "地址不能为空", "info");
        $("#suppAdress").focus();
        return false;
    }
    else {
        var json = '{';
        json += "\"SupplierId\":\"" + ($("#hid_SupplierId").val() == "" ? "0" : $("#hid_SupplierId").val()) + "\",\"Code\":\"" + $("#suppCode").val() + "\",\"FullName\":\"" + $("#suppName").val() + "\",\"AreaId\":\"" + ($("#suppArea").combobox("getValue") == "" ? "0" : $("#suppArea").combobox("getValue")) + "\",";
        json += "\"TaxNumber\":\"" + $("#suppTaxnum").val() + "\",\"ConstactPerson\":\"" + $("#suppContact").val() + "\",\"Phone\":\"" + $("#suppTel").val() + "\",\"MobilePhone\":\"" + $("#suppPhone").val() + "\",\"Email\":\"" + $("#suppEmail").val() + "\",";
        json += "\"Address\":\"" + $("#suppAdress").val() + "\",\"Bank\":\"" + $("#suppAccount").val() + "\",\"BankAccount\":\"" + $("#suppBank").val() + "\",\"Notes\":\"" + $("#suppNote").val() + "\",\"ExchangeDay\":\"" + $("#supExchangeDay").val() + "\",\"ExchangePercent\":\"" + $("#suppExchangePercent").val() + "\"";
        json += "}";
        $.ajax({
            url: '/BasicInfo/AddSuppSave',
            type: 'POST',
            data: { str: json },
            success: function (data) {
                if (data == "1") {
                    $.messager.show({
                        title: '提示',
                        msg: '保存成功!',
                    });
                    if (type == 0) {
                        $("#addsuppInfo").dialog("close");
                    }
                    else {
                        resetForm();
                        GetCode();
                    }
                    $("#suppInfo").datagrid("reload");
                }
                else if (data == "0") {
                    $.messager.alert("提示信息", "保存失败,请刷新重试", "info");
                }
                else if (data == "2") {
                    $.messager.alert("提示信息", "code重复", "info");
                }
                else {
                    $.messager.alert("提示信息", "获取参数失败,请刷新重试", "info");
                }
            },
        });
    }
}

//编辑,type=1,复制新增,type=0,编辑
function editForm(type)
{
    resetForm();
    var row = $("#suppInfo").datagrid("getSelected");
    if (row != null) {
        if (row.SupplierId == undefined)
        {
            return;
        }
        if (type == 0) {
            $("#hid_SupplierId").val(row.SupplierId);
            $("#suppCode").val(row.Code);
            $("#isSuppasc").prop("checked", true);
        }
        else {
            $("#hid_SupplierId").val("");
            $("#suppCode").val("");
            $("#isSuppasc").prop("checked", true);
            GetCode();
        }
        $("#suppName").val(row.FullName);
        $("#suppPhone").val(row.MobilePhone);
        $("#suppContact").val(row.ConstactPerson);
        $("#suppTaxnum").val(row.TaxNumber);
        $("#suppArea").combobox("setValue", row.AreaId);
        $("#suppTel").val(row.Phone);
        $("#supExchangeDay").val(row.ExchangeDay);
        $("#suppEmail").val(row.Email);
        $("#suppExchangePercent").val(row.ExchangePercent);
        $("#suppAdress").val(row.Address);
        $("#suppAccount").val(row.Bank);
        $("#suppBank").val(row.BankAccount);
        $("#suppNote").val(row.Notes);
        $("#addsuppInfo").dialog({
            title: '供应商信息-信息框',
            width: 415,
            height: 400,
            modal: true
        });
    }
    else {
        $.messager.alert("提示信息","请选择一行数据","info");
    }
}


//删除一行
function delSuppliser() {
    var row = $("#suppInfo").datagrid("getSelected");
    if (row != null) {
        if (row.SupplierId == undefined) {
            return;
        }
        $.messager.confirm("确认信息", "是否确认删除?", function (flag) {
            if (flag)
            {
                $.ajax({
                    url: '/BasicInfo/delSuppliser',
                    type: 'POST',
                    data: { id: row.SupplierId },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '删除成功',
                            });
                            $("#suppInfo").datagrid("reload");
                        }
                        else if (data == "0") {
                            $.messager.alert("提示信息", "删除失败", "info");
                            $("#suppInfo").datagrid("reload");
                        }
                        else {
                            $.messager.alert("提示信息", "获取参数失败,请刷新重试", "info");
                        }
                    }
                });

            }
        });
       
    }
    else {
        $.messager.alert("提示信息","请选择一行数据","info");
    }
}

//重置表单
function resetForm()
{
    $("#hid_SupplierId").val("");
    $("#addsuppInfo")[0].reset();
    $("#suppArea").combobox("setValue",'');
    $("#isSuppasc").prop("checked", true);
}

//关闭新增
function closesuppAdd()
{
    $("#addsuppInfo").dialog("close");
}

//获取编号
function GetCode()
{
    $.ajax({
        url: '/BasicInfo/GetSupperCode',
        async: false,
        type: 'POST',
        success: function (data) {
            $("#suppCode").val(data);
        }
    });
}

function CheckNum(obj)
{
    var test = /^[0-9]*$/;
    if(!test.test(obj.val()))
    {
        obj.val("0");

    }
}


function SuppChangeSize()
{
    var height=document.documentElement.clientHeight - 190;
    if (height > 400) {
        $("#supp_list").height(height);
    }

}