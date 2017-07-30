
$(function () {
    //绑定快速查询
    $("#ilv_PurchaseUnit").combobox({
        url: '/BasicInfo/GetSearchTypeList',
        valueField: 'id',
        textField: 'text',
        onSelect: function (res) {
            
        },
    });
    $("#ilv_PurchaseUnit").combobox("select", 0);

    //绑定过滤条件
    $("#ilv_glSelect").combobox({
        url: '/BasicInfo/GetGLList',
        valueField: 'id',
        textField: 'text',
        onSelect: function (res) {
            ILV_searchList(res.id);
        },
    });
    $("#ilv_glSelect").combobox("select", "-1");
    

    $("#ilv_pType").tree({
        url: '/BasicInfo/GetProductTypes',
        lines: true,
        dataType: 'text',
        dnd: true,
        onDblClick: function (node) {
           
        },
        onClick: function (node) {
            //var ptype = "\\";
            //var list = [];
            if (node.id == 0) {
                return;
            }
            //var parentNode = $("#ilv_pType").etree("getParent", node.target);
            //var pparentNode;
            //var ppparentNode;
            //if (parentNode != null && parentNode.id) {
            //    pparentNode = $("#ilv_pType").etree("getParent", parentNode.target);
            //    if (pparentNode != null && pparentNode.id != 0) {
            //        ppparentNode = $("#ilv_pType").etree("getParent", pparentNode.target);
            //    }

            //}
            //if (ppparentNode != null && ppparentNode.id != 0) {
            //    ptype += ppparentNode.text + "\\";
            //}
            //if (pparentNode != null && pparentNode.id != 0) {
            //    ptype += pparentNode.text + "\\";
            //}
            //if (parentNode != null && parentNode.id != 0) {
            //    ptype += parentNode.text + "\\";
            //}
            //ptype += node.text + "\\";
            $("#hid_ILVpType").val(node.id);
            ILV_searchList();

        },
        onSelect: function (node) {
            //var ptype = "\\";
            //var list = [];
            if (node.id == 0) {
                return;
            }
            //var parentNode = $("#ilv_pType").etree("getParent", node.target);
            //var pparentNode;
            //var ppparentNode;
            //if (parentNode != null && parentNode.id) {
            //    pparentNode = $("#ilv_pType").etree("getParent", parentNode.target);
            //    if (pparentNode != null && pparentNode.id != 0) {
            //        ppparentNode = $("#ilv_pType").etree("getParent", pparentNode.target);
            //    }

            //}
            //if (ppparentNode != null && ppparentNode.id != 0) {
            //    ptype += ppparentNode.text + "\\";
            //}
            //if (pparentNode != null && pparentNode.id != 0) {
            //    ptype += pparentNode.text + "\\";
            //}
            //if (parentNode != null && parentNode.id != 0) {
            //    ptype += parentNode.text + "\\";
            //}
            //ptype += node.text + "\\";
            $("#hid_ILVpType").val(node.id);
            ILV_searchList();
        }
    });

    $("#ilv_pList").datagrid({
        url: '/BasicInfo/InventoryLevelsProductList',
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
                field: 'ProductId',
                title: '',
                checkbox: true,
                width:100
            },
            {
                field: 'ProductCode',
                title: '商品编号',
                width:100
            },
            {
                title: '商品名称',
                field: 'ProductFullName',
                width: 100
            },
            {
                title: '商品简名',
                field: 'ProductShortName',
                width: 100
            },
            {
                title: '规格',
                field: 'ProductSpec',
                width: 100
            },
            {
                title: '库存数量',
                field: 'Quantity',
                width: 100
            },
            {
                title: '成本均价',
                field: 'Cost',
                width:100,
                formatter: function (value) {
                    if(value!=null&&value!=undefined)
                        return parseFloat(value).toFixed(2);
                }
            },
            {
                title: '库存金额',
                field: 'totalPrice',
                width: 100
            }
        ]],
        onLoadSuccess: function () {
            FillILVProductRows();
        },
        onDblClickRow: function (index, data) {
            ilv_ShowOrderList(data.ProductId);
        },
    });
    ILV_searchList();

    //商品订单
    $("#ilv_OrderList_List").datagrid({
        url: '/BasicInfo/InventoryLevelsOrderList',
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
                title: '订单编号',
                width: 100
            },
            {
                title: '商品名称',
                field: 'ProductFullName',
                width: 100
            },
            {
                title: '商品简名',
                field: 'ProductShortName',
                width: 100
            },
            {
                title: '订单日期',
                field: 'ComeDate',
                width: 130,
                formatter: function (value) {
                    if (value != null && value != undefined)
                        return GetDate(value);
                }
            },
            {
                title: '商品数量',
                field: 'Quantity',
                width: 70
            },
            {
                title: '商品价格',
                field: 'Price',
                width: 100,
                formatter: function (value) {
                    if (value != null && value != undefined)
                        return parseFloat(value).toFixed(2);
                }
            }
        ]],
        onLoadSuccess: function () {
            FillILVOrderListRows();
        },
    });

    ChangeSize();
})

//查询
function Search_Click()
{
    if ($("#ilv_sContent").val() == "") {
        $.messager.alert("提示信息","查询内容不能为空","info");
    }
    else {
        ILV_searchList();
    }
}

//当不够十行时补充十行空数据
function FillILVProductRows() {
    var data = $('#ilv_pList').datagrid('getData');
    var pageopt = $('#ilv_pList').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#ilv_pList').datagrid('appendRow', {});
        }
    }
}


//当不够十行时补充十行空数据
function FillILVOrderListRows() {
    var data = $('#ilv_OrderList_List').datagrid('getData');
    var pageopt = $('#ilv_OrderList_List').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#ilv_OrderList_List').datagrid('appendRow', {});
        }
    }
}

//刷新
function ILV_ReflashList(id)
{
    $("#ilv_pList").datagrid("reload");
    
}

//搜索
function ILV_searchList(status) {
    $("#ilv_pList").datagrid("load", {
        searchType: $("#ilv_PurchaseUnit").combobox("getValue"),
        type: $("#hid_ILVpType").val(),
        searchContent: $("#ilv_sContent").val(),
        status: status == undefined ? $("#ilv_glSelect").combobox("getValue") : status
    });
}

function ilv_ShowOrderList(id)
{
    $("#ILV_OrderList").dialog({
        title: '商品订单',
        width: 620,
        height: 385,
        modal: true
    });
    $("#ilv_OrderList_List").datagrid("load", {
        pid: id,
    });
}

//关闭查看订单窗口
function ILV_CloseOrderList() {
    $("#ILV_OrderList").dialog("close");
}

function ChangeSize() {
    var height = document.documentElement.clientHeight - 240;
    var width = document.documentElement.clientWidth - 310;
    if (height > 305) {
        $("#ilv_left").height(height);
        $("#ilv_right").height(height);
    }
    $("#ilv_right").width(width);

}