<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="failreportsum.aspx.cs" Inherits="report_failreportsum" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript">
        
        $(function () {
            createChart();
        });

        function getGridOption() {
            var data = getSearchCondition('failreportsum');
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["标段", "单位", "试验室", "试验资料总数", "不合格资料总数","不合格比率(%)"],
                colModel: [
                {
                    name: 'segment', index: 'segment', width: 100,   align: 'center', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'segment' + rowId + "\'";
                    }
                },
                {
                    name: 'company', index: 'company', width: 150,   align: 'center', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'company' + rowId + "\'";
                    }
                },
                {
                    name: 'testroom', index: 'testroom', width: 200,  align: 'center', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'testroom' + rowId + "\'";
                    }
                },
                { name: 'totalncount', index: 'totalncount', width: 100, sortable: false, align: 'center' },
                { name: 'counts', index: 'counts', width: 100, align: 'center', sortable: false },
                 { name: 'prenct', index: 'prenct', width: 100, align: 'center', sortable: false }
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
                sortname: 'segment',
                sortorder: 'asc',
                viewrecords: true,
                height: '100%',
                loadui: 'disable',
                postData: data,
                gridComplete: function () {
                    var gridName = "list1";
                    Merger(gridName, 'segment', '');
                    Merger(gridName, 'company', 'segment');
                    Merger(gridName, 'testroom', 'company');
                    adeptGridWidth();
                }
            };
            return obj;
        }

        function createGrid() {
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");
            $("#divChart").hide();
            $("#divList").show();
            $("#list1").GridUnload();
            jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });

        }

        function createChart(n) {
            $(".chart_btn").removeClass("bagc_8584");
            $(".list_btn").addClass("bagc_8584");
            $("#divChart").show();
            $("#divList").hide();
            var data = getSearchCondition('failreportsum');
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx") %>",
                data: data,
                success: function (msg) {


                    chart = new AmCharts.AmSerialChart();

                    chart.dataProvider = eval(msg);
                    chart.categoryField = "Description";
                    //chart.startDuration = 1;
                    chart.angle = 30;
                    chart.depth3D = 15;

                    chart.columnSpacing3D = 1;
                    chart.columnWidth = 0.6;
                    chart.equalSpacing = true;

                    //chart.color = "#FFFFFF";
                    chart.addListener("clickGraphItem", handleClick);


                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
                    categoryAxis.parseDates = false;
                    categoryAxis.dashLength = 0.5;
                    categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#438eb9";
                    //categoryAxis.title = "试验室";
                    categoryAxis.labelRotation = 45;


                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#438eb9";//左边
                    valueAxis.axisThickness = 2;
                    valueAxis.gridAlpha = 0;
                    valueAxis.title = "不合格报告数";
                    valueAxis.integersOnly = true;
                    chart.addValueAxis(valueAxis);

                    var valueAxis2 = new AmCharts.ValueAxis();
                    valueAxis2.position = "right"; // this line makes the axis to appear on the right
                    valueAxis2.axisColor = "#E76049";//右边
                    valueAxis2.gridAlpha = 0;
                    valueAxis2.title = "不合格率=(不合格资料数/资料总数)*100%";
                    valueAxis2.axisThickness = 2;
                    valueAxis2.integersOnly = true;
                    chart.addValueAxis(valueAxis2);


                    var graph1 = new AmCharts.AmGraph();
                    graph1.valueAxis = valueAxis;
                    graph1.valueField = "IntNumberMarks";
                    graph1.balloonText = "[[category]](不合格资料数): [[value]]";
                    graph1.type = "line";
                    graph1.bullet = "round";
                    graph1.lineColor = "#E76049";//线条
                    graph1.labelText = "[[IntNumberMarks]]";
                    graph1.title = "不合格资料总数";
                    graph1.labelPosition = "left";
                    graph1.lineThickness = 2;
                    graph1.showHandOnHover = true;
                    chart.addGraph(graph1);

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

       
 

        function handleClick(data) {
            $.colorbox({
                href: "popEvaluatedata.aspx?testcode=" + data.item.dataContext.Para1 + "&r=" + Math.random(),
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

      
        function getSearchCondition(stype, isUnit) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sType = stype;
            return data;
        }

        function searchClick() {
            if ($("#divChart").css("display") == 'block') {
                createChart(1);
            }
            else {
                createGrid();
            }
        }

        function Export() {
            var Para = jQuery.param(getSearchCondition('BHGSJTJPM'));

            window.open("../Export/Export.aspx?Ajax=" + Math.random() + "&" + Para);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
       
   

    <div class="piece">
    	<div class="piece_con">
        	 <h2 class="title">
                    <span class="left"><i></i>不合格数据统计排名</span>

                <ul class="searchbar_01 clearfix">
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

