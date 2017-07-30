$(function () {
    $("#CustomerInfo").datagrid({
        url: '/BasicInfo/GetCustomerList',
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
                field: 'Code',
                title: '客户代码',
                width: 100,
            },

              {
                  field: 'FullName',
                  title: '客户名称',
                  width: 100,
              },
                  {
                      field: 'BuyerType',
                      title: '客户类型',
                      width: 100,
                      formatter: function (value) {
                          if (value != undefined)
                          {
                              switch (value) {
                                  case 0:
                                      return "线上买家";
                                      break;
                                  case 1:
                                      return "线下买家";
                                      break;
                                  case 2:
                                      return "线上线下买家";
                                      break;
                              }
                          }
                      }
                  },
              {
                  field: 'Phone',
                  title: '电话',
                  width: 100,
              },
               {
                   field: 'MobilePhone',
                   title: '手机',
                   width: 100,
               },
                {
                    field: 'ConstactPerson',
                    title: '联系人',
                    width: 100,
                },
                 {
                     field: 'PriceLevel',
                     title: '价格等级',
                     width: 100,
                     formatter: function (value) {
                         if (value != undefined && value != "") {
                             switch (value) {
                                 default:
                                 case "1":
                                     return "等级一";
                                     break;
                                 case "2":
                                     return "等级二";
                                     break;
                                 case "3":
                                     return "等级三";
                                     break;
                                 case "4":
                                     return "等级四";
                                     break;
                                 case "5":
                                     return "等级五";
                                     break;
                                 case "6":
                                     return "等级六";
                                     break;
                                 case "7":
                                     return "等级七";
                                     break;
                             }
                         }
                     }
                 },
                  {
                      field: 'Status',
                      title: '状态',
                      width: 100,
                      formatter: function (node) {
                          if (node != undefined) {
                              if (node == 0) {
                                  return "正常";
                              }
                              else {
                                  return "停用";
                              }
                          }
                      }
                  },
                   {
                       field: 'Address',
                       title: '客户地址',
                       width: 100,
                   },
                    {
                        field: 'Bank',
                        title: '开户银行',
                        width: 100,
                    }
        ]],
        onLoadSuccess: function () {
            FillCustomerRows();
        }
    });

    $("#supArea").combobox({
        url: '/BasicInfo/GetAreaList',
        textField: 'text',
        valueField: 'id',
        hasDownArrow: false,
        icons: [{
            iconCls: 'combo-more',
            handler: function () {
                CheckCustArea();
            }
        }]
    });

    $("#supBuyerType").combobox({
        textField: 'text',
        valueField: 'id',
        data: [
            {
                'id': '0',
                text: '线上买家'
            },
            {
                'id': '1',
                text: '线下买家'
            },
            {
                'id': '2',
                text: '线上线下买家'
            }
        ],
        onLoadSuccess: function () {
            $("#supBuyerType").combobox("setValue", 0);
        }
    });
    

    $("#isCustomerasc").click(function () {
        if ($("#isCustomerasc").prop("checked")) {
            GetCustomerCode();
        }
    });

    //地区搜索

    $("#custAreaList").datagrid({
        url: '/BasicInfo/GetSupAreaList',
        rownumbers: true,
        loadMsg: '数据加载中。。',
        field: true,
        striped: true,
        remoteSort: false,
        singleSelect: true,
        columns: [[
            {
                field: 'Code',
                title: '地区编号',
                width: 100,
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
                $("#custAreaCheck").linkbutton("enable");
            }
            else {
                $("#custAreaCheck").linkbutton("disable");
            }
        }
    });

    $("#custAreaListSearch").combobox({
        url: '/BasicInfo/SearchAreaCondition',
        valueField: 'id',
        textField: 'text',
        width: 100,
    });
    $("#custAreaListSearch").combobox("setValue", 0);
    $("#custAreaCheck").linkbutton({
        disabled: true,
    });

    //自适应高度
    CustomerChangeSize();
});


