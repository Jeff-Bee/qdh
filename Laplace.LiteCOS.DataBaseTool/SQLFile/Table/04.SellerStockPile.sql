/*
功能：		创建【卖家库存】表
创建日期：	2016-11-03 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerStockPile') and o.name = 'FK_SellerStockPile_REFERENCE_SellerInfo')
alter table SellerStockPile
   drop constraint FK_SellerStockPile_REFERENCE_SellerInfo
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerStockPile') and o.name = 'FK_SELLERST_REFERENCE_PRODUCTI')
alter table SellerStockPile
   drop constraint FK_SELLERST_REFERENCE_PRODUCTI
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerStockPile') and o.name = 'FK_SELLERST_REFERENCE_SELLERST')
alter table SellerStockPile
   drop constraint FK_SELLERST_REFERENCE_SELLERST
go

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerStockPile]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerStockPile]

CREATE TABLE [dbo].[SellerStockPile] (
	[StockId]				int IDENTITY(10001,1)	NOT NULL PRIMARY KEY, 			/*库存编号：自增，10001--2147483 647	0--10000为系统保留*/
	[SellerId] 				int						NOT NULL, 						/*卖家编号(SellerInfo:SellerId)*/
	[ProductId]				int						not null,						/*商品编号(ProductInfo:ProductId)*/
	[StoreHouseId]			int						not null  DEFAULT((0)),			/*所在仓库编号(SellerStoreHouseInfo:StoreHouseId)*/
	[FirstEnterDate]		datetime				not null,						/*首次入库时间*/
	[LastLeaveDate]			datetime				not null,						/*最后一次出库时间,为NULL 时,此种商品从来没有卖过*/
	[Quantity]				int						not null,						/*数量*/
	[UpperLimit]			int						not null DEFAULT((0)),			/*库存上限*/
	[LowerLitmit]			int						not null DEFAULT((0)),			/*库存下限*/
	[Cost]					money					not null DEFAULT((0)),			/*库存成本(保留设计)*/
	[Price]					money					not null DEFAULT((0)),			/*加权价(保留设计)*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];


alter table SellerStockPile
   add constraint FK_SellerStockPile_REFERENCE_SellerInfo foreign key (SellerId)
      references SellerInfo (SellerId)
go

alter table SellerStockPile
   add constraint FK_SELLERST_REFERENCE_PRODUCTI foreign key (ProductId)
      references ProductInfo (ProductId)
go

alter table SellerStockPile
   add constraint FK_SELLERST_REFERENCE_SELLERST foreign key (StoreHouseId)
      references SellerStoreHouseInfo (StoreHouseId)
go
--创建索引
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20161103-225906] ON [dbo].[SellerStockPile]
(
	[SellerId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

