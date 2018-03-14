<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewProductImages.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ViewProductImages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <style type="text/css">
        .readymade-detail
        {
            float: left;
            width: 100%;
            padding: 5px 0 0 0;
        }
        .readymade-detail-pt1
        {
            float: left;
            width: 100%;
            padding: 0 0 10px 0;
        }
        /*.readymade-detail-left{float:left; width:24%; font-size:10px; color:#848383; line-height:20px; text-transform:uppercase;}*/
        .readymade-detail-left
        {
            float: left;
            width: 24%;
            color: #848383;
            line-height: 20px;
            font-family: Arial,Helvetica,sans-serif;
            text-transform: none;
            font-size: 12px;
        }
        
        .readymade-detail-right
        {
            float: right;
            width: 74%;
        }
        .option1
        {
            float: left;
            width: 75%;
            border: 1px solid #ddd;
            padding: 1px;
            font-size: 12px;
            color: #848383;
            line-height: 20px;
            height: 20px;
        }
        .readymade-detail-right span
        {
            float: left;
            margin: 0 0 0 5px;
            line-height: 20px;
            font-size: 12px;
            color: #848383;
        }
        .readymade-detail-right span a
        {
            text-decoration: none;
            color: #848383;
        }
        .readymade-detail .price-detail
        {
            float: left;
            width: 100%;
            padding: 5px 0 0 0;
        }
        .readymade-detail .price-detail-left
        {
            float: left;
            width: 285px;
        }
        .readymade-detail .price-detail-left p
        {
            float: left;
            width: 100%;
            font-size: 12px;
            color: #848383;
            line-height: 15px;
        }
        .readymade-detail .price-detail-left p tt
        {
            float: left;
            font-family: 'helvetica-l' ,Arial,Helvetica,sans-serif;
            font-size: 14px;
            padding: 0 5px 0 0;
        }
        .readymade-detail .price-detail-left p span
        {
            font-size: 12px;
            color: #848383;
            line-height: 15px;
        }
        .readymade-detail .price-detail-left p span.per
        {
            font-size: 12px;
            color: #b92127;
            line-height: 15px;
            margin: 0 0 0 2px;
        }
        
        .readymade-detail .price-detail-left p strong
        {
            font-size: 16px;
            color: #b92127;
            line-height: 15px;
            margin: 0 0 0 2px;
        }
        .readymade-detail .price-detail-right
        {
            float: right;
            margin: 0 0 0 0;
            text-align: left;
            padding: 0;
        }
        .readymade-detail .price-detail-right p tt
        {
            float: left;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            padding: 0 5px 0 0;
        }
        
        /* Style for Tabbing*/
        
        
        .item-left-row3
        {
            float: left;
            width: 100%;
            border-bottom: 1px solid #ddd;
            padding: 20px 0 0 0;
        }
        .item-tb-left
        {
            float: left;
            font-family: 'helvetica-l' ,Arial,Helvetica,sans-serif;
            width: 100%;
            padding: 0 0px 0px 0;
        }
        .item-tb-left-tab
        {
            float: left;
            width: 100%;
            padding-left: 0;
        }
        .tabing
        {
            float: left;
            height: 30px;
        }
        .tabing ul
        {
            list-style: none;
            padding: 0;
            margin: 0;
            float: left;
        }
        .tabing ul li
        {
            list-style: none;
            float: left;
            background: url(../images/tab_left.jpg) left -34px no-repeat;
            padding-left: 8px;
            height: 30px;
            padding: 0 0 0 3px;
            margin: 0 10px 0 0;
        }
        .tabing ul li:hover
        {
            background: url(/images/tab_left.jpg) left top no-repeat;
        }
        .tabing ul li a
        {
            float: left;
            outline: none;
            margin-right: 0px;
            text-decoration: none;
            background: url(/images/tab_right.jpg) right -34px no-repeat;
            padding: 9px 15px 5px 10px;
            font-size: 12px;
            text-transform: uppercase;
            font-weight: normal;
            color: #848383;
            height: 17px;
        }
        .tabing ul li a:hover
        {
            background: url(/images/tab_right.jpg) right 0 no-repeat;
            font-size: 12px;
            color: #848383;
            padding: 9px 15px 5px 10px;
            text-transform: uppercase;
            font-weight: normal;
        }
        .tabing ul li:hover a
        {
            background: url(/images/tab_right_active.jpg) right 0 no-repeat;
            font-size: 12px;
            color: #fff;
            padding: 9px 15px 5px 10px;
            text-transform: uppercase;
            font-weight: normal;
        }
        .tabing ul li.tabberactive
        {
            background: url(/images/tab_left_active.jpg) left top no-repeat;
        }
        .tabing ul li.tabberactive a
        {
            background: url(/images/tab_right_active.jpg) right 0 no-repeat;
            font-size: 12px;
            color: #fff;
            padding: 9px 15px 5px 10px;
            text-transform: uppercase;
            font-weight: normal;
        }
        .tabberactive ul
        {
            list-style: none;
            padding: 0;
            margin: 0;
            float: left;
        }
        .tabberactive ul li
        {
            list-style: none;
            padding: 0;
            margin: 0;
        }
        .tabberactive a
        {
            background: url(/images/tab_left.jpg) no-repeat left 0px;
            padding: 0 0 0 0px;
            float: left;
            color: #848383;
            font-weight: normal;
        }
        .tabberactive a:hover
        {
            background: url(/images/tab_right.jpg) no-repeat right 0px;
            padding: 0 5px 0 10px;
            float: left;
            color: #848383;
            font-weight: normal;
        }
        .tabberlive .tabbertabhide
        {
            display: none;
        }
        .tabberlive .tabbertab
        {
            width: 100%;
            float: left;
            position: relative;
            padding: 5px 0;
            border: 1px solid #eeeeee;
        }
        .tabberlive .tabbertab h6
        {
            display: none;
        }
        .tabberlive .tabbertabhide
        {
            display: none;
        }
        .tabberlive .tabbertab
        {
            width: 100%;
            float: left;
            position: relative;
            padding: 5px 0;
        }
        .tabberlive .tabbertab h6
        {
            display: none;
        }
        .tabberlive .tabbertab p
        {
            font-size: 13px;
            color: #848383;
            margin: 0;
            line-height: 16px;
            padding: 0 0 5px 0;
        }
        .tabbertab ul
        {
            padding: 0px 0px 0px 10px;
            width: 500px;
            margin: 0px;
            float: left;
            list-style: none;
            font-size: 11px;
            color: #000;
        }
        .tabbertab ul li
        {
            padding: 5px 0px 3px 15px;
            float: left;
            width: 500px;
            background: url(images/left_bullet.gif) no-repeat left 9px;
        }
        .tabbertab ul li ul
        {
            padding: 0px;
            margin: 0px;
        }
        .tabbertab ul li ul li
        {
            float: left;
            width: 300px;
            padding-bottom: 0px;
        }
        .checkbox
        {
            float: left;
        }
        .checkbox-text
        {
            float: left;
            width: 36%;
            line-height: 22px;
            text-align: left;
        }
        .checkout-td-text
        {
            border: 1px solid #ddd;
            color: #6C6D71;
            font-size: 12px;
            height: 18px;
            padding: 1px 1%;
            width: 60%;
            margin: 0;
        }
    </style>
      <script type="text/javascript">
          function keyRestrictForInventory(e, validchars) {
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
          </script>
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
    <style type="text/css">
        .content-table td
        {
            background-color: transparent !important;
        }
    </style>
     <style type="text/css">
        #tab-container-product ul.menu li {
            margin-bottom: 0;
        }

        #tab-container ul.menu li {
            margin-bottom: -1px;
        }

        #tab-container-1 ul.menu li {
            margin-bottom: -1px;
        }

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
    <title></title>
