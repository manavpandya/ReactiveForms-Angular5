<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="WareHouseVendormail.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.WareHouseVendormail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        function Validate() {
            if (document.getElementById('ContentPlaceHolder1_txtFrom') != null && (document.getElementById('ContentPlaceHolder1_txtFrom').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter From Email');
                document.getElementById('ContentPlaceHolder1_txtFrom').focus();
                return false;
            }
            else if ((document.getElementById('ContentPlaceHolder1_txtFrom').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtFrom').value)) {
                alert('Please Enter valid E-Mail Address');
                document.getElementById('ContentPlaceHolder1_txtFrom').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtTo') != null && (document.getElementById('ContentPlaceHolder1_txtTo').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter To Email');
                document.getElementById('ContentPlaceHolder1_txtTo').focus();
                return false;
            }
            else if ((document.getElementById('ContentPlaceHolder1_txtTo').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtTo').value)) {
                alert('Please Enter valid E-Mail Address');
                document.getElementById('ContentPlaceHolder1_txtTo').focus();
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtSubject') != null && (document.getElementById('ContentPlaceHolder1_txtSubject').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter Subject');
                document.getElementById('ContentPlaceHolder1_txtSubject').focus();
                return false;
            }
            return true;
        }
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                testresults = true
            else {
                testresults = false
            }
            return (testresults)
        }

        function printAllCheck() {
            //if (document.getElementById('getprint')) {
            var pri = document.getElementById("ifmcontentstoprint").contentWindow;
            pri.document.open();
            var contentAll = document.getElementById("cke_contents_ContentPlaceHolder1_txtDescription").innerHTML;  //CKEDITOR.instances.txtDescription.getData();
            pri.document.write(contentAll);
            pri.document.close();
            pri.focus();
            pri.print();
            // }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
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
                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
                                        style="padding: 2px;">
                                        <tbody>
                                            <tr>
                                                <th>
                                                    <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 0px;">
                                                        Vendor Email for Warehouse P.O.
                                                    </div>
                                                    <div class="main-title-right">
                                                    </div>
                                                </th>
                                            </tr>
                                            <tr>
                                                <td style="padding: 2px; border: 1px solid #e7e7e7;">
                                                    <table cellpadding="1" cellspacing="2" width="100%">
                                                        <tr>
                                                            <td style="width: 100px;">
                                                            </td>
                                                            <td align="right">
                                                                <asp:ImageButton ID="btnPrint" Visible="false" OnClientClick="printAllCheck();" runat="server" />&nbsp;&nbsp;&nbsp;
                                                                <asp:ImageButton ID="btnSendmailToVendor" runat="server" OnClientClick="return Validate();"
                                                                    OnClick="btnSendmailToVendor_Click" />&nbsp;&nbsp;&nbsp; <a style="padding-right: 10px;"
                                                                        href="javascript:history.go(-1);">
                                                                        <img src="/App_Themes/<%=Page.Theme %>/images/back.png" alt="Go to Generate Warehouse PO"
                                                                            title="Go to Generate Warehouse PO" />
                                                                    </a>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px;">
                                                                <span class="star">*</span>From :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFrom" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="star">*</span>To :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTo" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="star">*</span>Subject :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSubject" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="star">&nbsp;</span>CC :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCC" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="star">&nbsp;</span>BCC :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBCC" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <span class="star">&nbsp;</span>Attach PDF? :
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkPdfFile" runat="server" Checked="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                &nbsp;
                                                            </td>
                                                            <td valign="top">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td class="ckeditor-table">
                                                                            <div id="getprint">
                                                                                <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                                    Rows="10" Columns="80" runat="server" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                            </div>
                                                                            <div id="divScript" runat="server" visible="false" style="padding-left: 5px;">
                                                                                <script type="text/javascript">
                                                                                    CKEDITOR.replace('<%= txtDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
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
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="divPrint" style="display: none;">
                                        <asp:Literal ID="ltPrint" runat="server"></asp:Literal>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="visibility: hidden">
            <iframe id="ifmcontentstoprint"></iframe>
        </div>
    </div>
</asp:Content>
