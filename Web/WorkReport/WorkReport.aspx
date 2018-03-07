<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="WorkReport.aspx.cs" Inherits="WorkReport_WorkReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>
    <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript">
        function Export() {
            window.open("../Export/Export.aspx?sType=WORKREPORT&Ajax=" + Math.random() + "&RType="+$(':radio[name="RType"]:checked').val());
        }

        function createGrid() {
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");


            switch ($(':radio[name="RType"]:checked').val())
            {
                case 'W':
                    $('#divList1').hide();
                    $('#divList').show();
                    $('#List').datagrid({
                        url: 'WorkReport.aspx',
                        queryParams: {
                            Act: 'LIST',
                            RType: $(':radio[name="RType"]:checked').val(),
                            StartDate: $('#txt_startDate').val(),
                            EndDate: $('#txt_endDate').val()
                        }
                    });
                    break;
                case 'M':
                    $('#divList').hide();
                    $('#divList1').show();
                    $('#ListM').datagrid({
                        url: 'WorkReport.aspx',
                        queryParams: {
                            Act: 'LIST',
                            RType: $(':radio[name="RType"]:checked').val(),
                            StartDate: $('#txt_startDate').val(),
                            EndDate: $('#txt_endDate').val()
                        }
                    });
                    break;
            }

            
        }


        $(function () {


        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="piece" style="overflow-x: hidden; overflow-y: auto">
        <div class="piece_con">


            <h2 class="title">

                <span class="left"><i></i>工作汇报表

                
                 <input name="RType" type="radio" value="W"  checked="checked" />周
                 <input name="RType" type="radio" value="M"/>月
                
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


                                <th field="单位" width="150" align="left">单位
                                </th>

                                <th field="上周资料数量" width="30" align="center">上周资料数量
                                </th>

                                <th field="本周资料数量" width="30" align="center">本周资料数量
                                </th>

                                <th field="截止本周累计数量" width="50" align="center">截止本周累计数量
                                </th>
                                <th field="本周资料修改次数" width="50" align="center">本周资料修改次数
                                </th>
                                <th field="上周登录次数" width="50" align="center">上周登录次数
                                </th>
                                <th field="本周系统登录次数" width="50" align="center">本周系统登录次数
                                </th>
                                <th field="截止累计登录次数" width="50" align="center">截止累计登录次数
                                </th>
                                <th field="本周不合格报告数量" width="50" align="center">本周不合格报告数量
                                </th>
                                <th field="处理完成数量" width="50" align="center">处理完成数量
                                </th>
                                <th field="未处理不合格报告数量" width="50" align="center">未处理不合格报告数量
                                </th>

                                <th field="在岗人数" width="50" align="center">在岗人数
                                </th>
                                <th field="离岗人数" width="50" align="center">离岗人数
                                </th>
                                <th field="设备数量" width="50" align="center">设备数量
                                </th>
                            </tr>
                        </thead>

                    </table>
                   
                    <!-- 列表 -->

                </div>

                  <div id="divList1" class="chartdiv" style="display:none;">
                    <!-- 列表 -->
                    <table id="ListM" class="easyui-datagrid" iconcls="" rownumbers="true" fitcolumns="true" nowrap="false"
                        width="100%" height="100%" data-options='' url=""
                        striped="true" singleselect="true" pagination="false" fit="true"
                        sortname="" sortorder="">

                        <thead>
                            <tr>


                                <th field="单位" width="150" align="left">单位
                                </th>

                                <th field="上月资料数量" width="30" align="center">上月资料数量
                                </th>

                                <th field="本月资料数量" width="30" align="center">本月资料数量
                                </th>

                                <th field="截止本月累计数量" width="50" align="center">截止本月累计数量
                                </th>
                                <th field="本月资料修改次数" width="50" align="center">本月资料修改次数
                                </th>
                                <th field="上月登录次数" width="50" align="center">上月登录次数
                                </th>
                                <th field="本月系统登录次数" width="50" align="center">本月系统登录次数
                                </th>
                                <th field="截止累计登录次数" width="50" align="center">截止累计登录次数
                                </th>
                                <th field="本月不合格报告数量" width="50" align="center">本月不合格报告数量
                                </th>
                                <th field="处理完成数量" width="50" align="center">处理完成数量
                                </th>
                                <th field="未处理不合格报告数量" width="50" align="center">未处理不合格报告数量
                                </th>
                                <th field="在岗人数" width="50" align="center">在岗人数
                                </th>
                                <th field="离岗人数" width="50" align="center">离岗人数
                                </th>
                                <th field="设备数量" width="50" align="center">设备数量
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

