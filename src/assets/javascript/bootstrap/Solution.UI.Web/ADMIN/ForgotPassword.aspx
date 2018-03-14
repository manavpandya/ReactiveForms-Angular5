<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>forgot Password</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="login-page-bg">
        <h2>
            Forgot Password
        </h2>
        <div class="login-page">
            <div class="login-page-left">
                <img class="logo1" src="/App_Themes/<%=Page.Theme %>/images/logo.png" title="Half Price Drapes"
                    alt="Half Price Drapes" />
            </div>
            <div class="login-page-right">
                <div class="login-page-right-content">
                    <div class="login-page-right-textbox">
                        <label>
                            Email :</label>
                        <img width="10" height="31" class="img-left" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/input-left.jpg" />
                        <span class="overlay_wrapper">
                            <asp:TextBox CssClass="login-page-right-textbox-input" ID="txtEmail" runat="server"></asp:TextBox>
                        </span>
                        <img width="10" height="31" class="img-right" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/input-right.jpg" />
                    </div>
                    <div class="login-page-right-textbox1">
                        <asp:ImageButton TabIndex="3" ID="btnLogin" runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/images/login.png"
                            OnClientClick="return checkLogin();" ValidationGroup="Login" />
                        <a href="javascript:void(0);" onclick="return ShowForGotPassword();">Forgot Password?</a>
                    </div>
                </div>
            </div>
        </div>
        <img width="500" height="44" class="img-left" title="" alt="" src="/App_Themes/<%=Page.Theme %>/images/login-page-bottom.png" />
    </div>
    </form>
</body>
</html>
