/**分享页面弹层**/
$(document).on("click",".sharebtnjs",function(){
	$(".sharehidebox").show();
});
/**商品详情页资料未审核弹层**/
$(document).on("touchend",".Not_auditejs",function(){
	$(".Not_auditedhidebox").show();
});
/**个人中心拨打客服电话弹层**/
$(document).on("click",".callbtnjs",function(){
	$(".callhidebox").show();
});

/**单选**/
$(document).on("click",".paylist li",function(){
	$(this).addClass("active");
	$(this).find(".payput").attr("checked","checked");
	$(this).siblings().removeClass("active");
	$(this).siblings().find(".payput").removeAttr("checked");
});

/**购物车多选**/
$(document).on('touchend','.labeljs', function () {
	if($(this).hasClass("active")){
		$(this).removeClass("active");
		$(this).find(".labput").removeAttr("checked");
	}else{
		$(this).addClass("active");
		$(this).find(".labput").attr("checked","checked");
	}
});
//全选
$(document).on('click','.allselectjs', function () {
	$(".productlistbox").find(".labeljs").addClass("active");
	$(".productlistbox").find(".labput").attr("checked","checked");
});
//同意协议
$(document).on("touchend",".agreecheckjs",function(){
	if($(this).find(".icon").hasClass("icon-check-gray")){
		$(this).find(".icon").addClass("icon-check-on");
		$(this).find(".icon").removeClass("icon-check-gray");
		$(this).siblings().addClass("button-bule");
		$(this).siblings().removeClass("button-gray");
		 $("#iagree").attr("checked",true);
		// alert($("#iagree").is(':checked'));
	}else{
		$(this).find(".icon").removeClass("icon-check-on");
		$(this).find(".icon").addClass("icon-check-gray");
		$(this).siblings().removeClass("button-bule");
		$(this).siblings().addClass("button-gray");
		 $("#iagree").attr('checked',false); 
		// alert($("#iagree").is(':checked'));
	}
})
/*
 * 协议弹窗
 */
$(document).on("click","#xieyi-next",function(){
	if(!$("#iagree").is(':checked')){
		alert('请勾选我已阅读并同意协议后进入');
		return false;
	}
})
/**购物城编辑数量**/
 function selectPeople(object,min,max,price){
        var object = object;
        var pepole;
        var min = Number(min);
        var max = Number(max);
        var price = Number(price)
        //获取当前数
        var curPeople = Number(object.siblings().find("input").val());
        if(object.hasClass("decrease")){//点击减号
            curPeople -= 1;
            if(curPeople <=min){
                curPeople = min;
            }
			if(curPeople < (min+1)){
				object.addClass("gray");
				if(object.hasClass("indexbtn_cut")){
					object.hide();
				}
			}
		object.siblings().find("input").attr('value',curPeople);
        }else if(object.hasClass("increase")){//点击加号
            curPeople += 1;
            if(curPeople > max){
                curPeople = max;
            }
			if(curPeople > 0){
				object.siblings().removeClass("gray");
				object.siblings(".indexbtn_cut").show();
			}
			object.siblings().find("input").attr('value',curPeople);
        }
}
    $(document).ready(function(){
        $(".decrease,.increase").click(function(){
         var price = $(this).parent().attr('price');
         if (!price) price = 0;
         selectPeople($(this),1,100, price);
         });
    })

