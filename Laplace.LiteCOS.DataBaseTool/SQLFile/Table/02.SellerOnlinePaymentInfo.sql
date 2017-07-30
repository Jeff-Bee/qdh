/*
功能：		创建【卖家在线支付信息信息】表
创建日期：	2017-01-11 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerOnlinePaymentInfo') and o.name = 'FK_SellerOnlinePaymentInfo_REFERENCE_SellerInfo')
alter table SellerOnlinePaymentInfo
   drop constraint FK_SellerOnlinePaymentInfo_REFERENCE_SellerInfo


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerOnlinePaymentInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerOnlinePaymentInfo]

CREATE TABLE [dbo].[SellerOnlinePaymentInfo] (
	[SellerId] 				int						NOT NULL, 						/*卖家用户编号*/
	[PayType] 				tinyint					NOT NULL DEFAULT (0), 			/*支付类型:1：微信;2：支付宝	*/
	[AppId]					varchar(50)				NOT NULL DEFAULT (''),			/*(微信)应用Id*/
	[AppSecret]				varchar(50)				NOT NULL DEFAULT (''),			/*(微信)应用密钥*/
	[MchId]					varchar(50)				NOT NULL DEFAULT (''),			/*(微信)商户号/支付宝企业账号*/
	[PartnerId]				varchar(50)				NOT NULL DEFAULT (''),			/*(支付宝)合作者身份*/
	[AppKey]				varchar(50)				NOT NULL DEFAULT (''),			/*(微信)API密钥/支付宝安全校验码*/
	[IsUsed]				bit						NOT NULL DEFAULT (0),			/*启用状态*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
CONSTRAINT [PK_SellerOnlinePaymentInfo] PRIMARY KEY CLUSTERED 
(
	[SellerId] ASC,
	[PayType] ASC
)  
) ON [PRIMARY]


alter table SellerOnlinePaymentInfo
   add constraint FK_SellerOnlinePaymentInfo_REFERENCE_SellerInfo foreign key (SellerId)
      references SellerInfo (SellerId)


