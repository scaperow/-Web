<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="documentSummary.aspx.cs" Inherits="baseInfo_documentSummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript">

        $(function () {
            createChart(1);
        });

        function getGridOption() {
            var data = getSearchCondition('qxzlhzb', 1);
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["标段", "单位", "试验室", "试验名称", "录入总数", '时间段内录入数量', '不合格报告总数', '时间段内不合格数量', "testroomID", "modelID", "操作"],
                colModel: [
                {
                    name: 'segment', index: 'segment', width: 60, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'segment' + rowId + "\'";
                    }
                },
                {
                    name: 'company', index: 'company', width: 80, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'company' + rowId + "\'";
                    }
                },
                {
                    name: 'testroom', index: 'testroom', width: 80, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'testroom' + rowId + "\'";
                    }
                },
                { name: 'testname', index: 'testname', width: 160, sortable: false },
                { name: 'ncountA', index: 'ncountA', width: 60, align: 'center', sortable: false },
                { name: 'ncount', index: 'ncount', width: 60, align: 'center', sortable: false },
                { name: 'wncountA', index: 'wncountA', width: 60, align: 'center', sortable: false },
                { name: 'wncount', index: 'ncount', width: 60, align: 'center', sortable: false },
                 { name: 'testcode', index: 'testcode', width: 60, align: 'center', hidden: true, sortable: false },
                 { name: 'modelid', index: 'modelid', width: 60, align: 'center', hidden: true, sortable: false },
                   { name: 'act', index: 'act', width: 80, align: 'center', sortable: false }
                ],
                autowidth: false,
                shrinkToFit: true,
                jsonReader: {
                    page: "page",
                    total: "total",
                    repeatitems: false
                },
                pager: jQuery('#pager1'),
                rowNum: 14,
                rowList: [14, 30, 50, 100],
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
                    var ids = jQuery("#list1").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                       
                        be = "<span  style='color:#1c7c9c;cursor:pointer;' onclick=\"openPopbar('" + cl + "')\" >数量</span> <span  style='color:#1c7c9c;cursor:pointer;' onclick=\"openSearch('" + cl + "')\" >查看</span>";
                        jQuery("#list1").jqGrid('setRowData', ids[i], { act: be });
                    }
                }
            };
            return obj;
        }


        function openPopbar(data) {
            var row = $("#list1").getRowData(data);
            $.colorbox({
                href: "popjqgrid.aspx?testroomid=" + row["testcode"] + "&modelid=" + row["modelid"] + "&r=" + Math.random(),
                width: 800,
                height: 620,
                title: function () {
                    return "" + row["testroom"] + "- [" + row["testname"] + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '',
                iframe: true
            });
        }

        function openSearch(data) {
            var row = $("#list1").getRowData(data);
            $.colorbox({ 
                href: "documentSummarySearch.aspx?EndDate=" + $('#txt_endDate').val() + "&StartDate=" + $('#txt_startDate').val() + "&testroomid=" + row["testcode"] + "&modelid=" + row["modelid"] + "&r=" + Math.random(),
                width: '100%',
                height: '100%',
                title: function () {
                    return "" + row["testroom"] + "- [" + row["testname"] + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
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
            $("#list1").GridUnload();
            jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });
        }

        function createChart(n) {
            $(".chart_btn").removeClass("bagc_8584");
            $(".list_btn").addClass("bagc_8584");
            $("#divChart").show();
            $("#divList").hide();
            var data = getSearchCondition('qxzlhzbchart', n);
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
                    chart.addListener("clickGraphItem", handleClick);
                    chart.columnSpacing3D = 2;
                    chart.columnWidth = 0.5;
                    chart.equalSpacing = true;

                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
                    categoryAxis.parseDates = false;
                    categoryAxis.dashLength = 0.5;
                    categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#438eb9";
                    categoryAxis.labelRotation = 45;


                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#FEB23A";//左边
                    valueAxis.axisThickness = 2;
                    valueAxis.gridAlpha = 0;
                    valueAxis.title = "资料数量";
                    valueAxis.integersOnly = true;
                    chart.addValueAxis(valueAxis);



                    var graph = new AmCharts.AmGraph();
                    graph.valueAxis = valueAxis;
                    graph.valueField = "IntNumber";
                    graph.balloonText = "[[category]](资料总数): [[value]]";
                    graph.type = "column";
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 0.8;
                    graph.title = "资料总数";
                    graph.fillColors = "#438EB9";//柱状图
                    graph.width = "auto";
                    graph.labelText = "[[IntNumber]]";//设置直接显示数目
                    graph.labelPosition = "top";
                    graph.showHandOnHover = true;
                    chart.addGraph(graph);


                    chart.write("chartdiv");




                }
            });
        }

        function openPopChart(obj, data) {
            $.colorbox({
                href: "popBaseSummary.aspx?testcode=" + data.item.dataContext.Para1 + "&r=" + Math.random(),
                width: 1024,
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
                href: "popfailgrid.aspx?testcode=" + data.item.dataContext.Para1 + "&r=" + Math.random(),
                width: 800,
                height: 500,
                title: function () {
                    return "不合格试验数据分布图 [" + data.item.dataContext.Description + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '',
                iframe: true
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

        }

        function getSearchCondition(stype, isUnit) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sType = stype;
            data.isUnit = isUnit;
            return data;
        }

        function searchClick() {
            $('#hd_startDate').val($('#txt_startDate').val());
            $('#hd_endDate').val($('#txt_endDate').val());
            if ($("#divChart").css("display") == 'block') {
                createChart(1);
            }
            else {
                createGrid();
            }
        }

        function Export() {
            var Para = jQuery.param(getSearchCondition('QXZLTJ'));

            window.open("../Export/Export.aspx?Ajax=" + Math.random() + "&" + Para);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="piece">
        <div class="piece_con">

            <h2 class="title">
                <span class="left"><i></i>全线资料统计</span>

                <ul class="searchbar_01 clearfix">
                    <li class="right">
                        <input name="" type="button" class="list_btn" title="列表" onclick="createGrid()" /></li>
                    <li class="right">
                        <input name="" type="button" class="chart_btn" title="图表" onclick="createChart(1)" /></li>
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