//当不够十行时补充十行空数据
function FillSupAreaSearchRows() {
    var data = $('#custAreaList').datagrid('getRows');
    if (data.length < 10) {
        for (var i = data.length; i < 10; i++) {
            $('#custAreaList').datagrid('appendRow', {});
        }
    }
}

//选中地区
function CheckCustArea() {
    $("#GetCustomerAreaList").dialog({
        title: '地区选中框',
        width: 270,
        height: 400,
        modal: true
    });
}

//关闭所属地区选择
function CloseCheckCustArea() {
    $("#GetCustomerAreaList").dialog("close");
}

//选择地区搜索
function searchCustArea() {
    $("#custAreaList").datagrid("load", {
        id: $("#custAreaListSearch").combobox("getValue"),
        text: $("#custAreaSearchContent").val(),
    });
}

//地区选择-选中操作
function CustAreaChecked() {
    var row = $("#custAreaList").datagrid("getSelected");
    if (row != null) {
        $("#supArea").combobox("setValue", row.AreaId);
        CloseCheckCustArea();
    }
}


//当不够十行时补充十行空数据
function FillCustomerRows() {
    var data = $('#CustomerInfo').datagrid('getData');
    var pageopt = $('#CustomerInfo').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#CustomerInfo').datagrid('appendRow', {});
        }
    }
}


function searchUserInfo() {

}

//用户新增
function addUserInfo() {
    CustomerFormReset();
    $("#isCustomerasc").prop("checked", true);
    GetCustomerCode();
    $("#userInfo").dialog({
        title: '客户信息-信息框',
        width: 400,
        height: 430,
        modal: true
    });
}


//新增保存
function addUserInfoSave(type) {
    if ($("input[name=uname]").val() == "") {
        $.messager.alert("提示信息", "客户名称不能为空", "info");
        $("input[name=uname]").focus();
        return;
    }
    else if ($("#supCode").val() == "") {
        $.messager.alert("提示信息", "客户编号不能为空", "info");
        $("#supCode").focus();
        return;
    }
    else if ($("#supPhone").val() == "") {
        $.messager.alert("提示信息", "手机不能为空", "info");
        $("#supPhone").focus();
        return;
    }
    else if ($("#supArea").combobox("getValue") == "") {
        $.messager.alert("提示信息", "所属地区不能为空,如果还未定义地区,请先定义地区", "info");
        $("#supPhone").focus();
        return;
    }
    else if ($("#supAdress").val() == "") {
        $.messager.alert("提示信息", "地址不能为空", "info");
        //$("#supAdress").focus();
        return;
    }
    else {
        var json = '';
        json += "{";
        json += "\"BuyerId\":\"" + ($("#supCustomerId").val() == "" ? "0" : $("#supCustomerId").val()) + "\",\"FullName\":\"" + $("input[name=uname]").val() + "\",\"Code\":\"" + $("#supCode").val() + "\",\"AreaId\":\"" + $("#supArea").combobox("getValue") + "\",\"TaxNumber\":\"" + $("#supTaxnum").val() + "\",";//" + $("#supArea").val()
        json += "\"ConstactPerson\":\"" + $("#supContact").val() + "\",\"Phone\":\"" + $("#supTel").val() + "\",\"MobilePhone\":\"" + $("#supPhone").val() + "\",\"Email\":\"" + $("#supEmail").val() + "\",";
        json += "\"Address\":\"" + $("#supAdress").val() + "\",\"Bank\":\"" + $("#supAccount").val() + "\",\"BankAccount\":\"" + $("#supBank").val() + "\",\"Wechat\":\"" + $("#supWX").val() + "\",\"PriceLevel\":\"" + $("#priceLevel option:selected").val() + "\",\"Notes\":\"" + $("#supNote").val() + "\",\"BuyerType\":\"" + ($("#supBuyerType").combobox("getValue") == "" ? "2" : $("#supBuyerType").combobox("getValue")) + "\"";
        json += "}";
        $.ajax({
            url: '/BasicInfo/CorrespondentsAdd',
            type: 'POST',
            data: { str: json },
            success: function (data) {
                if (data == "1") {
                    $.messager.show({
                        title: '提示信息',
                        msg: '保存成功'
                    });
                    // $.messager.alert("提示信息", "保存成功", "info");
                    if (type == 1) {
                        CustomerFormReset();
                        $("#isCustomerasc").prop("checked", true);
                        GetCustomerCode();
                    }
                    else {
                        CustomerFormReset();
                        $("#userInfo").dialog("close");
                    }
                    $("#CustomerInfo").datagrid("reload");
                }
                else if (data == "2") {
                    $.messager.alert("提示信息", "code重复", "info");
                }
                else if (data == "0") {
                    $.messager.alert("提示信息", "保存失败", "info");
                }
                else {
                    $.messager.alert("提示信息", "参数获取失败,请刷新后重试", "info");
                }
            }
        });
    }
}


