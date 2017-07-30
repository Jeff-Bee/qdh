/*
功能：		创建【图片资源】表
创建日期：	2016-10-24 by 龚俊
更新：		
*/
--删除外键
if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('PictureInfo') and o.name = 'FK_PICTUREI_REFERENCE_SELLERIN')
alter table PictureInfo
   drop constraint FK_PICTUREI_REFERENCE_SELLERIN

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PictureInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[PictureInfo]



CREATE TABLE [dbo].[PictureInfo] (
	[PicId]					int IDENTITY(10000,1)	NOT NULL PRIMARY KEY, 	/*图片资源编号：自增，10000--2147483 647	0--10000为系统保留*/
	[SellerId] 				int						NOT NULL, 				/*卖家用户编号*/
	[Name]					nvarchar(50)			NOT NULL,
	[Resource]				varbinary(max)			NOT NULL,
	[Size]					int						NOT NULL,
	[Format]				varchar(10)				NOT NULL,
	[Width]					smallint				NOT NULL,
	[Height]				smallint				NOT NULL,
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL,						/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];

alter table PictureInfo
   add constraint FK_PICTUREI_REFERENCE_SELLERIN foreign key (SellerId)
      references SellerInfo (SellerId)
