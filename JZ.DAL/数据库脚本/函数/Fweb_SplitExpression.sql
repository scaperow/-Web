USE [SYGLDB_XiCheng]
GO
/****** Object:  UserDefinedFunction [dbo].[Fweb_SplitExpression]    Script Date: 2013/8/15 8:42:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[Fweb_SplitExpression]
    (
      @ExpressionToBeSplited NVARCHAR(MAX) ,
      @SplitChar CHAR(1)
    )
RETURNS @Result TABLE ( [Value] NVARCHAR(MAX) )
AS 
    BEGIN
        DECLARE @Temp NVARCHAR(MAX)
       
        DECLARE @Index INT
        SET @Index = CHARINDEX(@SplitChar, @ExpressionToBeSplited, 1)
        WHILE ( @Index > 0 ) 
            BEGIN
                SELECT  @Temp = SUBSTRING(@ExpressionToBeSplited, 1,
                                          @Index - 1) 
                INSERT  INTO @Result
                VALUES  ( @Temp )
                SET @ExpressionToBeSplited = SUBSTRING(@ExpressionToBeSplited,
                                                       @Index + 1,
                                                       LEN(@ExpressionToBeSplited)
                                                       - @Index)
                SET @Index = CHARINDEX(@SplitChar, @ExpressionToBeSplited, 1)
            END
        IF ( LEN(@ExpressionToBeSplited) > 0 ) 
            BEGIN
    
                INSERT  INTO @Result
                        ( VALUE )
                VALUES  ( @ExpressionToBeSplited )
            END
        
        RETURN
    END

