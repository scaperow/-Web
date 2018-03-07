<%@ Page Language="C#" AutoEventWireup="true" CodeFile="documentSummarySearch.aspx.cs" Inherits="baseInfo_documentSummarySearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="icon" href="../favicon.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="../favicon.ico" type="image/x-icon" />
    <link href="../css/initialize.css" rel="stylesheet" type="text/css" />
    <link href="../css/css.css" rel="stylesheet" />
    <link href="../css/ui.jqgrid.css" rel="Stylesheet" />
    <link href="../css/colorbox.css" rel="Stylesheet" />
    <link href="../css/themes/smoothness/jquery-ui-1.10.3.custom.min.css" rel="Stylesheet" />

    <script type="text/javascript" src="../js/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.10.3.custom.min.js"></script>
    <script type="text/javascript" src="../js/jquery.ui.datepicker-zh-CN.js"></script>
    <script type="text/javascript" src="../js/grid.locale-cn.js"></script>
    <script type="text/javascript" src="../js/jquery.jqGrid.min.js"></script>
    <script type="text/javascript" src="../js/jquery.colorbox-min.js"></script>
    <script type="text/javascript" src="../js/ajax_loader.js"></script>
    <script type="text/javascript" src="../js/main.js"></script>
    <script type="text/javascript" src="../js/ajaxLoaderBasePage.js"></script>


   

</head>
<body>
<table id="list1"></table>
<div id="pager1"></div>
</body>
</html>
<script type="text/javascript">

    $(function () {
        jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });
    });
    function getGridOption() {
   
        var obj = {

            url: '<%= ResolveUrl("documentSummarySearch.aspx?Act=List") %>',
            datatype: "json",
            mtype: 'POST',
            colNames: ["id", "标段", "单位", "试验室", "模板名称", "委托编号", '报告编号', '报告日期', '详情/采集信息', ''],
            colModel: [
           { name: "ID", index: "ID", hidden: true },
           { name: 'SegmentName', index: 'Segmentcode', width: 50, align: 'center', sortable: false },
           { name: 'CompanyName', index: 'CompanyCode', width: 50, align: 'center', sortable: false },
           { name: 'TestRoomName', index: 'TestRoomCode', width: 50, align: 'center', sortable: false },
           { name: 'MName', index: 'MName', align: 'center', sortable: false },
           { name: 'WTBH', index: 'WTBH', width: 60, align: 'center', sortable: false, hidden: true },
           { name: 'BGBH', index: 'BGBH', align: 'center', sortable: false },
           { name: 'BGRQ', index: 'BGRQ', width: 50, align: 'center', sortable: false },
           { name: 'act_caiji', index: 'act_caiji', width: 60, align: 'left', sortable: false },
           { name: 'DeviceType', index: 'DeviceType', width: 80, align: 'center', sortable: false, hidden: true }


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
            sortname: 'CreatedTime',
            sortorder: 'asc',
            viewrecords: true,
            height: '100%',
            loadui: 'disable',
            postData: {
                testroomid: "<%="testroomid".RequestStr()%>",
                modelid: "<%="modelid".RequestStr()%>",
                EndDate: "<%="EndDate".RequestStr()%>",
                StartDate: "<%="StartDate".RequestStr()%>",
                modelid: "<%="modelid".RequestStr()%>"
            },
            gridComplete: function () {

                //adeptGridWidth();
                var ids = jQuery("#list1").jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var cl = ids[i];
                    var rowData = jQuery("#list1").jqGrid('getRowData', ids[i]);

                    be = "&nbsp;&nbsp;&nbsp;&nbsp;<span  style='color:#1c7c9c;cursor:pointer;' onclick=\"openPopbar('" + cl + "')\" >详情</span> &nbsp;&nbsp;&nbsp;&nbsp;";
                    be1 = " <span  style='color:#1c7c9c;cursor:pointer;' onclick=\"openPopbar1('" + cl + "'," + rowData.DeviceType + ")\" >采集信息</span> ";

                    be1 = rowData.DeviceType == 0 ? "" : be1;
                    jQuery("#list1").jqGrid('setRowData', ids[i], { act_caiji: be + be1 });



                }
            }
        };
        return obj;

    }
    function openPopbar(data) {
        if (data != "") {
            var row = $("#list1").getRowData(data);
            var row = $("#list1").getRowData(data);
            $.colorbox({
                href: "../report/onesearch.aspx?id=" + row["ID"] + "&MName=" + row["MName"] + "&r=" + Math.random(),
                width: 950,
                height: 1240,
                title: function () {
                    return "报告编号：[" + row["BGBH"] + "]  报告日期：[" + row["BGRQ"] + "] ";// 委托编号：[" + row["WTBH"] + "] 
                },
                close: '',
                iframe: true
            });
        }
    }

    function openPopbar1(data, d) {
        if (data != "") {
            var row = $("#list1").getRowData(data);
            var row = $("#list1").getRowData(data);
            $.colorbox({
                href: "../report/testdata.aspx?id=" + row["ID"] + "&DeviceType=" + d + "&r=" + Math.random(),
                width: 700,
                height: 500,
                title: function () {
                    return "报告编号：[" + row["BGBH"] + "]  报告日期：[" + row["BGRQ"] + "] ";//委托编号：[" + row["WTBH"] + "]<br />
                },
                close: '',
                iframe: true
            });
        }
    }

</script>
