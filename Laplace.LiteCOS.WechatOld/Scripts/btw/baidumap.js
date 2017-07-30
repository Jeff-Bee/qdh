var HJZBaiDuMap = {
    callback: null,
    $page: null,
    bindGetPosition: function ($page, dom, callback) {
        var self = this;
        $page.find(dom).on('click', function () {
            self.startGetPosition($page, callback);
        });
    },
    startGetPosition: function ($page, callback) {
        var self = this;
        this.$page = $page;
        this.callback = 'undefined' != typeof (callback) ? callback : null;
        app.showPreloader("正在为您定位中...");
        require(['/js/map/CityList_min.js'], function () {
            self.getLocationByCity();
        });
    },
    getLocationByCity: function () {
        var self = this;
        var myCity = new BMap.LocalCity();
        myCity.get(function (result) {
            var cityName = result.name;
            self.getLocationByBrowser(cityName);
        });
    },
    bmapStatusMap: {
        'status-0': '检索成功',
        'status-1': '城市列表',
        'status-2': '位置结果未知',
        'status-3': '导航结果未知',
        'status-4': '非法密钥',
        'status-5': '非法请求',
        'status-6': '没有权限',
        'status-7': '服务不可用',
        'status-8': '超时'
    },
    getLocationByBrowser: function (cityName) {
        var self = this;
        var geolocation = new BMap.Geolocation();
        geolocation.getCurrentPosition(function (r) {
            app.hidePreloader();
            var status = this.getStatus();
            if (status == BMAP_STATUS_SUCCESS) {
                window.lng = r.point.lng;
                window.lat = r.point.lat;
                self.setAddress(r.point.lng, r.point.lat);
            } else {
                app.alert(self.bmapStatusMap['status' + status]);
            }
        }, { enableHighAccuracy: true });
    },
    setAddress: function (lng, lat) {
        var self = this;
        var myGeo = new BMap.Geocoder();
        myGeo.getLocation(new BMap.Point(lng, lat), function (rs) {
            if (!rs) {
                return app.alert('地址信息获取失败，请重新选择！');
            }
            var addComp = rs.addressComponents;
            addComp.lat = lat;
            addComp.lng = lng;
            self.$page.find('#cur-location').html(
                addComp.province
                + addComp.city
                + addComp.district
                + addComp.street
                + addComp.streetNumber
            );
            self.getCurAddress(addComp);
        });
    },
    getCurAddress: function (address) {
        var self = this;
        $$.getJSON(
            '/index/getcuraddress',
            address,
            function (response) {
                if (1 != response.success) {
                    app.alert(response.data);
                    return;
                }
                provinceId = response.data.province_id;
                if (null !== self.callback) {
                    self.callback(response.data);
                }
            }
        );
    }
};
