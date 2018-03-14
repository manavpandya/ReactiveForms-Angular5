<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Customer.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Customers.Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/App_Themes/<%=Page.Theme %>/js/CustomerValidation.js?5566" type="text/javascript"
        language="javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        function showhidesellereid(id)
        {
            
            if (document.getElementById('ContentPlaceHolder1_chknontaxable').checked == true)
            {
                $('#ContentPlaceHolder1_trsellerid').show();
            }
            else {
                $('#ContentPlaceHolder1_trsellerid').hide();
            }
        }
        function CheckFirstname()
        {
            if( $('#ContentPlaceHolder1_txtFirstName').val() != '')
            { return true;}
            else{  jAlert('Please Enter First Name!', 'Message');}                 
    
            return false;
    
        }
         
        function checkEmail()
        {
            if (document.getElementById('ContentPlaceHolder1_txtEmail').value == '') {
                jAlert('Please Enter EmailID.', 'Required Information', 'ContentPlaceHolder1_txtEmail');
                document.getElementById('ContentPlaceHolder1_txtEmail').focus();
                return false;
            }
        }
    
        $(function () {

            $('#ContentPlaceHolder1_txtFromDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
            $('#ContentPlaceHolder1_txtToDate').datetimepicker({
                showButtonPanel: true, ampm: false,
                showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: "button",
                buttonImage: "/App_Themes/<%=Page.Theme %>/images/date-icon.png", buttonImageOnly: true
            });
        });

        function openCenteredCrossSaleWindow(mode,clientid) 
        {
  
            //createCookie('prskus', document.getElementById(clientid).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<% = ddlStoreName.SelectedValue %>';
            var IDs= document.getElementById(clientid).value;
            if(IDs != '')
            {
                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                if(mode==1)
                    window.open('/Admin/Customers/PopUpProduct.aspx?&StoreID=' + StoreID + '&mode=' + mode +'&clientid=' +clientid + '&Ids=' +IDs, "Mywindow", windowFeatures);
                else if(mode==2)
                    window.open('/Admin/Customers/PopupCategory.aspx?&StoreID=' + StoreID + '&mode=' + mode +'&clientid=' +clientid + '&Ids=' +IDs, "Mywindow", windowFeatures);
            }
            else
            { var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                if(mode==1)
                    window.open('/Admin/Customers/PopUpProduct.aspx?&StoreID=' + StoreID + '&mode=' + mode +'&clientid=' +clientid, "Mywindow", windowFeatures);
                else if(mode==2)
                    window.open('/Admin/Customers/PopupCategory.aspx?&StoreID=' + StoreID + '&mode=' + mode +'&clientid=' +clientid , "Mywindow", windowFeatures);
            }

            return false;
        }
        function SetUploadDocVisible() {

            if (document.getElementById("ContentPlaceHolder1_chkUploadDoc") != null && document.getElementById("ContentPlaceHolder1_chkUploadDoc").checked == true) {
                {
                    document.getElementById("ContentPlaceHolder1_trUploadDoc").style.display = '';
                    //document.getElementById("ContentPlaceHolder1_trDelete").style.display = '';

                }
            }
            else {
                document.getElementById("ContentPlaceHolder1_trUploadDoc").style.display = 'none';
                //document.getElementById("ContentPlaceHolder1_trDelete").style.display = 'none';
            }
        }
        function SetCustHistoryVisible() {

            if (document.getElementById("ContentPlaceHolder1_chkShowCustHistory") != null && document.getElementById("ContentPlaceHolder1_chkShowCustHistory").checked == true) {
                {
                    document.getElementById("ContentPlaceHolder1_trCustHistory").style.display = '';
                    //document.getElementById("ContentPlaceHolder1_trDelete").style.display = '';

                }
            }
            else {
                document.getElementById("ContentPlaceHolder1_trCustHistory").style.display = 'none';
                //document.getElementById("ContentPlaceHolder1_trDelete").style.display = 'none';
            }
        }

        
        function ShowHideButton(imgid, divid, divid1) {
            if (document.getElementById(imgid) != null) {
                var src = document.getElementById(imgid).src;
                if (src.indexOf('expand.gif') > -1) {
                    document.getElementById(imgid).src = src.replace('expand.gif', 'minimize.png');
                    document.getElementById(imgid).title = 'Minimize';
                    document.getElementById(imgid).alt = 'Minimize';
                    document.getElementById(imgid).style.marginTop = "4px";
                    document.getElementById(imgid).className = 'minimize';
                    document.getElementById(divid).style.display = '';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = '';
                    }
                }
                else if (src.indexOf('minimize.png') > -1) {
                    document.getElementById(imgid).src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById(imgid).title = 'Show';
                    document.getElementById(imgid).style.marginTop = "0px";
                    document.getElementById(imgid).alt = 'Show';
                    document.getElementById(imgid).className = 'close';
                    document.getElementById(divid).style.display = 'none';
                    if (document.getElementById(divid1)) {
                        document.getElementById(divid1).style.display = 'none';
                    }
                }
            }
        }
     
        function ShowCustDetail(id) {
            //document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '600px';
            document.getElementById('frmdisplay1').width = '1000px';
            document.getElementById('frmdisplay1').scrolling = 'no';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 100px; padding: 0px;width:1000px;height:500px;");
            document.getElementById('popupContact1').style.width = '1000px';
            document.getElementById('popupContact1').style.height = '600px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id;
         
        
        }
                

        function Showaddress(id) {
            //document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '520px';
            document.getElementById('frmdisplay1').width = '1000px';
            document.getElementById('frmdisplay1').scrolling = 'no';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 100px; padding: 0px;width:1000px;height:520px;");
            document.getElementById('popupContact1').style.width = '1000px';
            document.getElementById('popupContact1').style.height = '520px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src = id +"&CustID="+ <%=CustomerID%> +"";        
        
        }
  
        function Editaddress(id)
        {
        
            //document.getElementById('header').style.zIndex = -1;
            document.getElementById("frmdisplay1").contentWindow.document.body.innerHTML = '';
            document.getElementById('frmdisplay1').height = '520px';
            document.getElementById('frmdisplay1').width = '1000px';
            document.getElementById('frmdisplay1').scrolling = 'no';
            document.getElementById('popupContact1').setAttribute("style", "z-index: 1000001; top: 100px; padding: 0px;width:1000px;height:520px;");
            document.getElementById('popupContact1').style.width = '1000px';
            document.getElementById('popupContact1').style.height = '520px';
            window.scrollTo(0, 0);
            document.getElementById('btnhelpdescri').click();
            document.getElementById('frmdisplay1').src ="/Admin/customers/Addnewaddress.aspx?" +id +"&CustID="+ <%=CustomerID%> +"";       
        
        }

        function SetDeleteaddressid(id,name)
        {


            jConfirm('Are you sure you want to delete this '+name+' Address?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById('ContentPlaceHolder1_hdndeleteaddresid').value = id;              
                    document.getElementById('ContentPlaceHolder1_btndeleteaddress').click();
                    return true;
                }
                else {

                    return false;
                }
            });  
            
            return false;
        }
    </script>
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
    </script>
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
                                                    <img class="img-left" title="Customer" alt="Customer" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                    <h2>
                                                        <asp:Label ID="lblHeader" runat="server" Text="Customer"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="right">
                                                <span class="star">*</span>Required Field
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #E3E3E3; font-weight: bold;">Security Information
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" style="padding-left: 10px;" cellpadding="0" cellspacing="0">
                                                    <tr class="oddrow">
                                                        <td style="width: 12%">
                                                            <span class="star">*</span>Store Name :
                                                        </td>
                                                        <td style="width: 80%; vertical-align: top">
                                                            <asp:DropDownList runat="server" ID="ddlStoreName" CssClass="add-product-list" Width="225px"
                                                                Height="20px" AutoPostBack="True" OnSelectedIndexChanged="ddlStoreName_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<font id="divCustDetail" runat="server"
                                                                visible="false">
                                                                <a href="javascript:void(0);" onclick='ShowCustDetail("/ADMIN/Customers/ViewCustomerDetail.aspx?CustID=<%=CustomerID%>");'>View Customer Data</a>
                                                            </font>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Email :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtEmail" CssClass="order-textfield" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                     <tr class="altrow">
                                                        <td>
                                                            <span class="star"></span>Alternate Email :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtAltEmail" CssClass="order-textfield" MaxLength="50"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Password :
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblpassword" runat="server" Text="Password is not generated"></asp:Label>
                                                            &nbsp;&nbsp;
                                                            <asp:LinkButton ID="lnkpassword" runat="server" Text="Reset Password" OnClick="lnkpassword_Click"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ImgSendMail" runat="server" AlternateText="Send Mail" ToolTip="Send Mail"
                                                                OnClick="ImgSendMail_Click" OnClientClick="return checkEmail();" />


                                                        </td>
                                                    </tr>
                                                    <tr id="trBlockIP" runat="server" style="display: none">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span> Block IP-Address :
                                                        </td>
                                                        <td valign="top" style="vertical-align: top;">
                                                            <asp:Label ID="lblIPAddress" runat="server" Style="vertical-align: top"></asp:Label>
                                                            &nbsp;
                                                            <asp:ImageButton ID="btnBlockIP" runat="server" AlternateText="Block IP-Address"
                                                                ToolTip="Block IP-Address" OnClick="btnBlockIP_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td valign="top">
                                                            <span class="star">*</span>First Name :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtFirstName" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">*</span>Last Name :
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtLastName" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" style="display: none;">
                                                        <td>
                                                            <span class="star">&nbsp;</span>Customer Level :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCustomerLevel" Width="225px" runat="server" CssClass="add-product-list">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;</span>No DisCount :
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkNoDiscount" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                         <td>
                                                            <span class="star">&nbsp;</span>Trade Template :
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddltradetemplate" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddltradetemplate_SelectedIndexChanged"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%" class="add-product" id="tblDiscount" runat="server">
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                                                                            <tr>
                                                                                <th colspan="6">
                                                                                    <div class="main-title-left">
                                                                                        <img class="img-left" title="Category Wise Discount Details" alt="Category Wise Discount Details"
                                                                                            src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                                                        <h2>Category Wise Discount Details</h2>
                                                                                    </div>
                                                                                    <div class="main-title-right">
                                                                                        <a href="javascript:void(0);" class="show_hideCateInfo" onclick="return ShowHideButton('ImgCate','tdCate','divcatdiscount');">
                                                                                            <img id="ImgCate" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                                                    </div>
                                                                                </th>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" id="tdCate" colspan="6">
                                                                                    <div id="divcatdiscount" class="slidingDivCateInfo">
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div style="overflow-y: auto;">
                                                                                                        <asp:GridView ID="grdCategory" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                                                                            EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                                            ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                                                                                            CellPadding="2" CellSpacing="1" PageSize="10" OnRowCommand="grdCategory_RowCommand"
                                                                                                            OnPageIndexChanging="grdCategory_PageIndexChanging" OnRowDeleting="grdCategory_RowDeleting">
                                                                                                            <EmptyDataTemplate>
                                                                                                                <center>
                                                                                                                    <span style="color: Red;">No Record(s) Found !</span></center>
                                                                                                            </EmptyDataTemplate>
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField Visible="False">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblMembershipDiscountID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'></asp:Label>
                                                                                                                        <asp:Label ID="lblCategoryID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CategoryId") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <HeaderTemplate>
                                                                                                                        Name
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblProName" runat="server" Text='<%# Bind("Productname") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <HeaderTemplate>
                                                                                                                        Discount
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDiscountPercent" runat="server" Text='<%# Bind("CategoryDiscount","{0:F2}") %>'></asp:Label>
                                                                                                                        <asp:TextBox ID="txtDiscountPercent" Style="text-align: center;" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                            runat="server" CssClass="order-textfield" Visible="false" Width="80px" MaxLength="6"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Edit Discount Percent">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton CommandName="Edit Percent" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'
                                                                                                                            ID="btnEdit" runat="server" CssClass="link-font" ImageUrl="~/Admin/images/file-icon.gif" />
                                                                                                                        <asp:ImageButton ID="btnSave" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CategoryId") %>'
                                                                                                                            CommandName="Add" Visible="False" ImageUrl="~/Admin/images/save.jpg" ToolTip="Save" />
                                                                                                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Stop" ImageUrl="~/Admin/images/cancel.jpg"
                                                                                                                            Visible="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'
                                                                                                                            ToolTip="Cancel" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Delete">
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton CommandName="Delete" ID="btnDelete" runat="server" CssClass="link-font"
                                                                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"CategoryId") %>' ImageUrl="~/Admin/images/delete-icon.gif"
                                                                                                                            OnClientClick="javascript:if(confirm('Are you sure you want to delete this Record?')){ return true;} else { return false;};" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                            <PagerSettings Mode="NumericFirstLast" Position="Bottom"></PagerSettings>
                                                                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                                        </asp:GridView>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="right">
                                                                                                <td>
                                                                                                    <a id="a1" name="aRelated" onclick="openCenteredCrossSaleWindow(2,'ContentPlaceHolder1_hdnCatWiseDiscountids');"
                                                                                                        style="margin-right: 15px; font-weight: bold; cursor: pointer;" title="Select Category(s)">Select Category(s) </a>&nbsp;&nbsp;&nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                                                                            <tr>
                                                                                <th colspan="6">
                                                                                    <div class="main-title-left">
                                                                                        <img class="img-left" title="Product Wise Discount Details" alt="Product Wise Discount Details"
                                                                                            src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                                                        <h2>Product Wise Discount Details</h2>
                                                                                    </div>
                                                                                    <div class="main-title-right">
                                                                                        <a href="javascript:void(0);" class="show_hideProductInfo" onclick="return ShowHideButton('ImgDesc','tdDesc','divproddiscount');">
                                                                                            <img id="ImgDesc" class="minimize" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                                                                                    </div>
                                                                                </th>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" id="tdDesc" colspan="6">
                                                                                    <div id="div2" class="slidingDivProductInfo">
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <div id="divproddiscount" style="overflow-y: auto;">
                                                                                                        <asp:GridView ID="grdProduct" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                                                                            EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                                            ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                                                                                            CellPadding="2" CellSpacing="1" PageSize="10" OnRowDeleting="grdProduct_RowDeleting"
                                                                                                            OnRowCommand="grdProduct_RowCommand" OnPageIndexChanging="grdProduct_PageIndexChanging">
                                                                                                            <EmptyDataTemplate>
                                                                                                                <center>
                                                                                                                    <span style="color: Red;">No Record(s) Found !</span></center>
                                                                                                            </EmptyDataTemplate>
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField Visible="False">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblMembershipDiscountID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'></asp:Label>
                                                                                                                        <asp:Label ID="lblProductId" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ProductId") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <HeaderTemplate>
                                                                                                                        Name
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblProName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="SKU">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblProSku" runat="server" Text='<%# Bind("Sku") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField>
                                                                                                                    <HeaderTemplate>
                                                                                                                        Discount
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDiscountPercent" runat="server" Text='<%# Bind("ProductDiscount","{0:F2}") %>'></asp:Label>
                                                                                                                        <asp:TextBox ID="txtDiscountPercent" runat="server" Style="text-align: center;" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                                                                            CssClass="order-textfield" Visible="false" Width="80px" MaxLength="6"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Edit Discount Percent">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton CommandName="Edit Percent" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'
                                                                                                                            ID="btnEdit" runat="server" CssClass="link-font" ImageUrl="~/Admin/images/file-icon.gif" />
                                                                                                                        <asp:ImageButton ID="btnSave" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ProductId") %>'
                                                                                                                            CommandName="Add" Visible="False" ImageUrl="~/Admin/images/save.jpg" ToolTip="Save" />
                                                                                                                        <asp:ImageButton ID="btnCancel" runat="server" CommandName="Stop" ImageUrl="~/Admin/images/cancel.jpg"
                                                                                                                            Visible="False" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"MembershipDiscountID") %>'
                                                                                                                            ToolTip="Cancel" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Delete">
                                                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton CommandName="Delete" ID="btnDelete" runat="server" CssClass="link-font"
                                                                                                                            CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ProductId") %>' ImageUrl="~/Admin/images/delete-icon.gif"
                                                                                                                            OnClientClick="javascript:if(confirm('Are you sure you want to delete this Record?')){ return true;} else { return false;};" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                            <PagerSettings Mode="NumericFirstLast" Position="Bottom"></PagerSettings>
                                                                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                                            <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                                        </asp:GridView>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr align="right">
                                                                                                <td>
                                                                                                    <a id="aRelated" title="Select Product(s)" name="aRelated" onclick="openCenteredCrossSaleWindow(1,'ContentPlaceHolder1_hdnProdWiseDiscountids');"
                                                                                                        style="margin-right: 15px; font-weight: bold; cursor: pointer;">Select Product(s)
                                                                                                    </a>&nbsp;&nbsp;&nbsp;
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
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span>Active :
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkActive" runat="server" CssClass="" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span>Is non taxable :
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chknontaxable" runat="server" CssClass="" onchange="showhidesellereid(this);" />
                                                        </td>
                                                    </tr>
                                                     <tr class="oddrow" id="trsellerid" style="display:none;" runat="server">
                                                        <td valign="top">
                                                            <span class="star">&nbsp;</span>Seller ID :
                                                        </td>
                                                        <td>
                                                           <asp:TextBox runat="server" ID="txtsellerId" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;</span>Upload Document:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="chkUploadDoc" onclick="javascript:SetUploadDocVisible();" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow" id="trUploadDoc" runat="server" style="display: none">
                                                        <td></td>
                                                        <td valign="middle">
                                                            <span>
                                                                <asp:FileUpload ID="fuUplodDoc" runat="server" />
                                                                <asp:ImageButton Style="vertical-align: middle" ID="btnUpload" runat="server" AlternateText="Upload"
                                                                    OnClick="btnUpload_Click" />
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" id="trDelete" runat="server" style="display: none">
                                                        <td></td>
                                                        <td>
                                                            <asp:Label ID="lblFileDocname" runat="server" Style="padding-right: 5px;"></asp:Label>
                                                            <a target="_blank" href='' id="btnDownload" runat="server" title="Download Document"
                                                                visible="false">
                                                                <img src="../Images/download-doc.gif" style="vertical-align: middle; padding-right: 5px;" />
                                                            </a>
                                                            <asp:ImageButton Style="vertical-align: middle; display: none" ID="btnDelete" runat="server"
                                                                AlternateText="Delete" OnClick="btnDelete_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow" id="trcoupcode" runat="server">
                                                        <td width="100%" colspan="2">
                                                            <table width="60%">
                                                                <tr>
                                                                    <td width="10%">Coupon Code :
                                                                    </td>
                                                                    <td width="14%">
                                                                        <asp:TextBox ID="txtcouponcode" runat="server" class="order-textfield" Style="width: 120px;"></asp:TextBox>
                                                                    </td>
                                                                    <td width="9%">
                                                                        <asp:ImageButton ID="btnGenrate" runat="server" Text="Generate" OnClick="btnGenrate_Click"
                                                                            Visible="false" OnClientClick="return CheckFirstname();" />
                                                                    </td>
                                                                    <td width="12%" style="display: none;">Discount Percent :
                                                                    </td>
                                                                    <td width="5%" style="display: none;">
                                                                        <asp:TextBox ID="txtdiscountpercent" runat="server" class="order-textfield" Text="0.00" Style="width: 40px;"
                                                                            onkeypress="return keyRestrict(event,'0123456789.');" MaxLength="6"></asp:TextBox>
                                                                    </td>
                                                                    <td width="8%">From Date :
                                                                    </td>
                                                                    <td width="12%">
                                                                        <asp:TextBox ID="txtFromDate" runat="server" class="order-textfield" Style="width: 70px; margin-right: 5px;"></asp:TextBox>
                                                                    </td>
                                                                    <td width="6%">To Date :
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtToDate" runat="server" class="order-textfield" Style="width: 70px; margin-right: 5px;"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table cellpadding="0" cellspacing="0" width="100%" style="border: 1px solid #E3E3E3;">
                                                                            <tr>
                                                                                <td style="background-color: #E3E3E3; font-weight: bold;" colspan="3">Billing Information

                                                                                    <a id="abillinglink" visible="false" runat="server" style="float: right; text-decoration: underline;" href="javascript:void(0)" onclick='Showaddress("/Admin/customers/AddnewAddress.aspx?addtype=billingadd&type=new");'>Add New Billing Address</a>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top" colspan="2">
                                                                                    <asp:CheckBox ID="chkSameAsAbove" runat="server" CssClass="" onclick="SameAsOption();" />
                                                                                    <asp:Label ID="lblSameAsAbove" runat="server" AssociatedControlID="chkSameAsAbove"
                                                                                        Text="Same As Security Information"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>First Name :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillFirstname" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Last Name :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillLastname" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">&nbsp;</span>Company :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillingCompany" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Address Line 1 :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBilladdressLine1" MaxLength="500" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">&nbsp;</span>Address Line 2 :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillAddressLine2" MaxLength="500" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">&nbsp;</span>Apt/Suite # :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillsuite" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>City :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillCity" MaxLength="100" onkeypress="return isNumberKey(event)"
                                                                                        CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Country :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlBillcountry" runat="server" CssClass="add-product-list"
                                                                                        Width="225px" OnSelectedIndexChanged="ddlBillcountry_SelectedIndexChanged" AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>State/Province :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlBillstate" runat="server" CssClass="add-product-list" Width="225px"
                                                                                        onchange="MakeBillingOtherVisible();">
                                                                                    </asp:DropDownList>
                                                                                    <div id="DIVBillingOther" style="display: none; padding-top: 5px;">
                                                                                        <span class="star">*</span>If Others, Specify &nbsp;&nbsp; : &nbsp;&nbsp;
                                                                                        <asp:TextBox ID="txtBillingOtherState" MaxLength="30" onkeypress="return isNumberKey(event)"
                                                                                            runat="server" CssClass="order-textfield" Width="78px"></asp:TextBox>
                                                                                        <asp:Label ID="lblBRFVOther" runat="server" CssClass="required-red" Visible="False"></asp:Label>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Zip Code :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillZipCode" CssClass="order-textfield" MaxLength="10"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Phone :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillphone" MaxLength="20" onkeypress="return onKeyPressPhone(event);"
                                                                                        CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">&nbsp;</span>Fax :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillingFax" MaxLength="20" onkeypress="return onKeyPressPhone(event);"
                                                                                        CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>E-Mail Address :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtBillEmail" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <table cellpadding="0" cellspacing="0" width="100%" style="border: 1px solid #E3E3E3;">
                                                                            <tr>
                                                                                <td style="background-color: #E3E3E3; font-weight: bold;" colspan="3">Shipping Information

                                                                                        <a id="ashippinglink" visible="false" runat="server" style="float: right; text-decoration: underline;" href="javascript:void(0)"
                                                                                            onclick='Showaddress("/Admin/customers/AddnewAddress.aspx?addtype=shippingadd&type=new");'>Add New Shipping Address</a>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top" colspan="2">
                                                                                    <asp:CheckBox ID="chkSameAsBilling" runat="server" CssClass="" OnCheckedChanged="chkSameAsBilling_CheckedChanged"
                                                                                        AutoPostBack="true" />
                                                                                    <asp:Label ID="lblSameAsBilling" runat="server" AssociatedControlID="chkSameAsBilling"
                                                                                        Text="Same As Billing Information"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>First Name :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShipFirstname" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Last Name :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShipLastname" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">&nbsp;</span>Company :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShippingCompany" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Address Line 1 :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShipAddressLine1" MaxLength="500" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">&nbsp;</span>Address Line 2 :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtshipAddressLine2" MaxLength="500" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">&nbsp;</span>Apt/Suite # :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShipSuite" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>City :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShipCity" MaxLength="100" onkeypress="return isNumberKey(event)"
                                                                                        CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Country :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlShipCounry" runat="server" CssClass="add-product-list" Width="225px"
                                                                                        OnSelectedIndexChanged="ddlShipCounry_SelectedIndexChanged" AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>State/Province :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlShipState" runat="server" CssClass="add-product-list" Width="225px"
                                                                                        onchange="MakeShippingOtherVisible()">
                                                                                    </asp:DropDownList>
                                                                                    <div id="DIVShippingOther" style="display: none; padding-top: 5px;">
                                                                                        <span class="star">*</span>If Others, Specify &nbsp;&nbsp; : &nbsp;&nbsp;
                                                                                        <asp:TextBox ID="txtShippingOtherState" MaxLength="30" onkeypress="return isNumberKey(event)"
                                                                                            runat="server" CssClass="order-textfield" Width="78px"></asp:TextBox>
                                                                                        <asp:Label ID="lblSRFVOther" runat="server" CssClass="required-red" Visible="False"></asp:Label>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Zip Code :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShipZipCode" CssClass="order-textfield" MaxLength="10"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>Phone :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShipPhone" MaxLength="20" onkeypress="return onKeyPressPhone(event);"
                                                                                        CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="oddrow">
                                                                                <td valign="top">
                                                                                    <span class="star">&nbsp;</span>Fax :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShippingFax" MaxLength="20" onkeypress="return onKeyPressPhone(event);"
                                                                                        CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr class="altrow">
                                                                                <td valign="top">
                                                                                    <span class="star">*</span>E-Mail Address :
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox runat="server" ID="txtShipEmailAddress" MaxLength="100" CssClass="order-textfield"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>


                                                    <tr id="treditdeleteaddress" runat="server">
                                                        <td colspan="2">
                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table cellpadding="0" cellspacing="0" width="100%" style="border: 1px solid #E3E3E3;">
                                                                            <tr>
                                                                                <td style="background-color: #E3E3E3; font-weight: bold;">Manage Billing Address
                                                                                 

                                                                                </td>
                                                                            </tr>


                                                                            <asp:Literal ID="ltBilling" runat="server"></asp:Literal>

                                                                        </table>
                                                                    </td>



                                                                    <td valign="top">
                                                                        <table cellpadding="0" cellspacing="0" width="100%" style="border: 1px solid #E3E3E3;">
                                                                            <tr>
                                                                                <td style="background-color: #E3E3E3; font-weight: bold;">Manage Shipping Address</td>
                                                                            </tr>


                                                                            <asp:Literal ID="ltShipping" runat="server"></asp:Literal>

                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>

                                                    </tr>


                                                    <tr class="altrow">
                                                         
                                                        <td colspan="6" align="left">
                                                            <asp:CheckBox runat="server" Text="Show Customer History&nbsp;" TextAlign="Left" ID="chkShowCustHistory" onclick="javascript:SetCustHistoryVisible();" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trCustHistory" runat="server">
                                                        <td valign="top" colspan="6">
                                                            <div class="slidingDivProductInfo">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="content-table border-td">
                                                                    <tr>
                                                                        <td>
                                                                            <div style="overflow-y: auto;">
                                                                                <asp:GridView ID="grdcusthistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                                                    EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                                    ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                                                                    CellPadding="2" CellSpacing="1" PageSize="10" OnPageIndexChanging="grdcusthistory_PageIndexChanging">
                                                                                    <EmptyDataTemplate>
                                                                                        <center>
                                                                                            <span style="color: Red;">No Record(s) Found !</span></center>
                                                                                    </EmptyDataTemplate>
                                                                                    <Columns>

                                                                                        <asp:TemplateField>
                                                                                            <HeaderTemplate>
                                                                                                Order #
                                                                                            </HeaderTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                            <ItemTemplate>
                                                                                                <a href='/admin/Orders/Orders.aspx?id=<%# Eval("OrderNumber") %>'>
                                                                                                <asp:Label ID="lblorderNumber" runat="server" Text='<%# Bind("OrderNumber") %>'></asp:Label></a>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Order Date">
                                                                                            <ItemTemplate>
                                                                                                <%# String.Format("{0:MM/dd/yyyy}", Eval("OrderDate"))%>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Order Total">
                                                                                            <ItemTemplate>
                                                                                                <%# String.Format("${0:0.00}", Eval("OrderTotal"))%>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <HeaderStyle HorizontalAlign="Right" BorderWidth="0" />
                                                                                        </asp:TemplateField>
                                                                                         <asp:TemplateField HeaderText="Transaction Status">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblTransactionStatus" runat="server" Text='<%# Eval("TransactionStatus")%>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                            <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Order Status">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Eval("OrderStatus")%>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                            <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                                                                                        </asp:TemplateField>

                                                                                    </Columns>
                                                                                    <PagerSettings Mode="NumericFirstLast" Position="Bottom"></PagerSettings>
                                                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                    <AlternatingRowStyle CssClass="altrow" />
                                                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                                </div>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td align="center" colspan="2">
                                                            <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgSave_Click" OnClientClick="return ValidatePage();" />
                                                            <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                OnClick="imgCancle_Click" CausesValidation="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>


                        <div style="display: none;">
                            <asp:HiddenField ID="hdnCatWiseDiscountids" runat="server" />
                            <asp:HiddenField ID="hdnProdWiseDiscountids" runat="server" />
                            <asp:Button ID="btnCustDiscountDetailid" runat="server" OnClick="btnCustDiscountDetailid_Click" />
                            <asp:Button ID="btnProdDiscountDetailid" runat="server" OnClick="btnProdDiscountDetailid_Click" />
                            <input type="button" id="btnreadmore" />
                            <input type="button" id="btnhelpdescri" />
                            <asp:Button ID="btndeleteaddress" runat="server" Text="Button" OnClick="btnDeleteaddress_Click" />
                            <asp:Button ID="btnrefressaddress" runat="server" Text="Button" OnClick="btnrefreshaddress_Click" />


                            <input id="hdndeleteaddresid" type="hidden" runat="server" />
                            <asp:ImageButton ID="popupContactClose1" ImageUrl="/images/close.png" runat="server"
                                ToolTip="Close" OnClientClick="return false;"></asp:ImageButton>
                        </div>
        </div>
    </div>

    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div id="popupContact1" style="z-index: 1000001; width: 750px; height: 350px;">
        <table border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td align="left">
                    <iframe id="frmdisplay1" frameborder="0" height="350px" width="750" scrolling="auto"></iframe>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
