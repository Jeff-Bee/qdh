


var userLogin = true;
var app = new Framework7({
    swipeBackPage: false,
    pushState: true,
    cache: false,
    cacheIgnore: ['user/address', 'order/cart', 'order/delcart', 'order/orderlist'],
    modalTitle: '冰团网',
    modalButtonOk: '确认',
    modalButtonCancel: '取消'
});
var $$ = Dom7;

/**
 * @var var HAction   H控制器
 */
var HAction = [];
//初始化
var HJZApp = {
    mainView: null,
    init: function () {
        this.initMainView();
        this.initPageEvents();
        if (true == isSingle) {
            return;
        }
        if (isLogined) {
            this.initLoading();
        } else {
            this.initLogin();
        }
    },
    initLogin: function () {
        require(['/js/login.js'], function () {
            HAction['login'].init();
        });
    },
    initLoading: function () {
        require(['/js/loading.js'], function () {
            HAction['loading'].init();
        });
    },
    initMainView: function () {
        this.mainView = app.addView('.view-main', {
            dynamicNavbar: false
        });
    },
    initPageEvents: function () {
        var self = this;
        app.onPageBeforeInit('*', function (page) {
            page.context = $$('#' + page.name);
            var js = page.context.attr('data-js');
            var pageLogined = page.context.attr('data-logined');
            if (2 == js) {
                return;
            }
            var t = (new Date()).getTime();
            var jsPath = '/js/' + page.name + '.js?_=' + t;
            require([jsPath], function () {
                console.info('[Done] - ' + jsPath);
                HAction[page.name].init();
            });
        });
    }
};

//初始化应用
HJZApp.init();
