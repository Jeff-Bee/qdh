
$(function () {
    $("#SR_Customer").combobox({
        url: '/BasicInfo/GetPOSCustomer?Stype=online',
        type: 'POST',
        textField: 'text',
        valueField: 'id',
        onSelect: function (record) {
            //$("#pos_ProductList").datagrid("load", {
            //    type: record.id,
            //});
        }
    });
    $("#SR_Customer").combobox("setValue", 0);

    //订单状态--SR_PayStatus
    $("#SR_PayStatus").combobox({
        textField: 'text',
        valueField: 'id',
        onSelect: function (record) {
            //$("#pos_ProductList").datagrid("load", {
            //    type: record.id,
            //});
        },
        data: [{ "id": "-1", "text": "全部状态" }, { "id": "1", "text": "已完成" }, { "id": "0", "text": "未完成" }]
    });
    $("#SR_PayStatus").combobox("setValue", -1);

    //报表类型--SR_ReportType
    $("#SR_ReportType").combobox({
        textField: 'text',
        valueField: 'id',
        onSelect: function (record) {
            //$("#pos_ProductList").datagrid("load", {
            //    type: record.id,
            //});
        },
        data: [{ "id": "0", "text": "线下" }, { "id": "1", "text": "线上" }]
    });
    if ($("#sp_hid_type").val() != "") {
        $("#SR_ReportType").combobox("setValue", $("#sp_hid_type").val());
    }
    else {
        $("#SR_ReportType").combobox("setValue", 1);
    }
    $("#SR_startTime").datebox({

    });
    var date = new Date().Format("yyyy-MM-dd");
    $("#SR_startTime").datebox("setValue", date);
    $("#SR_endTime").datebox({

    });
    $("#SR_endTime").datebox("setValue", date);

    SR_RegionDataGrid();

    SRChangeSize();
})

//渲染表格并加载表格
function SR_RegionDataGrid()
{
    if ($("#SR_ReportType").combobox("getValue") == "1")//线上
    {
        $("#SR_OrderList").datagrid({
            url: '/BasicInfo/GetOrderRun',
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
                    title: '订单编号',
                    field: 'Code',
                    width: 130,
                },
                {
                    title: '下单时间',
                    field: 'OrderDate',
                    width: 160,
                    formatter: function (row, data, index) {
                        if (row != undefined) {
                            //return data.OrderDate;
                            return GetDate(data.OrderDate);
                        }
                    },
                },

                  {
                      title: '客户名称',
                      field: 'FullName',
                      width: 100,
                  },
                    {
                        title: '商品总数',
                        field: 'ProductQuantity',
                        width: 100,
                    },
                   {
                       title: '订单金额',
                       field: 'Amount',
                       width: 100,
                       formatter: function (value) {
                           if (value != undefined) {
                               return parseFloat(value).toFixed(2);
                           }
                       }
                   },
                     {
                         title: '订单状态',
                         field: 'OrderState',
                         width: 100,
                         formatter: function (row, data, index) {
                             switch (row) {

                                 case 1:
                                     return "买家已取消";
                                 case 2:
                                     return "确认收货";
                                 case 10:
                                     return "已确认";
                                 case 11:
                                     return "已取消";
                                 case 12:
                                     return "已发货";
                                 case 100:
                                     return "完成";
                                 case 0:
                                     return "<span style=\"color:red;\">待确认</span>";
                             }
                         }
                     },
                     {
                         title: '支付状态',
                         field: 'PayState',
                         width: 100,
                         formatter: function (value) {
                             if (value != undefined) {
                                 switch (value) {
                                     case 0:
                                         return "未支付";
                                         break;
                                     case 11:
                                         return "货到付款(现金支付)";
                                         break;
                                 }
                             }
                         }
                     },
                     {
                         title: '摘要',
                         field: 'Notes',
                         width: 100,
                     }
            ]],
            onLoadSuccess: function () {
                FillSRRows();
            }
        });
    }
    else {
        $("#SR_OrderList").datagrid({
            url: '/BasicInfo/GetSalersOrderRun',
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
                    title: '订单编号',
                    field: 'Code',
                    width: 130,
                },
                {
                    title: '下单时间',
                    field: 'OrderDate',
                    width: 160,
                    formatter: function (row, data, index) {
                        if (row != undefined) {
                            //return data.OrderDate;
                            return GetDate(data.OrderDate);
                        }
                    },
                },

                  {
                      title: '客户名称',
                      field: 'FullName',
                      width: 100,
                  },
                    {
                        title: '商品总数',
                        field: 'ProductQuantity',
                        width: 100,
                    },
                   {
                       title: '订单金额',
                       field: 'Amount',
                       width: 100,
                       formatter: function (value) {
                           if (value != undefined) {
                               return parseFloat(value).toFixed(2);
                           }
                       }
                   },
                     {
                         title: '订单状态',
                         field: 'OrderState',
                         width: 100,
                         formatter: function (row, data, index) {
                             if (row != undefined)
                             {
                                 return "--";
                             }
                         }
                     },
                     {
                         title: '支付状态',
                         field: 'PayState',
                         width: 100,
                         formatter: function (value) {
                             if (value != undefined) {
                                 return "--";
                             }
                         }
                     },
                     {
                         title: '摘要',
                         field: 'Notes',
                         width: 100,
                     }
            ]],
            onLoadSuccess: function () {
                FillSRRows();
            }
        });
    }
}


