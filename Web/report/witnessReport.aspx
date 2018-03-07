<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="witnessReport.aspx.cs" Inherits="report_witnessReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript">

         $(function () {
             createGrid();
         });

         function getGridOption() {
             var data = getSearchCondition('jzreport');
             var obj = {
                 url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'GET',
                colNames: ["标段", "监理单位", "施工单位", "施工单位试验室", "试验名称", "标准见证频率(%)", "施工单位资料总数", "见证次数", "见证频率(%)", "是否满足要求", "testroomID", "modelID"],
                colModel: [
                {name: 'segment', index: 'segment', width: 80, sortable: false                },
                {name: 'jl', index: 'jl', width: 100, sortable: false                },
                {
                    name: 'sg', index: 'sg', width: 100, sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'sg' + rowId + "\'";
                    }
                },
                {
                    name: 'testroom', index: 'testroom', width: 110, sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'testroom' + rowId + "\'";
                    }
                },
                { name: 'modelName', index: 'modelName', width: 200, sortable: false },
                { name: 'condition', index: 'condition', width: 130, sortable: false, align: 'center' },
                 { name: 'zjCount', index: 'zjCount', width: 100, sortable: false, align: 'center' },
                  { name: 'pxCount', index: 'pxCount', width: 60, sortable: false, align: 'center' },
                   { name: 'frequency', index: 'frequency', width: 100, sortable: false, align: 'center' },
                     { name: 'result', index: 'result', width: 90, sortable: false, align: 'center' },
                     { name: 'testroomID', index: 'testroomID', width: 5, hidden: true },
                     { name: 'modelID', index: 'modelID', width: 5, hidden: true }
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
                postData: data,
                gridComplete: function () {
                    var gridName = "list1";
                    var ids = jQuery("#list1").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var id = ids[i];
                        var inlineGrid = jQuery("#list1");
                        if (inlineGrid.jqGrid('getCell', id, 'result') == "不满足") {
                            $("#" + id).find("td").css("background-color", "pink");
                        }
                    }
                }               
            };

            return obj;
        }

         function createGrid() {
            $("#list1").GridUnload();
            jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });
        }

        function openPopChart(obj, data) {
            $("#bt_hide").click();
            $("#bt_hide").colorbox({
                href: "popBaseSummary.aspx?testcode=" + data.item.dataContext.testcode,
                width: 800,
                height: 611,
                title: data.item.dataContext.testroom,
                close: '×',
                data: { dataid: 'abc' },
                iframe: true
            });
        }

        function openPopbar(obj, data) {
            var testroomid = data["testroomID"];
            var modelid = data["modelID"];

            $.colorbox({
                href: "popParallelReport.aspx?testroomid=" + data["testroomID"] + "&modelid=" + data["modelID"] + "&r=" + Math.random(),
                width: 800,
                height: 620,
                title: function () {
                    return "平行频率- [" + data["modelName"] + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '×',
                iframe: true
            });
        }

        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sType = stype;
            data.meet = $(':radio[name="meet"]:checked').val();
            return data;
        }

        function searchClick() {
            createGrid();
        }
        function Export() {
            var Para = jQuery.param(getSearchCondition('JZPLFX'));

            window.open("../Export/Export.aspx?sType=JZPLFX&Ajax=" + Math.random() + "&" + Para);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="piece">
    	<div class="piece_con">
         <h2 class="title">
                    <span class="left"><i></i>见证频率分析
               <input name="meet" type="radio" value="2" />满足
                 <input name="meet" type="radio" value="1"  checked="checked" />不满足
                 <input name="meet" type="radio" value="0"/>所有</span>
             <ul class="searchbar_01 clearfix">
             <li class="right"><input name="" type="button" class="list_btn" title="列表" onclick="createGrid()"  /></li>
                     <li class="right">
                        <input name="" type="button" class="export_btn" title="导出" onclick="Export()" /></li>
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


