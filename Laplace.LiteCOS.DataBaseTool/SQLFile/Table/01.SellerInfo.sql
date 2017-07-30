/*
功能：		创建【 卖家用户信息】表
创建日期：	2016-10-17 by 龚俊
更新：		
*/
--删除外键
if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerInfo') and o.name = 'FK_SELLERIN_REFERENCE_INDUSTRY')
alter table SellerInfo
   drop constraint FK_SELLERIN_REFERENCE_INDUSTRY

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerInfo]


CREATE TABLE [dbo].[SellerInfo] (
	[SellerId] 				int IDENTITY(10001,1)	NOT NULL PRIMARY KEY, 	/*用户编号：自增，1000001--2147483 647	0--1000000为系统保留*/
	[CompanyName]			nvarchar(50)			NOT null,
	[IndustryId]			int						NOT NULL DEFAULT (1001),
	[Address]				nvarchar(50)			NOT NULL DEFAULT (''),
	[ZipCode]				varchar(10)				NOT NULL DEFAULT (''),
	[ContactName]			nvarchar(10)			NOT NULL,
	[LoginName]				nvarchar(20)			NOT NULL DEFAULT (''),	
	[Password]				varchar(20)				NOT NULL,
	[MobilePhone]			varchar(20)				not null,
	[EMail]					varchar(20)				NOT NULL DEFAULT (''),		/*邮箱*/
	ValidityPeriod			datetime				not null,
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];

--添加外键
alter table SellerInfo
   add constraint FK_SELLERIN_REFERENCE_INDUSTRY foreign key (IndustryId)
      references IndustryInfo (IndustryId)

