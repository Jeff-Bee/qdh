
$(function () {
    $("#user").menubutton({
        menu: '#userList',
       
    });
    $("#tabs").tabs({
        fit: true,
        border:false,
    });
   

    //行业下拉框
    $("#industry").combobox({
        url: '/BackStage/GetInsudtryList',
        textField: 'Name',
        valueField: 'IndustryId',
    });

    //设置默认选中本用户注册时的类型
    if ($("#hid_index_IndustryId").val() != "") {
        $("#industry").combobox("setValue", $("#hid_index_IndustryId").val());
    }
    EditSellerUserInfo();

    //默认置灰未实现的功能菜单
    //消息
    $("#TotalMsg").menu('disableItem', $('#BusinessMsg'));
    $("#TotalMsg").menu('disableItem', $('#SystemMsg'));
    //线下销售
    //$("#OfflineSales").menu('disableItem', $('#index_SalesOut'));
    //$("#OfflineSales").menu('disableItem', $('#index_SalesReport'));
    //常用功能
    //$("#InventoryManagement").menu('disableItem', $('#AssemblyList'));
    //$("#InventoryManagement").menu('disableItem', $('#LossReport'));

    //更新订单个数
    ReflashOrderListMsg();
    setInterval("ReflashOrderListMsg()",1000);
});

//修改密码
function EditPwd() {
    $("#editPwd").dialog({
        title: '设置登录密码',
        width: 440,
        height: 460,
        top: 100,
        closed: true,
        closable:true,
        buttons: [{
            text: '保存',
            width:80,
            handler: function () {
                if ($("input[name=oldPwd]").val() == "")
                {
                    $.messager.alert("提示", "原密码不能为空");
                    $("input[name=oldPwd]").focus();
                }
                else if ($("input[name=newPwd]").val() == "") {
                    $.messager.alert("提示", "新密码不能为空");
                    $("input[name=newPwd]").focus();
                }
                else if ($("input[name=verifyPwd]").val() == "") {
                    $.messager.alert("提示", "确认密码不能为空");
                    $("input[name=verifyPwd]").focus();
                }
                else if ($("input[name=verifyPwd]").val() != $("input[name=newPwd]").val()) {
                    $.messager.alert("提示", "两次输入密码不一致");
                    $("input[name=verifyPwd]").focus();
                }
                else {
                    $.ajax({
                        url: '/UserCenter/ChangePwd',
                        type: 'post',
                        dataType: 'text',
                        data: { oldPwd: $("input[name=oldPwd]").val(), newPwd: $("input[name=newPwd]").val() },
                        success: function (data) {
                            if ((data + "").indexOf("原密码不正确") < 0) {
                                $.messager.alert("提示", data);
                                $("#editPwd").form('clear');
                                $("#editPwd").dialog('close');
                            }
                            else {
                                $.messager.alert("提示", data);
                                $("#editPwd").form('clear');
                            }
                        },
                    });
                }
            }
        }, {
            text: '取消(X)',
            width: 80,
            handler: function () {
                $("#editPwd").dialog("close");
            }
        }],
        modal: true
    });
    $("#editPwd").dialog('open');
}

//退出
function Equit() {
    if (confirm('您确定要退出么?'))
    {
        location.href = '/UserCenter/Login';
    }
}

