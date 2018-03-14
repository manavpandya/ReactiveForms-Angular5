<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="Importorderlnt.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.Importorderlnt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
            </div>
        </div>
        <div class="content-row2">

  <table width="100%" border="0" cellspacing="0" cellpadding="0">
             <tr>
                 <td>
                     <br />
                 </td>
             </tr>
     
                <tr>
                    <td class="border-td">
            <table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#FFFFFF" class="content-table">
                            <tbody><tr>
                                <td class="border-td-sub">
                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="add-product">
                                        <tbody><tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img src="/App_Themes/Gray/Images/customer-list-icon.png" alt="Item List Channelwise Report" title="Item List Channelwise Report" class="img-left" />
                                                    <h2>Import WayFair Orders</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td style="padding-right: 0px;">

                                                <table cellspacing="3" cellpadding="3" border="0" bgcolor="#FFFFFF" class="content-table" width="100%">
                                                    <tbody><tr>
                                                        <td align="center" style="padding-right: 0px; width: 80%">
                                                            <table width="50%" cellspacing="0" cellpadding="0" border="0" style=" border: solid 1px #e7e7e7;">
                                                                <tbody>
                                                                    <tr>
                                                                      
                                                                    <td align="center">
                                                                         <asp:Label ID="lblselect" runat="server" ForeColor="Black" Text=" Select CSV. File : "></asp:Label>
                                                                        &nbsp;
                                                                         <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />

            <asp:LinkButton ID="btnUpload" AlternateText="Upload" ImageAlign="Top"
                runat="server" Visible="false">Upload</asp:LinkButton>

            <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" ValidationGroup="importfile" CausesValidation="true" />


            <asp:RegularExpressionValidator ID="revImage" ControlToValidate="uploadCSV" ValidationExpression="(.*?)\.(csv)$"
                Text="Select csv File Only (Ex.: .csv)" runat="server"
                ValidationGroup="importfile" Display="Dynamic" ForeColor="Red" />
            <asp:RequiredFieldValidator ID="reqfile" runat="server" Text="Please select.csv file" ValidationGroup="importfile" ControlToValidate="uploadCSV" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
          
                                                                    </td>
                                                                    
                                                                </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                       <tr>
                                                                        <td align="center">
                                                                        <asp:Label ID="lblTotalRecords" Visible="false" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                     <tr>
                                                                        <td align="center">
                                                                        <asp:Label ID="lblAlreadyExist" Visible="false" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                     <tr>
                                                                        <td align="center">
                                                                        <asp:Label ID="lblImport" Visible="false" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                            </tbody></table>
                                                        </td>
                                                    </tr>
                                                </tbody></table>
                                            </td>
                                        </tr>
                                        
                                    </tbody></table>
                                </td>
                            </tr>
                        </tbody></table>
                        </td>
                    </tr>
      </table>
           
        </div>
    </div>
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
             <tr>
                 <td>
                     <br />
                 </td>
             </tr>
         </table>
</asp:Content>
