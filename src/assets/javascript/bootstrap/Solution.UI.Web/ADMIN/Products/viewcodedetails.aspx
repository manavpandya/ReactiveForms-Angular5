<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewcodedetails.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.viewcodedetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">

        function keyRestrict(e, validchars) {

            var key = '', keychar = '';

            key = getKeyCode(e);

            if (key == null) return true;

            keychar = String.fromCharCode(key);

            keychar = keychar.toLowerCase();

            validchars = validchars.toLowerCase();

            if (validchars.indexOf(keychar) != -1)

                return true;

            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)

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
        function clicksavebuttonnew() {
            if (document.getElementById('chkactive') != null && document.getElementById('chkactive').checked == true) {
                
                    document.getElementById('txtMinwidth').removeAttribute('disabled');
                    document.getElementById('txtMaxwidth').removeAttribute('disabled');
                    document.getElementById('txtMinlength').removeAttribute('disabled');
                    document.getElementById('txtMaxlength').removeAttribute('disabled');

                    document.getElementById('tdnsave').style.display = '';
               

            }
            
        }
        function clicksavebutton() {
            if (document.getElementById('chkactive') != null && document.getElementById('chkactive').checked == true) {
                if (confirm('Are you sure want to change?')) {
                    document.getElementById('txtMinwidth').removeAttribute('disabled');
                    document.getElementById('txtMaxwidth').removeAttribute('disabled');
                    document.getElementById('txtMinlength').removeAttribute('disabled');
                    document.getElementById('txtMaxlength').removeAttribute('disabled');

                    document.getElementById('tdnsave').style.display = '';
                }
                else {
                    document.getElementById('chkactive').checked = false;
                }

            }
            else {
                document.getElementById('txtMinwidth').disabled = 'disabled';
                document.getElementById('txtMaxwidth').disabled = 'disabled';
                document.getElementById('txtMinlength').disabled = 'disabled';
                document.getElementById('txtMaxlength').disabled = 'disabled';
                document.getElementById('tdnsave').style.display = 'none';
            }
        }
        function clicksaveshowproduct() {
            document.getElementById('tdsaveproduct').style.display = '';
        }
    </script>
    <style type="text/css">

        .tdpadding{
            padding-top:8px;
        }
    </style>
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
                                Measurement Range:
                            </div>
                            <div class="main-title-right">
                                <a href="javascript:void(0);" class="show_hideMainDiv" runat="server" id="btnClose"
                                    onclick="window.close();">
                                    <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                            </div>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td></td>
                                     <td colspan="2" style="text-align:left;">
                                        <asp:CheckBox ID="chkactive" runat="server" Text="&nbsp;Is Change" onchange="clicksavebutton();" />

                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>Minimum

                                    </td>
                                    <td>Maximum

                                    </td>
                                </tr>
                                <tr>
                                    <td>Width

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMinwidth" onkeypress="return keyRestrict(event,'0123456789');" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMaxwidth" onkeypress="return keyRestrict(event,'0123456789');" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>Length

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMinlength" onkeypress="return keyRestrict(event,'0123456789');" runat="server"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMaxlength" onkeypress="return keyRestrict(event,'0123456789');" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                   <td></td>
                                    <td id="tdnsave" style="display: none;" colspan="2">
                                        <asp:ImageButton ID="btnsaverange" runat="server" ImageUrl="/App_Themes/Gray/images/save.gif" OnClick="btnsaverange_Click" /></td>

                                </tr>
                            </table>
                        </td>

                    </tr>
                    <tr>
                        <th>
                            <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                                <asp:Label ID="lblfabrictype" runat="server"></asp:Label>
                                : 
                                <asp:Label ID="lblcode" runat="server"></asp:Label>
                            </div>

                        </th>
                    </tr>
                    <tr>
                        <td style="padding: 2px">
                            <div id="poorderprint" style="border: 0px solid #e7e7e7; overflow-y: auto; height: 280px; padding-top: 2px;">
                                <asp:GridView ID="gvfabric" runat="server" AutoGenerateColumns="False" Width="100%"
                                    class="order-table" Style="border: solid 1px #e7e7e7" ShowFooter="true" OnRowDataBound="gvfabric_RowDataBound">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <label style="color: Red">
                                            No records found ...</label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                SKU
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsku" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"sku") %>'></asp:Label>
                                                <asp:Label ID="lblproductid" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"productid") %>'></asp:Label>
                                                <asp:Label ID="lblisRomanp" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"isRomanp") %>'></asp:Label>

                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Name
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lblname" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                No. of Days
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtdays" Width="50px" Height="30px" Style="text-align: center;" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductFabricDays") %>' onkeypress="return keyRestrict(event,'0123456789');" onchange="clicksaveshowproduct();" onkeyup="clicksaveshowproduct();"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" CssClass="tdpadding" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Inventory
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lblinv" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Inventory") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Status
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lblstatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Active") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Is Custom
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lblcustom" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Iscustom") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>


                                    </Columns>
                                    <FooterStyle CssClass=".order-table td" />
                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="border:none;display:none;" id="tdsaveproduct">
                            <asp:ImageButton ID="btnSave" runat="server" ImageUrl="/App_Themes/Gray/images/save.gif" OnClick="btnSave_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