//修改注册信息
function EditUserInfo()
{
    $("#editUserInfo").dialog({
        title: '修改注册信息',
        width: 470,
        height: 320,
        top: 100,
        closed: true,
        closable: true,
        buttons: [{
            text: '提交',
            width: 60,
            handler: function () {
                //if ($("input[name=oldPhone]").val() == "") {
                //    $.messager.alert("提示", "手机号码不能为空");
                //    $("input[name=oldPhone]").focus();
                //}
                //else
                if ($("input[name=name]").val() == "") {
                    $.messager.alert("提示", "联系人不能为空");
                    $("input[name=name]").focus();
                }
                else if ($("input[name=newEmail]").val() == "") {
                    $.messager.alert("提示", "电子邮箱不能为空");
                    $("input[name=newEmail]").focus();
                }
                else if ($("input[name=businessName]").val() == "") {
                    $.messager.alert("提示", "企业名称不能为空");
                    $("input[name=businessName]").focus();
                }
                else if ($("#industry").combobox("getValue") == "") {
                    $.messager.alert("提示", "所属行业不能为空");
                    //$("input[name=industry]").focus();
                }
                else {
                    var json = "{";
                    json += "\"MobilePhone\":\"" + $("input[name=oldPhone]").val() + "\",\"ContactName\":\"" + $("input[name=name]").val() + "\",\"EMail\":\"" + $("input[name=newEmail]").val() + "\",\"CompanyName\":\"" + $("input[name=businessName]").val() + "\",\"IndustryId\":\"" + $("#industry").combobox("getValue") + "\"";
                    json += "}";
                    console.log(json);
                    $.ajax({
                        url: '/UserCenter/UpdateSellerUserInfo',
                        type: 'POST',
                        data: { str: json },
                        success: function (data) {
                            if ((data + "").indexOf("修改成功") >= 0 || (data + "").indexOf("参数错误") >= 0) {
                                $.messager.alert("提示", data);
                                $("#editUserInfo").dialog("close");
                                EditSellerUserInfo();
                            }
                            else {
                                $.messager.alert("提示", data);
                            }
                            
                        }
                    });
                   
                }
            }
        }, {
            text: '退出',
            width:60,
            handler: function () {
                $("#editUserInfo").dialog('close');
            }
        }],
        modal: true
    });
    $("#editUserInfo").dialog('open');
}

//手机格式验证
function CheckPhone(obj)
{
    if (!(/^1[34578]\d{9}$/.test(obj.val())))
    {
        obj.val("");
    }
}

