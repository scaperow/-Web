<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="FailReport.aspx.cs" Inherits="WorkReport_FailReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>
    <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript">
        function Export() {
            window.open("../Export/Export.aspx?sType=BHGBG&Ajax=" + Math.random() + "&StartDate=" + $('#txt_startDate').val() + "&EndDate=" + $('#txt_endDate').val());
        }

        function createGrid() {
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");


            $('#List').datagrid({
                url: 'FailReport.aspx',
                queryParams: {
                    Act: 'LIST',
                    StartDate: $('#txt_startDate').val(),
                    EndDate: $('#txt_endDate').val()
                }
            });


        }


        function FItem(value, row, index) {
            var _Temp = [];
            var _XM = '';
            var BHGS = value.split('||');
            for (var i = 0; i < BHGS.length; i++) {
                if (BHGS[i] == '') {
                    continue;
                }
                _Temp = BHGS[i].split(',');
                _XM += _Temp[0] + "<br />";
                _Temp = [];
            }
            return _XM;
        }


        function FVal1(value, row, index) {

            var _Temp = [];
            var _SZ = '';
            var BHGS = value.split('||');
            for (var i = 0; i < BHGS.length; i++) {
                if (BHGS[i] == '') {
                    continue;
                }
                _Temp = BHGS[i].split(',');
                _SZ += _Temp[1] + "<br />";
                _Temp = [];
            }
            return _SZ;
        }


        function FVal2(value, row, index) {
            var _Temp = [];

            var _WCZ = '';
            var BHGS = value.split('||');
            for (var i = 0; i < BHGS.length; i++) {
                if (BHGS[i] == '') {
                    continue;
                }
                _Temp = BHGS[i].split(',');

                _WCZ += _Temp[2] + "<br />";
                _Temp = [];
            }

            return _WCZ;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="piece" style="overflow-x: hidden; overflow-y: auto">
        <div class="piece_con">


            <h2 class="title">

                <span class="left"><i></i>不合格报告

                
                </span>
                <ul class="searchbar_01 clearfix">
                    <li class="right">
                        <input name="" type="button" class="list_btn  bagc_8584" title="列表" onclick="createGrid()" /></li>

                    <li class="right">
                        <input name="" type="button" class="export_btn" title="导出" onclick="Export()" /></li>



                </ul>
            </h2>
            <div class="content">


                <div id="divList">
                    <!-- 列表 -->
                    <table id="List" class="easyui-datagrid" iconcls="" rownumbers="true" fitcolumns="true" nowrap="false"
                        width="100%" height="100%" data-options='' url=""
                        striped="true" singleselect="true" pagination="false" fit="true"
                        sortname="" sortorder="">

                        <thead>
                            <tr>
                                <th field="st" width="50" align="center" >
                                    
                                </th>
                                <th field="ReportDate1" width="50" align="center" >
                                    日期
                                </th>
                                <th field="SectionName" width="50" align="center">标段
                                </th>
                                <th field="CompanyName" width="50" align="center">单位
                                </th>
                                <th field="TestRoomName" width="50" align="center">试验室
                                </th>
                                <th field="ReportName" width="30" align="center">试验报告
                                </th>
                                <th field="F_InvalidItem" width="80" align="center" formatter="FItem">不合格项目
                                </th>
                                <th field="F_InvalidItem1" width="30" align="center" formatter="FVal1">标准值
                                </th>
                                <th field="F_InvalidItem2" width="30" align="center" formatter="FVal2">实测值
                                </th>
                                 <th field="SGComment" width="50" align="center">原因分析
                                </th>
                                <th field="JLComment" width="50" align="center">监理意见
                                </th>
                                 <th field="DealResult" width="50" align="center">处理情况
                                </th>
                                 <th field="ReportNumber" width="50" align="center">报告编码
                                </th>
                                
                            </tr>
                        </thead>

                    </table>
                   
                    <!-- 列表 -->

                </div>

                  
            </div>
        </div>
    </div>
</asp:Content>


