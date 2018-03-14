<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewpricequotedetail.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Reports.viewpricequotedetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <script type="text/javascript">
     function checkfields() {

         document.getElementById("prepage").style.display = '';
         return true;
     }
 
 
 </script>
    <title></title>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table_border">
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="color: #fff; height: 28px; background: #7d7d7d;
                    border-top: 1px solid #E4E4E4; border-bottom: 1px solid #E4E4E4; padding-left: 10px;
                    font-size: 12px; font-weight: bold;">
                    <strong>Price quote Details</strong> <a href="javascript:window.close();" title="Close"
                        class="close"><span style="float: right;">
                            <img src="/App_Themes/<%=Page.Theme %>/images/close.gif" alt="Close" style="border: 0px"
                                title="Close" />
                        </span></a>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top">
                    <table width="100%" border="0" cellpadding="4" cellspacing="0" style="padding-top: 10px;
                        line-height: 20px; padding-left: 10px;">
                        <tr>
                            <td align="left" style="width: 10%;">
                                <b>Assign To </b>
                            </td>
                            <td style="width: 10%;">
                                :
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlsalesperson" runat="server" Width="300px" CssClass="order-list"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">
                                <b>Name </b>
                            </td>
                            <td style="width: 10%;">
                                :
                            </td>
                            <td align="left">
                                <asp:Literal ID="ltname" runat="server"></asp:Literal>
                                <asp:Label ID="lblfirstname" runat="server" Style="display: none;"></asp:Label>
                                <asp:Label ID="lbllastname" runat="server" Style="display: none;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <b>Email </b>
                            </td>
                            <td style="width: 10%;">
                                :
                            </td>
                            <td align="left">
                                <asp:Literal ID="ltemail" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr id="trAddress" runat="server">
                            <td align="left">
                                <b>Address </b>
                            </td>
                            <td style="width: 10%;">
                                :
                            </td>
                            <td align="left">
                                <asp:Literal ID="ltAddress" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr id="trCity" runat="server">
                            <td align="left">
                                <b>City </b>
                            </td>
                            <td style="width: 10%;">
                                :
                            </td>
                            <td align="left">
                                <asp:Literal ID="ltCity" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr id="trState" runat="server">
                            <td align="left">
                                <b>State </b>
                            </td>
                            <td style="width: 10%;">
                                :
                            </td>
                            <td align="left">
                                <asp:Literal ID="ltState" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <b>Zip Code </b>
                            </td>
                            <td style="width: 10%;">
                                :
                            </td>
                            <td align="left">
                                <asp:Literal ID="ltZipCode" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr id="trPhone" runat="server">
                            <td align="left">
                                <b>Phone </b>
                            </td>
                            <td style="width: 10%;">
                                :
                            </td>
                            <td align="left">
                                <asp:Literal ID="ltPhone" runat="server"></asp:Literal>
                            </td>
                        </tr>
                       
                        <tr>
                            <td colspan="3">
                                            <asp:Literal ID="ltpricequotedetail" runat="server"></asp:Literal>
                            </td>
                        </tr>
                          <tr>
                            <td align="left" valign="top">
                                <b>Number of Windows </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                              
                                    <asp:Literal ID="ltrNumberofwindow" runat="server"></asp:Literal>
                               
                            </td>
                        </tr>
                         <tr>
                            <td align="left" valign="top">
                                <b>Purpose of Drapery </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                              
                                    <asp:Literal ID="ltrPurposeofDrapery" runat="server"></asp:Literal>
                               
                            </td>
                        </tr>
                         <tr>
                            <td align="left" valign="top" style="width:26%;">
                                <b>If Functioning </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                              
                                    <asp:Literal ID="ltrFunctoning" runat="server"></asp:Literal>
                               
                            </td>
                        </tr>
                         <tr>
                            <td align="left" valign="top">
                                <b>Window Width </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                               
                                    <asp:Literal ID="ltrwindowwidth" runat="server"></asp:Literal>
                               
                            </td>
                        </tr>
                         <tr>
                            <td align="left" valign="top">
                                <b>Top of Window to Floor </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                               
                                    <asp:Literal ID="ltrTopwidowfloor" runat="server"></asp:Literal>
                                
                            </td>
                        </tr>
                         <tr>
                            <td align="left" valign="top">
                                <b>Ceiling Height </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                               
                                    <asp:Literal ID="ltrceilingheight" runat="server"></asp:Literal>
                              
                            </td>
                        </tr>
                          <tr>
                            <td align="left" valign="top">
                                <b>Drapery Style </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                               
                                    <asp:Literal ID="ltrdraperystyle" runat="server"></asp:Literal>
                               
                            </td>
                        </tr>
                         <tr>
                            <td align="left" valign="top">
                                <b>Lining Option </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                                
                                    <asp:Literal ID="ltrliningoption" runat="server"></asp:Literal>
                                
                            </td>
                        </tr>
                         <tr>
                            <td align="left" valign="top">
                                <b>Is Your Rod Already in Place? </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                                
                                    <asp:Literal ID="ltrisyourrod" runat="server"></asp:Literal>
                               
                            </td>
                        </tr>
                          <tr>
                            <td align="left" valign="top">
                                <b>Have you ordered with us before? </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                               
                                    <asp:Literal ID="ltrhaveyouordered" runat="server"></asp:Literal>
                               
                            </td>
                        </tr>
                         <tr>
                            <td align="left" valign="top">
                                <b>Message </b>
                            </td>
                            <td style="width: 10%;" valign="top">
                                :
                            </td>
                            <td align="left">
                                <div style="width: 100%; height: 70px; overflow-y: auto;">
                                    <asp:Literal ID="ltMessage" runat="server"></asp:Literal>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <td align="right">
                                                <asp:Button ID="btnsend" runat="server" OnClick="btnsend_Click" OnClientClick="return checkfields();"/>
                                            </td>
                                            <td align="left">
                                                <a href="javascript:window.close();" title="Cancel" class="Cancel">
                                                    <img src="/App_Themes/<%=Page.Theme %>/images/cancel.gif" alt="Cancel" style="width: 67px;
                                                        height: 23px; border: none; cursor: pointer;" title="Close" />
                                                </a>
                                            </td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                    </table>
                </td>
            </tr>
        </table>
        <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <div>
        <asp:HiddenField ID="hdnproductid" runat="server" />
    </div>
      
    </form>
</body>
</html>
