<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="userSummary.aspx.cs" Inherits="baseInfo_userSummary" %>


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
            var data = getSearchCondition('usersummary');
            var obj = {
                url: '<%= ResolveUrl("userSummary.aspx?Act=List") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["标段", "施工单位", "中心及分布", "姓名", '性别', '年龄', '职称', '职务', '工作年限', '联系方式', '学历', '毕业院校', '所学专业', 'id', "操作"],
                colModel: [
                {
                    name: '标段名称', index: '标段名称', width: 40, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'标段名称' + rowId + "\'";
                    }
                },
                {
                    name: '单位名称', index: '单位名称', width: 40, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'单位名称' + rowId + "\'";
                    }
                },
                {
                    name: '试验室名称', index: '试验室名称', width: 40, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'试验室名称' + rowId + "\'";
                    }
                },
                { name: '姓名', index: '姓名', width: 60, align: 'center', sortable: false },
                { name: '性别', index: '性别', width: 40, align: 'center', sortable: false },
                { name: '年龄', index: '年龄', width: 40, align: 'center', sortable: false },
                { name: '技术职称', index: '技术职称', width: 80, align: 'center', sortable: false },
                { name: '职务', index: '职务', width: 100, align: 'center', sortable: false },
                { name: '工作年限', index: '工作年限', width: 60, align: 'center', sortable: false },
                { name: '联系电话', index: '联系电话', width: 100, align: 'center', sortable: false },
                { name: '学历', index: '学历', width: 60, align: 'center', sortable: false  },
                  { name: '毕业学校', index: '毕业学校', width: 100, align: 'center', sortable: false },
                    { name: '专业', index: '专业', width: 100, align: 'center', sortable: false },
                    { name: 'ID', index: 'userid', width: 100, align: 'center', sortable: false, hidden: true },
                     { name: 'act', index: 'act', width: 80, align: 'center', sortable: false }
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
                        be = "<span  style='color:#1c7c9c;cursor:pointer;' onclick=\"openPopbar('" + cl + "')\" >查看</span>";
                        jQuery("#list1").jqGrid('setRowData', ids[i], { act: be });
                    }
                }
            };
            return obj;
        }

        function openPopbar(data) {
            var row = $("#list1").getRowData(data);
            $.colorbox({
                href: "oneuser.aspx?id=" + row["ID"] + "&r=" + Math.random(),
                width: 980,
                height: 620,
                title: function () {
                    return "" + row["标段名称"] + "-" + row["单位名称"] + "-" + row["试验室名称"] + "-试验人员技术档案[" + row["姓名"] + "]";
                },
                close: '',
                iframe: true
            });
        }

        function createGrid() {
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");
            $("#divChart").hide();
            $("#divListALL").hide();
            $("#divList").show();
            $("#list1").GridUnload();
            jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });
        }


        function createChart() {
            $(".chart_btn").removeClass("bagc_8584");
            $(".list_btn").addClass("bagc_8584");
            $("#divChart").show();
            $("#divList").hide();
            $("#divListALL").hide();
            var data = getSearchCondition('usersummary');
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "<%= ResolveUrl("~/ajax/ajaxChart.aspx") %>",
                data: data,
                success: function (msg) {


                    // SERIAL CHART    
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


                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
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
                    valueAxis.axisColor = "#438eb9";//左边
                    valueAxis.integersOnly = true;
                    //valueAxis.logarithmic = true; // this line makes axis logarithmic
                    valueAxis.title = "人数";
                    chart.addValueAxis(valueAxis);


                    var graph = new AmCharts.AmGraph();
                    graph.valueField = "IntNumber";
                    graph.balloonText = "[[category]]: [[value]]";
                    graph.labelText = "[[IntNumber]]";
                    graph.type = "column";
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 0.8;
                    graph.fillColors = "#438eb9";
                    graph.showHandOnHover = true;
                    chart.addGraph(graph);

                    // WRITE
                    chart.write("chartdiv");

                }
            });
        }

        function openPopChart(obj, data) {
            $.colorbox({
                href: "popuserSummary.aspx?testcode=" + data.item.dataContext.Para1,
                width: 1000,
                height: 620,
                title: "中心及分部人员技术档案 [" + data.item.dataContext.Description + "]",
                close: '',
                iframe: true
            });
        }

        function handleClick(e) {
            openPopChart(this, e);
        }

        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sType = stype;
            data.RPNAME = '<%="RPNAME".RequestStr()%>';
            data.NUM = '<%="NUM".RequestStr()%>';
            data.zglg = $(':radio[name="zglg"]:checked').val();
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
            var Para = jQuery.param(getSearchCondition('RYQK'));

            window.open("../Export/Export.aspx?sType=RYQK&Ajax=" + Math.random() + "&" + Para);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">






    <div class="piece" style="overflow-x: hidden; overflow-y: auto">
        <div class="piece_con">


            <h2 class="title">
                  <span class="left"><i></i>人员情况

                  
                        <input name="zglg"  type="radio" value="1" checked  />在岗
                        <input name="zglg"  type="radio" value="0" />离岗
                    </span>

                <ul class="searchbar_01 clearfix">
                    <li class="right">
                        <input name="" type="button" class="list_btn" title="列表" onclick="createGrid()" /></li>
                    <%-- <li class="right">
                        <input name="" type="button" class="tj" title="概况" onclick="createGridALL()" /></li>--%>
                    <li class="right">
                        <input name="" type="button" class="chart_btn" title="图表" onclick="createChart()" /></li>
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
                 <div id="divListALL">
                    <table id="list1ALL"></table>
           
                </div>
            </div>
        </div>
    </div>


</asp:Content>


