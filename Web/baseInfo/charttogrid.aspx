<%@ Page Language="C#" AutoEventWireup="true" CodeFile="charttogrid.aspx.cs" Inherits="baseInfo_charttogrid" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            createGrid();           
        });

        function getGridOption() {
            var code = getQueryString('testroomid');
            var modeid = getQueryString('modelid');
            $("#hid_testcode").val(code);
            $("#hid_modelid").val(modeid);         
            var data = getSearchCondition("qxzlhzbcharttogrid");
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["试验室","报告编号","委托编号", "报告日期"],
                colModel: [   
                {
                    name: 'testroom', index: 'testroom', width: 60, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'testroom' + rowId + "\'";
                    }
                },
                { name: 'BGBH', index: 'BGBH', width: 80, align: 'center', sortable: false },
                 { name: 'WTBH', index: 'WTBH', width: 80, align: 'center', sortable: false },              
                 { name: 'BGRQ', index: 'BGRQ', width: 60, align: 'center', sortable: false, formatter: "date", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d' } }
                ],
                autowidth: true,
                shrinkToFit: true,
                jsonReader: {
                    page: "page",
                    total: "total",
                    repeatitems: false
                },
                pager: jQuery('#pager1'),
                rowNum: 12,
                rowList: [12, 20, 30],
                sortname: 'SCTS',
                sortorder: 'asc',
                viewrecords: true,
                height: 325,
                loadui: 'disable',
                postData: data,
                gridComplete: function () {
                    var gridName = "list1";
                    Merger(gridName, 'testroom', '');
                }
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
            data.sType = stype;
            return data;
        }
   

        </script>
</head>
<body>
    <form id="form1" runat="server">
          <input type="hidden" id="hid_testcode" />
        <input type="hidden" id="hid_modelid" />
       

<div id="divList" style="width: 100%; padding-top:20px; min-height:380px;">
        <table id="list1"></table>
        <div id="pager1"></div>
    </div>
    </form>
</body>
</html>
