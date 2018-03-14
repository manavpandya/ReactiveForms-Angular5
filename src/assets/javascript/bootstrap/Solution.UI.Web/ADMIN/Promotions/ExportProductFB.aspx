<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ExportProductFB.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.ExportProductFB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <style type="text/css">
        #listCat {
            width: 288px;
        }

        .chklistCat label {
            padding-left: 10px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function CheckCategory() {
            //if (document.getElementById('ContentPlaceHolder1_ddlCategory').value == '' || document.getElementById('ContentPlaceHolder1_ddlCategory').value == '0') {
            //    jAlert('Please select category.', 'Message', 'ContentPlaceHolder1_ddlCategory');
            //    return false;
            //}
            //return true;
        }

        function checkallchkbox(id, id1, id2, id3, id4, id5, id6) {
            if (document.getElementById(id1).checked && document.getElementById(id2).checked && document.getElementById(id3).checked) {
                document.getElementById(id).checked = true;
            }
            else {
                document.getElementById(id).checked = false;
            }
        }

        function checkalladmin(id, id1, id2, id3, id4, id5, id6) {
            if (document.getElementById(id).checked) {
                document.getElementById(id1).checked = true;
                document.getElementById(id2).checked = true;
                document.getElementById(id3).checked = true;
            }
            else {
                document.getElementById(id1).checked = false;
                document.getElementById(id2).checked = false;
                document.getElementById(id3).checked = false;
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
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
                                                <h2>Export Product CSV for Facebook
                                                </h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td valign="middle" align="left"></td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr class="oddrow">
                                                    <td style="width: 10%">Category Name:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCategory" runat="server" Width="185px" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                                            CssClass="order-list" Style="margin-left: 0px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 10%">Sub Category Name:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSubCategory" runat="server" Width="185px" AutoPostBack="false"
                                                            CssClass="order-list" Style="margin-left: 0px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <tr class="oddrow">
                                                    <td style="width: 10%">Stock Status:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlStock" runat="server" Width="185px" AutoPostBack="false"
                                                            CssClass="order-list" Style="margin-left: 0px">
                                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="In Stock" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Out of stock" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td style="width: 10%">Product Type:
                                                    </td>
                                                    <td>
                                                        <p style="float: left; padding: 5px;">
                                                            <asp:CheckBox ID="chkSelectAll" Text="Select All" AutoPostBack="false" runat="server" Checked="false" onchange="checkalladmin('ContentPlaceHolder1_chkSelectAll','ContentPlaceHolder1_chkReadyMade','ContentPlaceHolder1_chkCustom','ContentPlaceHolder1_chkHardware','ContentPlaceHolder1_chkSwatch','ContentPlaceHolder1_chkRoman');" />
                                                        </p>
                                                        <p style="float: left; padding: 5px;">
                                                            <asp:CheckBox ID="chkReadyMade" runat="server" Text="Stock Products" AutoPostBack="false" Checked="false" onchange="checkallchkbox('ContentPlaceHolder1_chkSelectAll','ContentPlaceHolder1_chkReadyMade','ContentPlaceHolder1_chkCustom','ContentPlaceHolder1_chkHardware','ContentPlaceHolder1_chkSwatch','ContentPlaceHolder1_chkRoman');" />
                                                        </p>
                                                        <p style="float: left; padding: 5px; display: none;">
                                                            <asp:CheckBox ID="chkMadetoOrder" runat="server" Text="Made to Order" AutoPostBack="false" Checked="false" onchange="checkallchkbox('ContentPlaceHolder1_chkSelectAll','ContentPlaceHolder1_chkReadyMade','ContentPlaceHolder1_chkCustom','ContentPlaceHolder1_chkHardware','ContentPlaceHolder1_chkSwatch','ContentPlaceHolder1_chkRoman');" />
                                                        </p>
                                                        <p style="float: left; padding: 5px;">
                                                            <asp:CheckBox ID="chkCustom" runat="server" Text="Custom Products" AutoPostBack="false" Checked="false" onchange="checkallchkbox('ContentPlaceHolder1_chkSelectAll','ContentPlaceHolder1_chkReadyMade','ContentPlaceHolder1_chkCustom','ContentPlaceHolder1_chkHardware','ContentPlaceHolder1_chkSwatch','ContentPlaceHolder1_chkRoman');" />
                                                        </p>
                                                        <p style="float: left; padding: 5px;">
                                                            <asp:CheckBox ID="chkHardware" runat="server" Text="Hardware Products (Special Orders)" AutoPostBack="false" Checked="false" onchange="checkallchkbox('ContentPlaceHolder1_chkSelectAll','ContentPlaceHolder1_chkReadyMade','ContentPlaceHolder1_chkCustom','ContentPlaceHolder1_chkHardware','ContentPlaceHolder1_chkSwatch','ContentPlaceHolder1_chkRoman');" />
                                                        </p>
                                                        <p style="float: left; padding: 5px;">
                                                            <asp:CheckBox ID="chkSwatch" runat="server" AutoPostBack="false" Text="Swatch Products" Enabled="false" Checked="false" />
                                                        </p>
                                                        <p style="float: left; padding: 5px;">
                                                            <asp:CheckBox ID="chkRoman" runat="server" AutoPostBack="false" Text="Roman Shades Products" Enabled="false" Checked="false" />
                                                        </p>
                                                    </td>
                                                </tr>
                                                <tr class="oddrow">
                                                    <td style="width: 10%">Last Updated:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLastUpdated" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="altrow">
                                                    <td align="left" valign="top"></td>
                                                    <td align="left" style="padding: 3px; padding-left: 0px;">
                                                        <table width="630px">
                                                            <tr>
                                                                <td align="left"></td>
                                                                <td align="right" style="padding-left: 0px;">
                                                                    <asp:Button ID="btnSave" runat="server" ToolTip="Save for Export" OnClientClick="return CheckCategory();"
                                                                        OnClick="btnSave_Click" />
                                                                    <asp:Button ID="btnExport" runat="server" ToolTip="Export" OnClientClick="return CheckCategory();" Visible="false"
                                                                        OnClick="btnExport_Click" />
                                                                     <asp:Button ID="Button2" runat="server" ToolTip="New Arrival Save for Export" OnClientClick="return CheckCategory();"
                                                                        OnClick="btnSaveNew_Click" />
                                                                   
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
