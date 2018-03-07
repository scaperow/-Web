<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="loginLog.aspx.cs" Inherits="logInfo_loginLog" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
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
            if ('<%="Type".RequestStr()%>' != '') {
                createGrid();
            }
            else {
                createChart();
            }
        });

        function getGridOption() {
            var data = getSearchCondition('loginLog');
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["用户", '项目', '标段', '单位', '试验室', '登录时间', '退出时间'],
                colModel: [
                { name: 'UserName', index: 'UserName', width: 30, sortable: false, align: 'center' },
                 { name: 'ProjectName', index: 'ProjectName', width: 50, align: 'center', sortable: false, hidden: true },
                  { name: 'SegmentName', index: 'SegmentName', width: 60, align: 'center', sortable: false },
                   { name: 'CompanyName', index: 'CompanyName', width: 60, align: 'center', sortable: false },
                     { name: 'TestRoomName', index: 'TestRoomName', width: 70, align: 'center', sortable: false },
                       { name: 'FirstAccessTime', index: 'FirstAccessTime', width: 40, align: 'center', sortable: false, formatter: "date", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' } },
                         { name: 'LastAccessTime', index: 'LastAccessTime', width: 40, align: 'center', sortable: false, formatter: "date", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' } }
                ],
                autowidth: true,
                shrinkToFit: true,
                jsonReader: {
                    page: "page",
                    total: "total",
                    repeatitems: false
                },
                pager: jQuery('#pager1'),
                rowNum: 15,
                rowList: [15, 30, 50, 100],
                sortname: 'LastAccessTime',
                sortorder: 'asc',
                viewrecords: true,
                height: '100%',
                loadui: 'disable',
                postData: data,
                gridComplete: function () {
                    adeptGridWidth();
                }
            };
            return obj;
        }


        function openPopbar(obj, data) {
            $.colorbox({
                href: "loginlogpop.aspx?id=" + data["UserName"] + "&r=" + Math.random(),
                width: 800,
                height: 620,
                title: function () {
                    return "" + data["SegmentName"] + "-" + data["CompanyName"] + "-" + data["TestRoomName"] + "-[" + data["UserName"] + "]";
                },
                close: '×',
                iframe: true
            });
        }

        function createGrid() {

            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");
            $("#divChart").hide();
            $("#divList").show();
            $("#list1").GridUnload();
            jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });


        }



        function createChart() {
            $(".chart_btn").removeClass("bagc_8584");
            $(".list_btn").addClass("bagc_8584");
            $("#divChart").show();
            $("#divList").hide();
            var data = getSearchCondition('loginLog');


            $.post("loginLog.aspx", { Act: 'CHART', StartDate: data.StartDate, EndDate: data.EndDate }, function (d) {

           
                var ChartData = jQuery.parseJSON(d);

                // SERIALL CHART
                chart = new AmCharts.AmSerialChart();
                chart.dataProvider = ChartData.Data;
                chart.categoryField = "Description";
                chart.plotAreaBorderAlpha = 0.2;
                chart.rotate = false;
                chart.depth3D = 20;
                chart.angle = 30;
                chart.startDuration = 1;
                chart.addListener("clickGraphItem", handleClick);

                // AXES
                // Category
                var categoryAxis = chart.categoryAxis;
                categoryAxis.gridAlpha = 0.1;
                categoryAxis.axisAlpha = 0;
                categoryAxis.gridPosition = "start";
                categoryAxis.labelRotation = 45;

                // value
                var valueAxis = new AmCharts.ValueAxis();
                valueAxis.title = "登录次数";
                valueAxis.unit = "次";
                valueAxis.stackType = "regular";
                valueAxis.gridAlpha = 0.1;
                valueAxis.axisAlpha = 0;
                chart.addValueAxis(valueAxis);

                var color = ["#FF0F00", "#FF6600", "#FF9E01", "#FCD202", "#F8FF01", "#B0DE09", "#04D215", "#0D8ECF", "#0D52D1", "#2A0CD0", "#8A0CCF", "#CD0D74", "#754DEB", "#DDDDDD", "#999999", "#333333", "#000000"];

                // GRAPHS
                // firstgraph
                var c = 0; 
                for (var i = 0; i < ChartData.Test.length; i++) {
                    var graph = new AmCharts.AmGraph();
                    graph.title = ChartData.Test[i].TestRoomName;
                    graph.labelText = "[[title]] \n [[value]] 次";
                    graph.valueField = ChartData.Test[i].TestRoomName;
                    graph.type = "column";
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 1;
                    graph.lineColor = color[c];
                    graph.fillColors = color[c];

                    graph.balloonText = "[[title]]: [[value]] 次";

                    graph.showHandOnHover = true;

                    chart.addGraph(graph);

                    c++;
                }

                // LEGEND
                var legend = new AmCharts.AmLegend();
                legend.position = "right";
                legend.borderAlpha = 0.3;
                legend.horizontalGap = 10;
                legend.switchType = "v";
                //chart.addLegend(legend);

                chart.creditsPosition = "top-right";

                // WRITE
                chart.write("chartdiv");
            });
        }

        function handleClick(e) {
            openPopChart(this, e);
        }

        function openPopChart(obj, data) {
            $.colorbox({
                href: "loginpop.aspx?testcode=" + data.item.dataContext.nodecode,
                width: 1000,
                height: 620,
                title: " [" + data.item.dataContext.Description + "]-试验人员-登录日志-[" + $('#txt_startDate').val()
                        + " 至 " + $('#txt_endDate').val() + "]",
                close: '',
                iframe: true
            });
        }

        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sType = stype;
            data.username = $('#txt_username').val();
            data.RPNAME = '<%="RPNAME".RequestStr()%>';
            data.NUM = '<%="NUM".RequestStr()%>';
            return data;
        }

        function searchClick() {
            if ($("#divChart").css("display") == 'block') {
                createChart();
            }
            else {
                createGrid();
            }
        }

        function Export() {
            var Para = jQuery.param(getSearchCondition('LOGINLOG'));

            window.open("../Export/Export.aspx?Ajax=" + Math.random() + "&" + Para);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="piece">
        <div class="piece_con">


            <h2 class="title">
                <span class="left"><i></i>客户端登录日志</span>

                <ul class="searchbar_01 clearfix">
                    <li><span class="text">用户名称</span><input type="text" id="txt_username" /></li>
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
        <table id="list1"></table>
        <div id="pager1"></div>
    </div>
            </div>
        </div>
    </div>

</asp:Content>

