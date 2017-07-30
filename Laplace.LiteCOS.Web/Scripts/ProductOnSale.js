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

var POS_editIndex = undefined;
function endEditing() {
    if (POS_editIndex == undefined) { return true }
    if ($('#pos_ProductList').datagrid('validateRow', POS_editIndex)) {
        $('#pos_ProductList').datagrid('endEdit', POS_editIndex);
        POS_editIndex = undefined;
        return true;
    } else {
        return false;
    }
}
function onClickCell(index, field) {
    if (endEditing()) {
        $('#pos_ProductList').datagrid('selectRow', index)
                .datagrid('editCell', { index: index, field: field });
        POS_editIndex = index;
    }
}

$(function () {
    $("#pos_Customer").combobox({
        url: '/BasicInfo/GetPOSCustomer?Stype=online',
        type: 'POST',
        textField: 'text',
        valueField: 'id',
        onSelect: function (record) {
            $("#pos_ProductList").datagrid("load", {
                type: record.id,
            });
        }
    });
    $("#pos_Customer").combobox("setValue",0);

    $("#pos_ptype").combobox({
        url: '/BasicInfo/GetPOSPType',
        type: 'POST',
        textField: 'Name',
        valueField: 'ClassId',
        onSelect: function (record) {
            $("#pos_ProductList").datagrid("load", {
                pType: record.ClassId,
            });
        }
    });
    $("#pos_ptype").combobox("setValue", -1);

    $("#pos_ProductList").datagrid({
        url: '/BasicInfo/GetPOSList',
        rownumbers: true,
        loadMsg: '数据加载中。。',
        striped: true,
        remoteSort: false,
        pagination: true,
        //singleSelect: true,
        fixed: true,
        fitColumns: true,
        columns: [[
            {
                title: '图片',
                field: 'picSrc',
                width: 32,
                height: 32,
                formatter: function (value, row, index) {
                    if (value == "" || value == undefined)
                    {
                        return "";
                    }
                    return "<img src=\"" + value + "\" style=\"width:32px;height:32px;\" />";
                }
            },
            {
                title: '商品编号',
                field: 'ProductCode',
                width: 70,
            },
            {
                title: '商品名称',
                field: 'ProductFullName',
                width: 200,
            },
            {
                title: '商品分类',
                field: 'ClassName',
                width: 120,
            },
            {
                title: '是否上架&nbsp;<input type="checkbox" id="ck_all" value="" style="vertical-align:middle" />',
                field: 'SaleState',
                width: 50,
                align:'right',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        if (row.SaleState != undefined && row.SaleState == "1") {
                            return "<input type=\"checkbox\" class=\"ck_item\" value=\"" + row.ProductId + "\" checked=\"checked\" />";
                        }
                        else {
                            return "<input type=\"checkbox\" class=\"ck_item\" value=\"" + row.ProductId + "\" />";
                        }
                    }
                }
            },
            {
                title: '是否促销&nbsp;<input type="checkbox" id="ck_isProm_all" value="" style="vertical-align:middle" />',
                field: 'IsPromotion',
                width: 50,
                align: 'right',
                formatter: function (value, row, index) {
                    if (value != undefined)
                    {
                        if (row.IsPromotion != undefined && row.IsPromotion == "1") {
                            return "<input type=\"checkbox\" class=\"ck_isPromitem\" value=\"" + row.ProductId + "\" checked=\"checked\" />";
                        }
                        else {
                            return "<input type=\"checkbox\" class=\"ck_isPromitem\" value=\"" + row.ProductId + "\" />";
                        }

                    }
                   
                }
            },
            {
                title: '是否新品&nbsp;<input type="checkbox" id="ck_isNew_all" value="" style="vertical-align:middle" />',
                field: 'IsNew',
                width: 50,
                align: 'right',
                formatter: function (value, row, index) {
                    if (value != undefined) {
                        if (row.IsNew != undefined && row.IsNew == "1") {
                            return "<input type=\"checkbox\" class=\"ck_isNew_item\" value=\"" + row.ProductId + "\" checked=\"checked\" />";
                        }
                        else {
                            return "<input type=\"checkbox\" class=\"ck_isNew_item\" value=\"" + row.ProductId + "\" />";
                        }
                    }
                }
            },
             {
                 title: '设置价格',
                 field: 'Price1',
                 width: 80,
                 editor: {
                     type: 'numberbox', options: {
                         min: 0,
                         precision: 2
                     }
                 },
             },
            {
                title: '销售库存数量',
                field: 'SaleStoreQuantity',
                width: 70,
                editor: { type: 'numberbox', options: { min: 0, } },
            },
              {
                  title: '库存数量',
                  field: 'Quantity',
                  width: 70,
              },
               {
                   title: '规格',
                   field: 'ProductSpec',
                   width: 100,
               }
        ]],
        onLoadSuccess: function () {
            FillPOSRows();
        },
        onClickCell: function (index, field, value) {
            if (POS_editIndex != undefined&&POS_editIndex!=index) {
                $("#pos_ProductList").datagrid("endEdit", POS_editIndex);
            } 
            $("#pos_ProductList").datagrid("unselectAll");
            $("#pos_ProductList").datagrid("selectRow", index);
            if (field == "Price1" || field == "SaleStoreQuantity") {
                var row = $("#pos_ProductList").datagrid("getSelected");
                if (row != null && row.ProductId != undefined) {
                    onClickCell(index, field);
                }
            }
            else {
                if (POS_editIndex != undefined && POS_editIndex == index) {
                    $("#pos_ProductList").datagrid("endEdit", POS_editIndex);
                }
            }
        },
        onDblClickRow: function (index, data) {
            if (POS_editIndex != undefined) {
                $("#pos_ProductList").datagrid("endEdit", POS_editIndex);
            }
            if (data.ProductId != undefined && data.productId != "")
            {
                if (data.ProductId == undefined || data.ProductId == "")
                {
                    return false;
                }
                $.post("/BasicInfo/GetProductInfo", { id: data.ProductId }, function (data) {
                    var json = eval("(" + data + ")");
                    var data = json.productInfo[0];
                    var picInfo = json.pics;

                    $("input[name=POC_pcode]").val(data.ProductCode);
                    $("input[name=POC_pname]").val(data.ProductFullName);
                    $("input[name=POC_pformat]").val(data.ProductSpec);
                    $("input[name=POC_pxh]").val(data.ProductModel);
                    $("input[name=POC_phome]").val(data.Place);
                    $("input[name=POC_punit]").val(data.ProductUnit);
                    $("input[name=POC_pbarcode]").val(data.BarCode);
                    $("#POC_brochure").val(data.Summary);
                    $("input[name=POC_pym]").val(data.PinyinCode);

                    $("#POC_pkg").numberspinner("setValue", data.Weight);
                    //售价
                    $("#POC_pprice1").numberspinner("setValue", data.Price1);
                    $("#POC_pprice2").numberspinner("setValue", data.Price2);
                    $("#POC_pprice3").numberspinner("setValue", data.Price3);
                    $("#POC_pprice4").numberspinner("setValue", data.Price4);
                    $("#POC_pprice5").numberspinner("setValue", data.Price5);
                    $("#POC_pprice6").numberspinner("setValue", data.Price6);
                    $("#POC_pprice7").numberspinner("setValue", data.Price7);
                    $("#POC_pprice8").numberspinner("setValue", data.Price8);
                    
                    //图片信息
                    for (var i = 0; i < picInfo.length; i++) {
                        $("#POC_picUrl" + (i + 1)).attr("src", picInfo[i].picUrl);
                    }

                    $("#POS_ProductInfo").dialog({
                        title: '商品信息-信息框',
                        width: 632,
                        height: 530,
                        modal: true
                    });
                });
            }
        },

    });

    $("#ck_all").click(function () {
        if ($("#ck_all").prop("checked")) {
            $("#ck_all").prop("checked",true);
            $(".ck_item").each(function (i, e) {
                $(this).prop("checked", true);
            });
        }
        else {
            $(".ck_item").each(function (i, e) {
                $(this).prop("checked", false);
            });
        }
    });

    //是否促销
    $("#ck_isProm_all").click(function () {
        if ($("#ck_isProm_all").prop("checked")) {
            $("#ck_isProm_all").prop("checked", true);
            $(".ck_isPromitem").each(function (i, e) {
                $(this).prop("checked", true);
            });
        }
        else {
            $(".ck_isPromitem").each(function (i, e) {
                $(this).prop("checked", false);
            });
        }
    });

    //是否新品
    $("#ck_isNew_all").click(function () {
        if ($("#ck_isNew_all").prop("checked")) {
            $("#ck_isNew_all").prop("checked", true);
            $(".ck_isNew_item").each(function (i, e) {
                $(this).prop("checked", true);
            });
        }
        else {
            $(".ck_isNew_item").each(function (i, e) {
                $(this).prop("checked", false);
            });
        }
    });

    //自适应高度
    POSChangeSize();
});

