/*
���ܣ�		������ϵͳ��Ӫ�������á���
�������ڣ�	2015-11-18 by ����
*/


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemConfigInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SystemConfigInfo]
GO

CREATE TABLE [dbo].[SystemConfigInfo](
	[Business]		[varchar](50)	NOT NULL,					--ģ�����
	[ConfigKey]		[varchar](50)	NOT NULL,					--���ü�ֵ
	[ConfigKey2]	[varchar](50)	NOT NULL DEFAULT (('')),
	[ConfigKey3]	[varchar](50)	NOT NULL DEFAULT (('')),
	[Value]			[varchar](500)	NOT NULL,
	[Name]			[nvarchar](50)	NOT NULL DEFAULT (('')),

	/*********************************ͨ������*********************************/
	[Config]				varchar(1000),											/*����*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*��ע*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*������*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*��������*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*�޸���*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*�޸�����*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*״ֵ̬*/
 CONSTRAINT [PK_SystemConfigInfo] PRIMARY KEY CLUSTERED 
(
	[Business] ASC,
	[ConfigKey] ASC,
	[ConfigKey2] ASC,
	[ConfigKey3] ASC
)  
) ON [PRIMARY]
GO



