<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="parallelReport.aspx.cs" Inherits="report_parallelReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript">
        
        $(function () {
            //createChart();
            createGrid();
        });


        function getSearchCondition(stype) {
            var data = {};
            data.StartDate = $('#txt_startDate').val();
            data.EndDate = $('#txt_endDate').val();
            data.sType = stype;
            data.meet = $(':radio[name="meet"]:checked').val();
            
            return data;
        } 

        function getGridOption() {
            var data = getSearchCondition("pxreport");

            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'GET',
                colNames: ["标段", "监理单位", "施工单位", "施工单位试验室", "试验名称", "标准平行频率(%)", "施工单位资料总数", "平行次数", "平行频率(%)", "平行频率质量系数","是否满足要求",  "testroomID", "modelID","操作"],
                colModel: [
                { name: 'segment', index: 'segment', width: 80, sortable: false                },
                { name: 'jl', index: 'jl', width: 100, sortable: false            },
                { name: 'sg', index: 'sg', width: 100, sortable: false   },
                { name: 'testroom', index: 'testroom', width: 110, sortable: false      },
                { name: 'modelName', index: 'modelName', width: 200, sortable: false },
                { name: 'condition', index: 'condition', width: 130, sortable: false, align: 'center' },
                 { name: 'zjCount', index: 'zjCount', width: 100, sortable: false, align: 'center' },
                  { name: 'pxCount', index: 'pxCount', width: 60, sortable: false, align: 'center' },
                   { name: 'frequency', index: 'frequency', width: 100, sortable: false, align: 'center' },
                     { name: 'pxqulifty', index: 'pxqulifty', width: 120, sortable: false, align: 'center' },
                     { name: 'result', index: 'result', width: 90, sortable: false, align: 'center'},
                    
                     { name: 'testroomID', index: 'testroomID', width: 5, hidden: true },
                     { name: 'modelID', index: 'modelID', width: 5, hidden: true },
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
                rowNum: 15,
                rowList: [15, 30, 50, 100],
                sortname: 'segment',
                sortorder: 'asc',
                viewrecords: true,
                height: '100%',
                loadui:'disable',
                postData: data,
                gridComplete: function () {
                    var gridName = "list1";
                    var ids = jQuery("#list1").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var id = ids[i];

                        be = "<span  style='color:#1c7c9c;cursor:pointer;' onclick=\"openPopbar('" + id + "')\" >查看</span>";
                        jQuery("#list1").jqGrid('setRowData', ids[i], { act: be });

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
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");
            $("#divChart").hide();
            $("#divList").show();
            $("#list1").GridUnload();
            jqGrid = $("#list1").jqGrid(getGridOption()).navGrid("#pager1", { edit: false, add: false, del: false, search: false, pdf: true });
        } 

        function openPopbar(data)
        {
            var row = $("#list1").getRowData(data);
            var testroomid = row["testroomID"];
            var modelid = row["modelID"];

            $.colorbox({
                href: "popParallelReport.aspx?testroomid=" + row["testroomID"] + "&modelid=" + row["modelID"] + "&r=" + Math.random(),
                width: 800,
                height: 620,
                title: function () {
                    return "平行频率- [" + row["modelName"] + " " + $('#hd_startDate').val()
                        + " 至 " + $('#hd_endDate').val() + "]";
                },
                close: '×',
                iframe: true
            });
        }
        
        function searchClick() {
            if ($("#divChart").css("display") == 'block') {
                createChart();
            }
            else {
                createGrid();
            }
        }
        function Export() {
            var Para = jQuery.param(getSearchCondition('PXPLFX'));

            window.open("../Export/Export.aspx?sType=PXPLFX&Ajax=" + Math.random() + "&" + Para);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   

    <div class="piece">
    	<div class="piece_con">
        	 <h2 class="title">
                    <span class="left"><i></i>平行频率分析  

                          <input name="meet" type="radio" value="2" />满足
                 <input name="meet" type="radio" value="1"  checked="checked" />不满足
                 <input name="meet" type="radio" value="0"/>所有
                    </span>
              
                <ul class="searchbar_01 clearfix">
             <li class="right"><input name="" type="button" class="list_btn" title="列表" onclick="createGrid()"  /></li>
   <%--                 <li class="right"><input name="" type="button" class="chart_btn" title="图表" onclick="createChart()" /></li>--%>
                    <li class="right">
                        <input name="" type="button" class="export_btn" title="导出" onclick="Export()" /></li>
                        </ul>
                    </h2>
            <div class="content">

               
         <div id="divChart">
                    <div id="chartdiv"></div>
                </div>
                <div id="divList">
                    <table id="list1"></table>
                    <div id="pager1"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


