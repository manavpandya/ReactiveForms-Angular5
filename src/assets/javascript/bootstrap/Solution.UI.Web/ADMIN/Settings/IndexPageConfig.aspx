<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="IndexPageConfig.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.IndexPageConfig"
    Theme="Gray" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" language="javascript">

        function checkfabricbaner()
        {
            if(confirm('Are sure want to delete?'))
            {
                return true;
            }
            return false;
        }
        function Checkfabriccurtainsfields() {
            var fuData = document.getElementById('<%= FileUploadfabriccurtains.ClientID %>');

            var FileUploadPath = fuData.value;
            if (FileUploadPath == '' && document.getElementById('<%= imgfabriccurtainsname.ClientID %>').value == '') {
                jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadfabriccurtains');
                return false;
            }
            else {

                if (document.getElementById('<%= imgfabriccurtainsname.ClientID %>').value == '') {


                    var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                    if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                        return true; // Valid file type
                    }
                    else {
                        jAlert('Please Select valid Banner path.', 'Message', 'ContentPlaceHolder1_FileUploadfabriccurtains');
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
        }
        function CheckFreeswatchfields() {
            var fuData = document.getElementById('<%= FileUploadFreeSwatch.ClientID %>');
            var FileUploadPath = fuData.value;
            if (FileUploadPath == '') {
                jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadFreeSwatch');
                return false;
            }
            else {
                var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                    return true; // Valid file type
                }
                else {
                    jAlert('Please Select valid Banner File.', 'Message', 'ContentPlaceHolder1_FileUploadFreeSwatch');
                    return false;
                }
            }
        }


        function CheckCustomItemPagefields() {
            var fuData = document.getElementById('<%= FileUploadCustomItemPage.ClientID %>');
            var FileUploadPath = fuData.value;
            if (FileUploadPath == '') {
               // jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadCustomItemPage');
                return true;
            }
            else {
                var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                    return true; // Valid file type
                }
                else {
                    jAlert('Please Select valid Banner File.', 'Message', 'ContentPlaceHolder1_FileUploadCustomItemPage');
                    return false;
                }
            }
        }


        function CheckClearancefields() {
            var fuData = document.getElementById('<%= FileUploadClearanceBanner.ClientID %>');
            var FileUploadPath = fuData.value;
            if (FileUploadPath == '') {
              //  jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadClearanceBanner');
                return true;
            }
            else {
                var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                    return true; // Valid file type
                }
                else {
                    jAlert('Please Select valid Banner path.', 'Message', 'ContentPlaceHolder1_FileUploadClearanceBanner');
                    return false;
                }
            }
        }




        function CheckOnsalefields() {
            var fuData = document.getElementById('<%= fuonsale.ClientID %>');
            
            var FileUploadPath = fuData.value;
            if (FileUploadPath == '' && document.getElementById('<%= imgonsalename.ClientID %>').value=='') {
             //   jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_fuonsale');
                return true;
            }
            else {

                if (document.getElementById('<%= imgonsalename.ClientID %>').value == '') {


                    var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                    if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                        return true; // Valid file type
                    }
                    else {
                        jAlert('Please Select valid Banner path.', 'Message', 'ContentPlaceHolder1_fuonsale');
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        function Checkkidscollectionfields() {
            var fuData = document.getElementById('<%= FileUploadkidscollection.ClientID %>');

            var FileUploadPath = fuData.value;
            if (FileUploadPath == '' && document.getElementById('<%= imgkidscollectionname.ClientID %>').value == '') {
                jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadkidscollection');
                return false;
            }
            else {

                if (document.getElementById('<%= imgkidscollectionname.ClientID %>').value == '') {


                    var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                    if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                        return true; // Valid file type
                    }
                    else {
                        jAlert('Please Select valid Banner path.', 'Message', 'ContentPlaceHolder1_FileUploadkidscollection');
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
        }
        function Checkgrommetcurtainsfields() {
            var fuData = document.getElementById('<%= FileUploadgrommetcurtains.ClientID %>');

             var FileUploadPath = fuData.value;
             if (FileUploadPath == '' && document.getElementById('<%= imggrommetcurtainsname.ClientID %>').value == '') {
                 jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadgrommetcurtains');
                return false;
            }
            else {

                if (document.getElementById('<%= imggrommetcurtainsname.ClientID %>').value == '') {


                    var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                    if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                        return true; // Valid file type
                    }
                    else {
                        jAlert('Please Select valid Banner path.', 'Message', 'ContentPlaceHolder1_FileUploadgrommetcurtains');
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
        }

        function Checknewarrivalcurtainsfields() {
            var fuData = document.getElementById('<%= FileUploadnewarrivalcurtains.ClientID %>');

             var FileUploadPath = fuData.value;
             if (FileUploadPath == '' && document.getElementById('<%= imgnewarrivalcurtainsname.ClientID %>').value == '') {
               //  jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadnewarrivalcurtains');
                 return true;
             }
             else {

                 if (document.getElementById('<%= imgnewarrivalcurtainsname.ClientID %>').value == '') {


                     var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                     if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                         return true; // Valid file type
                     }
                     else {
                         jAlert('Please Select valid Banner path.', 'Message', 'ContentPlaceHolder1_FileUploadnewarrivalcurtains');
                         return false;
                     }
                 }
                 else {
                     return true;
                 }
             }
         }

        function CheckRomanCatfields() {
            var fuData = document.getElementById('<%= FileUploadRomanBanner.ClientID %>');
            var FileUploadPath = fuData.value;
            if (FileUploadPath == '') {
                jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadRomanBanner');
                return false;
            }
            else {
                var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                    return true; // Valid file type
                }
                else {
                    jAlert('Please Select valid Banner path.', 'Message', 'ContentPlaceHolder1_FileUploadRomanBanner');
                    return false;
                }
            }
        }
    </script>
    <script type="text/javascript">
        function Checkfields() {
            if (document.getElementById("ContentPlaceHolder1_FileUploadBanner").value == '' && (document.getElementById("ContentPlaceHolder1_imgBanner").src == '' || document.getElementById("ContentPlaceHolder1_imgBanner").src.toString().indexOf("image_not_available") > -1)) {

                jAlert('Please Select Banner Image.</br>( Size should be 279 x 260 )', 'Message', 'ContentPlaceHolder1_FileUploadBanner');
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
    </script>
    <script type="text/javascript">
        function checkondelete(message, id) {
            jConfirm(message, 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById("ContentPlaceHolder1_hdnDelete").value = id;
                    document.getElementById("ContentPlaceHolder1_btnhdnDelete").click();
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
        function checkondeleteFeaturedSystem(message, id) {
            jConfirm(message, 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById("ContentPlaceHolder1_hdnFeaturedProduct").value = id;
                    document.getElementById("ContentPlaceHolder1_btnhdnFeaturedProduct").click();
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
        function checkondeleteBestSeller(message, id) {
            jConfirm(message, 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById("ContentPlaceHolder1_hdnBestSeller").value = id;
                    document.getElementById("ContentPlaceHolder1_btnhdnBestSeller").click();
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
        function checkondeleteNewArrival(message, id) {
            jConfirm(message, 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById("ContentPlaceHolder1_hdnNewArrival").value = id;
                    document.getElementById("ContentPlaceHolder1_btnhdnNewArrival").click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }
        function Checkcustomcurtainsfields() {
            var fuData = document.getElementById('<%= FileUploadcustomcurtains.ClientID %>');

             var FileUploadPath = fuData.value;
             if (FileUploadPath == '' && document.getElementById('<%= imgcustomcurtainsname.ClientID %>').value == '') {
                jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadcustomcurtains');
                return false;
            }
            else {

                if (document.getElementById('<%= imgcustomcurtainsname.ClientID %>').value == '') {


                    var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                    if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
                        return true; // Valid file type
                    }
                    else {
                        jAlert('Please Select valid Banner path.', 'Message', 'ContentPlaceHolder1_FileUploadcustomcurtains');
                        return false;
                    }
                }
                else {
                    return true;
                }
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%; padding-top: 5px;">
                <table>
                    <tr>
                        <td>Store:
                            <asp:DropDownList ID="ddlStore" runat="server" CssClass="product-type" Height="21px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table class="content-table" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td class="border-td-sub">
                                    <table style="width: 100%" cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td>
                                                <div class="main-title-left">
                                                    <h2>Index Page Configuration</h2>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span3" style="font-size: 14px;">Roman Category Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span4">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="lblMsgRoman" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadRomanBanner" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imgRomanBanner" runat="server" visible="false" alt="" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgSaveRomanCat" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgSaveRomanCat_Click" OnClientClick="return CheckRomanCatfields();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span1" style="font-size: 14px;">Sale Clearance Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span2">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="lblMsgSale" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadClearanceBanner" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imgClearanceBanner" runat="server" visible="false" alt="" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr style="display:none;" >
                                                        <td valign="left">
                                                            <span class="star">&nbsp;&nbsp;</span>Description:
                                                        </td>
                                                        <td>
                                                            <CKEditor:CKEditorControl ID="SaleClearanceDescription" runat="server" BasePath="~/ckeditor/"
                                                                Width="800px" Height="300px"></CKEditor:CKEditorControl>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgSaveSaleClearance" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgSaveSaleClearance_Click" OnClientClick="return CheckClearancefields();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span7" style="font-size: 14px;">On Sale Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span8">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="lblonsale" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="fuonsale" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imgonsale" runat="server" visible="false" alt="" />
                                                                <input type="hidden" id="imgonsalename" runat="server" value="" />
                                                            </div>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                          <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span>Title :
                                                        </td>
                                                        <td align="left">
                                                           <asp:TextBox ID="txtonsaletitle" Text="" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                     <tr style="display:none;">
                                                        <td valign="left">
                                                            <span class="star">&nbsp;&nbsp;</span>Description:
                                                        </td>
                                                        <td>
                                                            <CKEditor:CKEditorControl ID="onsaledescription" runat="server" BasePath="~/ckeditor/"
                                                                Width="800px" Height="300px"></CKEditor:CKEditorControl>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgsaveonsale" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgsaveonsale_Click" OnClientClick="return CheckOnsalefields();" />
                                                               <asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete"
                                                                OnClick="btnDelete_Click" />

                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                         <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span7" style="font-size: 14px;">Kids Collection Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span8">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="lblkidscollectionmsg" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadkidscollection" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imgkidscollection" runat="server" visible="false" alt="" />
                                                                <input type="hidden" id="imgkidscollectionname" runat="server" value="" />
                                                            </div>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                          <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span>Title :
                                                        </td>
                                                        <td align="left">
                                                           <asp:TextBox ID="txtkidscollection" Text="" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgsavekidscollection" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgsavekidscollection_Click"  OnClientClick="return Checkkidscollectionfields();" />
                                                               <asp:ImageButton ID="btndeletekidscollection" runat="server" Visible="false" AlternateText="Delete"
                                                               OnClick="btndeletekidscollection_Click" />

                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span7" style="font-size: 14px;">Grommet Curtains Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span8">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="lblgrommetcurtainsmsg" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadgrommetcurtains" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imggrommetcurtains" runat="server" visible="false" alt="" />
                                                                <input type="hidden" id="imggrommetcurtainsname" runat="server" value="" />
                                                            </div>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                          <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span>Title :
                                                        </td>
                                                        <td align="left">
                                                           <asp:TextBox ID="txtgrommetcurtains" Text="" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgsavegrommetcurtains" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgsavegrommetcurtains_Click"  OnClientClick="return Checkgrommetcurtainsfields();" />
                                                               <asp:ImageButton ID="btndeletegrommetcurtains" runat="server" Visible="false" AlternateText="Delete"
                                                              OnClick="btndeletegrommetcurtains_Click" />

                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span7" style="font-size: 14px;">New Arrival Curtains Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span8">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="lblnewarrivalcurtainsmsg" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadnewarrivalcurtains" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imgnewarrivalcurtains" runat="server" visible="false" alt="" />
                                                                <input type="hidden" id="imgnewarrivalcurtainsname" runat="server" value="" />
                                                            </div>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                          <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span>Title :
                                                        </td>
                                                        <td align="left">
                                                           <asp:TextBox ID="txtnewarrivalcurtains" Text="" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                     <tr style="display:none;">
                                                        <td valign="left">
                                                            <span class="star">&nbsp;&nbsp;</span>Description:
                                                        </td>
                                                        <td>
                                                            <CKEditor:CKEditorControl ID="NewarrivalCurtainsDescription" runat="server" BasePath="~/ckeditor/"
                                                                Width="800px" Height="300px"></CKEditor:CKEditorControl>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgsavenewarrivalcurtains" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgsavenewarrivalcurtains_Click"  OnClientClick="return Checknewarrivalcurtainsfields();" />
                                                               <asp:ImageButton ID="btndeletenewarrivalcurtains" runat="server" Visible="false" AlternateText="Delete"
                                                             OnClick="btndeletenewarrivalcurtains_Click" />

                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span17" style="font-size: 14px;">Custom Curtains Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span18">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="lblcustomcurtainsmsg" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadcustomcurtains" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imgcustomcurtains" runat="server" visible="false" alt="" />
                                                                <input type="hidden" id="imgcustomcurtainsname" runat="server" value="" />
                                                            </div>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                          <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span>Title :
                                                        </td>
                                                        <td align="left">
                                                           <asp:TextBox ID="txtcustomcurtains" Text="" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgsavecustomcurtains" runat="server" AlternateText="Save" ToolTip="Save"
                                                               OnClick="imgsavecustomcurtains_Click"  OnClientClick="return Checkcustomcurtainsfields();" />
                                                               <asp:ImageButton ID="btndeletecustomcurtains" runat="server" Visible="false" AlternateText="Delete"
                                                              OnClick="btndeletecustomcurtains_Click" />

                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span1" style="font-size: 14px;">Free Swatch Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span2">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="lblmsgswatch" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadFreeSwatch" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imgFreeSwatch" runat="server" visible="false" alt="" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="ImgSaveFreeSwatch" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="ImgSaveFreeSwatch_Click" OnClientClick="return CheckFreeswatchfields();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span1" style="font-size: 14px;">Custom Item Page Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span2">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="LblCustomItemPage" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadCustomItemPage" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imgCustomItemPage" runat="server" visible="false" alt="" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                     <tr style="display:none;">
                                                        <td valign="left">
                                                            <span class="star">&nbsp;&nbsp;</span>Description:
                                                        </td>
                                                        <td>
                                                            <CKEditor:CKEditorControl ID="CustomItemPageDescription" runat="server" BasePath="~/ckeditor/"
                                                                Width="800px" Height="300px"></CKEditor:CKEditorControl>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="ImgSaveCustomItemPage" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="ImgSaveCustomItemPage_Click" OnClientClick="return CheckCustomItemPagefields();" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>


                                            <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7"
                                                    width="100%">
                                                    <tr class="altrow">
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="Span17" style="font-size: 14px;">Fabric Curtains Banner</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="Span18">is Required Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <asp:Label ID="lblfabriccurtainsmsg" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadfabriccurtains" runat="server" />
                                                            <div>
                                                                <br />
                                                                <img id="imgfabriccurtains" runat="server" visible="false" alt="" />
                                                                <input type="hidden" id="imgfabriccurtainsname" runat="server" value="" />
                                                            </div>
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                          <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span>Title :
                                                        </td>
                                                        <td align="left">
                                                           <asp:TextBox ID="txtfabriccurtains" Text="" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgsavefabriccurtains" runat="server" AlternateText="Save" ToolTip="Save"
                                                               OnClick="imgsavefabriccurtains_Click"  OnClientClick="return Checkfabriccurtainsfields();" />
                                                               <asp:ImageButton ID="btndeletefabriccurtains" runat="server" Visible="false" AlternateText="Delete"
                                                              OnClick="btndeletefabriccurtains_Click" OnClientClick="return checkfabricbaner();" />

                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <table align="left" class="table" border="0" cellpadding="1" cellspacing="1" style="border: 1px solid #e7e7e7; display: none;"
                                                    width="100%">
                                                    <tr class="altrow" style="display: none;">
                                                        <%-- <td style="width: 100px;">
                                                            <h3 style="height: 20px; width: 170px;">
                                                                <asp:Label runat="server" Text="DEAL OF DAY PRODUCT" ID="Label4"></asp:Label></h3>
                                                        </td>
                                                        <td align="right">
                                                            <span style="color: Red;">*</span><asp:Label ID="Label6" runat="server"> is Required Field</asp:Label>
                                                        </td>--%>
                                                        <td colspan="2">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <h3 style="height: 20px; width: 251px;">
                                                                            <span id="ContentPlaceHolder1_Label4" style="font-size: 14px;">DEAL OF DAY PRODUCT</span></h3>
                                                                    </td>
                                                                    <td align="right">
                                                                        <span style="color: Red;">*</span> <span id="ContentPlaceHolder1_Label6">is Required
                                                                            Field</span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="Center">
                                                            <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" style="width: 138px; height: 30px" valign="middle">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Product
                                                            Name :
                                                        </td>
                                                        <td align="left" style="height: 30px" valign="middle">
                                                            <asp:DropDownList ID="ddlProduct" runat="server" DataTextField="ProductName" DataValueField="ProductID"
                                                                Height="21px" Width="360px" CssClass="product-type">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" class="font-black01" style="width: 138px; height: 30px" valign="middle">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Product
                                                            Price :
                                                        </td>
                                                        <td align="left" style="height: 30px" valign="middle">
                                                            <asp:TextBox ID="txtHotdealprice" CssClass="order-textfield" Width="100px" onkeypress="return keyRestrict(event,'0123456789.');"
                                                                runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="font-black01" valign="top">
                                                            <span style="color: #ff0033"></span><span style="color: #ff0033">*</span> Banner
                                                            Image :
                                                        </td>
                                                        <td align="left">
                                                            <asp:FileUpload ID="FileUploadBanner" runat="server" />
                                                            <asp:Label ID="lblImgSize" runat="server" Text="Size should be 279 x 260"></asp:Label>
                                                            <div>
                                                                <br />
                                                                <img id="imgBanner" runat="server" visible="false" alt="" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td align="left" height="30" valign="top" style="width: 137px"></td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                OnClick="imgSave_Click" OnClientClick="return Checkfields();" />
                                                            <%-- <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                OnClick="imgCancle_Click" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="center" style="text-align: center; display: none;">
                                            <td align="center">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 80%">
                                                            <h3>
                                                                <asp:Label runat="server" Style="font-size: 14px;" Text="FEATURED CATEGORY" ID="lblTitle"></asp:Label></h3>
                                                        </td>
                                                        <td style="width: 20%" align="right">
                                                            <asp:ImageButton ID="ibtnFeaturecategory" runat="server" OnClientClick="openCenteredCrossSaleWindow1('category');" />
                                                            <div style="display: none;">
                                                                <asp:Button ID="btnFeatureCategory" runat="server" Text="bt" OnClick="btnFeatureCategory_Click" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 4px;">
                                            <td></td>
                                        </tr>
                                        <tr class="even-row" style="display: none;">
                                            <td>
                                                <table style="width: 100%" cellpadding="2" cellspacing="2">
                                                    <tr valign="middle">
                                                        <td style="width: 80%">
                                                            <asp:GridView ID="grdFeaturedcategory" GridLines="None" runat="server" ForeColor="White"
                                                                AutoGenerateColumns="False" RowStyle-ForeColor="Black" CellPadding="0" CellSpacing="1"
                                                                BorderColor="#e7e7e7" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                                BorderWidth="1" HeaderStyle-ForeColor="#3c2b1b" Width="100%" OnRowDataBound="grdFeaturedcategory_RowDataBound"
                                                                OnRowCancelingEdit="grdFeaturedcategory_RowCancelingEdit" OnRowEditing="grdFeaturedcategory_RowEditing"
                                                                OnRowUpdating="grdFeaturedcategory_RowUpdating">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Category Name" HeaderStyle-HorizontalAlign="left"
                                                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60%">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="2" cellspacing="1" border="0" align="left">
                                                                                <tr style="border: 0px;">
                                                                                    <td style="border: 0px; padding: 0px; background-color: transparent;">Category Name
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <%#Eval("Name") %>
                                                                            <asp:HiddenField ID="hdnCategoryid" runat="server" Value='<%#Eval("CategoryID") %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Parent Category Name" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPname" runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Display Order" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <%#Eval("DisplayOrder") %>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtDisplayorder" runat="server" Text='<%#Eval("DisplayOrder") %>'
                                                                                MaxLength="8" Width="50%" Style="text-align: center;" OnKeyPress="return isNumberKey(event);"></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Operations" ItemStyle-Width="10%">
                                                                        <EditItemTemplate>
                                                                            <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                                ImageUrl="~/App_Themes/Gray/button/edit.png"></asp:ImageButton>
                                                                            &nbsp;<asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                                ImageUrl="~/App_Themes/Gray/button/cancel.png"></asp:ImageButton>
                                                                        </EditItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="~/App_Themes/Gray/images/edit.gif"
                                                                                CausesValidation="False" CommandName="Edit" />
                                                                            <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/App_Themes/Gray/images/delete.gif"
                                                                                message='<%# Eval("CategoryID") %>' ToolTip="Delete" CommandName="DeleteFeaturedCategory"
                                                                                OnClientClick='return checkondelete("Are you sure want to delete selected Featured Category?", this.getAttribute("message"));'
                                                                                CommandArgument='<%# Eval("CategoryID") %>'></asp:ImageButton>
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
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                            <td></td>
                                        </tr>
                                        <tr align="center" style="text-align: center; display: none;">
                                            <td align="center">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 80%">
                                                            <h3>
                                                                <asp:Label runat="server" Style="font-size: 14px;" Text="FEATURED PRODUCT" ID="Label1"></asp:Label></h3>
                                                        </td>
                                                        <td style="width: 20%" align="right">
                                                            <asp:ImageButton ID="ibtnFeaturesystem" runat="server" OnClientClick="return openCenteredCrossSaleWindow1('feature');" />
                                                            <div style="display: none;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 4px; display: none;">
                                            <td>
                                                <input type="hidden" id="hdnTotFeaturecnt" runat="server" value="0" />
                                                <asp:GridView ID="grdFeaturedSystem" GridLines="None" runat="server" ForeColor="White"
                                                    AutoGenerateColumns="false" RowStyle-ForeColor="Black" CellPadding="0" CellSpacing="1"
                                                    BorderColor="#e7e7e7" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    BorderWidth="1" HeaderStyle-ForeColor="#3c2b1b" Width="100%" OnRowCancelingEdit="grdFeaturedSystem_RowCancelingEdit"
                                                    OnRowEditing="grdFeaturedSystem_RowEditing" OnRowUpdating="grdFeaturedSystem_RowUpdating">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Width="60%">
                                                            <HeaderTemplate>
                                                                <table cellpadding="2" cellspacing="1" border="0" align="left">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">Title
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#Eval("Name") %>
                                                                <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SKU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <%#Eval("SKU") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Display Order" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <%#Eval("DisplayOrder") %>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtDisplayorder" runat="server" OnKeyPress="return isNumberKey(event);"
                                                                    MaxLength="8" Text='<%#Eval("DisplayOrder") %>' Width="50%" Style="text-align: center"></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="10%">
                                                            <EditItemTemplate>
                                                                <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                    ImageUrl="~/App_Themes/Gray/button/edit.png"></asp:ImageButton>
                                                                &nbsp;<asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                    ImageUrl="~/App_Themes/Gray/button/cancel.png"></asp:ImageButton>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="~/App_Themes/Gray/images/edit.gif"
                                                                    CausesValidation="False" CommandName="Edit" />
                                                                <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/App_Themes/Gray/images/delete.gif"
                                                                    message='<%# Eval("ProductID") %>' ToolTip="Delete" OnClientClick='return checkondeleteFeaturedSystem("Are you sure want to delete selected Featured System?", this.getAttribute("message"));'
                                                                    CommandArgument='<%# Eval("ProductID") %>'></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                            <td></td>
                                        </tr>
                                        <tr align="center" style="text-align: center; display: none;">
                                            <td align="center">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 80%">
                                                            <h3>
                                                                <asp:Label runat="server" Style="font-size: 14px;" Text="BEST SELLER(s)" ID="Label2"></asp:Label></h3>
                                                        </td>
                                                        <td style="width: 20%" align="right">
                                                            <asp:ImageButton ID="ibtnBestseller" runat="server" OnClientClick="return openCenteredCrossSaleWindow1('best');" />
                                                            <div style="display: none;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 4px; display: none;">
                                            <td>
                                                <asp:GridView ID="grdBestSeller" GridLines="None" runat="server" ForeColor="White"
                                                    AutoGenerateColumns="false" RowStyle-ForeColor="Black" CellPadding="0" CellSpacing="1"
                                                    BorderColor="#e7e7e7" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    BorderWidth="1" HeaderStyle-ForeColor="#3c2b1b" Width="100%" OnRowCancelingEdit="grdBestSeller_RowCancelingEdit"
                                                    OnRowEditing="grdBestSeller_RowEditing" OnRowUpdating="grdBestSeller_RowUpdating">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Width="60%">
                                                            <HeaderTemplate>
                                                                <table cellpadding="2" cellspacing="1" border="0" align="center" width="100%">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">Title
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#Eval("Name") %>
                                                                <asp:HiddenField ID="hdnProductId" runat="server" Value='<%#Eval("ProductID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SKU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left"
                                                            ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <%#Eval("SKU") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Display Order" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <%#Eval("DisplayOrder") %>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtDisplayorder" runat="server" OnKeyPress="return isNumberKey(event);"
                                                                    MaxLength="8" Width="50%" Style="text-align: center;" Text='<%#Eval("DisplayOrder") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="10%">
                                                            <EditItemTemplate>
                                                                <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                    ImageUrl="~/App_Themes/Gray/button/edit.png"></asp:ImageButton>
                                                                &nbsp;<asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                    ImageUrl="~/App_Themes/Gray/button/cancel.png"></asp:ImageButton>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="~/App_Themes/Gray/images/edit.gif"
                                                                    CausesValidation="False" CommandName="Edit" />
                                                                <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/App_Themes/Gray/images/delete.gif"
                                                                    message='<%# Eval("ProductID") %>' ToolTip="Delete" CommandName="DeleteFeaturedCategory"
                                                                    OnClientClick='return checkondeleteBestSeller("Are you sure want to delete selected Best Seller(s)?", this.getAttribute("message"));'
                                                                    CommandArgument='<%# Eval("ProductID") %>'></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                            <td></td>
                                        </tr>
                                        <tr align="center" style="text-align: center; display: none;">
                                            <td align="center">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 80%">
                                                            <h3>
                                                                <asp:Label runat="server" Style="font-size: 14px;" Text="NEW ARRIVAL(s)" ID="Label3"></asp:Label></h3>
                                                        </td>
                                                        <td style="width: 20%" align="right">
                                                            <asp:ImageButton ID="ibtnnewarrival" runat="server" OnClientClick="return openCenteredCrossSaleWindow1('new');" />
                                                            <div style="display: none;">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 4px; display: none;">
                                            <td>
                                                <asp:GridView ID="grdNewarrival" GridLines="None" runat="server" ForeColor="White"
                                                    AutoGenerateColumns="false" RowStyle-ForeColor="Black" CellPadding="0" CellSpacing="1"
                                                    BorderColor="#e7e7e7" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                    BorderWidth="1" HeaderStyle-ForeColor="#3c2b1b" Width="100%" OnRowCancelingEdit="grdNewarrival_RowCancelingEdit"
                                                    OnRowEditing="grdNewarrival_RowEditing" OnRowUpdating="grdNewarrival_RowUpdating">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Width="60%">
                                                            <HeaderTemplate>
                                                                <table cellpadding="2" cellspacing="1" border="0" align="center" width="100%">
                                                                    <tr style="border: 0px;">
                                                                        <td style="border: 0px; padding: 0px; background-color: transparent;">Title
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#Eval("Name") %>
                                                                <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SKU" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <%#Eval("SKU") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Display Order" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <%#Eval("DisplayOrder") %>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtDisplayorder" OnKeyPress="return isNumberKey(event);" Width="50%"
                                                                    MaxLength="8" Style="text-align: center;" runat="server" Text='<%#Eval("DisplayOrder") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Operations" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ItemStyle-Width="10%">
                                                            <EditItemTemplate>
                                                                <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                    ImageUrl="~/App_Themes/Gray/button/edit.png"></asp:ImageButton>
                                                                &nbsp;<asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                    ImageUrl="~/App_Themes/Gray/button/cancel.png"></asp:ImageButton>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="~/App_Themes/Gray/images/edit.gif"
                                                                    CausesValidation="False" CommandName="Edit" />
                                                                <asp:ImageButton runat="server" ID="btnDelete" ImageUrl="~/App_Themes/Gray/images/delete.gif"
                                                                    message='<%# Eval("ProductID") %>' ToolTip="Delete" OnClientClick='return checkondeleteNewArrival("Are you sure want to delete selected New Arrival(s)?", this.getAttribute("message"));'
                                                                    CommandArgument='<%# Eval("ProductID") %>'></asp:ImageButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                </asp:GridView>
                                                <div style="display: none;">
                                                    <asp:Button ID="btnhdnDelete" runat="server" Text="Button" OnClick="btnhdnDelete_Click"
                                                        Style="display: ;" />
                                                    <asp:HiddenField ID="hdnDelete" runat="server" Value="0" />
                                                </div>
                                                <div style="display: none;">
                                                    <asp:Button ID="btnhdnFeaturedProduct" runat="server" Text="Button" OnClick="btnhdnFeaturedProduct_Click"
                                                        Style="display: ;" />
                                                    <asp:HiddenField ID="hdnFeaturedProduct" runat="server" Value="0" />
                                                </div>
                                                <div style="display: none;">
                                                    <asp:Button ID="btnhdnBestSeller" runat="server" Text="Button" OnClick="btnhdnBestSeller_Click"
                                                        Style="display: ;" />
                                                    <asp:HiddenField ID="hdnBestSeller" runat="server" Value="0" />
                                                </div>
                                                <div style="display: none;">
                                                    <asp:Button ID="btnhdnNewArrival" runat="server" Text="Button" OnClick="btnhdnNewArrival_Click"
                                                        Style="display: ;" />
                                                    <asp:HiddenField ID="hdnNewArrival" runat="server" Value="0" />
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
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script language="javascript" type="text/javascript">

        function openCenteredCrossSaleWindow1(mode) {
            if (document.getElementById('<%=ddlStore.ClientID %>').value != "0") {
                var width = 700;
                var height = 500;
                var left = parseInt((screen.availWidth / 2) - (width / 2));
                var top = parseInt((screen.availHeight / 2) - (height / 2));
                var StoreID = document.getElementById('<%=ddlStore.ClientID %>').value;
                var windowFeatures = "width=" + width + ",height=" + height + ",status=no,resizable=yes,scrollbars=yes,left=" + left + ",top=" + top + ",screenX=" + left + ",screenY=" + top;
                window.open('IndexPageConfigPopup.aspx?StoreID=' + StoreID + '&mode=' + mode, "Mywindow", windowFeatures);
                return false;
            }
            else {
                jAlert('Select Store!', 'Message', '<%=ddlStore.ClientID %>');
                return false;
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                jAlert('Enter Valid Display Order!', 'Message');
                return false;
            }
            return true;
        }

        function testi() {
            var bt = document.getElementById('<%=btnFeatureCategory.ClientID %>');
            if (bt) {
                bt.click();
            }
        }

    </script>
</asp:Content>
