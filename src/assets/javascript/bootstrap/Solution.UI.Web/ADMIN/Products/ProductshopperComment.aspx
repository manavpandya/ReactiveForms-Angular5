<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ProductshopperComment.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductshopperComment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <link rel="Stylesheet" type="text/css" href="/css/jquery.dataTables.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Repeater ID="RptList" runat="server" OnItemDataBound="RptList_ItemDataBound" OnItemCommand="RptList_ItemCommand">
                            <HeaderTemplate>
                                <table id="RptList" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th>Product Name</th>
                                            <th>Comments</th>
                                            <th>Approve</th>
                                            <th>Disapprove</th>
                                        </tr>
                                    </thead>
                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tbody>
                                    <tr>
                                            <td><%# Eval("Name") %> </td>
                                            <td><%# Eval("Comments") %> </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="btnApprove" ImageUrl="/App_Themes/<%= Page.Theme %>/images/approve.png"
                                                                ToolTip="Approve" CommandName="Approve" CommandArgument='<%# Eval("ProductshopperCommentID") %>'
                                                                message="Are you sure want to Approve current Comment?" OnClientClick='return confirm(this.getAttribute("message"));'></asp:ImageButton>
                                            </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="btnUnApprove" ImageUrl="/App_Themes/<%= Page.Theme %>/images/disapprove.png"
                                                                ToolTip="Disapprove" CommandName="Disapprove" CommandArgument='<%# Eval("ProductshopperCommentID") %>'
                                                                message="Are you sure want to Disapprove current Comment?" OnClientClick='return confirm(this.getAttribute("message"));'></asp:ImageButton>
                                        </td>
                                        </tr>

                                </tbody>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>

                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript" src="/admin/js/jquery.js"></script>
    <script type="text/javascript" src="/admin/js/jquery.dataTables.js"></script>
    <script type="text/javascript">

        jQuery(function () {
            CallDataTable();
        });


        function CallDataTable() {
            $('#RptList').dataTable({
                "oLanguage": {
                    "sEmptyTable": "No Comments available"
                }
            });
        }
    </script>

    
</asp:Content>
