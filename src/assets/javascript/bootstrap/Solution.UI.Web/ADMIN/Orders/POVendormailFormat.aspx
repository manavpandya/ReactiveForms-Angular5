<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POVendormailFormat.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.POVendormailFormat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script type="text/javascript">
        function Validate() {
            if (document.getElementById('txtFrom') != null && (document.getElementById('txtFrom').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter From Email');
                document.getElementById('txtFrom').focus();
                return false;
            }
            else if ((document.getElementById('txtFrom').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('txtFrom').value)) {
                alert('Please Enter valid E-Mail Address');
                document.getElementById('txtFrom').focus();
                return false;
            }
            else if (document.getElementById('txtTo') != null && (document.getElementById('txtTo').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter To Email');
                document.getElementById('txtTo').focus();
                return false;
            }
            else if ((document.getElementById('txtTo').value).replace(/^\s*\s*$/g, '') != '' && !checkemail1(document.getElementById('txtTo').value)) {
                alert('Please Enter valid E-Mail Address');
                document.getElementById('txtTo').focus();
                return false;
            }
            else if (document.getElementById('txtSubject') != null && (document.getElementById('txtSubject').value).replace(/^\s*\s*$/g, '') == '') {
                alert('Please Enter Subject');
                document.getElementById('txtSubject').focus();
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
            if (document.getElementById('getprint')) {
                var pri = document.getElementById("ifmcontentstoprint").contentWindow;
                pri.document.open();
                var contentAll = CKEDITOR.instances.txtDescription.getData();
                pri.document.write(contentAll);
                pri.document.close();
                pri.focus();
                pri.print();
            }
            return false;
        }
    </script>
</head>
<body style="background: none; font-size: 12px;">
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" class="table-none-border" style="border: none !important;">
            <tr>
                <td style="padding: 2px; border: 1px solid #e7e7e7;">
                    <table cellpadding="1" cellspacing="2" width="100%">
                        <tr>
                            <td style="width: 100px;">
                            </td>
                            <td align="right">
                                <asp:ImageButton ID="btnPrint" OnClientClick="printAllCheck();" runat="server" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="btnSendmailToVendor" runat="server" OnClientClick="return Validate();"
                                    OnClick="btnSendmailToVendor_Click" />&nbsp;&nbsp;&nbsp; <a style="padding-right: 10px;"
                                        href="javascript:history.go(-1);">
                                        <img src="/App_Themes/<%=Page.Theme %>/images/back.png" alt="Go to Generate PO" title="Go to Generate Warehouse PO" />
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
        </table>
        <div style="visibility: hidden">
            <iframe id="ifmcontentstoprint"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
