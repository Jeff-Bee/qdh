

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Insert_BuyerInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[proc_Insert_BuyerInfo]
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
--DECLARE @BuyerId int
--set @MobilePhone='13100000000'
--EXECUTE @RC = [dbo].[proc_Insert_BuyerInfo] 
--   @MobilePhone
--  ,@BuyerId OUTPUT 

-- ============================================= 
CREATE PROCEDURE [dbo].[proc_Insert_BuyerInfo] 
	@MobilePhone 			varchar(20),					/*注册手机号*/
	@Password				varchar(20),
	@BuyerId						int			out					/*返回新用户ID,如果操作失败返回0*/
AS 
BEGIN 
	SET NOCOUNT ON; 
	Set @BuyerId=0

    select @BuyerId=[BuyerId] from  [BuyerInfo]  WHERE [MobilePhone]=@MobilePhone

	IF @BuyerId>0
		--注册的手机号已经存在
		BEGIN
			Update [BuyerInfo] Set [Password]=@Password,ValidityPeriod='2100-01-01 00:00:00' Where BuyerId=@BuyerId
			return
		END	

	--插入新记录
	INSERT INTO [dbo].[BuyerInfo]
		([CompanyName]
		,[Address]
		,[ZipCode]
		,[ContactName]
		,[LoginName]
		,[Password]
		,[MobilePhone]
		,[EMail]
		,[ValidityPeriod])			
	VALUES('','','','',@MobilePhone,@Password,@MobilePhone,'','2100-01-01 00:00:00') 
	Set @BuyerId = @@identity	

	
END 
GO


