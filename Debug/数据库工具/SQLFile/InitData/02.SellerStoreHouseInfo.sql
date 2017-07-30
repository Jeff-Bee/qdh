/*
功能：		初始化【卖家仓库记录（系统默认）】记录
创建日期：	2016-11-01 by 龚俊
*/

Delete From [SellerStoreHouseInfo]  Where [StoreHouseId]=0
SET IDENTITY_Insert [SellerStoreHouseInfo] ON

INSERT INTO [dbo].[SellerStoreHouseInfo]([StoreHouseId],[SellerId],[Code],[Name],[Address],[Phone])
VALUES(0,0,'000','系统默认主仓','','')
SET IDENTITY_Insert [SellerStoreHouseInfo] OFF

Select * from [SellerStoreHouseInfo]