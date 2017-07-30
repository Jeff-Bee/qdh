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

var editIndex = undefined;
function endEditing() {
    if (editIndex == undefined) { return true }
    if ($('#pol_list').datagrid('validateRow', editIndex)) {
        $('#pol_list').datagrid('endEdit', editIndex);
        editIndex = undefined;
        return true;
    } else {
        return false;
    }
}
function onClickCell(index, field) {
    if (endEditing()) {
        $('#pol_list').datagrid('selectRow', index)
                .datagrid('editCell', { index: index, field: field });
        editIndex = index;
    }
}
$(function () {
    $("#pol_data").datebox({
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
    $("#pol_data").datebox("setValue", time.toLocaleTimeString());

    //进货单位下拉框
    $("#pol_PurchaseUnit").combobox({
        url: '/BasicInfo/GetSuppCbxList',
        textField: 'FullName',
        valueField: 'SupplierId',

    });
    //付款账户下拉框
    $("#payAccount").combobox({
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
    $("#payAccount").combobox("setValue", 0);

    //经手人下拉框
    $("#pol_guy").combobox({
        url: '/BasicInfo/GetGuy',
        textField: 'EmployeeFullName',
        valueField: 'EmployeeId',
        height: 22,
    });

    POL_List();
    //绑定快速查询
    $("#pol_product_search").combobox({
        url: '/BasicInfo/GetSearchTypeList',
        valueField: 'id',
        textField: 'text',
        onSelect: function (res) {

        },
    });
    $("#pol_product_search").combobox("select", 0);
    $("#enterNext").linkbutton();
    $("#returnUp").linkbutton();
    //进入下级按钮事件
    $("#enterNext").click(function () {
        if ($("#enterNext").linkbutton("options").disabled) {
            return;
        }
        var row = $("#pol_Product").datagrid("getSelected");
        if (row != null) {
            $("#pol_Product").datagrid("load", {
                type: row.ClassId,
            });
        }
        $("#enterNext").linkbutton("disable");
        $("#returnUp").linkbutton("disable");
    });
    $("#enterNext").linkbutton("disable");
    //返回上级按钮事件
    $("#returnUp").click(function () {
        if ($("#returnUp").linkbutton("options").disabled) {
            return;
        }
        var row = $("#pol_Product").datagrid("getSelected");
        if (row != null) {
            $.ajax({
                url: '/BasicInfo/GetPParentId',
                async: false,
                type: 'POST',
                data: { id: row.ParentId },
                success: function (data) {
                    if (data != "") {
                        $("#pol_Product").datagrid("load", {
                            type: data,
                        });
                    }
                    else {
                        $.messager.alert("提示信息", "获取数据失败,请刷新页面重试!", "info");
                    }
                }
            });


        }

        $("#enterNext").linkbutton("disable");
        $("#returnUp").linkbutton("disable");
    });
    $("#returnUp").linkbutton("disable");
    //选中操作
    $("#addInProduct").click(function () {
        var row = $("#pol_Product").datagrid("getSelected");
        if (row != null) {
            if (row.ProductId != "") {
                var num = $("#pol_list").datagrid("getRows").length;
                if (POL_InsertCount <= 10&&num==10) {
                    $("#pol_list").datagrid("deleteRow", 9);
                }
                $("#pol_list").datagrid("insertRow", {
                    index: (POL_InsertCount-1),
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

                $("#pol_list").datagrid("refreshRow", (POL_InsertCount - 1));
                POL_InsertCount++;
            }
            updateTotal();
        }
    });
    //获取按esc键的事件
    $(document).keyup(function (e) {
        var key = e.which;
        if (key == 27) {
            QuitTabs("商品进货");
            //$("#tabs").tabs('close', "进货单");
        }
    });

    //自适应高度
    POLChangeSize();


});
//添加商品数量
var POL_InsertCount = 1;

//获取进货单列表
function POL_List() {
    $("#pol_list").datagrid({
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
                    return "<div style=\"font-size: 0px; width: 16px; height: 16px; background-image: url(/images/del.png); background-position: 0px 0px; background-repeat: no-repeat;margin:0 auto;\" title=\"删除本行\" ></div>";//onclick=\"delPOLList(" + num + ")\"
                }
            }
        ]],
        onDblClickRow: function (index, data) {
            POL_Product_List();
        },
        onClickCell: function (index, field, value) {
            if (editIndex != undefined) {
                $("#pol_list").datagrid("endEdit", editIndex);
                var oldrow = $('#pol_list').datagrid('getData').rows[editIndex];
                oldrow.cash = (parseFloat(oldrow.nums) * parseFloat(oldrow.price)).toFixed(2);
                $("#pol_list").datagrid("updateRow", { index: editIndex, row: oldrow });
                $("#pol_list").datagrid("refreshRow", editIndex);
                updateTotal();
            }

            $("#pol_list").datagrid("selectRow", index);
            if (field != "Status") {
                var row = $("#pol_list").datagrid("getSelected");
                if (row != null && row.ProductId != "") {
                    onClickCell(index, field);
                }
            }
            else {
                var num = $("#pol_list").datagrid("getRows").length;
                if (num > 1) {
                    if (value == undefined)
                    {
                        POL_InsertCount--;
                    }
                    $("#pol_list").datagrid("deleteRow", index);
                    updateTotal();
                }
                else {
                    $.messager.alert("提示信息","剩最后一行了","info");
                }
            }
        }
    });
    var num = $("#pol_list").datagrid("getRows").length;
    for (var i = 0; i < 10 - num; i++) {
        $('#pol_list').datagrid('insertRow', {
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
function delPOLList(index) {
    console.log(index);
    if (index == -1) {
        var num = $("#pol_list").datagrid("getRows").length;
        $("#pol_list").datagrid("deleteRow", num-1);
    }
    else {
        $("#pol_list").datagrid("deleteRow", index);
    }
    updateTotal();
}

//选择商品弹框
function POL_Product_List() {

    $("#selectProduct").dialog({
        title: '库存商品选择框',
        width: 620,
        height: 500,
        modal: true
    });

    $("#pol_Product").datagrid({
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
             //{
             //    field: 'Quantity',
             //    title: '库存余量',
             //    width: 100,
             //},
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
            $("#enterNext").linkbutton("enable");
            $("#returnUp").linkbutton("enable");
            if (data.ProductId != "")//商品
            {
                console.log(data.ParentId);
                if (data.ParentId == "0") {
                    $("#returnUp").linkbutton("disable");
                }
                $("#enterNext").linkbutton("disable");
            }
            else {
                if (data.ParentId == "0") {
                    $("#returnUp").linkbutton("disable");
                }
                if (data.downId == "0") {
                    $("#enterNext").linkbutton("disable");
                }
            }
        },
        onDblClickRow: function (index, data) {
            if (data.ProductId == "" && data.downId != "0") {
                $("#pol_Product").datagrid("load", {
                    type: data.ClassId,
                });
            }
            else if (data.ProductId != "") {
                var row = data;
                var num = $("#pol_list").datagrid("getRows").length;
                if (POL_InsertCount <= 10 && num==10) {
                    $("#pol_list").datagrid("deleteRow", 9);
                }
                $("#pol_list").datagrid("insertRow", {
                    index: (POL_InsertCount - 1),
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
                POL_InsertCount++;
                updateTotal();
            }
        }
    });
    $("#pol_Product").datagrid("load", {
        type: $("#hid_pol_type").val(),
    });
}



//选择商品页面关闭
function POL_Close() {
    $("#selectProduct").dialog("close");
}

//选择商品搜索
function pol_add_search() {
    $("#pol_Product").datagrid("load", {
        type: $("#hid_pol_type").val(),
        searchType: $("#pol_product_search").val(),
        searchContent: $("#pol_add_scontent").val()
    });

}

//保存
function pol_Save() {
    //$.messager.defaults = { ok: "保存单据", cancel: "废弃退出" };
    $.messager.defaults.ok = "保存单据";
    $.messager.defaults.cancel = "废弃退出";
    $.messager.confirm("保存信息", "请选择对单据的处理,按《ESC》键放弃本次处理？", function (data) {
        //$.messager.defaults = { ok: "确定", cancel: "取消", width: 250 };
        $.messager.defaults.ok = "确定";
        $.messager.defaults.cancel = "取消";
        $.messager.defaults.width = "250";
        if (data) {
            if (checkForm()) {
                var json = "{";
                json += "\"SellerBuyInfo\":{";
                json += "\"BuyId\":\"0\",\"SellerId\":\"0\",\"DeptId\":\"0\",";
                json += "\"Code\":\"" + $("#pol_code").val() + "\",\"ComeDate\":\"" + $("#pol_data").datebox("getValue") + "\",\"EmployeeId\":\"" + $("#pol_guy").combobox("getValue") + "\",";
                json += "\"SupplierId\":\"" + $("#pol_PurchaseUnit").combobox("getValue") + "\",\"Remark\":\"" + $("#pol_abstract").val() + "\",\"PaymentAccount\":\"" + $("#payAccount").combobox("getValue") + "\",";
                json += "\"TotalAmount\":\"" + $("#pol_total_cash").text() + "\",\"Remark\":\"" + $("#pol_abstract").val() + "\",\"Notes\":\"" + $("#pol_translated").val() + "\",\"PaidAmount\":\"" + $("#payCount").val() + "\"";
                json += "},\"SellerBuyDetail\":[";

                var rows = $("#pol_list").datagrid("getRows");
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].ProductId != "") {
                        json += "{\"Index\":\"" + (i + 1) + "\",\"ProductId\":\"" + rows[i].ProductId + "\",\"Quantity\":\"" + rows[i].nums + "\",\"Price\":\"" + rows[i].price + "\"},";
                    }
                }
                if (json.substring(json.length - 1, 1) == ",") {
                    json = json.substring(0, json.length - 1);
                }
                json += "]";
                json += "}";
                $.ajax({
                    url: '/BasicInfo/POL_Save',
                    type: 'POST',
                    data: { str: json },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '保存成功',
                            });
                            POL_QuitTab();
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
            POL_QuitTab();
            //$("#tabs").tabs('close', "进货单");
        }
    });
}

//关闭tab
function POL_QuitTab()
{
    QuitTabs("商品进货");
}


//刷新本tab
function ReflashTab() {
    var currTab = $('#tabs').tabs('getTab', "进货单");
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
function checkForm() {

    if ($("#pol_data").datebox("getValue") == "") {
        $.messager.alert("提示信息", "录入时期不能为空!", "info");
        $("#pol_data").focus();
        return false;
    }
    else if ($("#pol_code").val() == "") {
        $.messager.alert("提示信息", "编号不能为空!", "info");
        $("#pol_code").focus();
        return false;
    }
    else if ($("#pol_PurchaseUnit").combobox("getValue") == "") {
        $.messager.alert("提示信息", "供货单位不能为空!", "info");
        //$("#pol_PurchaseUnit").focus();
        return false;
    }
    else if ($("#pol_guy").combobox("getValue") == "") {
        $.messager.alert("提示信息", "经手人不能为空!", "info");
        //$("#pol_guy").focus();
        return false;
    }
    //else if ($("#pol_abstract").val() == "") {
    //    $.messager.alert("提示信息", "摘要不能为空!", "info");
    //    $("#pol_abstract").focus();
    //    return false;
    //}
    //else if ($("#pol_translated").val() == "") {
    //    $.messager.alert("提示信息", "附加说明不能为空!", "info");
    //    $("#pol_translated").focus();
    //    return false;
    //}
    else if ($("#payAccount").combobox("getValue") == "") {
        $.messager.alert("提示信息", "付款账户不能为空!", "info");
        //$("#payAccount").focus();
        return false;
    }
    else {
        var rows = $("#pol_list").datagrid("getRows");
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
function updateTotal() {
    var totalNum = 0;
    var totalMoney = 0.00;
    var rows = $("#pol_list").datagrid("getRows");
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].ProductId != "") {
            totalNum += parseInt(rows[i].nums);
            totalMoney += parseFloat(rows[i].cash);
        }
    }
    $("#pol_total_num").text(totalNum); 
    $("#pol_total_cash").text(totalMoney.toFixed(2));
    $("#payCount").numberbox('setValue',totalMoney.toFixed(2));
}

function POLChangeSize() {
    var height = document.documentElement.clientHeight - 280;
    if (height > 400) {
        $("#pol_body").height(height);
    }

}