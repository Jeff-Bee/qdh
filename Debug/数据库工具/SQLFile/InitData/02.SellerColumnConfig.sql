/*
功能：		初始化【卖家表格列展示配置信息】记录
创建日期：	2016-10-30 by 龚俊
*/

Delete From [SellerColumnConfig]


INSERT INTO [dbo].[SellerColumnConfig]([SellerId],[TableName],[Index],[FieldName],[DefaultName],[DisplayName],[Visible])
Values
 (0,'ProductInfo'	,0	,'ProductCode'		,'商品编号'		,'商品编号'		,1)
,(0,'ProductInfo'	,1	,'ProductFullName'	,'商品全名'		,'商品全名'		,1)
,(0,'ProductInfo'	,2	,'ProductShortName'	,'商品简称'		,'商品简称'		,1)
,(0,'ProductInfo'	,3	,'PinyinCode'		,'拼音码'		,'拼音码'		,1)
,(0,'ProductInfo'	,4	,'ProductModel'		,'商品型号'		,'商品型号'		,1)
,(0,'ProductInfo'	,5	,'ProductSpec'		,'商品规格'		,'商品规格',1)
,(0,'ProductInfo'	,6	,'ProductUnit'		,'单位','单位',1)
,(0,'ProductInfo'	,7	,'Weight'			,'重量','重量',1)
,(0,'ProductInfo'	,9	,'Place'			,'商品产地','商品产地',1)
Select * from [SellerColumnConfig] Order by [index]