<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="TestRoomList.aspx.cs" Inherits="TestRoom_TestRoomList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
<link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
<script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>
<script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
<script type="text/javascript">
    $(function () {
        createGrid();
        $(window).resize(function () {
            $('#List').treegrid('resize');
        });
    });


    function createGrid() {
       
        $(".list_btn").removeClass("bagc_8584");
        $("#divChart").hide();
        $("#divList").show();
        $('#List').treegrid({
            url: 'TestRoomList.aspx',
            queryParams: {Act:"List"}
        });

    }
    function ActInfo(value, rowData, rowIndex) {

        if (rowData.id.length == 16) {
            return "<a onclick='openPopChart(\"testroominfo\",\"" + rowData.id + "\",\"" + rowData.text + "-[试验室综合情况]\")'>详情</a>";
        }
        return " ";
    }

    function openPopChart(src, testcode, title) {
      
        $.colorbox({
            href: src+".aspx?ID=" + testcode + "&r=" + Math.random(),
           
            width: 950,
            height: 1240,
            title: title,
            close: '',
            iframe: true
        });
    }

    function ActC(value, rowData, rowIndex) {

        if (rowData.id.length == 16) {
            return "<a>详情</a>";
        }
        return " ";
    }
    function ActR(value, rowData, rowIndex) {

        if (rowData.id.length == 16) {
            return "<a>详情</a>";
        }
        return " ";
    }
    function ActM(value, rowData, rowIndex) {

        if (rowData.id.length == 16) {
            return "<a>详情</a>";
        }
        return " ";
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="piece">
        <div class="piece_con">
            <h2 class="title">
                <span class="left"><i></i>试验室情况</span>

         

                <ul class="searchbar_01 clearfix">
                   

                    <li class="right"><input name="" type="button" class="list_btn" title="列表" onclick="createGrid()"  /></li>

                </ul>

            </h2>
            <div class="content">

 
                <div id="divList">
                    
                       <!-- 列表 -->
                    <table id="List" iconcls=""  rownumbers="true" fitcolumns="true" 
                        width="100%" height="500px;" data-options='' url=""
                         fit="true" striped="false" singleSelect="true"  idField='id' treeField='text'
                       >

                        <thead>
                            <tr> 
                                <th field="id" width="50" align="center"  hidden="hidden">
                                    
                                </th>
                                <th field="text" width="300" align="left">名称
                                </th>
                                 <th field="1" width="100" align="center"   formatter="ActInfo">试验室情况
                                </th>
                                  <th field="2" width="100" align="center"   formatter="ActM">母体实验室
                                </th>
                                  <th field="3" width="100" align="center"   formatter="ActC">外委单位
                                </th>
                                  <th field="4" width="100" align="center"  formatter="ActR">外委报告
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>


                    </table>

                    <!-- 列表 -->
                </div>
            </div>
        </div>
    </div>

</asp:Content>

