<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AgeremindUndo.aspx.cs" Inherits="report_AgeremindUndo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        $(function () {
            createGrid();

        });
        function getGridOption() {
            var data = getSearchCondition('ageremindundo');
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["标段", "单位", "试验室", "名称", '批号', '试件编号', '试件尺寸', '试验项目', '报告编号', '委托编号', '制件日期'],
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
                { name: 'F_Name', index: 'F_Name', width: 80, align: 'center', sortable: false },
                { name: 'F_PH', index: 'F_PH', width: 80, align: 'center', sortable: false },
                { name: 'F_SJBH', index: 'F_SJBH', width: 60, align: 'center', sortable: false },
                { name: 'F_SJSize', index: 'F_SJSize', width: 60, align: 'center', sortable: false },
                { name: 'F_SYXM', index: 'F_SYXM', width: 80, align: 'center', sortable: false },
                { name: 'F_BGBH', index: 'F_BGBH', width: 80, align: 'center', sortable: false },
                { name: 'F_WTBH', index: 'F_WTBH', width: 80, align: 'center', sortable: false },
                { name: 'F_ZJRQ', index: 'F_ZJRQ', width: 80, align: 'center', sortable: false, formatter: "date", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' } }
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


    <div class="piece">
        <div class="piece_con">
            <h2 class="title">
                <span class="left"><i></i>龄期过期未做提醒</span>

                <ul class="searchbar_01 clearfix">
                  
                    <li class="right">
                        <input name="" type="button" class="list_btn" title="列表" onclick="createGrid()" /></li>
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


