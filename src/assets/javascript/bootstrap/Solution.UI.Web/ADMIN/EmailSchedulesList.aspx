<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="EmailSchedulesList.aspx.cs" Inherits="Solution.UI.Web.ADMIN.EmailSchedulesList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <link rel="Stylesheet" type="text/css" href="/css/jquery.datetimepicker.css" />
    <link rel="Stylesheet" type="text/css" href="/css/jquery.dataTables.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                <span style="vertical-align: middle; margin: 10px; float: right;">
                    <%--<asp:ImageButton runat="server" ID="btnApprove" ImageUrl="~/ADMIN/Images/add.jpg" ToolTip="Add" CommandName="Cmd_Add"></asp:ImageButton>--%>
                </span>

                &nbsp;
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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" class="add-product" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <th width="100%">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Product List" alt="Product List" src="/App_Themes/<%=Page.Theme %>/Images/list-product-icon.png" />
                                                    <h2>Add Schedule</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #E3E3E3; font-weight: bold;" colspan="3"></td>
                                        </tr>
                                        <tr class="even-row">
                                            <td style="padding-top: 0px; padding-bottom: 0px;">
                                                <table style="width: 100%; text-align: right">
                                                    <tr>
                                                        <td align="left" style="width: 5%">Day:</td>
                                                        <td align="left" style="width: 80%">
                                                            <asp:DropDownList ID="ddlDay" Width="210px" AutoPostBack="false" runat="server" class="order-list">
                                                                <asp:ListItem Text="-- Select Day --" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="Monday" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Tuesday" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="Wednesday" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="Thursday" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="Friday" Value="5"></asp:ListItem>
                                                                <asp:ListItem Text="Saturday" Value="6"></asp:ListItem>
                                                                <asp:ListItem Text="Sunday" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 5%">Time:</td>
                                                        <td align="left" style="width: 80%">
                                                            <asp:TextBox runat="server" ID="txtTime" CssClass="order-textfield" Width="210px" ClientIDMode="Static"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 5%">From Group:</td>
                                                        <td align="left" style="width: 80%">
                                                            <asp:DropDownList ID="ddlGroupName" Width="210px" AutoPostBack="false" runat="server" class="order-list"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 5%"></td>
                                                        <td align="left" style="width: 80%">
                                                            <asp:HiddenField ID="hidDailyScheduleID" runat="server" Value="0" />
                                                            <asp:ImageButton ID="imgUpdate" runat="server" AlternateText="Save" ToolTip="Save" OnClick="imgUpdate_Click" Visible="false" />
                                                            <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save" OnClick="imgSave_Click" />
                                                            <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel" OnClick="imgCancle_Click" />
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

            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Repeater ID="RptList" runat="server" OnItemDataBound="RptList_ItemDataBound" OnItemCommand="RptList_ItemCommand">
                            <HeaderTemplate>
                                <table id="RptList" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th>&nbsp</th>
                                            <th>&nbsp</th>
                                            <th>Day of the Week</th>
                                            <th>Time</th>
                                            <th>Group</th>
                                            <th>&nbsp</th>
                                            <th>&nbsp</th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tbody>
                                    <tr>
                                        <td><asp:Button ID="btnClone" Text="Clone" runat="server" CommandName="cmd_clone" CommandArgument='<%# Eval("DailyScheduleID") %>' message="Are you sure want to Clone?" OnClientClick='return confirm(this.getAttribute("message"))' /></td>
                                        <td><asp:Button ID="btnEdit" Text="Edit" runat="server"  CommandName="cmd_edit" CommandArgument='<%# Eval("DailyScheduleID") %>' /></td>
                                        <td><asp:Label ID="lblDayoftheWeek"  runat="server" Text='<%# Eval("DayoftheWeek") %>' ></asp:Label> </td>
                                        <td><asp:Label ID="lblStartTime"  runat="server" Text='<%# Eval("StartTime") %>'></asp:Label> </td>
                                        <td>
                                            <asp:Label ID="lblGroup"  runat="server" Text='<%# Eval("Label") %>'></asp:Label>
                                            <asp:HiddenField ID="hidGroupID" runat="server" Value='<%# Eval("TemplateID") %>' />
                                        </td>
                                        <td><asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="cmd_delete" CommandArgument='<%# Eval("DailyScheduleID") %>' message="Are you sure want to delete current Date?" OnClientClick='return confirm(this.getAttribute("message"))' /></td>
                                        <td><asp:Button ID="btnSend" Text="Send" runat="server" CommandName="cmd_send" CommandArgument='<%# Eval("DailyScheduleID") %>' /></td>
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
                    <td>&nbsp;
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

            jQuery('#txtTime').datetimepicker({
                datepicker: false,
                format: 'H:i',
                mask:false
            });

            CallDataTable();
        });




        function CallDataTable() {
            $('#RptList').dataTable({
                "oLanguage": {
                    "sEmptyTable": "No Schedule available"
                }
            });
        }
    </script>
</asp:Content>
