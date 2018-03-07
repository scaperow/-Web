<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="MachineSummary.aspx.cs" Inherits="baseInfo_MachineSummary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <style>
        .showImg {
            color:#0094ff !important;
        }
    </style>
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
                createChart(1);
            }
        });



        function getGridOption() {
            var data = getSearchCondition('machinesummary');
            var obj = {
                url: '<%= ResolveUrl("MachineSummary.aspx?Act=List") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["标段", "单位", "试验室", "管理编号", "设备名称", '生产厂家', '规格型号', '数量', '检定情况', '检定证书编号', '上次校验日期', '预计下次校验日期', '检定周期', '是否过期','id'],
                colModel: [
                {
                    name: '标段名称', index: '标段名称', width: 40, align: 'center', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'标段名称' + rowId + "\'";
                    }
                },
                {
                    name: '单位名称', index: '单位名称', width: 40, align: 'center', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'单位名称' + rowId + "\'";
                    }
                },
                {
                    name: '试验室名称', index: '试验室名称', width: 40, align: 'center', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'试验室名称' + rowId + "\'";
                    }
                },
                { name: '管理编号', index: '管理编号', sortable: false, width: 60, align: 'center' },
                { name: '设备名称', index: '设备名称', width: 100, sortable: false, align: 'center' },
                { name: '生产厂家', index: '生产厂家', width: 120, sortable: false, align: 'center' },
                { name: '规格型号', index: '规格型号', width: 80, sortable: false, align: 'center' },
                { name: '数量', index: '数量', width: 40, sortable: false, align: 'center' },
                { name: '检定情况', index: '检定情况', width: 50, sortable: false, align: 'center' },
                { name: '检定证书编号', index: '检定证书编号', width: 100, sortable: false, align: 'center' },
                { name: '上次校验日期', index: '上次校验日期', width: 80, sortable: false, align: 'center' },
                { name: '预计下次校验日期', index: '预计下次校验日期', width: 100, sortable: false, align: 'center' },
                { name: '检定周期', index: '检定周期', width: 50, sortable: false, align: 'center' },
                { name: '是否过期', index: '预计下次校验日期', width: 50, sortable: false, align: 'center' },
                { name: 'id', index: 'id', width: 50, sortable: false, align: 'center', hidden:true }
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
                sortname: '标段名称',
                sortorder: 'asc',
                viewrecords: true,
                height: '100%',
                loadui: 'disable',
                postData: data,
                gridComplete: function () {
                    var gridName = "list1";
                    Merger(gridName, '标段名称', '');
                    Merger(gridName, '单位名称', '标段名称');
                    Merger(gridName, '试验室名称', '单位名称');
                    adeptGridWidth();

                    var ids = jQuery("#list1").jqGrid('getDataIDs');

                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];

                        var rowData = jQuery("#list1").jqGrid('getRowData', ids[i]);
                        var d = Date.now() - new Date(rowData.预计下次校验日期);


                        be = d > 0 ? "<font color='red'>是</font>" : "否";
                        var Td = new Date(rowData.上次校验日期).toLocaleDateString();
                        Td = Td == 'Invalid Date' ? '' : Td;

                        jQuery("#list1").jqGrid('setRowData', ids[i], { 是否过期: be });
                        jQuery("#list1").jqGrid('setRowData', ids[i], { 上次校验日期: Td });

                        Td = new Date(rowData.预计下次校验日期).toLocaleDateString();
                        Td = Td == 'Invalid Date' ? '' : Td;
                        jQuery("#list1").jqGrid('setRowData', ids[i], { 预计下次校验日期: Td });

                        jQuery("#list1").jqGrid('setRowData', ids[i], { 设备名称: '<a class="showImg" onclick="ShowImg(\'' + rowData.id + '\',\'I14\',\'' + rowData.设备名称 + '\');">' + rowData.设备名称 + '</a>' });
                        if (rowData.检定证书编号 != '') {
                            jQuery("#list1").jqGrid('setRowData', ids[i], { 检定证书编号: '<a class="showImg"  onclick="ShowImg(\'' + rowData.id + '\',\'A14\',\'' + rowData.设备名称 + '-[' + rowData.检定证书编号 + ']\');">' + rowData.检定证书编号 + '</a>' });
                        }
                    }


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

        function ShowImg(id, i,t)
        {
            $.colorbox({
                href: "MachineImg.aspx?ID=" +id +"&AI="+i+"&r=" + Math.random(),
              
                title: function () {
                    return t;
                }, close: ' ' 
            });
        }


        function createChart(n) {
            $(".chart_btn").removeClass("bagc_8584");
            $(".list_btn").addClass("bagc_8584");
            $("#divChart").show();
            $("#divList").hide();
            var data = getSearchCondition('machinesummary', n);
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "<%= ResolveUrl("MachineSummary.aspx?Act=Chart") %>",
                data: data,
                success: function (msg) {


                    // SERIAL CHART    
                    chart = new AmCharts.AmSerialChart();
                    chart.dataProvider = eval(msg);
                    chart.categoryField = "Description";
                    //chart.startDuration = 1;
                    chart.angle = 30;
                    chart.depth3D = 15;
                    chart.addListener("clickGraphItem", handleClick);
                    chart.columnSpacing3D = 1;
                    chart.columnWidth = 0.6;
                    chart.equalSpacing = true;

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
                    valueAxis.dashLength = 1;
                    //valueAxis.logarithmic = true; // this line makes axis logarithmic
                    valueAxis.title = "设备数量";
                    valueAxis.integersOnly = true;
                    chart.addValueAxis(valueAxis);


                    var graph = new AmCharts.AmGraph();
                    graph.valueField = "IntNumber";
                    graph.balloonText = "[[category]]: [[value]]";
                    graph.labelText = "[[IntNumber]]";//设置直接显示数目
                    graph.type = "column";
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 0.8;
                    graph.fillColors = "#438eb9";
                    graph.title = "已检";
                    graph.showHandOnHover = true;
                    chart.addGraph(graph);


                    var graph1 = new AmCharts.AmGraph();
                    graph1.valueField = "IntNumberMarks";
                    graph1.balloonText = "过检 [[category]]: [[value]]";
                    graph1.labelText = "[[IntNumberMarks]]";
                    graph1.type = "column";
                    graph1.lineAlpha = 0;
                    graph1.fillAlphas = 0.8;
                    graph1.fillColors = "#E76049";
                    graph1.title = "过检";
                    graph1.showHandOnHover = true;
                    chart.addGraph(graph1);
                    // WRITE

                    var legend = new AmCharts.AmLegend();//设置图例
                    legend.align = "left";
                    chart.addLegend(legend);
                    // WRITE


                    chart.write("chartdiv");

                }
            });
        }

        function openPopChart(obj, data) {
            $.colorbox({
                href: "popmachineSummary.aspx?testcode=" + data.item.dataContext.Para1,
                width: 1024,
                height: 620,
                title: "[" + data.item.dataContext.Description + "]-设备情况/记录数(条)",
                close: '×',
                iframe: true
            });
        }

        function handleClick(e) {
            openPopChart(this, e);
        }

        function getSearchCondition(stype, isUnit) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sType = stype;
            data.isUnit = isUnit;
            data.RPNAME = '<%="RPNAME".RequestStr()%>';
            data.NUM = '<%="NUM".RequestStr()%>';
            data.meet = $(':radio[name="meet"]:checked').val();
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
            var Para = jQuery.param(getSearchCondition('SBQK'));

            window.open("../Export/Export.aspx?sType=SBQK&Ajax=" + Math.random() + "&" + Para);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 



    <div class="piece">
    	<div class="piece_con">
        	   <h2 class="title">
                    <span class="left"><i></i>设备情况

                        <input name="meet"  type="radio" value="0" <%=("RPName".RequestStr() == "ALL" || !"RPName".IsRequest() ? "checked=\"checked\"" : "") %> />全部
                        <input name="meet"  type="radio" value="1"  <%=("RPName".RequestStr() == "DBD" ? "checked=\"checked\"" : "") %> />过检
                        <input name="meet"  type="radio" value="2" />未过检
                    </span>

                <ul class="searchbar_01 clearfix">
             <li class="right"><input name="" type="button" class="list_btn" title="列表" onclick="createGrid()"  /></li>
                    <li class="right"><input name="" type="button" class="chart_btn" title="图表" onclick="createChart(1)" /></li>
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
