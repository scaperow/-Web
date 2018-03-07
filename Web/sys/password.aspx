<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="password.aspx.cs" Inherits="sys_password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript" language="javascript">

         function CheckUser() {
             if ($("#<%=txt_new.ClientID%>").val() == "") {
                alert("新密码不能为空!");
                return false;
            }
            if ($("#<%=txt_confrim.ClientID%>").val() == "") {
                alert("确认密码不能为空!");
                return false;
            }
            if ($("#<%=txt_confrim.ClientID%>").val() != $("#<%=txt_new.ClientID%>").val()) {
                alert("密码不一致!");
                return false;
            }
            $("#<%=Button1.ClientID%>").click();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="piece">
    	<div class="piece_con">
        	  <h2 class="title">
                <span class="left"><i></i>修改密码</span>

                <ul class="searchbar_01 clearfix">
            
                    <li class="right"><input name="" type="button" class="list_btn" title="列表" onclick="createGrid()"  /></li>
                </ul>
            </h2>

            <div class="content">

                <br />

               <table style="  width: 80%;">
                 
                        <tr>
                            <td style="height: 30px; text-align: right;">用户名：
                               
                            </td>
                            <td> <asp:Label ID="lbl_username" ForeColor="Red" runat="server" Text=""></asp:Label></td>

                            <td style="height: 30px; text-align: right;">当前密码： 
                               
                            </td>
                            <td> <asp:Label ID="lbl_old" ForeColor="Red" runat="server" Text=""></asp:Label></td>

                           </tr> <tr>
                            <td style="height: 30px; text-align: right;">新密码：
                            </td>
                            <td>
                                <asp:TextBox ID="txt_new" runat="server"></asp:TextBox>
                               
                            </td>
                            <td style="height: 30px; text-align: right;">确认密码：
                            </td>
                            <td>
                                <asp:TextBox ID="txt_confrim" runat="server"></asp:TextBox>
                               

                            </td>
                        </tr>

                        <tr>

                            <td colspan="4" style="text-align: center;">

                                <input type="button" value="提交" onclick="CheckUser();" />
 
                       

                                <asp:Label ID="Label1" ForeColor="Red" runat="server" Text=""></asp:Label>

                            </td>

                        </tr>




                    </table>
     
            </div>
        </div>
    </div>    <div style="display:none;">   <asp:Button ID="Button1" runat="server" Text="提交" OnClick="Button1_Click" /></div>
</asp:Content>

