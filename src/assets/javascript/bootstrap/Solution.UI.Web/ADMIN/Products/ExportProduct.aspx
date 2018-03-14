<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ExportProduct.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ImportProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <style type="text/css">
        #listCat
        {
            width: 288px;
        }
        .chklistCat label
        {
            padding-left: 10px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (on.toString() == 'false') {
                        if (myform.elements[i].checked) {
                            myform.elements[i].checked = false;
                        }
                    }
                    else {
                        myform.elements[i].checked = true;
                    }
                }
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        function checkCount() {
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Check at least One Column!', 'Message', ''); });
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
                <span style="vertical-align: middle; margin-right: 3px; float: left;">
                    <table style="margin-top: 5px; float: left">
                        <tr>
                            <td align="left">
                                Store :
                                <asp:DropDownList ID="ddlStore" runat="server" Width="185px" AutoPostBack="false"
                                    CssClass="order-list" Style="margin-left: 0px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </span><span style="vertical-align: middle; margin-right: 3px; float: right;"></span>
            </div>
        </div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt="" />
                </td>
            </tr>
            <tr>
                <td style="height: 5px" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" alt="" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table style="width: 100%; border: 0; bgcolor: #FFFFFF;" cellpadding="0" cellspacing="0"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Export Product CSV" alt="Export Product CSV" src="/App_Themes/<%=Page.Theme %>/Images/product_export.png" />
                                                <h2>
                                                    Export Product CSV
                                                </h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td valign="middle" align="left">
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr class="oddrow">
                                                    <td style="width: 10%">
                                                        Column Name :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBoxList ID="chklistCat" runat="server" CssClass="chklistCat" RepeatDirection="Horizontal"
                                                            RepeatColumns="6" Style="width: 650px; height: 305px;">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td align="left" valign="top">
                                                    </td>
                                                    <td align="left" style="padding: 3px; padding-left: 0px;">
                                                        <table width="630px">
                                                            <tr>
                                                                <td align="left">
                                                                    <span><a id="lkbAllowAll" href="javascript:selectAll(true);">Check All</a> | <a id="lkbClearAll"
                                                                        href="javascript:selectAll(false);">Clear All</a> </span>
                                                                </td>
                                                                <td align="right" style="padding-left: 0px;">
                                                                    <asp:Button ID="btnExport" runat="server" ToolTip="Export" OnClientClick="return checkCount();"
                                                                        OnClick="btnExport_Click" />
                                                                    <asp:Button ID="btnCancel" runat="server" ToolTip="Cancel" OnClick="btnCancel_Click" />
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
