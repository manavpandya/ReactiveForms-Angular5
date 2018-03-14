<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="Productsdisplayorder.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.Productsdisplayorder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
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
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add Country" alt="Add Country" src="/App_Themes/<%=Page.Theme %>/images/category-list-icon.png">
                                                    <h2>Product Display Order List</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                                            </td>

                                        </tr>
                                        <tr class="even-row">
                                            <td>

                                                <asp:HiddenField ID="hdnCategory" runat="server" />
                                                <asp:GridView ID="grdParentCategory" runat="server" GridLines="None" AutoGenerateColumns="false" Width="100%"
                                                    AllowPaging="false" DataKeyNames="CategoryID" CssClass="border-td"
                                                    ShowFooter="false" OnRowDataBound="grdParentCategory_RowDataBound" OnRowCommand="grdParentCategory_RowCommand">
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="&nbsp;Category Name">
                                                            <ItemTemplate>
                                                                <div style="float: left;">
                                                                    <a href="javascript:expandcollapse('divchild<%# Eval("CategoryID") %>', 'one');">
                                                                        <img id="imgdiv<%# Eval("CategoryID") %>" title="Collapse" alt="Expand" src="/images/expand.gif">
                                                                    </a>
                                                                </div>
                                                                &nbsp;&nbsp;<asp:Literal ID="lttitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Literal><span
                                                                    style="float: right;">
                                                                    <%--<asp:ImageButton ID="ImgDelete" OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){chkHeight();  return true;}else{return false;}"
                                                                            runat="server" ImageUrl="/images/delete-icon.gif" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ParentCategoryID") %>' />--%>
                                                                    <%--<input type="hidden" id="hdnisPublish" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"IsPublish") %>' />--%>
                                                                </span>
                                                                <%--  <input type="hidden" id="hdnParentCategoryID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"ParentCategoryID") %>' />--%>
                                                                <input type="hidden" id="hdnCategoryID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"CategoryID") %>' />
                                                            </ItemTemplate>

                                                            <HeaderStyle Width="37.5%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Display&nbsp;Order" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="left">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="1" border="0" align="center">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                            Display&nbsp;Order
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtDisplayOrder" runat="server" Text=' <%#Eval("DisplayOrder").ToString().Equals("999")?null:Eval("DisplayOrder")%>'></asp:Label>

                                                            </ItemTemplate>

                                                            <HeaderStyle HorizontalAlign="Left" Width="39.5%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                                                            HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%--<img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />--%>
                                                                <asp:HiddenField ID="hdnActive" runat="server" Value='<%# (DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                                                <asp:Literal ID="ltrStatus" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="11%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td colspan="100%">

                                                                        <div id="divchild<%# Eval("CategoryID") %>" style="display: none; position: relative; left: 15px; overflow: auto; width: 97%; margin-top: 10px;">
                                                                            <asp:GridView ID="gvCategory" AutoGenerateColumns="false" runat="server" GridLines="None" DataKeyNames="CategoryID"
                                                                                Width="100%" CellPadding="2" CellSpacing="1" CssClass="border-td"
                                                                                RowStyle-ForeColor="Black" HeaderStyle-ForeColor="#3c2b1b" PagerSettings-Mode="NumericFirstLast" ShowFooter="true"
                                                                                AllowPaging="false" AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                OnRowDataBound="gvCategory_RowDataBound" OnRowCommand="gvCategory_RowCommand"
                                                                                OnPageIndexChanging="gvCategory_PageIndexChanging" PageSize="20">
                                                                                <EmptyDataTemplate>
                                                                                    <span style="color: Red; font-size: 12px; text-align: center;">No Record(s) Found !</span>
                                                                                </EmptyDataTemplate>
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" CssClass="input-checkbox" />
                                                                                            <asp:HiddenField ID="hdnCatid" runat="server" Value='<%#Eval("CategoryID") %>' />
                                                                                            <asp:HiddenField ID="hdnProductID" runat="server" Value='<%#Eval("ProductID") %>' />
                                                                                            <%-- <asp:HiddenField ID="hdnParentCatID" runat="server" Value='<%#Eval("ParentCategoryID") %>' />--%>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <div class="row-fluid" style="width: 150px;">

                                                                                                <span><a id="lkbAllowAll" runat="server" class="btn btn-mini btn-info">Check All</a> | <a id="lkbClearAll" runat="server"
                                                                                                    class="btn btn-mini btn-info">Clear All</a> </span><span style="float: right;"></span>

                                                                                            </div>
                                                                                        </FooterTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="tdpro-checkbox" Width="7%" />
                                                                                        <ItemStyle HorizontalAlign="Center" CssClass="tdpro-checkbox" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Product Name" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"
                                                                                        HeaderStyle-HorizontalAlign="Left">
                                                                                        <HeaderTemplate>
                                                                                            <table cellpadding="0" cellspacing="1" border="0" align="left">
                                                                                                <tr style="border: 0px;">
                                                                                                    <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                                                        Product Name 

                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <div style="text-align: justify; width: 300px;">
                                                                                                <%--<a href='<%# "Category.aspx?Mode=edit&ID="+ DataBinder.Eval(Container.DataItem, "CategoryID") +"&storeid="+DataBinder.Eval(Container.DataItem, "StoreID")    %>'>--%>
                                                                                                <asp:Literal ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Literal>
                                                                                                <%-- </a>--%>
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Display&nbsp;Order" ItemStyle-HorizontalAlign="Center"
                                                                                        ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="left">
                                                                                        <HeaderTemplate>
                                                                                            <table cellpadding="0" cellspacing="1" border="0" align="center">
                                                                                                <tr style="border: 0px;">
                                                                                                    <td style="border: 0px; padding: 0px; background-color: transparent;">
                                                                                                        Display&nbsp;Order 
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtDisplayOrder" runat="server" Text=' <%#Eval("DisplayOrder").ToString().Equals("999")?null:Eval("DisplayOrder")%>'></asp:TextBox>

                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:ImageButton CommandName="ChildAllSave" ID="btnSave" runat="server" />
                                                                                        </FooterTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>

                                                                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                                                                                        HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <%--<img src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>' />--%>
                                                                                            <asp:HiddenField ID="hdnActive" runat="server" Value='<%# (DataBinder.Eval(Container.DataItem,"Active")) %>' />
                                                                                            <asp:Literal ID="ltrStatus" runat="server"></asp:Literal>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="29px"
                                                                                        HeaderStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>

                                                                                            <asp:ImageButton CommandName="ChildSingleSave" ID="btnSingleSave" runat="server"></asp:ImageButton>
                                                                                            <%-- <a title='' id="hplEdit" runat="server" href='<%# "Category.aspx?Mode=edit&ID="+ DataBinder.Eval(Container.DataItem, "CategoryID") +"&storeid="+DataBinder.Eval(Container.DataItem, "StoreID")    %>'><span class="green"><i class="icon-edit bigger-160"></i></span></a>--%>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" Width="29px" />

                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                                                <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                                                <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </td>

                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" VerticalAlign="Top" />
                                                    <AlternatingRowStyle CssClass="altrow" VerticalAlign="Top" />
                                                    <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>


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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div style="display: none;">
        <input type="hidden" id="hdnrowid" runat="server" value="" />
        <input type="hidden" id="hdngroupid" runat="server" value="" />
        <input type="hidden" id="hdngroupactiontype" runat="server" value="" />
        <input type="hidden" id="hdngroupname" runat="server" value="" />
        <table style="width: 100%; margin-top: 10px;">
            <tr id="tr1" runat="server">
                <td style="width: 20%; float: left;">
                    <div class="span6">
                        <div class="row-fluid" style="width: 900px; display: none;">

                            <span><a id="lkbAllowAll" href="javascript:selectAll(true);" class="btn btn-mini btn-info">Check All</a> | <a id="lkbClearAll"
                                href="javascript:selectAll(false);" class="btn btn-mini btn-info">Clear All</a> </span><span style="float: right;"></span>

                        </div>
                    </div>
                </td>
                <td style="float: right; display: none;">
                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-mini btn-info"
                        OnClientClick="return checkCount();" ToolTip="Delete">Delete</asp:LinkButton>
                    <div style="display: none;">
                        <asp:Button ID="Button1" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
   

    <script language="javascript" type="text/javascript">


        function expandcollapse(obj, row) {
            var alldiv = document.getElementById('ContentPlaceHolder1_grdParentCategory').getElementsByTagName('div');


            if (alldiv != null && alldiv.length > 0) {
                for (var i = 0; i < alldiv.length; i++) {
                    var divall = alldiv[i];

                    if (divall.id == obj) {
                        var div = document.getElementById(obj);
                        var img = obj.toString().replace('divchild', 'imgdiv'); //

                        if (div.style.display == "none") {
                            div.style.display = "block";
                            document.getElementById('ContentPlaceHolder1_hdnrowid').value = obj;
                            if (row == 'alt') {
                                //document.getElementById(img).src = "/images/collapse.png";

                                document.getElementById(img).src = '/images/minimize.png';

                            }
                            else {
                                //document.getElementById(img).src = "/images/collapse.png";

                                document.getElementById(img).src = '/images/minimize.png';
                            }
                            //document.getElementById(img).alt = "Close to view other Listing";
                        }
                        else {
                            div.style.display = "none";
                            document.getElementById('ContentPlaceHolder1_hdnrowid').value = "";
                            if (row == 'alt') {
                                //document.getElementById(img).src = "/images/expand.png";

                                document.getElementById(img).src = '/images/expand.gif';
                            }
                            else {
                                //document.getElementById(img).src = "/images/expand.png";

                                document.getElementById(img).src = '/images/expand.gif';


                            }
                            //document.getElementById(img).alt = "Expand to show Listing";
                        }
                    }
                    else if (divall.id != "" && divall.id != obj) {

                        var imgid = divall.id.toString().replace('divchild', 'imgdiv');
                        var img = document.getElementById(imgid);
                        divall.style.display = "none";
                        if (row == 'alt') {
                            //img.src = "/images/expand.png";

                            img.src = '/images/expand.gif';
                        }
                        else {
                            // img.src = "/images/expand.png";

                            img.src = '/images/expand.gif';
                        }
                        //img.alt = "Expand to show Listing";

                    }
                }
            }
        }
    </script>



    <script language="javascript" type="text/javascript">
        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (on.toString() == 'false') {

                        if (myform.elements[i].checked) {
                            myform.elements[i].checked = false;
                        }
                    }
                    else {
                        myform.elements[i].checked = true;
                    }
                }
            }

        }
        function selectAllGrid(on, id) {
            var myform = document.getElementById(id).getElementsByTagName('input');
            var len = myform.length;
            for (var i = 0; i < len; i++) {
                if (myform[i].type == 'checkbox') {
                    if (on.toString() == 'false') {

                        if (myform[i].checked) {
                            myform[i].checked = false;
                        }
                    }
                    else {
                        myform[i].checked = true;
                    }
                }
            }

        }

        function SetBlank(id) {
            var myform = document.getElementById(id).getElementsByTagName('input');
            var len = myform.length;

            for (var i = 0; i < len; i++) {
                if (myform[i].type == 'text') {
                    var chkid = myform[i].id.toString().replace('_txtDisplayOrder_', '_chkSelect_');

                    if (document.getElementById(chkid).checked && myform[i].value == '') {

                        jAlert('PLease enter display order', 'Required', myform[i].id.toString());
                        return false;

                    }
                }
            }
            return true;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function checkCount(id) {
            var myform = document.getElementById(id).getElementsByTagName('input');
            var len = myform.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform[i].type == 'checkbox' && myform[i].checked == true) {
                    count++;
                }
            }
            if (count == 0) {
                jAlert('Check at least One Record!', 'Message');
                return false;
            }
            if (SetBlank(id)) {
                return true;
            }
            else {
                return false;
            }
            return true;
        }
        function checkaa() {
            jConfirm('Are you sure want to delete all selected Category ?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById('ContentPlaceHolder1_btnDeleteTemp').click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }
    </script>
</asp:Content>
