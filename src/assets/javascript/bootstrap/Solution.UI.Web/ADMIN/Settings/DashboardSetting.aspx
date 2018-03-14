<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="DashboardSetting.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.DashboardSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/tabs.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        function Tabdisplay(id) {

            document.getElementById("ContentPlaceHolder1_hdnCurrentTab").value = id;

            for (var i = 1; i < 5; i++) {
                var divid = "divtab" + i.toString()
                var liid = "li" + i.toString()
                if (document.getElementById(divid) != null && ('divtab' + id == divid)) {
                    document.getElementById(divid).style.display = '';
                }
                else {
                    if (document.getElementById(divid) != null) {
                        document.getElementById(divid).style.display = 'none';
                    }
                }
                if (document.getElementById(liid) && ('li' + id == liid)) {
                    document.getElementById(liid).className = 'active';
                }
                else {
                    if (document.getElementById(liid) != null) {
                        document.getElementById(liid).className = '';
                    }
                }
            }
        }

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('prepage').style.height = windowHeight + 'px';
            document.getElementById('prepage').style.display = '';
        }
    </script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
    <script type="text/javascript">
        function SetDefault(id) {
            if (document.getElementById(id).value == '') {
                document.getElementById(id).value = 0;
            }
        }
    </script>
    <div class="content-row1" style="width: 100%">
        <table width="100%" style="margin-top: 4px;">
            <tr>
                <td align="left" style="width: 40px;">
                    Store :&nbsp;
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlstore" runat="server" Width="185px" AutoPostBack="true"
                        BorderColor="#BCC0C1" BorderStyle="Solid" BorderWidth="1" onchange="javascript:document.getElementById('ContentPlaceHolder1_hdnCurrentTab').value=1;chkHeight();">
                    </asp:DropDownList>
                    <input type="hidden" id="hdnCurrentTab" runat="server" value="1" />
                </td>
            </tr>
        </table>
    </div>
    <div class="content-row2">
        <table cellspacing="0" cellpadding="0" border="0" width="100%" style="background: none;">
            <tbody>
                <tr>
                    <th id="test" runat="server" colspan="2">
                        <div class="main-title-left">
                            <img class="img-left" title="Dashboard Configuration" alt="Dashboard Configuration"
                                src="/App_Themes/<%=Page.Theme %>/Images/dashboard-configuration-icon.png" />
                            <h2>
                                <asp:Label runat="server" Text="Dashboard Configuration" ID="lblTitle"></asp:Label></h2>
                        </div>
                    </th>
                </tr>
                <tr>
                    <td valign="top" style="background-color: White; padding: 10px 0 0 10px;">
                        Admin List :
                        <asp:DropDownList ID="ddlAdmins" runat="server" CssClass="order-list" Width="350px"
                            AutoPostBack="true" onchange="javascript:document.getElementById('ContentPlaceHolder1_hdnCurrentTab').value=1;chkHeight();"
                            OnSelectedIndexChanged="ddlAdmins_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td valign="top" align="right" style="background-color: White; padding: 10px 0 0 10px;display:none;">
                        <asp:Label ID="lblCloneHeader" runat="server" Text="Clone From :" Style="vertical-align: top;
                            line-height: 22px;"></asp:Label>
                        <asp:DropDownList ID="ddlCloneStore" runat="server" CssClass="order-list" Width="200px"
                            Style="vertical-align: top;">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnClone" runat="server" AlternateText="Clone" ToolTip="Clone"
                            CausesValidation="true" OnClick="btnClone_Click" OnClientClick="chkHeight();">
                        </asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <table width="100%" style="background-color: White">
                            <tr>
                                <td>
                                    <div id="tab-container-1">
                                        <ul class="menu">
                                            <li class="active" id="li1" onclick="Tabdisplay(1);">LEFT</li>
                                            <li id="li2" class="" onclick="Tabdisplay(2);">CENTER</li>
                                            <li id="li3" class="" onclick="Tabdisplay(3);">RIGHT</li>
                                        </ul>
                                        <span class="clear"></span>
                                        <div class="tab-content general-tab" style="margin-top: margin-top: 1px; margin-right: 10px;
                                            width: 97%; margin-left: 8px;" id="divtab1">
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:ObjectDataSource ID="_gridLeftControlObjectDataSource" runat="server" EnablePaging="True"
                                                            TypeName="Solution.Bussines.Components.DashboardComponent" SelectMethod="GetDataByFilterControl"
                                                            StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SelectCountMethod="GetCount">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddlstore" DbType="Int32" DefaultValue="" Name="pStoreId" />
                                                                <asp:ControlParameter ControlID="ddlAdmins" DbType="Int32" DefaultValue="0" Name="pAdminId" />
                                                                <asp:Parameter Name="Position" DbType="String" DefaultValue="Left" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:GridView ID="grdLeftControls" runat="server" AutoGenerateColumns="False" DataKeyNames="SettingID"
                                                            EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                            EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="50%"
                                                            BackColor="White" GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                            PagerSettings-Mode="NumericFirstLast" CellPadding="2" DataSourceID="_gridLeftControlObjectDataSource"
                                                            Style="background: none;">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" ItemStyle-Width="10%" Visible="false">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSettingID" runat="server" Text='<%#Eval("SettingID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Section Name" ItemStyle-Width="30%">
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                    <HeaderStyle HorizontalAlign="left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblsectionname" Text='<%#Eval("SectionName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Display" ItemStyle-Width="20%">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsDisplay" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsDisplay"))%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Display Order" ItemStyle-Width="20%">
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                    <HeaderStyle HorizontalAlign="left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtdisplaypos" runat="server" BorderColor="#BCC0C1" BorderStyle="Solid"
                                                                            BorderWidth="1" Style="text-align: center" Text='<%# Eval("DisplayPosition") %>'
                                                                            Width="70px" MaxLength="2" onkeypress="return isNumberKey(event)" onblur="SetDefault(this.id);"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Is Admin Allow" ItemStyle-Width="10%">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsAdminAllow" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsAdminAllow"))%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                            <HeaderStyle ForeColor="Red" Font-Bold="false" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="tab-content po-tab" id="divtab2" style="margin-top: 1px; margin-right: 10px;
                                            width: 97%; margin-left: 8px; display: none;">
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:ObjectDataSource ID="_gridCenterControlObjectDataSource" runat="server" EnablePaging="True"
                                                            TypeName="Solution.Bussines.Components.DashboardComponent" SelectMethod="GetDataByFilterControl"
                                                            StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SelectCountMethod="GetCount">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddlstore" DbType="Int32" DefaultValue="" Name="pStoreId" />
                                                                <asp:ControlParameter ControlID="ddlAdmins" DbType="Int32" DefaultValue="0" Name="pAdminId" />
                                                                <asp:Parameter Name="Position" DbType="String" DefaultValue="Center" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:GridView ID="grdCenterControls" runat="server" AutoGenerateColumns="False" DataKeyNames="SettingID"
                                                            EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                            EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="50%"
                                                            GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                            PagerSettings-Mode="NumericFirstLast" CellPadding="2" DataSourceID="_gridCenterControlObjectDataSource">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" ItemStyle-Width="10%" Visible="false">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSettingID" runat="server" Text='<%#Eval("SettingID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Section Name" ItemStyle-Width="30%">
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                    <HeaderStyle HorizontalAlign="left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblsectionname" Text='<%#Eval("SectionName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Display" ItemStyle-Width="20%">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsDisplay" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsDisplay"))%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Display Order" ItemStyle-Width="20%">
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                    <HeaderStyle HorizontalAlign="left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtdisplaypos" onkeypress="return isNumberKey(event)" MaxLength="2"
                                                                            runat="server" BorderColor="#BCC0C1" BorderStyle="Solid" BorderWidth="1" Style="text-align: center"
                                                                            Text='<%# Eval("DisplayPosition") %>' Width="70px" onblur="SetDefault(this.id);"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Is Admin Allow" ItemStyle-Width="10%">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsAdminAllow" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsAdminAllow"))%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                            <HeaderStyle ForeColor="Red" Font-Bold="false" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="tab-content invoice-tab" id="divtab3" style="margin-top: 1px; margin-right: 10px;
                                            width: 97%; margin-left: 8px; display: none;">
                                            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:ObjectDataSource ID="_gridRightControlObjectDataSource" runat="server" EnablePaging="True"
                                                            TypeName="Solution.Bussines.Components.DashboardComponent" SelectMethod="GetDataByFilterControl"
                                                            StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SelectCountMethod="GetCount">
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="ddlstore" DbType="Int32" DefaultValue="" Name="pStoreId" />
                                                                <asp:ControlParameter ControlID="ddlAdmins" DbType="Int32" DefaultValue="0" Name="pAdminId" />
                                                                <asp:Parameter Name="Position" DbType="String" DefaultValue="Right" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <asp:GridView ID="grdRightControls" runat="server" AutoGenerateColumns="False" DataKeyNames="SettingID"
                                                            EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                            EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="50%"
                                                            GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                            PagerSettings-Mode="NumericFirstLast" CellPadding="2" DataSourceID="_gridRightControlObjectDataSource">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select" ItemStyle-Width="10%" Visible="false">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSettingID" runat="server" Text='<%#Eval("SettingID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Section Name" ItemStyle-Width="30%">
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                    <HeaderStyle HorizontalAlign="left" />
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblsectionname" Text='<%#Eval("SectionName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Display" ItemStyle-Width="20%">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsDisplay" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsDisplay"))%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Display Order" ItemStyle-Width="20%">
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                    <HeaderStyle HorizontalAlign="left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtdisplaypos" onkeypress="return isNumberKey(event)" MaxLength="2"
                                                                            runat="server" Text='<%# Eval("DisplayPosition") %>' Width="70px" Style="text-align: center"
                                                                            BorderColor="#BCC0C1" BorderStyle="Solid" BorderWidth="1" onblur="SetDefault(this.id);"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Is Admin Allow" ItemStyle-Width="10%">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsAdminAllow" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsAdminAllow"))%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                            <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                            <HeaderStyle ForeColor="Red" Font-Bold="false" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <table align="center">
                                                        <tr>
                                                            <td>
                                                                <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                    CausesValidation="true" OnClick="btnSave_Click" OnClientClick="chkHeight();" />
                                                                <asp:ImageButton ID="btnCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                    CausesValidation="false" OnClick="btnCancel_Click" OnClientClick="chkHeight();" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="prepage" style="position: absolute; font-family: arial; font-size: 16; left: 0px;
        top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white;
        height: 100%; width: 100%; z-index: 1000; display: none;">
        <table width="100%" style="padding-top: 400px;" align="center">
            <tr>
                <td align="center" style="color: #fff;" valign="middle">
                    <img alt="" src="/images/loding.png" /><br />
                    <b>Loading ... ... Please wait!</b>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
