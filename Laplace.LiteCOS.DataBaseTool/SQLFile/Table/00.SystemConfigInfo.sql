/*
功能：		创建【系统运营参数配置】表
创建日期：	2015-11-18 by 龚俊
*/


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemConfigInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SystemConfigInfo]
GO

CREATE TABLE [dbo].[SystemConfigInfo](
	[Business]		[varchar](50)	NOT NULL,					--模块代码
	[ConfigKey]		[varchar](50)	NOT NULL,					--配置键值
	[ConfigKey2]	[varchar](50)	NOT NULL DEFAULT (('')),
	[ConfigKey3]	[varchar](50)	NOT NULL DEFAULT (('')),
	[Value]			[varchar](500)	NOT NULL,
	[Name]			[nvarchar](50)	NOT NULL DEFAULT (('')),

	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
 CONSTRAINT [PK_SystemConfigInfo] PRIMARY KEY CLUSTERED 
(
	[Business] ASC,
	[ConfigKey] ASC,
	[ConfigKey2] ASC,
	[ConfigKey3] ASC
)  
) ON [PRIMARY]
GO



