USE [SYGLDB_XiCheng]
GO

/****** Object:  View [dbo].[v_bs_evaluateData]    Script Date: 2013/8/8 17:14:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






ALTER view [dbo].[v_bs_evaluateData] 
  AS
  
 select 
        a.ID,
        a.ModelCode,
        a.ModelIndex,
        c.Description as SectionName,	
		c.NodeCode AS SectionCode,
        b.Description as CompanyName,	
		b.NodeCode AS CompanyCode,  
        d.Description as TestRoomName,	
		d.NodeCode AS TestRoomCode,
        a.ReportName,
        a.ReportNumber,
        a.SCTS AS ReportDate,
        a.F_InvalidItem, 
        a.SGComment, 
        a.JLComment 
        from 
        sys_biz_reminder_evaluateData as a,
		 ( select a.NodeCode,b.Description from sys_engs_Tree as a,sys_engs_CompanyInfo as b where a.RalationID = b.ID) as b,
        (select a.NodeCode,b.Description from sys_engs_Tree as a,sys_engs_SectionInfo as b where a.RalationID = b.ID) as c,
        (select a.NodeCode,b.Description from sys_engs_Tree as a,sys_engs_ItemInfo as b where a.RalationID = b.ID) as d  
        where b.NodeCode = substring(a.ModelCode,0,Len(a.ModelCode)-7) and 
              c.NodeCode = substring(a.ModelCode,0,Len(a.ModelCode)-11) and
              d.NodeCode = substring(a.ModelCode,0,Len(a.ModelCode)-3) 





GO


