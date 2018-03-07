<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>铁路试验动态监控平台</title>
    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />

    <link href="css/login.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.10.1.min.js"></script>
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
    <form id="form1" runat="server">
        <h1 class="logo_tit"><span class="color_01">铁路</span>试验信息<span class="color_02">管理系统</span></h1>
        <dl class="loginform">
            <dt><i></i>请输入您的用户名和密码</dt>
            <dd class="user">


                <input type="text" style="color: #000;" tabindex="1" id="txtUsername" value="请输入您的用户名" runat="server" />
            </dd>
            <dd class="pass">
                <input name="" type="text" id="txt2" value="请输入您的密码" /><input type="password" tabindex="1" id="txtPass" runat="server" style="display: none;" /></dd>
          
            <dd class="btn"><span>

                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>

            </span>

                <asp:Button ID="btnLogin" CssClass="loginbtn" runat="server" Text="登录" OnClick="btnLogin_Click" />

            </dd> 
        </dl>
     
       <h1 class="logo_tit2">北京金舟神创科技发展有限公司 &#169;2005-2014</h1>
    </form>
</body>
</html>
