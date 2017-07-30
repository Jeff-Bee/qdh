/*
功能：		创建【商品分类表】表
创建日期：	2016-10-24 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('ProductClassInfo') and o.name = 'FK_PRODUCTC_REFERENCE_SELLERIN')
alter table ProductClassInfo
   drop constraint FK_PRODUCTC_REFERENCE_SELLERIN


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductClassInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[ProductClassInfo]



CREATE TABLE [dbo].[ProductClassInfo] (
	[ClassId]				int IDENTITY(10000,1)	NOT NULL PRIMARY KEY, 	/*商品分类表：自增，10000--2147483 647	0--1000000为系统保留*/
	[SellerId] 				int						NOT NULL, 				/*卖家用户编号*/
	[Name]					nvarchar(50)			NOT NULL,
	[PinyinCode]			varchar(20)				NOT NULL,
	[ParentId]				int						NOT NULL DEFAULT ((0)),
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL,						/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];


alter table ProductClassInfo
   add constraint FK_PRODUCTC_REFERENCE_SELLERIN foreign key (SellerId)
      references SellerInfo (SellerId)

