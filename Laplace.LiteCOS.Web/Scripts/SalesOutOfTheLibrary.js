$.extend($.fn.datagrid.methods, {
    editCell: function (jq, param) {
        return jq.each(function () {
            var opts = $(this).datagrid('options');
            var fields = $(this).datagrid('getColumnFields', true).concat($(this).datagrid('getColumnFields'));
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).datagrid('getColumnOption', fields[i]);
                col.editor1 = col.editor;
                if (fields[i] != param.field) {
                    col.editor = null;
                }
            }
            $(this).datagrid('beginEdit', param.index);
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).datagrid('getColumnOption', fields[i]);
                col.editor = col.editor1;
            }
        });
    }
});

var stl_editIndex = undefined;
function stl_endEditing() {
    if (stl_editIndex == undefined) { return true }
    if ($('#stl_list').datagrid('validateRow', stl_editIndex)) {
        $('#stl_list').datagrid('endEdit', stl_editIndex);
        stl_editIndex = undefined;
        return true;
    } else {
        return false;
    }
}
function stl_onClickCell(index, field) {
    if (stl_endEditing()) {
        $('#stl_list').datagrid('selectRow', index)
                .datagrid('editCell', { index: index, field: field });
        stl_editIndex = index;
    }
}
$(function () {
    $("#stl_data").datebox({
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
    $("#stl_data").datebox("setValue", time.toLocaleTimeString());

    //买家信息下拉框
    $("#stl_PurchaseUnit").combobox({
        url: '/BasicInfo/GetPOSCustomer?Stype=underline&IsAll=1',
        type: 'POST',
        textField: 'text',
        valueField: 'id',
        

    });
    //付款账户下拉框
    $("#stl_payAccount").combobox({
        textField: 'text',
        valueField: 'id',
        height: 22,
        data: [
            {
                id: '0',
                text: '默认账户',
            },
        ],
    });
    $("#stl_payAccount").combobox("setValue", 0);

    //经手人下拉框
    $("#stl_guy").combobox({
        url: '/BasicInfo/GetGuy',
        textField: 'EmployeeFullName',
        valueField: 'EmployeeId',
        height: 22,
    });

    stl_List();
    //绑定快速查询
    $("#stl_product_search").combobox({
        url: '/BasicInfo/GetSearchTypeList',
        valueField: 'id',
        textField: 'text',
        onSelect: function (res) {

        },
    });
    $("#stl_product_search").combobox("select", 0);
    $("#stl_enterNext").linkbutton();
    $("#stl_returnUp").linkbutton();
    //进入下级按钮事件
    $("#stl_enterNext").click(function () {
        if ($("#stl_enterNext").linkbutton("options").disabled) {
            return;
        }
        var row = $("#stl_Product").datagrid("getSelected");
        if (row != null) {
            $("#stl_Product").datagrid("load", {
                type: row.ClassId,
            });
        }
        $("#stl_enterNext").linkbutton("disable");
        $("#stl_returnUp").linkbutton("disable");
    });
    $("#stl_enterNext").linkbutton("disable");
    //返回上级按钮事件
    $("#stl_returnUp").click(function () {
        if ($("#stl_returnUp").linkbutton("options").disabled) {
            return;
        }
        var row = $("#stl_Product").datagrid("getSelected");
        if (row != null) {
            $.ajax({
                url: '/BasicInfo/GetPParentId',
                async: false,
                type: 'POST',
                data: { id: row.ParentId },
                success: function (data) {
                    if (data != "") {
                        $("#stl_Product").datagrid("load", {
                            type: data,
                        });
                    }
                    else {
                        $.messager.alert("提示信息", "获取数据失败,请刷新页面重试!", "info");
                    }
                }
            });


        }

        $("#stl_enterNext").linkbutton("disable");
        $("#stl_returnUp").linkbutton("disable");
    });
    $("#stl_returnUp").linkbutton("disable");
    //选中操作
    $("#addstlInProduct").click(function () {
        var row = $("#stl_Product").datagrid("getSelected");
        if (row != null) {
            if (row.ProductId != "") {
                var num = $("#stl_list").datagrid("getRows").length;
                if (Soot_InsertCount <= 10 && num==10) {
                    $("#stl_list").datagrid("deleteRow", 9);
                }
                $("#stl_list").datagrid("insertRow", {
                    index: (Soot_InsertCount - 1),
                    row: {
                        ProductId: row.ProductId,
                        ProductFullName: row.ProductFullName,
                        Package: row.Package,
                        ProductSpec: row.ProductSpec,
                        ProductUnit: row.ProductUnit,
                        nums: 1,
                        price: row.Price1,
                        cash: row.Price1,
                        Status: row.Status,
                        notes: row.notes

                    }
                });
                Soot_InsertCount++;
            }
            updateStlTotal();
        }
    });
    //获取按esc键的事件
    $(document).keyup(function (e) {
        var key = e.which;
        if (key == 27) {
            QuitTabs("销售出库");
            //$("#tabs").tabs('close', "进货单");
        }
    });

    //自适应高度
    STLChangeSize();


});

//添加商品数量
var Soot_InsertCount = 1;

//获取进货单列表
function stl_List() {
    $("#stl_list").datagrid({
        singleSelect: true,
        rownumbers: true,
        striped: true,
        loadMsg: '数据加载中。。',
        pagination: false,
        fixed: true,
        fitColumns: true,
        columns: [[
            {
                field: 'ProductId',
                title: '商品编号',
                width: 100
            },
            {
                field: 'ProductFullName',
                title: '商品名称',
                width: 100
            },
            {
                field: 'Package',
                title: '封装',
                width: 100
            },
            {
                field: 'ProductSpec',
                title: '规格',
                width: 100
            },
            {
                field: 'ProductUnit',
                title: '单位',
                width: 100
            },
            {
                field: 'nums',
                title: '数量',
                width: 100,
                editor: { type: 'numberbox', options: { min: 0, } },
            },
            {
                field: 'price',
                title: '单价',
                width: 100,
                editor: {
                    type: 'numberbox', options: {
                        min: 0,
                        precision: 2
                    }
                },
            },
            {
                field: 'cash',
                title: '金额',
                width: 100,
                formatter: function (row, data, index) {
                    if (row != undefined && row != "")
                    {
                        return row;
                    }
                }
            },
            {
                field: 'notes',
                title: '备注',
                width: 100
            },
            {
                field: 'Status',
                title: '删除',
                width: 40,
                formatter: function (row, data, index) {
                    return "<div style=\"font-size: 0px; width: 16px; height: 16px; background-image: url(/images/del.png); background-position: 0px 0px; background-repeat: no-repeat;margin:0 auto;\" title=\"删除本行\" ></div>";//onclick=\"delStlList(" + index + ")\"
                }
            }
        ]],
        onDblClickRow: function (index, data) {
            STL_Product_List();
        },
        onClickCell: function (index, field, value) {
            if (stl_editIndex != undefined) {
                $("#stl_list").datagrid("endEdit", stl_editIndex);
                var oldrow = $('#stl_list').datagrid('getData').rows[stl_editIndex];
                oldrow.cash = (parseFloat(oldrow.nums) * parseFloat(oldrow.price)).toFixed(2);
                $("#stl_list").datagrid("updateRow", { index: stl_editIndex, row: oldrow });
                $("#stl_list").datagrid("refreshRow", stl_editIndex);
                updateStlTotal();
            }

            $("#stl_list").datagrid("selectRow", index);
            if (field != "Status") {
                var row = $("#stl_list").datagrid("getSelected");
                if (row != null && row.ProductId != "") {
                    stl_onClickCell(index, field);
                }
            }
            else {
                var num = $("#stl_list").datagrid("getRows").length;
                if (num > 1) {
                    if (value == undefined) {
                        Soot_InsertCount--;
                    }
                    $("#stl_list").datagrid("deleteRow", index);
                    updateStlTotal();
                }
                else {
                    $.messager.alert("提示信息", "剩最后一行了", "info");
                }
                
            }
        }
    });
    var num = $("#stl_list").datagrid("getRows").length;
    for (var i = 0; i < 10 - num; i++) {
        $('#stl_list').datagrid('insertRow', {
            index: i,	// 索引从0开始
            row: {
                ProductId: '',
                ProductFullName: '',
                Package: '',
                ProductSpec: '',
                ProductUnit: '',
                nums: '',
                price: '',
                cash: '',
                notes: '',
                Status: ''
            }
        });
    }

}

//删除一行根据index
function delStlList(index) {
    $("#stl_list").datagrid("deleteRow", index);
    updateStlTotal();
}

//选择商品弹框
function STL_Product_List() {

    $("#stl_selectProduct").dialog({
        title: '库存商品选择框',
        width: 620,
        height: 500,
        modal: true
    });

    $("#stl_Product").datagrid({
        url: '/BasicInfo/GetPOLProduct',
        singleSelect: true,
        rownumbers: true,
        striped: true,
        loadMsg: '数据加载中。。',
        pagination: true,
        fixed: true,
        fitColumns: true,
        columns: [[
            {
                field: 'ProductId',
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
                 field: 'Quantity',
                 title: '库存余量',
                 width: 100,
             },
             {
                 field: 'ProductUnit',
                 title: '单位',
                 width: 100,
             },
             {
                 field: 'Place',
                 title: '产地',
                 width: 100,
             },
            {
                field: 'BarCode',
                title: '条码',
                width: 100,
            }
        ]],
        onSelect: function (index, data) {
            $("#stl_enterNext").linkbutton("enable");
            $("#stl_returnUp").linkbutton("enable");
            if (data.ProductId != "")//商品
            {
                if (data.ParentId == "0") {
                    $("#stl_returnUp").linkbutton("disable");
                }
                $("#stl_enterNext").linkbutton("disable");
            }
            else {
                if (data.ParentId == "0") {
                    $("#stl_returnUp").linkbutton("disable");
                }
                if (data.downId == "0") {
                    $("#stl_enterNext").linkbutton("disable");
                }
            }
        },
        onDblClickRow: function (index, data) {
            if (data.ProductId == "" && data.downId != "0") {
                $("#stl_Product").datagrid("load", {
                    type: data.ClassId,
                });
            }
            else if (data.ProductId != "") {
                var row = data;
                var num = $("#stl_list").datagrid("getRows").length;
                if (Soot_InsertCount <= 10 && num==10) {
                    $("#stl_list").datagrid("deleteRow", 9);
                }
                $("#stl_list").datagrid("insertRow", {
                    index: (Soot_InsertCount - 1),
                    row: {
                        ProductId: row.ProductId,
                        ProductFullName: row.ProductFullName,
                        Package: row.Package,
                        ProductSpec: row.ProductSpec,
                        ProductUnit: row.ProductUnit,
                        nums: 1,
                        price: row.Price1,
                        cash: row.Price1,
                        Status: row.Status,
                        notes: row.notes

                    }
                });
                Soot_InsertCount++;
                updateStlTotal();
            }
        }
    });
    $("#stl_Product").datagrid("load", {
        type: $("#hid_stl_type").val(),
    });
}



//选择商品页面关闭
function STL_Close() {
    $("#stl_selectProduct").dialog("close");
}

//选择商品搜索
function stl_add_search() {
    $("#stl_Product").datagrid("load", {
        type: $("#hid_stl_type").val(),
        searchType: $("#stl_product_search").val(),
        searchContent: $("#stl_add_scontent").val()
    });

}

//保存
function stl_Save() {
    $.messager.defaults.ok = "保存单据";
    $.messager.defaults.cancel = "废弃退出";
    $.messager.confirm("保存信息", "请选择对单据的处理,按《ESC》键放弃本次处理？", function (data) {
        $.messager.defaults.ok = "确定";
        $.messager.defaults.cancel = "取消";
        $.messager.defaults.width = "250";
        if (data) {
            if (stl_checkForm()) {
                var json = "{";
                json += "\"SalersOrderRun\":{";
                json += "\"OrderId\":\"0\",\"SellerId\":\"0\",";
                json += "\"Code\":\"" + $("#stl_code").val() + "\",\"OrderDate\":\"" + $("#stl_data").datebox("getValue") + "\",\"RMan\":\"" + $("#stl_guy").combobox("getValue") + "\",";
                json += "\"BuyerId\":\"" + $("#stl_PurchaseUnit").combobox("getValue") + "\",\"PayAmount\":\"" + $("#stl_payCount").val() + "\",\"ProductQuantity\":\"" + $("#stl_total_num").text() + "\",";
                json += "\"Amount\":\"" + $("#stl_total_cash").text() + "\",\"Notes\":\"" + $("#stl_translated").val() + "\",\"Summary\":\"" + $("#stl_abstract").val() + "\"";
                json += "},\"SalersOrderDetail\":[";

                var rows = $("#stl_list").datagrid("getRows");
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].ProductId != "") {
                        json += "{\"Index\":\"" + (i + 1) + "\",\"ProductId\":\"" + rows[i].ProductId + "\",\"ProductQuantity\":\"" + rows[i].nums + "\",\"ProductPrice\":\"" + rows[i].price + "\",\"ProductUnit\":\"" + rows[i].ProductUnit + "\"},";
                    }
                }
                if (json.substring(json.length - 1, 1) == ",") {
                    json = json.substring(0, json.length - 1);
                }
                json += "]";
                json += "}";
                $.ajax({
                    url: '/BasicInfo/STL_Save',
                    type: 'POST',
                    data: { str: json },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '保存成功',
                            });
                            stl_ReflashTab();
                            //$("#tabs").tabs("close", "进货单");
                            //$("#tabs").tabs("add", {
                            //    title: '进货单',
                            //    closable: true,
                            //    href: "/BasicInfo/PurchaseOrderList",
                            //});
                        }
                        else if (data == "0") {
                            $.messager.alert("提示信息", "保存失败", "info");
                        }
                        else if (data == "2") {
                            $.messager.alert("提示信息", "code重复", "info");
                        }
                        else if (data == "3") {
                            $.messager.alert("提示信息", "更新库存表出错!", "info");
                        }
                        else {
                            $.messager.alert("提示信息", "获取参数失败,请刷新重试", "info");
                        }
                    },
                });
            }
        }
        else {
            STL_QuitTab();
            
        }
    });
}

