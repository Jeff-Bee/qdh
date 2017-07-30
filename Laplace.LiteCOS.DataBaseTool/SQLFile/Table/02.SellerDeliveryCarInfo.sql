/*
功能：		创建【卖家送货车辆信息】表
创建日期：	2016-11-08 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerDeliveryCarInfo') and o.name = 'FK_SELLERDELIVERYCARINFO_REFERENCE_SELLERIN')
alter table SellerDeliveryCarInfo
   drop constraint FK_SELLERDELIVERYCARINFO_REFERENCE_SELLERIN
go



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerDeliveryCarInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerDeliveryCarInfo]

CREATE TABLE [dbo].[SellerDeliveryCarInfo] (
	[CarId]					int	IDENTITY(10001,1)	NOT NULL PRIMARY KEY, 			/*车辆编号：自增，10001--2147483 647	0--10000为系统保留*/
	[Code]	                varchar(10)				not null,						/*车辆代码：6位*/
	[SellerId] 				int						NOT NULL, 						/*卖家编号(SellerInfo:SellerId)*/
	[CarNumber]			 	nvarchar(10)			NOT NULL, 						/*车牌号*/
	[Name]			 		nvarchar(10)			NOT NULL DEFAULT (''), 			/*车辆名称*/
	[Driver]			 	nvarchar(10)			NOT NULL DEFAULT (''), 			/*司机*/
	[MobilePhone]			varchar(20)				NOT NULL DEFAULT (''),			/*联系电话*/
	[IsUsed]				bit						NOT NULL DEFAULT (1),			/*是否启用*/

	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];


alter table SellerDeliveryCarInfo
   add constraint FK_SELLERDELIVERYCARINFO_REFERENCE_SELLERIN foreign key (SellerId)
      references SellerInfo (SellerId)
go



