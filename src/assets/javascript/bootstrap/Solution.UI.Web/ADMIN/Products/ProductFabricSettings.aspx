<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="ProductFabricSettings.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ProductFabricSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function ValidatePage() {
            if (document.getElementById('ContentPlaceHolder1_txtFabricTypeName')) {
                if ((document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value).replace(/^\s*\s*$/g, '') == '') {
                    jAlert('Please Enter Fabric Category Name', 'Message', 'ContentPlaceHolder1_txtFabricTypeName');
                    return false;
                }
            }
            return true;
        }
        function OpenCenterWindow(vendorid, wi, he) {
            var w = wi;
            var h = he;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            window.open(vendorid, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
        function validation() {
            var a = document.getElementById('<%=txtSearch.ClientID %>').value;
            if (a == "") {
                $(document).ready(function () { jAlert('Please Enter Search Value.', 'Message', '<%=txtSearch.ClientID %>'); });
                 return false;
             }
             else {
                 chkHeight();
             }
             return true;
         }
         function clearField() {
             if (document.getElementById('ContentPlaceHolder1_txtFabricTypeName')) {
                 document.getElementById('ContentPlaceHolder1_txtFabricTypeName').value = '';
             }
             if (document.getElementById('ContentPlaceHolder1_txtDisplayOrder')) {
                 document.getElementById('ContentPlaceHolder1_txtDisplayOrder').value = '';
             }
             if (document.getElementById('ContentPlaceHolder1_chkActive')) {
                 document.getElementById('ContentPlaceHolder1_chkActive').checked = false;
             }
             return false;
         }
         function CheckValidation(id, msg) {
             if (document.getElementById(id) != null && document.getElementById(id).value == '') {
                 jAlert(msg, 'Message', id);
                 return false;
             }
             return true;
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
    </script>
    <script type="text/javascript">
        function checkfabricCode(RowIndex)
        {
           
           
            if (document.getElementById('ContentPlaceHolder1_grdFabricType_grdFabricCode_0_chkActive_' + RowIndex) != null && document.getElementById('ContentPlaceHolder1_grdFabricType_grdFabricCode_0_hdnchkactive_' + RowIndex) != null)
            {
                var oldvalue = document.getElementById('ContentPlaceHolder1_grdFabricType_grdFabricCode_0_hdnchkactive_' + RowIndex ).value;
                
                if (!document.getElementById('ContentPlaceHolder1_grdFabricType_grdFabricCode_0_chkActive_' + RowIndex).checked)
                {
                    var newvalue = "0";
                    if(oldvalue.toLowerCase()=="true" || oldvalue=="1")
                    {
                        javascript: if (confirm('Are you sure want to InActive this record?')) { chkHeight(); return true; } else { return false; }
                    }
                    else
                    {
                        return true;
                    }
                }

            }

            return true;
           
        }


        function checkfabriccategory(RowIndex)
        {
            
            if (document.getElementById('ContentPlaceHolder1_grdFabricType_chkActive_' + RowIndex) != null && document.getElementById('ContentPlaceHolder1_grdFabricType_hdnchkactive_' + RowIndex) != null) {
                var oldvalue = document.getElementById('ContentPlaceHolder1_grdFabricType_hdnchkactive_' + RowIndex).value;
               
                if (!document.getElementById('ContentPlaceHolder1_grdFabricType_chkActive_' + RowIndex).checked) {
                    var newvalue = "0";
                    if (oldvalue.toLowerCase() == "true" || oldvalue == "1") {
                        javascript: if (confirm('Are you sure want to InActive this record?')) { chkHeight(); return true; } else { return false; }
                    }
                    else {
                        return true;
                    }
                }

            }

            return true;
        }
    </script>
    <script runat="server">
        string _ReturnUrl;
        public string SetImage(bool _Value)
        {
            if (_Value == true)
            {
                _ReturnUrl = "../Images/isActive.png";
            }
            else
            {
                _ReturnUrl = "../Images/isInactive.png";
            }
            return _ReturnUrl;
        }
    </script>
    <style type="text/css">
        .altrowmain td {
            background: #E3E3E3 !important;
            color: #000 !important;
        }

        .altrowmainsub td {
            background: #ffffff !important;
            color: #000 !important;
        }
        /*.content-table th{background:#545454 !important;color:#000 !important;}*/
    </style>
    <style type="text/css">
        .divfloatingcss {
            bottom: 0;
            right: 0;
            position: fixed;
            width: 10%;
            margin-right: 40%;
            background-image: url("/Admin/images/title-bg-trans.png");
            -webkit-border-top-left-radius: 15px;
            -webkit-border-top-right-radius: 15px;
            -moz-border-radius-topleft: 15px;
            -moz-border-radius-topright: 15px;
            border-top-left-radius: 15px;
            border-top-right-radius: 15px; /*-webkit-border-radius: 20px;
-moz-border-radius: 20px;
border-radius: 20px;*/
        }

        .auto-style1 {
            width: 11%;
        }

        .auto-style2 {
            width: 9%;
        }
    </style>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#divfloating').attr("class", "divfloatingcss");
            $(window).scroll(function () {
                if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                    $('#divfloating').attr("class", "");
                }
                else {
                    $('#divfloating').attr("class", "divfloatingcss");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order">
            &nbsp;
        </div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="10" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr style="width: 1126px;">
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Product Options" alt="Product Options" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                <h2>Product Fabric</h2>
                                            </div>
                                            <div class="main-title-right" style="display: none;">
                                                <a href="javascript:void(0);" class="show_hideMainDiv" onclick="return ShowHideButton('imgMainDiv','tdMainDiv');">
                                                    <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/close.png" /></a>
                                            </div>
                                            <div class="main-title-right">
                                                <a title="Back" id="BackLink" runat="server" visible="false">
                                                    <img title="Back" alt="Back" src="/App_Themes/<%=Page.Theme %>/button/back.png" /></a>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td id="tdMainDiv">
                                            <div id="divMain" class="slidingDivMainDiv">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                                    <tr class="altrow">
                                                        <td align="center">
                                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="even-row" id="trAddOption" runat="server">
                                                        <td align="center">
                                                            <fieldset>
                                                                <legend><b>Add Fabric Category</b></legend>
                                                                <table width="99%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr class="oddrow">
                                                                        <td width="13%">
                                                                            <span class="star">*</span>Fabric Category Name :
                                                                        </td>
                                                                        <td width="88%" align="left">
                                                                            <asp:TextBox ID="txtFabricTypeName" runat="server" MaxLength="100" class="order-textfield"
                                                                                Width="250px"></asp:TextBox>
                                                                        </td>

                                                                    </tr>
                                                                    <tr class="altrow">
                                                                        <td width="13%">
                                                                            <span class="star">&nbsp;</span>Display Order :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtDisplayOrder" runat="server" MaxLength="9" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                Width="100px" CssClass="order-textfield"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr class="oddrow">
                                                                        <td width="13%">
                                                                            <span class="star">&nbsp;</span>Active :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:CheckBox ID="chkActive" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trSavecancel" runat="server" class="altrow">
                                                                        <td width="13%"></td>
                                                                        <td align="left">
                                                                            <asp:ImageButton ID="btnSave" runat="server" OnClientClick="return ValidatePage();"
                                                                                OnClick="btnSave_Click" />
                                                                            <asp:ImageButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" OnClientClick="return clearField();" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr class="even-row" id="trFabricDetails" runat="server">
                                                        <td align="left">
                                                            <fieldset>
                                                                <legend><b>Add Fabric Code</b></legend>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr class="oddrow">
                                                                        <td align="left"></td>
                                                                        <td align="left">
                                                                            <span class="star">*</span>Fabric Category Name :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList ID="ddlFabricType" runat="server" class="product-type" OnSelectedIndexChanged="ddlFabricType_SelectedIndexChanged"
                                                                                AutoPostBack="true">
                                                                            </asp:DropDownList></td>
                                                                        <td align="left">
                                                                            <span class="star"></span>Code :
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtSearch" runat="server" Width="160px" CssClass="order-textfield"></asp:TextBox>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
                                                                        </td>
                                                                          <td align="left">
                                                                            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" /></td>
                                                                           <td align="left" style="float:right;margin-left:700px;">
 <asp:Button ID="btnimport" PostBackUrl="~/ADMIN/Products/ImportFabricData.aspx"  runat="server" OnClick="btnExport_Click" />
                                                                               </td>
                                                                    </tr>

                                                                </table>
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">

                                                                    <tr>
                                                                        <td width="100%">
                                                                            <asp:GridView ID="grdFabricType" runat="server" GridLines="None" AutoGenerateColumns="false"
                                                                                CssClass="checklist-main border-right-all" AllowPaging="false" DataKeyNames="FabricTypeID"
                                                                                BorderColor="#d7d7d7" BorderWidth="1px" BorderStyle="Solid" ShowFooter="true"
                                                                                OnRowDataBound="grdFabricType_RowDataBound" OnRowCommand="grdFabricType_RowCommand"
                                                                                OnRowEditing="grdFabricType_RowEditing" Width="100%" EmptyDataText="No Records Found(0)" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-ForeColor="Red">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="&nbsp;Fabric Category">
                                                                                        <ItemTemplate>
                                                                                            <%--<div style="float: left; margin-top: 3px;">
                                                                                                <img src="/Admin/images/expanded.png" alt="" border="0" />
                                                                                            </div>--%>
                                                                                            &nbsp;&nbsp;<asp:Literal ID="ltrFabricTypeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FabricTypename") %>'></asp:Literal>
                                                                                            <asp:TextBox ID="txtFabricTypeName" runat="server" Style="width: 600px;" Visible="false"
                                                                                                CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"FabricTypename") %>'></asp:TextBox>
                                                                                            <input type="hidden" id="hdnFabricTypeID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricTypeID") %>' />
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle Width="70%" HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="&nbsp;Is Active">
                                                                                        <ItemTemplate>
                                                                                            <img id="imgTypeActive" runat="server" src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>'
                                                                                                alt="" />
                                                                                            <asp:CheckBox ID="chkActive" runat="server" Visible="false" Checked='<%#Convert.ToBoolean(Eval("Active")) %>' />
                                                                                             <input type="hidden" id="hdnchkactive" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Active") %>' />
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="&nbsp;Display Order">
                                                                                        <ItemTemplate>
                                                                                            &nbsp;&nbsp;<asp:Literal ID="ltdisplayorder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:Literal>
                                                                                            <asp:TextBox ID="txtdisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                MaxLength="6" runat="server" Style="width: 80px; text-align: center;" Visible="false"
                                                                                                CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <FooterStyle HorizontalAlign="Left" />
                                                                                        <HeaderStyle Width="12%" HorizontalAlign="Center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="&nbsp;Action">
                                                                                        <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="imgSave" ToolTip="Save Fabric Category" runat="server" ImageUrl="/App_Themes/Gray/Images/save.png"
                                                                                                CommandName="Save" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricTypeID") %>'
                                                                                                Visible="false" />
                                                                                            <asp:ImageButton ID="imgcancel" runat="server" ImageUrl="/admin/Images/isInactive.png"
                                                                                                CommandName="Exit" ToolTip="Cancel Fabric Category" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricTypeID") %>'
                                                                                                Visible="false" />
                                                                                            <asp:ImageButton ID="imgEdit" ToolTip="Edit Fabric Category" runat="server" ImageUrl="/App_Themes/Gray/Images/Edit.gif"
                                                                                                CommandName="Edit" OnClientClick="chkHeight();" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricTypeID") %>' />
                                                                                            <asp:ImageButton ID="imgDelete" ToolTip="Remove Fabric Category" OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){chkHeight(); return true;}else{return false;}"
                                                                                                runat="server" ImageUrl="/App_Themes/Gray/Images/delete-icon.png" CommandName="Remove"
                                                                                                CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricTypeID") %>' />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" />
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <%--<tr>
                                                                                                <td colspan="100%" style="background-color: #d7d7d7; line-height: 18px; border: solid 1px #d8d8d8">
                                                                                                    <b>Add Fabric Code</b>
                                                                                                </td>
                                                                                            </tr>--%>
                                                                                            <tr>
                                                                                                <td colspan="100%" align="left">
                                                                                                    <div id="divchild" style="margin: 2px 0 0 0; position: relative; left: 15px; overflow: auto; padding-bottom: 5px; width: 100%;">
                                                                                                        <asp:GridView ID="grdFabricCode" runat="server" GridLines="None" AutoGenerateColumns="false"
                                                                                                            CssClass="checklist-main border-right-all" AllowPaging="false" ShowHeader="True"
                                                                                                            ShowFooter="true" EmptyDataText="No Record Found(s)." EditRowStyle-HorizontalAlign="Center"
                                                                                                            BorderColor="#d7d7d7" BorderWidth="1px" BorderStyle="Solid" DataKeyNames="FabricCodeID"
                                                                                                            OnRowDataBound="grdFabricCode_RowDataBound" OnRowCommand="grdFabricCode_RowCommand"
                                                                                                            OnRowDeleting="grdFabricCode_RowDeleting" OnRowEditing="grdFabricCode_RowEditing"
                                                                                                            Width="98%">
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Code">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%--<div style="float: left; margin-top: 3px;">
                                                                                                                            <img src="/Admin/images/expanded.png" alt="" border="0" />
                                                                                                                        </div>--%>
                                                                                                                        <asp:Literal ID="ltrFabricCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Code") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtFabricCode" Style="width: 200px;" runat="server" Visible="false"
                                                                                                                            CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"Code") %>'></asp:TextBox>
                                                                                                                        <input type="hidden" id="hdnFabricTypeID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricTypeID") %>' />
                                                                                                                        <input type="hidden" id="hdnFabricCodeID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricCodeID") %>' />
                                                                                                                        <input type="hidden" id="hdnvendorids" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricVendorIds") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle   HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle  />
                                                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtFooterFabricCode" CssClass="order-textfield" Text=""
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                        <input type="hidden" id="hdnFooterFabricTypeID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricTypeID") %>' />
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Name">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltrFabricName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtFabricName" Style="width: 250px;" runat="server" Visible="false"
                                                                                                                            CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle  HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle   />
                                                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtFooterFabricName" CssClass="order-textfield" Text=""
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Left"  Width="20%"/>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Vendor Name">
                                                                                                                    <ItemTemplate>
                                                                                                                       

                                                                                                                            <asp:DropDownList ID="ddlVendorList" runat="server" Enabled="false"></asp:DropDownList>
                                                                                                                            <asp:CheckBoxList ID="chkvendorlist" runat="server" RepeatDirection="Horizontal" Visible="false"
                                                                                                                                RepeatColumns="6">
                                                                                                                            </asp:CheckBoxList>
                                                                                                                       
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle   />
                                                                                                                    <ItemStyle HorizontalAlign="left" />
                                                                                                                    <FooterTemplate>
                                                                                                                      
                                                                                                                             <asp:DropDownList ID="ddlVendorListFooter" runat="server"></asp:DropDownList>
                                                                                                                            <asp:CheckBoxList ID="chkvendorlistfooter" runat="server" RepeatDirection="Horizontal" RepeatColumns="6" Visible="false">
                                                                                                                            </asp:CheckBoxList>
                                                                                                                       
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="Left" Width="20%" />
                                                                                                                </asp:TemplateField>
                                                                                                                     <asp:TemplateField HeaderText="&nbsp;Min Alert Qty">
                                                                                                                    <ItemTemplate>
                                                                                                                          <asp:Literal ID="ltMinQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"MinQty") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtMinQty" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            MaxLength="6" Style="text-align: center;width:60px;" runat="server" Visible="false"
                                                                                                                            CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"MinQty") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                       <HeaderStyle   HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle   />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtfooterMinQty" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            CssClass="order-textfield" Width="60px" Text="" Style="width: 120px; text-align: center;"
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                 
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Per Yard Retail Price">
                                                                                                                    <ItemTemplate>
                                                                                                                          <asp:Literal ID="ltperyard" runat="server" Text='<%# String.Format("{0:0.00}",DataBinder.Eval(Container.DataItem,"YardPrice")) %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtperyard" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            MaxLength="6" Style=" text-align: center;width:60px;" runat="server" Visible="false"
                                                                                                                            CssClass="order-textfield" Text='<%# String.Format("{0:0.00}",DataBinder.Eval(Container.DataItem,"YardPrice") )%>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                       <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtfooterperyard" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            CssClass="order-textfield" Text="" Style="width: 60px; text-align: center;"
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                   <asp:TemplateField HeaderText="&nbsp;Safety Lock">
                                                                                                                        <ItemTemplate>
                                                                                                                          <asp:Literal ID="ltsafetylock" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SafetyLock") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtsaftylock" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            MaxLength="6" Style=" text-align: center;width:60px;" runat="server" Visible="false"
                                                                                                                            CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"SafetyLock") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                          <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtfootersafetylock" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            CssClass="order-textfield"  Text="" Style="width: 60px;text-align: center;"
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                   <asp:TemplateField HeaderText="&nbspDelivery Days">
                                                                                                                        <ItemTemplate>
                                                                                                                          <asp:Literal ID="ltdays" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"NoOfDays") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtdays" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            MaxLength="6" Style=" text-align: center;width:60px;" runat="server" Visible="false"
                                                                                                                            CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"NoOfDays") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                          <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtfooterdays" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            CssClass="order-textfield" Width="60px" Text="" Style="width: 120px; text-align: center;"
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                   <asp:TemplateField HeaderText="&nbspDiscontinue">
                                                                                                                           <ItemTemplate>
                                                                                                                        <img id="imgdiscontinue" runat="server" src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Discontinue"))) %>'
                                                                                                                            alt="" />
                                                                                                                        <asp:CheckBox ID="chkdiscontinue" runat="server" Visible="false" Checked='<%#Convert.ToBoolean(Eval("Discontinue")) %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                             <HeaderStyle  HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle  />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:CheckBox ID="chkfooterdiscontinue" runat="server" />
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Active">
                                                                                                                    <ItemTemplate>
                                                                                                                        <img id="imgCodeActive" runat="server" src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>'
                                                                                                                            alt="" />
                                                                                                                        <asp:CheckBox ID="chkActive" runat="server" Visible="false" Checked='<%#Convert.ToBoolean(Eval("Active")) %>' />
                                                                                                                          <input type="hidden" id="hdnchkactive" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"Active") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:CheckBox ID="chkFooterActive" runat="server" />
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
 <asp:TemplateField HeaderText="&nbsp;UPC">
                                                                                                                    <ItemTemplate>
                                                                                                                       <asp:Literal ID="ltupc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FabricUPC") %>'></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtupc" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                             Style=" text-align: center;width:90px;" runat="server" Visible="false"
                                                                                                                            CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"FabricUPC") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtfooterupc" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            CssClass="order-textfield" Width="90px" Text="" Style="width: 120px; text-align: left;"
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="&nbsp;Display Order"  visible="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Literal ID="ltdisplayorder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>' Visible="false"></asp:Literal>
                                                                                                                        <asp:TextBox ID="txtdisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            MaxLength="6" Style="width: 60px; text-align: center;" runat="server" Visible="false"
                                                                                                                            CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="15%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="15%" />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:TextBox ID="txtFooterDisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                            CssClass="order-textfield" Width="60px" Text="" Style="width: 120px; text-align: center;"
                                                                                                                            runat="server"></asp:TextBox>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="center" />
                                                                                                                </asp:TemplateField >
                                                                                                                   <asp:TemplateField  HeaderText="Assigned<br />Products">
                                                                                                                    <ItemTemplate>
                                                                                                                       <a onclick=" OpenCenterWindow('/Admin/Products/viewcodedetails.aspx?fabriccodeid=' + <%# DataBinder.Eval(Container.DataItem, "FabricCodeID") %> + '',900,500)"
                                                                    href="javascript:void(0);">View</a><br /><a onclick="OpenCenterWindow('/Admin/Products/fabricdescription.aspx?fabriccodeid=' + <%# DataBinder.Eval(Container.DataItem, "FabricCodeID") %> + '',900,500)"
                                                                    href="javascript:void(0);">Description</a>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                    <ItemStyle Width="10%" />
                                                                                                                    <ItemStyle HorizontalAlign="center" />
                                                                                                                 
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Action&nbsp;&nbsp;">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton ID="imgSave" ToolTip="Save Fabric Code" runat="server" ImageUrl="/App_Themes/Gray/Images/save.png"
                                                                                                                            CommandName="CodeSave" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricCodeID") %>'
                                                                                                                            Visible="false" />
                                                                                                                        <asp:ImageButton ID="imgcancel" runat="server" ImageUrl="/admin/Images/isInactive.png"
                                                                                                                            CommandName="CodeExit" ToolTip="Cancel Fabric Code" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricCodeID") %>'
                                                                                                                            Visible="false" />
                                                                                                                        <asp:ImageButton ID="imgEdit" ToolTip="Edit Fabric Code" runat="server" ImageUrl="/App_Themes/Gray/Images/Edit.gif"
                                                                                                                            CommandName="CodeEdit" OnClientClick="chkHeight();" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricCodeID") %>' />
                                                                                                                        &nbsp;<asp:ImageButton ID="imgDelete" ToolTip="Remove Fabric Code" OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){chkHeight(); return true;}else{return false;}"
                                                                                                                            runat="server" ImageUrl="/App_Themes/Gray/Images/delete-icon.png" CommandName="CodeRemove"
                                                                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricCodeID") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle HorizontalAlign="right" Width="30%" />
                                                                                                                    <ItemStyle HorizontalAlign="right" Width="30%" />
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:LinkButton ID="lnkAdd" runat="server" CommandName="CodeAdd" CssClass="buttons"
                                                                                                                            ToolTip="Add New Fabric Code" Text="<strong><span style=''>Add</span></strong>"
                                                                                                                            Style="float: none; margin-right: 10px;"></asp:LinkButton>
                                                                                                                    </FooterTemplate>
                                                                                                                    <FooterStyle HorizontalAlign="right" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField Visible="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <tr>
                                                                                                                            <td colspan="9" style="background-color: #d7d7d7; line-height: 18px; border: solid 1px #d8d8d8">
                                                                                                                                <b>Fabric Width</b>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td colspan="100%" align="center">
                                                                                                                                <div id="divchildname" style="margin: 2px 0 0 0; position: relative; left: 15px; overflow: auto; width: 97%; text-align: center;">
                                                                                                                                    <asp:GridView ID="grdFabricWidth" runat="server" GridLines="None" AutoGenerateColumns="false"
                                                                                                                                        Width="98%" CssClass="checklist-main border-right-all" AllowPaging="false" ShowHeader="True"
                                                                                                                                        BorderColor="#d7d7d7" BorderWidth="1px" BorderStyle="Solid" ShowFooter="true"
                                                                                                                                        DataKeyNames="FabricWidthID" OnRowDataBound="grdFabricWidth_RowDataBound" OnRowCommand="grdFabricWidth_RowCommand"
                                                                                                                                        OnRowDeleting="grdFabricWidth_RowDeleting" OnRowEditing="grdFabricWidth_RowEditing">
                                                                                                                                        <Columns>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Width">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltrFabricWidth" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Width") %>'></asp:Literal>
                                                                                                                                                    <asp:TextBox ID="txtFabricWidth" Style="width: 300px;" runat="server" Visible="false"
                                                                                                                                                        CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"Width") %>'></asp:TextBox>
                                                                                                                                                    <input type="hidden" id="hdnFabricWidthID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricWidthID") %>' />
                                                                                                                                                    <input type="hidden" id="hdnFabricCodeID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricCodeID") %>' />
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle />
                                                                                                                                                <ItemStyle HorizontalAlign="left" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtFooterFabricWidth" CssClass="order-textfield" Width="300px" Text=""
                                                                                                                                                        runat="server"></asp:TextBox>
                                                                                                                                                    <input type="hidden" id="hdnFooterFabricCodeID" runat="server" value='<%# DataBinder.Eval(Container.DataItem,"FabricCodeID") %>' />
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="Left" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Active">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <img id="imgCodeActive" runat="server" src='<%# SetImage(Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"Active"))) %>'
                                                                                                                                                        alt="" />
                                                                                                                                                    <asp:CheckBox ID="chkActive" runat="server" Visible="false" Checked='<%#Convert.ToBoolean(Eval("Active")) %>' />
                                                                                                                                                  
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="10%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="10%" />
                                                                                                                                                <ItemStyle HorizontalAlign="center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:CheckBox ID="chkFooterActive" runat="server" />
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Display Order">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:Literal ID="ltdisplayorder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:Literal>
                                                                                                                                                    <asp:TextBox ID="txtdisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        MaxLength="6" Style="width: 120px; text-align: center;" runat="server" Visible="false"
                                                                                                                                                        CssClass="order-textfield" Text='<%# DataBinder.Eval(Container.DataItem,"DisplayOrder") %>'></asp:TextBox>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle Width="15%" HorizontalAlign="center" VerticalAlign="Middle" CssClass="table-title" />
                                                                                                                                                <ItemStyle Width="15%" />
                                                                                                                                                <ItemStyle HorizontalAlign="center" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:TextBox ID="txtDisplayorder" onkeypress="return keyRestrict(event,'0123456789');"
                                                                                                                                                        MaxLength="6" CssClass="order-textfield" Width="120px" Text="" Style="width: 120px; text-align: center;"
                                                                                                                                                        runat="server"></asp:TextBox>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:TemplateField HeaderText="&nbsp;Action">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:ImageButton ID="imgSave" ToolTip="Save Fabric Weight" runat="server" ImageUrl="/App_Themes/Gray/Images/save.png"
                                                                                                                                                        CommandName="widthsave" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricWidthID") %>'
                                                                                                                                                        Visible="false" />
                                                                                                                                                    <asp:ImageButton ID="imgcancel" runat="server" ImageUrl="/admin/Images/isInactive.png"
                                                                                                                                                        CommandName="WidthExit" ToolTip="Cancel Fabric Weight" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricWidthID") %>'
                                                                                                                                                        Visible="false" />
                                                                                                                                                    <asp:ImageButton ID="imgEdit" ToolTip="Edit Fabric Weight" runat="server" ImageUrl="/App_Themes/Gray/Images/Edit.gif"
                                                                                                                                                        CommandName="WidthEdit" OnClientClick="chkHeight();" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricWidthID") %>' />
                                                                                                                                                    &nbsp;<asp:ImageButton ID="imgDelete" ToolTip="Remove Fabric Weight" OnClientClick="javascript:if(confirm('Are you sure want to delete this record?')){chkHeight(); return true;}else{return false;}"
                                                                                                                                                        runat="server" ImageUrl="/App_Themes/Gray/Images/delete-icon.png" CommandName="RemoveWidth"
                                                                                                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"FabricWidthID") %>' />
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                                                                                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                                                                                <FooterTemplate>
                                                                                                                                                    <asp:LinkButton ID="lnkAdd" runat="server" CommandName="AddWidth" CssClass="buttons"
                                                                                                                                                        Text="<strong><spa>Add</span></strong>" ToolTip="Add New Fabric Weight" Style="float: none;"></asp:LinkButton>
                                                                                                                                                </FooterTemplate>
                                                                                                                                                <FooterStyle HorizontalAlign="center" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                        </Columns>
                                                                                                                                        <AlternatingRowStyle HorizontalAlign="left" />
                                                                                                                                    </asp:GridView>
                                                                                                                                </div>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                            <AlternatingRowStyle CssClass="altrowmainsub" />
                                                                                                            <RowStyle CssClass="altrowmainsub" />
                                                                                                            <FooterStyle CssClass="altrowmainsub" />
                                                                                                        </asp:GridView>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <AlternatingRowStyle CssClass="altrowmain" HorizontalAlign="left" />
                                                                                <RowStyle CssClass="altrowmain" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" id="trsavevendor" runat="server" visible="false">
                                                        <td align="center">
                                                            <div id="divfloating" class="divfloatingcss" style="width: 300px;">
                                                                <div style="margin-bottom: 1px; margin-top: 3px;">
                                                                    <asp:ImageButton ID="btnSavevendor" runat="server" OnClick="btnSavevendor_Click" />
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <span style="padding-left: 20px"></span>
                                                            <input type="hidden" value="0" id="hdnVariantId" runat="server" />
                                                            <input type="hidden" value="0" id="hdnVariantIdGrid" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
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
                    <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                </td>
            </tr>
            <tr>
                <td>
                    <div style="display: none;">
                        <input type="hidden" id="hdnrelatedsku1" runat="server" value='' />
                        <input type="hidden" id="hdnrelatedsku" runat="server" value='' />
                    </div>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="20%" style="margin-top: 25%;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
