<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="betonReport.aspx.cs" Inherits="report_betonReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript">
        var tongChart;
        $(function () {
            creatchart();
        });

        function creatchart() {
            if (tongChart!=undefined && tongChart!=null) {
                tongChart.clear();
            }
            $.ajax({
                type: "get",
                dataType: "json",
                data: getSearchCondition(),
                async: false,
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx?sType=tongreport&r=") %>" + Math.random(),
                success: function (msg) {
                  
               
                    tongChart = new AmCharts.AmSerialChart();
                    tongChart.dataProvider = eval(msg);
                    tongChart.categoryField = "DescriptionDate";
                    tongChart.pathToImages = "../images/";//添加拖动的图片
                    var categoryAxis = tongChart.categoryAxis;
                    categoryAxis.parseDates = true;
                    categoryAxis.minPeriod = "DD";
                    categoryAxis.axisColor = "#DADADA";
                    categoryAxis.title = "日期";
                    categoryAxis.labelRotation = 45;
                    categoryAxis.dateFormats = [{
                        period: 'DD',
                        format: 'M月D日'
                    }, {
                        period: 'WW',
                        format: 'M月D日'
                    }, {
                        period: 'MM',
                        format: 'M月'
                    }, {
                        period: 'YYYY',
                        format: 'YYYY年'
                    }];

                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#438EB9";
                    valueAxis.dashLength = 1;
                    valueAxis.title = "组值";

                    //debugger;
                    valueAxis.minimum = minsize(msg, msg[0].Para2, "DoubleNumber") - 1;
                    valueAxis.maximum = maxsize(msg, msg[0].Para1, "DoubleNumber") + 1;
                    ////valueAxis.precision = 0;//数值后的小数位数
                    valueAxis.minMaxMultiplier = 1.1;//上下刻度的冗余
                    valueAxis.autoGridCount = false;//自动生成刻度线
                    valueAxis.integersOnly = true;
                    valueAxis.gridCount = parseInt(valueAxis.maximum - valueAxis.minimum) + 2;//纵坐标显示几个刻度线
                    valueAxis.labelFrequency = 2;//每个几个刻度显示一个label


                    tongChart.addValueAxis(valueAxis);

                    var guide = new AmCharts.Guide();
                    guide.value = msg[0].Para1;
                    guide.lineColor = "#DE5757";
                    guide.dashLength = 4;
                    guide.label = "基准值(" + msg[0].Para1 + ")";
                    guide.lineThickness = 1;
                    guide.labelRotation = 45;
                    guide.inside = true;
                    guide.lineAlpha = 1;
                    guide.position = "right";
                    valueAxis.addGuide(guide);

                    var guide = new AmCharts.Guide();
                    guide.value = msg[0].Para2;
                    guide.lineColor = "#DE5757";
                    guide.position = "right";
                    guide.dashLength = 4;
                    guide.label = "最小值(" + msg[0].Para2 + ")";
                    guide.lineThickness = 1;
                    guide.inside = true;
                    guide.lineAlpha = 1;
                    valueAxis.addGuide(guide);



                    var guide = new AmCharts.Guide();
                    guide.value = msg[0].Para3;
                    guide.lineColor = "#179430";
                    guide.position = "right";
                    guide.dashLength = 0;
                    guide.label = "平均值(" + msg[0].Para3 + ")";
                    guide.lineThickness = 2;
                    guide.labelRotation = 45;
                    guide.inside = true;
                    guide.lineAlpha = 1;
                    valueAxis.addGuide(guide);




                    var graph = new AmCharts.AmGraph();
                    graph.type = "smoothedLine";
                    //graph.title = "组值";
                    graph.bullet = "round";
                    graph.bulletSize = 4;
                    graph.bulletColor = "#438EB9";
                    graph.balloonText = "[[Para4]]";
                    graph.valueField = "DoubleNumber";
                    graph.lineThickness = 0;
                    graph.lineColor = "#438EB9";
                    tongChart.addGraph(graph);

                    var chartScrollbar = new AmCharts.ChartScrollbar();//鼠标拖动时间
                    tongChart.addChartScrollbar(chartScrollbar);

                    //var legend = new AmCharts.AmLegend();
                    //tongChart.addLegend(legend);
                    // WRITE
                    tongChart.write("chartdiv");
                }

            });
        }

        function minsize(data, m, column) {
            var len = data.length;
            var j = 0;
            if (len > 0) {
                var j = data[0][column];
                for (var i = 1; i < len ; i++) {
                    if (data[i][column] < j) {
                        j = data[i][column];
                    }
                }
            }
            if (m < j) {
                j = m;
            }
            return j;
        }

        function maxsize(data, m, column) {
            var len = data.length;
            var j = 0;
            if (len > 0) {
                var j = data[0][column];
                for (var i = 1; i < len ; i++) {
                    if (data[i][column] > j) {
                        j = data[i][column];
                    }
                }
            }
            if (m > j) {
                j = m;
            }
            return j;
        }


        function getSearchCondition() {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sModelID = $('#op_model option:selected').val();
            data.sUnit = $('#op_type option:selected').val();
            data.sDay = $('#op_days option:selected').val();

            return data;
        }

        function searchClick() {
            creatchart();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="piece">
        <div class="piece_con">
            <h2 class="title">

                <span class="left"><i></i>砼强度评定</span>

                <ul class="searchbar_01 clearfix">


                    <li><span class="text">试验模板</span>
                        <span class="select">
                            <span class="txt">
                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label></span>
                            <select id="op_model" style=" width:240px;">                           

                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>                              
                            </select>
                        </span><span class="ico_dow"></span></li>
                    <li><span class="text">  <asp:Label ID="Label2" runat="server" Text=""></asp:Label></span>
                        <span class="select">
                            <span class="txt">28天</span>
                            <select id="op_days">

                                     

                                 <asp:Literal ID="Literal2" runat="server"></asp:Literal>         
                            </select>
                        </span><span class="ico_dow"></span></li>

                    <li><span class="text">试验类型</span>
                        <span class="select">
                            <span class="txt">  <asp:Label ID="Label3" runat="server" Text=""></asp:Label></span>
                            <select id="op_type">
                                 <asp:Literal ID="Literal3" runat="server"></asp:Literal>         
                               
                            </select>
                        </span><span class="ico_dow"></span></li>

                  <%--  <li><span class="text">施工部位</span>
                        <span class="select">
                            <span class="txt">桥</span>
                            <select id="op_part">

                                <option value="桥">桥</option>
                                <option value="桥墩">桥墩</option>


                            </select>
                        </span><span class="ico_dow"></span></li>--%>

                    <li class="right">
                        <input name="" type="button" class="chart_btn" title="图表" onclick="searchClick()" /></li>
                </ul>
            </h2>
            <div class="content">


                <div id="chartdiv"></div>
            </div>
        </div>
    </div>



</asp:Content>

