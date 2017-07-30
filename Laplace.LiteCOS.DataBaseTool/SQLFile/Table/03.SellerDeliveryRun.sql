/*
功能：		创建【卖家订单送货物流记录】表
创建日期：	2016-11-08 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerDeliveryRun') and o.name = 'FK_SELLERDELIVERYCARINFO_REFERENCE_SELLERDE')
alter table SellerDeliveryRun
   drop constraint FK_SELLERDELIVERYCARINFO_REFERENCE_SELLERDE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerDeliveryRun') and o.name = 'FK_SELLERDE_REFERENCE_SELLEROR')
alter table SellerDeliveryRun
   drop constraint FK_SELLERDE_REFERENCE_SELLEROR
go


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerDeliveryRun]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerDeliveryRun]

CREATE TABLE [dbo].[SellerDeliveryRun] (
	[Id]					int	IDENTITY(10001,1)	NOT NULL PRIMARY KEY, 			/*物流编号：自增，10001--2147483 647	0--10000为系统保留*/
	[Code]	                varchar(10)				NOT NULL,						/*物流代码：格式：WL-0-20161107-XXXXX*/
	[CarId] 				int						NOT NULL, 						/*车辆编号(SellerDeliveryCarInfo:CarId)*/
	[SellerId] 				int						NOT NULL, 						/*卖家编号(SellerInfo:SellerId)*/
	[OrderId]				int						not null,						/*订单编号(SellerOrderRun:OrderId)*/
	[Driver]			 	nvarchar(10)			NOT NULL DEFAULT (''), 			/*运送司机*/
	[MobilePhone]			varchar(20)				NOT NULL DEFAULT (''),			/*联系电话*/
	
	[Source]				nvarchar(100)			null,							/*出发地*/
	[Destination]			nvarchar(100)			null,							/*目的地*/
	[StartTime]				datetime				null,							/*开始时间*/
	[EndTime]				datetime				null,							/*结束时间*/


	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];

alter table SellerDeliveryRun
   add constraint FK_SELLERDELIVERYCARINFO_REFERENCE_SELLERDE foreign key (CarId)
      references SellerDeliveryCarInfo (CarId)
go

alter table SellerDeliveryRun
   add constraint FK_SELLERDE_REFERENCE_SELLEROR foreign key (OrderId)
      references SellerOrderRun (OrderId)
go
