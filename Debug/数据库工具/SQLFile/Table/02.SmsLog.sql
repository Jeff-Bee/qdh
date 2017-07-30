/*
功能：		创建【手机发送短信日志】表
创建日期：	2015-11-12 by 龚俊
*/


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SmsLog]') AND type in (N'U'))
	DROP TABLE [dbo].[SmsLog]


CREATE TABLE [dbo].[SmsLog] (
	[MobilePhone]			varchar(20)	 			NOT NULL, 						/*手机号*/
	[SmsTime]				Datetime				NOT NULL DEFAULT (getdate()), 	/*发送短信时间*/
	[UserId]				int						NOT NULL DEFAULT (0),			/*用户编号*/
	[LogType] 				smallint				NOT NULL, 						/*日志类型*/
	[SmsContent]			nvarchar(500)			NOT NULL, 						/*短信内容*/

	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
 CONSTRAINT [PK_SmsLog] PRIMARY KEY CLUSTERED 
(
	[MobilePhone] ASC,
	[SmsTime] ASC
)  
) ON [PRIMARY]

