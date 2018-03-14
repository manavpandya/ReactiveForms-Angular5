<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CountryDemo.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.CountryDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="content-row1">
     
    <table>
        <tr>
            <td>
                Country
            </td>
            <td>
                <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Two Letter ISO Code
            </td>
            <td>
                <asp:TextBox ID="txtTwolatterISOCode" MaxLength="2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Three Letter ISO Code
            </td>
            <td>
                <asp:TextBox ID="txtThreelatterISOCode"  MaxLength="3" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Numeric ISO Code
            </td>
            <td>
                <asp:TextBox ID="txtNumericISOCode" MaxLength="3" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Display Order
            </td>
            <td>
                <asp:TextBox ID="txtDispalyOrder"  MaxLength="3"  runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
