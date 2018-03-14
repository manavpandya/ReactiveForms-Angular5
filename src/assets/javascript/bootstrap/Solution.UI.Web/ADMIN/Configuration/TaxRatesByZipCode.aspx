<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="TaxRatesByZipCode.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Configuration.TaxRatesByZipCode" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script language="javascript" type="text/javascript">
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
        <div class="create-new-order" style="width: 100%;">
            <span style="vertical-align: middle; margin-right: 3px; margin-top: 4px; float: left;">
                <table>
                    <tr>
                        <td>
                            Store :
                        </td>
                        <td>
                          <asp:DropDownList ID="ddlStore" runat="server" CssClass="order-list" Width="185px" AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </span>
        </div>
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
                                                    <img class="img-left" title="Zip Code Tax Rate" alt="Zip Code Tax Rate" src="/App_Themes/<%=Page.Theme %>/Images/zip-code-tax-rate-icon.png" />
                                                    <h2>
                                                        Zip Code Tax Rate
                                                    </h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table style="width: 100%" cellpadding="3" cellspacing="3">
                                                    <tr>
                                                        <td style="width: 25%" align="left">
                                                           
                                                 
                                                        </td>
                                                        <td align="right" id="tdExportImport">
                                                          
                                                            <asp:Label ID="lblImportCSv" runat="server" Text="Import CSV :"></asp:Label>

                                                            <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />

                                                            <asp:LinkButton ID="btnUpload" AlternateText="Upload" ImageAlign="Top"
                                                                runat="server" Visible="false">Upload</asp:LinkButton>

                                                            <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" ValidationGroup="importfile" CausesValidation="true" />


                                                            <asp:RegularExpressionValidator ID="revImage" ControlToValidate="uploadCSV" ValidationExpression="(.*?)\.(csv)$"
                                                                Text="Select csv File Only (Ex.: .csv)" runat="server"
                                                                ValidationGroup="importfile" Display="Dynamic" ForeColor="Red" />
                                                            <asp:RequiredFieldValidator ID="reqfile" runat="server" Text="Please select.csv file" ValidationGroup="importfile" ControlToValidate="uploadCSV" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                                            </td>
                                                         
                                                        <td style="width: 19%" align="right">
                                                            Search :
                                                        </td>
                                                        <td style="width: 2%">
                                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="order-textfield" Width="124px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 1%">
                                                            <asp:ImageButton ID="ibtnsearch" runat="server" OnClientClick="return validation();"
                                                                OnClick="btnGo_Click" />
                                                        </td>
                                                        <td style="width: 1%">
                                                            <asp:ImageButton ID="ibtnShowall" runat="server" OnClick="btnSearchall_Click" CommandName="ShowAll" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Label ID="lblMsg" runat="server" CssClass="font-red"></asp:Label>
                                                </center>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:GridView ID="gvZipCodeTaxRate" runat="server" DataKeyNames="ZipTaxId" AutoGenerateColumns="false"
                                                    GridLines="None" Width="100%"  BorderStyle="Solid" BorderWidth="1" BorderColor="#E7E7E7" CellSpacing="1" EmptyDataText="No Records Found!" RowStyle-ForeColor="Black"
                                                    HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" AllowPaging="True"
                                                    PageSize="<%$ appSettings:GridPageSize %>" OnRowDeleting="gvZipCodeTaxRate_RowDeleting"
                                                    OnRowCancelingEdit="gvZipCodeTaxRate_RowCancelingEdit" OnRowEditing="gvZipCodeTaxRate_RowEditing"
                                                    OnRowUpdating="gvZipCodeTaxRate_RowUpdating" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ShowFooter="true" ViewStateMode="Enabled"
                                                    OnPageIndexChanging="gvZipCodeTaxRate_PageIndexChanging" OnRowDataBound="gvZipCodeTaxRate_RowDataBound" 
                                                    OnRowCommand="gvZipCodeTaxRate_RowCommand" OnSorting="gvZipCodeTaxRate_Sorting">
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="10" />
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
</asp:Content>
