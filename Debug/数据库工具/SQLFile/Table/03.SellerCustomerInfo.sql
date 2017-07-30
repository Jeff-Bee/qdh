/*
功能：		创建【卖家客户信息】表
创建日期：	2016-11-01 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerCustomerInfo') and o.name = 'FK_SELLERCU_REFERENCE_SELLERIN')
alter table SellerCustomerInfo
   drop constraint FK_SELLERCU_REFERENCE_SELLERIN
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerCustomerInfo') and o.name = 'FK_SELLERCU_REFERENCE_BUYERINF')
alter table SellerCustomerInfo
   drop constraint FK_SELLERCU_REFERENCE_BUYERINF
go


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerCustomerInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerCustomerInfo]

CREATE TABLE [dbo].[SellerCustomerInfo] (
	[BuyerId]				int						NOT NULL PRIMARY KEY, 			/*买家（客户编号）BuyerInfo:BuyerId*/
	[SellerId] 				int						NOT NULL, 						/*卖家用户编号*/
	[Code]					varchar(10)				not null,						/*客户代码*/
	[FullName]				nvarchar(20)			NOT NULL,						/*客户全名*/
	[ShortName]				nvarchar(10)			NOT NULL,						/*客户简名*/
	[PinyinCode]			varchar(10)				NOT NULL,						/*拼音码*/
	[AreaId]				int						NOT NULL DEFAULT((0)),			/*所属地区编号，SellerAreaInfo:AreaId*/
	[PriceLevel]			tinyint					not null DEFAULT((0)),			/*价格等级*/
	[TaxNumber]				varchar(20)				NOT NULL DEFAULT (''),			/*税号*/
	ConstactPerson			nvarchar(10)			NOT NULL DEFAULT (''),			/*联系人*/
	Phone					varchar(20)				NOT NULL DEFAULT (''),			/*电话*/
	MobilePhone				varchar(20)				NOT NULL DEFAULT (''),			/*手机*/
	Email					varchar(20)				NOT NULL DEFAULT (''),			/*电子邮件*/
	[Address]				nvarchar(250)			NOT NULL DEFAULT (''),			/*地址*/

	[Bank]					nvarchar(20)			null,							/*开户银行*/
	[BankAccount]			varchar(20)				null,							/*银行账户*/
	Wechat					nvarchar(100)			NOT NULL DEFAULT (''),			/*微信账号*/
	[BuyerType]				tinyint					not null DEFAULT (0),			/*买家类型 0：线上买家；1：线下买家；2：线上线下买家*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];

alter table SellerCustomerInfo
   add constraint FK_SELLERCU_REFERENCE_SELLERIN foreign key (SellerId)
      references SellerInfo (SellerId)
go

alter table SellerCustomerInfo
   add constraint FK_SELLERCU_REFERENCE_BUYERINF foreign key (BuyerId)
      references BuyerInfo (BuyerId)
go

