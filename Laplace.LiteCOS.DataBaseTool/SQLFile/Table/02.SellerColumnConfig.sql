/*
功能：		创建【卖家表格列展示配置信息】表
创建日期：	2016-10-30 by 龚俊
更新：		
*/


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerColumnConfig]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerColumnConfig]



CREATE TABLE [dbo].[SellerColumnConfig] (
	[SellerId] 				int						NOT NULL, 						/*卖家用户编号，为0时是系统默认配置*/
	TableName				varchar(50)				not null,						/*表名*/
	FieldName				varchar(50)				not null,						/*字段名*/
	DefaultName				nvarchar(50)			not null,						/*列（默认）名称*/
	DisplayName				nvarchar(50)			not null,						/*（用户自定义）显示名称*/
	Visible					bit						not null	DEFAULT ((1)),		/*是否可见*/
	[Index]					int						not null	DEFAULT ((0)),		/*列序号*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
	constraint PK_SELLERCOLUMNCONFIG primary key (SellerId, TableName, FieldName)
)