//邮箱验证
function CheckEmail(obj)
{
    if (!/^(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$/.test(obj.val()))
    {
        obj.val("");
    }
}

//点击菜单事件
function AddTab(title,url)
{
    if (!$("#tabs").tabs('exists', title))
    {
        $("#tabs").tabs("add", {
            title: title,
            closable: true,
            href:url,
        });
    }
    else {
        $("#tabs").tabs('select', title);
    }
}


function EditSellerUserInfo()
{
    $.ajax({
        url: '/UserCenter/GetSellerUserInfo',
        type: 'POST',
        success: function (data) {
            var json = eval("(" + data + ")");
            if (json != "")
            {
                $("input[name=oldPhone]").val("");
                $("input[name=name]").val(json.ContactName);
                $("input[name=newEmail]").val(json.EMail);
                $("input[name=businessName]").val(json.CompanyName);
                $("#old_phone").text(json.MobilePhone);
                //$("#user").text(json.CompanyName);
                if (json.IndustryId != undefined && json.IndustryId != "") {
                    $("#industry").combobox("setValue", json.IndustryId);
                }
            }
        }
    });
}

//显示时间
function GetDate(row) {
    return (row + "").replace("T", " ").split('.')[0]
    //var date = new Date(+new Date(row) + 8 * 3600 * 1000).toISOString().replace(/T/g, ' ').replace(/\.[\d]{3}Z/, '');
    //return date;
}

//关闭tab
function QuitTabs(name) {
    $("#tabs").tabs('close', name);
}

//点击左侧玄幻导航页内容
function LeftNavite(type)
{
    $("#tabs").tabs('select', "导航页");
    if (type == 1)//商城
    {
        $(".index_dh").css("display", "none");
        $("#index_Product").show();
        $("#Market").attr("src", "/images/Market.png");
        $("#Index_Sale").attr("src", "/images/saleoutstock_normal.png");
        $("#Index_Stock").attr("src", "/images/Stock_normal.png");
        $("#index_info").attr("src", "/images/finance_normal.png");
        isInfoClick = false;
        isMarketClick = true;
        isStock = false;
        isSale = false;
    }
    else if (type == 2)//销售
    {
        $(".index_dh").css("display", "none");
        $("#index_UnderLineSale").show();
        $("#index_info").attr("src", "/images/finance_normal.png");
        $("#Market").attr("src", "/images/Market_normal.png");
        $("#Index_Sale").attr("src", "/images/saleoutstock_hover.png");
        $("#Index_Stock").attr("src", "/images/Stock_normal.png");
        isInfoClick = false;
        isMarketClick = false;
        isStock = false;
        isSale = true;
    }
    else if (type == 3)//进货
    {
        $(".index_dh").css("display", "none");
        $("#index_PurchaseOrder").show();
        $("#index_info").attr("src", "/images/finance_normal.png");
        $("#Market").attr("src", "/images/Market_normal.png");
        $("#Index_Sale").attr("src", "/images/saleoutstock_normal.png");
        $("#Index_Stock").attr("src", "/images/Stock_hover.png");
        isInfoClick = false;
        isMarketClick = false;
        isStock = true;
        isSale = false
    }
    else if (type == 4)//资料
    {
        $(".index_dh").css("display", "none");
        $("#index_dh").show();
        $("#index_info").attr("src", "/images/finance_hover.png");
        $("#Market").attr("src", "/images/Market_normal.png");
        $("#Index_Sale").attr("src", "/images/saleoutstock_normal.png");
        $("#Index_Stock").attr("src", "/images/Stock_normal.png");
        isInfoClick = true;
        isMarketClick = false;
        isStock = false;
        isSale=false
    }
}

//刷新订单信息
function ReflashOrderListMsg()
{
    $.ajax({
        url: '/BackStage/GetOrderMsgCount',
        type: 'post',
        dataType: 'text',
        success: function (data) {
            if (data != "") {
                $("#totalMsgCount").text(data);
                $("#NewOrderMsg").text(data);
            }
            else {
                $.messager.alert("刷新订单消息失败");
            }
        },
    });
   
}

//新订单消息跳转
function OpenOrderManager()
{
    if (parseInt($("#NewOrderMsg").text()) > 0)
    {
        AddTab('订单管理', '/BasicInfo/OrderControl?type=msg');

    }
}
var isMarketClick = false;//导航左侧菜单是否被点击过 --商城
var isSale = false;//导航左侧菜单是否被点击过 --销售
var isStock = false;//导航左侧菜单是否被点击过 --进货
var isInfoClick = true;//导航左侧菜单是否被点击过 --资料
//图片鼠标滑过换颜色
function Let_Over(id)
{
    if (id == 0)//商城
    {
        if (isMarketClick) {
            return;
        }
        $("#Market").attr("src", "/images/Market.png");
    }
    else if (id == 1)//销售
    {
        if (isSale) {
            return;
        }
        $("#Index_Sale").attr("src", "/images/saleoutstock_hover.png");
    }
    else if (id == 2)//进货
    {
        if (isStock) {
            return;
        }
        $("#Index_Stock").attr("src", "/images/Stock_hover.png");
    }
    else if (id == 3)//资料
    {
        if (isInfoClick) {
            return;
        }
        $("#index_info").attr("src", "/images/finance_hover.png");
    }
}

//图片鼠标离开恢复原来图片
function Let_Out(id) {
    if (id == 0)//商城
    {
        if (isMarketClick) {
            return;
        }
        $("#Market").attr("src", "/images/Market_normal.png");
    }
    else if (id == 1)//销售
    {
        if (isSale) {
            return;
        }
        $("#Index_Sale").attr("src", "/images/saleoutstock_normal.png");
    }
    else if (id == 2)//进货
    {
        if (isStock) {
            return;
        }
        $("#Index_Stock").attr("src", "/images/Stock_normal.png");
    }
    else if (id == 3)//资料
    {
        if (isInfoClick)
        {
            return;
        }
        $("#index_info").attr("src", "/images/finance_normal.png");
    }
}

//在线咨询
function OnlineConsulting(){
    var url = "http://wpa.qq.com/msgrd?v=3&uin=2025986443&site=qq&menu=yes";
    window.open(url);
}


Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}