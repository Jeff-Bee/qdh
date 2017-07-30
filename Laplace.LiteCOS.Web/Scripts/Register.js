//返回登录页面
function ReturnLogin()
{
    location.href = "/UserCenter/Login";
}

//获取cookie
function getCookie(cookie_name) {
    var allcookies = document.cookie;
    var cookie_pos = allcookies.indexOf(cookie_name);   //索引的长度

    // 如果找到了索引，就代表cookie存在，
    // 反之，就说明不存在。
    if (cookie_pos != -1) {
        // 把cookie_pos放在值的开始，只要给值加1即可。
        cookie_pos += cookie_name.length + 1;      //这里容易出问题，所以请大家参考的时候自己好好研究一下
        var cookie_end = allcookies.indexOf(";", cookie_pos);

        if (cookie_end == -1) {
            cookie_end = allcookies.length;
        }

        var value = unescape(allcookies.substring(cookie_pos, cookie_end));         //这里就可以得到你想要的cookie的值了。。。
    }
    return value;
}

//获取校验码
function GetCheckCode()
{
    var varCode = $("input[name=verCode]").val();
    if ($("input[name=phone]").val() == "") {
        alert('请输入手机号');
        $("input[name=phone]").focus();
    }
    else if ($("input[name=verCode]").val() == "")
    {
        alert('请输入验证码');
        $("input[name=verCode]").focus();
    }
    else if (varCode.toUpperCase() != getCookie("CheckCode"))
    {
        alert('验证码不正确,请重新输入');
        $("input[name=verCode]").val("");
        $("input[name=verCode]").focus();
    }
    else {
        $.ajax({
            url: '/UserCenter/GetVerCode',
            type: 'POST',
            data: { phone: $("input[name=phone]").val() },
            async:false,
            success: function (data) {
                if (data != "") {
                    alert(data);
                }
                else {
                    num = 90;
                    $("#getCode").val(num);
                    $("#getCode").attr("disabled", true);
                    //$("#getCode").css("color","gray");
                    setInterval('updateNum()', 1000);
                    
                }
            }
        });
    }
}
var num=90;
//动态显示时间
function updateNum() {
    if (num > 0) {
        num = num - 1;
        $("#getCode").val(num)
    }
    else {
        $("#getCode").attr("disabled", false);
        $("#getCode").val("获取验证码");
    }
}

//手机格式验证
function CheckPhone(obj)
{
    var test = /^1[34578]\d{9}$/;
    if(!test.test(obj.val()))
    {
        obj.val("");
        obj.focus();
    }
}

//点击下一步，提交表单
function NextSubmit()
{
    var varCode = $("input[name=verCode]").val();
    if ($("input[name=phone]").val() == "")
    {
        alert('请输入手机号');
        $("input[name=phone]").focus();
        return false;
    }
    else if ($("input[name=verCode]").val() == "") {
        alert('请输入验证码');
        $("input[name=verCode]").focus();
        return false;
    }
    else if (varCode.toUpperCase() != getCookie("CheckCode")) {
        alert('验证码不正确,请重新输入');
        $("input[name=verCode]").val("");
        $("input[name=verCode]").focus();
        return false;
    }
    else if ($("input[name=jyCode]").val() == "") {
        alert('校验码不能为空');
        $("input[name=jyCode]").val("");
        $("input[name=jyCode]").focus();
        return false;
    }
    else if (!$("input[name=promise]").is(":checked")) {
        alert('请先同意条款');
        $("input[name=promise]").val("");
        $("input[name=promise]").focus();
        return false;
    }
    else {
        return true;
    }
}

/*----------第一步验证结束-----------*/

/*----------第二步验证-----------*/
//验证第二步的提交
function Step2Sybmit()
{
    if ($("input[name=cname]").val() == "")
    {
        alert("请输入公司名称");
        $("input[name=cname]").focus();
        return false;
    }
    if ($("input[name=pwd]").val() == "") {
        alert("设置你的登录密码");
        $("input[name=pwd]").focus();
        return false;
    }
    if ($("input[name=contact]").val() == "") {
        alert("请输入联系人姓名");
        $("input[name=contact]").focus();
        return false;
    }
    if ($("#industry").val() == "") {
        alert("请选择所选行业");
        $("#industry").focus();
        return false;
    }
    else {
        return true;
    }
}

/*----------第二步验证结束-----------*/

/*--------------找回密码验证-----------------*/
//提交验证
function ForgetPwdCheck()
{

    if ($("input[name=telphone]").val() == "") {
        alert("请输入手机号");
        $("input[name=telphone]").focus();
        return false;
    }
    else {
        $.ajax({
            url: '/UserCenter/GetPwd',
            type: 'POST',
            data: { telphone: $("input[name=telphone]").val() },
            async: false,
            success: function (data) {
                if (data =="1") {
                    alert("密码已发送到你的手机,请注意查收!");
                    location.href = "/UserCenter/Login";
                }
                else {
                    alert(data);
                }
            }
        });
    }
}

//获取验证码
function GetVarCode()
{
    if ($("input[name=telphone]").val() == "") {
        alert("请输入手机号");
        $("input[name=telphone]").focus();
    }
    else {
        $.ajax({
            url: '/UserCenter/GetForgetPwdCode',
            type: 'POST',
            data: { phone: $("input[name=telphone]").val() },
            async: false,
            success: function (data) {
                if (data != "") {
                    alert(data);
                }
                else {
                    num = 90;
                    $("#getCode").val(num);
                    $("#getCode").css("color", "gray");
                    setInterval('updateNum()', 1000);
                }
            }
        });
    }
}
/*--------------找回密码验证结束-----------------*/
