<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="OperateLog.aspx.cs" Inherits="BLOB_OperateLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Plugin/EasyUI/jquery.easyui.min.js"></script>
    <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
    
    <script src="../Plugin/EasyUI/datagrid-detailview.js"></script>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
      <div class="piece">
        <div class="piece_con">

            <h2 class="title">
                <span class="left"><i></i>修改日志查询</span>
                
                
                <ul class="searchbar_01 clearfix">
                    <li><span class="text">用户</span><input type="text" id="Person" value="" /></li>
                    <li class="right">
                        <input name="" type="button" class="list_btn " title="列表" onclick="createGrid()" /></li>
                    <li class="right">
                        <input name="" type="button" class="chart_btn" title="图表" onclick="createChart(1)"  style="display:none;" /></li>   
                </ul>
            </h2>
            <div class="content">

                <div id="divChart" style="display:none;">
                    <div id="chartdiv"></div>
                </div>
                <div id="divList">
                       <!-- 列表 -->
             <table id="List"     iconcls="" rownumbers="true" fitcolumns="true" nowrap="false"
        width="100%" height="100%" data-options='' url="OperateLog.aspx?Act=List"
        striped="true" singleselect="true"  pagination="true" fit="true" 
           sortName="" sortOrder=""
           >
        
             <thead>
            <tr>


                <th field="YH"  width="40"  align="center" >
                    用户
                </th>
                <th field="BD"  width="30"  align="center" >
                    标段
                </th>
                <th field="DW"  width="50"  align="center" >
                    单位
                </th>

                <th field="SYS"  width="50"  align="center" >
                    试验室
                </th>
                 <th field="CZRQ"  width="50"  align="center" formatter="FDatetime">
                    操作日期
                </th>
                <th field="CZLX"  width="30"  align="center" >
                    操作类型
                </th>
                 <th field="MB"  width="50"  align="center">
                    模板
                </th>
                <th field="BGMC"  width="50"  align="center" >
                    报告名称
                </th>
                 <th field="BGBH"  width="50"  align="center" >
                    报告编号
                </th>

            </tr>
                  </thead>
            
      
                 
    </table>
       
            <!-- 列表 -->
            
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function FDatetime(value, row, index) {
            return value.replace('T', ' ');
        }



        $(function () {
           createGrid();
            $(window).resize(function () {
                try {
                    $('#List').datagrid('resize');
                } catch (ex) { }
            });

        });



        function createGrid() {
            $(".chart_btn").addClass("bagc_8584");
            $(".list_btn").removeClass("bagc_8584");

            $("#divList").show();
            $('#List').datagrid({
                url: 'OperateLog.aspx',
                queryParams: {
                    Act: 'List',
                    Person: $('#Person').val()
                },
                view: detailview,
                detailFormatter: function (index, row) {
                    return '<div class="ddv" style="padding:5px 0"></div>';
                },
                onClickRow: function (rowIndex, rowData) {
                    $('#List').datagrid('expandRow', rowIndex);
                },
                onExpandRow: function (index, row) {
                    try {
                        var ddv = $(this).datagrid('getRowDetail', index).find('div.ddv');
                        //expandRow collapseRow 

                        var vs;
                        var v = '';
                        var inner = '';
                        vs = eval(row.XGRZ);
                        for (var i = 0; i < vs.length; i++) {
                            v += i == 0 ? "'"+vs[i].SheetID+"'" : ",'" + vs[i].SheetID+"'";
                        }
                        if (v != '') {
                            $.post('OperateLog.aspx', { Act: 'Sheet',SheetID:v }, function (d) {
                                var dd = eval(d);

                                for (var i = 0; i < vs.length; i++) {
                                    for (var k = 0; k < dd.length; k++) {
                                        if (dd[k].id == vs[i].SheetID) {
                                            inner += '<li style="height:28px; border-bottom:#CCC 1px dashed; line-height:28px;">' + (i+1) + '.【' + dd[k].name + '】 [' + vs[i].CellPosition + ']  <font color="green"> ' + vs[i].OriginalValue + '</font> => <font color="red">' + vs[i].CurrentValue + '</font></li>';
                                        }
                                    }
                                }
                                $(ddv).html('<ol>' + inner + '</ol>');
                                $('#List').datagrid('fixDetailRowHeight', index);
                            });
                        }
                    }
                    catch (ex) { }
                }
            });
        }


    </script>
</asp:Content>

