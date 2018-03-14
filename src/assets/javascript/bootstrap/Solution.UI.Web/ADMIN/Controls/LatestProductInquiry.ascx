<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LatestProductInquiry.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.LatestProductInquiry" %>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-center">
        <tbody>
            <tr>
                <th colspan="7">
                    <div class="main-title-left">
                        <img class="img-left" title="Latest Product Inquiry" alt="Latest Product Inquiry"
                            src="/App_Themes/<%=Page.Theme %>/icon/shipment-delivery.png" />
                        <h2>
                            Latest Product Inquiry</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgProductInquiry','trProductInquiry','trProductInquiryView');">
                            <img class="minimize" id="imgProductInquiry" title="Minimize" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="trProductInquiry">
                <td colspan="8" align="left">
                    <asp:GridView ID="grdProductInquiry" runat="server" CssClass="dashboard-center" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" BorderStyle="Solid"
                        BorderColor="#DFDFDF" GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="even-row" EmptyDataRowStyle-ForeColor="Red" Style="margin-bottom: 0 !important;">
                        <Columns>
                            <asp:TemplateField HeaderText="Subject">
                                <ItemTemplate>
                                    <a href="javascript:void(0);" style="color: #212121;" onclick='<%# "javascript:window.open(\"/Admin/Reports/MailDetail.aspx?MID=" + DataBinder.Eval(Container.DataItem,"MailID")  +"\", \"\",\"height=600,width=820,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");" %> '>
                                        <%# DataBinder.Eval(Container.DataItem,"Subject") %>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="Left" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IP Address">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "IPAddress")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SentOn">
                                <ItemTemplate>
                                    <%#Convert.ToDateTime(Eval("MailDate")).ToString("MM/dd/yyyy")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" BorderWidth="0" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="even-row" />
                    </asp:GridView>
                </td>
            </tr>
            <tr id="trProductInquiryView">
                <td align="left" class="even-row" style="padding: 0pt;" colspan="3" id="tdviewReport"
                    runat="server">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="right" width="88%">
                                </td>
                                <td align="right">
                                    <a title="View List" href="/Admin/Reports/LatestProductInquiryList.aspx" style="cursor: pointer; color: #F93A21;
                                        font-size: 12px; font-weight: bold; text-decoration: none;"><span style="text-decoration: none;">
                                            View List</span></a>
                                    <img class="img-right" title="View List" alt="View List" src="/App_Themes/<%=Page.Theme %>/icon/view-report.png"
                                        style="float: left" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</div>
