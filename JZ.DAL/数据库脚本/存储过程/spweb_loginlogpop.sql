USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_loginlogpop]    Script Date: 2013/8/15 8:43:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-4
-- Description:	全线资料列表弹出层
-- =============================================
ALTER PROCEDURE [dbo].[spweb_loginlogpop] 
	-- Add the parameters for the stored procedure here
	@testcode VARCHAR(50), --标准试验室id
	@ftype TINYINT, -- 频率类型1=平行，2=见证
	@startdate varchar(30),
	@enddate varchar(30)
AS
BEGIN



	 IF	@testcode!=''
	 BEGIN
    SELECT UserName,FirstAccessTime,LastAccessTime FROM dbo.sys_loginlog WHERE UserName=@testcode AND FirstAccessTime>@startdate AND FirstAccessTime<=@enddate
	 END
  
  ELSE
  BEGIN
	  SELECT UserName,FirstAccessTime,LastAccessTime FROM dbo.sys_loginlog WHERE 1=0
  END
     
			
END

