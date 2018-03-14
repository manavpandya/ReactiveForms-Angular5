<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewminvendorPrice.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ViewminvendorPrice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .hidediv {
            display: none;
        }

        .right-1 {
            float: right;
        }
        #chkrevertparent{padding-left:10px;padding-right:10px;vertical-align:middle;}
        #chkusefixprice{padding-left:10px;padding-right:10px;vertical-align:middle;}
    </style>
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
        function loadfrm() {
            var tt = $('.fancybox-inner', window.parent.document);

            var tscreen = window.parent.innerHeight;

            if (parseInt(form1.scrollHeight) > parseInt(tscreen)) {

                $('.fancybox-opened', window.parent.document).css('top', '20px');

                tt.attr('style', 'height:' + (tscreen - 55) + 'px !important;');
            }
            else {
                $('.fancybox-opened', window.parent.document).css('top', '20px');
                tt.attr('style', 'height:' + form1.scrollHeight + 'px !important;');
            }

        }
    </script>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Checkfields() {

            if (document.getElementById('chkusefixprice') != null && document.getElementById('chkusefixprice').checked==true && document.getElementById('txtfixprice').value == '') {
                jAlert('Please enter Fix Price.', 'Message', 'txtfixprice');
                return false;
            }


            return true;
        }
        function ShowHidetd() {
           <%-- if (document.getElementById('<%=chkrevertparent.ClientID %>').checked) {

                if (document.getElementById('<%=tdchkusefixprice.ClientID %>') != null) {
                    document.getElementById('<%=tdchkusefixprice.ClientID %>').style.display = 'none';
                }
            }--%>
           if (document.getElementById('chkusefixprice') != null) {
               if (document.getElementById('chkusefixprice').checked) {
                   if (document.getElementById('tdfixprice') != null) {
                       document.getElementById('tdfixprice').style.display = '';
                   }
                   if (document.getElementById('tdformulaprice') != null) {
                       document.getElementById('tdformulaprice').style.display = 'none';
                   }

                   
               }
               else {
                   if (document.getElementById('tdfixprice') != null) {
                       document.getElementById('tdfixprice').style.display = 'none';
                   }
                   if (document.getElementById('tdformulaprice') != null) {
                       document.getElementById('tdformulaprice').style.display = '';
                   }
               }


           }

       }

    </script>

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
                                <div style="font-size: 16px; color: #000; font-weight: bold;">Min Price Configuration</div>
                            </div>
                            <div class="main-title-right">
                                <a href="javascript:void(0);" class="show_hideMainDiv" runat="server" id="btnClose"
                                    onclick="window.close();">
                                    <img id="imgMainDiv" class="close" title="close" style="display: none;" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                            </div>
                        </th>
                    </tr>
                     <tr>
                    <td style="padding: 2px">
                        Product Name: <asp:Label ID="lblname" runat="server"></asp:Label>
                        <br />
                        SKU:<asp:Label ID="lblsku" runat="server"></asp:Label>
                        <br />
                        ASIN:<asp:Label ID="lblasin" runat="server"></asp:Label>
                        </td>
                    </tr>
               


                    <tr id="tdformulaprice" runat="server">
                        <td style="padding: 2px">
                            <div id="poorderprint" style="border: 5px solid #e7e7e7;padding-top: 2px;">
                                <asp:GridView ID="gvminprice" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataText="No Record Found." OnRowDataBound="gvminprice_RowDataBound" OnRowEditing="gvminprice_RowEditing" OnRowDeleting="gvminprice_RowDeleting" OnRowCancelingEdit="gvminprice_RowCancelingEdit"
                                    class="order-table" Style="border: solid 1px #e7e7e7;margin-bottom:20px;" ShowFooter="true" PageSize="30" AllowPaging="false" OnPageIndexChanging="gvminprice_PageIndexChanging" OnRowCommand="gvminprice_RowCommand">
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <EmptyDataTemplate>
                                        <label style="color: Red">
                                            No records found ...</label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div style="float: left; font-weight: bold;">Field Name</div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblfieldname" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FieldName") %>'></asp:Label>

                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div class="right-1" style="font-weight: bold;">Percentage(%)</div>
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:Label ID="lblPercentage" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Percentage") %>'></asp:Label>
                                                <asp:TextBox ID="txtPercentage" onkeypress="return keyRestrict(event,'0123456789');" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Percentage") %>' Visible="false"></asp:TextBox>

                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" />
                                            <HeaderStyle HorizontalAlign="right" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div class="right-1" style="font-weight: bold;">Cost</div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                $<asp:Label ID="lblPrice" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Cost") %>'></asp:Label>
                                                <asp:Label ID="lbltotal1" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Total") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" />
                                            <HeaderStyle HorizontalAlign="right" />
                                            <FooterTemplate>
                                                $<asp:Label ID="lbltotal" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Total") %>'></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Operations">
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                            <ItemTemplate>
                                                <asp:ImageButton CssClass="btnclass" Style="border: 1px solid #ba2b19;" runat="server" ID="_editLinkButton" ToolTip="Edit"
                                                    CommandName="edit" CommandArgument='<%# Eval("MinChildID") %>'></asp:ImageButton>
                                                <asp:ImageButton CssClass="btnclass" ID="btnSave" Visible="false"
                                                    runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"MinChildID") %>'
                                                    CommandName="Save"></asp:ImageButton>&nbsp;<asp:ImageButton CssClass="btnclass" ID="btnCancel" runat="server" Visible="false" CommandName="Cancel"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"MinChildID") %>'></asp:ImageButton>


                                            </ItemTemplate>
                                        </asp:TemplateField>



                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                    <FooterStyle CssClass=".order-table td" />
                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                    <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                    <PagerSettings Position="TopAndBottom" />
                                </asp:GridView>
                                <asp:CheckBox ID="chkrevertparent" runat="server" Text="Revert To Parent " TextAlign="Left" />
                                <asp:HiddenField ID="hdnflag" runat="server" Value="0" />
                            </div>
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 2px;padding-top:8px">
                            <table width="100%">
                                <tr>
                                    
                                    <td id="tdchkusefixprice" runat="server" style="width:10%;">
                                        <asp:CheckBox ID="chkusefixprice" runat="server" Text="Use Fix Price " TextAlign="Left" onchange="ShowHidetd();loadfrm();" />
                                    </td>
                                    <td id="tdfixprice" runat="server">
                                        <asp:TextBox ID="txtfixprice" Style="width:120px;" runat="server" onkeypress="return keyRestrict(event,'0123456789');"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                            OnClientClick="return Checkfields();" OnClick="btnSave_Click" />
                                        <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel" Style="display:none;"
                                            CausesValidation="false" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>


                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
