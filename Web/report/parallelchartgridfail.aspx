<%@ Page Language="C#" AutoEventWireup="true" CodeFile="parallelchartgridfail.aspx.cs" Inherits="report_parallelchartgridfail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

      <link href="../css/default.css" rel="stylesheet" />
    <link href="../css/ui.jqgrid.css" rel="Stylesheet" />
    <link href="../css/themes/smoothness/jquery-ui-1.10.3.custom.min.css" rel="Stylesheet" />
 
  
 <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-1.9.0.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery-ui-1.10.3.custom.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.layout-latest.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/grid.locale-cn.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.jqGrid.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/ajax_loader.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/js/main.js") %>"></script>
    <script type="text/javascript">

        $(function () {
            var code = getQueryString('testroomid');
            var model = getQueryString('modelid');
            var sNuit = getQueryString('sNuit');

            $("#hid_testcode").val(code);
            $("#hid_modelid").val(model);
            $("#hid_snuit").val(sNuit);
            createGrid();
        });

        function getGridOption() {
            var data = getSearchCondition("parallelchartgridfail");
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'GET',
                colNames: ["试验名称", "标准平行频率(%)", "施工单位资料总数", "平行次数", "平行频率(%)", "平行频率质量系数", "是否满足要求", "testroomID", "modelID"],
                colModel: [
                { name: 'modelName', index: 'modelName', width: 200, sortable: false },
                { name: 'condition', index: 'condition', width: 130, sortable: false, align: 'center' },
                 { name: 'zjCount', index: 'zjCount', width: 100, sortable: false, align: 'center' },
                  { name: 'pxCount', index: 'pxCount', width: 60, sortable: false, align: 'center' },
                   { name: 'frequency', index: 'frequency', width: 100, sortable: false, align: 'center' },
                     { name: 'pxqulifty', index: 'pxqulifty', width: 120, sortable: false, align: 'center' },
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
                rowNum: 30,
                rowList: [30, 50, 100],
                sortname: 'modelName',
                sortorder: 'asc',
                viewrecords: true,
                height: '100%',
                postData: data,
                gridComplete: function () {
                    var gridName = "list1";
                    //Merger(gridName, 'segment', '');
                    //Merger(gridName, 'jl', 'segment');
                    //Merger(gridName, 'sg', 'jl');
                    //Merger(gridName, 'testroom', 'sg');
                }
                //,
                //onSelectRow: function (rowid) {
                //    openPopbar(this, $("#list1").getRowData(rowid));
                //}
            };

            return obj;
        }



        function createGrid() {
            $("#list1").GridUnload();
            jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });

        }


        function getSearchCondition(stype) {
            var data = {};
            data.sTestcode = $('#hid_testcode').val();
            data.sModelID = $('#hid_modelid').val();
            data.sNuit = $('#hid_snuit').val();
            data.sType = stype;
            return data;
        }


        </script>
</head>
<body>
    <form id="form1" runat="server">
          <input type="hidden" id="hid_testcode" />
        <input type="hidden" id="hid_modelid" />
         <input type="hidden" id="hid_snuit" />
       

<div id="divList" style="width: 100%; padding-top:20px; min-height:380px;">
        <table id="list1"></table>
        <div id="pager1"></div>
    </div>
    </form>
</body>
</html>
