/**
 * @version $Id$
 * @author xjiujiu <xjiujiu@foxmail.com>
 * @description HongJuZi Framework
 * @copyright Copyright (c) 2011-2012 http://www.xjiujiu.com.All right reserved
 */
HAction['index'] = {
    $page: null,
    myGeo: null,
    init: function () {
        this.$page = $$('#index');
        this.bindCartNumBtns(this.$page);
        this.bindLoadMoreList();
        this.bindGetPosition();
        this.bindSwiper();
    },
    bindSwiper: function () {
        app.swiper('.swiper-1', {
            pagination: '.swiper-1 .swiper-pagination',
            spaceBetween: 50
        });
    },
    bindGetPosition: function () {
        var self = this;
        if (provinceId) {
            return;
        }
        HJZBaiDuMap.startGetPosition(this.$page, function (loc) {
            window.location.reload();
        });
    },
    bindLoadMoreList: function () {
        var self = this;
        this.$page.find('a.btn-index-load-more').click(function () {
            var $this = $$(this);
            var page = $this.attr('data-page');
            var brand = self.$page.find('input.goods-brand').val();
            var type = self.$page.find('input.goods-type').val();
            $$.getJSON(
                'goods/alist',
                { page: page, b: brand, c: type },
                function (response) {
                    if (false === response.rs) {
                        app.alert(response.message);
                        return;
                    }
                    if (response.data.list.length < 1) {
                        app.alert('已经全部加载完成啦～');
                        $this.hide();
                        return;
                    }
                    $this.attr('data-page', response.data.page);
                    self.appendGoodsList(response.data.list);
                }
            );
        });
    },
    appendGoodsList: function (list) {
        var listHtml = '';
        var itemTpl = $$('#goods-item-tpl').html();
        var t = (new Date()).getTime();
        for (var ele in list) {
            var item = list[ele];
            var prices = this._getGoodsPrice(item);
            var level = this._getLevelNums(item);
            var xianling = this._getXianLingData(item);
            var typeMap = this._getTypeMap(item);
            var activityInfo = this._getActivityInfo(item, typeMap);
            var amount = this._getAmount(item);
            var actType = item.activity_goods ? typeMap[item.activity_goods.type][0] : '';
            var itemHtml = itemTpl.replace(/{id}/g, item.goods_id)
            .replace(/{name}/g, item.goods_name.substring(0, 8))
            .replace(/{fullname}/g, item.goods_name)
            .replace(/{img}/g, item.goods_thumb)
            .replace(/{t}/g, t)
            .replace(/{aid}/g, item.goods_area_id)
            .replace(/{sp}/g, prices.activity_price)
            .replace(/{mp}/g, prices.shop_price)
            .replace(/{level}/g, level)
            .replace(/{xianling}/g, xianling)
            .replace(/{activityinfo}/g, activityInfo)
            .replace(/{acttype}/g, actType)
            .replace(/{receivenum}/g, item.activity_goods.receiving_num)
            .replace(/{freenum}/g, item.activity_goods.free_num)
            .replace(/{activityid}/g, item.activity_goods.activity_id)
            .replace(/{amount}/g, amount);

            listHtml += itemHtml;
        }

        this.$page.find('#goods-list-box').append(listHtml);
        this.bindCartNumBtns($$('div.append-' + t));
    },
    _getActivityInfo: function (item, typeMap) {
        if (!item.activity_goods) {
            return '';
        }
        var tpl = '<span class="label-activity ' + typeMap[item.activity_goods.type][0] + '-span"></span><font>' + typeMap[item.activity_goods.type][1] + '</font>';
        if (item.activity_goods.type == 3 && item.activity_goods.goods_num > 0) {
            return tpl;
        } else {
            return tpl;
        }
    },
    _getGoodsPrice: function (item) {
        var time = new Date().getTime();
        var prices = {};
        if (item.activity_goods.type == 3 && item.activity_goods.goods_num > 0 && item.activity_goods.end_time > time) {
            prices.activity_price = item.activity_goods.activity_price;
            prices.shop_price = item.market_price;
        } else {
            prices.activity_price = item.shop_price;
            prices.shop_price = item.market_price
        }

        return prices;
    },
    _getLevelNums: function (item) {
        if (item.activity_goods && item.activity_goods.goods_num > 0) {
            return item.activity_goods.goods_num;
        }

        return item.amount;
    },
    _getXianLingData: function (item) {
        if (item.activity_goods.type == 2) {
            return '限领<span class="text-red">' + item.activity_goods.free_num + '</span>' + item.goods_unit;
        }
        return ' ';
    },
    _getTypeMap: function (item) {
        var typeMap = [];
        typeMap[3] = ['limit', '限时抢购'];
        typeMap[2] = ['free', '免费限领'];
        typeMap[1] = ['song', '买' + item.activity_goods.receiving_num + '送' + item.activity_goods.free_num];

        return typeMap;
    },
    _getAmount: function (item) {
        var amount = item.amount;
        if (item.activity_goods.type == 1 && item.activity_goods.goods_num > 0) {
            amount = item.activity_goods.goods_num;
        }
        if (item.activity_goods.type == 2) {
            amount = item.activity_goods.free_num;
        }
        if (item.activity_goods.type == 3 && item.activity_goods.goods_num > 0) {
            amount = item.activity_goods.goods_num;
        }

        return amount;
    },
    bindCartNumBtns: function ($dom) {
        var num = parseInt($dom.find('input.num').val());
        $dom.find('.btn-sub').each(function () {
            var numObj = $$(this).parent().find('input.num');
            if (numObj.val() < 1) {
                $$(this).addClass('hide');
            }
        });
        $dom.find('input.num').on('change', function () {
            var $num = $$(this);
            var $$parent = $$(this).parent();
            var amount = parseInt($num.attr("data-amount"));
            var num = parseInt($num.val());
            var actType = $$parent.attr('data-act-type');
            if (num > amount) {
                var message = "添加商品的数量不能大于库存数哦～";
                if (actType == 'free') {
                    message = '添加商品的数量不能大于最大限购额哦～';
                }
                app.alert(message);
                $num.val(amount);
                $$parent.find('.btn-add').attr('disabled', 'disabled');
                return;
            }
            if (typeof actType !== 'undefined' && actType == 'song') {
                var receiveNum = parseInt($$parent.attr('data-receive-num'));
                var freeNum = parseInt($$parent.attr('data-free-num'));
                var songNum = parseInt(num / receiveNum) * freeNum;
                var songStr = '';
                if (songNum > 0) {
                    songStr = '+' + songNum;
                }
                if ((parseInt(num) + parseInt(songNum)) > amount) {
                    app.alert('添加商品的数量不能大于库存数哦～');
                    return;
                }
                $$parent.children('.song').html(songStr);
            }
            if (num - 1 < 0) {
                $$parent.find('input.num').val(0);
                $$parent.find('.btn-sub').addClass('hide');
                $$parent.find('.no-border').removeClass('hide');
                $$parent.find('input').addClass('hide');
                $$parent.find('.btn-add').removeAttr('disabled');
            } else {
                $num.parent().find('.btn-sub').removeClass('hide');
            }
            var data = {
                goods_id: $$(this).attr('data-id'),
                goods_num: num,
                goods_name: $$(this).attr('goods-name'),
                goods_area_id: $$(this).attr('area-id'),
                activity_id: $$(this).attr('activity_id'),
                from_change: 1
            };
            $$.post('order/AjaxCart', data, function (data) {
                var result = JSON.parse(data);
                if (result.data.total_nums > 0) {
                    $dom.find('.icon-num-info').removeClass('hide');
                }
                $dom.find('#cart_nums').text(result.data.total_nums);
                //相关运费判断
                if (result.data.total_price > 30) {
                    $dom.find('#yunfei-info').text('购物车');
                } else {
                    $dom.find('#yunfei-info').text('￥30起免运费');
                }
            });
        });
        $dom.find('a.btn-add').on('click', function () {
            var $$parent = $$(this).parent();
            $$parent.find('.btn-sub').removeClass('hide');
            $$parent.find('.no-border').addClass('hide');
            $$parent.find('input').removeClass('hide');
            var $num = $$parent.find('input.num');
            var amount = parseInt($num.attr("data-amount"));
            var num = parseInt($num.val());
            num += 1;
            var actType = $$parent.attr('data-act-type');
            if (num > amount) {
                var message = "添加商品的数量不能大于库存数哦～";
                if (actType == 'free') {
                    message = '添加商品的数量不能大于最大限购额哦～';
                }
                app.alert(message);
                return;
            }
            if (num == amount) {
                $$(this).attr('disabled', 'disabled');
            }
            $num.val(num);
            if (typeof actType !== 'undefined' && actType == 'song') {
                var receiveNum = parseInt($$parent.attr('data-receive-num'));
                var freeNum = parseInt($$parent.attr('data-free-num'));
                var songNum = parseInt(num / receiveNum) * freeNum;
                var songStr = '';
                if (songNum > 0) {
                    songStr = '+' + songNum;
                }
                if ((parseInt(num) + parseInt(songNum)) == amount) {
                    $$(this).attr('disabled', 'disabled');
                }
                $$parent.children('.song').html(songStr);
            }
            var data = {
                goods_id: $$(this).attr('data-id'), goods_num: 1,
                goods_name: $$(this).attr('goods-name'),
                goods_area_id: $$(this).attr('area-id'),
                activity_id: $$(this).attr('activity_id')
            };
            $$.post('order/AjaxCart', data, function (data) {
                var result = JSON.parse(data);
                if (result.data.total_nums > 0) {
                    $dom.find('.icon-num-info').removeClass('hide');
                }
                $dom.find('#cart_nums').text(result.data.total_nums);
                //相关运费判断
                if (result.data.total_price > 30) {
                    $dom.find('#yunfei-info').text('购物车');
                } else {
                    $dom.find('#yunfei-info').text('￥30起免运费');
                }
            });
        });
        $dom.find('a.btn-sub').on('click', function () {
            var $$parent = $$(this).parent();
            var num = parseInt($$parent.find('input.num').val());
            if (num - 1 <= 0 || isNaN(num)) {
                $$parent.find('.btn-sub').addClass('hide');
                $$parent.find('.no-border').removeClass('hide');
                $$parent.find('input').addClass('hide');
            }
            var actType = $$parent.attr('data-act-type');
            if (typeof actType !== 'undefined' && actType == 'song') {
                var receiveNum = parseInt($$parent.attr('data-receive-num'));
                var freeNum = parseInt($$parent.attr('data-free-num'));
                var songNum = parseInt((num - 1) / receiveNum) * freeNum;
                var songStr = '';
                if (songNum > 0) {
                    songStr = '+' + songNum;
                }
                $$parent.children('.song').html(songStr);
            }
            if (num - 1 < 0 || isNaN(num)) {
                $$parent.find('input.num').val(0);
                return;
            } else {
                $$parent.find('a.btn-add').removeAttr('disabled');
                var data = {
                    goods_id: $$(this).attr('data-id'), goods_num: -1,
                    goods_name: $$(this).attr('goods-name'),
                    goods_area_id: $$(this).attr('area-id'),
                    activity_id: $$(this).attr('activity_id')
                };
                $$.post('order/AjaxCart', data, function (data) {
                    var result = JSON.parse(data);
                    if (result.data.total_nums == 0) {
                        $dom.find('.icon-num-info').addClass('hide');
                    } else {
                        $dom.find('#cart_nums').text(result.data.total_nums);
                    }
                    //相关运费判断
                    if (result.data.total_price > 30) {
                        $dom.find('#yunfei-info').text('购物车');
                    } else {
                        $dom.find('#yunfei-info').text('￥30起免运费');
                    }
                });
            }
            $$parent.find('input.num').val(num - 1);
        });
    }
};
