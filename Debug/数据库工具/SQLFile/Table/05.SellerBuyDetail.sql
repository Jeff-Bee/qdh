/*
功能：		创建【卖家进货单明细】表
创建日期：	2016-11-01 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerBuyDetail') and o.name = 'FK_SELLERBU_REFERENCE_SELLERBU')
alter table SellerBuyDetail
   drop constraint FK_SELLERBU_REFERENCE_SELLERBU
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerBuyDetail') and o.name = 'FK_SELLERBU_REFERENCE_PRODUCTI')
alter table SellerBuyDetail
   drop constraint FK_SELLERBU_REFERENCE_PRODUCTI
go

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerBuyDetail]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerBuyDetail]

CREATE TABLE [dbo].[SellerBuyDetail] (
	[BuyId]					int						NOT NULL, 						/*进货单编号（SellerBuyInfo:BuyId）*/
	[SellerId] 				int						NOT NULL, 						/*卖家用户编号(SellerInfo:SellerId)*/
	[Index]					smallint				not null,						/*序号*/
	ProductId				int						not null,						/*商品编号(ProductInfo:ProductId)*/
	Quantity				int						not null,						/*数量*/
	Price					money					not null DEFAULT((0)),			/*价格*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
	constraint PK_SELLERBUYDETAIL primary key (BuyId, [Index])
) 



alter table SellerBuyDetail
   add constraint FK_SELLERBU_REFERENCE_SELLERBU foreign key (BuyId)
      references SellerBuyInfo (BuyId)
go

alter table SellerBuyDetail
   add constraint FK_SELLERBU_REFERENCE_PRODUCTI foreign key (ProductId)
      references ProductInfo (ProductId)
go
