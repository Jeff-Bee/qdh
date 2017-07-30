$(function () {
    
    GetStatus();
    PMM_ChangeSize();
   
})

//获取微信/支付宝的绑定状态
function GetStatus()
{
    $.ajax({
        url: '/BasicInfo/GetPMStatus',
        type: 'POST',
        anysc:false,
        success: function (data) {
            if (data != "")
            {
                var json = eval("(" + data + ")");
                if (json.PayType1 != undefined && json.PayType1!="")//微信
                {
                    $("#PM_WX_NoOpen").hide();
                    $("#PM_WX_UName").text(json.MchId1);
                    $("#PM_WX_UName").attr("title", json.MchId1);
                    $("#PM_WX_bottom_NoOpen").hide();
                    $("#PM_WX_bottom_Open").show();

                    $("#PM_WX_ZF_NoOpen").hide();
                    $("#PM_WX_ZF_Open").show();
                    if (json.IsUsed1 == "1") {
                        $("#PM_WX_Open_On").show();
                        $("#PM_WX_Open_Off").hide();

                        
                    }
                    else {
                        $("#PM_WX_Open_Off").show();
                        $("#PM_WX_Open_On").hide();
                    }
                   
                }
                if (json.PayType2 != undefined && json.PayType2 != "")//支付宝
                {
                    $("#PM_ZFB_NoOpen").hide();
                    $("#PM_ZFB_UNanme").text(json.MchId2);
                    $("#PM_ZFB_UNanme").attr("title", json.MchId2);
                    $("#PM_ZFB_Bottom_NoOpen").hide();
                    $("#PM_ZFB_Bottom_Open").show();

                    $("#Pm_ZFB_ZH_NoOpen").hide();
                    $("#Pm_ZFB_ZH_Open").show();
                    if (json.IsUsed2 == "1") {
                        $("#PM_ZFB_Open_On").show();
                        $("#PM_ZFB_Open_Off").hide();

                        
                    }
                    else {
                        $("#PM_ZFB_Open_Off").show();
                        $("#PM_ZFB_Open_On").hide();
                    }
                }
            }
        }
    });
}




//支付宝修改
function PM_ZFB_Edit()
{
    $("#PM_ZFB_Edit").val("1");
    $.ajax({
        url: '/BasicInfo/GetPMModel',
        type: 'POST',
        anysc: false,
        data: { type: 2 },
        success: function (data) {
            if (data != "") {
                var json = eval("(" + data + ")");
                $("#PM_ZFB_APPId").val(json.MchId);
                $("#PM_ZFB_Key").val(json.PartnerId);
                $("#PM_ZFB_MerchantNumber").val(json.AppKey);
                if (json.IsUsed) {
                    $("input[name='PM_ZFB_Status']:eq(0)").prop("checked", true);
                }
                else {
                    $("input[name='PM_ZFB_Status']:eq(1)").prop("checked", true);
                }
                $("#PM_Set_ZFB").dialog({
                    title: '支付宝签约信息',
                    width: '530',
                    modal: true,
                    height: '600'
                });
            }
            else {
                $.messager.alert("提示信息","参数获取失败","info");
            }
        }
    });

   
}

//支付宝状态修改
function PM_ZFB_Change() {
    var state = $("#PM_ZFB_Open_On").css("display");
    if (state == "none") {
        $.messager.confirm("确认", "确认启用　支付宝支付　？", function (flag) {
            if (flag) 
            {
                $.ajax({
                    url: '/BasicInfo/PM_Change_IsUsed',
                    type: 'POST',
                    anysc: false,
                    data: { type: 2 },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '状态修改成功!'
                            });
                            GetStatus();
                        }
                        else if (data == "0") {
                            $.messager.alert("提示信息", "修改状态失败!", "info");
                        }
                        else {
                            $.messager.alert("提示信息", "获取参数出错!", "info");
                        }
                    }
                });
            }
        });
    }
    else {
        $.messager.confirm("确认", "该操作将导致订货客户无法使用 支付宝支付 付款，确认停用？", function (flag) {
            if (flag) 
            {
                $.ajax({
                    url: '/BasicInfo/PM_Change_IsUsed',
                    type: 'POST',
                    anysc: false,
                    data: { type: 2 },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '状态修改成功!'
                            });
                            GetStatus();
                        }
                        else if (data == "0") {
                            $.messager.alert("提示信息", "修改状态失败!", "info");
                        }
                        else {
                            $.messager.alert("提示信息", "获取参数出错!", "info");
                        }
                    }
                });
            }
        });
    }
   
}

