/**
 * @version $Id$
 * @author xjiujiu <xjiujiu@foxmail.com>
 * @description HongJuZi Framework
 * @copyright Copyright (c) 2011-2012 http://www.xjiujiu.com.All right reserved
 */
HAction['loading'] = {
    page: null,
    init: function () {
        this.page = $$('#loading');
        var url = window.location.href;
        var part = url.indexOf('!');
        if (1 == this.page.find('#is-init').val()) {
            if (part < 0) {
                HJZApp.mainView.router.loadPage({ 'url': 'index/main', 'ignoreCache': true });
                return;
            }
            var modal = url.substring(part).split('/');
            if (!modal[1]) {
                HJZApp.mainView.router.loadPage({ 'url': 'index/main', 'ignoreCache': true });
                return;
            }
            if (modal[1] == 'index' && modal[2] == '') {
                HJZApp.mainView.router.loadPage({ 'url': 'index/main', 'ignoreCache': true });
                return;
            }
            if (modal[1] == 'index' && modal[2] == 'index') {
                HJZApp.mainView.router.loadPage({ 'url': 'index/main', 'ignoreCache': true });
                return;
            }
            console.log(modal);
        }
    }
};
