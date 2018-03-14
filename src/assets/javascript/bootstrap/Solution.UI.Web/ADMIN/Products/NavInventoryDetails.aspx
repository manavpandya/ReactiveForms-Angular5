<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="NavInventoryDetails.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.NavInventoryDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">


        function chkallinvnetoryupdate() {
            jConfirm('Are you sure want to update inventory  ?', 'Confirmation', function (r) {
                if (r == true) {
                   
                    document.getElementById('ContentPlaceHolder1_btnNavInventorySync').onclick = function () { }
                    document.getElementById('ContentPlaceHolder1_btnNavInventorySync').click();
                    return true;
                }
                else {

                    return false;
                }

            });
            return false;
        }


    </script>
     <div class="content-row1">
        <div class="create-new-order">
            &nbsp;
        </div>
    </div>
   
    <div class="content-row2">
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
        <table width="100%" border="0" class="add-product" cellpadding="0" cellspacing="0">
            <tr class="even-row">
                                            <th width="100%" colspan="6" >
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Nav Inventory Details" alt="Nav Inventory Details" src="/App_Themes/<%=Page.Theme %>/Images/list-product-icon.png" />
                                                    <h2>Nav Inventory Details</h2>
                                                </div>
                                            </th>
                                        </tr>
            <tr >
                <td align="left" style="font-weight:bold;width:8%">
                   Last Updated By:
                </td>
                 <td align="left" style="font-weight:bold;">
                    <asp:Label ID="lblUpdatedBy" runat="server" Text=""></asp:Label>

                </td>
                
        </tr>
          
             <tr>
                 <td align="left" style="font-weight:bold;width:8%">
                   Last Updated On:
                </td>
                  <td align="left" style="font-weight:bold;">
                    <asp:Label ID="lblUpdatedOn" runat="server" Text=""></asp:Label>

                </td>
        </tr>
           
            <tr>
                 <td align="left" style="font-weight:bold;width: 12%">
                    <asp:Button ID="btnNavInventorySync" runat="server" OnClientClick="return chkallinvnetoryupdate();"  OnClick="btnNavInventorySync_Click" />
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
