<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="SubjectStatus.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.SubjectStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        var testresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                testresults = true
            else {
                testresults = false
            }
            return (testresults)
        }
        function Checkfields() {

            if (document.getElementById('<%=txtSubject.ClientID%>').value == '') {

                jAlert('Please enter Subject Name.', 'Message1', '<%=txtSubject.ClientID%>');
                return false;

            }
            else if (document.getElementById('<%=txtEmailID.ClientID%>').value == '') {

                jAlert('Please enter EmailID.', 'Message2', '<%=txtEmailID.ClientID%>');
                return false;
            }
            <%--else if ((document.getElementById('<%=txtEmailID.ClientID%>').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('<%=txtEmailID.ClientID%>').value)) {
                jAlert('Please enter Valid EmailID.', 'Message3', '<%=txtEmailID.ClientID%>');
                return false;
            }--%>
        return true;
    }

    function clearfield() {

        if (document.getElementById("ContentPlaceHolder1_txtSubject") != null) { document.getElementById("ContentPlaceHolder1_txtCountryName").value = ''; }
        if (document.getElementById("ContentPlaceHolder1_txtEmailID") != null) { document.getElementById("ContentPlaceHolder1_txttwoISOCode").value = ''; }
       
        return false;
    }
    </script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5">
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
                                                    <img class="img-left" title="Email Configuration" alt="Email Configuration" src="/App_Themes/<%=Page.Theme %>/Images/country-list-icon.png">
                                                    <h2>
                                                        <asp:Label runat="server" Text="Email Configuration" ID="lblTitle"></asp:Label></h2>
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
                                                        <td>
                                                            <span class="star">*</span>Subject:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtSubject" CssClass="order-textfield" MaxLength="200"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>EmailID:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtEmailID" CssClass="order-textfield" TextMode="Multiline" style="width:500px;height:100px;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        <span class="star">&nbsp;</span>Show On ContactUS:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="" />
                                                        </td>
                                                    </tr>
                                                  
                                                    
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 80%">
                                                        <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClick="imgSave_Click" OnClientClick="return Checkfields();" />
                                                        <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            OnClick="imgCancle_Click" />
                                                    </td>
                                                    </tr>
                                                </table>
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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
