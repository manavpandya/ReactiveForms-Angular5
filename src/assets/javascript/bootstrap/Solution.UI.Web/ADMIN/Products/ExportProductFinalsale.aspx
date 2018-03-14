<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ExportProductFinalsale.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ExportProductFinalsale" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>

    <script type="text/javascript" src="../../js/jquery-1.8.2.js"></script>
    <link href="../../css/jQuery.fancybox.css" rel="stylesheet" />
    <script type="text/javascript" src="../../js/jquery.elevatezoom.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.fancybox.pack.js"></script>
    <style type="text/css">
        .fancybox-wrap {
            top: 40px !important;
        }

        .fancybox-desktop {
            top: 40px !important;
        }

        .fancybox-type-iframe {
            top: 40px !important;
        }

        .fancybox-opened {
            top: 40px !important;
        }

        .btn-small > [class*="icon-"] {
            margin-right: 6px !important;
        }
    </style>
    <style type="text/css">
        .fancybox-wrap {
            top: 40px !important;
        }

        .fancybox-desktop {
            top: 40px !important;
        }

        .fancybox-type-iframe {
            top: 40px !important;
        }

        .fancybox-opened {
            top: 40px !important;
        }

        .textleft {
            text-align: right !important;
        }

        .fancybox-wrap {
            top: 40px !important;
            width: 1300px !important;
        }

        .fancybox-desktop {
            top: 40px !important;
            width: 1300px !important;
        }

        .fancybox-type-iframe {
            top: 40px !important;
            width: 1300px !important;
        }

        .fancybox-opened {
            top: 40px !important;
            width: 1300px !important;
        }

        .fancybox-outer {
            width: 1300px !important;
        }

        .fancybox-opened .fancybox-skin {
            width: 1300px !important;
        }

        .fancybox-skin {
            width: 1300px !important;
        }

        .fancybox-inner {
            width: 1300px !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('a.fancybox').fancybox();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order" style="width: 100%">
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
                                                <img class="img-left" title="Import Export FinalSale" alt="Import Export FinalSale" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png" />
                                                <h2>Import / Export for FinalSale Products</h2>
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
                                                    <tr class="even-row">
                                                        <td align="left">
                                                            <fieldset>

                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr class="oddrow">
                                                                        <td></td>
                                                                        <td>

                                                                            <asp:Button ID="btnexport1" OnClick="btnexport1_Click" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblImportCSv" runat="server" Text="Import CSV :"></asp:Label>
                                                                            <asp:FileUpload ID="uploadCSV" runat="server" Style="border: 1px solid #1a1a1a; background: #f5f5f5; color: #000000;" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Button ID="btnImport" runat="server" OnClick="btnImport_Click" /></td>

                                                                        <td style="margin-left: 950px; float: right;">
                                                                            <a style="color: red; text-decoration: none;" id="alog" runat="server" class="fancybox fancybox.iframe"
                                                                                href="/Admin/Products/ViewonsaleLog.aspx">
                                                                                <img src="/App_Themes/Gray/images/viewlog.png" /></a>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Literal ID="ltrmsg" runat="server"></asp:Literal>
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

        </table>
    </div>
</asp:Content>

