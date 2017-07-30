/*
功能：		创建【卖家商品上线销售用户策略】表
创建日期：	2016-11-07 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerProductOnlineCustomerStrategy') and o.name = 'FK_SELLERPRODUCTONLINECUSTOMERSTRATEGY_REFERENCE_SELLERCUSTOMERINFO')
alter table SellerProductOnlineCustomerStrategy
   drop constraint FK_SELLERPRODUCTONLINECUSTOMERSTRATEGY_REFERENCE_SELLERCUSTOMERINFO
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerProductOnlineCustomerStrategy') and o.name = 'FK_SELLERPRODUCTONLINECUSTOMERSTRATEGY_REFERENCE_PRODUCTI')
alter table SellerProductOnlineCustomerStrategy
   drop constraint FK_SELLERPRODUCTONLINECUSTOMERSTRATEGY_REFERENCE_PRODUCTI
go


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerProductOnlineCustomerStrategy]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerProductOnlineCustomerStrategy]

CREATE TABLE [dbo].[SellerProductOnlineCustomerStrategy] (
	[ProductId]				int						NOT NULL,						/*商品编号(ProductInfo:ProductId)*/
	[SellerId] 				int						NOT NULL, 						/*卖家编号(SellerInfo:SellerId)*/
	[BuyerId]			int						not null,						/*卖家客户编号(SellerCustomerInfo:BuyerId)*/
	[SaleState]				int						not null DEFAULT((0)),			/*销售状态*/
	[SaleStartDate]			datetime				not null DEFAULT (getdate()),	/*销售开始日期*/
	[SaleEndDate]			datetime				not null DEFAULT ('2100-01-01'),/*销售结束日期*/
	[SaleStoreQuantity]		int						not null,						/*销售库存量*/
	[Price1]				money					not null,						/*销售价格1*/
	[MinOrder1]				smallint				not null DEFAULT((1)),			/*最小订购量*/
	[MaxOrder1]				int						not null DEFAULT((99999)),		/*最大订购量*/

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
constraint PK_SELLERPRODUCTONLINECUSTOMER primary key (ProductId, BuyerId)
)
alter table SellerProductOnlineCustomerStrategy
   add constraint FK_SELLERPRODUCTONLINECUSTOMERSTRATEGY_REFERENCE_PRODUCTI foreign key (ProductId)
      references ProductInfo (ProductId)
go

alter table SellerProductOnlineCustomerStrategy
   add constraint FK_SELLERPRODUCTONLINECUSTOMERSTRATEGY_REFERENCE_SELLERCUSTOMERINFO foreign key (BuyerId)
      references SellerCustomerInfo (BuyerId)
go




