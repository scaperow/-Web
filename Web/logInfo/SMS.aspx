<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="SMS.aspx.cs" Inherits="logInfo_SMS" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%= ResolveUrl("~/js/amcharts.js") %>"></script>
    <script type="text/javascript">

        $(function () {

            createGrid();

        });




        function getGridOption() {
            var data = getSearchCondition('SMS');
            var obj = {
                url: '<%= ResolveUrl("~/ajax/ajaxJQGrid.aspx") %>',
                datatype: "json",
                mtype: 'POST',
                colNames: ["标段", "单位", "试验室", "发送时间", '收件人姓名', '收件人电话', '短信内容'],
                colModel: [
               
                {
                    name: 'segment', index: 'segment', width: 40, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'segment' + rowId + "\'";
                    }
                },
                {
                    name: 'company', index: 'company', width: 40, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'company' + rowId + "\'";
                    }
                },
                {
                    name: 'testroom', index: 'testroom', width: 40, align: 'left', sortable: false,
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'id=\'testroom' + rowId + "\'";
                    }
                },

                { name: 'SentTime', index: 'SentTime', width: 80, align: 'center', sortable: false, formatter: "date", formatoptions: { srcformat: 'Y-m-d H:i:s', newformat: 'Y-m-d H:i:s' } },
                { name: 'PersonName', index: 'PersonName', width: 60, align: 'center', sortable: false },
                 { name: 'SMSPhone', index: 'SMSPhone', width: 60, align: 'center', sortable: false },
                
                {
                    name: 'SMSContent', index: 'SMSContent', width: 300, align: 'left', sortable: false,
                cellattr: function (rowId, tv, rawObject, cm, rdata) {
                    return 'id=\'SMSContent' + rowId + "\'";
                }
            }
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
                    Merger(gridName, 'SMSContent', '');
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
            data.tel = $('#txt_tel').val();
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
                <span class="left"><i></i>短信查询</span>

                <ul class="searchbar_01 clearfix">
                    <li><span class="text">姓名</span><input type="text" id="txt_username" /></li>
                     <li><span class="text">电话</span><input type="text" id="txt_tel" /></li>
                    <li class="right"><input name="" type="button" class="list_btn" title="列表" onclick="createGrid()"  /></li>
                </ul>
            </h2>

            <div class="content">

              
    <div id="divList" style="word-wrap: break-word; ">
        <table id="list1"></table>
        <div id="pager1"></div>
    </div>
            </div>
        </div>
    </div>
</asp:Content>

