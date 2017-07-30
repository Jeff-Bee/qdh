/*
功能：		创建【卖家进货单】表
创建日期：	2016-11-01 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerBuyInfo') and o.name = 'FK_SELLERBU_REFERENCE_SELLERIN')
alter table SellerBuyInfo
   drop constraint FK_SELLERBU_REFERENCE_SELLERIN
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerBuyInfo') and o.name = 'FK_SELLERBU_REFERENCE_SELLERST')
alter table SellerBuyInfo
   drop constraint FK_SELLERBU_REFERENCE_SELLERST
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerBuyInfo') and o.name = 'FK_SELLERBU_REFERENCE_SELLEREM')
alter table SellerBuyInfo
   drop constraint FK_SELLERBU_REFERENCE_SELLEREM
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerBuyInfo') and o.name = 'FK_SELLERBU_REFERENCE_SELLERSU')
alter table SellerBuyInfo
   drop constraint FK_SELLERBU_REFERENCE_SELLERSU
go


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerBuyInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerBuyInfo]

CREATE TABLE [dbo].[SellerBuyInfo] (
	[BuyId]					int IDENTITY(10000,1)	NOT NULL PRIMARY KEY, 			/*进货单编号：自增，10000--2147483 647	0--1000000为系统保留*/
	[SellerId] 				int						NOT NULL, 						/*卖家用户编号*/
	[Code]					varchar(20)				not null,						/*进货单代码*/
	ComeDate				datetime				not null,						/*录单日期*/
	EmployeeId				int						not null,						/*经手人编号（SellerEmployeeInfo:EmployeeId）*/
	DeptId					int						not null DEFAULT((0)),			/*部门编号(SellerDeptInfo:DeptId)*/
	SupplierId				int						not null,						/*供货单位编号(SellerSupplierInfo:SupplierId)*/
	StoreHouseId			int						not null DEFAULT((0)),			/*接收仓库编号(SellerStoreHouseInfo:StoreHouseId)*/
	Remark					nvarchar(250)			not null DEFAULT (''),			/*摘要*/
	PaymentAccount			int						not null DEFAULT((0)),			/*付款账户(??)*/
	TotalAmount				money					not null DEFAULT((0)),			/*合计金额*/
	DiscountAmount			money					not null DEFAULT((0)),			/*优惠金额*/
	ActualAmount			money					not null DEFAULT((0)),			/*优惠后金额*/
	PaidAmount				money					not null DEFAULT((0)),			/*已付款金额*/
	ChargeAmount			money					not null DEFAULT((0)),			/*代付款金额*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];



alter table SellerBuyInfo
   add constraint FK_SELLERBU_REFERENCE_SELLERIN foreign key (SellerId)
      references SellerInfo (SellerId)
go

alter table SellerBuyInfo
   add constraint FK_SELLERBU_REFERENCE_SELLERST foreign key (StoreHouseId)
      references SellerStoreHouseInfo (StoreHouseId)
go

alter table SellerBuyInfo
   add constraint FK_SELLERBU_REFERENCE_SELLEREM foreign key (EmployeeId)
      references SellerEmployeeInfo (EmployeeId)
go

alter table SellerBuyInfo
   add constraint FK_SELLERBU_REFERENCE_SELLERSU foreign key (SupplierId)
      references SellerSupplierInfo (SupplierId)
go
