<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ChangeEmail.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.ChangeEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        var testresults
        function checkemail1(str) {
            str = str.replace(/^\s+|\s+$/g, "");
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                testresults = true
            else {
                testresults = false
            }

            return (testresults)
        }
    </script>
    <script type="text/javascript">


        function validation() {
            if (document.getElementById('ContentPlaceHolder1_txtoldemail') != null && (document.getElementById('ContentPlaceHolder1_txtoldemail').value).replace(/^\s*\s*$/g, '') == '') {

                jAlert('Please Enter Old Email', 'Message', 'ContentPlaceHolder1_txtoldemail');
                return false;


            }
            else if (document.getElementById("ContentPlaceHolder1_txtoldemail") != null && document.getElementById("ContentPlaceHolder1_txtoldemail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtoldemail").value)) {

                jAlert('Please Enter Valid Old Email', 'Message', 'ContentPlaceHolder1_txtoldemail');
                return false;
            }

            else if (document.getElementById('ContentPlaceHolder1_txtnewemail') != null && (document.getElementById('ContentPlaceHolder1_txtnewemail').value).replace(/^\s*\s*$/g, '') == '') {

                jAlert('Please Enter New Email', 'Message', 'ContentPlaceHolder1_txtnewemail');
                return false;


            }
            else if (document.getElementById("ContentPlaceHolder1_txtnewemail") != null && document.getElementById("ContentPlaceHolder1_txtnewemail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtnewemail").value)) {

                jAlert('Please Enter Valid New Email', 'Message', 'ContentPlaceHolder1_txtnewemail');
                return false;
            }
            return true;
            //if (confirm('Are you sure want to Update Email for all Orders?')) { return true; } else { return false; }
            //return false;


        }
        function validation2() {
            if (document.getElementById('ContentPlaceHolder1_txtoldemail') != null && (document.getElementById('ContentPlaceHolder1_txtoldemail').value).replace(/^\s*\s*$/g, '') == '') {

                jAlert('Please Enter Old Email', 'Message', 'ContentPlaceHolder1_txtoldemail');
                return false;


            }
            else if (document.getElementById("ContentPlaceHolder1_txtoldemail") != null && document.getElementById("ContentPlaceHolder1_txtoldemail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtoldemail").value)) {

                jAlert('Please Enter Valid Old Email', 'Message', 'ContentPlaceHolder1_txtoldemail');
                return false;
            }

            else if (document.getElementById('ContentPlaceHolder1_txtnewemail') != null && (document.getElementById('ContentPlaceHolder1_txtnewemail').value).replace(/^\s*\s*$/g, '') == '') {

                jAlert('Please Enter New Email', 'Message', 'ContentPlaceHolder1_txtnewemail');
                return false;


            }
            else if (document.getElementById("ContentPlaceHolder1_txtnewemail") != null && document.getElementById("ContentPlaceHolder1_txtnewemail").value != '' && !checkemail1(document.getElementById("ContentPlaceHolder1_txtnewemail").value)) {

                jAlert('Please Enter Valid New Email', 'Message', 'ContentPlaceHolder1_txtnewemail');
                return false;
            }

            if (confirm('Are you sure you to merge email addresses , you can\'t undo this operation?')) { return true; } else { return false; }
            return false;


        }
        function CheckPass() {
            if ((document.getElementById('ContentPlaceHolder1_txtpassword').value).replace(/^\s*\s*$/g, '') == '') {
                jAlert('Please Enter Password', 'Message', 'ContentPlaceHolder1_txtpassword');
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>

            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <th>
                                <div class="main-title-left" style="width: 100% !important;">
                                    <img class="img-left" title="Merge Email / Email Updates" alt="Merge Email / Email Updates" src="/App_Themes/<%=Page.Theme %>/Images/emailTemplate.png" />
                                    <h2>Merge Email / Email Updates</h2>
                                </div>
                            </th>
                        </tr>
                        <tr class="altrow">
                            <div id="password" runat="server" visible="true">


                                <table border="0" style="padding-top: 20px" cellpadding="5" cellspacing="0">
                                    <tr>
                                        <td valign="top" style="font-size: 11px;">
                                            <span class="required-red"></span>Password
                                        </td>
                                        <td>:
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtpassword" runat="server" Style="font-size: 11px;" CssClass="order-textfield"
                                                Width="143" TextMode="Password"></asp:TextBox>
                                        </td>


                                    </tr>

                                    <tr class="oddrow">
                                        <td colspan="2"></td>
                                        <td align="left">
                                            <asp:ImageButton ID="btnsubmit" OnClientClick="return CheckPass();"
                                                OnClick="btnsubmit_Click" runat="server" ImageUrl="/App_Themes/Gray/images/submit.gif" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </tr>

                        <tr id="divsearch" runat="server" visible="false">
                            <td>


                                <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" class="content-table">


                                    <tr class="even-row">

                                        <td id="tdMainDiv">
                                            <div id="divMain" class="slidingDivMainDiv">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">

                                                    <tr class="even-row">
                                                        <td align="left">


                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr class="oddrow">

                                                                    <td>Old Email:
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtoldemail" CssClass="order-textfield" runat="server"></asp:TextBox> &nbsp;&nbsp;&nbsp;<asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr class="evenrow">

                                                                    <td>New Email:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtnewemail" CssClass="order-textfield" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr class="oddrow">
                                                                    <td></td>
                                                                    <td>
                                                                        <asp:Button ID="btnshoworder" runat="server" OnClick="btnshoworder_Click" OnClientClick="return validation();" />

                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </td>


                                    </tr>

                                    <tr class="altrow">
                                        <td>
                                            <asp:Literal ID="ltrold" runat="server"></asp:Literal>
                                            <asp:GridView ID="grdold" runat="server" AutoGenerateColumns="False"
                                                EmptyDataText="Order not Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnPageIndexChanging="grdold_PageIndexChanging"
                                                BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1"
                                                GridLines="None" Width="100%" AllowPaging="True" PageSize="25"
                                                PagerSettings-Mode="NumericFirstLast">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <HeaderTemplate>
                                                            Order Number
                                                               
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLabel" runat="server" Text='<%# Bind("Ordernumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Order Date
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%# String.Format("{0:MM/dd/yyyy}", Eval("OrderDate"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Billing Email
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblbillingemail" runat="server" Text='<%# Bind("BillingEmail") %>' Style="text-align: left;"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Shipping Email
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblshippingemail" runat="server" Text='<%# Bind("ShippingEmail") %>' Style="text-align: left;"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Order Total
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%# String.Format("${0:0.00}", Eval("OrderTotal"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField Visible="false">
                                                        <HeaderTemplate>
                                                            Customer Type
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltype" runat="server" Text='<%# Bind("type") %>' Style="text-align: left;"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                <AlternatingRowStyle CssClass="altrow" />
                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                            </asp:GridView>
                                        </td>
                                        
                                    </tr>
                                   
                                    <tr class="altrow">
                                        <td> <br /><br />
                                             <asp:Literal ID="ltrnew" runat="server"></asp:Literal>
                                            <asp:GridView ID="grdnew" runat="server" AutoGenerateColumns="False"
                                                EmptyDataText="Order not Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" OnPageIndexChanging="grdnew_PageIndexChanging"
                                                BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1"
                                                GridLines="None" Width="100%" AllowPaging="True" PageSize="25"
                                                PagerSettings-Mode="NumericFirstLast">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <HeaderTemplate>
                                                            Order Number
                                                               
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLabel" runat="server" Text='<%# Bind("Ordernumber") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Order Date
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%# String.Format("{0:MM/dd/yyyy}", Eval("OrderDate"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Billing Email
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblbillingemail" runat="server" Text='<%# Bind("BillingEmail") %>' Style="text-align: left;"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Shipping Email
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblshippingemail" runat="server" Text='<%# Bind("ShippingEmail") %>' Style="text-align: left;"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Order Total
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%# String.Format("${0:0.00}", Eval("OrderTotal"))%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <HeaderTemplate>
                                                            Customer Type
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltype" runat="server" Text='<%# Bind("type") %>' Style="text-align: left;"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                <AlternatingRowStyle CssClass="altrow" />
                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
					<tr>
                                        <td align="center">
                                            <br/>
                                        </td>
                                    </tr>

                                    <tr id="trupdate" runat="server" visible="false">
                                        <td align="left" style="padding-left: 70px !important;">
                                            <asp:Button ID="btnupdate" runat="server" OnClick="btnupdate_Click" OnClientClick="return validation2();" />
                                        </td>
                                    </tr>
					<tr>
                                        <td align="center">
                                            <br/>
						<br/>
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
