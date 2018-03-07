<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeFile="ModifyContent.aspx.cs" Inherits="BLOB_ModifyContent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../Plugin/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <script src="../js/jquery-1.9.0.min.js"></script>
    <script src="../Plugin/bootstrap/js/bootstrap.js"></script>
</head>
<body>
 <div class="container-fluid">
            
            <div class="page-header" style="margin:0px 0 10px 0; border:none;"></div>

            <form class="form-horizontal" role="form" id="ff">
            <!-- 字段信息-->
             



                 <input type="hidden" id="KMID" value="<%=Obj.KMID %>" />


                   <div class="form-group">
                    <label for="a" class="col-sm-3  col-md-3 col-lg-3 control-label">
                        建设单位意见</label>
                    <div class="col-sm-7 col-md-7 col-lg-7">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-align-justify"></span></span>
                            <textarea id="Content"  class="form-control "  rows="10" placeholder="意见..."><%=Obj.Content %></textarea>
                        </div>
                    </div>

                </div>

            <!-- 字段信息-->
                    <!-- 按钮-->
        <div class="form-group">
            <div class=" col-xs-12 col-sm-12 col-md-12 col-lg-12" style="text-align: center;">
                <button id="but_save" data-loading-text="正在保存..." type="button" class="btn btn-success btn-sm" onclick="Save(1);">
                    <span class="glyphicon glyphicon-ok"></span>通过</button>
                <button id="but_reset" data-loading-text="正在保存..." type="reset" class="btn btn-warning  btn-sm"  onclick="Save(2);">
                    <span class="glyphicon glyphicon-repeat"></span>拒绝</button>

            </div>
        </div>
        <!-- 按钮--> 
            </form>
 </div>
</body>
</html>
<!--[if lt IE 9]>
		<script src="Js/html5shiv.js"></script>
		<script src="Js/respond.min.js"></script>
<![endif]-->
<script type="text/javascript">

    $(function () {



    });



    function Save(tp) {

      

        $('#but_save,#but_reset').button('loading');
        $.post('ModifyContent.aspx?Act=EDIT&AJAX=' + Math.random(), {

            KMID: $('#KMID').val(),
            Content: $('#Content').val(),
            Type: tp
        },
        function (d) {
            $('#but_save,#but_reset').button('reset')
            if (d == 'true') {
                alert('保存成功');
            }
            else {
                alert('保存失败');
            }
        }
        );
    }

</script>
