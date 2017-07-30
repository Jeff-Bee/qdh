$(function () {
    $("#POR_Customer").combobox({
        url: '/BasicInfo/GetSuppCbxList',
        textField: 'FullName',
        valueField: 'SupplierId',
    });
    $("#POR_Customer").combobox("setValue", -1);

    var date = new Date().Format("yyyy-MM-dd");
    $("#POR_startTime").datebox({

    });
    $("#POR_startTime").datebox("setValue", date);

    $("#POR_endTime").datebox({

    });
    $("#POR_endTime").datebox("setValue", date);

    $("#POR_OrderList").datagrid({
        url: '/BasicInfo/GetPorRun',
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
                field: 'ComeDate',
                width: 160,
                formatter: function (row, data, index) {
                    if (row != undefined) {
                        return GetDate(row);
                    }
                },
            },
              {
                  title: '供应商名称',
                  field: 'FullName',
                  width: 100,
              },
               {
                   title: '订单金额',
                   field: 'TotalAmount',
                   width: 100,
                   formatter: function (row, data, index) {
                       if (row != undefined) {
                           return parseFloat(row).toFixed(2);
                       }
                   },
               },
                 {
                     title: '摘要',
                     field: 'Notes',
                     width: 100,
                 },
                 {
                     title: '操作',
                     field: 'BuyId',
                     width: 60,
                     formatter: function (value, row, index) {
                         if (value != undefined) {
                             return '<a href=\"javascript:;\" onclick=\"POR_showDetail(' + value + ',' + index + ')\">订单详情</a>';
                         }
                     }
                 }
        ]],
        onLoadSuccess: function () {
            FillPORRows();
        },

    });
    SearchPOR();

    PORChangeSize();
})

function SearchPOR() {
    $("#POR_OrderList").datagrid("load", {
        SupplierId: $("#POR_Customer").combobox("getValue"),
        StartTime: $("#POR_startTime").datebox("getValue"),
        EndTime: $("#POR_endTime").datebox("getValue"),
        content: $("#POR_SearchContent").val()
    });
}

function POR_showDetail(id, index) {
    console.log(id);
    var data = $('#POR_OrderList').datagrid('getData').rows[index];
    if (data != null) {
        $("#por_Code").val(data.Code);
        $("#por_Supplier").val(data.FullName);
        $("#por_Notes").val(data.Notes);
        $("#por_TotalAmount").val(parseFloat(data.TotalAmount).toFixed(2));
    }
    if (id != undefined && id != null) {
        $("#por_productList").datagrid({
            url: '/BasicInfo/GetPorDetail',
            rownumbers: true,
            loadMsg: '数据加载中。。',
            striped: true,
            remoteSort: false,
            pagination: false,
            singleSelect: true,
            fixed: true,
            fitColumns: true,
            columns: [[
                {
                    title: '商品编号',
                    field: 'ProductCode',
                    width: 130,
                },
                {
                    title: '商品名称',
                    field: 'ProductFullName',
                    width: 160,

                },
                {
                    title: '商品金额',
                    field: 'Price',
                    width: 100,
                },
                {
                    title: '商品数量',
                    field: 'Quantity',
                    width: 100,
                }
            ]],
            onBeforeLoad: function (para) {
                para.BuyId = id;
            }
        });

    }
    $("#POR_ShowDetail").dialog({
        title: '订单详情',
        width: 420,
        height: 265,
        modal: true
    });
}
//当不够十行时补充十行空数据
function FillPORRows() {
    var data = $('#POR_OrderList').datagrid('getData');
    var pageopt = $('#POR_OrderList').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#POR_OrderList').datagrid('appendRow', {});
        }
    }
}

function PORChangeSize() {
    var height = document.documentElement.clientHeight - 230;
    if (height > 400) {
        $("#POR_List").height(height);
    }

}


function por_product_listClose() {
    $("#POR_ShowDetail").dialog("close");
}
