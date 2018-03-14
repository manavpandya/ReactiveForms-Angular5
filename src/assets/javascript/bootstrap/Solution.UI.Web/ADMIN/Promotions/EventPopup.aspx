<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EventPopup.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.EventPopup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts-main.js"></script>
    <title></title>
    <style type="text/css">
        body
        {
            background: none;
            font-family: Tahoma;
            font-size: 12px;
        }
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

        function keyRestrictforIntOnly(e, validchars) {
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

        //function getpid()
        //{
        //    var id = window.opener.document.getElementById('ContentPlaceHolder1_txtvalidforprod').innerHTML;

        //    if (document.getElementById('hdnparentpid') != null)
        //    {
        //    document.getElementById('hdnparentpid').value = id;
        //    //alert(document.getElementById('hdnparentpid').value);
        //  }

        //}

    </script>

      <script type="text/javascript">
          //$(document).ready(function () {
          //    var id = window.opener.document.getElementById('ContentPlaceHolder1_txtvalidforprod').innerHTML;
          //    alert(id);
          //    if (document.getElementById('hdnparentpid') != null) {
          //        document.getElementById('hdnparentpid').value = id;
          //        alert("1");
          //    }
          //});
    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    
    <div id="dvProduct" runat="server">
        <table style="width: 100%" cellpadding="3" cellspacing="3" class="content-table border-td">
            <tr>
                 
                       <td colspan="3" width="70%;">
                           <asp:Label ID="lblselectedpid" runat="server" Text="Selected Product(s)" ></asp:Label>
                       </td>
                        <td align="right">
                            <asp:ImageButton ID="btnSaveProducts" runat="server"  AlternateText="Save Changes"
                        onclick="btnSaveProducts_Click" />&nbsp;&nbsp;
                             <asp:ImageButton ID="btnproductclose" runat="server" OnClientClick="return closeWin();" />
                            </td>
                
               
            </tr>
                <tr>
                <td colspan="4">
                    
                     <div style="float:left;overflow-y:auto;max-height:200px;width:100%;">
                           <asp:GridView ID="grdselectedproduct" runat="server" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="No Record Found!" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                        EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="False" PageSize="15" OnRowCommand="grdselectedproduct_RowCommand">
                        <Columns>
                            
                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("Name") %>'></asp:Label>
                                     <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SKU">
                                <ItemTemplate>
                                   <asp:Label ID="lblsku" runat="server" Text='<%#Eval("SKU") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remove">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btndel" runat="server" ImageUrl="~/Admin/images/btndel.gif"
                                                                OnClientClick="javascript:if(confirm('Are you sure , you want to delete this Product ?')){ chkHeight();}else{return false;}"
                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductID") %>' CommandName="delMe" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="center" />
                                                        <ItemStyle Width="8%" />
                                                    </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle HorizontalAlign="Center"></EmptyDataRowStyle>
                        <HeaderStyle ForeColor="White"></HeaderStyle>
                        <PagerSettings Position="TopAndBottom" />
                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                        <RowStyle ForeColor="Black"></RowStyle>
                    </asp:GridView>

                         </div>

                </td>
            </tr>
            <tr>
                
                <td width="15%" >
                    <asp:RadioButtonList ID="rdosearch" runat="server"   RepeatDirection="Horizontal" >
                        <asp:ListItem  Value="1" Selected="True">Single</asp:ListItem>
                        <asp:ListItem  Value="2">Multi</asp:ListItem>
                    </asp:RadioButtonList> 
                    
                </td>
                 
                <td style="width: 40%">
                   Search :&nbsp;&nbsp;
                    <asp:TextBox ID="txtproductsearch" runat="server" Width="80%"></asp:TextBox>
                      <asp:HiddenField ID="hdnparentpid" Value="0" runat="server" />
                </td>
                <td style="width: 20%">
                    <asp:ImageButton ID="btnproductsearch" runat="server" OnClientClick="return fvalidation();"
                        OnClick="btnproductsearch_Click" />&nbsp;
                    <asp:ImageButton ID="btnproductshowall" runat="server" OnClick="btnproductshowall_Click" />
                </td>
                <td style="width: 20%" align="center">
                    <asp:ImageButton ID="btnproductaddtoselect" runat="server" OnClientClick="return fcheckCount();"
                        OnClick="btnproductaddtoselect_Click" />&nbsp;&nbsp;
                    
                   
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
                <td colspan="2" align="left">
                    <asp:Label ID="lblProducterror" runat="server" ForeColor="Red"></asp:Label>
               
       </td>
                </tr>
            <tr>
                <td colspan="4">
                     <div style="float:left;overflow-y:auto;height:320px;width:100%;">

                    <asp:GridView ID="grdProduct" runat="server" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="No Record Found!" RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b"
                        EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="False" PageSize="15" OnPageIndexChanging="grdProduct_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="8%">
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="1" border="0" align="center">
                                        <tr style="border: 0px;">
                                            <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                <strong>Select</strong>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                    <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Name">
                                <ItemTemplate>
                                    <%#Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SKU">
                                <ItemTemplate>
                                    <%#Eval("SKU") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle HorizontalAlign="Center"></EmptyDataRowStyle>
                        <HeaderStyle ForeColor="White"></HeaderStyle>
                        <PagerSettings Position="TopAndBottom" />
                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                        <RowStyle ForeColor="Black"></RowStyle>
                    </asp:GridView>
                         </div>
                </td>
            </tr>
        </table>
    </div>
   
    
    <script language="javascript" type="text/javascript">
        function closeWin() {
            window.close();
        }
        function checkCount() {

            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Check at least One Record !', 'Message'); });
                //                alert('Check at least One Record!', 'Message');
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript" language="javascript">
        function fvalidation() {
            var a = document.getElementById('<%=txtproductsearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Enter Keyword to Search Product!', 'Message', '<%=txtproductsearch.ClientID %>'); });
                return false;
            }
            return true;
        }

    </script>
    <script language="javascript" type="text/javascript">
        function fcheckCount() {

            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Check at least One Record !', 'Message'); });
                //                alert('Check at least One Record !', 'Message');
                return false;
            }
            return true;
        }
    </script>
    
    <script language="javascript" type="text/javascript">
        function bcheckCount() {

            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }

            if (count == 0) {
                $(document).ready(function () { jAlert('Check at least One Record !', 'Message'); });
                return false;
            }
            return true;
        }
    </script>
    </form>
</body>
</html>
