<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreaditRatingEdit.aspx.cs" Inherits="Rating_CreaditRatingEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
             



                <div class="form-group">
                    <label for="a" class="col-sm-3  col-md-3 col-lg-3 control-label">
                        试验室</label>
                    <div class="col-sm-7 col-md-7 col-lg-7">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-align-justify"></span></span>
                            
                                <select id="TestRoomCode" class="form-control "  onchange="TestRoomClick(this);">
                                
                            </select>
                            <input type="hidden" id="SysID" value="<%=Obj.SysID %>" />
                        </div>
                    </div>

                </div>

                <div class="form-group">
                    <label for="a" class="col-sm-3  col-md-3 col-lg-3 control-label">
                        单位性质</label>
                    <div class="col-sm-7 col-md-7 col-lg-7">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-align-justify"></span></span>
                           
                            <select id="CompanyType" class="form-control " >
                                 <option <%= Obj.CompanyType =="施工"?"selected":"" %>>施工</option>
                                <option <%= Obj.CompanyType =="监理"?"selected":"" %>>监理</option>
                            </select>
                        </div>
                    </div>

                </div>

                    <div class="form-group">
                    <label for="a" class="col-sm-3  col-md-3 col-lg-3 control-label">
                        评价类别</label>
                    <div class="col-sm-7 col-md-7 col-lg-7">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-align-justify"></span></span>
                           
                            <select id="RType" class="form-control " onchange="RTypeClick(this);">
                                 <option <%= Obj.RType =="试验室"?"selected":"" %>>试验室</option>
                                <option <%= Obj.RType =="个人"?"selected":"" %>>个人</option>
                            </select>
                        </div>
                    </div>

                </div>

                <div class="form-group">
                    <label for="a" class="col-sm-3  col-md-3 col-lg-3 control-label">
                        姓名</label>
                    <div class="col-sm-7 col-md-7 col-lg-7">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-align-justify"></span></span>
                            
                                 <select id="Name" class="form-control " onchange="PersonTypeClick(this);">
                                     </select>
                        </div>
                    </div>

                </div>

                <div class="form-group">
                    <label for="a" class="col-sm-3  col-md-3 col-lg-3 control-label">
                        身份证号</label>
                    <div class="col-sm-7 col-md-7 col-lg-7">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-align-justify"></span></span>
                            <input id="IDCard" type="text" class="form-control" placeholder="身份证号" value="<%=Obj.IDCard %>">
                                
                        </div>
                    </div>

                </div>



                <div class="form-group">
                    <label for="a" class="col-sm-3  col-md-3 col-lg-3 control-label">
                        职务</label>
                    <div class="col-sm-7 col-md-7 col-lg-7">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-align-justify"></span></span>
                            <input id="Job" type="text" class="form-control" placeholder="职务" value="<%=Obj.Job %>">
                
                        </div>
                    </div>

                </div>

                <div class="form-group">
                    <label for="a" class="col-sm-3  col-md-3 col-lg-3 control-label">
                        扣分总数</label>
                    <div class="col-sm-7 col-md-7 col-lg-7">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-align-justify"></span></span>
                            <input id="Deduct" type="text" class="form-control" placeholder="扣分总数" value="<%=Obj.Deduct %>">
                                
                        </div>
                    </div>

                </div>


                   <div class="form-group">
                    <label for="a" class="col-sm-3  col-md-3 col-lg-3 control-label">
                        备注</label>
                    <div class="col-sm-7 col-md-7 col-lg-7">
                        <div class="input-group">
                            <span class="input-group-addon"><span class="glyphicon glyphicon-align-justify"></span></span>
                            <textarea id="Remark"  class="form-control "  rows="5" placeholder="备注..."><%=Obj.Remark %></textarea>
                        </div>
                    </div>

                </div>

            <!-- 字段信息-->
                    <!-- 按钮-->
        <div class="form-group">
            <div class=" col-xs-12 col-sm-12 col-md-12 col-lg-12" style="text-align: center;">
                <button id="but_save" data-loading-text="正在保存..." type="button" class="btn btn-success btn-sm" onclick="Save();">
                    <span class="glyphicon glyphicon-ok"></span>保存</button>
                <button id="but_reset" data-loading-text="正在保存..." type="reset" class="btn btn-warning  btn-sm" >
                    <span class="glyphicon glyphicon-repeat"></span>重置</button>

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

        $.post('CreaditRatingEdit.aspx?Act=GetTESTROOM&AJAX=' + Math.random(), {}, function (d) {

            var _List = eval(d);
            for (var i = 0; i < _List.length;i++)
            {
                $('#TestRoomCode').append('<option value="' + _List[i].nodecode + '"  ' + (_List[i].nodecode =='<%=Obj.TestRoomCode%>'?"selected":"") + '  >' + _List[i].description + '</option>');
            }
            $('#RType').change();
        });

       
    });

    function PersonTypeClick(o)
    {
        $('#Job').val($('#Name option:selected').attr("Job"));
        $('#IDCard').val($('#Name option:selected').attr("IDCard"));
    }

    function TestRoomClick(o)
    {
        $('#Name').empty();
        if ($('#RType option:selected').val() == '试验室')
        {
            return;
        }
        $.post('CreaditRatingEdit.aspx?Act=GetPerson&AJAX=' + Math.random(), { TestRoomCode: $('#TestRoomCode option:selected').val() }, function (d) {
            var _List = eval(d);
            for (var i = 0; i < _List.length; i++) {
                $('#Name').append('<option value="' + _List[i].Name + '" Job="' + _List[i].Job + '"  IDCard="' + _List[i].IDCard + '"   ' + (_List[i].Name == '<%=Obj.Name%>' ? "selected" : "") + '  >' + _List[i].Name + '</option>');
            }
            PersonTypeClick(null);
        });
    }

    function RTypeClick(o)
    {
  
        if ($('#RType option:selected').val() == '试验室') {
            $('#Name').prop('disabled', true).empty();
            $('#IDCard,#Job').prop('readonly', true).val('');
        }
        else {
            $('#Name').prop('disabled', false);
            $('#Name,#IDCard,#Job').prop('readonly', true);
            TestRoomClick(null);
        }
    }

    function Save() {


        $('#but_save,#but_reset').button('loading');
        $.post('CreaditRatingEdit.aspx?Act=<%=ACT%>&AJAX=' + Math.random(), {
           
            TestRoomCode: $('#TestRoomCode option:selected').val(),
            CompanyType: $('#CompanyType option:selected').val(),
            RType: $('#RType option:selected').val(),
            Name: $('#Name option:selected').val(),
            IDCard: $('#IDCard').val(),
            Job: $('#Job').val(),
            Deduct: $('#Deduct').val(),
            Remark: $('#Remark').val(),
            SysID: $('#SysID').val()
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