</head>
<body style="background: none;">
     <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" language="javascript"> </script>
    <form id="form1" runat="server">
   <div>
           <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td">
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <th>
                                                                                                                        <div class="main-title-left">
                                                                                                                            <img class="img-left" title="Images" alt="Images" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                                            <h2>Images</h2>
                                                                                                                        </div>
                                                                                                                        <div class="main-title-right">
                                                                                                                            <a href="javascript:void(0);" class="show_hideImage" title="Close" onclick="return ShowHideButton('ImgImages','tdImages');">
                                                                                                                                <img id="ImgImages" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" class="minimize"  hidden="hidden"
                                                                                                                                    title="Minimize" alt="Minimize"></a>
                                                                                                                        </div>
                                                                                                                    </th>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td id="tdImages">
                                                                                                                        <div id="divImage" class="slidingDivImage">
                                                                                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">

                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                            <tr>
                                                                                                                                                <td width="20%" valign="top">Main Image:
                                                                                                                                                </td>
                                                                                                                                                <td valign="middle">
                                                                                                                                                    <div style="float: left;">
                                                                                                                                                        <img  id="ImgLarge" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                            runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                    </div>
                                                                                                                                                        </td>
                                                                                                                                                 <td valign="top" colspan="2">
                                                                                                                                                     
                                                                                                                                                     <table>
                                                                                                                                                         <tr>
                                                                                                                                                             <td align="left">
                                                                                                                                                <div style="float: left;display:none;">
                                                                                                                                                        Icon Width:
                                                                                                                                <asp:TextBox ID="txtIconWidth" CssClass="order-textfield" Style="width: 70px; text-align: center;"
                                                                                                                                    runat="server" onKeyPress="rerutn keyRestrictForInventory(event,'0123456789');"
                                                                                                                                    MaxLength="4"></asp:TextBox>
                                                                                                                                                        &nbsp;&nbsp;Icon Height:
                                                                                                                                <asp:TextBox ID="txtIconHeigth" CssClass="order-textfield" Style="width: 70px; text-align: center;"
                                                                                                                                    runat="server" onKeyPress="rerutn keyRestrictForInventory(event,'0123456789');"
                                                                                                                                    MaxLength="4"></asp:TextBox>
                                                                                                                                                    </div>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                                         <tr>
                                                                                                                                                             <td>
                                                                                                                                                                 Image Description :
                                                                                                                                                             </td>
                                                                                                                                                         </tr>
                                                                                                                                                         <tr>
                                                                                                                                                             <td>
                                                                                                                                                                 <asp:TextBox ID="txtImgDesc" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                        runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                                                                                                                             </td>
                                                                                                                                                         </tr>
                                                                                                                                                     </table>
                                                                                                                                                     </td>
                                                                                                                                               
                                                                                                                                                    
                                                                                                                                                
                                                                                                                                                
                                                                                                                                            </tr>
                                                                                                                                            


                                                                                                                                            <tr>
                                                                                                                                                <td>&nbsp;
                                                                                                                                                </td>
                                                                                                                                                <td>
                                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                        <tr>
                                                                                                                                                            <td width="10%">
                                                                                                                                                                <asp:FileUpload ID="fuProductIcon" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                    TabIndex="25" Visible="false" />
                                                                                                                                                            </td>
                                                                                                                                                           
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                             <td width="9%">
                                                                                                                                                                <asp:ImageButton ID="btnUpload" runat="server" AlternateText="Upload" OnClick="btnUpload_Click"
                                                                                                                                                                    TabIndex="26" Visible="false" />
                                                                                                                                                                 <asp:ImageButton ID="btnDelete" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                    OnClick="btnDelete_Click" />
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%">
                                                                                                                                                                
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </td>
                                                                                                                                            </tr>

                                                                                                                                        </table>
                                                                                                                                    </td>
                                                                                                                                    <td valign="bottom" id="trUploadFiles" runat="server">
                                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                            <tr>
                                                                                                                                                <td valign="bottom" style="padding: 0px 2px;">
                                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">

                                                                                                                                                        <tr>
                                                                                                                                                            <td>
                                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" id="tduploadMoreImages"
                                                                                                                                                                    runat="server" visible="false">
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td>
                                                                                                                                                                           
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                    <tr>
                                                                                                                                                                        <td style="padding: 0px 2px;">
                                                                                                                                                                        </td>
                                                                                                                                                                    </tr>
                                                                                                                                                                </table>
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                            <td id="tduploadPdf" runat="server" visible="false">
                                                                                                                                                               
                                                                                                                                                            </td>
                                                                                                                                                        </tr>
                                                                                                                                                    </table>
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td>
                                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                        <tr>
                                                                                                                                                            <td>
                                                                                                                                                            </td>
                                                                                                                                                            <td></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr>
                                                                                                                                                            <td width="10%">
                                                                                                                                                               
                                                                                                                                                            </td>
                                                                                                                                                            <td width="9%">
                                                                                                                                                               
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%"></td>
                                                                                                                                                        </tr>
                                                                                                                                                        <tr runat="server" id="trvideodelete" visible="false">
                                                                                                                                                            <td align="right" width="10%">
                                                                                                                                                               
                                                                                                                                                            </td>
                                                                                                                                                            <td width="9%" align="center">
                                                                                                                                                               
                                                                                                                                                                <div style="display: none">
                                                                                                                                                                    
                                                                                                                                                                </div>
                                                                                                                                                            </td>
                                                                                                                                                            <td width="64%"></td>
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
                                                                                                                <tr>
                                                                                                                    <th style="line-height: 30px;">&nbsp;Upload Alternate Images</th>

                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Atl 1 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img  id="ImgAlt1" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td>
                                                                                                                                            <td valign="top" colspan="2">
                                                                                                                                                
                                                                                                                                                <table>
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                              Image Description :
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                        <td>
                                                                                                                                                         <asp:TextBox ID="txtalt1" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                                                                                                                            </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                              </td>
                                                                                                                                            
                                                                                                                                       
                                                                                                                                            
                                                                                                                                        </tr>

                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt1" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" Visible="false" />
                                                                                                                                                        </td>
                                                                                                                                                       
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                         <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt1" runat="server" AlternateText="Upload" OnClick="imgbtnAlt1_Click"
                                                                                                                                                                TabIndex="26" Visible="false" />
                                                                                                                                                              <asp:ImageButton ID="imgbtnAlt1del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt1del_Click" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                           
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td2" runat="server"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr class="altrow">
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Alt 2 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img  id="ImgAlt2" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td>
                                                                                                                                             <td valign="top" colspan="2">
                                                                                                                                                 <table>
                                                                                                                                                     <tr>
                                                                                                                                                         <td>
                                                                                                                                                          Image Description :
                                                                                                                                                             </td>
                                                                                                                                                     </tr>
                                                                                                                                                     <tr>
                                                                                                                                                         <td>
                                                                                                                                                               <asp:TextBox ID="txtalt2" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                                                                                                                         </td>
                                                                                                                                                     </tr>
                                                                                                                                                 </table>
                                                                                                                                                </td>
                                                                                                                                         

                                                                                                                                              
                                                                                                                                            
                                                                                                                                        </tr>


                                                                                                                                       

                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt2" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" Visible="false"/>
                                                                                                                                                        </td>
                                                                                                                                                        
                                                                                                                                                       
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                         <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt2" Visible="false" runat="server" AlternateText="Upload" OnClick="imgbtnAlt2_Click"
                                                                                                                                                                TabIndex="26" />
                                                                                                                                                             <asp:ImageButton ID="imgbtnAlt2del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt2del_Click" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                            
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td4" runat="server"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Alt 3 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img  id="ImgAlt3" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td>
                                                                                                                                         
                                                                                                                                                 <td valign="top" colspan="2">
                                                                                                                                                     <table>
                                                                                                                                                         <tr>
                                                                                                                                                             <td>
                                                                                                                                                                   Image Description :
                                                                                                                                                             </td>
                                                                                                                                                         </tr>
                                                                                                                                                         <tr>
                                                                                                                                                             <td>
                                                                                                                                                                  <asp:TextBox ID="txtalt3" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                                                                                                                             </td>
                                                                                                                                                         </tr>
                                                                                                                                                     </table>
                                                                                                                                                   </td>
                                                                                                                                            
                                                                                                                                        </tr>


                                                                                                                                 

                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt3" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" Visible="false" />
                                                                                                                                                        </td>
                                                                                                                                                       
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                         <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt3" Visible="false" runat="server" AlternateText="Upload" OnClick="imgbtnAlt3_Click"
                                                                                                                                                                TabIndex="26" />
                                                                                                                                                              <asp:ImageButton ID="imgbtnAlt3del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt3del_Click" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                           
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td6" runat="server"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr class="altrow">
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Alt 4 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img  id="ImgAlt4" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td>
                                                                                                                                             <td valign="top" colspan="2">
                                                                                                                                                 <table>
                                                                                                                                                     <tr>
                                                                                                                                                         <td>
                                                                                                                                                             Image Description :
                                                                                                                                                         </td>
                                                                                                                                                     </tr>
                                                                                                                                                     <tr>
                                                                                                                                                         <td>
                                                                                                                                                             <asp:TextBox ID="txtalt4" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                                                                                                                         </td>
                                                                                                                                                     </tr>
                                                                                                                                                 </table>
                                                                                                                                          
                                                                                                                                        </tr>


                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt4" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" Visible="false" />
                                                                                                                                                        </td>
                                                                                                                                                       
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                         <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt4" runat="server" AlternateText="Upload" OnClick="imgbtnAlt4_Click"
                                                                                                                                                                TabIndex="26" Visible="false" />
                                                                                                                                                              <asp:ImageButton ID="imgbtnAlt4del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt4del_Click" />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                           
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td8" runat="server"></td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                            <tr>
                                                                                                                                <td>
                                                                                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                        <tr>
                                                                                                                                            <td width="20%" valign="top">Alt 5 Image:
                                                                                                                                            </td>
                                                                                                                                            <td valign="middle">
                                                                                                                                                <div style="float: left;">
                                                                                                                                                    <img  id="ImgAlt5" src="/App_Themes/<%=Page.Theme %>/images/icon-image.gif"
                                                                                                                                                        runat="server" width="150" style="margin-bottom: 5px; border: 1px solid darkgray" />
                                                                                                                                                </div>

                                                                                                                                            </td >
                                                                                                                                             <td valign="top" colspan="2">
                                                                                                                                                 <table>
                                                                                                                                                     <tr>
                                                                                                                                                         <td>
                                                                                                                                                              Image Description :
                                                                                                                                                         </td>
                                                                                                                                                     </tr>
                                                                                                                                                     <tr>
                                                                                                                                                     <td>
                                                                                                                                                         <asp:TextBox ID="txtalt5" CssClass="order-textfield" Style="width: 300px; height: 60px;"
                                                                                                                                                    runat="server" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                                                                                                                     </td>
                                                                                                                                                         </tr>
                                                                                                                                                 </table>
                                                                                                                                                </td>
                                                                                                                                            
                                                                                                                                        </tr>


                                                                                                                                        <tr>
                                                                                                                                            <td>&nbsp;
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                                                                                    <tr>
                                                                                                                                                        <td width="10%">
                                                                                                                                                            <asp:FileUpload ID="flalt5" runat="server" Width="220px" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;"
                                                                                                                                                                TabIndex="25" Visible="false" />
                                                                                                                                                        </td>
                                                                                                                                                       
                                                                                                                                                    </tr>
                                                                                                                                                    <tr>
                                                                                                                                                         <td width="9%">
                                                                                                                                                            <asp:ImageButton ID="imgbtnAlt5" runat="server" AlternateText="Upload" OnClick="imgbtnAlt5_Click"
                                                                                                                                                                TabIndex="26" Visible="false" />
                                                                                                                                                              <asp:ImageButton ID="imgbtnAlt5del" runat="server" Visible="false" AlternateText="Delete"
                                                                                                                                                                OnClick="imgbtnAlt5del_Click"  />
                                                                                                                                                        </td>
                                                                                                                                                        <td width="64%">
                                                                                                                                                           
                                                                                                                                                        </td>
                                                                                                                                                    </tr>
                                                                                                                                                </table>
                                                                                                                                            </td>
                                                                                                                                        </tr>

                                                                                                                                    </table>
                                                                                                                                </td>
                                                                                                                                <td valign="bottom" id="Td10" runat="server"></td>
                                                                                                                                 <td align="center">
                                                          
                                                        </td>
                                                                                                                            </tr>
                                                                                                                            </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
             
                                                                                                        </table>
        <table style="width:100%;">
            <tr>
                <td align="Center">
                  <div style="width: 200px;display:none;">
                                                                <div style="margin-bottom: 1px; margin-top: 3px;">
                                                                  
                                                                    &nbsp;<asp:ImageButton ID="btnSave" runat="server" OnClientClick="if(ValidatePage()){return true;}else {return false;}"
                                                                        OnClick="btnSave_Click" TabIndex="38" />
                                                                    &nbsp;
                                                        <asp:ImageButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" TabIndex="39" />
                                                                </div>
                                                            </div>
                    </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
