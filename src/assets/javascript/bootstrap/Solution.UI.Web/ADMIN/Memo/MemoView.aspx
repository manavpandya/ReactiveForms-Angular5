<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemoView.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Memo.MemoView" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="MemoView.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Memo.MemoView"
    ValidateRequest="false" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type='text/javascript' src='../fullcalendar/jquery-1.8.1.min.js'></script>
    <script type='text/javascript' src='../jquery-ui-1.8.23.custom.min.js'></script>
    <script type='text/javascript' src='../fullcalendar/fullcalendar.min.js'></script>
    <script type='text/javascript' src='../fullcalendar/popup.js'></script>
    <link rel='stylesheet' type='text/css' href='../fullcalendar/fullcalendar.css' />
    <link rel='stylesheet' type='text/css' href='../fullcalendar/fullcalendar.print.css'
        media='print' />
    <style type='text/css'>
        #calendar
        {
            width: 90%;
            margin: 0 auto;
        }
    </style>
    <script type='text/javascript'>
        function ModalPopupShow(id) {

            document.getElementById("<%= hdnValue.ClientID %>").value = id;
            var clickButton = document.getElementById("<%= btnPopup.ClientID %>");
            clickButton.click();
            // __doPostBack('Button1_Click', 'OnClick');
        }
    </script>
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;</div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
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
                                                <img class="img-left" title="Internal Memo" alt="Internal Memo" src="/App_Themes/<%=Page.Theme %>/Images/header-links-icon.png" />
                                                <h2>
                                                    <asp:Label runat="server" Text="Internal Memo" ID="lblTitle"></asp:Label></h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right">
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <div id='calendar'>
                                                <asp:Literal ID="ltrEventCalendar" runat="server"></asp:Literal></div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="10" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" alt="" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="popupContact" style="z-index: 1000001; width: 500px; display: none;">
        <table width="100%" border="0" cellspacing="0" cellpadding="3" class="popupborder">
            <tr style="height: 25px;" class="popupbackground">
                <td valign="middle" align="left">
                    &nbsp;Memo View
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="popupContactClose" Style="position: relative; top: 0;" ImageUrl="../images/cancel-icon.png"
                        runat="server" ToolTip="Close" OnClientClick="return false;document.getElementById('ContentPlaceHolder1_lblShowPassword').value='';">
                    </asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <table width="100%" border="0" cellpadding="5" cellspacing="0">
                        <tr class="">
                            <td style="text-align: left; width: 20%;">
                                <strong>Subject :</strong>
                            </td>
                            <td style="text-align: left; width: 80%;">
                                <asp:Label ID="lblMemoTitle" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="">
                            <td style="text-align: left; width: 20%;">
                                <strong>Status :</strong>
                            </td>
                            <td style="text-align: left; width: 80%;">
                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="">
                            <td style="text-align: left; width: 20%;">
                                <strong>Start Date :</strong>
                            </td>
                            <td style="text-align: left; width: 80%;">
                                <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="">
                            <td style="text-align: left; width: 20%; vertical-align: top">
                                <strong>Description :</strong>
                            </td>
                            <td style="text-align: left; width: 80%;">
                                <asp:Label ID="lblDescription" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="">
                            <td style="text-align: left; vertical-align: top;" colspan="2">
                                <asp:Literal ID="ltrMoreDetails" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <%--  <tr class="">
                            <td style="text-align: left;width:20%;">
                                End Date :
                            </td>
                            <td style="text-align: left;width:80%;">
                                <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                            </td>
                        </tr>--%>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div style="display: none;">
        <asp:HiddenField ID="hdnValue" runat="server" />
        <asp:Button ID="btnPopup" runat="server" OnClick="btnPopup_Click" Text="Button" />
    </div>
</asp:Content>
