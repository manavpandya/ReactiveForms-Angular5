<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="Category.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.category"
    Theme="Gray" ValidateRequest="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/tabs.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <%-- <link href="/css/general.css" rel="stylesheet" type="text/css" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .MasterTreeView {
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
            color: #727272;
        }

            .MasterTreeView a {
                text-decoration: none;
                color: #9b2414;
                word-wrap: break-word;
            }

                .MasterTreeView a:hover {
                    text-decoration: underline;
                    color: #9b2414;
                }

                .MasterTreeView a.active {
                    text-decoration: underline;
                    color: #000;
                }

        .MasterTreeViewnew td {
            padding: 0 2px;
        }

        #ContentPlaceHolder1_tvMasterCategoryList_0.active {
            color: #000;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#ordernotes11").click();

        });
        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            }
            else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/";
        }

        function openCenteredCrossSaleWindowDouble(x) {
            createCookie('prskus', document.getElementById(x).value, 1)
            var width = 900;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var StoreID = '<%=Request.QueryString["StoreID"]%>';
            var CategoryID = '<%=Request.QueryString["ID"]%>';
            var windowFeatures = "width=" + width + ",height=" + height + ",status,resizable,scrollbars=yes,left=" + left + ",top=" + top + "screenX=" + left + ",screenY=" + top;
            myWindow = window.open('ProductSkuByCategory.aspx?StoreID=' + StoreID + '&CategoryID=' + CategoryID + '&clientid=' + x, "subWind", windowFeatures);
        }

    </script>
    <script type="text/javascript">
        function ChkRootNode() {
            if (document.getElementById('ContentPlaceHolder1_tvCategoryn0CheckBox') && document.getElementById('ContentPlaceHolder1_tvCategoryn0CheckBox').checked == true) {
                document.getElementById('ContentPlaceHolder1_trShowonHeader').style.display = '';
            }
            else {
                document.getElementById('ContentPlaceHolder1_trShowonHeader').style.display = 'none';
            }
        }
        function changecanonical() {
            if (document.getElementById("ContentPlaceHolder1_chkcanonical").checked == true) {
                document.getElementById("ContentPlaceHolder1_txtcanonical").removeAttribute("readonly");

            }
            else {
                document.getElementById('ContentPlaceHolder1_txtcanonical').attr('readonly', 'true');
            }

        }
    </script>
    <script type="text/javascript">
        function ShowDiv(imgid, divid) {
            if (document.getElementById(imgid) != null) {
                var src = document.getElementById(imgid).src;
                if (src.indexOf('expand.gif') > -1) {
                    document.getElementById(imgid).src = src.replace('expand.gif', 'minimize.png');
                    document.getElementById(imgid).title = 'Minimize';
                    document.getElementById(imgid).alt = 'Minimize';
                    document.getElementById(imgid).style.marginTop = "4px";
                    document.getElementById(imgid).className = 'minimize';
                    if (document.getElementById(divid)) {
                        document.getElementById(divid).style.display = '';
                    }

                }
                else if (src.indexOf('minimize.png') > -1) {
                    document.getElementById(imgid).src = src.replace('minimize.png', 'expand.gif');
                    document.getElementById(imgid).title = 'Show';
                    document.getElementById(imgid).style.marginTop = "0px";
                    document.getElementById(imgid).alt = 'Show';
                    document.getElementById(imgid).className = 'close';
                    if (document.getElementById(divid)) {
                        document.getElementById(divid).style.display = 'none';
                    }

                }
            }
        }


        function Tabdisplay(id) {
            document.getElementById('ContentPlaceHolder1_hdnTabid').value = id;
            for (var i = 1; i <= 5; i++) {

                var divid = "divtab" + i.toString()
                var liid = "ContentPlaceHolder1_li" + i.toString()
                if (document.getElementById(divid) != null && ('divtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('ContentPlaceHolder1_li' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }

        }

        function iframeAutoheight(iframe) {
            var height = iframe.contentWindow.document.body.scrollHeight;
            iframe.height = height + 5;
        }
        function iframereload(iframe) {
            //chkHeight();
            document.getElementById(iframe).src = document.getElementById(iframe).src;

        }

    </script>
    <script type="text/javascript">
        function ChkRootNode() {

            var allElts = document.getElementById('ContentPlaceHolder1_tvCategory').getElementsByTagName('input');

            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];

                if (elt.type == "checkbox") {
                    if (document.getElementById('ContentPlaceHolder1_tvCategoryn0CheckBox').checked == true && elt.id != 'ContentPlaceHolder1_tvCategoryn0CheckBox') {
                        elt.checked = false;
                        elt.disabled = true;
                    }
                    else {
                        elt.disabled = false;
                    }
                }
            }
        }

    </script>
    <%--<asp:ScriptManager ID="sc1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>--%>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="float: left">
                <table cellpadding="0" cellspacing="0" align="left" style="margin-left: 0px; margin-top: 5px">
                    <tr>
                        <td>Store :
                        </td>
                        <td style="clear: left; float: left;">&nbsp;
                            <asp:DropDownList ID="ddlStore" onchange="chkHeight();" CssClass="order-list" runat="server"
                                AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" Width="180px">
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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5">
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
                                            <th colspan="2">
                                                <div class="main-title-left">
                                                    <img alt="Add Category" title="Add Category" class="img-left" src="/App_Themes/<%=Page.Theme %>/images/add-category-icon.png">
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Category" ID="lblTitle"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="padding-top: 14px; width: 260px;">
                                                <table cellpadding="0" cellspacing="0" width="100%" class="MasterTreeView" style="border: 1px solid #9B2414;">
                                                    <tr>
                                                        <td colspan="2" align="left" style="padding-left: 5px">
                                                            <a href="/Admin/Products/Category.aspx" onclick="chkHeight();">
                                                                <img alt="Add Category" title="Add Category" src="/App_Themes/<%=Page.Theme %>/images/add-category.png" /></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="left" style="border-bottom: 1px solid #e7e7e7; padding-left: 5px;">
                                                            <a href="/Admin/Products/Category.aspx?Root=1" onclick="chkHeight();">
                                                                <img alt="Add Root Category" title="Add Root Category" src="/App_Themes/<%=Page.Theme %>/images/add-root-category.png" /></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Search By :
                                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="order-list" Style="width: 163px;">
                                                                <asp:ListItem Value="Name">Category Name</asp:ListItem>
                                                                <asp:ListItem Value="ParentCatName">Parent Category Name</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 189px" valign="top">Search : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtSearch" runat="server"
                                                            CssClass="order-textfield" Width="110px"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <asp:ImageButton ID="ibtngo" OnClientClick="chkHeight();" runat="server" OnClick="ibtngo_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <asp:RadioButtonList ID="rdlCategoryStatus" AutoPostBack="true" onchange="chkHeight();"
                                                                CssClass="order-textfield" Style="border: none; width: 160px;" RepeatDirection="Horizontal"
                                                                runat="server" OnSelectedIndexChanged="rdlCategoryStatus_SelectedIndexChanged">
                                                                <asp:ListItem Value="All" Selected="True">All</asp:ListItem>
                                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                                <asp:ListItem Value="InActive">In Active</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr id="trCollexpand" runat="server">
                                                        <td align="center" colspan="2" style="border-bottom: 1px solid #e7e7e7;">
                                                            <asp:LinkButton ID="lnkCollapseAll" OnClientClick="chkHeight();" runat="server" OnClick="lnkCollapseAll_Click"><img src="/admin/images/collapsed.png" alt="" />&nbsp;Collapse All</asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="lnkExpandAll" OnClientClick="chkHeight();" runat="server" OnClick="lnkExpandAll_Click"><img src="/admin/images/expanded.png" alt="" />&nbsp;Expand All</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:TreeView ID="tvMasterCategoryList" ShowLines="true" CssClass="MasterTreeViewnew"
                                                                runat="server">
                                                            </asp:TreeView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top">
                                                <%--commented--%>
                                                <div id="tab-container-1">
                                                    <ul class="menu">
                                                        <li id="li1" runat="server" class="active" onclick="Tabdisplay(1);">GENERAL</li>
                                                        <li id="li2" class="" visible="false" onclick="Tabdisplay(2);iframereload('ContentPlaceHolder1_frmProducts');"
                                                            runat="server">PRODUCT</li>
                                                        <li id="li3" runat="server" class="" visible="false" onclick="Tabdisplay(3);chkHeight();iframereload('ContentPlaceHolder1_frmDisplayOrder');document.getElementById('prepage').style.display = 'none';">DISPLAY ORDER</li>
                                                        <li id="li4" runat="server" class="" visible="false" onclick="Tabdisplay(4);chkHeight();iframereload('ContentPlaceHolder1_frminventory');document.getElementById('prepage').style.display = 'none';">INVENTORY</li>
                                                        <li id="li5" runat="server" class="" visible="false" onclick="Tabdisplay(5);chkHeight();iframereload('ContentPlaceHolder1_frmprice');document.getElementById('prepage').style.display = 'none';">PRICE</li>
                                                    </ul>
                                                    <span class="clear"></span>
                                                    <div class="tab-content general-tab" style="margin-top: 1px; font-size: 12px;" id="divtab1">
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                                                            class="content-table">
                                                            <tr class="altrow">
                                                                <td align="right">
                                                                    <span class="star">*</span>Required Field
                                                                </td>
                                                            </tr>
                                                            <tr class="altrow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="even-row">
                                                                <td>
                                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                        <%--<tr class="oddrow">
                                                                            <td style="width: 15%">
                                                                                <span class="star">*</span> Store Name :
                                                                            </td>
                                                                            <td style="width: 80%; clear: left; float: left;">
                                                                                <asp:DropDownList ID="ddlStore" CssClass="order-list" runat="server" 
                                                                                    Width="30%" AutoPostBack="True" 
                                                                                    onselectedindexchanged="ddlStore_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>--%>
                                                                        <tr class="altrow">
                                                                            <td>
                                                                                <span class="star">*</span> Name :
                                                                            </td>
                                                                            <td style="clear: left; float: left;">
                                                                                <asp:TextBox ID="txtname" CssClass="order-textfield" MaxLength="500" runat="server"
                                                                                    Width="600px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top" class="oddrow">
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span>Short Title :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox CssClass="order-textfield" ID="txtShortTitle" runat="server" MaxLength="50"
                                                                                    Width="250px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top" class="altrow">
                                                                            <td style="width: 11%; clear: right">
                                                                                <span class="star">*</span> Parent :
                                                                            </td>
                                                                            <td class="treeview">
                                                                                <asp:TreeView ID="tvCategory" runat="server">
                                                                                </asp:TreeView>
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top" class="oddrow">
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span>Feature Template :
                                                                            </td>

                                                                            <td>
                                                                                <asp:DropDownList ID="ddlproductfeature" runat="server" class="product-type" Width="250px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>

                                                                        <tr class="altrow">
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span> Display Order :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox CssClass="order-textfield" onkeypress="return isNumberKey(event)" MaxLength="5"
                                                                                    ID="txtdisplayorder" runat="server" Width="10%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="oddrow">
                                                                            <td style="padding-top: 10px" valign="top">
                                                                                <span class="star">&nbsp;&nbsp;</span> Icon :
                                                                            </td>
                                                                            <td>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td valign="bottom">
                                                                                            <img alt="" id="imgIcon" width="241" runat="server" style="border: 1px solid darkgray" />
                                                                                        </td>
                                                                                        <td valign="bottom">
                                                                                            <asp:FileUpload ID="fuIcon" runat="server" />
                                                                                        </td>
                                                                                        <td valign="bottom">
                                                                                            <asp:ImageButton ID="ibtnUpload" runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/images/upload.gif"
                                                                                                OnClick="ibtnUpload_Click" OnClientClick="chkHeight();" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="oddrow">
                                                                            <td style="padding-top: 10px" valign="top">
                                                                                <span class="star">&nbsp;&nbsp;</span> Category Banner1 :
                                                                            </td>
                                                                            <td style="width: 36%;">
                                                                                <img id="imgBanner" runat="server" width="295" />
                                                                                <br />
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:FileUpload ID="fucategorybanner" runat="server" />&nbsp;<br />
                                                                                            <span style="font-size: 10px;">Size Should be: 785 X 295</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ibtnBanner" runat="server" onchange="chkHeight();" ImageUrl="/App_Themes/<%=Page.Theme %>/images/upload.gif"
                                                                                                OnClick="ibtnBanner_Click" />&nbsp;
                                                                                            <asp:ImageButton ID="imgdelete" runat="server" ImageUrl="/App_Themes/gray/images/delet.gif"
                                                                                                OnClick="imgdelete_Click" Visible="false" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <%--<td  colspan="2">
                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td style="width: 13%; padding-top: 10px" valign="top">
                                                                                            <span class="star">&nbsp;&nbsp;</span> Icon :
                                                                                        </td>
                                                                                        <td style="width: 35%;">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td valign="bottom">
                                                                                                        <img alt="" id="imgIcon" width="241" height="142" runat="server" style="border: 1px solid darkgray" />
                                                                                                    </td>
                                                                                                    <td valign="bottom">
                                                                                                        <asp:FileUpload ID="fuIcon" runat="server" />
                                                                                                    </td>
                                                                                                    <td valign="bottom">
                                                                                                        <asp:ImageButton ID="ibtnUpload" runat="server" ImageUrl="/App_Themes/<%=Page.Theme %>/images/upload.gif"
                                                                                                            OnClick="ibtnUpload_Click" OnClientClick="chkHeight();" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td style="width: 8%; display: none">
                                                                                            <span class="star">&nbsp;&nbsp;</span> Category Banner :
                                                                                        </td>
                                                                                        <td style="width: 36%; display: none">
                                                                                            <img id="imgBanner" runat="server" height="60" width="60" />
                                                                                            <br />
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:FileUpload ID="fucategorybanner" runat="server" />&nbsp;
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="ibtnBanner" runat="server" onchange="chkHeight();" ImageUrl="/App_Themes/<%=Page.Theme %>/images/upload.gif"
                                                                                                            OnClick="ibtnBanner_Click" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>--%>
                                                                        </tr>
                                                                        <tr class="oddrow">
                                                                            <td style="padding-top: 10px" valign="top">
                                                                                <span class="star">&nbsp;&nbsp;</span> Category Banner2 :
                                                                            </td>
                                                                            <td style="width: 36%;">
                                                                                <img id="imgBanner2" runat="server" width="295" />

                                                                                <br />
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:FileUpload ID="fucategorybanner2" runat="server" />&nbsp;<br />
                                                                                            <span style="font-size: 10px;">Size Should be: 785 X 295</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ibtnBanner2" runat="server" onchange="chkHeight();" ImageUrl="/App_Themes/<%=Page.Theme %>/images/upload.gif"
                                                                                                OnClick="ibtnBanner2_Click" />&nbsp;
                                                                                            <asp:ImageButton ID="imgdelete2" runat="server" ImageUrl="/App_Themes/gray/images/delet.gif"
                                                                                                OnClick="imgdelete2_Click" Visible="false" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="oddrow">
                                                                            <td style="padding-top: 10px" valign="top">
                                                                                <span class="star">&nbsp;&nbsp;</span> Category Banner3 :
                                                                            </td>
                                                                            <td style="width: 36%;">
                                                                                <img id="imgBanner3" runat="server" width="295" />

                                                                                <br />
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:FileUpload ID="fucategorybanner3" runat="server" />&nbsp;<br />
                                                                                            <span style="font-size: 10px;">Size Should be: 785 X 295</span>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:ImageButton ID="ibtnBanner3" runat="server" onchange="chkHeight();" ImageUrl="/App_Themes/<%=Page.Theme %>/images/upload.gif"
                                                                                                OnClick="ibtnBanner3_Click" />&nbsp;
                                                                                            <asp:ImageButton ID="imgdelete3" runat="server" ImageUrl="/App_Themes/gray/images/delet.gif"
                                                                                                OnClick="imgdelete3_Click" Visible="false" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="oddrow">
                                                                            <td valign="top" style="padding-top: 10px">
                                                                                <span class="star">&nbsp;&nbsp;</span> Header Title :
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox CssClass="order-textfield" ID="txtHeaderText" MaxLength="500" runat="server"
                                                                                    Width="69%"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 8%; padding-top: 10px" valign="top">
                                                                                <span class="star">&nbsp;&nbsp;</span> Header Text :
                                                                            </td>
                                                                            <td style="width: 36%;">
                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td class="ckeditor-table">
                                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtDescription"
                                                                                                Rows="10" Columns="80" runat="server" MaxLength="500" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                            <script type="text/javascript">
                                                                                                CKEDITOR.replace('<%= txtDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 150, width: 600 });
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
                                                                            <td style="width: 8%; padding-top: 10px" valign="top">
                                                                                <span class="star">&nbsp;&nbsp;</span> Banner Text :
                                                                            </td>
                                                                            <td style="width: 36%;">
                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td class="ckeditor-table">
                                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtbannertext"
                                                                                                Rows="10" Columns="80" runat="server" MaxLength="500" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                            <script type="text/javascript">
                                                                                                CKEDITOR.replace('<%= txtbannertext.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 150, width: 600 });
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

                                                                        <tr class="altrow">
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span>EFF Name :
                                                                            </td>
                                                                            <td style="clear: left; float: left;">
                                                                                <asp:TextBox ID="txteffname" CssClass="order-textfield" MaxLength="500" runat="server"
                                                                                    Width="600px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="altrow">
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span>EFF URL :
                                                                            </td>
                                                                            <td style="clear: left; float: left;">
                                                                                <asp:TextBox ID="txteffurl" CssClass="order-textfield" runat="server"
                                                                                    Width="600px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 8%; padding-top: 10px" valign="top">
                                                                                <span class="star">&nbsp;&nbsp;</span>EFF Header Text :
                                                                            </td>
                                                                            <td style="width: 36%;">
                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td class="ckeditor-table">
                                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtEFFDescription"
                                                                                                Rows="10" Columns="80" runat="server" MaxLength="500" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                            <script type="text/javascript">
                                                                                                CKEDITOR.replace('<%= txtEFFDescription.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 150, width: 600 });
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


                                                                        <tr valign="top" style="display: none;">
                                                                            <td style="width: 13%; padding-top: 10px" valign="top">
                                                                                <span class="star">&nbsp;&nbsp;</span>Footer Text :
                                                                            </td>
                                                                            <td style="width: 35%;">
                                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td class="ckeditor-table">
                                                                                            <asp:TextBox TextMode="multiLine" class="description-textarea" ID="txtSummary" Rows="10"
                                                                                                Columns="80" runat="server" MaxLength="500" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
                                                                                            <script type="text/javascript">
                                                                                                CKEDITOR.replace('<%= txtSummary.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 150, width: 600 });
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
                                                                        <%--      <tr>
                                                                                    <td colspan="2">
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border: 1px solid #DDDDDD;
                                                                                            line-height: 22px;" border="1px;">
                                                                                            <tr>
                                                                                                <th colspan="2" width="100%" style="color: #ffffff;">
                                                                                                    <strong style="float: left; padding-left: 12px;">SEO Detail</strong>
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr class="oddrow">
                                                                                                <td style="width: 13%;">
                                                                                                    <span class="star">&nbsp;&nbsp;</span> SETitle :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtsetitle" runat="server" Width="50%"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="altrow">
                                                                                                <td>
                                                                                                    <span class="star">&nbsp;&nbsp;</span> SEKeywords :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtsekeyword" runat="server" Width="50%"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="oddrow">
                                                                                                <td>
                                                                                                    <span class="star">&nbsp;&nbsp;</span> SEDescription :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtseodescription" runat="server" Width="50%"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr class="altrow">
                                                                                                <td>
                                                                                                    <span class="star">&nbsp;&nbsp;</span> Tooltip :
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txttooltip" runat="server" Width="50%"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>--%>


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
                                                                                            <td align="left">
                                                                                                <table cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td align="left">Is Canonical
                                                                                                        </td>
                                                                                                        <td align="left">
                                                                                                            <asp:CheckBox ID="chkcanonical" runat="server" onchange="changecanonical();" onclick="changecanonical();" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="left">Canonical URL
                                                                                                        </td>
                                                                                                        <td align="left">
                                                                                                            <asp:TextBox ID="txtcanonical" runat="server" class="order-textfield" Width="450px" TabIndex="5"></asp:TextBox>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>

                                                                                            </td>

                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td id="tdSEO">
                                                                                                <div id="tab-container" class="slidingDivSEO">
                                                                                                    <ul class="menu">
                                                                                                        <li class="active" id="ordernotes1" onclick='jQuery("#ordernotes1").addClass("active");jQuery("#privatenotes1").removeClass("active");jQuery("#giftnotes1").removeClass("active");jQuery("#myaccount1").removeClass("active");jQuery("div.order-notes").fadeIn();jQuery("div.private-notes").css("display", "none");jQuery("div.gift-notes").css("display", "none");jQuery("div.my-account").css("display", "none");'>PAGE TITLE</li>
                                                                                                        <li id="privatenotes1" onclick='jQuery("#ordernotes1").removeClass("active");jQuery("#privatenotes1").addClass("active"); jQuery("#giftnotes1").removeClass("active");jQuery("#myaccount1").removeClass("active");jQuery("div.private-notes").fadeIn();jQuery("div.order-notes").css("display", "none");jQuery("div.gift-notes").css("display", "none");jQuery("div.my-account").css("display", "none");'>KEYWORDS</li>
                                                                                                        <li id="giftnotes1" onclick='jQuery("#giftnotes1").addClass("active");jQuery("#privatenotes1").removeClass("active");jQuery("#ordernotes1").removeClass("active");jQuery("#myaccount1").removeClass("active");jQuery("div.gift-notes").fadeIn();jQuery("div.private-notes").css("display", "none");jQuery("div.order-notes").css("display", "none");jQuery("div.my-account").css("display", "none");'>DESCRIPTION</li>
                                                                                                        <%--<li id="giftooltip" onclick='jQuery("#giftooltip").addClass("active");jQuery("#giftnotes1").removeClass("active");jQuery("#privatenotes1").removeClass("active");jQuery("#ordernotes1").removeClass("active");jQuery("#myaccount1").removeClass("active");jQuery("div.gift-notes").fadeIn();jQuery("div.private-notes").css("display", "none");jQuery("div.order-notes").css("display", "none");jQuery("div.my-account").css("display", "none");'>
                                                                                                                    Tooltip</li>--%>
                                                                                                    </ul>
                                                                                                    <span class="clear"></span>
                                                                                                    <div class="tab-content-2 order-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtsetitle" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" MaxLength="500" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 private-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtsekeyword" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 gift-notes">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtseodescription" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
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
                                                                                                    <img class="img-left" title="EFF SEO" alt="EFF SEO" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                                    <h2>EFF SEO</h2>
                                                                                                </div>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td id="tdSEO1">
                                                                                                <div id="tab-container1" class="slidingDivSEO">
                                                                                                    <ul class="menu">
                                                                                                        <li class="active" id="ordernotes11" onclick='jQuery("#ordernotes11").addClass("active");jQuery("#privatenotes11").removeClass("active");jQuery("#giftnotes11").removeClass("active");jQuery("#myaccount11").removeClass("active");jQuery("div.order-notes1").fadeIn();jQuery("div.private-notes1").css("display", "none");jQuery("div.gift-notes1").css("display", "none");jQuery("div.my-account11").css("display", "none");'>PAGE TITLE</li>
                                                                                                        <li id="privatenotes11" onclick='jQuery("#ordernotes11").removeClass("active");jQuery("#privatenotes11").addClass("active"); jQuery("#giftnotes11").removeClass("active");jQuery("#myaccount11").removeClass("active");jQuery("div.private-notes1").fadeIn();jQuery("div.order-notes1").css("display", "none");jQuery("div.gift-notes1").css("display", "none");jQuery("div.my-account11").css("display", "none");'>KEYWORDS</li>
                                                                                                        <li id="giftnotes11" onclick='jQuery("#giftnotes11").addClass("active");jQuery("#privatenotes11").removeClass("active");jQuery("#ordernotes11").removeClass("active");jQuery("#myaccount11").removeClass("active");jQuery("div.gift-notes1").fadeIn();jQuery("div.private-notes1").css("display", "none");jQuery("div.order-notes1").css("display", "none");jQuery("div.my-account11").css("display", "none");'>DESCRIPTION</li>
                                                                                                        <%--<li id="giftooltip" onclick='jQuery("#giftooltip").addClass("active");jQuery("#giftnotes1").removeClass("active");jQuery("#privatenotes1").removeClass("active");jQuery("#ordernotes1").removeClass("active");jQuery("#myaccount1").removeClass("active");jQuery("div.gift-notes").fadeIn();jQuery("div.private-notes").css("display", "none");jQuery("div.order-notes").css("display", "none");jQuery("div.my-account").css("display", "none");'>
                                                                                                                    Tooltip</li>--%>
                                                                                                    </ul>
                                                                                                    <span class="clear"></span>
                                                                                                    <div class="tab-content-2 order-notes1">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtEFFsetitle" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" MaxLength="500" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 private-notes1">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtEFFsekeyword" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="tab-content-2 gift-notes1">
                                                                                                        <div class="tab-content-3">
                                                                                                            <asp:TextBox ID="txtEFFseodescription" runat="server" CssClass="order-textfield" Width="100%"
                                                                                                                Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="middle" class="altrow">
                                                                            <td>
                                                                                <span class="star">*</span> Status
                                                                            </td>
                                                                            <td>
                                                                                <asp:RadioButtonList ID="rblPublished" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="True" Selected="True">Yes</asp:ListItem>
                                                                                    <asp:ListItem Value="False">No</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span> Feature Category :
                                                                            </td>
                                                                            <td>&nbsp;<asp:RadioButton ID="rblIsfeatured" runat="server" Text="Yes" EnableViewState="true"
                                                                                GroupName="rdoFeature" />
                                                                                <asp:RadioButton ID="rblIsfeaturedNo" runat="server" Text="No" EnableViewState="true"
                                                                                    GroupName="rdoFeature" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="middle" class="altrow">
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span> Show On EFF
                                                                            </td>
                                                                            <td>
                                                                                <asp:RadioButtonList ID="rdoShowOnEff" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="True" Selected="True">Yes</asp:ListItem>
                                                                                    <asp:ListItem Value="False">No</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                         <% if (Request.QueryString["ID"] != null && Convert.ToInt32(Request.QueryString["ID"].ToString()) > 0)
                                                                           {%>
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span> Best Seller SKUs                                                                            
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBestSellerSKUs" TextMode="MultiLine" runat="server" CssClass="status-textfield"
                                                                                    Width="70%" TabIndex="32"></asp:TextBox>

                                                                                <a id="aOptAcc" name="aOptAcc" onclick="openCenteredCrossSaleWindowDouble('ContentPlaceHolder1_txtBestSellerSKUs');"
                                                                                    style="margin-right: 15px; font-weight: bold; cursor: pointer;" tabindex="34">Select Product(s) </a>
                                                                            </td>
                                                                        </tr>
                                                                        <%} %>
                                                                        <tr valign="middle" class="altrow" id="trSearsCatID" runat="server" visible="false">
                                                                            <td>
                                                                                <span class="star">*</span><asp:Label ID="lblSearsCategoryID" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSearsCategoryID" CssClass="order-textfield" runat="server" Width="150px"
                                                                                    onkeypress="return isNumberKey(event);"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr valign="top" class="altrow" id="trShowonHeader" runat="server" style="display: none;">
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span>&nbsp;Show&nbsp;on&nbsp;Item&nbsp;Header&nbsp;:
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkShowOnHeader" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                         <tr valign="top" class="altrow" >
                                                                            <td>
                                                                                <span class="star">&nbsp;&nbsp;</span>Faceted Page off:
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkfaceted" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                                    ImageUrl="/App_Themes/<%=Page.Theme %>/images/save.gif" OnClick="imgbtnSave_Click"
                                                                                    OnClientClick="return validation();" />&nbsp;&nbsp;
                                                                                <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                                    ImageUrl="/App_Themes/<%=Page.Theme %>/images/cancel.gif" OnClick="imgCancle_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div class="tab-content po-tab" id="divtab2" style="margin-top: 1px; display: none; font-size: 12px;">
                                                        <iframe id="frmProducts" runat="server" scrolling="no" frameborder="0" marginheight="0"
                                                            marginwidth="0" name="frmProducts" width="100%"></iframe>
                                                    </div>
                                                    <div class="tab-content DisplayOrder-tab" id="divtab3" style="margin-top: 1px; display: none; font-size: 12px;">
                                                        <iframe id="frmDisplayOrder" runat="server" scrolling="no" frameborder="0" marginheight="0"
                                                            marginwidth="0" name="frmDisplayOrder" width="100%"></iframe>
                                                    </div>
                                                    <div class="tab-content inventory-tab" id="divtab4" style="margin-top: 1px; display: none; font-size: 12px;">
                                                        <iframe id="frminventory" runat="server" scrolling="no" frameborder="0" marginheight="0"
                                                            marginwidth="0" name="frminventory" width="100%"></iframe>
                                                    </div>
                                                    <div class="tab-content price-tab" id="divtab5" style="margin-top: 1px; display: none; font-size: 12px;">
                                                        <iframe id="frmprice" runat="server" scrolling="no" frameborder="0" marginheight="0"
                                                            marginwidth="0" name="frmprice" width="100%"></iframe>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <%--commented--%>
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
    <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ibtnUpload" />
            <asp:PostBackTrigger ControlID="ibtnBanner" />
            <asp:PostBackTrigger ControlID="imgSave" />
            <asp:PostBackTrigger ControlID="imgCancle" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <script language="javascript" type="text/javascript">
        function validation() {

            var a = document.getElementById('<%=ddlStore.ClientID %>').value;
            if (a == "- Select Store -") {
                jAlert('Select Store!', 'Message');
                document.getElementById('<%=ddlStore.ClientID %>').focus();
                return false;
            }

            var b = document.getElementById('<%=txtname.ClientID %>').value;
            if (b == "") {
                jAlert('Enter Category Name!', 'Message');
                document.getElementById('<%=txtname.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%=txtSearsCategoryID.ClientID %>') != null && document.getElementById('<%=txtSearsCategoryID.ClientID %>').value == "") {
                var CatName = document.getElementById('<%=lblSearsCategoryID.ClientID %>').innerHTML;
                jAlert('Enter ' + CatName.toString(), 'Message');
                document.getElementById('<%=txtSearsCategoryID.ClientID %>').focus();
                return false;
            }

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
                //alert('Check Atleast One Record!');
                jAlert('Check at least One Category!', 'Message');
                return false;
            }

            return true;
        }

    </script>
    <script language="javascript" type="text/javascript">
        function vsble() {
            if (document.getElementById('frmProducts').style.display == '')
                document.getElementById('frmProducts').style.display = '';
            else
                document.getElementById('frmProducts').style.display = '';
        }
    </script>
    <script language="javascript" type="text/javascript">
        function vsbleDisplayOrder() {
            if (document.getElementById('frmDisplayOrder').style.display == '')
                document.getElementById('frmDisplayOrder').style.display = '';
            else
                document.getElementById('frmDisplayOrder').style.display = '';
        }
    </script>
    <script language="javascript" type="text/javascript">
        function vsblefrminventory() {
            if (document.getElementById('frminventory').style.display == '')
                document.getElementById('frminventory').style.display = '';
            else
                document.getElementById('frminventory').style.display = '';
        }
    </script>
    <script language="javascript" type="text/javascript">
        function vsblefrmprice() {
            if (document.getElementById('frmprice').style.display == '')
                document.getElementById('frmprice').style.display = '';
            else
                document.getElementById('frmprice').style.display = '';
        }
    </script>
    <div style="display: none;">
        <input type="hidden" id="hdnTabid" runat="server" value="1" />
        <%-- <input type="hidden" id="hdnAmount" runat="server" value="0" />--%>
    </div>
</asp:Content>
