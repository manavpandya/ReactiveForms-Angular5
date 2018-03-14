<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductvariantFabric.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductvariantFabric" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title></title>
    <script type="text/javascript">
        function ChkValidate() {
            if (document.getElementById('ddlProductColor') != null && document.getElementById("ddlProductColor").selectedIndex == 0) {
                alert('Please Select Product Color', 'Message');
                document.getElementById('ddlProductColor').focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                          Select Fabric
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr id="trAddSeletedItems" runat="server">
                    <td style="text-align: right; height: 30px; padding: 2px; padding-right: 4px;">
                        <div class="slidingDivImage" style="padding-top: 8px; padding-bottom: 8px;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table cellpadding="1" cellspacing="1" width="100%">
                                            <tr>
                                                <td style="width: 100px;" align="right">
                                                    <span style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">Select
                                                        Fabric Type : </span>:
                                                </td>
                                                <td align="left" style="padding-left: 3px; width: 140px;">
                                                    <asp:DropDownList ID="ddlFabricType" runat="server" CssClass="order-list" Style="width: 135px;"
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="ddlFabricType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                               
                                            </tr>
                                             <tr>
                                                <td style="width: 100px;" align="right">
                                                    <span style="font-family: Arial,sans-serif; color: #212121; font-size: 13px">Select
                                                        Fabric Code : </span>:
                                                </td>
                                                <td align="left" style="padding-left: 3px; width: 140px;">
                                                   <asp:DropDownList ID="ddlFabricCode" runat="server" class="product-type"></asp:DropDownList>
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td style="width: 100px;">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="2">
                                                    &nbsp;<asp:ImageButton ID="btnAddToSelectionlist" runat="server" OnClientClick="return ChkValidate();"
                                                        OnClick="btnAddToSelectionlist_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
