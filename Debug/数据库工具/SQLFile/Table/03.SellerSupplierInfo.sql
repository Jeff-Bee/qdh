/*
功能：		创建【卖家供应商信息】表
创建日期：	2016-11-01 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerSupplierInfo') and o.name = 'FK_SUPPLIER_REFERENCE_AREAINFO')
alter table SellerSupplierInfo
   drop constraint FK_SUPPLIER_REFERENCE_AREAINFO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerSupplierInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerSupplierInfo]

CREATE TABLE [dbo].[SellerSupplierInfo] (
	[SupplierId]			int IDENTITY(10000,1)	NOT NULL PRIMARY KEY, 			/*供应商编号：自增，10000--2147483 647	0--1000000为系统保留*/
	[SellerId] 				int						NOT NULL, 						/*卖家用户编号*/
	[Code]					varchar(10)				not null,						/*供应商代码*/
	[FullName]				nvarchar(20)			NOT NULL,						/*单位全名*/
	[ShortName]				nvarchar(10)			NOT NULL,						/*单位简名*/
	[PinyinCode]			varchar(10)				NOT NULL,						/*拼音码*/
	[AreaId]				int						NOT NULL DEFAULT((0)),			/*所属地区编号*/
	[TaxNumber]				varchar(20)				NOT NULL DEFAULT (''),			/*税号*/
	ConstactPerson			nvarchar(10)			NOT NULL DEFAULT (''),			/*联系人*/
	Phone					varchar(20)				NOT NULL DEFAULT (''),			/*电话*/
	MobilePhone				varchar(20)				NOT NULL DEFAULT (''),			/*手机*/
	Email					varchar(20)				NOT NULL DEFAULT (''),			/*电子邮件*/
	[Address]				nvarchar(250)			NOT NULL DEFAULT (''),			/*地址*/
	Bank					nvarchar(20)			NOT NULL DEFAULT (''),			/*开户银行*/
	BankAccount				varchar(20)				NOT NULL DEFAULT (''),			/*银行账户*/
	ExchangeDay				smallint				NOT NULL DEFAULT (0),			/*换货天数*/
	ExchangePercent			smallint				NOT NULL DEFAULT (100),			/*换货比例*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];


alter table SellerSupplierInfo
   add constraint FK_SUPPLIER_REFERENCE_AREAINFO foreign key (AreaId)
      references SellerAreaInfo (AreaId)
