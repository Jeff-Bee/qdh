/*
功能：		创建【卖家订单明细】表
创建日期：	2016-11-07 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SalersOrderDetail') and o.name = 'FK_SalersOrderDetail_REFERENCE_SellerOrderRun')
alter table SalersOrderDetail
   drop constraint FK_SalersOrderDetail_REFERENCE_SellerOrderRun
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SalersOrderDetail') and o.name = 'FK_SalersOrderDetail_REFERENCE_ProductInfo')
alter table SalersOrderDetail
   drop constraint FK_SalersOrderDetail_REFERENCE_ProductInfo
go



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalersOrderDetail]') AND type in (N'U'))
	DROP TABLE [dbo].[SalersOrderDetail]

CREATE TABLE [dbo].[SalersOrderDetail] (
	[OrderId]				int						NOT NULL, 						/*订单编号(SellerOrderRun:OrderId)*/
	[Index]					smallint				NOT NULL DEFAULT (0),			/*明细序号*/
	[SellerId] 				int						NOT NULL, 						/*卖家编号(SellerInfo:SellerId)*/
	[BuyerId]			 	int						NOT NULL, 						/*买家编号(BuyerInfo:BuyerId)*/

	[ProductId]				int						NOT NULL,						/*商品编号(ProductInfo:ProductId)*/
	[ProductName]			nvarchar(50)			NOT NULL,						/*商品名称(ProductInfo:ProductFullName)*/
	[ProductQuantity]		int						NOT NULL,						/*商品数量*/
	[ProductUnit]			varchar(10)				NOT NULL,						/*商品单位(ProductInfo:ProductUnit)*/
	[ProductPrice]			money					NOT NULL,						/*商品单价*/
	[TotalPrice]			money					NOT NULL,						/*商品总价*/
	
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
constraint PK_SalersOrderDetail primary key (OrderId, [Index])
)

alter table SalersOrderDetail
   add constraint FK_SalersOrderDetail_REFERENCE_SellerOrderRun foreign key (OrderId)
      references SellerOrderRun (OrderId)
go

alter table SalersOrderDetail
   add constraint FK_SalersOrderDetail_REFERENCE_ProductInfo foreign key (ProductId)
      references ProductInfo (ProductId)
go
