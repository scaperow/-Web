<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="stadiumLog.aspx.cs" Inherits="logInfo_stadiumLog" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript">


        $(function () {

            createGrid();

            $("#txt_startDate").datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#txt_endDate").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#txt_endDate").datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                numberOfMonths: 1,
                onClose: function (selectedDate) {
                    $("#txt_startDate").datepicker("option", "maxDate", selectedDate);
                }
            });
        });




        function getGridOption() {
            var data = getSearchCondition('stadiumLog');
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["标段", "单位", "试验室", "名称", '批号', '试件编号', '试件尺寸', '试验项目','报告编号','委托编号'],
                colModel: [

                { name: '标段名称', index: '标段名称', width: 60 },
                { name: '单位名称', index: '单位名称', width: 60, align: 'center' },
                { name: '试验室', index: '试验室', width: 80, align: 'center' },
                { name: '名称', index: '名称', width: 80, align: 'center' },
                { name: '批号', index: '批号', width: 60, align: 'center' },
                 { name: '试件编号', index: '试件编号', width: 120, align: 'center' },
                { name: '试件尺寸', index: '试件尺寸', width: 100, align: 'center' },
                { name: '试验项目', index: '试验项目', width: 120, align: 'center' },
                { name: '报告编号', index: '报告编号', width: 120, align: 'center' },
                 { name: '委托编号', index: '委托编号', width: 120, align: 'center' }

                ],
                autowidth: true,
                shrinkToFit: true,
                jsonReader: {
                    page: "page",
                    total: "total",
                    repeatitems: false
                },
                pager: jQuery('#pager1'),
                rowNum: 10,
                rowList: [10, 20, 30],
                sortname: '标段名称',
                sortorder: 'asc',
                viewrecords: true,
                height: 400,
                loadui: 'disable',
                postData: data
            };
            return obj;
        }

        function createGrid() {
            $("#divChart").hide();
            $("#divList").show();
            $("#list1").GridUnload();
            jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });


        }



        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sType = stype;
            return data;
        }

        function searchClick() {

            createGrid();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="background-color: #0094ff;">
        龄期提醒
        

       <div style="display: none;">
           起始时间：<input type="text" id="txt_startDate" value="<%= StartDate%>" />
           结束时间：<input type="text" id="txt_endDate" value="<%= EndDate%>" />

           <input type="button" value="查询" id="bt_search" onclick="searchClick()" />

       </div>
        

    </div>
    <div style="margin: 10px;">

       
        <div id="divList" style="width: 100%;">
            <table id="list1"></table>
            <div id="pager1"></div>
        </div>
    </div>
</asp:Content>


