/*
功能：		创建【买家（终端）用户信息】表
创建日期：	2016-10-17 by 龚俊
更新：		
*/

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BuyerInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[BuyerInfo]
GO


CREATE TABLE [dbo].[BuyerInfo] (
	[BuyerId] 				int IDENTITY(10001,1)	NOT NULL PRIMARY KEY, 	/*用户编号：自增，10001--2147483 647	0--10000为系统保留*/
	CompanyName				nvarchar(50)			not null,						
	[Address]				nvarchar(50)			not null,
	[ZipCode]				varchar(10)				not null,
	[ContactName]			nvarchar(10)			not null,
	[LoginName]				nvarchar(20)			not null,
	[Password]				varchar(20)				not null,
	[MobilePhone]			varchar(20)				not null,
	[EMail]					varchar(20) 			not null,						/*邮箱*/
	ValidityPeriod			datetime				not null,
	[WechatId]				varchar(20)				not null DEFAULT (''),
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