//微信状态修改
function PM_WX_Change() {
    var state = $("#PM_WX_Open_On").css("display");
    if (state == "none") {
        $.messager.confirm("确认", "确认启用　微信支付　？", function (flag) {
            if (flag) {
                $.ajax({
                    url: '/BasicInfo/PM_Change_IsUsed',
                    type: 'POST',
                    anysc: false,
                    data: { type: 1 },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '状态修改成功!'
                            });
                            GetStatus();
                        }
                        else if (data == "0") {
                            $.messager.alert("提示信息", "修改状态失败!", "info");
                        }
                        else {
                            $.messager.alert("提示信息", "获取参数出错!", "info");
                        }
                    }
                });
            }
        });
    }
    else {
        $.messager.confirm("确认", "该操作将导致订货客户无法使用 微信支付 付款，确认停用？", function (flag) {
            if (flag) {
                $.ajax({
                    url: '/BasicInfo/PM_Change_IsUsed',
                    type: 'POST',
                    anysc: false,
                    data: { type: 1 },
                    success: function (data) {
                        if (data == "1") {
                            $.messager.show({
                                title: '提示',
                                msg: '状态修改成功!'
                            });
                            GetStatus();
                        }
                        else if (data == "0") {
                            $.messager.alert("提示信息", "修改状态失败!", "info");
                        }
                        else {
                            $.messager.alert("提示信息", "获取参数出错!", "info");
                        }
                    }
                });
            }
        });
    }
    
}

//微信修改
function PM_WX_Edit() {
    $("#PM_WX_Edit").val("1");
    $.ajax({
        url: '/BasicInfo/GetPMModel',
        type: 'POST',
        anysc: false,
        data: { type: 1 },
        success: function (data) {
            if (data != "") {
                var json = eval("(" + data + ")");
                $("#PM_APPId").val(json.AppId);
                $("#PM_Key").val(json.AppSecret);
                $("#PM_MerchantNumber").val(json.MchId);
                $("#PM_APPKey").val(json.AppKey);
                if (json.IsUsed) {
                    $("input[name='PM_Status']:eq(0)").prop("checked", true);
                }
                else {
                    $("input[name='PM_Status']:eq(1)").prop("checked", true);
                }
                $("#PM_Set_WX").dialog({
                    title: '微信签约信息',
                    width: '400',
                    modal: true,
                    height: '370'
                });
            }
            else {
                $.messager.alert("提示信息", "参数获取失败", "info");
            }
        }
    });


   
}

//支付宝页面设置
function PM_SetZFB()
{
    $("#PM_Set_ZFB").dialog({
        title: '支付宝签约信息',
        width: '530',
        modal: true,
        height:'600'
    });
}
//支付宝设置页面关闭
function PM_SetZFB_close()
{
    $("#PM_Set_ZFB").dialog("close");
    $("#PM_Set_ZFB")[0].reset();
    $("#PM_ZFB_Edit").val("0");
}

//微信页面设置
function PM_SetWX() {
    $("#PM_Set_WX").dialog({
        title: '微信签约信息',
        width: '400',
        modal: true,
        height: '370'
    });
}
//微信设置页面关闭
function PM_SetWX_close() {
    $("#PM_Set_WX").dialog("close");
    $("#PM_Set_WX")[0].reset();
    $("#PM_WX_Edit").val("0");
}

