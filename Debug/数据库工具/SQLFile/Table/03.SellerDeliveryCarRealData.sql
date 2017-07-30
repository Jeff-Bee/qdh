/*
功能：		创建【卖家送货车辆行车实时状态】表
创建日期：	2016-11-08 by 龚俊
更新：		
*/
if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerDeliveryCarRealData') and o.name = 'FK_SELLERDELIVERYCARREALDATA_REFERENCE_SELLERDE')
alter table SellerDeliveryCarRealData
   drop constraint FK_SELLERDELIVERYCARREALDATA_REFERENCE_SELLERDE
go


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerDeliveryCarRealData]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerDeliveryCarRealData]

CREATE TABLE [dbo].[SellerDeliveryCarRealData] (
	[CarId]					int						NOT NULL PRIMARY KEY, 			/*车辆编号(SellerDeliveryCarInfo:CarId)*/
	[ReceiptTime]	        datetime				NOT NULL DEFAULT (getdate()),	/*接收时间*/
	[UpdateTime]	        datetime				NOT NULL DEFAULT (getdate()),	/*最后更新时间*/
	[Longitude] 			REAL					NOT NULL DEFAULT (0), 			/*经度*/
	[Latitude]			 	REAL					NOT NULL DEFAULT (0), 			/*纬度*/
	[Location]				nvarchar(100)			NOT NULL DEFAULT (''), 			/*地理位置*/
	--[LocationState]			tinyint					NOT NULL DEFAULT ((0)), 		/*位置信息状态*/
	[Version]				int						NOT NULL DEFAULT (''),			/*固件版本*/
	[Online]				tinyint					NOT NULL DEFAULT (0),			/*在线状态*/
	[Notes] 				nvarchar(100)			NOT NULL DEFAULT (''),			/*备注*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];
alter table SellerDeliveryCarRealData
   add constraint FK_SELLERDELIVERYCARREALDATA_REFERENCE_SELLERDE foreign key (CarId)
      references SellerDeliveryCarInfo (CarId)
go


