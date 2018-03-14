<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestMail.aspx.cs" Inherits="Solution.UI.Web.TestMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/js/jquery-1.3.1.min.js"></script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div style="display:none;">
        <table cellpadding="0" cellspacing="0">
        <tr>
        <td colspan="2"><a href="javascript:void(0);">test</a></td>
        </tr>
            <tr>
                <td>
                    <img id="main_img1" src="" />
                    <img id="loader_img1" src="/images/loding.png" />
                </td>
                <td>
                    <img id="main_img2" src="/images/banner.jpg" />
                    <img id="loader_img2" src="/images/loding.png" />
                </td>
                <td>
                    <img id="main_img3" src="/images/free-shipping-banner.jpg" />
                    <img id="loader_img3" src="/images/loding.png" />
                </td>
            </tr>
            <tr>
                <td>
                    <img id="main_img4" src="/images/free-shipping-banner.jpg" />
                    <img id="loader_img4" src="/images/loding.png" />
                </td>
                <td>
                    <img id="main_img5" src="/images/banner.jpg" />
                    <img id="loader_img5" src="/images/loding.png" />
                </td>
                <td>
                    <img id="main_img6" src="/images/best-seller.jpg" />
                    <img id="loader_img6" src="/images/loding.png" />
                </td>
            </tr>
        </table>
    </div>
    <div style="display:none;">
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtEmailid" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" Visible="false" runat="server" Text="Send Mail" OnClick="Button1_Click" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUsername" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMesage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div id="myDiv">
    </div>
    <div>
        <asp:Repeater ID="RepProduct" runat="server">
            <ItemTemplate>
                
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div id="Div1">
        <p>
            <img src="/images/ajax-loader.gif"></p>
    </div>
    <div style="display: none;">
        <input type="hidden" id="hdn1" value="10" />
        <input type="hidden" id="hdn2" value="0" />
    </div>
    </form>
</body>
</html>
