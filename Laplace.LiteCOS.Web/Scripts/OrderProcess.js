$(function () {
    $("#OP_ProductList").datagrid({
        url: '/BasicInfo/GetOPProductList',
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
                title: '商品编号',
                field: 'ProductCode',
                width: 100,
            },
             {
                 title: '商品名称',
                 field: 'ProductName',
                 width: 100,
             },
              {
                  title: '封装',
                  field: 'Package',
                  width: 100,
              },
               {
                   title: '规格',
                   field: 'ProductSpec',
                   width: 100,
               },
                {
                    title: '单位',
                    field: 'ProductUnit',
                    width: 100,
                },
                 {
                     title: '数量',
                     field: 'ProductQuantity',
                     width: 100,
                 },
                 {
                     title: '单价',
                     field: 'ProductPrice',
                     width: 100,
                 },
                 {
                     title: '金额',
                     field: 'TotalPrice',
                     width: 100,
                 }
                 ,
                 {
                     title: '备注',
                     field: 'Notes',
                     width: 100,
                 }
        ]],
        onLoadSuccess: function () {
            FillOPRows();
        },
    });

    $("#OP_ProductList").datagrid("load", {
        OrderId: $("#hid_OP_OrderId").val()
    });
    //付款账户下拉框
    $("#op_payAccount").combobox({
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
    $("#op_payAccount").combobox("setValue", 0);
    $("#op_payAccount").combobox("disable");

    //获取按esc键的事件
    $(document).keyup(function (e) {
        var key = e.which;
        if (key == 27) {

            $("#tabs").tabs('close', "订单处理");
        }
    });

    $("#OP_CarList").datagrid({
        url: '/BasicInfo/GetOPCars',
        rownumbers: true,
        loadMsg: '数据加载中。。',
        striped: true,
        remoteSort: false,
        fixed: true,
        fitColumns: true,
        singleSelect: true,
        columns: [[
            {
                title: '车辆编号',
                field: 'Code',
                width: 100,
            },
             {
                 title: '车辆名称',
                 field: 'Name',
                 width: 100,
             }
        ]],
    });
    init_op();

    //自适应高度
    OPChangeSize();
});
//打印
function ExportOP() {
    $("#printIframe")[0].contentWindow.print();
    return false;
}


//删除订单中的商品
function delOPList(row,index) {
    if (row != undefined && row != "") {
        $.messager.confirm("确认信息", "您确认要删除该商品信息么", function (flag) {
            if (flag)
            {
                $("#OP_ProductList").datagrid("deleteRow", index);
                var rows = $("#OP_ProductList").datagrid("getData");
                var num = 0;
                var price = 0.00;
                for (var i = 0; i < rows.total; i++) {
                    if (rows.rows[i] != null && rows.rows[i].ProductCode != undefined) {
                        num += rows.rows[i].ProductQuantity;
                        price += rows.rows[i].TotalPrice;
                    }
                }
                $("#op_TotalPrice").text(price.toFixed(2));
                $("#op_totalNum").text(num);
            }
        });
    }
    else {
        var num = $("#OP_ProductList").datagrid("getRows").length-1;
        $("#OP_ProductList").datagrid("deleteRow", num);
    }
}

