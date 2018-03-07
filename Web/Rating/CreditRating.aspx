<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="CreditRating.aspx.cs" Inherits="Rating_CreditRating" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
     <script type="text/javascript">

         $(function () {
             createChart(1);
         });

         function getGridOption() {

             var data = getSearchCondition('List');
             var obj = {
                 url: '<%= ResolveUrl("CreditRating.aspx") %>',
                 datatype: "json",
                 mtype: 'POST',
                 colNames: ["SysID", "标段", "施工单位", "试验室", "单位性质", "评价类别", "姓名", "身份证号", "职务", "总扣分数", "评价时间", "评价人", "备注", "操作"],

                 colModel: [
                      { name: "SysID", index: "SysID", hidden: true },
                 {
                     name: '标段', index: '标段名称', width: 50, align: 'left', sortable: false,
                     cellattr: function (rowId, tv, rawObject, cm, rdata) {
                         return 'id=\'标段名称' + rowId + "\'";
                     }
                 },
                 {
                     name: '单位', index: '单位名称', width: 60, align: 'left', sortable: false,
                     cellattr: function (rowId, tv, rawObject, cm, rdata) {
                         return 'id=\'单位名称' + rowId + "\'";
                     }
                 },
                
                 {
                     name: '试验室', index: '试验室名称', width: 80, align: 'left', sortable: false,
                     cellattr: function (rowId, tv, rawObject, cm, rdata) {
                         return 'id=\'试验室名称' + rowId + "\'";
                     }
                 },
                  { name: '单位性质', index: '单位性质', width: 50, align: 'center', sortable: false },
                   { name: '评价类别', index: '评价类别', width: 50, align: 'center', sortable: false },
                 { name: '姓名', index: '姓名', width: 50, align: 'center', sortable: false },
                 { name: '身份证', index: '身份证号', width: 100, align: 'center', sortable: false },
                 { name: '职务', index: '职务', width: 80, align: 'center', sortable: false },
                 { name: '总扣分数', index: '总扣分数', width: 50, align: 'center', sortable: false },
                 { name: '评价时间', index: '评价时间', width: 100, align: 'center', sortable: false, },
                 { name: '评价人', index: '评价人', width: 50, align: 'center', sortable: false, },
                 { name: '备注', index: '备注', width: 150, align: 'center', sortable: false, },
                 { name: '操作', index: 'act_caiji', width: 60, align: 'left', sortable: false }
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

                     var gridName = "list1ALL";
                     Merger(gridName, '标段名称', '');
                     Merger(gridName, '单位名称', '标段名称');
                     Merger(gridName, '试验室名称', '单位名称');
                     adeptGridWidth();


                     var ids = jQuery("#list1").jqGrid('getDataIDs');
                     for (var i = 0; i < ids.length; i++) {
                         var cl = ids[i];
                         var rowData = jQuery("#list1").jqGrid('getRowData', ids[i]);

                         be = "&nbsp;&nbsp;<span  style='color:#1c7c9c;cursor:pointer;' onclick=\"create('" + rowData.SysID + "')\" >编辑</span> &nbsp;&nbsp;";
                         be1 = " <span  style='color:#1c7c9c;cursor:pointer;' onclick=\"Del('" + rowData.SysID + "')\" >删除</span> ";

                         jQuery("#list1").jqGrid('setRowData', ids[i], { 操作: be + be1 });
                         try{
                             jQuery("#list1").jqGrid('setRowData', ids[i], { 评价时间: rowData.评价时间.replace('T',' ') });
                         }
                         catch(ex){}

                     }


                 }
             };
             return obj;
         }


         function createGrid() {
             $(".chart_btn").addClass("bagc_8584");
             $(".list_btn").removeClass("bagc_8584");

             $("#divChart,#forchart").hide();

             $("#divList,#forlist").show();
             $("#list1").GridUnload();
             jqGrid = $("#list1").jqGrid(getGridOption());//.navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });
         }


         function getSearchCondition(stype) {
             var data = {};
             data.StartDate = $('#txt_startDate').val();
             data.EndDate = $('#txt_endDate').val();
             data.sType = stype;
             data.isUnit = 1;
             data.testroom = $('#testroom').val();
             data.name = $('#name').val();
             data.createdby = $('#createdby').val();
             data.deduct = $('#deduct').val();

             return data;
         }


         function Del(id)
         {
             if (confirm("是否删除此条评价")) {
                 $.post("CreditRating.aspx", { ACT: 'DEL', SysID: id }, function (d) {
                     if (d == 'true') {
                         alert('删除成功');
                         createGrid();
                     }
                     else {
                         alert('删除失败');
                     }
                 });
             }
         }

         function create(id)
         {
       
             $.colorbox({
                 href: "CreaditRatingEdit.aspx?ID="+id+"&AJAX"+ Math.random(),
                 width: 850,
                 height: 590,
                 title: function () {
                     return "新建-铁路建设项目信用评价";
                 },
                 close: '',
                 iframe: true
             });
         }

         function Criterion() {
             $.colorbox({
                 href: "RatingCriterion.aspx?AJAX" + Math.random(),
                 width: 850,
                 height: 590,
                 title: function () {
                     return "铁路建设项目信用评价标准";
                 },
                 close: '',
                 iframe: true
             });
         }
         function Export() {
             var Para = jQuery.param(getSearchCondition('XYPJ'));

             window.open("../Export/Export.aspx?Ajax=" + Math.random() + "&" + Para);
         }

         //////////////////////////////////////////////////////////////////////////

         function createChart(n) {
             $(".chart_btn").removeClass("bagc_8584");
             $(".list_btn").addClass("bagc_8584"); 
             $("#divChart").show();
             $('#forchart').show();
             $('#forlist').hide();

             $("#divList").hide();
             var data = getSearchCondition('CHART');
             $.ajax({
                 type: "POST",
                 dataType: "json",
                 url: "<%= ResolveUrl("CreditRating.aspx") %>?n="+n,
                data: data,
                success: function (msg) {


                    chart = new AmCharts.AmSerialChart();

                    chart.dataProvider = eval(msg);
                    chart.categoryField = "description";
                    //chart.startDuration = 1;
                    chart.angle = 30;
                    chart.depth3D = 15;

                    chart.columnSpacing3D = 1;
                    chart.columnWidth = 0.6;
                    chart.equalSpacing = true;
                    //chart.addListener("clickGraphItem", handleClick);
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
                    valueAxis.title = "扣分";
                    valueAxis.integersOnly = true;
                    chart.addValueAxis(valueAxis);



                    var graph = new AmCharts.AmGraph();
                    graph.valueAxis = valueAxis;
                    graph.valueField = "deduct";
                    graph.balloonText = "[[category]](总扣分): [[value]]";
                    graph.type = "column";
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 0.8;
                    graph.title = "资料总数";
                    graph.fillColors = "#438EB9";//柱状图
                    graph.width = "auto";
                    graph.labelText = "[[deduct]]";//设置直接显示数目
                    graph.labelPosition = "top";
                    graph.showHandOnHover = true;
                    chart.addGraph(graph);


                    chart.write("chartdiv");




                }
            });
        }




 
    </script>
    <style type="text/css">
        .e76 {
            color: #e76049;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="piece" style="overflow-x: hidden; overflow-y: auto">
        <div class="piece_con">


            <h2 class="title">
                <span class="left" > <i></i>铁路建设项目信用评价
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <a href="#" onclick="Criterion()" style="font-size:12px;">查看标准</a>
                    <span style="font-size:12px;display:none;" id="forlist" >
                       试验室： <input type="text" id="testroom"  /> 
                       姓名： <input type="text" id="name" size="10"/> 
                       评价人： <input type="text" id="createdby" size="10" /> 
                        详细:<input type="text" id="deduct" /> 
                    </span>
                    <span style="font-size:12px;" id="forchart">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<a href="#"  id="bysys" onclick="createChart(1);">按试验室排名</a>
                    &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a href="#"  id="byper" onclick="createChart(0);">按人员排名</a>
                    
                </span>
               
                </span>

                <ul class="searchbar_01 clearfix">
                    <li class="right">
                        <input name="" type="button" class="list_btn  bagc_8584" title="列表" onclick="createGrid()" /></li>
                      <li class="right">
                        <input name="" type="button" class="chart_btn" title="图表" onclick="createChart(1)" /></li>
                    <li class="right">
                        <input name="" type="button" class="export_btn" title="导出" onclick="Export()" /></li>
                    <li class="right">
                        <input name="" type="button" class="addnew" title="新建"  onclick="create('')" /></li>

                   
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