//关闭新增
function closeCustomerAdd() {
    $("#userInfo").dialog("close");
}

//复制新增,type=1   修改type=0
function editCustomerForm(type) {
    var row = $("#CustomerInfo").datagrid("getSelected");
    if (row != null) {
        if (row.BuyerId == undefined) {
            return;
        }
        if (type == 0) {
            $("#supCustomerId").val(row.BuyerId);
            $("#supCode").val(row.Code);
            $("#isCustomerasc").prop("checked", true);
            $("#customerAdd").hide();
        }
        else {
            $("#supCustomerId").val("");
            $("#isCustomerasc").prop("checked", true);
            GetCustomerCode();
            $("#customerAdd").show();
        }
        $("input[name=uname]").val(row.FullName);
        $("#supPhone").val(row.MobilePhone);
        $("#supContact").val(row.ConstactPerson);
        $("#supTaxnum").val(row.TaxNumber);
        $("#supArea").combobox("setValue", row.AreaId);
        $("#supBuyerType").combobox("setValue", row.BuyerType);

        $("#supTel").val(row.Phone);
        $("#supWX").val(row.Wechat);
        $("#supEmail").val(row.Email);
        $("#priceLevel").val(row.PriceLevel);
        $("#supAdress").val(row.Address);
        $("#supAccount").val(row.Bank);
        $("#supBank").val(row.BankAccount);
        $("#supNote").val(row.Notes);
        $("#userInfo").dialog({
            title: '客户信息-信息框',
            width: 400,
            height: 430,
            modal: true
        });
    }
    else {
        $.messager.alert("提示信息", "请先选择一行数据", "info");
    }
}

//删除一行
function delCustomer() {
    var row = $("#CustomerInfo").datagrid("getSelected");
    if (row != null) {
        if (row.BuyerId == undefined) {
            return;
        }
        $.messager.confirm("警告信息", "您是否确认删除该客户信息?", function (flag) {
            if (flag) {
                $.ajax({
                    url: '/BasicInfo/delCustomerInfo',
                    type: 'POST',
                    data: { id: row.BuyerId },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '删除成功!',
                            });
                            $("#CustomerInfo").datagrid("reload");
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
        $.messager.alert("提示信息", "请先选择一行数据", "info");
    }
}

//重置表单
function CustomerFormReset() {
    $("#supCustomerId").val("");
    $("#userInfo")[0].reset();
    $("#supArea").combobox("setValue", "");
    $("#customerAdd").show();
    $("#supBuyerType").combobox("setValue", 0);
}

//获取code
function GetCustomerCode() {
    $.ajax({
        url: '/BasicInfo/GetCustomerCode',
        type: 'Post',
        success: function (data) {
            $("#supCode").val(data);
        }
    });
}

function CustomerChangeSize() {
    var height = document.documentElement.clientHeight - 190;
    if (height > 400) {
        $("#Customer_List").height(height);
    }

}