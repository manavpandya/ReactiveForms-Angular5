<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="WeAreOpenToday.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.WeAreOpenToday" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>


    <link rel="Stylesheet" type="text/css" href="/css/jquery.datetimepicker.css" />
    <link rel="Stylesheet" type="text/css" href="/css/jquery.dataTables.css" />


    <style type="text/css">
        .input {
        }

        .input-wide {
            width: 500px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
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
                                                    <img class="img-left" title="Add Admin" alt="Add Admin" src="/App_Themes/<%=Page.Theme %>/Images/admin-list-icon.png">
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Admin" ID="lblTitle"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                            <td align="left">
                                                
                                            </td>

                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr class="oddrow">
                                                        <td>
                                                            Start Date:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:TextBox runat="server" ID="date_timepicker_start" CssClass="order-textfield" ClientIDMode="Static" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            End Date:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="date_timepicker_end" CssClass="order-textfield" ClientIDMode="Static" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    
                                                    
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td style="width: 80%">
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
                    <td>
                        <asp:Repeater ID="RptList" runat="server" OnItemDataBound="RptList_ItemDataBound" OnItemCommand="RptList_ItemCommand">
                            <HeaderTemplate>
                                <table id="RptList" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th>Start Date</th>
                                            <th>End Date</th>
                                            <th>Operations</th>
                                        </tr>
                                    </thead>
                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tbody>
                                    <tr>
                                            <td><%# Eval("StartDate") %> </td>
                                            <td><%# Eval("EndDate") %> </td>
                                            <td><asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="DeleteDate"
                                                                    CommandArgument='<%# Eval("WearetodayID") %>' message="Are you sure want to delete current Date?"
                                                                    OnClientClick='return confirm(this.getAttribute("message"))'></asp:ImageButton></td>
                                        </tr>

                                </tbody>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>

                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>


    <script type="text/javascript" src="/admin/js/jquery.js"></script>
    <script type="text/javascript" src="/admin/js/jquery.datetimepicker.full.js"></script>
    <script type="text/javascript" src="/admin/js/jquery.dataTables.js"></script>
    <script type="text/javascript">


        jQuery(function () {
            jQuery('#date_timepicker_start').datetimepicker({
                format: 'm/d/Y H:i',
                mask: true,
                onShow: function (ct) {
                    this.setOptions({
                        minDate: 0,
                        minTime: 0

                    })
                },
            });
            jQuery('#date_timepicker_end').datetimepicker({
                format: 'm/d/Y H:i',
                mask: true,
                onShow: function (ct) {
                    var d = $('#date_timepicker_start').datetimepicker('getValue');
                    this.setOptions({
                        minDate: d ? d : false
                    })
                }

            });
            CallDataTable();
        });

        function CallDataTable() {
            $('#RptList').DataTable({
                "StartDate": [[0, "desc"]]
            });
        }
    </script>
</asp:Content>
