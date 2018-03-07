<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AnalysisofTestData.aspx.cs" Inherits="BLOB_AnalysisofTestData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts1.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/serial.js") %>"></script>
    <style>
         .Item ,.Year{
            display:inline-block;
            border:#ccc 1px solid;
            padding:0 5px;
            color:#fff;
            background:#ccc;
        }
        .Active {
            background:#e76049;
            color:#FFF;
             border:#e76049 1px solid;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="piece">
        <div class="piece_con">

            <h2 class="title">
                <span class="left"><i></i>试验检测数据分析</span>
               
                <ul class="searchbar_01 clearfix">
                     <li style="display: inline-block; padding-left: 20px; margin: 0px;" id="Item">
                     </li>
                    <li class="right">
                          <a value="USEMI" class="Year  Active" onclick="Chart('', null,this); ">上半年</a>
                          <a value="DSEMI" class="Year  " onclick="Chart('', null,this); ">下半年</a>
                          <a value="ALL" class="Year " onclick="Chart('', null,this); ">全年</a>
                      </li>
                    

                </ul>
            </h2>
            <div class="content">

                <div id="divChart">
                    <div id="chartdiv"></div>

                </div>
                
            </div>
        </div>
    </div>
    <script type="text/javascript">


        $(function () {
            LoadItem();

           
        });


        function LoadItem() {
            $('#Item').empty();
            $.post("AnalysisofTestData.aspx", { Act: "Item" }, function (d) {
                if (d == 'null') { return; }
             
                var json = eval(d);
                for (var i = 0; i < json.length; i++) {
                    $('#Item').append(' <a onclick="Chart(\'' + json[i].ItemID + '\',this,null)" class="Item  ' + (i == 0 ? 'Active' : '') + '" value="' + json[i].ItemID + '">' + json[i].ItemName + '</a>');
                }

                Chart(json[0].ItemID, $('a.Item:first')[0], $('a.Year:first')[0]);
            });
        }





        var chart;
        function Chart(ItemID,o,t) {

            if (o != null) {
                $('a.Item').removeClass('Active');
                $(o).addClass('Active');
            }

            if (t != null) {
                $('a.Year').removeClass('Active');
                $(t).addClass('Active');

                switch ($(t).attr('value'))
                {
                    case "USEMI":
                        $('#txt_startDate').val('<%=DateTime.Now.Year%>-1-1');
                        $('#txt_endDate').val('<%=DateTime.Now.Year%>-6-30');
                        break;
                    case "DSEMI":
                        $('#txt_startDate').val('<%=DateTime.Now.Year%>-7-1');
                        $('#txt_endDate').val('<%=DateTime.Now.Year%>-12-31');
                        break;
                    case "ALL":
                        $('#txt_startDate').val('<%=DateTime.Now.Year%>-1-1');
                        $('#txt_endDate').val('<%=DateTime.Now.Year%>-12-31');
                        break;
                }
            }

            if (ItemID == '') {
                ItemID = $('a.Item.Active').attr('value')
            }

            if (chart != undefined && chart != null) {
                chart.clear();
            }

            $.post("AnalysisofTestData.aspx", { Act: 'Chart',  ID: ItemID, StartDate: $('#txt_startDate').val(), EndDate: $('#txt_endDate').val() }, function (d) {
               
                // SERIAL CHART
                chart = new AmCharts.AmSerialChart();
                chart.dataProvider = eval(d);
                chart.categoryField = "FactoryName";
                //chart.rotate = true;


                chart.fontSize = 14;
                chart.startDuration = 1;
                chart.plotAreaFillAlphas = 0.2;
                chart.addListener("clickGraphItem", handleClick);

                // the following two lines makes chart 3D
                chart.depth3D = 20;
                chart.angle = 30;

                // AXES
                // category
                var categoryAxis = chart.categoryAxis;
                categoryAxis.labelRotation = 90;
                categoryAxis.dashLength = 5;
                categoryAxis.gridPosition = "start";
                categoryAxis.labelRotation = 45;
                // value
                var valueAxis = new AmCharts.ValueAxis();
                //valueAxis.title = "使用总量(吨)";
                valueAxis.unit = "(吨)";
                valueAxis.dashLength = 5;
                chart.addValueAxis(valueAxis);

                // GRAPH
                var graph = new AmCharts.AmGraph();
                graph.labelText = "[[val]] 吨 ";
                graph.valueField = "val";
                graph.colorField = 'color';
                graph.fillColors = "#438eb9";
                graph.balloonText = "[[category]]: [[value]] 吨  资料:[[c]] 份";
                graph.type = "column";
                graph.lineAlpha = 0;
                graph.fillAlphas = 1;
                //graph.fixedColumnWidth = 0.3;
                //graph.columnWidth = 0.1;
                graph.showHandOnHover = true;
                chart.addGraph(graph);



                // CURSOR
                var chartCursor = new AmCharts.ChartCursor();
                chartCursor.cursorAlpha = 0;
                chartCursor.zoomable = false;
                chartCursor.categoryBalloonEnabled = false;
                chart.addChartCursor(chartCursor);

                chart.creditsPosition = "top-right";

                chart.write("chartdiv");

            });


        }




        function handleClick(e) {


            $.colorbox({
                href: "AnalysisofItemTestData.aspx?FID=" + e.item.dataContext.FactoryID + "&Name=" + $('a.Item.Active').text() + "&ID=" + $('a.Active').attr('value') + "&r=" + Math.random() + "&StartDate=" + $('#txt_startDate').val() + "&EndDate=" + $('#txt_endDate').val(),
                width: '100%',
                height: '100%',
                title: function () {
                    return e.item.dataContext.FactoryName + " [" + $('a.Item.Active').text() + " 检测指标分析]";
                },
                close: '',
                iframe: true
            });

        }




       
    </script>
</asp:Content>

