/*
功能：		创建【卖家新品发布】表
创建日期：	2017-01-14 by 龚俊
更新：		
*/



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NewProductLaunch]') AND type in (N'U'))
	DROP TABLE [dbo].[NewProductLaunch]

CREATE TABLE [dbo].[NewProductLaunch] (
	[AdsId]					int IDENTITY(10000,1)	NOT NULL PRIMARY KEY, 			/*新品发布编号：自增，10001--2147483 647	0--10000为系统保留*/
	[SellerId] 				int						NOT NULL, 						/*卖家用户编号*/
	[Title]					Nvarchar(20)			NOT NULL DEFAULT(''),			/*新品发布标题*/
	[Content]				Nvarchar(500)			NOT NULL DEFAULT(''),			/*新品发布内容*/
	[Picture]				int						NOT NULL DEFAULT(0),			/*商品编号，ProductInfo:ProductId*/
	[ProductId]				int						NOT NULL DEFAULT(0),			/*新品发布图片*/
	[Link]					Nvarchar(500)			NOT NULL DEFAULT(''),			/*新品发布链接*/
	[Receiver]				int						NOT NULL DEFAULT(0),			/*新品发布接收终端类型*/
	StartTime				datetime				NOT NULL DEFAULT (getdate()),	/*新品发布开始时间*/
	EndTime					datetime				NOT NULL DEFAULT ('2100-1-1'),	/*新品发布结束时间*/
	[IsUsed]				bit						NOT NULL DEFAULT (0),			/*新品发布是否有效*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];