//支付宝设置保存
function ZFB_Save()
{
    if ($("#PM_ZFB_APPId").val() == "")
    {
        $.messager.alert("提示信息", "支付宝企业账户不能为空", "info");
        $("#PM_ZFB_APPId").focus();
        return false;
    }
    else if ($("#PM_ZFB_Key").val() == "")
    {
        $.messager.alert("提示信息", "合作者身份不能为空", "info");
        $("#PM_ZFB_Key").focus();
        return false;
    }
    else if ($("#PM_ZFB_MerchantNumber").val() == "") {
        $.messager.alert("提示信息", "安全校验码不能为空", "info");
        $("#PM_ZFB_MerchantNumber").focus();
        return false;
    }
    else {
        var json = "{";
        json += "\"PayType\":\"2\",\"MchId\":\"" + $("#PM_ZFB_APPId").val() + "\",\"PartnerId\":\"" + $("#PM_ZFB_Key").val() + "\",\"AppKey\":\"" + $("#PM_ZFB_MerchantNumber").val() + "\",\"IsUsed\":" + $("input[name='PM_ZFB_Status']:checked").val() + "";
        json += "}";
        $.ajax({
            url: '/BasicInfo/ZFBSetSave',
            type: 'POST',
            anysc:false,
            data: { str:json, type: $("#PM_ZFB_Edit").val()=="0"?"add":"edit"},
            success: function (data) {
                if (data == "1")
                {
                    $.messager.show({
                        title: '提示',
                        msg:'保存成功!'
                    });
                    GetStatus();
                    PM_SetZFB_close();
                    $("#PM_Set_ZFB")[0].reset();
                    
                }
                else if (data == "0") {
                    $.messager.alert("提示信息","保存失败","info");
                   
                }
                else {
                    $.messager.alert("提示信息", "获取参数失败", "info");
                }
            }
        });
    }
}
//微信设置保存
function WX_Save() {
    if ($("#PM_APPId").val() == "")
    {
        $.messager.alert("提示信息", "应用ID不能为空", "info");
        $("#PM_APPId").focus();
        return false;
    }
    else if ($("#PM_Key").val() == "")
    {
        $.messager.alert("提示信息", "应用密钥不能为空", "info");
        $("#PM_Key").focus();
        return false;
    }
    else if ($("#PM_MerchantNumber").val() == "") {
        $.messager.alert("提示信息", "商户号不能为空", "info");
        $("#PM_MerchantNumber").focus();
        return false;
    }
    else if ($("#PM_APPKey").val() == "") {
        $.messager.alert("提示信息", "API密钥不能为空", "info");
        $("#PM_APPKey").focus();
        return false;
    }
    else {
        var json = "{";
        json += "\"PayType\":\"1\",\"AppId\":\"" + $("#PM_APPId").val() + "\",\"AppSecret\":\"" + $("#PM_Key").val() + "\",\"MchId\":\"" + $("#PM_MerchantNumber").val() + "\",\"AppKey\":\"" + $("#PM_APPKey").val() + "\",\"IsUsed\":" + $("input[name='PM_Status']:checked").val() + "";
        json += "}";
        $.ajax({
            url: '/BasicInfo/ZFBSetSave',
            type: 'POST',
            anysc: false,
            data: { str: json, type: $("#PM_WX_Edit").val()=="0"?"add":"edit" },
            success: function (data) {
                if (data == "1") {
                    $.messager.show({
                        title: '提示',
                        msg: '保存成功!'
                    });
                    GetStatus();
                    PM_SetWX_close();
                    $("#PM_Set_WX")[0].reset();
                }
                else if (data == "0") {
                    $.messager.alert("提示信息", "保存失败", "info");

                }
                else {
                    $.messager.alert("提示信息", "获取参数失败", "info");
                }
            }
        });
    }
}

//支付宝资费说明
function PM_ZFB_ZFSM()
{
    $("#PM_ZFB_ZFSM").dialog({
        title: '支付宝资费说明',
        width: '565',
        modal: true,
        height: '570'
    });
}
//支付宝资费说明关闭
function PM_ZFB_ZFSM_close()
{
    $("#PM_ZFB_ZFSM").dialog("close");
}

//微信资费说明
function PM_WX_ZFSM() {
    $("#PM_WX_ZFSM").dialog({
        title: '微信资费说明',
        width: '510',
        modal: true,
        height: '260'
    });
}
//微信资费说明关闭
function PM_WX_ZFSM_close() {
    $("#PM_WX_ZFSM").dialog("close");
}


function PMM_ChangeSize()
{
    //OnLinePayment
    var height = document.documentElement.clientHeight - 250;
    if (height > 400) {
        $("#OnLinePayment").height(height);
    }
}