/*
功能：		初始化【卖家账号信息（系统默认）】记录
创建日期：	2016-11-01 by 龚俊
*/

Delete From [SellerAccountInfo] Where [AccountId]=0
SET IDENTITY_Insert [SellerAccountInfo] ON

INSERT INTO [dbo].[SellerAccountInfo]([AccountId],[SellerId],[Code],[FullName])
VALUES(0,0,'000','系统默认账户')
SET IDENTITY_Insert [SellerAccountInfo] OFF
Select * from [SellerAccountInfo]