//显示订单信息
function init_op() {
    if ($("#hid_OP_OrderId").val() == "")
    {
        $.messager.alert("提示信息", "获取订单编号失败", "info");
        return;
    }
    $.ajax({
        url: '/BasicInfo/GetOrderInfo',
        type: 'POST',
        async: false,
        data: { OrderId: $("#hid_OP_OrderId").val() },
        success: function (data) {
            if (data != "") {
                var json = eval("(" + data + ")");
                $("#OP_date").text(GetDate(json.OrderDate));
                $("#OP_code").val(json.Code);
                $("#OP_code").attr("disabled",true);
                $("#OP_customer_Name").val(json.CompanyName);
                $("#OP_customer_Name").attr("disabled",true);
                $("#OP_abstract").val(json.Notes);
                $("#OP_OrderStatus").css("color", "");
                $("#OP_OrderStatus").attr("disabled", false);

                //先初始化各个按钮
                $("#op_orderQUit").linkbutton();//取消订单
                $("#op_FirmOrder").linkbutton();//确认订单
                $("#op_orderDeliver").linkbutton();//发货
                $("#op_resumedOrder").linkbutton();//恢复订单
                $("#op_imposedOrderFinish").linkbutton();//强制确认收货
                $("#op_orderFinish").linkbutton();//确认收款


                $("#op_orderQUit").linkbutton("enable");
                $("#op_orderDeliver").linkbutton("enable");
                switch (json.OrderState) {
                    case 0:
                        $("#OP_OrderStatus").val("待确认");
                        $("#OP_OrderStatus").css("color", "red");
                        $("#op_orderDeliver").linkbutton("disable");//发货
                        $("#op_resumedOrder").linkbutton("disable");//恢复订单
                        $("#op_imposedOrderFinish").linkbutton("disable");//强制确认收货
                        $("#op_orderFinish").linkbutton("disable");//确认收款
                        break;
                    case 12:
                        $("#OP_OrderStatus").val("已发货");
                        //$("#op_orderQUit").hide();
                        $("#op_orderDeliver").linkbutton("disable");

                        $("#op_resumedOrder").linkbutton("disable");
                        OP_GetForcedReceipt();
                        $("#op_FirmOrder").linkbutton("disable");
                        $("#op_orderQUit").linkbutton("disable");
                        if (json.PayState != 0) {
                            $("#op_orderFinish").linkbutton("disable");
                        }
                        break;
                    case 100:
                        $("#OP_OrderStatus").val("完成");
                        $("#op_orderDeliver").linkbutton("disable");//发货
                        $("#op_resumedOrder").linkbutton("disable");//恢复订单
                        $("#op_imposedOrderFinish").linkbutton("disable");//强制确认收货
                        $("#op_orderFinish").linkbutton("disable");//确认收款
                        $("#op_orderQUit").linkbutton("disable");//取消订单
                        $("#op_FirmOrder").linkbutton("disable");//确认订单
                        break;
                    case 11:
                        $("#OP_OrderStatus").val("已取消");
                        $("#op_orderDeliver").linkbutton("disable");
                        $("#op_imposedOrderFinish").linkbutton("disable");
                        $("#op_orderFinish").linkbutton("disable");
                        $("#op_FirmOrder").linkbutton("disable");
                        $("#op_orderQUit").linkbutton("disable");
                        break;
                    case 10:
                        $("#OP_OrderStatus").val("已确认");
                        $("#op_resumedOrder").linkbutton("disable");
                        OP_GetForcedReceipt();
                        if (json.PayState != 0) {
                            $("#op_orderFinish").linkbutton("disable");
                        }
                        $("#op_FirmOrder").linkbutton("disable");
                        $("#op_orderQUit").linkbutton("disable");
                        break;
                    case 2:
                        $("#OP_OrderStatus").val("确认收货");
                        $("#op_orderDeliver").linkbutton("disable");
                        $("#op_resumedOrder").linkbutton("disable");
                        OP_GetForcedReceipt();
                        $("#op_FirmOrder").linkbutton("disable");
                        $("#op_orderQUit").linkbutton("disable");
                        if (json.PayState != 0) {
                            $("#op_orderFinish").linkbutton("disable");
                        }
                        break;
                    case 1:
                        $("#OP_OrderStatus").val("买家已取消");
                        $("#op_orderDeliver").linkbutton("disable");//发货
                        $("#op_resumedOrder").linkbutton("disable");//恢复订单
                        $("#op_imposedOrderFinish").linkbutton("disable");//强制确认收货
                        $("#op_orderFinish").linkbutton("disable");//确认收款
                        $("#op_orderQUit").linkbutton("disable");//取消订单
                        $("#op_FirmOrder").linkbutton("disable");//确认订单
                        break;
                }
                $("#OP_OrderStatus").attr("disabled", true);

                $("#op_TotalPrice").text(json.Amount);
                //默认收款
                $("#op_payCount").val(json.Amount);
                //$("#op_hid_TPrice").val(json.Amount);
                $("#op_payCount").attr("disabled", true);
                //确认收款页面显示
                $("#Confirm_Amount").val(json.Amount);
                $("#Confirm_Amount").attr("disabled", true);


                $("#op_totalNum").text(json.ProductQuantity);
                
            }
            else {

            }
        }
    });
}