//关闭tab
function STL_QuitTab() {
    QuitTabs("销售出库");
}


//刷新本tab
function stl_ReflashTab() {
    var currTab = $('#tabs').tabs('getTab', "销售出库");
    var url = $(currTab.panel('options')).attr('href');
    $('#tabs').tabs('update', {
        tab: currTab,
        options: {
            href: url
        }
    });
    currTab.panel('refresh');
}

//保存检查
function stl_checkForm() {

    if ($("#stl_data").datebox("getValue") == "") {
        $.messager.alert("提示信息", "录入时期不能为空!", "info");
        $("#stl_data").focus();
        return false;
    }
    else if ($("#stl_code").val() == "") {
        $.messager.alert("提示信息", "编号不能为空!", "info");
        $("#stl_code").focus();
        return false;
    }
    else if ($("#stl_PurchaseUnit").combobox("getValue") == "") {
        $.messager.alert("提示信息", "买家不能为空!", "info");
        //$("#pol_PurchaseUnit").focus();
        return false;
    }
    else if ($("#stl_guy").combobox("getValue") == "") {
        $.messager.alert("提示信息", "经手人不能为空!", "info");
        return false;
    }
    else if ($("#stl_payAccount").combobox("getValue") == "") {
        $.messager.alert("提示信息", "付款账户不能为空!", "info");
        return false;
    }
    else {
        var rows = $("#stl_list").datagrid("getRows");
        if (rows.length > 0) {
            var isnull = true;
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].ProductId != "") {
                    isnull = false;
                    break;
                }
            }
            if (!isnull) {
                return true;
            }
            else {
                $.messager.alert("提示信息", "请选择商品!", "info");
                return false;
            }
        }
        else {
            $.messager.alert("提示信息", "请选择商品!", "info");
            return false;
        }
    }
}

//更新总计数量和金额
function updateStlTotal() {
    var totalNum = 0;
    var totalMoney = 0.00;
    var rows = $("#stl_list").datagrid("getRows");
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].ProductId != "") {
            totalNum += parseInt(rows[i].nums);
            totalMoney += parseFloat(rows[i].cash);
        }
    }
    $("#stl_total_num").text(totalNum);
    $("#stl_total_cash").text(totalMoney.toFixed(2));
    $("#stl_payCount").numberbox("setValue",totalMoney.toFixed(2));
}

function STLChangeSize() {
    var height = document.documentElement.clientHeight - 280;
    if (height > 400) {
        $("#stl_body").height(height);
    }

}