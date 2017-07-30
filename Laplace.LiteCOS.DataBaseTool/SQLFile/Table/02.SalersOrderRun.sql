﻿/*
功能：		创建【卖家线下销售单记录】表
创建日期：	2017-01-17 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SalersOrderRun') and o.name = 'FK_SalersOrderRun_REFERENCE_SellerInfo')
alter table SalersOrderRun
   drop constraint FK_SalersOrderRun_REFERENCE_SellerInfo
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SalersOrderRun') and o.name = 'FK_SalersOrderRun_REFERENCE_BuyerInfo')
alter table SalersOrderRun
   drop constraint FK_SalersOrderRun_REFERENCE_BuyerInfo
go



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalersOrderRun]') AND type in (N'U'))
	DROP TABLE [dbo].[SalersOrderRun]

CREATE TABLE [dbo].[SalersOrderRun] (
	[OrderId]				int	IDENTITY(10001,1)	NOT NULL PRIMARY KEY, 			/*订单编号：自增，10001--2147483 647	0--10000为系统保留*/
	[Code]	                varchar(20)				not null,						/*订单代码：格式：XS-20161107-XXXXX*/
	[SellerId] 				int						NOT NULL, 						/*卖家编号(SellerInfo:SellerId)*/
	[BuyerId]			 	int						NOT NULL, 						/*买家编号(BuyerInfo:BuyerId)*/
	[OrderDate]				datetime				not null DEFAULT (getdate()),	/*下单日期*/
	[ProductType]	        smallint				not null DEFAULT (1),			/*商品类型*/
	[ProductQuantity]	    int						not null DEFAULT (1),			/*商品总数*/
	[Amount]				money					not null,						/*订单总金额*/
	[OrderState]			smallint				not null DEFAULT (0),			/*订单状态*/
	[PayState]				smallint				not null DEFAULT (11),			/*支付状态*/
	[PayAmount]             money                   not null,						/*付款金额*/
	[LogisticsState]		smallint				not null DEFAULT (1),			/*物流状态（出库发货状态）*/
	[Summary]				nvarchar(50)			NOT NULL,						/*摘要*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];


alter table SalersOrderRun
   add constraint FK_SalersOrderRun_REFERENCE_SellerInfo foreign key (SellerId)
      references SellerInfo (SellerId)
go

alter table SalersOrderRun
   add constraint FK_SalersOrderRun_REFERENCE_BuyerInfo foreign key (BuyerId)
      references BuyerInfo (BuyerId)
go


