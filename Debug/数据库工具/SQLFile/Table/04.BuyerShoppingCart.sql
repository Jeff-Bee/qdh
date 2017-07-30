/*
功能：		创建【买家购物车信息】表
创建日期：	2016-12-05 by 龚俊
更新：		
*/



if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BuyerShoppingCart') and o.name = 'FK_BUYERSHO_REFERENCE_BUYERINF')
alter table BuyerShoppingCart
   drop constraint FK_BUYERSHO_REFERENCE_BUYERINF
go

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BuyerShoppingCart]') AND type in (N'U'))
	DROP TABLE [dbo].[BuyerShoppingCart]



/*==============================================================*/
/* Table: BuyerShoppingCart                                     */
/*==============================================================*/
CREATE TABLE [dbo].[BuyerShoppingCart] (
	[BuyerId]				int						NOT NULL,						/*买家编号(BuyerInfo:BuyerId)*/
	[SellerId] 				int						NOT NULL, 						/*卖家编号(SellerInfo:SellerId)*/
	[ProductId]				int						NOT NULL,						/*商品编号(ProductInfo:ProductId)*/
	[ProductQuantity]		int						NOT NULL DEFAULT ((0)),
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
   constraint PK_BUYERSHOPPINGCART primary key (BuyerId, ProductId)
)

alter table BuyerShoppingCart
   add constraint FK_BUYERSHO_REFERENCE_BUYERINF foreign key (BuyerId)
      references BuyerInfo (BuyerId)
go
