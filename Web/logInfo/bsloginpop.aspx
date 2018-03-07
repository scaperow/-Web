<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bsloginpop.aspx.cs" Inherits="logInfo_bsloginpop" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
            <link href="../css/default.css" rel="stylesheet" />
        <link href="../css/colorbox.css" rel="Stylesheet" />
        <link href="../css/css.css" rel="stylesheet" />
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-1.9.0.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/main.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajaxLoaderPopPage.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajax_loader.js") %>"></script>
     <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.colorbox-min.js") %>"></script>
    <script type="text/javascript">

        var tongChart;
        var pageCount, currPage = 1;
        $(function () {
            creatchart();
        });




        function creatchart() {
            var id = $("#hd_name").val();
            var data = getSearchCondition('bsloginpop');
            if (tongChart != undefined && tongChart != null) {
                tongChart.clear();
            }
            $.ajax({
                type: "get",
                dataType: "json",
                async: false,
                data: data,
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx?sUser=") %>" + encodeURI(id) + "&r=" + Math.random(),
                success: function (msg) {
                    // SERIAL CHART    
                   
                    var json = eval(msg);
                    pageCount = json.pageCount;
                    getPageTemp(currPage);


                    tongChart = new AmCharts.AmSerialChart();
                    tongChart.dataProvider = json.Data;


                    tongChart.categoryField = "loginday";
                    //tongChart.startDuration = 1;
                    tongChart.angle = 30;
                    tongChart.depth3D = 15;

                    tongChart.columnSpacing3D = 1;
                    tongChart.columnWidth = 0.6;
                    tongChart.equalSpacing = true;

                    // AXES
                    // category
                    var categoryAxis = tongChart.categoryAxis;
                    categoryAxis.parseDates = false;
                    categoryAxis.dashLength = 0.5;
                    categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#E76049";
                    //categoryAxis.title = "单位";
                    categoryAxis.labelRotation = 45;


                    //// value                
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.dashLength = 1;
                    valueAxis.axisThickness = 2;
                    valueAxis.axisColor = "#E76049";//左边
                    //valueAxis.logarithmic = true; // this line makes axis logarithmic
                    valueAxis.title = "登录次数";
                    valueAxis.integersOnly = true;
                    tongChart.addValueAxis(valueAxis);



                    var graph = new AmCharts.AmGraph();
                    graph.valueField = "counts";
                    graph.balloonText = "[[category]]\登录次数: [[value]]";
                    graph.type = "line";
                    graph.bullet = "round";
                    graph.lineColor = "#E76049";//线条
                    graph.labelText = "[[IntNumber]]";
                    graph.labelPosition = "left";
                    graph.labelText = "[[counts]]";
                    graph.lineThickness = 2;
                    graph.showHandOnHover = true;
                    tongChart.addGraph(graph);

                    // WRITE
                    tongChart.write("chartdiv");

                }

            });
        }


        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sType = stype;
            data.rows = 10;
            data.page = currPage;
            data.sidx = "ID";
            data.sord = "asc";
            return data;
        }



        var pages = {
            Start: function () {
                var firstPage = $("div.pages a[rel='page'][class='selected']").text();
                if (firstPage * 1 != 1) {
                    currPage = 1;
                    creatchart();
                }
            },
            PageAgo: function () {
                var firstPage = $("div.pages a[rel='page']:first").text();
                if (firstPage * 1 > 1) {
                    currPage = firstPage * 1 - 1;
                    creatchart();
                }
            },
            Ago: function () {
                var thisPage = $("div.pages a[rel='page'][class='selected']");
                var firstPage = $("div.pages a[rel='page']:first");
                if (thisPage.text() != firstPage.text()) {
                    currPage = currPage * 1 - 1;
                    creatchart();
                }
            },
            Page: function (e) {
                currPage = $(e).text();
                creatchart();
            },
            After: function () {
                var thisPage = $("div.pages a[rel='page'][class='selected']");
                var lastPage = $("div.pages a[rel='page']:last");
                if (thisPage.text() != lastPage.text()) {
                    currPage = currPage * 1 + 1;
                    creatchart();
                }
            },
            PageAfter: function () {
                var lastPage = $("div.pages a[rel='page']:last").text();
                if (lastPage * 1 < pageCount * 1) {
                    currPage = lastPage * 1 + 1;
                    creatchart();
                }
            },
            End: function () {
                var lastPage = $("div.pages a[rel='page'][class='selected']").text();
                if (lastPage * 1 != pageCount * 1) {
                    currPage = pageCount;
                    creatchart();
                }
            }
        }

        var getPageTemp = function (currPage) {
            var Start_currPage = currPage * 1 - 4;
            var End_currPage = currPage * 1 + 4;
            if (Start_currPage <= 1) {
                Start_currPage = 1;
                if (pageCount > 9) {
                    End_currPage = 9;
                } else {
                    End_currPage = pageCount;
                }
            }
            if (End_currPage >= pageCount) {
                End_currPage = pageCount;
                if (pageCount - 8 > 0) {
                    Start_currPage = pageCount - 8;
                }
                else {
                    Start_currPage = 1;
                }
            }
            var limit = "<span style='color:#666666;'>[第{1}页/共{0}页]</span>";
            var start = "<a onclick=\"pages.Start()\">首页</a>";
            var pageAgo = "<a onclick=\"pages.PageAgo()\"><<</a>";
            var ago = "<a onclick=\"pages.Ago()\">上一页</a>";
            var pageSelect = "<a rel=\"page\" onclick=\"pages.Page(this)\" class=\"selected\">{0}</a>";
            var page = "<a rel=\"page\" onclick=\"pages.Page(this)\">{0}</a>";
            var after = "<a onclick=\"pages.After()\">下一页</a>";
            var PageAfter = "<a onclick=\"pages.PageAfter()\">>></a>";
            var end = "<a onclick=\"pages.End()\">尾页</a>";
            var pageTemp = "<span style='color:#666666;'>[第" + currPage + "页/共" + pageCount + "页]</span>";

            pageTemp += start;
            pageTemp += ago;
            pageTemp += pageAgo;
            for (var i = Start_currPage; i <= End_currPage; i++) {
                if (i == currPage) pageTemp += "<a rel=\"page\" onclick=\"pages.Page(this)\" class=\"selected\">" + i + "</a>";
                else pageTemp += "<a rel=\"page\" onclick=\"pages.Page(this)\">" + i + "</a>";
            }
            pageTemp += after;
            pageTemp += PageAfter;
            pageTemp += end;
            $("div.pages").html(pageTemp);
        }
    </script>
</head>
<body>
       <input type="hidden" id="hd_startDate" value="<%= StartDate%>"/>
        <input type="hidden" id="hd_endDate" value="<%= EndDate%>"/>
 <input type="hidden" id="hd_name" value="<%= Request.Params["username"].ToString()%>"/>
     <div class="pages"></div>

    <div id="chartdiv" style="width: 100%; height: 510px;"></div>

</body>
</html>