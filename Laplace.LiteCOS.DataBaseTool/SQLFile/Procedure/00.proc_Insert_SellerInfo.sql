

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Insert_SellerInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[proc_Insert_SellerInfo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================= 
-- Author:		龚俊 
-- Create date: 2016-10-18
-- Description:	 添加卖家用户
-- 调用示例：
--DECLARE @MobilePhone varchar(20)
--DECLARE @SellerId int
--set @MobilePhone='13100000000'
--EXECUTE @RC = [dbo].[proc_Insert_SellerInfo] 
--   @MobilePhone
--  ,@SellerId OUTPUT 

-- ============================================= 
CREATE PROCEDURE [dbo].[proc_Insert_SellerInfo] 
	@CompanyName			nvarchar(50),
	@Password				varchar(20),
	@ContactName			nvarchar(10),
	@MobilePhone 			varchar(20),	 							/*注册手机号*/
	@IndustryId				int,
	@SellerId						int			out								/*返回新用户ID,如果操作失败返回0*/
AS 
BEGIN 
	SET NOCOUNT ON; 
	Set @SellerId=0
	IF EXISTS(select 1 from  [SellerInfo]  WHERE [MobilePhone]=@MobilePhone OR [CompanyName]=@CompanyName)
		--注册的手机号已经存在OR公司名称已经存在，按理说，不应该出现
		BEGIN
			return
		END	

	--插入新记录
	INSERT INTO [dbo].[SellerInfo]
		([CompanyName]
		,[IndustryId]
		,[Address]
		,[ZipCode]
		,[ContactName]
		,[LoginName]
		,[Password]
		,[MobilePhone]
		,[EMail]
		,[ValidityPeriod])			
	VALUES(@CompanyName,@IndustryId,'','',@ContactName,@MobilePhone,@Password,@MobilePhone,'','2100-01-01 00:00:00') 
	Set @SellerId = @@identity	

	
END 
GO