//当不够十行时补充十行空数据
function FillOPRows() {
    var data = $('#OP_ProductList').datagrid('getData');
    var pageopt = $('#OP_ProductList').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#OP_ProductList').datagrid('appendRow', {});
        }
    }
}

//保存
function op_Save() {
    $.messager.defaults.ok = "保存单据";
    $.messager.defaults.cancel = "废弃退出";
    $.messager.confirm("保存信息", "请选择对单据的处理,按《ESC》键放弃本次处理？", function (data) {
        $.messager.defaults = { ok: "确定", cancel: "取消", width: 250 };
        $.messager.defaults.ok = "确定";
        $.messager.defaults.cancel = "取消";
        $.messager.defaults.width = "250";
        if (data) {
            //if ($("#op_payCount").numberbox("getValue") == "")
            //{
            //    $.messager.alert("提示信息", "付款金额不能为空", "info");
            //    return false;
            //}
            //var rows = $("#OP_ProductList").datagrid("getData");
            //if (rows.total <= 0)
            //{
            //    $.messager.alert("提示信息", "订单必须包含商品", "info");
            //    return false;
            //}
            //var num = 0;
            //var pids = "";
            //for (var i = 0; i < rows.total; i++)
            //{
            //    if (rows.rows[i] != null && rows.rows[i].ProductCode != undefined)
            //    {
            //        num++;
            //        pids += rows.rows[i].ProductId + ",";
            //    }
            //}
            //if (num <= 0)
            //{
            //    $.messager.alert("提示信息", "订单必须包含商品", "info");
            //    return false;
            //}
            $.ajax({
                url: '/BasicInfo/OP_Order_Save',
                type: 'POST',
                //data: { id: $("#hid_OP_OrderId").val(), payCount: $("#op_payCount").numberbox("getValue"), totalNum: $("#op_totalNum").text(), totalPrice: $("#op_TotalPrice").text(),pids:pids},
                data: { id: $("#hid_OP_OrderId").val(), abstract: $("#OP_abstract").val() },
                success: function (data) {
                    if (data == "1")
                    {
                        $.messager.show({
                            title: '提示',
                            msg: "保存成功！",
                        });
                        SaveSuccess();
                    }
                    else if (data == "2") {
                        $.messager.alert("警告信息", "保存出错", "info");
                    }
                    else {
                        $.messager.alert("警告信息", "获取参数错误", "info");
                    }
                }
            });
        }
        else{//退出
            SaveSuccess();
        }
    });
}

//取消订单
function OP_unsubscribe() {
    if ($("#op_orderQUit").linkbutton('options').disabled == true) {
        return;
    }
    $.messager.confirm("确认信息", "您确认要退订么", function (flag) {
        if (flag)
        {
            if ($("#hid_OP_OrderId").val() == "") {
                $.messager.alert("提示信息", "获取订单编号错误", "info");
                return;
            }
            $.ajax({
                url: '/BasicInfo/UnSubscribeOrder',
                type: 'POST',
                data: { id: $("#hid_OP_OrderId").val() },
                success: function (data) {
                    if (data == "1") {
                        $.messager.show({
                            title: '提示',
                            msg: "订单取消成功！",
                        });
                        SaveSuccess();
                    }
                    else if (data == "0") {
                        $.messager.alert("警告信息", "订单取消失败", "info");
                    }
                    else {
                        $.messager.alert("警告信息", "获取参数失败", "info");
                    }
                },
            });
        }
    })
   
}

