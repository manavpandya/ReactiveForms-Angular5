<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewamazonrepricevendor.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.viewamazonrepricevendor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .hidediv{display:none;}
        .vendorcolor{background:lightgreen;}
        .right-1{float:right;}
    </style>
    <link href="/css/amazon-1.css" rel="stylesheet" />
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
            style="padding: 2px;">
            <tbody>
               
                <tr>
                    <th>
                        <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                        <div style="font-size:16px;color:#000;font-weight:bold;">Vendor Details</div>
                        </div>
                        <div class="main-title-right">
                            <a href="javascript:void(0);" class="show_hideMainDiv" runat="server" id="btnClose"
                                onclick="window.close();">
                                <img id="imgMainDiv" class="close" title="close" style="display:none;" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td style="padding: 2px">
                        Product Name: <asp:Label ID="lblname" runat="server"></asp:Label>
                        <br />
                        SKU:<asp:Label ID="lblsku" runat="server"></asp:Label>
                        <br />
                        ASIN:<asp:Label ID="lblasin" runat="server"></asp:Label>
                        </td>
                    </tr>
               
              
                <tr>
                    <td style="padding: 2px">
                        <div id="poorderprint" style="border: 5px solid #e7e7e7; overflow-y: auto; height: 250px;
                            padding-top: 2px;">
                            <asp:GridView ID="gvhemminglog" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataText="No Record Found." OnRowDataBound="gvhemminglog_RowDataBound"
                                class="order-table" Style="border: solid 1px #e7e7e7" ShowFooter="true" PageSize="30" AllowPaging="True" OnPageIndexChanging="gvhemminglog_PageIndexChanging"  >
                                <EmptyDataRowStyle HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <label style="color: Red">
                                        No records found ...</label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                        <div style="float:left;font-weight:bold;">  Vendor Name</div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                         <asp:Label ID="lblusername" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"vendorid") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                         <div class="right-1" style="font-weight:bold;"> Price</div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                          
                                            $<asp:Label ID="lblprice" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem,"price") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                        <HeaderStyle HorizontalAlign="right" />
                                    </asp:TemplateField>

                                      <asp:TemplateField>
                                        <HeaderTemplate>
                                         <div class="right-1"  style="font-weight:bold;"> Shipping</div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                          
                                            $<asp:Label ID="lblshipping" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem,"shipping") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                        <HeaderStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                     <asp:TemplateField>
                                        <HeaderTemplate>
                                        <div class="right-1" style="font-weight:bold;"> Total</div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                          
                                            $<asp:Label ID="lbltotal" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem,"Total") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="right" />
                                        <HeaderStyle HorizontalAlign="right" />
                                    </asp:TemplateField>
                                     
                                  
                               
                                </Columns>
                                 <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                <FooterStyle CssClass=".order-table td" />
                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                 <PagerSettings Position="TopAndBottom" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="amazonframe" runat="server"></div>
                    </td>
                </tr>
               
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>