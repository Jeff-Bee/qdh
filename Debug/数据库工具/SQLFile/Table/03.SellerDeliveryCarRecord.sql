/*
功能：		创建【卖家送货车辆行车记录】表
创建日期：	2016-11-08 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerDeliveryCarRecord') and o.name = 'FK_SELLERDE_REFERENCE_SELLERDE')
alter table SellerDeliveryCarRecord
   drop constraint FK_SELLERDE_REFERENCE_SELLERDE
go


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerDeliveryCarRecord]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerDeliveryCarRecord]

CREATE TABLE [dbo].[SellerDeliveryCarRecord] (
	[CarId]					int						NOT NULL, 						/*车辆编号(SellerDeliveryCarInfo:CarId)*/
	[StartTime]	            datetime				NOT NULL DEFAULT (getdate()),	/*记录开始时间*/
	[EndTime]	            datetime				NOT NULL DEFAULT (getdate()),	/*记录结束时间*/
	[Longitude] 			REAL					NOT NULL DEFAULT (0), 			/*经度*/
	[Latitude]			 	REAL					NOT NULL DEFAULT (0), 			/*纬度*/
	[Location]				nvarchar(100)			NOT NULL DEFAULT (''), 			/*地理位置*/
	--[LocationState]			tinyint					NOT NULL DEFAULT ((0)), 		/*位置信息状态*/
	[Notes] 				nvarchar(100)			NOT NULL DEFAULT (''),			/*备注*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
   constraint PK_SELLERDELIVERYCARRECORD primary key (CarId, StartTime)
)
alter table SellerDeliveryCarRecord
   add constraint FK_SELLERDE_REFERENCE_SELLERDE foreign key (CarId)
      references SellerDeliveryCarInfo (CarId)
go



