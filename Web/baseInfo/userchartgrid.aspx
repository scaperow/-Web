<%@ Page Language="C#" AutoEventWireup="true" CodeFile="userchartgrid.aspx.cs" Inherits="baseInfo_userchartgrid" %>

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
            var companycode = $("#hid_companycode").val();
            var testcode = $("#hid_testcode").val();
            var nunit = $("#hid_nunit").val();
            var classname = $("#hid_classname").val();

            var data = getSearchCondition("usersummarycharttogrid");
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["中心及分布", "姓名", '性别', '年龄', '职称', '职务', '工作年限', '联系方式', '学历', '毕业院校', '所学专业'],
                colModel: [               
                {
                    name: '试验室名称', index: '试验室名称', width: 80, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'试验室名称' + rowId + "\'";
                    }
                },
                //{ name: 'num', index: 'num', width: 50, sortable: false },
                { name: '姓名', index: '姓名', width: 60, align: 'center', sortable: false },
                { name: '性别', index: '性别', width: 40, align: 'center', sortable: false },
                { name: '年龄', index: '年龄', width: 40, align: 'center', sortable: false },
                { name: '技术职称', index: '技术职称', width: 100, align: 'center', sortable: false },
                { name: '职务', index: '职务', width: 100, align: 'center', sortable: false },
                { name: '工作年限', index: '工作年限', width: 60, align: 'center', sortable: false },
                { name: '联系电话', index: '联系电话', width: 100, align: 'center', sortable: false },
                { name: '学历', index: '学历', width: 100, align: 'center', sortable: false },
                  { name: '毕业学校', index: '毕业学校', width: 150, align: 'center', sortable: false },
                    { name: '专业', index: '专业', width: 100, align: 'center', sortable: false }
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
                rowList: [10,  20, 30],
                sortname: 'id',
                sortorder: 'asc',
                viewrecords: true,
                height: '100%',
                loadui: 'disable',
                postData: data,
                gridComplete: function () {
                    var gridName = "list1";                
                    Merger(gridName, '试验室名称', '');
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
            data.sCompanycode = $('#hid_companycode').val();
            data.sNuit = $('#hid_nunit').val();
            data.sClassname = $('#hid_classname').val();
            data.sType = stype;
            return data;
        }


        </script>
</head>
<body>
    <form id="form1" runat="server">
          <input type="hidden" id="hid_testcode" value="<%= Request.Params["testcode"].ToString()%>" />
        <input type="hidden" id="hid_companycode" value="<%= Request.Params["companycode"].ToString()%>" />
         <input type="hidden" id="hid_nunit" value="<%= Request.Params["nnuit"].ToString()%>" />
         <input type="hidden" id="hid_classname" value="<%= Request.Params["classname"].ToString()%>" />

<div id="divList" style="width: 100%;">
        <table id="list1"></table>
        <div id="pager1"></div>
    </div>
    </form>
</body>
</html>
