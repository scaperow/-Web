<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="operateLog.aspx.cs" Inherits="logInfo_operateLog" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript">
       
        $(function () {
           
            createGrid();
         
        });




        function getGridOption() {
            var data = getSearchCondition('operateLog');
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["用户", "标段", "单位", "试验室", "操作日期", '操作类型', '模板'],
                colModel: [
                { name: 'modifiedby', index: 'modifiedby', width: 60, align: 'center', sortable: false },
                { name: 'segmentName', index: 'segmentName', width: 60, align: 'center', sortable: false },
                { name: 'companyName', index: 'companyName', width: 60, align: 'center', sortable: false },
                { name: 'testRoom', index: 'testRoom', width: 80, align: 'center', sortable: false },
                { name: 'modifiedDate', index: 'modifiedDate', width: 80, align: 'center', sortable: false, formatter: "date", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' } },
                { name: 'optType', index: 'optType', width: 60, align: 'center', sortable: false },
                 { name: 'modelName', index: 'modelName', width: 120, align: 'center', sortable: false }
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
                sortname: 'modifiedDate',
                sortorder: 'asc',
                viewrecords: true,
                height: '100%',
                loadui: 'disable',
                postData: data,
                gridComplete: function () {
                    adeptGridWidth();
                }
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
            data.username = $('#txt_username').val();
            return data;
        }

        function searchClick() {

            createGrid();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 

    <div class="piece">
    	<div class="piece_con">
        	  <h2 class="title">
                <span class="left"><i></i>客户端修改日志</span>

                <ul class="searchbar_01 clearfix">
                    <li><span class="text">用户名称</span><input type="text" id="txt_username" /></li>
                    <li class="right"><input name="" type="button" class="list_btn" title="列表" onclick="createGrid()"  /></li>
                </ul>
            </h2>

            <div class="content">

              
    <div id="divList">
        <table id="list1"></table>
        <div id="pager1"></div>
    </div>
            </div>
        </div>
    </div>
</asp:Content>

