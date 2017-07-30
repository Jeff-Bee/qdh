

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proc_Insert_SellerOrderRun]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[proc_Insert_SellerOrderRun]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================= 
-- Author:		龚俊 
-- Create date: 2017-01-02
-- Description:	 买家添加订单
-- 调用示例：
--DECLARE @OrderId int
--DECLARE @OrderCode varchar(20)	
--DECLARE @OrderDate dateTime
--Set @OrderDate=GETDATE()
--EXECUTE [dbo].[proc_Insert_SellerOrderRun] 
--   10005,1000016,@OrderDate,0,1,10.00,0,0,0,0,'config','Note'
--  ,@OrderId OUTPUT
--  ,@OrderCode OUTPUT

-- ============================================= 
CREATE PROCEDURE [dbo].[proc_Insert_SellerOrderRun] 
	@SellerId				int,
	@BuyerId				int,
	@OrderDate				datetime,
	@ProductType			smallint,
	@ProductQuantity		int,
	@Amount					money,
	@OrderState				smallint,
    @PayState				smallint,
    @PayAmount				money,
    @LogisticsState			smallint,
	@Config					varchar(1000),
	@Notes					nvarchar(250),
	@OrderId				int					out,					/*返回订单号，如果失败返回0*/
	@OrderCode				varchar(20)			out
AS 
BEGIN 
	SET NOCOUNT ON; 
	

	--插入新记录
	INSERT INTO [dbo].[SellerOrderRun]
           ([Code]
           ,[SellerId]
           ,[BuyerId]
           ,[OrderDate]
           ,[ProductType]
           ,[ProductQuantity]
           ,[Amount]
           ,[OrderState]
           ,[PayState]
           ,[PayAmount]
           ,[LogisticsState]
           ,[Config]
           ,[Notes])
     VALUES
           ('',@SellerId,@BuyerId,@OrderDate,@ProductType,@ProductQuantity
           ,@Amount,@OrderState,@PayState,@PayAmount,@LogisticsState
           ,@Config,@Notes)
	Set @OrderId = @@identity
	Declare @Count int
	select @Count=Count(*) from  [SellerOrderRun]  
		WHERE /*[SellerId]=@SellerId And*/ OrderId<@OrderId And convert(varchar(8),OrderDate,112)=convert(varchar(8),@OrderDate,112)  
	Set @Count=@Count+1
	Set @OrderCode = 'DH-W-'+ convert(varchar(8),@OrderDate,112)+ '-'+right(('00000'+ Cast(@Count as varchar(4))),4)
	Update [SellerOrderRun] Set Code=@OrderCode Where OrderId=@OrderId
	--Print Cast(@OrderId as varchar(10)) 
	--Print @OrderCode
END 
GO


