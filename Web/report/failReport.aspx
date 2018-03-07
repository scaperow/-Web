<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="failReport.aspx.cs" Inherits="report_failReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>
    <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript">

        var meet='';

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
            $(":radio[name='meet'][value=\"<%="RPNAME".RequestStr()%>\"]").prop('checked', 'checked');
            meet = "<%="RPNAME".RequestStr()%>";
            if ('<%="Type".RequestStr()%>' != '') {
                createGrid();
            }
            else {
                createChart();
            }

           
               
          

            $(window).resize(function () {
                $('#List').datagrid('resize');
            });
        });




        function FItem(value, row, index) {
            var _Temp = [];
            var _XM = '';
            var BHGS = value.split('||');
            for (var i = 0; i < BHGS.length; i++) {
                if (BHGS[i] == '') {
                    continue;
                }
                _Temp = BHGS[i].split(',');
                _XM += _Temp[0] + "<br />";             
                _Temp = [];
            }
            return _XM;
        }


        function FVal1(value, row, index) {

            var _Temp = [];
            var _SZ = '';
            var BHGS = value.split('||');
            for (var i = 0; i < BHGS.length; i++) {
                if (BHGS[i] == '') {
                    continue;
                }
                _Temp = BHGS[i].split(',');
                _SZ += _Temp[1] + "<br />";
                _Temp = [];
            }
            return _SZ;
        }


        function FVal2(value, row, index) {
            var _Temp = [];
           
            var _WCZ = '';
            var BHGS = value.split('||');
            for (var i = 0; i < BHGS.length; i++) {
                if (BHGS[i] == '') {
                    continue;
                }
                _Temp = BHGS[i].split(',');
               
                _WCZ += _Temp[2] + "<br />";
                _Temp = [];
            }

            return _WCZ;
        }

        function FAct(value, row, index) {

            be = " <span  style=\"cursor:pointer;\" onclick=\"openPopbar('" + row.IndexID + "','" + row.SectionName + "','" + row.CompanyName + "','" + row.TestRoomName + "','" + row.ReportName + "')\" >查看详情</span><br /> ";
            ///////  采集曲线 打开报告
            be1 = " <span  style=\"cursor:pointer;\" onclick=\"openPopbar1('" + row.IndexID + "','" + row.ReportName + "','" + row.ReportNumber + "','" + row.WTBH + "','" + row.ReportDate + "')\" >打开报告</span><br /> ";
            be2 = " <span  style=\"cursor:pointer;\" onclick=\"openPopbar2('" + row.IndexID + "','" + row.ReportNumber + "','" + row.DeviceType + "','" + row.ReportDate + "')\" >采集曲线</span> ";
            be1 = row.DeviceType == 0 ? "" : be1;
            return be + be1 + be2;
        }

        ///////////////////

        function openPopbar1(ID,RName,RNum,WTBH,RDate) {
        
                $.colorbox({
                    href: "onesearch.aspx?id=" + ID + "&MName=" + RName + "&r=" + Math.random(),
                    width: 950,
                    height: 1240,
                    title: function () {
                        return "报告编号：[" + RNum + "] 委托编号：[" + WTBH + "] 报告日期：[" + RDate + "] ";
                    },
                    close: '',
                    iframe: true
                });
        
        }

        function openPopbar2(ID, RNum, DeviceType, RDate) {

                $.colorbox({
                    href: "testdata.aspx?id=" + ID + "&DeviceType=" + DeviceType + "&r=" + Math.random(),
                    width: 700,
                    height: 500,
                    title: function () {
                        return "报告编号：[" + RNum + "]  报告日期：[" + RDate + "] ";//委托编号：[" + row["WTBH"] + "]<br />
                    },
                    close: '',
                    iframe: true
                });
          
        }


        function openPopbar(ID,Sn,Cn,Tr,Rn) {
       
            $.colorbox({
                href: "onefailreport.aspx?id=" + ID + "&r=" + Math.random(),
                width: 1024,
                height: 620,
                title: function () {
                    return "[" + Sn + "-" + Cn + "-" + Tr + "-" + Rn + "]";
                },
                close: '',
                iframe: true
            });
        }

        function createGrid() {
            $("#pan1").show();
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");
            $("#divChart").hide();
            $("#divList").show();
            $('#List').datagrid({
                url: 'failReport.aspx',
                queryParams: getSearchCondition('List')
            });


        }

        /////////////////////



        function createChart() {
            $("#pan1").hide();
            $(".chart_btn").removeClass("bagc_8584");
            $(".list_btn").addClass("bagc_8584");
            $("#divChart").show();
            $("#divList").hide();
            var data = getSearchCondition('Chart');
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "<%= ResolveUrl("failReport.aspx") %>",
                data: data,
                success: function (msg) {

                  
                    // SERIAL CHART    
                    chart = new AmCharts.AmSerialChart();
                    chart.dataProvider = eval(msg);
                    chart.categoryField = "Description";
                    //chart.startDuration = 1;
                    //chart.angle = 30;
                    //chart.depth3D = 15;

                    chart.columnSpacing3D = 1;
                    chart.columnWidth = 0.6;
                    chart.equalSpacing = true;
                    chart.addListener("clickGraphItem", handleClick);


                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
                    categoryAxis.parseDates = false;
                    categoryAxis.dashLength = 0.5;
                    categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#DADADA";
                    //categoryAxis.title = "施工单位";
                    categoryAxis.labelRotation = 45;


                    //// value                
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisThickness = 2;
                    valueAxis.axisColor = "#438eb9";//左边
                    valueAxis.dashLength = 2;
                    //valueAxis.logarithmic = true; // this line makes axis logarithmic
                    valueAxis.title = "资料总数";
                    valueAxis.integersOnly = true;
                    chart.addValueAxis(valueAxis);

                   
                    var valueAxis2 = new AmCharts.ValueAxis();
                    valueAxis2.position = "right"; // this line makes the axis to appear on the right
                    valueAxis2.axisColor = "#E76049";//右边
                    valueAxis2.gridAlpha = 0;
                    valueAxis2.title = "不合格资料数";
                    valueAxis2.integersOnly = true;
                    valueAxis2.axisThickness = 2;
                    chart.addValueAxis(valueAxis2);


                    var graph = new AmCharts.AmGraph();
                    graph.valueField = "IntNumber";
                    graph.balloonText = "[[category]]: [[value]]";
                    graph.labelText = "[[IntNumber]]";
                    graph.type = "column";
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 0.8;
                    graph.fillColors = "#438eb9";
                    graph.showHandOnHover = true;
                    graph.title = "资料总数";
                    chart.addGraph(graph);

                

                    var graph1 = new AmCharts.AmGraph();
                    graph1.valueAxis = valueAxis2;
                    graph1.valueField = "IntNumberMarks";
                    graph1.balloonText = "[[category]]: [[value]]";
                    graph1.labelText = "[[IntNumberMarks]]";
                    graph1.type = "column";
                    graph1.lineAlpha = 0;
                    graph1.fillAlphas = 0.8;
                    graph1.fillColors = "#E76049";
                    graph1.title = "不合格资料数";
                    graph1.showHandOnHover = true;
                    chart.addGraph(graph1);

                    var graph2 = new AmCharts.AmGraph();
                    graph2.valueAxis = valueAxis2;
                    graph2.valueField = "FloatNumber1";
                    graph2.balloonText = "[[category]]: [[value]]";
                    graph2.labelText = "[[FloatNumber1]]";
                    graph2.type = "column";
                    graph2.lineAlpha = 0;
                    graph2.fillAlphas = 0.8;
                    graph2.fillColors = "#FFD22B";
                    graph2.title = "未处理不合格资料数";
                    graph2.showHandOnHover = true;
                    chart.addGraph(graph2);

                    var graph2 = new AmCharts.AmGraph();
                    graph2.valueAxis = valueAxis2;
                    graph2.valueField = "DoubleNumber";
                    graph2.balloonText = "[[category]](不合格率)=[[DoubleNumber]]%";
                    graph2.type = "line";
                    graph2.bullet = "round";
                    graph2.lineColor = "#555555";//线条
                    graph2.lineThickness = 2;
                    graph2.title = "不合格率";//图例标题
                    graph2.labelText = "[[DoubleNumber]]%";
                    graph2.labelPosition = "right";
                    graph2.showHandOnHover = true;
                    chart.addGraph(graph2);

                    var legend = new AmCharts.AmLegend();//设置图例
                    legend.align = "left";
                    chart.addLegend(legend);
                    // WRITE
                    chart.write("chartdiv");

                }
            });
        }

        function handleClick(e) {
            var d = e.item.graph.valueField;
            if (d == "IntNumber") {
                openPopChart(this, e);
            }
            if (d == "IntNumberMarks") {
                openPopChart1(this, e);
            }
            if (d == "FloatNumber1") {
                openPopChart2(this, e);
            }

        }


        function openPopChart(obj, data) {
            $.colorbox({
                href: "../baseInfo/popBaseSummary.aspx?testcode=" + data.item.dataContext.Para1 + "&r=" + Math.random() + "&StartDate=" + $('#hd_startDate').val() + "&EndDate=" + $('#hd_endDate').val(),
                width: 900,
                height: 620,
                title: function () {
                    return "试验数据分布图 [" + data.item.dataContext.Description + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '',
                iframe: true
            });
        }

        function openPopChart1(obj, data) {
            $.colorbox({
                href: "popEvaluatedata.aspx?testcode=" + data.item.dataContext.Para1 + "&r=" + Math.random() + "&StartDate=" + $('#hd_startDate').val() + "&EndDate=" + $('#hd_endDate').val(),
                width: 1024,
                height: 620,
                title: function () {
                    return "不合格试验数据分布图 [" + data.item.dataContext.Description + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '',
                iframe: true
            });

        }


        function openPopChart2(obj, data) {
            $.colorbox({
                href: "unpopEvaluatedata.aspx?testcode=" + data.item.dataContext.Para1 + "&r=" + Math.random() + "&StartDate=" + $('#hd_startDate').val() + "&EndDate=" + $('#hd_endDate').val(),
                width: 1024,
                height: 620,
                title: function () {
                    return "不合格试验数据分布图 [" + data.item.dataContext.Description + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '',
                iframe: true
            });

        }


        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.Act = stype;
            data.sReportName = $('#txt_reportname').val();
            data.sReportCode = $('#txt_reportcode').val();
            
            data.NUM = '<%="NUM".RequestStr()%>';
          
            data.RPNAME = $(':radio[name="meet"]:checked').val();
            if (meet != '') {
                data.RPNAME = meet;
                meet = '';
            }
            return data;
        }


        function Export() {
            var Para = jQuery.param(getSearchCondition('BHGSJFX'));

            window.open("../Export/Export.aspx?sType=BHGSJFX&Ajax=" + Math.random() + "&" + Para);
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="piece">
        <div class="piece_con">
            <h2 class="title">
                <span class="left"><i></i>不合格数据分析

                
                 <input name="meet" type="radio" value="已处理"  checked="checked" />已处理
                 <input name="meet" type="radio" value="未处理"/>未处理
                </span>

         

                <ul class="searchbar_01 clearfix">
                   
                  
                    <li> <span class="text">报告名称</span><input class="searchadd" type="text" id="txt_reportname" /></li>
                    <li>
                        <span class="text">报告编号</span><input class="searchadd" type="text" id="txt_reportcode" />
                    </li>
                    <li class="right"><input name="" type="button" class="list_btn" title="列表" onclick="createGrid()"  /></li>
                    <li class="right"><input name="" type="button" class="chart_btn" title="图表" onclick="createChart()" /></li>
                     <li class="right">
                        <input name="" type="button" class="export_btn" title="导出" onclick="Export()" /></li>
                </ul>

            </h2>
            <div class="content">

                <div id="divChart">
                    <div id="chartdiv"></div>
                </div>
                <div id="divList">
                    
                       <!-- 列表 -->
                    <table id="List" iconcls="" rownumbers="true" fitcolumns="true" nowrap="false"
                        width="100%" height="500px;" data-options='' url=""
                        striped="true" singleselect="true" pagination="true" fit="true"
                        sortname="" sortorder="">

                        <thead>
                            <tr> 
                                <th field="ReportDate1" width="50" align="center" >
                                    日期
                                </th>
                                <th field="SectionName" width="50" align="center">标段
                                </th>
                                <th field="CompanyName" width="50" align="center">单位
                                </th>
                                <th field="TestRoomName" width="50" align="center">试验室
                                </th>
                                <th field="ReportName" width="30" align="center">试验报告
                                </th>
                                <th field="F_InvalidItem" width="80" align="center" formatter="FItem">不合格项目
                                </th>
                                <th field="F_InvalidItem1" width="30" align="center" formatter="FVal1">标准值
                                </th>
                                <th field="F_InvalidItem2" width="30" align="center" formatter="FVal2">实测值
                                </th>
                                 <th field="SGComment" width="50" align="center">原因分析
                                </th>
                                <th field="JLComment" width="50" align="center">监理意见
                                </th>
                                 <th field="DealResult" width="50" align="center">处理情况
                                </th>
                                 <th field="ReportNumber" width="50" align="center">报告编码
                                </th>
                                <th field="WTBH" width="30" align="center" hidden="hidden">
                                </th>
                                <th field="IndexID" width="40" align="center" formatter="FAct">操作
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
