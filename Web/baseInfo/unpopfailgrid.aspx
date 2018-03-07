<%@ Page Language="C#" AutoEventWireup="true" CodeFile="unpopfailgrid.aspx.cs" Inherits="baseInfo_unpopfailgrid" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
           <link href="../css/colorbox.css" rel="Stylesheet" />
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
         <script type="text/javascript" src="<%= ResolveUrl("~/js/jquery.colorbox-min.js") %>"></script>
    <script type="text/javascript">


        $(function () {
            createGrid();
        });

        function getGridOption() {
            var code = getQueryString('testcode');
            $("#hid_testcode").val(code);
            var data = getSearchCondition("unpopfailgrid");
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["试验室", "报告名称", "报告编号", "报告日期", "不合格项", 'id', '操作'],
                colModel: [
                {
                    name: 'TestRoomName', index: 'TestRoomName', width: 40, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'TestRoomName' + rowId + "\'";
                    }
                },
                   { name: 'ReportName', index: 'ReportName', width: 60, align: 'center', sortable: false },
                      { name: 'ReportNumber', index: 'ReportNumber', width: 60, align: 'center', sortable: false },
                 { name: 'ReportDate', index: 'ReportDate', width: 60, align: 'center', sortable: false, formatter: "date", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' } },
{ name: 'F_InvalidItem', index: 'F_InvalidItem', width: 60, align: 'center', sortable: false },
 { name: 'IndexID', index: 'IndexID', width: 100, align: 'center', sortable: false, hidden: true },
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
                rowNum: 10,
                rowList: [10, 20, 30],
                sortname: 'SCTS',
                sortorder: 'asc',
                viewrecords: true,
                height: 325,
                loadui: 'disable',
                postData: data,
                gridComplete: function () {
                    var gridName = "list1";
                    Merger(gridName, 'TestRoomName', '');
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
                href: "../report/onefailreport.aspx?id=" + row["IndexID"] + "&r=" + Math.random(),
                width: 850,
                height: 400,
                title: function () {
                    return "[" + row["TestRoomName"] + "-" + row["ReportName"] + "]";
                    //return "不合格报告明细";
                },
                close: '',
                iframe: true
            });
        }

        function createGrid() {
            $("#list1").GridUnload();
            jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });
        }

        function getSearchCondition(stype) {
            var data = {};
            data.sTestcode = $('#hid_testcode').val();
            data.sType = stype;
            return data;
        }

        </script>
</head>
<body>
    <form id="form1" runat="server">

        

          <input type="hidden" id="hid_testcode" />
<div id="divList" style="width: 100%; padding-top:20px;">
        <table id="list1"></table>
        <div id="pager1"></div>
    </div>
    </form>
</body>
</html>

