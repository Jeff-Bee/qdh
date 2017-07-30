/*
功能：		初始化【卖家记录（系统默认）】记录
创建日期：	2016-11-01 by 龚俊
*/

--Delete From [SellerInfo]
SET IDENTITY_Insert [SellerInfo] ON

INSERT INTO [dbo].[SellerInfo]([SellerId],[CompanyName],[ContactName],[Password],[MobilePhone],ValidityPeriod)
VALUES(0,'系统内部卖家','','','','2100-01-01 00:00:00')
SET IDENTITY_Insert [SellerInfo] OFF

Select * from [SellerInfo]