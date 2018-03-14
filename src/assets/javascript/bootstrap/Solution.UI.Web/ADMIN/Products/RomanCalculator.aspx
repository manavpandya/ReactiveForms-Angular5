<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RomanCalculator.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.RomanCalculator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts.js"></script>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <link href="../../App_Themes/Gray/css/jquery.alerts.css" type="text/css" rel="stylesheet" />
    <link href="../../App_Themes/Gray/css/jquery-ui-1.8.custom.css" type="text/css" rel="stylesheet" />
    <link href="../../App_Themes/Gray/css/jqx.base.css" type="text/css" rel="stylesheet" />
    <link href="../../App_Themes/Gray/css/jqx.summer.css" type="text/css" rel="stylesheet" />
    <link href="../../App_Themes/Gray/css/popup.css" type="text/css" rel="stylesheet" />
    <link href="../../App_Themes/Gray/css/style.css" type="text/css" rel="stylesheet" />
     <script type="text/javascript">
         function keyRestrict(e, validchars) {
             var key = '', keychar = '';
             key = getKeyCode(e);
             if (key == null) return true;
             keychar = String.fromCharCode(key);
             keychar = keychar.toLowerCase();
             validchars = validchars.toLowerCase();
             if (validchars.indexOf(keychar) != -1)
                 return true;
             if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
                 return true;
             return false;
         }

         function getKeyCode(e) {
             if (window.event)
                 return window.event.keyCode;
             else if (e)
                 return e.which;
             else
                 return null;
         }
    </script>
    <script type="text/javascript">
        function closeWin() {
            window.close();
        }
        function ValidatePage() {

            if ('<%=Request.QueryString["type"]%>' == 'length') {
              
                if (document.getElementById('ddlRoman') != null && document.getElementById('ddlRoman').selectedIndex == 0) {
                    //if ((document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Roman Type.', 'Message', 'ddlRoman');
                    return false;
                    //}
                }
                else if (document.getElementById('txtlengthfrom') != null && document.getElementById('txtlengthfrom').value.replace(/^\s*\s*$/g, '') == '') {
                    //if ((document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Length From.', 'Message', 'txtlengthfrom');
                    return false;
                    //}
                }
                else if (document.getElementById('txtlengthto') != null && document.getElementById('txtlengthto').value.replace(/^\s*\s*$/g, '') == '') {
                    //if ((document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Length to.', 'Message', 'txtlengthto');
                    return false;
                    //}
                }
                else if (document.getElementById('txtlengthto') != null && document.getElementById('txtlengthfrom') != null && parseFloat(document.getElementById('txtlengthfrom').value) > parseFloat(document.getElementById('txtlengthto').value))
                {
                    jAlert('Please Enter valid  length range.', 'Message', 'txtlengthfrom');
                    return false;
                }
                else if (document.getElementById('txtlengthprice') != null && (document.getElementById('txtlengthprice').value.replace(/^\s*\s*$/g, '') == '')) {
                    //if ((document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Length price.', 'Message', 'txtlengthprice');
                    return false;
                    //}
                }

            }
            else {
                if (document.getElementById('ddlRoman') != null && document.getElementById('ddlRoman').selectedIndex == 0) {
                    //if ((document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Roman Type.', 'Message', 'ddlRoman');
                    return false;
                    //}
                }

                else if (document.getElementById('txtwidthfrom') != null && document.getElementById('txtwidthfrom').value.replace(/^\s*\s*$/g, '') == '') {
                    //if ((document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Width From.', 'Message', 'txtwidthfrom');
                    return false;
                    //}
                }
                else if (document.getElementById('txtwidthto') != null && document.getElementById('txtwidthto').value.replace(/^\s*\s*$/g, '') == '') {
                    //if ((document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Width To.', 'Message', 'txtwidthto');
                    return false;
                    //}
                }
                else if (document.getElementById('txtwidthto') != null && document.getElementById('txtwidthfrom') != null && parseFloat(document.getElementById('txtwidthfrom').value) > parseFloat(document.getElementById('txtwidthto').value)) {
                    jAlert('Please Enter valid width range.', 'Message', 'txtwidthto');
                    return false;
                }
                else if (document.getElementById('txtWidthPrice') != null && document.getElementById('txtWidthPrice').value.replace(/^\s*\s*$/g, '') == '') {
                    //if ((document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Width Price.', 'Message', 'txtWidthPrice');
                    return false;
                    //}
                }

            }


            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="SM1" runat="server">
            </asp:ScriptManager>
            <div class="content-row2">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td valign="top" align="left" height="5">
                                <img src="/App_Themes/Gray/images/spacer.gif" width="1" height="5">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" height="5">
                                <img src="/App_Themes/Gray/images/spacer.gif" width="1" height="5">
                            </td>
                        </tr>
                        <tr>
                            <td class="border-td">
                                <table class="content-table" width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#FFFFFF">
                                    <tbody>
                                        <tr>
                                            <td class="border-td-sub">
                                                <table class="add-product" width="100%" cellspacing="0" cellpadding="0" border="0">
                                                    <tbody>
                                                        <tr>
                                                            <th colspan="4">
                                                                <div class="main-title-left">
                                                                    <img class="img-left" title="Roman Calculator" alt="Roman Calculator" src="/App_Themes/Gray/Images/admin-list-icon.png">
                                                                    <h2>Roman Calculator</h2>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="content-table ">
                                                                    <tr class="oddrow">

                                                                        <td align="left">
                                                                            <span class="star"></span>Fabric Category Name :
                                                                        </td>

                                                                        <td align="left">
                                                                            <asp:DropDownList ID="ddlRoman" runat="server" class="product-type"   AutoPostBack="false">
                                                                            </asp:DropDownList></td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr class="evenrow" id="trlngthfrom" runat="server">
                                                                        <td align="left">
                                                                            <span class="star"></span>Length  From :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtlengthfrom" onkeypress="return keyRestrict(event,'0123456789.');" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                                            <br />
                                                                           
                                                                        </td>
                                                                        <td align="left">
                                                                            <span class="star"></span>Length  To :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtlengthto" onkeypress="return keyRestrict(event,'0123456789.');" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                                            <br />
                                                                           
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr class="oddrow" id="trlngthprice" runat="server">
                                                                        <td align="left">
                                                                            <span class="star"></span>Length Starting  Price :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtlengthprice" onkeypress="return keyRestrict(event,'0123456789.');" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                                            <br />
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr class="evenrow" id="trWidthfrom" runat="server">
                                                                        <td align="left">
                                                                            <span class="star"></span>Width  From :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtwidthfrom" onkeypress="return keyRestrict(event,'0123456789.');" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                                            <br />
                                                                            
                                                                        </td>
                                                                        <td align="left">
                                                                            <span class="star"></span>Width  To :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtwidthto" onkeypress="return keyRestrict(event,'0123456789.');" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                                            <br />
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr class="oddrow" id="trWidthprice" runat="server">
                                                                        <td align="left">
                                                                            <span class="star"></span>Width Starting  Price :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtWidthPrice" onkeypress="return keyRestrict(event,'0123456789.');" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                                            <br />
                                                                            
                                                                        </td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr align="center">
                                                                        <td colspan="4">
                                                                            <asp:ImageButton ID="btnSave" runat="server" src="/App_Themes/gray/images/save.gif" OnClientClick="return ValidatePage();" OnClick="btnSave_Click" />
                                                                            <asp:ImageButton ID="btnupdate" runat="server" Visible="false" src="/App_Themes/gray/images/update.png" OnClick="btnupdate_Click" />
                                                                            <asp:ImageButton ID="btnCancle" runat="server" src="/App_Themes/gray/images/cancel.gif" OnClick="btnCancle_Click" />


                                                                            <%-- <asp:ImageButton ID="btnSave" runat="server" src="/App_Themes/gray/images/save.gif" />--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="content-row2">
                <asp:HiddenField ID="hdnfabcode" runat="server" />
                <asp:HiddenField ID="hdnfabcodeid" runat="server" />
                <asp:HiddenField ID="hdnRomanLengthId" runat="server" />
                <asp:HiddenField ID="hdnRomanWidhId" runat="server" />
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr class="even-row">
                        <td>
                            <asp:GridView ID="grdSearchProductType" runat="server" AutoGenerateColumns="False"
                                DataKeyNames="FabricCodeId" EmptyDataText="No Record(s) Found." AllowSorting="false"
                                EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center" OnPageIndexChanging="grdSearchProductType_PageIndexChanging"
                                ViewStateMode="Enabled" Width="100%" OnDataBound="grdSearchProductType_DataBound" OnRowCommand="grdSearchProductType_RowCommand" OnRowDeleting="grdSearchProductType_RowDeleting"
                                GridLines="None" AllowPaging="True" PageSize="50" PagerSettings-Mode="NumericFirstLast" OnRowEditing="grdSearchProductType_RowEditing" ForeColor="Black" OnRowDataBound="grdSearchProductType_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Code">
                                        <HeaderTemplate>
                                            &nbsp;Code
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="lblFabricCodeId" runat="server" Visible="false" Text='<%# Bind("FabricCodeId") %>'></asp:Label>
                                            &nbsp;<asp:Label ID="lblRomanLengthId" runat="server" Visible="false" Text='<%# Bind("RomanLengthId") %>'></asp:Label>
                                            &nbsp;<asp:Label ID="lblRomanWidhId" runat="server" Visible="false" Text='<%# Bind("RomanWidhId") %>'></asp:Label>
                                            &nbsp;<asp:Label ID="lblCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                            <%-- <asp:Label ID="lblStoreId" runat="server" Visible="false" Text='<%# Bind("StoreId") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            &nbsp;Type
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="lblTypeName" runat="server" Text='<%#(DataBinder.Eval(Container.DataItem,"TypeName")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            &nbsp;Width From
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="lblWidthfrom" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Widthfrom")),2) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            &nbsp;Width To
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="lblWidthTo" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"WidthTo")),2) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            &nbsp;Width Price($)
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="lblPrice1" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price1")),2) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            &nbsp;Length From
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="lblLengthFrom" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"LengthFrom")),2) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            &nbsp;Length To
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="lblLengthTo" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"LengthTo")),2) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            &nbsp;Length Price ($)
                                        </HeaderTemplate>
                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                        <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                        <ItemTemplate>
                                            &nbsp;<asp:Label ID="lblPrice" runat="server" Text='<%# Math.Round(Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Price")),2) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Status" >
                                                            <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgIsActive" runat="server" AlternateText='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active")).ToString() %>'
                                                                    ToolTip='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active")).ToString() %>'
                                                                    ImageUrl='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Operations">
                                        <ItemStyle HorizontalAlign="Center" Width="8%" />
                                        <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                CommandArgument='<%# Eval("FabricCodeId") %>' src="/App_Themes/Gray/Images/Edit.gif"></asp:ImageButton>
                                              <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="Delete" src="/App_Themes/Gray/Images/delete-icon.png"
                                                                                                    CommandArgument='<%# Eval("FabricCodeId") %>' message="Are you sure want to delete this record?"
                                                                                                    OnClientClick='return confirm(this.getAttribute("message"))'></asp:ImageButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                <AlternatingRowStyle CssClass="altrow" />
                                <HeaderStyle ForeColor="black" Font-Bold="false" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
