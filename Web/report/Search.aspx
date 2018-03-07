<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="report_Search" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>
    <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript">
    
        $(function () {
            
         
            switch ('<%="Type".RequestStr()%>') {
                case '1':
                    $('#txt_startDate').val(new Date().getFullYear() + "-" + (new Date().getMonth() + 1) + "-1");
                    $('#txt_endDate').val('<%= DateTime.Now.ToString("yyyy-MM-dd")%>');
                    break;
                case '3':
                    $('#txt_startDate').val(new Date().getFullYear() + "-1-1");
                    $('#txt_endDate').val('<%= DateTime.Now.ToString("yyyy-MM-dd")%>');
                    break;
                case '2':
                    $('#txt_startDate').val(new Date().getFullYear() + "-" + (new Date().getMonth()) + "-1");
                    $('#txt_endDate').val('<%= new DateTime(DateTime.Now.Year,DateTime.Now.Month,1).AddDays(-1).ToString("yyyy-MM-dd")%>');
                    break;
            }
               
               
        
            createGrid();

            $(window).resize(function () {
                $('#List').datagrid('resize');
            });
        });

  
        function FAct(value, row, index) {

            be = "&nbsp;&nbsp;&nbsp;&nbsp;<span  style='color:#1c7c9c;cursor:pointer;' onclick=\"openPopbar('" + row.ID + "','" + row.MName + "','" + row.BGBH + "','" + row.BGRQ + "')\" >详情</span> &nbsp;&nbsp;&nbsp;&nbsp;";
            be1 = " <span  style='color:#1c7c9c;cursor:pointer;' onclick=\"openPopbar1('" + row.ID + "','" + row.BGBH + "','" + row.BGRQ + "','" + row.DeviceType + "')\" >采集信息</span> ";

            be1 = value == 0 ? "" : be1;

            return be + be1;
        }


        function openPopbar(ID,MName,BGBH,BGRQ) {
     
                $.colorbox({
                    href: "onesearch.aspx?id=" + ID + "&MName="+MName+"&r=" + Math.random(),
                    width: 950,
                    height: 1240,
                    title: function () {
                        return "报告编号：[" + BGBH + "]  报告日期：[" + BGRQ + "] ";// 委托编号：[" + row["WTBH"] + "] 
                    },
                    close: '',
                    iframe: true
                });
                  
        }

        function openPopbar1(ID, BGBH, BGRQ, DeviceType) {
           
                $.colorbox({
                    href: "testdata.aspx?id=" + ID + "&DeviceType=" + DeviceType + "&r=" + Math.random(),
                    width: 700,
                    height: 500,
                    title: function () {
                        return "报告编号：[" + BGBH + "]  报告日期：[" + BGRQ + "] ";//委托编号：[" + row["WTBH"] + "]<br />
                    },
                    close: '',
                    iframe: true
                });
       
        }


        function createGrid() {

            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");
            $("#divChart").hide();
            $("#divList").show();

            $('#List').datagrid({
                url: 'Search.aspx',
                queryParams: getSearchCondition('List')
            });

           
        }





        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.Act = stype;
            data.bgmc = $('#txt_bgmc').val();
            data.bgbh = $('#txt_bgbh').val();
            data.RPNAME = "<%="RPNAME".RequestStr()%>";
            data.NUM = '<%="NUM".RequestStr()%>';
            return data;
        }

        function searchClick() {

            createGrid();

        }

        function Export()
        {
            var Para = jQuery.param(getSearchCondition('DOCUMENTSEARCH'));

            window.open("../Export/Export.aspx?sType=DOCUMENTSEARCH&Ajax=" + Math.random() + "&" + Para);
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="piece">
        <div class="piece_con">


            <h2 class="title">
                <span class="left"><i></i>资料查询</span>

                <ul class="searchbar_01 clearfix">
                    <li><span class="text">报告名称</span><input type="text" id="txt_bgmc" style="width:250px;" value="" /></li>
                     <li><span class="text">报告编号</span><input type="text" id="txt_bgbh" style="width:250px;" /></li>
                    <li class="right">
                        <input name="" type="button" class="list_btn" title="列表" onclick="createGrid()" /></li>
                     <li class="right">
                        <input name="" type="button" class="export_btn" title="导出" onclick="Export()" /></li>
                </ul>
            </h2>

            <div class="content">


                <div id="divList">
                      <!-- 列表 -->
                    <table id="List" iconcls="" rownumbers="true" fitcolumns="true" nowrap="false"
                        width="100%" height="500px;" data-options='' url=""
                        striped="true" singleselect="true" pagination="true" fit="true"
                        sortname="" sortorder="">

                        <thead>
                            <tr>
                   
                                <th field="ID" width="80" align="center" hidden="hidden">
                                </th>
                                <th field="SegmentName" width="50" align="center">标段
                                </th>
                                <th field="CompanyName" width="30" align="center">单位
                                </th>
                                <th field="TestRoomName" width="100" align="center">试验室
                                </th>
                                <th field="MName" width="30" align="center">模板名称
                                </th>
                                <th field="WTBH" width="30" align="center">委托编号
                                </th>
                                <th field="BGBH" width="30" align="center">报告编号
                                </th>
                                <th field="BGRQ" width="30" align="center">报告日期
                                </th>
                                <th field="DeviceType" width="40" align="center" formatter="FAct">详情/采集信息
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>


                    </table>

                    <!-- 列表 -->
                </div>
            </div>
        </div>
    </div>



</asp:Content>


