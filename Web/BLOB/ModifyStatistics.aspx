<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ModifyStatistics.aspx.cs" Inherits="BLOB_ModifyStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>

      <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
 <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
      <div class="piece">
        <div class="piece_con">

            <h2 class="title">
                <span class="left"><i></i>关键字段修改统计</span>
               
                <ul class="searchbar_01 clearfix">
                    <li class="right">
                        <input name="" type="button" class="list_btn " title="列表" onclick="createGrid()" /></li>
                    <li class="right">
                        <input name="" type="button" class="chart_btn" title="图表" onclick="createChart(1)"  style="display:none;" /></li>
                    
                        
                </ul>
            </h2>
            <div class="content">

                <div id="divChart" style="display:none;">
                    <div id="chartdiv"></div>
                </div>
                <div id="divList">
                       <!-- 列表 -->
             <table id="List"  class="easyui-datagrid"   iconcls="" rownumbers="true" fitcolumns="true" nowrap="false"
        width="100%" height="100%" data-options='' url="ModifyStatistics.aspx?Act=List"
        striped="true" singleselect="true"  pagination="true" fit="true" 
           sortName="" sortOrder=""
           >
        
             <thead>
            <tr>
               

                <th field="DESCRIPTION"  width="40"  align="center" >
                    试验室
                </th>
                <th field="name"  width="30"  align="center" >
                    模板名称
                </th>
                <th field="BGBH"  width="30"  align="center" >
                    报告编号
                </th>
                <th field="ModifyItem"  width="200"  align="center" formatter="FModifyItem">
                    修改内容
                </th>

                <th field="ModifyBy"  width="30"  align="center" >
                    修改人
                </th>
                 <th field="ModifyTime"  width="30"  align="center" formatter="FDatetime">
                    修改日期
                </th>
                <th field="SGContent"  width="50"  align="center" >
                    施工意见
                </th>
                 <th field="JLContent"  width="50"  align="center">
                    监理意见
                </th>
                <th field="YZContent"  width="50"  align="center" >
                    业主意见
                </th>
                <th field="Status"  width="50"  align="center" formatter="FStatus">
                    状态
                </th>
                 <th field="KMID"  width="50"  align="center" formatter="FAct" <%=(("rolename".SessionStr().ToString() =="A")||("rolename".SessionStr().ToString() =="SS")?" ":"hidden=\"true\"") %>"><!--判断 业主 监理 -->
                    操作
                </th>
                 <th field="Status"  width="50"  align="center"  hidden="true" styler="SS">
                    
                </th>
            </tr>
                  </thead>
            
      
                 
    </table>
       
            <!-- 列表 -->
            
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function FDatetime(value, row, index) {
            return value.replace('T', ' ');
        }

        function FStatus(value, row, index) {

            switch (value)
            {
                case 0:
                    return "";
                    break;
                case 1:
                    return "通过";
                    break;
                case 2:
                    return "拒绝";
                    break;
            }
        }

        

        //A 业主 D监理
        function SS(value, row, index) {
            if (value == 1) {
                
                return 'background-color:red;color:#fff;';
            }

        }


        function FAct(value, row, index) {
            return  '<a href="#" onclick="OpenWin(\'' + row.KMID + '\');">处理</a>' ;
        }
        
        function OpenWin(id)
        {
            $.colorbox({
                href: "ModifyContent.aspx?ID=" + id + "&AJAX" + Math.random(),
                width:600,
                height: 500,
                title: function () {
                    return "关键字段修改统计-意见";
                },
                close: '',
                iframe: true
            });
        }
  
        function FModifyItem(value, row, index) {

            var vs;
            var v='';
            try {
                vs = eval(value);
                for (var i = 0; i < vs.length; i++)
                {
                    v += vs[i].Description + '['+vs[i].OriginalValue + '] 修改为 :[' + vs[i].CurrentValue+']<br>';
                }
                return v;
            }
            catch (ex) { }
            
        }






        function createGrid() {
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");

            $("#divList").show();
            $('#List').datagrid({
                url: 'ModifyStatistics.aspx',
                queryParams: {
                    Act: 'List',
                    StartDate: $('#txt_startDate').val(),
                    EndDate: $('#txt_endDate').val()

                }
            });


        }





    </script>
</asp:Content>

