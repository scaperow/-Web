<%@ Page Language="C#" AutoEventWireup="true" CodeFile="machinegrid.aspx.cs" Inherits="baseInfo_machinegrid" %>

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
            createGrid();
        });

        function getGridOption() {
            var testcode = getQueryString('testcode');
            $("#hid_testcode").val(testcode);
            var data = getSearchCondition('machinesummarygrid');
            var obj = {
                url: '<%= ResolveUrl("machinegrid.aspx?Act=List&testcode="+"testcode".RequestStr()) %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["管理编号", "设备名称", '生产厂家', '规格型号', '数量', '备注'],
                colModel: [   
                { name: '管理编号', index: '管理编号', sortable: false, width: 50 },
                { name: '设备名称', index: '设备名称', width: 100, sortable: false, align: 'center' },
                { name: '生产厂家', index: '生产厂家', width: 120, sortable: false, align: 'center' },
                { name: '规格型号', index: '规格型号', width: 120, sortable: false, align: 'center' },
                //{ name: '购置日期', index: '购置日期', width: 60, sortable: false, align: 'center' },
                { name: '数量', index: '数量', width: 40, sortable: false, align: 'center' },
                 { name: '备注', index: '备注', width: 40, sortable: false, align: 'center' }
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
                sortname: 'ID',
                sortorder: 'asc',
                viewrecords: true,
                height: '100%',
                loadui: 'disable',
                postData: data,
                gridComplete: function () {
                    var gridName = "list1";               
                    //adeptGridWidth();
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
            data.sType = stype;
            return data;
        }


        </script>
</head>
<body>
    <form id="form1" runat="server">
          <input type="hidden" id="hid_testcode" />
        <input type="hidden" id="hid_companycode" />
         <input type="hidden" id="hid_nunit" />
         <input type="hidden" id="hid_classname" />

<div id="divList" style="width: 100%; padding-top:20px;">
        <table id="list1"></table>
        <div id="pager1"></div>
    </div>
    </form>
</body>
</html>

