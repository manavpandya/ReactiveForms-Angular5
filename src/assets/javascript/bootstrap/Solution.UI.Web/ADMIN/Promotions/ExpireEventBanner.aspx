<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ExpireEventBanner.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Promotions.ExpireEventBanner" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
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
            $j('#ContentPlaceHolder1_txtEndDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        }
         )
    </script>
<script src="/admin/js/jquery-1.4.1.min.js"></script>
     <script type="text/javascript" language="javascript">
         function openCenteredCrossSaleWindow1(mode) {

            <%-- if (document.getElementById('<%=ddlStore.ClientID %>').value != "0") {--%>
                //var ids;
                var couponid = document.getElementById('<%=hdnevent.ClientID %>').value;
                var width = 700;
                var height = 500;
                if (mode == 'product') {
                    width = 850;
                    height = 500;
                }

                var left = parseInt((screen.availWidth / 2) - (width / 2));
                var top = parseInt((screen.availHeight / 2) - (height / 2));
                var pis = document.getElementById('<%=txtvalidforprod.ClientID %>').value;
           
             var StoreID = 1;
             document.getElementById('idsset').src = 'Setsession.aspx?Ids=' + pis;
             
                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                window.open('EventPopup.aspx?StoreID=' + StoreID + '&mode=' + mode + '&eventid=' + couponid, "Mywindow", windowFeatures);
                return false;
               // Session["pids"] = document.getElementById('<%=txtvalidforprod.ClientID %>').value;
               
            <%--}
            else {
                jAlert('Please select Store.', 'Message', '<%=ddlStore.ClientID %>');
                return false;
            }--%>
        }
    </script>
    <script type="text/javascript">
        function MutExChkList(chk) {
            var chkList = chk.parentNode.parentNode.parentNode;
            var chks = chkList.getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i] != chk && chk.checked) {
                    chks[i].checked = false;
                }
            }
        }
        function validation() {

            if (document.getElementById("ContentPlaceHolder1_txtEventName") != null && document.getElementById("ContentPlaceHolder1_txtEventName").value.replace(/^\s+|\s+$/g, '') == '') {

                jAlert('Please Enter Event Name.', 'Message');
                document.getElementById("ContentPlaceHolder1_txtEventName").focus();
                //   $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtEventName').offset().top - 250 }, 'slow');
                return false;
            }

            if (document.getElementById("ContentPlaceHolder1_txtStartDate") != null && document.getElementById("ContentPlaceHolder1_txtStartDate").value.replace(/^\s+|\s+$/g, '') == '') {

                jAlert('Please Enter StartDate.', 'Message');
                document.getElementById("ContentPlaceHolder1_txtStartDate").focus();
                //  $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtStartDate').offset().top - 250 }, 'slow');
                return false;
            }

            if (document.getElementById("ContentPlaceHolder1_txtEndDate") != null && document.getElementById("ContentPlaceHolder1_txtEndDate").value.replace(/^\s+|\s+$/g, '') == '') {

                jAlert('Please Enter EndDate.', 'Message');
                document.getElementById("ContentPlaceHolder1_txtEndDate").focus();
                //  $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtEndDate').offset().top - 250 }, 'slow');
                return false;
            }
            var startDate = new Date(document.getElementById('ContentPlaceHolder1_txtStartDate').value);
            var endDate = new Date(document.getElementById('ContentPlaceHolder1_txtEndDate').value);
            if (startDate > endDate) {

                jAlert('Please Select Valid Date.', 'Message');
                document.getElementById("ContentPlaceHolder1_txtEndDate").focus();
                return false;
            }

            return true;
        }


        function Searchvalidation() {
            if (document.getElementById("ContentPlaceHolder1_txtsearch") != null && document.getElementById("ContentPlaceHolder1_txtsearch").value.replace(/^\s+|\s+$/g, '') == '') {

                jAlert('Please Enter Search Value.', 'Message');
                document.getElementById("ContentPlaceHolder1_txtsearch").focus();
                //   $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtEventName').offset().top - 250 }, 'slow');
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
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">

                                                    <h2>
                                                        <asp:Label ID="lblTitle" runat="server" Text="Add New Event Banner"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td class="border-td-sub" width="50%" valign="top">
                                                <fieldset class="fldset" style="width: 98%;">
                                                    <table align="left" class="add-product" border="0" cellpadding="0" cellspacing="0"
                                                        width="100%">
                                                        <tr>
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">*</span> Event Name :
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtEventName" runat="server"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnevent" runat="server" Value="0" />
                                                            </td>

                                                        </tr>
 <tr>
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> URL :
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtUrlname" runat="server"></asp:TextBox>
                                                              
                                                            </td>

                                                        </tr>
                                                        <tr class="even-row">

                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">*</span> Start Date :
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtStartDate" runat="server"
                                                                    Style="margin-right: 3px;"></asp:TextBox>
                                                            </td>

                                                        </tr>
                                                        <tr class="even-row">

                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">*</span> End Date :
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtEndDate" runat="server"
                                                                    Style="margin-right: 3px;"></asp:TextBox>
                                                            </td>

                                                        </tr>
                                                        <tr class="even-row">
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Coupon Code :
                                                            </td>

                                                            <td align="left" valign="top">
<div style="height:200px;overflow-y:auto;float:left;width:20%;border:solid 1px #d7d7d7;">
                                                                <asp:CheckBoxList ID="chkCouponCode" runat="server" Width="100%"></asp:CheckBoxList>
</div>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Banner Title :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="TxtbannerTitle" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr >
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Banner URL :
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="TxtBannerURL" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Target :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlTarget" runat="server" CssClass="order-list">
                                                                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                                    <asp:ListItem Value="_self">Self</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr class="even-row">
                                                            <td align="left" class="font-black01" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Event Banner :
                                                            </td>
                                                            <td align="left">

                                                                <asp:FileUpload ID="FileUploadExpireEvent" runat="server" />

                                                                <div>
                                                                    <br />
                                                                    <img id="imgBanner" runat="server" style="height: 100%" />
 <br /> <br />
                                                                      <asp:ImageButton ID="btndelete" runat="server" Visible="false" OnClick="btndelete_Click" OnClientClick="javascript:if(confirm('Are you sure want to delete?')){return true;}else{return false;}" />
                                                                </div>
                                                            </td>

                                                        </tr>
 <tr class="oddrow">
                                                            <td align="left" class="font-black01" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Event Logo :
                                                            </td>
                                                            <td align="left">

                                                                <asp:FileUpload ID="fileuploadlogo" runat="server" /><br><span style="color:red;">(Size Should be <= 280(w) X 100(h) )</span>

                                                                <div>
                                                                    <br />
                                                                    <img id="imglogo" runat="server" style="height: 100%" />
                                                                    <br />
                                                                    <br />
                                                                    <asp:ImageButton ID="btndeletlogo" runat="server" Visible="false" OnClick="btndeletlogo_Click" OnClientClick="javascript:if(confirm('Are you sure want to delete?')){return true;}else{return false;}" />

                                                                </div>
                                                            </td>

                                                        </tr>

                                                        <tr>
                                                            <td align="left" class="font-black01" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Logo Position :
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlposition" runat="server" CssClass="order-list">
                                                                    <asp:ListItem Value="topleft">Top-Left</asp:ListItem>
                                                                    <asp:ListItem Value="topcenter">Top-Center</asp:ListItem>
                                                                    <asp:ListItem Value="topright">Top-Right</asp:ListItem>
                                                                    <asp:ListItem Value="bottomleft">Bottom-Left</asp:ListItem>
                                                                    <asp:ListItem Value="bottomcenter">Bottom-Center</asp:ListItem>
                                                                    <asp:ListItem Value="bottomright">Bottom-Right</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                       <tr class="oddrow">
                                                    <td>
                                                        <span class="star">&nbsp;&nbsp;</span>Valid For Product(s) :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtvalidforprod" CssClass="order-textfield" TextMode="MultiLine"
                                                            Width="224px" Height="100px"></asp:TextBox>&nbsp;
                                                        <asp:CheckBox ID="chkvalidforprod" Visible="false" runat="server" Text=" All Products  &nbsp;&nbsp;&nbsp;|  &nbsp;&nbsp;&nbsp;(Enter Comma seperated list of Product ID's)  &nbsp;&nbsp;&nbsp;|"
                                                            onchange="javascript:DisableBox(this,'ContentPlaceHolder1_txtvalidforprod');" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                                            ID="btnpro" runat="server" ToolTip="Add Product" OnClientClick="return openCenteredCrossSaleWindow1('product');"
                                                            CausesValidation="true" />
                                                    </td>
                                                </tr>
<tr class="even-row">
                                                            <td align="left">
                                                                <span style="color: Red;">&nbsp;&nbsp;</span> Title :
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtpagetitle" runat="server" CssClass="order-textfield"></asp:TextBox>

                                                            </td>

                                                        </tr>
                                                        <tr class="even-row">
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Description Position :
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:DropDownList ID="ddldescposition" runat="server" CssClass="order-list">
                                                                    <asp:ListItem Value="top">Top</asp:ListItem>
                                                                    <asp:ListItem Value="bottom">Bottom</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>


 
<tr >
                                                        <td valign="left">
                                                            <span class="star">&nbsp;&nbsp;</span>Description:
                                                        </td>
                                                        <td>
                                                            <CKEditor:CKEditorControl ID="SalesEventDescription" runat="server" BasePath="~/ckeditor/"
                                                                Width="800px" Height="300px"></CKEditor:CKEditorControl>
                                                        </td>
                                                    </tr>
<tr class="even-row">
                                                             <td colspan="2">
                                                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="border-td content-table">
                                                                <tbody>
                                                                    <tr>
                                                                        <th>
                                                                            <div class="main-title-left">
                                                                                <img class="img-left" title="SEO" alt="SEO" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                <h2>SEO</h2>
                                                                            </div>

                                                                        </th>
                                                                    </tr>

                                                                    <tr>
                                                                        <td id="tdSEO">
                                                                            <div id="tab-container" class="slidingDivSEO">
                                                                                <ul class="menu">
                                                                                    <li class="active" id="ordernotes1" onclick='$("#ordernotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("div.order-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.gift-notes").css("display", "none");'>Page Title</li>
                                                                                    <li id="privatenotes1" onclick='$("#ordernotes1").removeClass("active");$("#privatenotes1").addClass("active"); $("#giftnotes1").removeClass("active");$("div.private-notes").fadeIn();$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");'>Keywords</li>
                                                                                    <li id="giftnotes1" onclick='$("#giftnotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("div.gift-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");'>Description</li>

                                                                                </ul>
                                                                                <span class="clear"></span>
                                                                                <div class="tab-content-2 order-notes">
                                                                                    <div class="tab-content-3">
                                                                                        <asp:TextBox ID="txtSETitle" runat="server" CssClass="order-textfield" Width="100%"
                                                                                            Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="29"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tab-content-2 private-notes">
                                                                                    <div class="tab-content-3">
                                                                                        <asp:TextBox ID="txtSEKeyword" runat="server" CssClass="order-textfield" Width="100%"
                                                                                            Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="30"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tab-content-2 gift-notes">
                                                                                    <div class="tab-content-3">
                                                                                        <asp:TextBox ID="txtSEDescription" runat="server" CssClass="order-textfield" Width="100%"
                                                                                            Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="31"></asp:TextBox>
                                                                                    </div>
                                                                                </div>

                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        </tr>

 
                                                        <tr style="display:none;">
                                                            <td align="left" class="font-black01" valign="top" width="20%;">Search By Sku :
                                                            </td>
                                                            <td align="left" class="font-black01" valign="top">
                                                                <asp:TextBox ID="txtsearch" runat="server"></asp:TextBox>
                                                                <asp:Button ID="btnSearch" runat="server" OnClientClick="return Searchvalidation();" OnClick="btnSearch_Click" />
                                                                <asp:Button ID="btnShowAll" runat="server" OnClick="btnShowAll_Click" />
                                                            </td>

                                                        </tr>

                                                        <tr class="even-row" style="display:none;">
                                                            <td colspan="2">Product Selection :  
                                                
                                                            </td>

                                                        </tr>
                                                        <tr class="even-row" style="display:none;">
                                                            <td colspan="2">
                                                                <asp:GridView ID="grdProductDetail" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                                    AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                    ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="false"
                                                                    CellPadding="2"
                                                                    BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1"
                                                                    CellSpacing="1">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                Select
                                                                            </HeaderTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkselect" runat="server" />
                                                                                <asp:HiddenField ID="hdnproductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="30%">
                                                                            <HeaderTemplate>
                                                                                Product Name
                                                            
                                                                            </HeaderTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductName" runat="server" Text='<%# bind("Name") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="30%">
                                                                            <HeaderTemplate>
                                                                                SKU
                                                            
                                                                            </HeaderTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSKU" runat="server" Text='<%# bind("SKU") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="30%">
                                                                            <HeaderTemplate>
                                                                                UPC
                                                            
                                                                            </HeaderTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUPC" runat="server" Text='<%# bind("UPC") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                    </Columns>

                                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                    <AlternatingRowStyle CssClass="altrow" />
                                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>

                                                              
                                        <tr class="even-row" align="center">
                                            <td align="center">

                                                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return validation();" />
                                                   <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cencel" ToolTip="Cancel" OnClick="imgCancle_Click"
                                                                                    />
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
       <div style="display:none;">
           <iframe id="idsset"></iframe>
       </div>
    </div>

</asp:Content>
