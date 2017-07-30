/*
功能：		创建【卖家员工信息】表
创建日期：	2016-10-27 by 龚俊
更新：		
*/

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerEmployeeInfo') and o.name = 'FK_SELLEREM_REFERENCE_SELLERIN')
alter table SellerEmployeeInfo
   drop constraint FK_SELLEREM_REFERENCE_SELLERIN

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SellerEmployeeInfo') and o.name = 'FK_SELLEREM_REFERENCE_SELLERDE')
alter table SellerEmployeeInfo
   drop constraint FK_SELLEREM_REFERENCE_SELLERDE

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SellerEmployeeInfo]') AND type in (N'U'))
	DROP TABLE [dbo].[SellerEmployeeInfo]

CREATE TABLE [dbo].[SellerEmployeeInfo] (
	[EmployeeId]			int IDENTITY(10000,1)	NOT NULL PRIMARY KEY, 			/*员工编号：自增，10000--2147483 647	0--1000000为系统保留*/
	[SellerId] 				int						NOT NULL, 						/*卖家用户编号*/
	[EmployeeCode]			varchar(10)				not null,						/*员工代码*/
	[EmployeeFullName]		nvarchar(20)			NOT NULL,						/*员工全名*/
	[EmployeeShortName]		nvarchar(10)			NOT NULL,						/*员工简名*/
	[PinyinCode]			varchar(10)				NOT NULL,						/*拼音码*/
	[DeptId]				int						NOT NULL DEFAULT((0)),			/*所属部门编号*/
	[MobilePhone]			varchar(20)				NOT NULL DEFAULT (''),			/*绑定手机*/
	[IsUsed]				bit						NOT NULL DEFAULT((1)),			/*是否启用*/
	/*********************************通用属性*********************************/
	[Config]				varchar(1000),											/*配置*/
	[Notes] 				nvarchar(250)			NOT NULL DEFAULT (''),			/*备注*/
	[RMan]					int						NOT NULL DEFAULT ((0)), 		/*创建人*/
	[RDate]					datetime				NOT NULL DEFAULT (getdate()),	/*创建日期*/
	[LMan]					int						NOT NULL DEFAULT ((0)), 		/*修改人*/
	[LDate]					datetime				NOT NULL DEFAULT (getdate()),	/*修改日期*/
	[Status] 				int 					NOT NULL DEFAULT ((0)), 		/*状态值*/
) ON [PRIMARY];


alter table SellerEmployeeInfo
   add constraint FK_SELLEREM_REFERENCE_SELLERIN foreign key (SellerId)
      references SellerInfo (SellerId)

alter table SellerEmployeeInfo
   add constraint FK_SELLEREM_REFERENCE_SELLERDE foreign key (DeptId)
      references SellerDeptInfo (DeptId)
