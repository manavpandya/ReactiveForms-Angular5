<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="Topic.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Content.Topic" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add Staic Website Page" alt="Add Staic Website Page" src="/App_Themes/<%=Page.Theme %>/Images/add-topic-icon.png" />
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Add Staic Website Page"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <span class="star">*</span>Required Field
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">&nbsp;&nbsp;</span>Store Name:
                                                        </td>
                                                        <td style="width: 80%; vertical-align: top">
                                                            <asp:DropDownList runat="server" ID="drpStoreName" CssClass="add-product-list" Width="190px"
                                                                Height="20px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Title:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtTitle" CssClass="order-textfield" MaxLength="50"
                                                                Width="300px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorTitle" runat="server" ControlToValidate="txtTitle"
                                                                ErrorMessage=" Enter Static Website Page Title" SetFocusOnError="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Staic Website Page  Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtTopicName" CssClass="order-textfield" Width="300px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTopicName"
                                                                ErrorMessage=" Enter Static Website Page Name" SetFocusOnError="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>Description:
                                                        </td>
                                                        <td>
                                                            <CKEditor:CKEditorControl ID="ckeditordescription" runat="server" BasePath="~/ckeditor/"
                                                                Width="800px" Height="500px"></CKEditor:CKEditorControl>
                                                        </td>
                                                    </tr>
                                                    <%-- <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>SE-Description:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSEDescription" runat="server" Height="63px" Rows="3" TextMode="MultiLine"
                                                                Width="291px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>SE-Keywords:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSEKeywords" runat="server" Height="63px" Rows="3" TextMode="MultiLine"
                                                                Width="291px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>SE-Title:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSETitle" runat="server" Height="63px" Rows="3" TextMode="MultiLine"
                                                                Width="291px"></asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Show on SiteMap:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkShowOnSiteMap" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="80%" cellspacing="0" cellpadding="0" border="0" class="border-td content-table">
                                                                <tbody>
                                                                    <tr>
                                                                        <th>
                                                                            <div class="main-title-left">
                                                                                <img class="img-left" title="SEO" alt="SEO" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                <h2>
                                                                                    SEO</h2>
                                                                            </div>
                                                                        </th>
                                                                    </tr>
                                                                    <tr>
                                                                        <td id="tdSEO">
                                                                            <div id="tab-container" class="slidingDivSEO">
                                                                                <ul class="menu">
                                                                                    <li class="active" id="ordernotes1" onclick='$("#ordernotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.order-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                        Page Title</li>
                                                                                    <li id="privatenotes1" onclick='$("#ordernotes1").removeClass("active");$("#privatenotes1").addClass("active"); $("#giftnotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.private-notes").fadeIn();$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                        Keywords</li>
                                                                                    <li id="giftnotes1" onclick='$("#giftnotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("#myaccount1").removeClass("active");$("div.gift-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");$("div.my-account").css("display", "none");'>
                                                                                        Description</li>
                                                                                </ul>
                                                                                <span class="clear"></span>
                                                                                <div class="tab-content-2 order-notes">
                                                                                    <div class="tab-content-3">
                                                                                        <asp:TextBox ID="txtSETitle" runat="server" CssClass="order-textfield" Width="100%"
                                                                                            Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox></div>
                                                                                </div>
                                                                                <div class="tab-content-2 private-notes">
                                                                                    <div class="tab-content-3">
                                                                                        <asp:TextBox ID="txtSEKeywords" runat="server" CssClass="order-textfield" Width="100%"
                                                                                            Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tab-content-2 gift-notes">
                                                                                    <div class="tab-content-3">
                                                                                        <asp:TextBox ID="txtSEDescription" runat="server" CssClass="order-textfield" Width="100%"
                                                                                            Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox></div>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="center">
                                                <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                    OnClick="btnSave_Click" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:ImageButton ID="btnCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                    OnClick="btnCancle_Click" CausesValidation="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
