
$(function () {
    $("#OC_Customer").combobox({
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
    $("#OC_Customer").combobox("setValue", 0);
    $("#oc_reflashTime").combobox({
        textField: 'text',
        valueField: 'id',
        data: [
            { "id": "10", "text": "10秒1次" },
            { "id": "30", "text": "30秒1次" },
            { "id": "60", "text": "1分钟1次" }
        ],
        onSelect: function (data) {
            if ($("#oc_isAuto").prop("checked")) {
                clearInterval(OC_timer);
                var time = data.id;
                OC_timer = setInterval(SearchOC, time * 1000);
            }
        }
    });
    $("#oc_reflashTime").combobox("setValue", 10);
    $("#oc_startTime").datebox({

    });
    var date = new Date().Format("yyyy-MM-dd");
    $("#oc_startTime").datebox("setValue", date);
    $("#show_start_time").text(date);
    $("#show_end_time").text(date);
    $("#oc_endTime").datebox({

    });
    $("#oc_endTime").datebox("setValue", date);
    $("#OC_OrderList").datagrid({
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
                        return GetDate(row);
                    }
                },
            },
             
              {
                  title: '客户名称',
                  field: 'FullName',
                  width: 100,
              },
               {
                   title: '订单金额',
                   field: 'Amount',
                   width: 100,
               },
                 {
                     title: '订单状态',
                     field: 'OrderState',
                     width: 100,
                     formatter: function (row, data, index) {
                         switch (row)
                         {
                             
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
                         if (value != undefined)
                         {
                             switch (value)
                             {
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
                 },
                 {
                     title: '操作',
                     field: 'OrderId',
                     width: 60,
                     formatter: function (value, row, index) {
                         if (value != undefined)
                         {
                             return '<a href=\"javascript:;\" onclick=\"OC_showDetail(' + value + ')\">订单详情</a>';
                         }
                     }
                 }
        ]],
        onLoadSuccess: function () {
            FillOCRows();
        },
        onDblClickRow: function (rowIndex, rowData) {
            //AddTab("订单处理", "/BasicInfo/OrderProcess?id=" + rowData.OrderId);
            if (rowData != null && rowData.OrderId != undefined && rowData.OrderId != "") {
                AddTab("订单处理", "/BasicInfo/OrderProcess?id=" + rowData.OrderId);
            }
        }
    });
    if ($("#oc_type").val() != "") {

        $("#OC_OrderList").datagrid("load", {
            customerId: $("#OC_Customer").combobox("getValue"),
            startTime: $("#oc_startTime").datebox("getValue"),
            endTime: $("#oc_endTime").datebox("getValue"),
            type: $("#oc_type").val(),
            scontent: $("#oc_SearchContent").val()
        });
    }
    else {
        SearchOC();
    }
    $("#oc_isAuto").click(function () {
        if ($(this).prop("checked")) {
            var time = $("#oc_reflashTime").combobox("getValue");
            OC_timer = setInterval(SearchOC, time * 1000);
        }
        else {
            clearInterval(OC_timer);
        }
    });

    //自适应高度
    OCChangeSize();

});
var OC_timer;//订单管理定时刷新

//订单详情
function OC_showDetail(value)
{
    AddTab("订单处理", "/BasicInfo/OrderProcess?id=" + value);
}


//当不够十行时补充十行空数据
function FillOCRows() {
    var data = $('#OC_OrderList').datagrid('getData');
    var pageopt = $('#OC_OrderList').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#OC_OrderList').datagrid('appendRow', {});
        }
    }
}

function SearchOC()
{
    $("#OC_OrderList").datagrid("load", {
        customerId: $("#OC_Customer").combobox("getValue"),
        startTime: $("#oc_startTime").datebox("getValue"),
        endTime: $("#oc_endTime").datebox("getValue"),
        scontent: $("#oc_SearchContent").val()
    });
}

//退出
function exitOP() {
    $("#tabs").tabs('close', "订单管理");
}

function OCChangeSize() {
    var height = document.documentElement.clientHeight - 230;
    if (height > 400) {
        $("#OC_List").height(height);
    }

}