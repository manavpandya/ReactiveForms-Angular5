<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageBox.aspx.cs" Inherits="Solution.UI.Web.ADMIN.MessageBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
  <%--  <meta http-equiv="refresh" content="5">--%>
    <script type="text/javascript">
        function RefreshParent() {

            window.parent.location.href = window.parent.location.href;
        }</script>
</head>
<body style="background: white;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scr1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="grdmessagebox" runat="server" CssClass="dashboard-left " CellPadding="0"
                Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red" Visible="false"
                Style="margin-bottom: 0 !important;">
                <Columns>
                    <asp:TemplateField HeaderText="Recent Messages" ItemStyle-BorderStyle="None">
                        <ItemTemplate>
                            <b style="font-size: 11px">
                                <%# Eval("senderfname")%>
                                To
                                <%# Eval("receiverfname")%>
                            </b>
                            <br>
                            <span style="font-size: 12px">
                                <%# Eval("MessageText")%></span>
                            <br />
                            <span style="float: right; color: gray; padding-right: 2px">
                                <%# Eval("CreatedOn")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="even-row" />
                <HeaderStyle Height="26px" />
            </asp:GridView>
            <asp:Timer ID="Timer1" runat="server" Interval="3000" OnTick="Timer1_Tick">
            </asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
