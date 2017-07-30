
/*单选*/
$(document).on("click",".check-box",function(){
    $('.check-box').click(function(){
       $('.check-box').children('span').removeClass('icon-radio-blue').addClass('icon-radio-gray');
       $(this).children('span').addClass('icon-radio-blue');
    });
     $('#slider-list-box').swipeSlide({
            continuousScroll:true,
            speed : 3000,
            transitionType : 'cubic-bezier(0.22, 0.69, 0.72, 0.88)',
            callback : function(i){
                $('.dot').children().eq(i).addClass('cur').siblings().removeClass('cur');
            }
        });
})

