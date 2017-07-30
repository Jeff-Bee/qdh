/*
功能：		创建【卖家商品上线销售】表
创建日期：	2016-11-07 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerProductOnline') and o.name = 'FK_SELLERPR_REFERENCE_PRODUCTI')
alter table SellerProductOnline
   drop constraint FK_SELLERPR_REFERENCE_PRODUCTI
go

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerProductOnline]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerProductOnline]

CREATE TABLE [dbo].[SellerProductOnline] (
	[ProductId]				int						NOT NULL PRIMARY KEY,			/*商品编号(ProductInfo:ProductId)*/
	[SellerId] 				int						NOT NULL, 						/*卖家编号(SellerInfo:SellerId)*/
	
	[SaleState]				int						not null DEFAULT((0)),			/*销售状态*/
	[SaleStartDate]			datetime				not null DEFAULT (getdate()),	/*销售开始日期*/
	[SaleEndDate]			datetime				not null DEFAULT ('2100-01-01'),/*销售结束日期*/
	[SaleStoreQuantity]		int						not null,						/*销售库存量*/
	[Price1]				money					not null,						/*销售价格1*/
	[Price2]				money					not null DEFAULT((0)),			/*销售价格2*/
	[Price3]				money					not null DEFAULT((0)),			/*销售价格3*/
	[Price4]				money					not null DEFAULT((0)),			/*销售价格4*/
	[Price5]				money					not null DEFAULT((0)),			/*销售价格5*/
	[Price6]				money					not null DEFAULT((0)),			/*销售价格6*/
	[Price7]				money					not null DEFAULT((0)),			/*销售价格7*/
	[Price8]				money					not null DEFAULT((0)),			/*销售价格8*/
	[MinOrder1]				smallint				not null DEFAULT((1)),			/*最小订购量1*/
	[MinOrder2]				smallint				not null DEFAULT((1)),			/*最小订购量2*/
	[MinOrder3]				smallint				not null DEFAULT((1)),			/*最小订购量3*/
	[MinOrder4]				smallint				not null DEFAULT((1)),			/*最小订购量4*/
	[MinOrder5]				smallint				not null DEFAULT((1)),			/*最小订购量5*/
	[MinOrder6]				smallint				not null DEFAULT((1)),			/*最小订购量6*/
	[MinOrder7]				smallint				not null DEFAULT((1)),			/*最小订购量7*/
	[MinOrder8]				smallint				not null DEFAULT((1)),			/*最小订购量8*/


	[MaxOrder1]				int						not null DEFAULT((9999)),		/*最大订购量1*/
	[MaxOrder2]				int						not null DEFAULT((9999)),		/*最大订购量2*/
	[MaxOrder3]				int						not null DEFAULT((9999)),		/*最大订购量3*/
	[MaxOrder4]				int						not null DEFAULT((9999)),		/*最大订购量4*/
	[MaxOrder5]				int						not null DEFAULT((9999)),		/*最大订购量5*/
	[MaxOrder6]				int						not null DEFAULT((9999)),		/*最大订购量6*/
	[MaxOrder7]				int						not null DEFAULT((9999)),		/*最大订购量7*/
	[MaxOrder8]				int						not null DEFAULT((9999)),		/*最大订购量8*/
	[Strategy]				int						not null DEFAULT((0)),			/*销售策略*/

	[IsNew]					bit						not null DEFAULT((0)),			/*标记是否是新品*/
	[IsPromotion]			bit						not null DEFAULT((0)),			/*标记是否是促销商品*/
	

	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];

alter table SellerProductOnline
   add constraint FK_SELLERPR_REFERENCE_PRODUCTI foreign key (ProductId)
      references ProductInfo (ProductId)
go



