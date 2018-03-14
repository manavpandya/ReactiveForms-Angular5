<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="EFFHomePageBanners.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.EFFHomePageBanners" %>
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
                                                     <asp:CheckBox  style="width:25px;width:17px;" ID="chksection1" runat="server" />  Active Section 1  &nbsp;&nbsp;&nbsp  Display Order : <asp:TextBox ID="txtDOSection1" runat="server"></asp:TextBox></h2>
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
                                    <img id="img0" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 770 x 355</b></font>
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
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 770 x 355</b></font>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="55%" style="border-bottom: solid 1px #d7d7d7;" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td colspan="2" align="left">
                                    <img id="img1" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left"><font color="red" size="3"><b>Size Should be 770 x 355</b></font>
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
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 770 x 355</b></font>
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
                                                <asp:CheckBox  style="width:25px;width:17px;" ID="chksection2" runat="server" />        Active Section 2  &nbsp;&nbsp;&nbsp  Display Order : <asp:TextBox ID="txtDOSection2" runat="server"></asp:TextBox></h2>
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
                                    <img id="img2" runat="server" />
                                </td>

                            </tr>
                            <tr>
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 1570 x 200</b></font>
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
                                <td align="left" width="100%"><font color="red" size="3"><b>Size Should be 1570 x 200</b></font>
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
            

        </div>
    </div>




</asp:Content>
