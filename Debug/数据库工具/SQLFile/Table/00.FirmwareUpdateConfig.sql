/*
功能：		创建【固件升级配置】表
创建日期：	2016-05-17 by 龚俊
*/


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FirmwareUpdateConfig]') AND type in (N'U'))
	DROP TABLE [dbo].[FirmwareUpdateConfig]
GO


CREATE TABLE [dbo].[FirmwareUpdateConfig] (
	[Id] 					int						NOT NULL DEFAULT((0)) PRIMARY KEY, 	/*固件Id*/
	[Version]				int	 					NOT NULL , 							/*版本号*/
	[Data] 					varbinary(max)			NOT NULL, 							/*升级文件*/
	[Model]					int	 					NOT NULL DEFAULT((0)), 				/*升级方式*/
	[IsUsed] 				bit 					NOT NULL DEFAULT ((1)), 			/*启用状态*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];



