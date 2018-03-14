<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="SearchProductType.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.SearchProductType" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function checkondelete(id) {
            jConfirm('Are you sure want to delete ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById(id).onclick = function () { }
                    document.getElementById(id).click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }
        function Checkfields() {
            if (document.getElementById('<%=ddlSearchType.ClientID %>').selectedIndex == 0) {
                jAlert('Please Select Search Type.', 'Message', '<%=ddlSearchType.ClientID %>');
                return false;
            }
            else if (document.getElementById('<%=txtSearchName.ClientID %>').value == '') {
                jAlert('Please Enter Search Name.', 'Message', '<%=txtSearchName.ClientID %>');
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

    function ChangeSizeLabel() {

        if (document.getElementById('<%=ddlSearchType.ClientID %>').selectedIndex == 1) {

            document.getElementById('<%=lblSize.ClientID %>').innerHTML = "Size should be 120 x 120";
        }
        else if (document.getElementById('<%=ddlSearchType.ClientID %>').selectedIndex == 2) {

            document.getElementById('<%=lblSize.ClientID %>').innerHTML = "Size should be 216 x 116";
            }
            else {

                document.getElementById('<%=lblSize.ClientID %>').innerHTML = "";
            }
    }


    </script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

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
                                                    <img class="img-left" title="Add Search Product Type" alt="Add Search Product Type"
                                                        src="/App_Themes/<%=Page.Theme %>/Images/state-list-icon.png" />
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Search Product Type" ID="lblTitle"></asp:Label></h2>
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
                                                    <tr>
                                                        <td colspan="2">
                                                            <center>
                                                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label></center>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Search Type:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:DropDownList ID="ddlSearchType" runat="server" Width="140px" onchange="ChangeSizeLabel();" CssClass="add-product-list">
                                                                <asp:ListItem Text="Select Type" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Color" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Pattern" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="Fabric" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="Style" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="Header" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="Custom Style" Value="6"></asp:ListItem>
                                                                <asp:ListItem Text="Options" Value="7"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">*</span>Search Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtSearchName" CssClass="order-textfield" MaxLength="300"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>&nbsp;&nbsp;Additional Price:
                                                        </td>
                                                        <td>$<asp:TextBox runat="server" ID="txtPrice" CssClass="order-textfield" Width="80px"
                                                            MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (12.00)
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>&nbsp;&nbsp;Per Inch:
                                                        </td>
                                                        <td>$<asp:TextBox runat="server" ID="txtPerInch" CssClass="order-textfield" Width="80px"
                                                            MaxLength="6" onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                                                            Ex (8.09)
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>&nbsp;&nbsp;Image:
                                                        </td>
                                                        <td>
                                                            <div id="divImage" class="slidingDivImage">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td width="10%">
                                                                                                    <asp:FileUpload ID="fuProductIcon" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                        TabIndex="25" />
                                                                                                </td>
                                                                                                <td valign="middle">
                                                                                                    <img alt="Upload" id="ImgLarge" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                        runat="server" style="margin-bottom: 5px; border: 1px solid darkgray" /><br />
                                                                                                </td>
                                                                                                <td width="15%">
                                                                                                    <asp:Label ID="lblSize" ForeColor="Red" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td width="5%">
                                                                                                    <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClick="btnUpload_Click"
                                                                                                        TabIndex="26" />
                                                                                                </td>
                                                                                                <td width="64%">
                                                                                                    <asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete" OnClientClick="return checkondelete('ContentPlaceHolder1_btnDelete');"
                                                                                                        OnClick="btnDelete_Click" />
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
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Active:
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkActive" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Display Order:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDisplayOrder" Width="80px" CssClass="order-textfield"
                                                                MaxLength="4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow" id="trindex" runat="server">
                                                        <td valign="top">&nbsp;&nbsp;Home Page Image:
                                                        </td>
                                                        <td>
                                                            <div id="divImage1" class="slidingDivImage" runat="server">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td width="10%" valign="top">
                                                                                                    <asp:FileUpload ID="FileUploadhomeImage" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                        TabIndex="25" />
                                                                                                </td>
                                                                                                <td valign="middle">
                                                                                                    <img alt="Upload" id="ImgIndexImage" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                        runat="server" style="margin-bottom: 5px; border: 1px solid darkgray" /><br />
                                                                                                </td>
                                                                                                <td width="15%" valign="middle">
                                                                                                    <asp:Label ID="lblhomeimage" ForeColor="Red" Text="Size should be 275x300" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td width="5%" valign="middle">
                                                                                                    <asp:ImageButton ID="btnUploadHomeImage" runat="server" AlternateText="Upload" OnClick="btnUploadHomeImage_Click"
                                                                                                        TabIndex="26" />
                                                                                                </td>
                                                                                                <td width="64%" valign="middle">
                                                                                                    <asp:ImageButton ID="btnDeleteHomeImage" runat="server" Visible="false" AlternateText="Delete" OnClientClick="return checkondelete('ContentPlaceHolder1_btnDeleteHomeImage');"
                                                                                                        OnClick="btnDeleteHomeImage_Click" />
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
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span class="star">&nbsp;&nbsp;</span>Title :
                                                        </td>
                                                        <td style="clear: left; float: left;">
                                                            <asp:TextBox ID="txtPageTitle" CssClass="order-textfield" runat="server"
                                                                Width="600px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 8%; padding-top: 10px" valign="top">
                                                            <span class="star">&nbsp;&nbsp;</span>Description :
                                                        </td>
                                                        <td style="width: 36%;">
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="ckeditor-table">
                                                                        <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtPageDescription"
                                                                            Rows="10" Columns="80" runat="server" MaxLength="500" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                        <script type="text/javascript">
                                                                            CKEDITOR.replace('<%= txtPageDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 150, width: 600 });
                                                                            CKEDITOR.on('dialogDefinition', function (ev) {
                                                                                if (ev.data.name == 'image') {
                                                                                    var btn = ev.data.definition.getContents('info').get('browse');
                                                                                    btn.hidden = false;
                                                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                }
                                                                                if (ev.data.name == 'link') {
                                                                                    var btn = ev.data.definition.getContents('info').get('browse');
                                                                                    btn.hidden = false;
                                                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                                }
                                                                            });
                                                                        </script>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>


                                                    <tr>

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
                                                    <tr class="altrow">
                                                        <td>&nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgSave_Click" OnClientClick="return Checkfields();" />
                                                            <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                OnClick="imgCancle_Click" />
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
</asp:Content>
