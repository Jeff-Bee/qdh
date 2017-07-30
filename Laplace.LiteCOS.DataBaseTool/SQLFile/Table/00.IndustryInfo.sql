/*
功能：		创建【行业信息】表
创建日期：	2016-10-18 by 龚俊
更新：		
*/


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IndustryInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[IndustryInfo]
GO


CREATE TABLE [dbo].[IndustryInfo] (
	[IndustryId] 			int IDENTITY(1000,1)	NOT NULL PRIMARY KEY,			/*行业编号*/
	[Name]					nvarchar(50)			not null,						/*行业名称*/
	[ParentId]				int						NOT NULL DEFAULT ((0)),			/*上级行业编号*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250),											/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];


