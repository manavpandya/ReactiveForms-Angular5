<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateNewFolder.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.WebMail.CreateNewFolder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">

        function CheckEmail(address) {
            //var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{1,4})$/;
            var reg = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
            //var address = document.forms[form_id].elements[email].value;
            if (reg.test(address) == false) {
                //alert('Invalid Email Address');
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript" language="javascript">
        function chkselect() {
            var allElts = document.getElementById("trvfolderlist").getElementsByTagName('INPUT');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {
                jAlert('Please select atleast one folder.', 'Message');
                return false;
            }
            else if (Chktrue > 1) {
                jAlert('Please select only one folder.', 'Message');
                return false;
            }

            if (document.getElementById("txtdisplayname") != null && document.getElementById("txtdisplayname").value == '') {
                jAlert('Please Enter Display Name.', 'Message', 'txtdisplayname');
                return false;
            }

            if (document.getElementById('txtemailid') != null && document.getElementById('txtemailid').value == '') {
                jAlert('Please Enter Email Address.', 'Message', 'txtemailid');
                return false;
            }
            if (document.getElementById('txtemailid') != null && document.getElementById('txtemailid').value != '') {
                if (!CheckEmail(document.getElementById('txtemailid').value)) {
                    jAlert('Please Enter valid Email Address.', 'Message', 'txtemailid');
                    return false;
                }
            }
            return true;
        }

        function conclear() {
            var allElts = document.getElementById("trvfolderlist").getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = false;
                }
            }
            document.getElementById("txtdisplayname").value = '';
            document.getElementById("txtemailid").value = '';
            return false;
        }
    </script>
</head>
<body style="background: none;">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <form id="form1" runat="server">
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th colspan="3" valign="middle" style="padding: 0px">
                                            <div class="main-title-left">
                                                <h2 style="padding: 7px 0 0 0;">
                                                    Create New Folder
                                                </h2>
                                            </div>
                                            <div style="float: right; padding: 3px 5px 0 0;">
                                                <a href="javascript:void(0);" onclick="javascript:window.parent.document.getElementById('ContentPlaceHolder1_popupContactClose').click();"
                                                    title="Close">
                                                    <img src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" alt="Close" /></a>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td align="right" colspan="2">
                                            <span class="star">*</span>Required Field
                                        </td>
                                    </tr>
                                    <tr class="oddrow">
                                        <td>
                                            <span class="star">*</span>Folder List :
                                        </td>
                                        <td style="vertical-align: top">
                                            <div id="treeSelectedvalue" style="overflow: auto;">
                                                <asp:TreeView ID="trvfolderlist" runat="server" Width="150px" Height="100%" LeafNodeStyle-CssClass="LeafeNode"
                                                    NodeStyle-CssClass="Node" PopulateNodesFromClient="True" ShowLines="true">
                                                    <Nodes>
                                                    </Nodes>
                                                    <NodeStyle CssClass="Node" />
                                                    <LeafNodeStyle CssClass="LeafeNode" />
                                                </asp:TreeView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <span class="star">*</span>Display Name &nbsp;:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdisplayname" runat="server" CssClass="textfield_small" Width="292px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="oddrow">
                                        <td>
                                            <span class="star">*</span>Email ID :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtemailid" runat="server" CssClass="textfield_small" Width="292px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="altrow">
                                        <td valign="top">
                                            <span class="star">&nbsp;</span>
                                        </td>
                                        <td valign="top" style="vertical-align: top;">
                                            <asp:Button ID="btnSave" runat="server" Style="font-size: 9px; background: url(../images/Floppy-Drive-icon.png) no-repeat;
                                                width: 80px; height: 32px; border: 0; cursor: pointer; font-size: 12px;" Text="    Save"
                                                CssClass="" OnClientClick="return chkselect();" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="" OnClientClick="return conclear();"
                                                Style="font-size: 9px; background: url(../images/Status-dialog-error-icon.png) no-repeat;
                                                width: 100px; height: 32px; border: 0; cursor: pointer; font-size: 12px;" CausesValidation="False" />
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
    </form>
</body>
</html>
