<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="EditHomePageBanner.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.EditHomePageBanner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#ffffff" class="content-table">
                <tbody>
                    <tr>
                        <td class="border-td-sub">
                            <table align="left" class="add-product" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <th colspan="2">
                                        <div class="main-title-left">
                                            <img class="img-left" title="Add Admin" alt="Add Admin" src="/App_Themes/<%=Page.Theme %>/Images/home-page-banner-icon.png">
                                            <h2>
                                                <asp:Label runat="server" Text="Edit Home Page Banner" ID="lblTitle"></asp:Label></h2>
                                        </div>
                                    </th>
                                </tr>
                                <tr>
                                    <td colspan="2" align="Center">
                                        <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td colspan="2" align="right">
                                        <span style="color: Red;">*</span><asp:Label ID="Label1" runat="server"> is Required Field</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="font-black01" style="width: 138px; height: 30px" valign="middle">
                                        <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Store
                                        Name :
                                    </td>
                                    <td align="left" style="height: 30px" valign="middle">
                                        <asp:DropDownList ID="ddlStore" runat="server" DataTextField="StoreName" DataValueField="StoreID"
                                            CssClass="ddlStore">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td align="left" valign="top">
                                        <span style="color: Red;">*</span> Banner Title :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtbannerTitle" Width="350px" runat="server" CssClass="textfild"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtbannerTitle"
                                            ErrorMessage="Please Enter Banner Title" CssClass="rferror" SetFocusOnError="True"
                                            ValidationGroup="AppConfig"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class="table_bg" style="display: none">
                                    <td align="left" valign="top">
                                        &nbsp;&nbsp; Banner Description :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtBannerDescription" Width="350px" Height="50" TextMode="MultiLine"
                                            runat="server" CssClass="textfild"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td align="left" valign="top">
                                         <span style="color: Red;">*</span> Banner URL :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtBannerURL" Width="350px" runat="server" CssClass="textfild"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="FileUploadBanner"
                                                ErrorMessage="Select File" CssClass="rferror" SetFocusOnError="True" ValidationGroup="AppConfig"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class="table_bg">
                                    <td align="left" class="font-black01" valign="top">
                                        &nbsp;&nbsp; Banner Image :
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="FileUploadBanner" runat="server" />
                                        <asp:Label ID="lblImgSize" runat="server" Text="Size should be 695 x 305"></asp:Label><%--Width=960px,Height=245--%>
                                        <%--  <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="FileUploadBanner"
                                                ErrorMessage="Select File" CssClass="rferror" SetFocusOnError="True" ValidationGroup="AppConfig"></asp:RequiredFieldValidator>--%>
                                        <div>
                                        <br />
                                            <img id="imgBanner" runat="server" visible="false"/></div>
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td align="left" valign="top">
                                        &nbsp;&nbsp; Display Order :
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="TxtDisplayOrder" Width="40px" Style="text-align: center" MaxLength="2"
                                            runat="server" CssClass="textfild"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="table_bg">
                                    <td align="left" valign="top">
                                        &nbsp;&nbsp;&nbsp;Active :
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkActive" runat="server" />
                                    </td>
                                </tr>
                                <tr class="altrow">
                                    <td align="left" height="30" valign="top" style="width: 137px">
                                    </td>
                                    <td align="left">
                                        <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                            OnClick="imgSave_Click" OnClientClick="return Checkfields();" />
                                        <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                            OnClick="imgCancle_Click" />
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</asp:Content>
