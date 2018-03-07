<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="UploadRanking.aspx.cs" Inherits="WorkReport_UploadRanking" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>
    <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript">
        function Export() {
            window.open("../Export/Export.aspx?sType=SCZLPM&Ajax=" + Math.random() + "&StartDate=" + $('#txt_startDate').val() + "&EndDate=" + $('#txt_endDate').val());
        }

        function createGrid() {
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");


            $('#List').datagrid({
                url: 'UploadRanking.aspx',
                queryParams: {
                    Act: 'LIST',
                    StartDate: $('#txt_startDate').val(),
                    EndDate: $('#txt_endDate').val()
                }
            });


        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="piece" style="overflow-x: hidden; overflow-y: auto">
        <div class="piece_con">


            <h2 class="title">

                <span class="left"><i></i>上传资料排名

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
                                <th  colspan="2">施工单位周期前三名</th>
                                <th  colspan="2">施工单位周期后三名</th>
                                <th  colspan="2">监理单位周期前三名</th>
                                <th  colspan="2">登陆系统次数周期前三名</th>
                                <th  colspan="2">登陆系统次数周期后三名</th>
                            </tr>
                            <tr>
                                <th data-options="field:'0',width:80,align:'center'">单位</th>
                                <th data-options="field:'1',width:80,align:'center'">数量</th>
                                <th data-options="field:'2',width:80,align:'center'">单位</th>
                                <th data-options="field:'3',width:80,align:'center'">数量</th>
                                <th data-options="field:'4',width:80,align:'center'">单位</th>
                                <th data-options="field:'5',width:80,align:'center'">数量</th>
                                <th data-options="field:'6',width:80,align:'center'">单位</th>
                                <th data-options="field:'7',width:80,align:'center'">数量</th>
                                <th data-options="field:'8',width:80,align:'center'">单位</th>
                                <th data-options="field:'9',width:80,align:'center'">数量</th>
                            </tr>
                        </thead>

                    </table>

                    <!-- 列表 -->

                </div>

                
            </div>
        </div>
    </div>
</asp:Content>

