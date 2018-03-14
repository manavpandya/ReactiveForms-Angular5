<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="VendorQuoteReply.aspx.cs" Inherits="Solution.UI.Web.VendorQuoteReply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height();
            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
        function loader() {
            CheckQty();
            chkHeight();
        }
        function keyRestrict(e, validchars) {
            var key = '', keychar = '';
            key = getKeyCode(e);
            if (key == null) return true;
            keychar = String.fromCharCode(key);
            keychar = keychar.toLowerCase();
            validchars = validchars.toLowerCase();
            if (validchars.indexOf(keychar) != -1)
                return true;
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
                return true;
            return false;
        }
        function getKeyCode(e) {
            if (window.event)
                return window.event.keyCode;
            else if (e)
                return e.which;
            else
                return null;
        }

        function CheckQty() {
            var allElts = document.getElementById("Gridlist").getElementsByTagName("input");

            var i, cnt = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];

                if (elt.type == "checkbox" && elt.checked == true) {
                    var checkname = elt.id;
                    var pricename = checkname.replace('chkid', 'txtPrice');
                    checkname = checkname.replace('chkid', 'txtQuantity');
                    if (document.getElementById(checkname).value == "") {
                        alert('Please enter Quantity.');
                        document.getElementById(checkname).focus();
                        return false;
                    }
                    else {
                        if (parseInt(document.getElementById(checkname).value) > 0) {

                        }
                        else {
                            alert('Quantity should be greater then 0');
                            document.getElementById(checkname).focus();
                            return false;

                        }
                    }
                    if (document.getElementById(pricename).value == "") {
                        alert('Please enter price.');
                        document.getElementById(pricename).focus();
                        return false;
                    }
                    else {
                        if (parseInt(document.getElementById(pricename).value) > 0) {

                        }
                        else {
                            alert('Price should be greater then 0');
                            document.getElementById(pricename).focus();
                            return false;
                        }
                    }
                    cnt = cnt + 1;
                }
            }
            if (cnt == 0) {
                alert('Please select atleast one product.');
                return false;
            }
            return true;
        }
    </script>
    <div class="breadcrumbs">
        <a href="/index.aspx" title="Home">Home </a>> <span>Submit Your Quote</span></div>
    <div class="content-main">
        <div class="static-title">
            <span>Submit Your Quote</span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <div style="padding-bottom: 5px; text-align: center;">
                    <center>
                        <asp:Label ID="lblMsg" runat="server" Style="color: Red;" CssClass="checkout-red"></asp:Label></center>
                </div>
                <asp:Literal ID="ltrSaveMsg" runat="server"></asp:Literal>
                <table cellspacing="0" cellpadding="0" border="0" style="border: 1px solid #DFDFDF;
                    padding: 5px;" width="791px" id="maintbl" runat="server">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr id="trVendordetails" runat="server">
                                    <td align="left" colspan="3" style="padding-left: 40px; padding-right: 40px" width="100%">
                                        <table cellpadding="0" cellspacing="0" border="0" style="border: solid 1px #dfdfdf;"
                                            width="100%">
                                            <tr>
                                                <td style="width: 100%; background-color: #f3f3f3; line-height: 30px; text-indent: 10px;
                                                    font-weight: bold; border-bottom: solid 1px #dfdfdf;" colspan="2">
                                                    Vendor Details :
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="padding-left: 10px">
                                                    <table>
                                                        <tr>
                                                            <td align="right" style="line-height: 30px; font-weight: bold;">
                                                                Vendor Name:
                                                            </td>
                                                            <td align="left" style="line-height: 30px">
                                                                <asp:Literal ID="ltvname" runat="server"></asp:Literal>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="line-height: 30px; font-weight: bold;">
                                                                Phone:
                                                            </td>
                                                            <td align="left" style="line-height: 30px">
                                                                <asp:Literal ID="ltvphone" runat="server"></asp:Literal>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="line-height: 30px; font-weight: bold;">
                                                                Address:
                                                            </td>
                                                            <td align="left" style="line-height: 30px">
                                                                <asp:Literal ID="ltvaddress" runat="server"></asp:Literal>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" style="line-height: 30px; font-weight: bold;">
                                                                E-mail:
                                                            </td>
                                                            <td align="left" style="line-height: 30px">
                                                                <asp:Literal ID="ltvemail" runat="server"></asp:Literal>
                                                            </td>
                                                        </tr>
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
                        <td align="center" valign="top" style="padding-top: 20px; padding-bottom: 10px;">
                            <div id="Gridlist">
                                <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                    ViewStateMode="Enabled" BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1px"
                                    CellPadding="2" CellSpacing="1" GridLines="None" Width="90%" OnRowDataBound="gvVendor_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductID") %>'></asp:Label>
                                                <asp:Label ID="lblVendorId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"VendorID") %>'></asp:Label>
                                                <asp:Label ID="lblChkVendorReqId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ChkVendorReqId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select">
                                            <HeaderStyle BackColor="#E7E7E7" />
                                            <ItemStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkid" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <HeaderStyle BackColor="#E7E7E7" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <HeaderTemplate>
                                                Product Name
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                <br />
                                                <%# !string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem," ProductOption").ToString().Trim())? "Product Options :" :"" %>
                                                <asp:Label ID="lblProOption" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductOption") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#E7E7E7" />
                                            <HeaderTemplate>
                                                SKU
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="center" />
                                            <HeaderStyle BackColor="#E7E7E7" />
                                            <HeaderTemplate>
                                                Quantity
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuantity" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:Label>
                                                <asp:TextBox CssClass="checkout-textfild" Style="width: 50px; text-align: center;"
                                                    ID="txtQuantity" onkeypress="return keyRestrict(event,'0123456789');" MaxLength="4"
                                                    runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle BackColor="#E7E7E7" HorizontalAlign="center" Width="10%" />
                                            <HeaderTemplate>
                                                Your Price<br />
                                                (per unit)
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="right" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPrice" onkeypress="return keyRestrict(event,'0123456789.');"
                                                    Text='<%# String.Format("{0:0.00}",Convert.ToDouble(Convert.ToString(DataBinder.Eval(Container.DataItem,"Price")))) %>'
                                                    CssClass="checkout-textfild" MaxLength="10" runat="server" Style="width: 60px;
                                                    text-align: right;"></asp:TextBox>
                                                <asp:Label ID="lblPrice" runat="server" Visible="false" Text='<%# String.Format("{0:0.00}",Convert.ToDouble(Convert.ToString(DataBinder.Eval(Container.DataItem,"Price")))) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="center" />
                                            <HeaderStyle BackColor="#E7E7E7" />
                                            <HeaderTemplate>
                                                Available<br />
                                                [in Days]
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="center" />
                                            <ItemTemplate>
                                                <input type="hidden" id="availdays" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"AvailableDays") %>' />
                                                <asp:DropDownList ID="drpAvailDays" runat="server" CssClass="select-box" Width="45px">
                                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="3 " Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                                    <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                                    <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                                    <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <AlternatingRowStyle CssClass="altrow" />
                                    <PagerStyle CssClass="paging" />
                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                    <PagerSettings Position="TopAndBottom" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" style="padding-top: 10px;">
                                <tr id="tr2" runat="server">
                                    <td align="left" colspan="3" style="padding-left: 40px; padding-right: 40px" width="100%">
                                        <table cellpadding="0" cellspacing="3" border="0" width="100%">
                                            <tr>
                                                <td style="width: 130px;">
                                                    Location :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLocation" runat="server" class="checkout-textfild" TextMode="MultiLine"
                                                        Width="400px" Height="60px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trNotes" runat="server">
                                                <td>
                                                    Notes :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotes" runat="server" class="checkout-textfild" TextMode="MultiLine"
                                                        Width="400px" Height="60px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trRequestNotes" runat="server" visible="false">
                                                <td>
                                                    Notes From Merchant :
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Label ID="lblRequestNotes" Width="339px" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td align="left" style="padding-left: 150px;">
                                                    <asp:ImageButton ID="btnsubmit" runat="server" alt="SUBMIT" title="SUBMIT" OnClientClick="loader();"
                                                        ImageUrl="/images/finish.jpg" OnClick="btnsubmit_Click" /><br />
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
            <table width="100%">
                <tr>
                    <td align="center" style="color: #fff;">
                        <img alt="" src="/images/loding.png" /><br />
                        <b>Loading ... ... Please wait!</b>
                    </td>
                </tr>
            </table>
        </div>
        </div>
</asp:Content>