//确认收款
function OP_OrderFinish() {
    if ($("#op_orderFinish").linkbutton('options').disabled == true) {
        return;
    }
    if ($("#hid_OP_OrderId").val() == "") {
        $.messager.alert("提示信息", "获取订单编号错误", "info");
        return;
    }
    $.ajax({
        url: '/BasicInfo/OrderFinish',
        type: 'POST',
        data: { id: $("#hid_OP_OrderId").val() },
        success: function (data) {
            if (data == "1") {
                $.messager.show({
                    title: '提示',
                    msg: "确认收款成功！",
                });
                Confirm_Pay_Close();
                SaveSuccess();
            }
            else if (data == "0") {
                $.messager.alert("警告信息", "确认收款失败", "info");
            }
            else {
                $.messager.alert("警告信息", "获取参数失败", "info");
            }
        },
    });
   
}

//发货
function OP_Deliver() {
    if ($("#op_orderDeliver").linkbutton('options').disabled == true) {
        return;
    }
    if ($("#hid_OP_OrderId").val() == "") {
        $.messager.alert("提示信息", "获取订单编号错误", "info");
        return;
    }
    var haveCar = false;
    //查看是否有车辆信息
    $.ajax({
        url: '/BasicInfo/GetCarCount',
        type: 'POST',
        async: false,
        success: function (data) {
            if (data != "") {
                if (parseInt(data) > 0) {
                    haveCar = true;
                }
            }
        }
    });
    if (haveCar) {//存在车
        $("#OP_SelectCar").dialog({
            title: '选择送货车辆',
            width: 255,
            height: 310,
        });
    }
    else {
        $.messager.confirm("确认信息", "您确认要发货么？", function (flag) {
            if (flag)
            {
                $.ajax({
                    url: '/BasicInfo/SaveOPDeliver',
                    type: 'POST',
                    data: { id: $("#hid_OP_OrderId").val() },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: "保存成功！",
                            });
                            SaveSuccess();
                        }
                        else if (data == "0") {
                            $.messager.alert("警告信息", "保存失败", "info");
                        }
                        else {
                            $.messager.alert("警告信息", "获取参数失败", "info");
                        }
                    },
                });
            }
        });
        
    }


}

//关闭选择车辆页面
function OP_SelectCar_Quit() {
    $("#OP_SelectCar").dialog("close");
}

//发货选择车辆确认
function OP_SelectCar_Save() {
    var row = $("#OP_CarList").datagrid("getSelected");
    if (row != null && row.CarId != undefined) {
        $.ajax({
            url: '/BasicInfo/SaveOPDeliver',
            type: 'POST',
            data: { id: $("#hid_OP_OrderId").val(), carId: row.CarId },
            success: function (data) {
                if (data == "1") {
                    $.messager.show({
                        title: '提示',
                        msg: "保存成功！",
                    });
                    OP_SelectCar_Quit();
                    SaveSuccess();
                }
                else if (data == "0") {
                    $.messager.alert("警告信息", "保存失败", "info");
                }
                else if (data == "2") {
                    $.messager.alert("警告信息", "添加配置车辆信息出错!", "info");
                }
                else if (data == "3") {
                    $.messager.alert("警告信息", "更新库存出错!", "info");
                }
                else {
                    $.messager.alert("警告信息", "获取参数失败", "info");
                }
            },
        });
    }
    else {
        $.ajax({
            url: '/BasicInfo/SaveOPDeliver',
            type: 'POST',
            data: { id: $("#hid_OP_OrderId").val() },
            success: function (data) {
                if (data == "1") {
                    $.messager.show({
                        title: '提示',
                        msg: "保存成功！",
                    });
                    OP_SelectCar_Quit();
                    SaveSuccess();
                }
                else if (data == "0") {
                    $.messager.alert("警告信息", "保存失败", "info");
                }
                else {
                    $.messager.alert("警告信息", "获取参数失败", "info");
                }
            },
        });
    }
}

