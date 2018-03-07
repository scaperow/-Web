USE [SYGLDB_XiCheng]
GO

/****** Object:  View [dbo].[v_bs_codeName]    Script Date: 2013/8/8 17:14:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




ALTER view [dbo].[v_bs_codeName] 
  AS
  
  select 
c.¹¤³Ì±àÂë,
c.¹¤³ÌÃû³Æ,
d.±ê¶Î±àÂë,
d.±ê¶ÎÃû³Æ,
e.µ¥Î»±àÂë,
e.µ¥Î»Ãû³Æ,
f.ÊÔÑéÊÒ±àÂë,
f.ÊÔÑéÊÒÃû³Æ
from 
(select a.NodeCode as '¹¤³Ì±àÂë',b.Description as '¹¤³ÌÃû³Æ' from sys_engs_Tree as a,sys_engs_ProjectInfo as b where a.RalationID = b.ID) as c,
(select a.NodeCode as '±ê¶Î±àÂë',b.Description as '±ê¶ÎÃû³Æ' from sys_engs_Tree as a,sys_engs_SectionInfo as b where a.RalationID = b.ID) as d,
(select a.NodeCode as 'µ¥Î»±àÂë',b.Description  as 'µ¥Î»Ãû³Æ' from sys_engs_Tree as a,sys_engs_CompanyInfo as b where a.RalationID = b.ID) as e,
(select a.NodeCode as 'ÊÔÑéÊÒ±àÂë',b.Description as 'ÊÔÑéÊÒÃû³Æ' from sys_engs_Tree as a,sys_engs_ItemInfo as b where a.RalationID = b.ID) as f
where 
substring(f.ÊÔÑéÊÒ±àÂë,1,4) = c.¹¤³Ì±àÂë and 
substring(f.ÊÔÑéÊÒ±àÂë,1,8) = d.±ê¶Î±àÂë and 
substring(f.ÊÔÑéÊÒ±àÂë,1,12) = e.µ¥Î»±àÂë



GO


