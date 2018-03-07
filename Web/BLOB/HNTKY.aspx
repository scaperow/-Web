<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="HNTKY.aspx.cs" Inherits="BLOB_HNTKY" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts1.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/serial.js") %>"></script>
    <style>
         .Attr,.YCItem ,.Year{
            display:inline-block;
            border:#ccc 1px solid;
            padding:0 3px;
            color:#fff;
            background:#ccc;
            font-size:12px;
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
                <span class="left"><i></i>混凝土检测数据分析


                </span>
               
                <ul class="searchbar_01 clearfix">
                    <li style="display: inline-block; padding-left: 20px; margin: 0px;" >
                        <a class="YCItem" onclick="Chart(this,null,null)" value="95ba20cd-ee3f-4ed7-b6ba-0f2436e93ff2" >引气剂</a>
                        <a class="YCItem" onclick="Chart(this,null,null)" value="5ef46ba4-f1a7-4f8a-a056-3a52489406bd">混凝土外加剂</a> 
                        <a class="YCItem" onclick="Chart(this,null,null)" value="7709a0b1-d949-46e3-822c-6e4c6353799d">细骨料</a> 
                        <a class="YCItem" onclick="Chart(this,null,null)" value="86c816f7-f01a-4e08-b960-a941ed615cc1">粉煤灰</a> 
                        <a class="YCItem" onclick="Chart(this,null,null)" value="e54ae4e0-b718-4fe1-8d07-a98cb6c0c66d">速凝剂</a> 
                        <a class="YCItem Active "onclick="Chart(this,null,null)"  value="4EFCE442-DDF2-4DC4-978F-B5B35216C42B">水泥</a>
                        <a class="YCItem" onclick="Chart(this,null,null)" value="b2f9ed26-1d19-4cf8-9208-b62b818447d7">粗骨料</a>  
                        <a class="YCItem" onclick="Chart(this,null,null)" value="14abf6ba-75e9-4d58-aab2-daa90ffd876f">矿粉</a>
                        
                        <select id="QDDJ" style="float:none;" > 
                            <option>C15</option>
                            <option>C20</option>
                            <option>C25</option>
                            <option>C30</option>
                            <option>C35</option>
                            <option selected>C40</option>
                            <option>C40水下</option>
                            <option>C45</option>
                            <option>C50</option>
                            <option>C55</option>
                            <option>C60</option>
                        </select>
                       <%-- 用料量:
                        <input type="text" style="float:none;" value="" size="5" id="YLL" />
                        ±<input type="text" style="float:none;" value="" size="5"  id="YLLOffset" />--%>

                        <input type="button" value="查询" id="searchbut"  onclick="Chart(null, null, null); " style="float:none; " />
                    </li>
                   
                    <li class=" right">
                            <%
                                for (var i = 1; i < 13; i++)
                                {
                                    Response.Write("<a value=\"M\" class=\"Year "+(i==DateTime.Now.Month?"Active":"")+"\" onclick=\"Chart(null, null,this); \">"+i+"</a>"); 
                                }
                                 %> 
                       
                          <a value="USEMI" class="Year  " onclick="Chart(null, null,this); ">上半年</a>
                          <a value="DSEMI" class="Year  " onclick="Chart(null, null,this); ">下半年</a>
                          <a value="ALL" class="Year " onclick="Chart(null, null,this); ">全年</a>
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
    <div class="piece">
        <div class="piece_con">

            <h2 class="title">

                <ul class="searchbar_01 clearfix">
                    <li style="display: inline-block; padding-left: 2px; margin: 0px;" id="Attr">
                       
                        
                    </li>
                    
                  

                </ul>
            </h2>
            <div class="content">

                <div id="divChart1">
                    <div id="chartdiv1" class="chartdiv"></div>

                </div>
                
            </div>
        </div>
    </div>
    <script type="text/javascript">


        $(function () {
          
            LoadAttr();

            $('.YCItem').bind("click", function () { LoadAttr(); });
        });


        function LoadAttr() {
            $('#Attr').empty();
            $.post("HNTKY.aspx", { Act: "Attr", ItemID: $('a.YCItem.Active').attr('value') }, function (d) {
                       if (d == 'null') { return; }
                       var json = eval(d);
                       for (var i = 0; i < json.length; i++) {
                           $('#Attr').append(' <a onclick="Chart(null,this,null)" class="Attr  ' + (i == 0 ? 'Active' : '') + '" value="' + json[i].BindField + '">' + json[i].ItemName + '</a>');

                       }
                       $('a.Year.Active').click();
                   });

               }



        var chart;
        var MD = {
            <%
        for (int i = 1; i < 13; i++)
        {
            Response.Write(i+":"+DateTime.DaysInMonth(DateTime.Now.Year, i));
            if (i < 12)
            {
                Response.Write(","); 
            }
        }
        %>
        };
        function Chart(YCItem,Attr,t) {


            if (YCItem != null) {
                $('a.YCItem').removeClass('Active');
                $(YCItem).addClass('Active');

               
            }

            if (Attr != null) {
                $('a.Attr').removeClass('Active');
                $(Attr).addClass('Active');
                
            }
           
            if (t != null) {


                $('a.Year').removeClass('Active');
                $(t).addClass('Active');

                switch ($(t).attr('value')) {
                    case "M":
                        var TM = $(t).text();
                        $('#txt_startDate').val('<%=DateTime.Now.ToString("yyyy")%>-'+TM+'-1');
                        $('#txt_endDate').val('<%=DateTime.Now.ToString("yyyy")%>-' + TM +'-'+ MD[TM]); 
                        break;
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
            YCItem = $('a.YCItem.Active').attr('value');
            Attr = $('a.Attr.Active').attr('value');

            if (chart != undefined && chart != null) {
                chart.clear();
            }

            $.post("HNTKY.aspx", {
                Act: 'Chart',
                QDDJ: $('#QDDJ').find("option:selected").val(),
                YCItem: YCItem,
                Attr: Attr,
                StartDate: $('#txt_startDate').val(),
                EndDate: $('#txt_endDate').val(),
                YLL: $('#YLL').val(),
                YLLOffset: $('#YLLOffset').val()
            }, function (d) {

                var data = jQuery.parseJSON(d);



                // SERIAL CHART
                chart = new AmCharts.AmSerialChart();
                chart.dataProvider = data.D1;
                chart.categoryField = "BGRQ";
                chart.startDuration = 0.5;
                chart.balloon.color = "#000000";
                chart.addListener("clickGraphItem", handleClick);

                // AXES
                // category
                var categoryAxis = chart.categoryAxis;
                categoryAxis.fillAlpha = 1;
                categoryAxis.fillColor = "#FAFAFA";
                categoryAxis.gridAlpha = 0;
                categoryAxis.axisAlpha = 0;
                categoryAxis.gridPosition = "start";
                categoryAxis.position = "top";




                var valueAxis = new AmCharts.ValueAxis();
                valueAxis.title = "平均值";
                valueAxis.unit = "";
                valueAxis.dashLength = 5;
                valueAxis.axisAlpha = 0;
                //valueAxis.minimum = data.Min;
                //valueAxis.maximum = data.Max;
                valueAxis.integersOnly = true;
                valueAxis.gridCount = 10;
                //valueAxis.reversed = true; // this line makes the value axis reversed
                chart.addValueAxis(valueAxis);
	            		
                var graph = new AmCharts.AmGraph();
                graph.title = $('#QDDJ').find("option:selected").val();
                graph.valueField = 'avg';
                graph.dashLength = 3;
                graph.balloonText = "[[title]] [[value]] 报告日期:[[BGRQ]] 序号:[[OrderID]]";
                graph.lineAlpha = 1;
                graph.bullet = "round";

                graph.showHandOnHover = true;
                chart.addGraph(graph);

                // CURSOR
                var chartCursor = new AmCharts.ChartCursor();
                chartCursor.cursorPosition = "mouse";
                chartCursor.zoomable = false;
                chartCursor.cursorAlpha = 0;
                chart.addChartCursor(chartCursor);

                // LEGEND
                var legend = new AmCharts.AmLegend();
                legend.useGraphSettings = true;
                legend.valueText = '';
                chart.addLegend(legend);


                chart.write("chartdiv");


                ////////////////////////////////////////
                // SERIAL CHART
                chart = new AmCharts.AmSerialChart();
                chart.dataProvider = data.D2;
                chart.categoryField = "BGRQ";
                chart.startDuration = 0.5;
                chart.balloon.color = "#000000";
                chart.addListener("clickGraphItem", handleClick);

                // AXES
                // category
                var categoryAxis = chart.categoryAxis;
                categoryAxis.fillAlpha = 1;
                categoryAxis.fillColor = "#FAFAFA";
                categoryAxis.gridAlpha = 0;
                categoryAxis.axisAlpha = 0;
                categoryAxis.gridPosition = "start";
                categoryAxis.position = "top";




                var valueAxis = new AmCharts.ValueAxis();
                valueAxis.title = "检测值";
                valueAxis.unit = "";
                valueAxis.dashLength = 5;
                valueAxis.axisAlpha = 0;
                //valueAxis.minimum = data.Min;
                //valueAxis.maximum = data.Max;
                valueAxis.integersOnly = true;
                valueAxis.gridCount = 10;
                //valueAxis.reversed = true; // this line makes the value axis reversed
                chart.addValueAxis(valueAxis);

                var graph = new AmCharts.AmGraph();
                graph.title = $('a.Attr.Active').text();
                graph.valueField = Attr;
                graph.dashLength = 3;
                graph.balloonText = "[[title]] [[value]] 报告日期:[[BGRQ]] 序号:[[OrderID]]";
                graph.lineAlpha = 1;
                graph.bullet = "round";

                graph.showHandOnHover = true;
                chart.addGraph(graph);

                // CURSOR
                var chartCursor = new AmCharts.ChartCursor();
                chartCursor.cursorPosition = "mouse";
                chartCursor.zoomable = false;
                chartCursor.cursorAlpha = 0;
                chart.addChartCursor(chartCursor);

                // LEGEND
                var legend = new AmCharts.AmLegend();
                legend.useGraphSettings = true;
                legend.valueText = '';
                chart.addLegend(legend);


                chart.write("chartdiv1");

            });


        }



        function handleClick(e) {


            $.colorbox({
                href: "../report/onesearch.aspx?id=" + e.item.dataContext.DataID + "&MName=" + $('a.Item.Active').text() + "&r=" + Math.random() ,
                width: 950,
                height: 1240,
                title: function () {
                    return "报告编号：[" + e.item.dataContext.BGBH + "]  报告日期：[" + e.item.dataContext.BGRQ + "] ";
                },
                close: '',
                iframe: true
            });

        }







    </script>
</asp:Content>



