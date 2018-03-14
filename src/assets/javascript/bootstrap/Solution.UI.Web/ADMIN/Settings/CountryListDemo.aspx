<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="CountryListDemo.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.CountryListDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="content-row2">
    <table width="100%" style="font-family: Verdana; font-size: 12px;">
        <tr>
            <td>
                <br />
                <b>Content - Country List</b>
                <br />
                <br />
                <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                    TypeName="Solution.Bussines.Components.CountryComponent" SelectMethod="GetDataByFilter"
                    StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                    SelectCountMethod="GetCount">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdnCountry" DbType="String" DefaultValue="CountryID" Name="CName"  />
                        
                    </SelectParameters>
                </asp:ObjectDataSource>
               <asp:HiddenField ID="hdnCountry" runat="server" /> 
                <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" />
                <a href="Country.aspx">Add New Country</a>
                <asp:GridView ID="_CountryGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="CountryID"
                    EmptyDataText="There are no data for the current filter!" AllowSorting="True"
                    ViewStateMode="Enabled" OnRowCommand="_CountryGridView_RowCommand" Width="100%"
                    AllowPaging="True" PageSize="<%$appSettings:GridPageSize %>" PagerSettings-Mode="NumericFirstLast"
                    DataSourceID="_gridObjectDataSource">
                    <Columns>
                      
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="TwoLetterISOCode" HeaderText="Two Letter ISO Code" SortExpression="TwoLetterISOCode" />
                        <asp:BoundField DataField="ThreeLetterISOCode" HeaderText="Three Letter ISO Code" SortExpression="ThreeLetterISOCode" />
                  
                          <asp:TemplateField HeaderText="Operations" SortExpression="CountryID">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="_editLinkButton" ImageUrl="~/ADMIN/Images/Edit.gif"
                                    ToolTip="Edit" CommandName="edit" CommandArgument='<%# Eval("CountryID") %>'>
                                </asp:ImageButton>
                                <asp:ImageButton runat="server" ID="_deleteLinkButton" ImageUrl="~/ADMIN/Images/Delete.gif"
                                    ToolTip="Delete" CommandName="DeleteCountry" CommandArgument='<%# Eval("CountryID") %>'
                                    message="Are you sure that you want to delete the current Country?" OnClientClick='return confirm(this.getAttribute("message"))'>
                                </asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle HorizontalAlign="Center" />
                </asp:GridView>
            </td>
        </tr>
    </table>
    </div>
 
</asp:Content>
