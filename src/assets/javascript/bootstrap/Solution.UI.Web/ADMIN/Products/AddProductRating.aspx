<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="AddProductRating.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.AddProductRating" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
      <script type="text/javascript">
          var $j = jQuery.noConflict();
          $j(function () {
             <%-- <%=Funname%>--%>
            $j('#ContentPlaceHolder1_txtStartDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
         
        }
         )
    </script>
    <script type="text/javascript">
        var Emailresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                Emailresults = true
            else {
                Emailresults = false
            }
            return (Emailresults)
        }
        function ratingImage() {
            var indx = document.getElementById("<%=ddlrating.ClientID %>").selectedIndex;
            if (indx == 0) {
                document.getElementById("imgr1").src = '/images/star-form1.jpg';
                document.getElementById("imgr2").src = '/images/star-form1.jpg';
                document.getElementById("imgr3").src = '/images/star-form1.jpg';
                document.getElementById("imgr4").src = '/images/star-form1.jpg';
                document.getElementById("imgr5").src = '/images/star-form1.jpg';
            }
            else if (indx == 1) {
                document.getElementById("imgr1").src = '/images/star-form.jpg';
                document.getElementById("imgr2").src = '/images/star-form1.jpg';
                document.getElementById("imgr3").src = '/images/star-form1.jpg';
                document.getElementById("imgr4").src = '/images/star-form1.jpg';
                document.getElementById("imgr5").src = '/images/star-form1.jpg';

            }
            else if (indx == 2) {
                document.getElementById("imgr1").src = '/images/star-form.jpg';
                document.getElementById("imgr2").src = '/images/star-form.jpg';
                document.getElementById("imgr3").src = '/images/star-form1.jpg';
                document.getElementById("imgr4").src = '/images/star-form1.jpg';
                document.getElementById("imgr5").src = '/images/star-form1.jpg';

            }
            else if (indx == 3) {
                document.getElementById("imgr1").src = '/images/star-form.jpg';
                document.getElementById("imgr2").src = '/images/star-form.jpg';
                document.getElementById("imgr3").src = '/images/star-form.jpg';
                document.getElementById("imgr4").src = '/images/star-form1.jpg';
                document.getElementById("imgr5").src = '/images/star-form1.jpg';

            }
            else if (indx == 4) {

                document.getElementById("imgr1").src = '/images/star-form.jpg';
                document.getElementById("imgr2").src = '/images/star-form.jpg';
                document.getElementById("imgr3").src = '/images/star-form.jpg';
                document.getElementById("imgr4").src = '/images/star-form.jpg';
                document.getElementById("imgr5").src = '/images/star-form1.jpg';

            }
            else if (indx == 5) {
                document.getElementById("imgr1").src = '/images/star-form.jpg';
                document.getElementById("imgr2").src = '/images/star-form.jpg';
                document.getElementById("imgr3").src = '/images/star-form.jpg';
                document.getElementById("imgr4").src = '/images/star-form.jpg';
                document.getElementById("imgr5").src = '/images/star-form.jpg';

            }
        }

        function CheckSKUfield() {

            if (document.getElementById('<%=txtSKU.ClientID %>').value == '') {

                jAlert('Please Enter SKU.', 'Message', '<%=txtSKU.ClientID %>');
                 return false;
             }
            
             return true;
        }

        function Checkfields() {

            if (document.getElementById('<%=ddlproduct.ClientID %>').selectedIndex == 0) {

                jAlert('Please Select Product.', 'Message', '<%=ddlproduct.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtName.ClientID %>').value == '') {

                jAlert('Please Enter  Name.', 'Message', '<%=txtName.ClientID %>');
                return false;
            }
           <%-- else if (document.getElementById('<%=txtemail.ClientID %>').value == '') {

                jAlert('Please Enter Email.', 'Message', '<%=txtemail.ClientID %>');
                 return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_txtemail').value.replace(/^\s+|\s+$/g, "") != '' && !checkemail1(document.getElementById('ContentPlaceHolder1_txtemail').value.replace(/^\s+|\s+$/g, ""))) {
                jAlert('Please Enter Valid Email Address.', 'Message', 'ContentPlaceHolder1_txtemail');
                //document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }--%>
            else if (document.getElementById('<%=txtcomment.ClientID %>').value == '') {

                jAlert('Please Enter Comment.', 'Message', '<%=txtcomment.ClientID %>');
                return false;
            }
            else if (document.getElementById('ContentPlaceHolder1_ddlrating').selectedIndex == 0) {
                jAlert('Please Select Rating.', 'Message', 'ContentPlaceHolder1_ddlrating');

                return false;
            }
            else if (document.getElementById("ContentPlaceHolder1_txtStartDate") != null && document.getElementById("ContentPlaceHolder1_txtStartDate").value.replace(/^\s+|\s+$/g, '') == '') {

                jAlert('Please Enter Date.', 'Message');
                document.getElementById("ContentPlaceHolder1_txtStartDate").focus();
                return false;
            }

            
            return true;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;
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
                                                    <img class="img-left" title="Add Product Review" alt="Add Product Review" src="/App_Themes/<%=Page.Theme %>/Images/shipping-services-icon.png" />
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Product Review" ID="lblTitle"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <span class="star">*</span>Required Field
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Product Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlproduct" runat="server" CssClass="order-list" Width="185px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                      <td align="left">
                                                                            <span class="star"></span>SKU :
                                                                        </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtSKU" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" OnClientClick="return CheckSKUfield();" />
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>

                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtName" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;</span>Email:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtemail" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Comment:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtcomment" Width="222px" TextMode="MultiLine" Columns="5" Rows="5"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Rating:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlrating" runat="server" CssClass="select-box" Width="69px"
                                                                Style="margin-left: 2px; float: left;" AutoPostBack="false" onchange="ratingImage();">
                                                                <asp:ListItem Value="0" Text="Rating"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <img src="/images/star-form1.jpg" id="imgr1" style="vertical-align: middle" alt="Rating" />
                                                            <img src="/images/star-form1.jpg" id="imgr2" style="vertical-align: middle" alt="Rating" />
                                                            <img src="/images/star-form1.jpg" id="imgr3" style="vertical-align: middle" alt="Rating" />
                                                            <img src="/images/star-form1.jpg" id="imgr4" style="vertical-align: middle" alt="Rating" />
                                                            <img src="/images/star-form1.jpg" id="imgr5" style="vertical-align: middle" alt="Rating" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Is Approved:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkapprove" runat="server" />
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Is Verified:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkVerified" runat="server" />
                                                        </td>
                                                    </tr>
                                                     <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Created On:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtStartDate" Style="margin-right: 3px;"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <td></td>
                                                    <td style="width: 80%">
                                                        <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClick="imgSave_Click" OnClientClick="return Checkfields();" />
                                                        &nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                            OnClick="imgCancel_Click" />
                                                    </td>
                                                </table>
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
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
