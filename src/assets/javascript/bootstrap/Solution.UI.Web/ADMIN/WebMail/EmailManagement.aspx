<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="EmailManagement.aspx.cs" EnableViewState="true" Inherits="Solution.UI.Web.ADMIN.WebMail.EmailManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/general.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/jquery-1.3.2.js" type="text/javascript"></script>
   
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <link rel="stylesheet" href="/App_Themes/<%=Page.Theme %>/css/jqx.base.css"
        type="text/css" />
    <link rel="stylesheet" href="/App_Themes/<%=Page.Theme %>/css/jqx.summer.css"
        type="text/css" />
    <script type="text/javascript" src="/Admin/js/jqxcore.js"></script>
    <script type="text/javascript" src="/Admin/js/jqxbuttons.js"></script>
    <script type="text/javascript" src="/Admin/js/jqxsplitter.js"></script>
     <script src="/js/popup.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function splitterOnResize(width) {
            // do any other work that needs to happen when the 
            // splitter resizes. this is a good place to handle 
            // any complex resizing rules, etc.

            // make sure the width is in number format
            if (typeof width == "string") {
                width = new Number(width.replace("px", ""));
            }

        }
	
 
    </script>
    <script type="text/javascript">
        function CreateNewFolder() {
            if (document.getElementById("ifrmCreateNewFolder") != null) {
                document.getElementById("ifrmCreateNewFolder").src = "/Admin/WebMail/CreateNewFolder.aspx";
            }
            window.scrollTo(0, 0);
            document.getElementById('btnreadmore').click();
            return false;
        }
    </script>
    <script type="text/javascript">

        function checksearch() {
            if (document.getElementById('ContentPlaceHolder1_ddlsearchby') != null && document.getElementById('ContentPlaceHolder1_ddlsearchby').selectedIndex == 0) {
                jAlert('Please select search option.', 'Message', 'ContentPlaceHolder1_ddlsearchby');
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_txtsearchby') != null && document.getElementById('ContentPlaceHolder1_txtsearchby').value == '') {
                jAlert('Please enter search keyword.', 'Message', 'ContentPlaceHolder1_txtsearchby');
                return false;
            }
            return true;
        }

        function chkselect234() {
            var frame = document.getElementById('ContentPlaceHolder1_ifrmcontent');

            var checkboxes = frame.contentWindow.document.getElementsByTagName('input');
            document.getElementById('ContentPlaceHolder1_allids').value = '0';

            var jj = 0;
            for (var i = 0; i < checkboxes.length; i++) {
                var elt = checkboxes[i];
                if (elt.type == "checkbox" && elt.id.indexOf('chkSelect') > -1) {
                    if (checkboxes[i].checked == true) {
                        jj++;
                        var idss = checkboxes[i].id;
                        idss = idss.replace('chkSelect', '');
                        for (var j = 0; j < checkboxes.length; j++) {
                            var elth = checkboxes[j];
                            var test = elth.id.replace('lblMailIDhdn', '');

                            if (elth.type == "hidden" && elth.id.indexOf('lblMailIDhdn') > -1 && idss == test) {
                                document.getElementById('ContentPlaceHolder1_allids').value += ',' + elth.value;
                            }
                        }
                    }
                }
            }
            if (jj == 0) {
                alert('Please select at least one record');
                document.getElementById('ContentPlaceHolder1_ddlmoveto').selectedIndex = 0;
                return false;
            }
            __doPostBack('ctl00$ContentPlaceHolder1$ddlmoveto', '');
            return true;
        }

    </script>
    <script type="text/javascript">
        function treeNodeConfirmation(mEvent) {
            var o;
            // Internet Explorer   
            if (mEvent.srcElement) {
                o = mEvent.srcElement;
            }
            // Netscape and Firefox
            else if (mEvent.target) {
                o = mEvent.target;
            }

            if (o.tagName == 'A' || o.tagName == 'a') {
                if (o.innerHTML != 'Email Management') {

                    chkHeight();
                }
            }
            else if (o.tagName == 'B' || o.tagName == 'b') {

                chkHeight();
            }
        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
    </script>
    <style type="text/css" media="all">
        .MasterTreeView
        {
            font-family: Arial,Helvetica,sans-serif;
            font-size: 12px;
        }
        .MasterTreeView a
        {
            text-decoration: none !important;
            color: #707070 !important;
            word-wrap: break-word;
        }
        
        .MasterTreeView a:hover
        {
            text-decoration: underline !important;
            color: #707070 !important;
        }
        .MasterTreeView a.active
        {
            text-decoration: underline !important;
            color: #000;
        }
        .MasterTreeViewnew td
        {
            padding: 0 2px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var docwidth = $(document).width();
            var docheight = $(document).height();
            docwidth = docwidth - 36;
            docheight = docheight - 1120;
            $("#jqxSplitter").width(docwidth);
            $("#jqxSplitter").height(docheight);
            $("#jqxSplitter").jqxSplitter({ theme: 'summer', panels: [{ size: '350px' }, { size: '0px'}] });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%;">
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
                                            <th colspan="3">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Mail Log" alt="Mail Log" src="/App_Themes/<%=Page.Theme %>/icon/report_icon.gif" />
                                                    <h2>
                                                        Web Mail</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left" style="vertical-align: top; background-color: #f2f2f2; border: solid 1px #ddd;"
                                                colspan="2">
                                                <input type="hidden" id="allids" runat="server" value="0" />
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr id="spamoveto" style="vertical-align: middle;">
                                                        <td style="width: 65px;">
                                                            <asp:LinkButton ID="lnkCompose" runat="server" OnClick="lnkCompose_Click" Width="55"
                                                                Height="48" Style="background: url(/Admin/images/webmail_new.png) no-repeat scroll 0% 0% transparent;
                                                                width: 55px; height: 48px; border: 0pt none;"></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <a href="javascript:void(0);" id="button1" onclick="return CreateNewFolder();">
                                                                <img src="/Admin/images/webmail_new folder.png" width="74px" height="48px" border="0" /></a>
                                                        </td>
                                                        <td style="height: 24px; text-align: left;">
                                                            Search By :
                                                            <asp:DropDownList ID="ddlsearchby" runat="server" CssClass="order-list" Height="20px">
                                                                <asp:ListItem Value="Select">Select an Option</asp:ListItem>
                                                                <asp:ListItem Value="EmailID">Email ID</asp:ListItem>
                                                                <asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
                                                                <asp:ListItem Value="OrderNumber">Order Number</asp:ListItem>
                                                                <asp:ListItem Value="ZipCode">Zip Code</asp:ListItem>
                                                                <asp:ListItem Value="Subject">Subject</asp:ListItem>
                                                                <asp:ListItem Value="Body">Body</asp:ListItem>
                                                            </asp:DropDownList>
                                                            &nbsp;&nbsp;
                                                            <asp:TextBox ID="txtsearchby" runat="server" CssClass="textfield_small" Width="170px"></asp:TextBox>&nbsp;&nbsp;
                                                            <asp:Button ID="btnGo" runat="server" OnClientClick="return checksearch();" CssClass="button"
                                                                OnClick="btnGo_Click" Text="" Style="background: url(/Admin/images/webmail_search.png) no-repeat scroll 0% 0% transparent;
                                                                width: 32px; height: 32px; border: 0pt none; cursor: pointer;" Width="32px" Height="32px" />&nbsp;<asp:Button
                                                                    ID="btnShowAll" Visible="false" OnClientClick="javascript:if(document.getElementById('ctl00_ContentPlaceHolder1_txtsearchby')){document.getElementById('ctl00_ContentPlaceHolder1_txtsearchby').value='';} if(document.getElementById('ctl00_ContentPlaceHolder1_ddlsearchby')){document.getElementById('ctl00_ContentPlaceHolder1_ddlsearchby').selectedIndex=0;}if(document.getElementById('ctl00_ContentPlaceHolder1_ifrmcontent')){document.getElementById('ctl00_ContentPlaceHolder1_ifrmcontent').src=document.getElementById('ctl00_ContentPlaceHolder1_ifrmcontent').src;} return false;"
                                                                    runat="server" CssClass="button" OnClick="btnShowAll_Click" Text="" Style="background: url(/Admin/images/show_all_btn.gif) no-repeat scroll 0% 0% transparent;
                                                                    width: 59px; height: 22px; border: 0pt none;" Width="59px" Height="22px" />
                                                        </td>
                                                        <td style="height: 24px; width: 270px;" id="tdMoveto" valign="top">
                                                            <span style="float: right; margin-right: 20px; margin-top: 17px;">Move To:&nbsp;<asp:DropDownList
                                                                ID="ddlmoveto" runat="server" CssClass="order-list" Height="20px" Style="width: 180px;"
                                                                AutoPostBack="True" onchange="return chkselect234();" OnSelectedIndexChanged="ddlmoveto_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            </span>
                                                        </td>
                                                        <td style="text-align: right;">
                                                            <asp:LinkButton ID="btnrefresh" runat="server" OnClientClick="javascript:if(document.getElementById('ctl00_ContentPlaceHolder1_ifrmcontent')){document.getElementById('ctl00_ContentPlaceHolder1_ifrmcontent').src=document.getElementById('ctl00_ContentPlaceHolder1_ifrmcontent').src;} return false;"
                                                                OnClick="btnrefresh_Click" Style="background: url(/Admin/images/webmail_Refresh.png) no-repeat scroll 0% 0% transparent;
                                                                width: 50px; height: 48px; border: 0pt none;" Width="52" Height="22"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="oddrow">
                                            <td align="left" id="TDCategory" runat="server" valign="top" style="width: 22%; height: 100%;">
                                                <div id="jqxSplitter">
                                                    <div id="treeSelectedvalue" style="height: 780px; overflow: auto;">
                                                        <asp:TreeView ID="trvFolders" CssClass="MasterTreeView" runat="server" Width="150px"
                                                            Height="100%" PopulateNodesFromClient="True" ShowLines="true" OnSelectedNodeChanged="trvFolders_SelectedNodeChanged">
                                                        </asp:TreeView>
                                                        <div style="display: none">
                                                            <asp:Button ID="btnloadtreeagain" runat="server" Text="test" OnClick="btnloadtreeagain_Click" />
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <iframe frameborder="0" scrolling="no" height="820px" width="100%" id="ifrmcontent"
                                                            runat="server" src="EmailInboxmaster.aspx?ShowType=Inbox&ID=Inbox"></iframe>
                                                    </div>
                                                </div>
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
    </div>
    <%-- <VwdCms:SplitterBar runat="server" ID="vsbSplitter1" LeftResizeTargets="TDCategory;treeSelectedvalue"
        MinWidth="200" MaxWidth="700" BackgroundColor="transparent" BackgroundColorLimit="transparent"
        BackgroundColorHilite="transparent" BackgroundColorResizing="transparent" OnResize="splitterOnResize"
        Style="background-image: url(/Admin/Images/vsplitter.gif); background-position: center center;
        background-repeat: no-repeat;" />--%>
    <div style="display: none">
        <input type="button" id="btnreadmore" />
    </div>
    <div id="popupContact" style="z-index: 1000001; width: 500px; height: 400px;">
        <asp:ImageButton ID="popupContactClose" Style="position: relative; display: none;"
            ImageUrl="~/images/cancel.png" runat="server" ToolTip="Close" OnClientClick="return false;">
        </asp:ImageButton>
        <iframe id="ifrmCreateNewFolder" frameborder="0" width="500px" height="400px" scrolling="auto">
        </iframe>
    </div>
    <div id="backgroundPopup" style="z-index: 1000000;">
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 200px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