//当不够十行时补充十行空数据
function FillSRRows() {
    var data = $('#SR_OrderList').datagrid('getData');
    var pageopt = $('#SR_OrderList').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#SR_OrderList').datagrid('appendRow', {});
        }
    }
}

//查询
function SRSearch()
{
    SR_RegionDataGrid();
    if ($("#SR_ReportType").combobox("getValue") == "1")//线上
    {
        $("#SR_OrderList").datagrid("load", {
            customerId: $("#SR_Customer").combobox("getValue"),
            orderStatus: $("#SR_PayStatus").combobox("getValue"),
            startTime: $("#SR_startTime").datebox("getValue"),
            endTime: $("#SR_endTime").datebox("getValue")
        });

    }
    else {
        $("#SR_OrderList").datagrid("load", {
            customerId: $("#SR_Customer").combobox("getValue"),
            startTime: $("#SR_startTime").datebox("getValue"),
            endTime: $("#SR_endTime").datebox("getValue")
        });
       
    }
}

//汇出
function ExportSR()
{
    if ($("#SR_ReportType").combobox("getValue") == "1")//线上
    {
        $.ajax({
            url: '/BasicInfo/Export',
            type: 'POST',
            anysc: false,
            data: { customerId: $("#SR_Customer").combobox("getValue"), orderStatus: $("#SR_PayStatus").combobox("getValue"), startTime: $("#SR_startTime").datebox("getValue"), endTime: $("#SR_endTime").datebox("getValue") },
            success: function (data) {
                if (data != "") {
                    window.open(data);
                }
                else {
                    $.messager.alert("提示信息", "获取参数失败", "info");
                }
            }
        });
    }
    else {
        $.ajax({
            url: '/BasicInfo/ExportOSR',
            type: 'POST',
            anysc: false,
            data: { customerId: $("#SR_Customer").combobox("getValue"), startTime: $("#SR_startTime").datebox("getValue"), endTime: $("#SR_endTime").datebox("getValue") },
            success: function (data) {
                if (data != "") {
                    window.open(data);
                }
                else {
                    $.messager.alert("提示信息", "获取参数失败", "info");
                }
            }
        });
    }
}

function SRChangeSize() {
    var height = document.documentElement.clientHeight - 230;
    if (height > 400) {
        $("#SR_List").height(height);
    }

}