USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_pager]    Script Date: 2013/8/15 8:44:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[spweb_pager]
(
@tblName     nvarchar(200),        ----要显示的表或多个表的连接
@fldName     nvarchar(500) = '*',    ----要显示的字段列表
@pageSize    int = 1,        ----每页显示的记录个数
@page        int = 10,        ----要显示那一页的记录
@pageCount    int = 1 output,            ----查询结果分页后的总页数
@Counts    int = 1 output,                ----查询到的记录数
@fldSort    nvarchar(200) = null,    ----排序字段列表或条件
@Sort        bit = 1,        ----排序方法，0为升序，1为降序(如果是多字段排列Sort指代最后一个排序字段的排列顺序(最后一个排序字段不加排序标记)--程序传参如：' SortA Asc,SortB Desc,SortC ')
@strCondition    nvarchar(4000) = null,    ----查询条件,不需where
@ID        nvarchar(150),        ----主表的主键
@Dist                 bit = 0           ----是否添加查询字段的 DISTINCT 默认0不添加/1添加
)
AS
SET NOCOUNT ON
Declare @sqlTmp nvarchar(1000)        ----存放动态生成的SQL语句
Declare @strTmp nvarchar(1000)        ----存放取得查询结果总数的查询语句
Declare @strID     nvarchar(1000)        ----存放取得查询开头或结尾ID的查询语句

Declare @strSortType nvarchar(10)    ----数据排序规则A
Declare @strFSortType nvarchar(10)    ----数据排序规则B

Declare @SqlSelect nvarchar(50)         ----对含有DISTINCT的查询进行SQL构造
Declare @SqlCounts nvarchar(50)          ----对含有DISTINCT的总数查询进行SQL构造


if @Dist  = 0
begin
    set @SqlSelect = 'select '
    set @SqlCounts = 'Count(*)'
end
else
begin
    set @SqlSelect = 'select distinct '
    set @SqlCounts = 'Count(DISTINCT '+@ID+')'
end


if @Sort=0
begin
    set @strFSortType=' ASC '
    set @strSortType=' DESC '
end
else
begin
    set @strFSortType=' DESC '
    set @strSortType=' ASC '
end

 

--------生成查询语句--------
--此处@strTmp为取得查询结果数量的语句
if @strCondition is null or @strCondition=''     --没有设置显示条件
begin
    set @sqlTmp =  @fldName + ' From ' + @tblName
    set @strTmp = @SqlSelect+' @Counts='+@SqlCounts+' FROM '+@tblName
    set @strID = ' From ' + @tblName
end
else
begin
    set @sqlTmp = + @fldName + 'From ' + @tblName + ' where (1>0) ' + @strCondition
    set @strTmp = @SqlSelect+' @Counts='+@SqlCounts+' FROM '+@tblName + ' where (1>0) ' + @strCondition
    set @strID = ' From ' + @tblName + ' where (1>0) ' + @strCondition
end

----取得查询结果总数量-----
exec sp_executesql @strTmp,N'@Counts int out ',@Counts out
declare @tmpCounts int
if @Counts = 0
    set @tmpCounts = 1
else
    set @tmpCounts = @Counts

    --取得分页总数
    set @pageCount=(@tmpCounts+@pageSize-1)/@pageSize

    /**//**当前页大于总页数 取最后一页**/
    if @page>@pageCount
        set @page=@pageCount

    --/*-----数据分页2分处理-------*/
    declare @pageIndex int --总数/页大小
    declare @lastcount int --总数%页大小 

    set @pageIndex = @tmpCounts/@pageSize
    set @lastcount = @tmpCounts%@pageSize
    if @lastcount > 0
        set @pageIndex = @pageIndex + 1
    else
        set @lastcount = @pagesize
 --//***显示分页
    if @strCondition is null or @strCondition=''     --没有设置显示条件
    begin
        if @pageIndex<2 or @page<=@pageIndex / 2 + @pageIndex % 2   --前半部分数据处理
            begin 
                set @strTmp=@SqlSelect+' top '+ CAST(@pageSize as VARCHAR(4))+' '+ @fldName+' from '+@tblName
                        +' where '+@ID+' not in('+ @SqlSelect+' top '+ CAST(@pageSize*(@page-1) as Varchar(20)) +' '+ @ID +' from '+@tblName
                        +' order by '+ @fldSort +' '+ @strFSortType+')'
                        +' order by '+ @fldSort +' '+ @strFSortType 
            end
        else
            begin
            set @page = @pageIndex-@page+1 --后半部分数据处理
                if @page <= 1 --最后一页数据显示
                    set @strTmp=@SqlSelect+' * from ('+@SqlSelect+' top '+ CAST(@lastcount as VARCHAR(4))+' '+ @fldName+' from '+@tblName
                        +' order by '+ @fldSort +' '+ @strSortType+') AS TempTB'+' order by '+ @fldSort +' '+ @strFSortType 
                else                
                    set @strTmp=@SqlSelect+' * from ('+@SqlSelect+' top '+ CAST(@pageSize as VARCHAR(4))+' '+ @fldName+' from '+@tblName
                        +' where '+@ID+' not in('+ @SqlSelect+' top '+ CAST(@pageSize*(@page-2)+@lastcount as Varchar(20)) +' '+ @ID +' from '+@tblName
                        +' order by '+ @fldSort +' '+ @strSortType+')'

                        +' order by '+ @fldSort +' '+ @strSortType+') AS TempTB'+' order by '+ @fldSort +' '+ @strFSortType 
            end
    end

    else --有查询条件
    begin
        if @pageIndex<2 or @page<=@pageIndex / 2 + @pageIndex % 2   --前半部分数据处理
        begin 
                set @strTmp=@SqlSelect+' top '+ CAST(@pageSize as VARCHAR(4))+' '+ @fldName +' from  '+@tblName
                    +' where '+@ID+' not in('+ @SqlSelect+' top '+ CAST(@pageSize*(@page-1) as Varchar(20)) +' '+ @ID +' from '+@tblName
                    +' Where (1>0) ' + @strCondition + ' order by '+ @fldSort +' '+ @strFSortType+')'
                    +' ' + @strCondition + ' order by '+ @fldSort +' '+ @strFSortType                 
        end
        else
        begin 
            set @page = @pageIndex-@page+1 --后半部分数据处理
            if @page <= 1 --最后一页数据显示
                    set @strTmp=@SqlSelect+' * from ('+@SqlSelect+' top '+ CAST(@lastcount as VARCHAR(4))+' '+ @fldName+' from '+@tblName
                        +' where (1>0) '+ @strCondition +' order by '+ @fldSort +' '+ @strSortType+') AS TempTB'+' order by '+ @fldSort +' '+ @strFSortType
            else
                    set @strTmp=@SqlSelect+' * from ('+@SqlSelect+' top '+ CAST(@pageSize as VARCHAR(4))+' '+ @fldName+' from '+@tblName
                        +' where '+@ID+' not in('+ @SqlSelect+' top '+ CAST(@pageSize*(@page-2)+@lastcount as Varchar(20)) +' '+ @ID +' from '+@tblName
                        +' where (1>0) '+ @strCondition +' order by '+ @fldSort +' '+ @strSortType+')'
                        + @strCondition +' order by '+ @fldSort +' '+ @strSortType+') AS TempTB'+' order by '+ @fldSort +' '+ @strFSortType 
        end    
    end

------返回查询结果-----
exec sp_executesql @strTmp
--print @strTmp
SET NOCOUNT OFF 


