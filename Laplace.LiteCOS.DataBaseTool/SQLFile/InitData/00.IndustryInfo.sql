/*
功能：		初始化【行业信息】记录
创建日期：	2016-10-18 by 龚俊
*/

Delete From [IndustryInfo]
SET IDENTITY_Insert [IndustryInfo] ON

INSERT INTO [dbo].[IndustryInfo]([IndustryId],[Name],[ParentId])
Values(1001,'冷饮食品',0)
SET IDENTITY_Insert [IndustryInfo] OFF

Select * from [IndustryInfo]