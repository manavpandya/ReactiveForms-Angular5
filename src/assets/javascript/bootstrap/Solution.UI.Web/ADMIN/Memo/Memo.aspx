<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="Memo.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Memo.Memo" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../../ckeditor/ckeditor.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">

        function checkDescription() {
            var oEditor = CKEDITOR.instances.ContentPlaceHolder1_txtDescription.getData();
            if (oEditor == '') {
                jAlert('Please Enter Memo Description.', 'Message', 'CKEDITOR.instances.ContentPlaceHolder1_txtDescription');
                CKEDITOR.instances.ContentPlaceHolder1_txtDescription.focus();
                return false;
            }
            chkHeight();
            return true;
        }

        function chkHeight() {

            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }

        function ValidatePage() {
            if (document.getElementById("ContentPlaceHolder1_txtTitle") != null && document.getElementById("ContentPlaceHolder1_txtTitle").value == '') {
                jAlert('Please enter Title.', 'Message', 'ContentPlaceHolder1_txtTitle');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtTitle').offset().top }, 'slow');
                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_ddlAssignTo") != null && document.getElementById("ContentPlaceHolder1_ddlAssignTo").selectedIndex == 0) {
                jAlert('Please select Assign To.', 'Message', 'ContentPlaceHolder1_ddlAssignTo');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ddlAssignTo').offset().top }, 'slow');
                return false;
            }

            return checkDescription();

        }

    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                                    <img class="img-left" title="Add Memo" alt="Add Memo" src="/App_Themes/<%=Page.Theme %>/Images/add-topic-icon.png" />
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Add Memo"></asp:Label></h2>
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
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Title:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtTitle" CssClass="order-textfield" MaxLength="50"
                                                                Width="350px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Start Date:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblStartDate" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Status:
                                                        </td>
                                                        <td>
                                                            Open
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Assign To:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAssignTo" runat="server" CssClass="order-list" Width="300px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvAssignTo" runat="server" ControlToValidate="ddlAssignTo"
                                                                ErrorMessage="Select Assign To" SetFocusOnError="True" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top" style="width:12%" >
                                                            <span class="star">&nbsp;&nbsp;</span>Description:
                                                        </td>
                                                        <td class="ckeditor-table">
                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                            <script type="text/javascript">
                                                                CKEDITOR.replace('<%= txtDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 300, toolbarStartupExpanded: false });
                                                                CKEDITOR.on('dialogDefinition', function (ev) {
                                                                    if (ev.data.name == 'image') {
                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                        btn.hidden = false;
                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                    }
                                                                    if (ev.data.name == 'link') {
                                                                        var btn = ev.data.definition.getContents('info').get('browse');
                                                                        btn.hidden = false;
                                                                        btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                    }
                                                                });
                                                            </script>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Attach Document:
                                                        </td>
                                                        <td>
                                                            <asp:FileUpload ID="uploadMemoDocument" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="btnSave_Click" OnClientClick="return ValidatePage();" />
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                OnClick="btnCancel_Click" CausesValidation="false" />
                                                            <br />
                                                            <br />
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
            </table>
        </div>
    </div>
</asp:Content>
