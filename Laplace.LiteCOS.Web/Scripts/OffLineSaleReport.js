
$(function () {
    $("#OSR_Customer").combobox({
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
    $("#OSR_Customer").combobox("setValue", 0);
    $("#OSR_startTime").datebox({

    });
    var date = new Date().Format("yyyy-MM-dd");
    $("#OSR_startTime").datebox("setValue", date);
    $("#OSR_endTime").datebox({

    });
    $("#OSR_endTime").datebox("setValue", date);

    $("#OSR_OrderList").datagrid({
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
                     title: '摘要',
                     field: 'Notes',
                     width: 100,
                 },
                 //{
                 //    title: '操作',
                 //    field: 'OrderId',
                 //    width: 60,
                 //    formatter: function (value, row, index) {
                 //        if (value != undefined) {
                 //            return '<a href=\"javascript:;\" onclick=\"OC_showDetail(' + value + ')\">订单详情</a>';
                 //        }
                 //    }
                 //}
        ]],
        onLoadSuccess: function () {
            FillOSRRows();
        }
    });

    OSRChangeSize();
})



//当不够十行时补充十行空数据
function FillOSRRows() {
    var data = $('#OSR_OrderList').datagrid('getData');
    var pageopt = $('#OSR_OrderList').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#OSR_OrderList').datagrid('appendRow', {});
        }
    }
}

//查询
function OSRSearch() {
    $("#OSR_OrderList").datagrid("load", {
        customerId: $("#OSR_Customer").combobox("getValue"),
        startTime: $("#OSR_startTime").datebox("getValue"),
        endTime: $("#OSR_endTime").datebox("getValue")
    });
}

//汇出
function ExportOSR() {
    $.ajax({
        url: '/BasicInfo/ExportOSR',
        type: 'POST',
        anysc: false,
        data: { customerId: $("#OSR_Customer").combobox("getValue"), startTime: $("#OSR_startTime").datebox("getValue"), endTime: $("#OSR_endTime").datebox("getValue") },
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

function OSRChangeSize() {
    var height = document.documentElement.clientHeight - 230;
    if (height > 400) {
        $("#OSR_List").height(height);
    }

}