//保存成功
function SaveSuccess() {
    ReflashOPTabs();
    //QuitTabs("订单处理");
    //if ($("#tabs").tabs('exists', "订单管理"))
    //{
    //    $("#tabs").tabs("select", "订单管理");
    //    if ($("#OC_OrderList") != null) {
    //        $("#OC_OrderList").datagrid("reload");
    //    }
    //}
   
}

//自适应高度
function OPChangeSize() {
    var height = document.documentElement.clientHeight - 265;
    if (height > 400) {
        $("#OP_List").height(height);
    }

}

//恢复订单
function OP_resumedOrder()
{
    if ($("#op_resumedOrder").linkbutton('options').disabled == true) {
        return;
    }
    $.ajax({
        url: '/BasicInfo/resumedOrder',
        type: 'POST',
        data: { id: $("#hid_OP_OrderId").val() },
        success: function (data) {
            if (data == "1") {
                $.messager.show({
                    title: '提示',
                    msg: "恢复订单成功！",
                });
                SaveSuccess();
            }
            else if (data == "0") {
                $.messager.alert("警告信息", "恢复订单失败", "info");
            }
            else {
                $.messager.alert("警告信息", "获取参数失败", "info");
            }
        }
    });
}

//刷新tab
function ReflashOPTabs() {
    var selectTab = $('#tabs').tabs('getSelected');

    //var url = $(selectTab.panel('options').content).attr('src');

    //$('#tabs').tabs('update', {
    //    tab: selectTab,
    //    options: {
    //        href: url
    //    }
    //});

    selectTab.panel('refresh');
}

//确认订单
function OP_FirmOrder() {
    if ($("#op_FirmOrder").linkbutton('options').disabled==true)
    {
        return;
    }
    $.ajax({
        url: '/BasicInfo/FirmOrder',
        type: 'POST',
        data: { id: $("#hid_OP_OrderId").val()},
        success: function (data) {
            if (data == "1") {
                $.messager.show({
                    title: '提示',
                    msg: "确认订单成功！",
                });
                SaveSuccess();
            }
            else if (data == "0") {
                $.messager.alert("警告信息", "确认订单失败", "info");
            }
            else {
                $.messager.alert("警告信息", "获取参数失败", "info");
            }
        }
    });
}

//强制确认订单
function OP_imposedOrderFinish() {
    if ($("#op_imposedOrderFinish").linkbutton('options').disabled == true) {
        return;
    }
    $.messager.confirm("确认信息", "您确认要强制确认收货么", function (flag) {
        if (flag) {
            if ($("#hid_OP_OrderId").val() == "") {
                $.messager.alert("提示信息", "获取订单编号错误", "info");
                return;
            }
            $.ajax({
                url: '/BasicInfo/OC_ForcedReceiptSave',
                type: 'POST',
                data: { id: $("#hid_OP_OrderId").val() },
                success: function (data) {
                    if (data == "1") {
                        $.messager.show({
                            title: '提示',
                            msg: "强制确认收货成功！",
                        });
                        SaveSuccess();
                    }
                    else if (data == "0") {
                        $.messager.alert("警告信息", "强制确认收货失败", "info");
                    }
                    else {
                        $.messager.alert("警告信息", "获取参数失败", "info");
                    }
                },
            });
        }
    })
}

//是否显示强制确认收货
function OP_GetForcedReceipt()
{
    if ($("#hid_OP_OrderId").val() == "") {
        $.messager.alert("提示信息", "获取订单编号错误", "info");
        return;
    }
    $.ajax({
        url: '/BasicInfo/OC_GetForcedReceipt',
        type: 'POST',
        data: { id: $("#hid_OP_OrderId").val() },
        success: function (data) {
            if (data != "1") {
                $("#op_imposedOrderFinish").linkbutton("disable");//强制确认收货
            }
        },
    });
}

//确认收款
function Confirm_Pay()
{
    $("#OP_ConfirmReceipt").dialog({
        title: '确认收款',
        width: 255,
        height: 155,
        modal: true
    });
}

//确认收款-取消
function Confirm_Pay_Close()
{
    $("#OP_ConfirmReceipt").dialog("close");
}