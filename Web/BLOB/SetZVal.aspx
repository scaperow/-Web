<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetZVal.aspx.cs" Inherits="BLOB_SetZVal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <!--[if !IE]> -->
    <script type="text/javascript">
        window.jQuery || document.write("<script src='<%=ResolveUrl("../Plugin/ACE/js/jquery.min2.1.0.js") %>'>" + "<" + "/script>");
    </script>
    <!-- <![endif]-->
    <!--[if IE]>
    <script type="text/javascript">
     window.jQuery || document.write("<script src='<%=ResolveUrl("../Plugin/ACE/js/jquery.min1.11.0.js") %>'>"+"<"+"/script>");
    </script>
    <![endif]-->

    <script src="../Plugin/EasyUI/jquery.easyui.min.js"></script>
    <script src="../Plugin/EasyUI/easyui-lang-zh_CN.js"></script>
    <link href="../Plugin/EasyUI/themes/default/easyui.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
                   原材料:
<select id="Item">
</select>
检测指标:
<select id="Attr">
</select>
型号:
<select id="Model">
</select>
        模板:
                    <select id="Module">
                    </select>
    </div>

        <div id="ZValDIV"><hr />
            <textarea id="ZVal" rows="10" cols="50"></textarea>
            <input type="button" value="保存" onclick="Save();" />
        </div>

                            <!-- 列表 -->
       <table id="List"    iconcls="" rownumbers="true" fitcolumns="false" nowrap="false"
        width="500px" height="500px" data-options='' url=""
        striped="true" singleselect="true"  pagination="true" fit="false" 
           sortName="" sortOrder=""
           >
        
             <thead>
            <tr>
                
                <th field="YC"  width="100"  align="center" >
                    原材
                </th>
                <th field="ItemName"  width="100"  align="center" >
                    检测指标
                </th>
                <th field="Model"  width="100"  align="center" >
                    型号
                </th>
                <th field="MName"  width="100"  align="center" >
                    模板
                </th>
                <th field="StandardValue"  width="100"  align="center" >
                    标准值
                </th>
                 <th field="IndexID"  width="100"  align="center" formatter="FAct">
                    
                </th>
                
            </tr>
                  </thead>
            <tbody>
                 
                
                </tbody>
      
                 
    </table>
       
            <!-- 列表 -->
    </form>
</body>
</html>
<script type="text/javascript">

    function FAct(value, row, index) {
        return '<a href="#" onclick="Del(\''+value+'\');">删除</a>';
    }

    function Del(id)
    {
        $.post("SetZVal.aspx", { Act: "Del", IndexID: id }, function (d) {
           
            List();
            alert(d);
        });
    }

    $(function () {
        LoadItem();
       

        $('#Item').change(function () {
            LoadAttr($('#Item').find("option:selected").val());
            LoadModel($('#Item').find("option:selected").val());
            LoadModule($('#Item').find("option:selected").val());
        });
  
        List();
    });



    function LoadItem() {
        $('#Item').empty();
        $.post("SetZVal.aspx", { Act: "Item" }, function (d) {
            if (d == 'null') { return; }
            $('#Item').append(' <option value="">请选择</option>');
            var json = eval(d);
            for (var i = 0; i < json.length; i++) {
                $('#Item').append(' <option value="' + json[i].ItemID + '">' + json[i].ItemName + '</option>');
            }
        });
    }

    function LoadModule(id) {

        $('#Module').empty();
        $.post("SetZVal.aspx", { Act: "Module", ItemID: id }, function (d) {
            if (d == 'null') { return; }
            $('#Module').append(' <option value="/">/</option>');
            var json = eval(d);
            for (var i = 0; i < json.length; i++) {
                $('#Module').append(' <option value="' + json[i].ModuleID + '">' + json[i].Name + '</option>');
            }
        });
    }

    function LoadAttr(id) {
        $('#Attr').empty();
        $.post("SetZVal.aspx", { Act: "Attr", ItemID: id }, function (d) {
            if (d == 'null') { return; }
            var json = eval(d);
            for (var i = 0; i < json.length; i++) {
                $('#Attr').append(' <option value="' + json[i].BindField + '">' + json[i].ItemName + '</option>');
            }
        });
    }

    function LoadModel(id) {
        $('#Model').empty();

        $.post("SetZVal.aspx", { Act: "Model", ItemID: id }, function (d) {
            $('#Model').append(' <option value="">全部</option>');
            if (d == 'null') { return; }
            var json = eval(d);
            for (var i = 0; i < json.length; i++) {
                $('#Model').append(' <option value="' + json[i].XH + '">' + json[i].XH + '</option>');
            }

            $('#Model').change(function () { List();});
        });
    }


    function Save()
    {
        if ($('#Item').find("option:selected").val() == '' || $('#Attr').find("option:selected").val() == '' || $('#Model').find("option:selected").val() == '') {
            alert("请选择 原材 检测指标 型号");
            return;
        }

        $.post("SetZVal.aspx", {
            Act: "SAVE",
            Item: $('#Item').find("option:selected").val(),
            Attr: $('#Attr').find("option:selected").val(),
            AttrName: $('#Attr').find("option:selected").text(),
            Model: $('#Model').find("option:selected").val(),
            ZVal: $('#ZVal').val(),
            ModuleID: $('#Module').find("option:selected").val()
        }, function (d) {
          
            List();
        });

    }


    function List()
    {
        $('#List').datagrid({
            url: 'SetZVal.aspx',
            width:800,height:400,
            queryParams: {
                Act: 'List',
                Item: $('#Item').find("option:selected").val(),
                Attr: $('#Attr').find("option:selected").val(),
                AttrName: $('#Attr').find("option:selected").text(),
                Model: $('#Model').find("option:selected").val()
            }
        });
    }


    </script>