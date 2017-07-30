/*
功能：		创建【卖家仓库信息】表
创建日期：	2016-11-01 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerStoreHouseInfo') and o.name = 'FK_SELLERST_REFERENCE_SELLERIN')
alter table SellerStoreHouseInfo
   drop constraint FK_SELLERST_REFERENCE_SELLERIN


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerStoreHouseInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerStoreHouseInfo]

CREATE TABLE [dbo].[SellerStoreHouseInfo] (
	[StoreHouseId]			int IDENTITY(10000,1)	NOT NULL PRIMARY KEY, 			/*仓库编号：自增，10000--2147483 647	0--1000000为系统保留*/
	[SellerId] 				int						NOT NULL, 						/*卖家用户编号*/
	[Code]					varchar(10)				NOT NULL,						/*仓库代码*/
	[Name]					nvarchar(20)			NOT NULL,						/*仓库名称*/
	[Address]				nvarchar(250)			NOT NULL DEFAULT (''),			/*地址*/
	[Phone]					varchar(20)				NOT NULL DEFAULT (''),			/*电话*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];


alter table SellerStoreHouseInfo
   add constraint FK_SELLERST_REFERENCE_SELLERIN foreign key (SellerId)
      references SellerInfo (SellerId)


