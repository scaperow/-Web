<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="XianLogin_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>铁路试验动态监控平台</title>
    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
    <link href="../Plugin/bootstrap/css/bootstrap.css" rel="stylesheet" />
     <link href="Css/Css.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/jquery-1.10.1.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".select select").change(function () {
                $(this).prev(".txt").text($(this).find("option:selected").text());
            });

            $(".ico_dow").click(function () {
                $(this).prev(".select").children("select").click().change();
            });
            //输入框获取焦点
            $('#txtUsername').focus(function () {
                var txtUsername_value = $('#txtUsername').val();
                if (txtUsername_value == "请输入您的用户名") {
                    $(this).val('').css({ "color": "#000" });
                }
            })

            //输入框失去焦点
            $('#txtUsername').blur(function () {
                var txtUsername_value = $('#txtUsername').val();
                if (txtUsername_value == "")
                    $(this).val('请输入您的用户名').css({ "color": "#000" }); {
                    }
            })

            var $txt2_obj = $('#txt2');//获取id为txt2的jquery对象
            var $txtPass_obj = $('#txtPass');//获取id为txtPass的jquery对象
            $txt2_obj.focus(function () {
                $txtPass_obj.show().focus();//使密码输入框获取焦点
                $txt2_obj.hide();//隐藏文本输入框

            })
            $txtPass_obj.blur(function () {
                if ($txtPass_obj.val() == '') {//密码输入框失去焦点后，若输入框中没有输入字符时，则显现文本输入框
                    $txt2_obj.show();
                    $txtPass_obj.hide();
                }

            })
        });
    </script>
</head>
<body>  
    <div id="wrap">
    <div class="top"> <div class="container ">
         <p class="text-muted textcenter">
             <img src="images/toptext.png" />
         </p>
        </div>
    </div>
    
  
    <div class="container main">
        
          <div class="col-md-4 col-sm-4 col-md-offset-7 col-sm-offset-7">
            

              <div class="loinform container">
                  <div class="row bg01356d">
                      <p><img src="images/dq.png" /> 信息化办公登录</p>
                  </div>
                  <div class="row from">
                      
                            <form id="form2" runat="server" role="form" class="">
                            <div class="form-group  has-feedback">
    <label for="exampleInputEmail1">用户名</label>
    <input type="text" class="form-control" tabindex="1" id="txtUsername" value="请输入您的用户名" runat="server" />
                                 <span class=" form-control-feedback"><img src="images/l.jpg" style="padding:5px;" /></span>

  </div>
                            <div class="form-group has-feedback">
    <label for="exampleInputPassword1">密　码</label>
    <input name="" type="text" class="form-control" id="txt2" value="请输入您的密码" />
      <input type="password" tabindex="1" id="txtPass" class="form-control"  runat="server" style="display: none;" />
        <span class=" form-control-feedback"><img src="images/s.jpg" style="padding:5px;" /></span>
  </div>

                            <asp:Button ID="btnLogin" CssClass="btn btn-primary  btn-sm pull-right" runat="server" Text="登&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;录" OnClick="btnLogin_Click" />
                             
                       
                              </form>
                       
                  </div>
           <div class="row bg01356d">
                     <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                  </div>
              </div>

              <img src="images/lock.png" class="pull-right" style=" position:absolute; bottom:-10px; right:-10px; " />
          </div>

    </div>

    </div>
       <div class="footer">
       
      <div class="container ">
        <p class="text-muted textcenter">北京金舟神创科技发展有限公司 &#169;2005-2014 </p>
      </div>
    </div>
     

</body>
</html>

               