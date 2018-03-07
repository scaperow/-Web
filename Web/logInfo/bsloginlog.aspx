<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="bsloginlog.aspx.cs" Inherits="logInfo_bsloginlog" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript">

        var tongChart;
        var pageCount, currPage = 1;
        $(function () {
            creatchart();
        });
        function creatchart() {
            $(".chart_btn").removeClass("bagc_8584");         
            $("#divChart").show();
            var data = getSearchCondition('bsloginlog');
            if (tongChart != undefined && tongChart != null) {
                tongChart.clear();
            }
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx") %>",
                data: data,
                success: function (msg) { 
                    // SERIAL CHART    
                    var json = eval(msg);
                    pageCount = json.pageCount;
                    getPageTemp(currPage);
                    tongChart = new AmCharts.AmSerialChart();
                    tongChart.dataProvider = json.Data;

                    tongChart.categoryField = "UserName";
                    //tongChart.startDuration = 1;
                    tongChart.angle = 30;
                    tongChart.depth3D = 15;

                    tongChart.columnSpacing3D = 1;
                    tongChart.columnWidth = 0.6;
                    tongChart.equalSpacing = true;

                    tongChart.addListener("clickGraphItem", handleClick);


                    // AXES
                    // category
                    var categoryAxis = tongChart.categoryAxis;
                    categoryAxis.parseDates = false;
                    categoryAxis.dashLength = 0.5;
                    categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#438eb9";
                    //categoryAxis.title = "单位";
                    categoryAxis.labelRotation = 45;


                    //// value                
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.dashLength = 1;
                    valueAxis.axisThickness = 2;
                    valueAxis.axisColor = "#E76049";//左边
                    valueAxis.integersOnly = true;
                    valueAxis.title = "登录次数";
                    tongChart.addValueAxis(valueAxis);
 

                  
                    var graph1 = new AmCharts.AmGraph();
                    graph1.valueAxis = valueAxis;
                    graph1.valueField = "counts";
                    graph1.balloonText = "[[category]](登录次数): [[value]]";
                    graph1.type = "line";
                    graph1.bullet = "round";
                    graph1.lineColor = "#E76049";//线条
                    graph1.labelText = "[[counts]]";
                    graph1.title = "登录次数";
                    graph1.labelPosition = "left";
                    graph1.lineThickness = 2;
                    graph1.showHandOnHover = true;
                    tongChart.addGraph(graph1);

                    // WRITE
                    tongChart.write("chartdiv");

                }
            });
        }

      
        function handleClick(e) {
            openPopChart(this, e);
        }

        function openPopChart(obj, data) {
            $.colorbox({
                href: "bsloginpop.aspx?username=" + encodeURI(data.item.dataContext.UserName),
                width: 1000,
                height: 650,
                title: " [" + data.item.dataContext.UserName + "]-登录日志-[" + $('#txt_startDate').val()
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
                if (i == currPage) pageTemp += "<a rel=\"page\" onclick=\"pages.Page(this)\" class=\"selected\">"+i+"</a>";
                else pageTemp += "<a rel=\"page\" onclick=\"pages.Page(this)\">"+i+"</a>";
            }
            pageTemp += after;
            pageTemp += PageAfter;
            pageTemp += end;
            $("div.pages").html(pageTemp);
        }

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="piece">
        <div class="piece_con">


            <h2 class="title">
                <span class="left"><i></i>网页版登录日志</span>

                <ul class="searchbar_01 clearfix">                  
                     <li class="right"><input name="" type="button" class="chart_btn" title="图表" onclick="creatchart()" /></li>
                </ul>
            </h2>

            <div class="content">
                 <div class="pages"></div>
                  <div id="divChart">
        <div id="chartdiv"></div>
    </div>
  
            </div>
        </div>
    </div>

</asp:Content>
