/*
功能：		创建【商品信息】表
创建日期：	2016-10-24 by 龚俊
更新：		
*/
--删除外键
if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('ProductInfo') and o.name = 'FK_PRODUCTI_REFERENCE_SELLERIN')
alter table ProductInfo
   drop constraint FK_PRODUCTI_REFERENCE_SELLERIN

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('ProductInfo') and o.name = 'FK_PRODUCTI_REFERENCE_PRODUCTC')
alter table ProductInfo
   drop constraint FK_PRODUCTI_REFERENCE_PRODUCTC

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[ProductInfo]


CREATE TABLE [dbo].[ProductInfo] (
	[ProductId]				int IDENTITY(10000,1)	NOT NULL PRIMARY KEY, 	/*商品编号：自增，10000--2147483 647	0--1000000为系统保留*/
	[SellerId] 				int						NOT NULL, 				/*卖家用户编号*/
	ClassId					int						NOT NULL,
	ProductCode				varchar(10)				NOT NULL,
	ProductFullName			nvarchar(50)			NOT NULL,
	ProductShortName		nvarchar(10)			NOT NULL,
	PinyinCode				varchar(20)				NOT NULL,
	Package					nvarchar(50)			NOT NULL,
	ProductSpec				nvarchar(50)			NOT NULL,
	ProductModel			nvarchar(50)			NOT NULL,
	ProductUnit				varchar(10)				NOT NULL,
	[Weight]				real					NOT NULL default 0,
	BarCode					varchar(50)				NOT NULL,
	Place					nvarchar(50)			NOT NULL,
	ProductState			smallint				NOT NULL,
	Price1					money					NOT NULL,
	Price2					money					NOT NULL,
	Price3					money					NOT NULL,
	Price4					money					NOT NULL,
	Price5					money					NOT NULL,
	Price6					money					NOT NULL,
	Price7					money					NOT NULL,
	Price8					money					NOT NULL,
	Picture1				int						NOT NULL DEFAULT ((0)),
	Picture2				int						NOT NULL DEFAULT ((0)),
	Picture3				int						NOT NULL DEFAULT ((0)),
	Picture4				int						NOT NULL DEFAULT ((0)),
	Picture5				int						NOT NULL DEFAULT ((0)),
	Picture6				int						NOT NULL DEFAULT ((0)),
	Summary					nvarchar(100)			NOT NULL,
	Supply					nvarchar(100)			NOT NULL,
	CreateType				smallint				NOT NULL,
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL,						/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];

--添加外键
alter table ProductInfo
   add constraint FK_PRODUCTI_REFERENCE_SELLERIN foreign key (SellerId)
      references SellerInfo (SellerId)

alter table ProductInfo
   add constraint FK_PRODUCTI_REFERENCE_PRODUCTC foreign key (ClassId)
      references ProductClassInfo (ClassId)