//保存
function addPOSSave()
{
    var rows = $("#pos_ProductList").datagrid("getData");
    if ($("#pos_Customer").combobox("getValue") == "")
    {
        $.messager.alert("提示信息", "请选择客户", "info");
        return;
    }
    else if ($("#pos_ptype").combobox("getValue") == "") {
        $.messager.alert("提示信息", "请选择产品类型", "info");
        return;
    }
    else {
        var num = 0;
        var json = "[";
        if ($("#pos_Customer").combobox("getValue") == "0") {
            for(var i=0;i<rows.total;i++)
            {
                if (rows.rows[i] != null && rows.rows[i].ProductId != undefined) {
                    json += "{\"ProductId\":\"" + rows.rows[i].ProductId + "\",\"SaleState\":\"" + ($(".ck_item").eq(i).prop("checked") ? "1" : "0") + "\",\"IsPromotion\":" + ($(".ck_isPromitem").eq(i).prop("checked") ? "1" : "0") + ",\"IsNew\":" + ($(".ck_isNew_item").eq(i).prop("checked") ? "1" : "0") + ",\"SaleStoreQuantity\":\"" + rows.rows[i].SaleStoreQuantity + "\",\"Price1\":\"" + rows.rows[i].Price1 + "\"},";
                    num++;
                }
            }
        }
        else {
            for (var i = 0; i < rows.total; i++) {
                if (rows.rows[i] != null && rows.rows[i].ProductId != undefined) {
                    json += "{\"ProductId\":\"" + rows.rows[i].ProductId + "\",\"BuyerId\":\"" + $("#pos_Customer").combobox("getValue") + "\",\"SaleState\":\"" + ($(".ck_item").eq(i).prop("checked") ? "1" : "0") + "\",\"IsPromotion\":" + ($(".ck_isPromitem").eq(i).prop("checked") ? "1" : "0") + ",\"IsNew\":" + ($(".ck_isNew_item").eq(i).prop("checked") ? "1" : "0") + ",\"SaleStoreQuantity\":\"" + rows.rows[i].SaleStoreQuantity + "\",\"Price1\":\"" + rows.rows[i].Price1 + "\"},";
                    num++;
                }
            }
        }
        if (json.substring(json.length - 1, 1) == ",")
        {
            json = json.substring(0, json.length - 1);
        }
        json += "]";
        if (num == 0)
        {
            $.messager.alert("提示信息", "暂无商品信息,请先选择有商品分分类，如果还没有商品，请先添加商品", "info");
            return;
        }
        $.ajax({
            url: '/BasicInfo/SavePOS',
            type: 'POST',
            data: { str: json, type: $("#pos_Customer").combobox("getValue") },
            success: function (data) {
                if (data == "1")
                {
                    $.messager.show({
                        title: '提示',
                        msg: '保存成功!',
                    });
                    //$.messager.alert("提示信息", "保存成功!", "info");
                    exitPos();
                    $("#tabs").tabs("add", {
                        title: '商品上架',
                        closable: true,
                        href: '/BasicInfo/ProductOnSale',
                    });
                }
                else if (data == "0") {
                    $.messager.alert("提示信息", "保存失败!", "info");
                }
                else {
                    $.messager.alert("提示信息", "获取参数失败!", "info");
                }
            },
        });

    }
}

//退出
function exitPos()
{
    $("#tabs").tabs('close', "商品上架");
}

//当不够十行时补充十行空数据
function FillPOSRows() {
    var data = $('#pos_ProductList').datagrid('getData');
    var pageopt = $('#pos_ProductList').datagrid('getPager').data("pagination").options;
    if (data.total < pageopt.pageSize) {
        for (var i = data.total; i < pageopt.pageSize; i++) {
            $('#pos_ProductList').datagrid('appendRow', {});
        }
    }
}


//关闭查看商品信息
function ClosePos()
{
    $("#POS_ProductInfo").dialog("close");
}

function POSChangeSize() {
    var height = document.documentElement.clientHeight - 230;
    if (height > 400) {
        $("#POS_List").height(height);
    }

}