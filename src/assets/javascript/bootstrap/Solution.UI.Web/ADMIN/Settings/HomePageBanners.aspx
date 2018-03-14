<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="HomePageBanners.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.HomePageBanners" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
     <style type="text/css">
       

        .slidingDiv {
            height: 300px;
            padding: 20px;
            margin-top: 10px;
        }

        .show_hide {
            display: block;
        }

        .footerBorder {
            border-top: 1px solid #DFDFDF;
            border-right: 1px solid #DFDFDF;
        }

        .footerBorderinventory {
            border-top: 1px solid #DFDFDF;
        }

        .divfloatingcss {
            bottom: 0;
            right: 0;
            position: fixed;
            width: 14%;
            margin-right: 43%;
            background-image: url("/Admin/images/title-bg-trans.png");
            -webkit-border-top-left-radius: 15px;
            -webkit-border-top-right-radius: 15px;
            -moz-border-radius-topleft: 15px;
            -moz-border-radius-topright: 15px;
            border-top-left-radius: 15px;
            border-top-right-radius: 15px;
        }
    </style>
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
    <script type="text/javascript">
        function Checkfields() {
            // document.getElementById('').value = '';
            var fup1 = document.getElementById('<%=FileUpload1.ClientID %>');
            var fileName1 = fup1.value;
            var ext1 = fileName1.substring(fileName1.lastIndexOf('.') + 1);
            ext1 = ext1.toString().toLowerCase().Trim();
            alert(fileName1);
            alert(ext1);

            var fup2 = document.getElementById('<%=FileUpload2.ClientID %>');
            var fileName2 = fup2.value;
            var ext2 = fileName2.substring(fileName2.lastIndexOf('.') + 1);
            ext2 = ext2.toString().toLowerCase().Trim();

            var fup3 = document.getElementById('<%=FileUpload3.ClientID %>');
            var fileName3 = fup3.value;
            var ext3 = fileName3.substring(fileName3.lastIndexOf('.') + 1);
            ext3 = ext3.toString().toLowerCase().Trim();

            var fup4 = document.getElementById('<%=FileUpload4.ClientID %>');
            var fileName4 = fup4.value;
            var ext4 = fileName4.substring(fileName4.lastIndexOf('.') + 1);
            ext4 = ext4.toString().toLowerCase().Trim();

            var fup5 = document.getElementById('<%=FileUpload5.ClientID %>');
            var fileName5 = fup5.value;
            var ext5 = fileName5.substring(fileName5.lastIndexOf('.') + 1);
            ext5 = ext5.toString().toLowerCase().Trim();

            var fup6 = document.getElementById('<%=FileUpload6.ClientID %>');
            var fileName6 = fup6.value;
            var ext6 = fileName1.substring(fileName6.lastIndexOf('.') + 1);
            ext6 = ext6.toString().toLowerCase().Trim();

            var fup7 = document.getElementById('<%=FileUpload7.ClientID %>');
            var fileName7 = fup7.value;
            var ext7 = fileName7.substring(fileName7.lastIndexOf('.') + 1);
            ext7 = ext7.toString().toLowerCase().Trim();

            var fup8 = document.getElementById('<%=FileUpload8.ClientID %>');
            var fileName8 = fup8.value;
            var ext8 = fileName8.substring(fileName8.lastIndexOf('.') + 1);
            ext8 = ext8.toString().toLowerCase().Trim();

            var fup9 = document.getElementById('<%=FileUpload9.ClientID %>');
            var fileName9 = fup9.value;
            var ext9 = fileName9.substring(fileName9.lastIndexOf('.') + 1);
            ext9 = ext9.toString().toLowerCase().Trim();

            var fup10 = document.getElementById('<%=FileUpload10.ClientID %>');
            var fileName10 = fup10.value;
            var ext10 = fileName10.substring(fileName10.lastIndexOf('.') + 1);
            ext10 = ext10.toString().toLowerCase().Trim();

            var fup11 = document.getElementById('<%=FileUpload11.ClientID %>');
            var fileName11 = fup11.value;
            var ext11 = fileName11.substring(fileName11.lastIndexOf('.') + 1);
            ext11 = ext11.toString().toLowerCase().Trim();

            var fup12 = document.getElementById('<%=FileUpload12.ClientID %>');
            var fileName12 = fup12.value;
            var ext12 = fileName12.substring(fileName12.lastIndexOf('.') + 1);
            ext12 = ext12.toString().toLowerCase().Trim();

            var fup13 = document.getElementById('<%=FileUpload13.ClientID %>');
            var fileName13 = fup13.value;
            var ext13 = fileName13.substring(fileName13.lastIndexOf('.') + 1);
            ext13 = ext13.toString().toLowerCase().Trim();

            //var fuData = document.getElementById('ContentPlaceHolder1_FileUploadExpireEvent');
            //var FileUploadPath = fuData.value;
            //var chkimg = '';
            //if (document.getElementById('ContentPlaceHolder1_ImgSaveExpireEventPage') != null && document.getElementById('ContentPlaceHolder1_ImgSaveExpireEventPage').src != "") {
            //    chkimg = document.getElementById('ContentPlaceHolder1_ImgSaveExpireEventPage').src;
            //}
            //if (FileUploadPath == '' && chkimg == '') {

            //    jAlert('Please Select Banner Image.', 'Message', 'ContentPlaceHolder1_FileUploadExpireEvent');
            //    return false;
            //}
            //else {
            //    var Extension = FileUploadPath.substring(FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

            //    if (Extension == "jpg" || Extension == "jpeg" || Extension == "png" || Extension == "gif" || Extension == "bmp") {
            //        return true; // Valid file type
            //    }
            //    else {
            //        if (chkimg == '') {
            //            jAlert('Please Select valid Banner File.', 'Message', 'ContentPlaceHolder1_FileUploadExpireEvent');

            //            return false;
            //        }
            //        else {
            //            return true;
            //        }
            //    }
            //}
            return false;
            if (fileName1 == '' && document.getElementById('<%=img1.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload1.ClientID %>');

                return false;
            }
            else {

                if (ext1 == '' || (ext1 == "gif" || ext1 == "jpeg" || ext1 == "jpg" || ext1 == "png" || ext1 == "bmp")) {
                    if (document.getElementById('<%=img1.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload1.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload1.ClientID %>');
                    return false;
                }


            }



            if (fileName2 == '' && document.getElementById('<%=img2.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload2.ClientID %>');

                return false;
            }
            else {

                if (ext2 == '' || (ext2 == "gif" || ext2 == "jpeg" || ext2 == "jpg" || ext2 == "png" || ext2 == "bmp")) {
                    if (document.getElementById('<%=img2.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload2.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload2.ClientID %>');
                    return false;
                }


            }

            if (fileName3 == '' && document.getElementById('<%=img3.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload3.ClientID %>');

                return false;
            }
            else {

                if (ext3 == '' || (ext3 == "gif" || ext3 == "jpeg" || ext3 == "jpg" || ext3 == "png" || ext3 == "bmp")) {
                    if (document.getElementById('<%=img3.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload3.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload3.ClientID %>');
                    return false;
                }


            }


            if (fileName4 == '' && document.getElementById('<%=img4.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload4.ClientID %>');

                return false;
            }
            else {

                if (ext4 == '' || (ext4 == "gif" || ext4 == "jpeg" || ext4 == "jpg" || ext4 == "png" || ext4 == "bmp")) {
                    if (document.getElementById('<%=img4.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload4.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload4.ClientID %>');
                    return false;
                }


            }



            if (fileName5 == '' && document.getElementById('<%=img5.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload5.ClientID %>');

                return false;
            }
            else {

                if (ext5 == '' || (ext5 == "gif" || ext5 == "jpeg" || ext5 == "jpg" || ext5 == "png" || ext5 == "bmp")) {
                    if (document.getElementById('<%=img5.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload5.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload5.ClientID %>');
                    return false;
                }


            }



            if (fileName6 == '' && document.getElementById('<%=img6.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload6.ClientID %>');

                 return false;
             }
             else {

                 if (ext6 == '' || (ext6 == "gif" || ext6 == "jpeg" || ext6 == "jpg" || ext6 == "png" || ext6 == "bmp")) {
                     if (document.getElementById('<%=img6.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload6.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload6.ClientID %>');
                    return false;
                }


            }



            if (fileName7 == '' && document.getElementById('<%=img7.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload7.ClientID %>');

                 return false;
             }
             else {

                 if (ext7 == '' || (ext7 == "gif" || ext7 == "jpeg" || ext7 == "jpg" || ext7 == "png" || ext7 == "bmp")) {
                     if (document.getElementById('<%=img7.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload7.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload7.ClientID %>');
                    return false;
                }


            }


            if (fileName8 == '' && document.getElementById('<%=img8.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload8.ClientID %>');

                 return false;
             }
             else {

                 if (ext8 == '' || (ext8 == "gif" || ext8 == "jpeg" || ext8 == "jpg" || ext8 == "png" || ext8 == "bmp")) {
                     if (document.getElementById('<%=img8.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload8.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload8.ClientID %>');
                    return false;
                }


            }


            if (fileName9 == '' && document.getElementById('<%=img9.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload9.ClientID %>');

                 return false;
             }
             else {

                 if (ext9 == '' || (ext9 == "gif" || ext9 == "jpeg" || ext9 == "jpg" || ext9 == "png" || ext9 == "bmp")) {
                     if (document.getElementById('<%=img9.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload9.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload9.ClientID %>');
                    return false;
                }


            }


            if (fileName10 == '' && document.getElementById('<%=img10.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload10.ClientID %>');

                 return false;
             }
             else {

                 if (ext10 == '' || (ext10 == "gif" || ext10 == "jpeg" || ext10 == "jpg" || ext10 == "png" || ext10 == "bmp")) {
                     if (document.getElementById('<%=img10.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload10.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload10.ClientID %>');
                    return false;
                }


            }


            if (fileName11 == '' && document.getElementById('<%=img11.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload11.ClientID %>');

                 return false;
             }
             else {

                 if (ext11 == '' || (ext11 == "gif" || ext11 == "jpeg" || ext11 == "jpg" || ext11 == "png" || ext11 == "bmp")) {
                     if (document.getElementById('<%=img11.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload11.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload11.ClientID %>');
                    return false;
                }


            }


            if (fileName12 == '' && document.getElementById('<%=img12.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload12.ClientID %>');

                 return false;
             }
             else {

                 if (ext12 == '' || (ext12 == "gif" || ext12 == "jpeg" || ext12 == "jpg" || ext12 == "png" || ext12 == "bmp")) {
                     if (document.getElementById('<%=img12.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload12.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload12.ClientID %>');
                    return false;
                }


            }


            if (fileName13 == '' && document.getElementById('<%=img13.ClientID %>').src == '') {
                jAlert('Please select image file.', 'Required', '<%=FileUpload13.ClientID %>');

                 return false;
             }
             else {

                 if (ext13 == '' || (ext13 == "gif" || ext13 == "jpeg" || ext13 == "jpg" || ext13 == "png" || ext13 == "bmp")) {
                     if (document.getElementById('<%=img13.ClientID %>').src == '') {
                        jAlert('Please select image file.', 'Required', '<%=FileUpload13.ClientID %>');

                        return false;

                    }

                }
                else {
                    jAlert('Please select only image file.', 'Required', '<%=FileUpload13.ClientID %>');
                    return false;
                }


            }




            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="content-width">
        <div class="content-row1">
        </div>
        <div class="content-row2">
            <fieldset>
                <legend style="font-weight:bold;">
                    Group 1
  </legend>
              
           
            <table  width="100%" border="0" cellspacing="0" cellpadding="0" class="content-table">
                <tr>
                    <th colspan="3">
                          <div class="main-title-left">
                                                 
                                                    <h2>
                                                     <asp:CheckBox  style="width:25px;width:17px;" ID="chksection1" runat="server" />   Active Section 1  &nbsp;&nbsp;&nbsp  Display Order : <asp:TextBox ID="txtDOSection1" runat="server"></asp:TextBox>


                                                    </h2>
                                                </div>

                    </th>
                    

                 <%--   <td colspan="3"  align="left" style="border: solid 1px #d7d7d7; border-collapse: collapse" valign="top">
                      
                    </td>--%>
                </tr>
                <tr>
                    <td colspan="2" width="45%;" align="left" style="border: solid 1px #d7d7d7; border-collapse: collapse" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                             <tr>
                                <td align="left" width="100%" style="border: solid 1px #d7d7d7; border-collapse: collapse; border-bottom: none;">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <img id="img0" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 250</b></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner File :

                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="FileUpload0" runat="server" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                                    ControlToValidate="FileUpload0" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Banner Title :

                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtbannerTitle0" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner URL :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtBannerURL0" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Target : 
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTarget0" runat="server" CssClass="order-list">
                                                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                    <asp:ListItem Value="_self">Self</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trf0" visible="false">
                                            <td align="left">Error Message : 
                                            </td>
                                            <td align="left"><font color="red" size="3"><b>Size Should be 780 x 250</b></font>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td align="left" width="100%" style="border: solid 1px #d7d7d7; border-collapse: collapse; border-top: none;">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td colspan="2" align="left">
                                    <img id="img1" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 250</b></font>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner File :

                                </td>
                                <td align="left">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                        ControlToValidate="FileUpload1" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Banner Title :

                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtbannerTitle1" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner URL :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtBannerURL1" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Target : 
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTarget1" runat="server" CssClass="order-list">
                                        <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                        <asp:ListItem Value="_self">Self</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trf1" visible="false">
                                <td align="left">Error Message : 
                                </td>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 250</b></font>
                                </td>
                            </tr>
                               </table>
                                    </td>
                                    </tr>
                                     
                        </table>
                    </td>
                    <td width="55%;" align="left" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td align="left" width="100%" style="border: solid 1px #d7d7d7; border-collapse: collapse; border-bottom: none;">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <img id="img2" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 250</b></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner File :

                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="FileUpload2" runat="server" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                                    ControlToValidate="FileUpload2" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Banner Title :

                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtbannerTitle2" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner URL :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtBannerURL2" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Target : 
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTarget2" runat="server" CssClass="order-list">
                                                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                    <asp:ListItem Value="_self">Self</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trf2" visible="false">
                                            <td align="left">Error Message : 
                                            </td>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 250</b></font>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="100%" style="border: solid 1px #d7d7d7; border-collapse: collapse; border-top: none;">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <img id="img3" runat="server" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 250</b></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner File :

                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="FileUpload3" runat="server" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                                    ControlToValidate="FileUpload3" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Banner Title :

                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtbannerTitle3" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner URL :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtBannerURL3" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Target : 
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTarget3" runat="server" CssClass="order-list">
                                                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                    <asp:ListItem Value="_self">Self</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trf3" visible="false">
                                            <td align="left">Error Message : 
                                            </td>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 250</b></font>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>


                        </table>


                    </td>

                </tr>



                 <tr>
                      <th colspan="3">
                          <div class="main-title-left">
                                                 
                                                    <h2>
                                                     <asp:CheckBox  style="width:25px;width:17px;" ID="chksection2" runat="server" />  Active Section 2  &nbsp;&nbsp;&nbsp  Display Order : <asp:TextBox ID="txtDOSection2" runat="server"></asp:TextBox></h2>
                                                </div>

                    </th>
                   <%-- <td colspan="4" width="45%;" align="left" style="border: solid 1px #d7d7d7; border-collapse: collapse" valign="top">
                      Active Section 2  <asp:CheckBox ID="chksection2" runat="server" />
                    </td>--%>
                </tr>

                <tr>
                    <td colspan="2" width="45%" style="border-bottom: solid 1px #d7d7d7;" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td colspan="2" align="left">
                                    <img id="img4" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 355</b></font>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner File :

                                </td>
                                <td align="left">
                                    <asp:FileUpload ID="FileUpload4" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                        ControlToValidate="FileUpload4" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Banner Title :

                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtbannerTitle4" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner URL :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtBannerURL4" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Target : 
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTarget4" runat="server" CssClass="order-list">
                                        <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                        <asp:ListItem Value="_self">Self</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trf4" visible="false">
                                <td align="left">Error Message : 
                                </td>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 355</b></font>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="55%" style="border-bottom: solid 1px #d7d7d7;" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td colspan="2" align="left">
                                    <img id="img5" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left"><font color="red" size="3"><b>Size Should be 780 x 355</b></font>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner File :

                                </td>
                                <td align="left">
                                    <asp:FileUpload ID="FileUpload5" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                        ControlToValidate="FileUpload5" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Banner Title :

                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtbannerTitle5" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner URL :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtBannerURL5" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Target : 
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTarget5" runat="server" CssClass="order-list">
                                        <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                        <asp:ListItem Value="_self">Self</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trf5" visible="false">
                                <td align="left">Error Message : 
                                </td>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 780 x 355</b></font>
                                </td>
                            </tr>
                        </table>



                    </td>
                </tr>



            </table>

                </fieldset>
             <fieldset>
                   <legend style="font-weight:bold;">
                    Group 2
                </legend>
            <table  border="0" cellspacing="0" cellpadding="0" class="content-table">
              
                <tr>
                      <th colspan="5">
                          <div class="main-title-left">
                                                 
                                                    <h2>
                                        <asp:CheckBox  style="width:25px;width:17px;" ID="chksection3" runat="server" />                Active Section 3  &nbsp;&nbsp;&nbsp  Display Order : <asp:TextBox ID="txtDOSection3" runat="server"></asp:TextBox> </h2>
                                                </div>

                    </th>
                  <%--  <td colspan="5" width="45%;" align="left" style="border: solid 1px #d7d7d7; border-collapse: collapse" valign="top">
                      Active Section 3  <asp:CheckBox ID="chksection3" runat="server" />
                    </td>--%>
                </tr>
                <tr>
                    <td colspan="2" width="33%" style="border-bottom: solid 1px #d7d7d7;" valign="top">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <img id="img6" runat="server" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 500</b></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner File :

                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="FileUpload6" runat="server" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                                    ControlToValidate="FileUpload6" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Banner Title :

                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtbannerTitle6" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner URL :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtBannerURL6" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Target : 
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTarget6" runat="server" CssClass="order-list">
                                                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                    <asp:ListItem Value="_self">Self</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trf6" visible="false">
                                            <td align="left">Error Message : 
                                            </td>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 500</b></font>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <img id="img7" runat="server" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 220</b></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner File :

                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="FileUpload7" runat="server" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                                    ControlToValidate="FileUpload7" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Banner Title :

                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtbannerTitle7" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner URL :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtBannerURL7" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Target : 
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTarget7" runat="server" CssClass="order-list">
                                                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                    <asp:ListItem Value="_self">Self</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trf7" visible="false">
                                            <td align="left">Error Message : 
                                            </td>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 220</b></font>
                                            </td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>

                        </table>



                    </td>
                    <td colspan="2" width="33%" style="border-bottom: solid 1px #d7d7d7;" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td colspan="2" align="left">
                                    <img id="img8" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 750</b></font>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner File :

                                </td>
                                <td align="left">
                                    <asp:FileUpload ID="FileUpload8" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                        ControlToValidate="FileUpload8" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Banner Title :

                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtbannerTitle8" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner URL :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtBannerURL8" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Target : 
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTarget8" runat="server" CssClass="order-list">
                                        <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                        <asp:ListItem Value="_self">Self</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trf8" visible="false">
                                <td align="left">Error Message : 
                                </td>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 750</b></font>
                                </td>
                            </tr>
                        </table>



                    </td>
                    <td colspan="2" width="34%" style="border-bottom: solid 1px #d7d7d7;" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <img id="img9" runat="server" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 250</b></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner File :

                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="FileUpload9" runat="server" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                                    ControlToValidate="FileUpload9" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Banner Title :

                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtbannerTitle9" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner URL :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtBannerURL9" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Target : 
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTarget9" runat="server" CssClass="order-list">
                                                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                    <asp:ListItem Value="_self">Self</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trf9" visible="false">
                                            <td align="left">Error Message : 
                                            </td>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 250</b></font>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <img id="img10" runat="server" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 470</b></font>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner File :

                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="FileUpload10" runat="server" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                                    ControlToValidate="FileUpload10" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Banner Title :

                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtbannerTitle10" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">Banner URL :
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="TxtBannerURL10" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td align="left">Target : 
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlTarget10" runat="server" CssClass="order-list">
                                                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                    <asp:ListItem Value="_self">Self</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trf10" visible="false">
                                            <td align="left">Error Message : 
                                            </td>
                                            <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 470</b></font>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>

            </table>
                 
            <br />

           
              
            <table  border="0" cellspacing="0" cellpadding="0" class="content-table">
                  <tr>
                        <th colspan="5">
                          <div class="main-title-left">
                                                 
                                                    <h2>
                                                <asp:CheckBox  style="width:25px;width:17px;" ID="chksection4" runat="server" />        Active Section 4  &nbsp;&nbsp;&nbsp  Display Order : <asp:TextBox ID="txtDOSection4" runat="server"></asp:TextBox></h2>
                                                </div>

                    </th>
                <%--    <td colspan="5" width="45%;" align="left" style="border: solid 1px #d7d7d7; border-collapse: collapse" valign="top">
                      Active Section 4  <asp:CheckBox ID="chksection4" runat="server" />
                    </td>--%>
                </tr>
                <tr>
                    <td width="100%" style="border-bottom: solid 1px #d7d7d7;" valign="top">

                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td colspan="2" align="left">
                                    <img id="img11" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 1590 x 200</b></font>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner File :

                                </td>
                                <td align="left">
                                    <asp:FileUpload ID="FileUpload11" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                        ControlToValidate="FileUpload11" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Banner Title :

                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtbannerTitle11" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner URL :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtBannerURL11" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Target : 
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTarget11" runat="server" CssClass="order-list">
                                        <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                        <asp:ListItem Value="_self">Self</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trf11" visible="false">
                                <td align="left">Error Message : 
                                </td>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 1590 x 200</b></font>
                                </td>
                            </tr>
                        </table>



                    </td>
                </tr>
            </table>

            <table  border="0" cellspacing="0" cellpadding="0" class="content-table">
                 <tr>
                      <th colspan="5">
                          <div class="main-title-left">
                                                 
                                                    <h2>
                                                 <asp:CheckBox  style="width:25px;width:17px;" ID="chksection5" runat="server" />      Active Section 5   &nbsp;&nbsp;&nbsp  Display Order : <asp:TextBox ID="txtDOSection5" runat="server"></asp:TextBox></h2>
                                                </div>

                    </th>



                   <%-- <td colspan="5" width="45%;" align="left" style="border: solid 1px #d7d7d7; border-collapse: collapse" valign="top">
                      Active Section 5  <asp:CheckBox ID="chksection5" runat="server" />
                    </td>--%>
                </tr>
                <tr>
                    <td width="50%" style="border-bottom: solid 1px #d7d7d7;" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td colspan="2" align="left">
                                    <img id="img12" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 1050 x 300</b></font>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner File :

                                </td>
                                <td align="left">
                                    <asp:FileUpload ID="FileUpload12" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                        ControlToValidate="FileUpload12" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Banner Title :

                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtbannerTitle12" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner URL :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtBannerURL12" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Target : 
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTarget12" runat="server" CssClass="order-list">
                                        <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                        <asp:ListItem Value="_self">Self</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trf12" visible="false">
                                <td align="left">Error Message : 
                                </td>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 1050 x 300</b></font>
                                </td>
                            </tr>
                        </table>

                    </td>
                    <td width="50%" style="border-bottom: solid 1px #d7d7d7;" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td colspan="2" align="left">
                                    <img id="img13" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 300</b></font>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner File :

                                </td>
                                <td align="left">
                                    <asp:FileUpload ID="FileUpload13" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ErrorMessage="Please enter a valid image file" ValidationGroup="validate"
                                        ControlToValidate="FileUpload13" ForeColor="#FF3300" ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G))$"></asp:RegularExpressionValidator>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Banner Title :

                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtbannerTitle13" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">Banner URL :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="TxtBannerURL13" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td align="left">Target : 
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTarget13" runat="server" CssClass="order-list">
                                        <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                        <asp:ListItem Value="_self">Self</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trf13" visible="false">
                                <td align="left">Error Message : 
                                </td>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 510 x 300</b></font>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
            <table style="text-align:center;width:100%">
                <tr>
                    <td width="100%;" align="center">
                         <div id="divfloating" class="divfloatingcss" style="width: 300px;">
                                                                <div style="margin-bottom: 1px; margin-top: 3px;">
                        <asp:ImageButton ID="imgSave" runat="server" ValidationGroup="validate" AlternateText="Save" ToolTip="Save"
                            OnClick="imgSave_Click" />
                        <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cencel" ToolTip="Cencel" OnClick="imgCancle_Click" />
                                                                    </div>
                             </div>
                    </td>
                </tr>
            </table>
                </fieldset>
        </div>
        <div style="display: none;">
             <input type="hidden" id="Hidden0" runat="server" value="" />
            <input type="hidden" id="Hidden1" runat="server" value="" />
            <input type="hidden" id="Hidden2" runat="server" value="" />
            <input type="hidden" id="Hidden3" runat="server" value="" />
            <input type="hidden" id="Hidden4" runat="server" value="" />
            <input type="hidden" id="Hidden5" runat="server" value="" />
            <input type="hidden" id="Hidden6" runat="server" value="" />
            <input type="hidden" id="Hidden7" runat="server" value="" />
            <input type="hidden" id="Hidden8" runat="server" value="" />
            <input type="hidden" id="Hidden9" runat="server" value="" />
            <input type="hidden" id="Hidden10" runat="server" value="" />
            <input type="hidden" id="Hidden11" runat="server" value="" />
            <input type="hidden" id="Hidden12" runat="server" value="" />
            <input type="hidden" id="Hidden13" runat="server" value="" />

        </div>
    </div>




</asp:Content>
