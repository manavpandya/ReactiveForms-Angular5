<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ImportFabricData.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ImportFabricData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function call() {

           
            document.getElementById('ContentPlaceHolder1_btnexporta').click();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="content-row1">
            <div class="create-new-order" style="width: 100%">
               <span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                    id="ahAddProduct" href="~/ADMIN/Products/ProductFabricSettings.aspx" runat="server">
                    <img alt="Back" title="Back" src="/App_Themes/<%=Page.Theme %>/images/back.png" /></a></span>
            </div>
        </div>

      <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="10" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>

            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                   
                                    <tr style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Import Fabric Inventory" alt="Import Fabric Inventory" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                <h2>Import Fabric Data</h2>
                                            </div>
                                           
                                        </th>
                                    </tr>
                                    <tr>
                                        <td id="tdMainDiv">
                                            <div id="divMain" class="slidingDivMainDiv">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                    <tr class="altrow">
                                                        <td align="center">
                                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="even-row">
                                                        <td align="left">
                                                            <fieldset>
                                                               
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr class="oddrow">
                                                                       
                                                                        <td>
                                                                            <asp:Label ID="lblImportCSv" runat="server" Text="Import CSV :"></asp:Label>
                                                                            <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" /></td>
                                                                          <td style="margin-left:950px;float:right;">
                                                                            <a href="~/ADMIN/Files/FabricVendorProductsData.csv" style="display:none;" id="btnexporta"  runat="server">
                                                                                
                                                                                </a>
                                                                             <asp:Button ID="btnexport1" OnClientClick="call();return false;" runat="server"     />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                   
                                                </table>
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
    </div>
</asp:Content>
