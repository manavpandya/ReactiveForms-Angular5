<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="SalesRepresentiveReport.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Reports.SalesRepresentiveReport" ValidateRequest="false" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
<script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
<script type="text/javascript">
    function checkValidation() {

        if (document.getElementById('ContentPlaceHolder1_txtname') != null && document.getElementById('ContentPlaceHolder1_txtname').value == '') {
            jAlert('Please Enter Name.', 'Required Information', 'ContentPlaceHolder1_txtname');
           // document.getElementById('ContentPlaceHolder1_txtname').focus();
            return false;
        }

        if (document.getElementById('prepage') != null) {
            document.getElementById('prepage').style.display = '';
        }
        return true;
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
                                                    <img class="img-left" title="Customer" alt="Customer" src="/App_Themes/<%=Page.Theme %>/icon/report_icon.gif" />
                                                    <h2>
                                                        Sales Representative Report</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left">
                                                <table cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td align="left">
                                                            Store :
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="175px" AutoPostBack="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #E3E3E3;font-weight: bold;">
                                                Search By
                                            </td>
                                        </tr>
                                     <tr>
                                            <td style="font-weight: bold" Width="100%" align="left">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                <td align="left">
                                                Enter Rep. Name
                                                </td>
                                                <td align="left">
                                                <asp:TextBox ID="txtname" CssClass="order-textfield" runat="server" Width="300px"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                 <asp:ImageButton ID="imgSearch" runat="server" AlternateText="Search" ToolTip="Search"
                                                    OnClick="imgSearch_Click" OnClientClick="return checkValidation();" />
                                                </td>
                                                <td align="left"><asp:ImageButton ID="imgShowAll" runat="server"  OnClientClick="javascript:document.getElementById('ContentPlaceHolder1_txtname').value = '';if (document.getElementById('prepage') != null) {document.getElementById('prepage').style.display = '';}"
                                                        AlternateText="Show All" ToolTip="Show All" onclick="imgShowAll_Click"  
                                                      /></td>
                                                </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="grvSalesrepReport" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                    CellPadding="2" CellSpacing="0" GridLines="None" Width="90%" PageSize="20" 
                                                      BorderColor="#E7E7E7"  ShowFooter="false"
                                                    BorderWidth="1px" AllowPaging="false"
                                                    EmptyDataText="No Record found for this Representative Name or entered Representative Name is wrong!" EmptyDataRowStyle-ForeColor="Red" 
                                                    EmptyDataRowStyle-HorizontalAlign="Center"
                                                    PagerSettings-Mode="NumericFirstLast">
                                                     
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Store Name</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                               
                                                                <asp:Label ID="lblStoreName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"StoreName") %>'></asp:Label>

                                                            </ItemTemplate>
                                                            
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Sales Rep. Name</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SalesRepName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                             
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Order Number(s)</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                 <asp:Label ID="lblTotalOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"OrderNumber") %>'></asp:Label>
                                                                
                                                            </ItemTemplate>
                                                            
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="left" CssClass="border" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <strong>Total Amount</strong>
                                                            </HeaderTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderamount" runat="server" Text='<%# String.Format("{0:C}",Convert.ToDecimal(Eval("OrderTotal"))) %>'></asp:Label>
                                                            </ItemTemplate>
                                                         
                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Right" CssClass="border" />
                                                        </asp:TemplateField>
                                                        
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                    <FooterStyle ForeColor="White" Font-Bold="true"  BackColor="#545454" />
                                                </asp:GridView>
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
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 200px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
