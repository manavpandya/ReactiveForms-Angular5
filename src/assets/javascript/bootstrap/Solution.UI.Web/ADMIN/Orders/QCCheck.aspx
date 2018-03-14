<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="QCCheck.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Orders.QCCheck" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        function Autosearch()
        {
         
            if (document.getElementById('ContentPlaceHolder1_txtUPC') != null && document.getElementById('ContentPlaceHolder1_txtUPC').value!='')
            {
                document.getElementById('ContentPlaceHolder1_btnGo').click();
            }
           
        }
        function vaslidation()
        {
            
            if (document.getElementById('ContentPlaceHolder1_txtUPC') != null && document.getElementById('ContentPlaceHolder1_txtUPC').value == '') {
                jAlert('Please enter Order ID / UPC.', 'Message', 'ContentPlaceHolder1_txtUPC');
                $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_txtUPC').offset().top }, 'slow');
                return false;
                
            }
            return true;
        }
    </script>
     <br />
                 <br />
     <div id="content-width">
          
             <div class="content-row1" style="margin-top:40px;">
               

            <table>
        <tr>
            <td>
                <asp:Label ID="lblUPC" runat="server" Text="Order ID / UPC : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtUPC" runat="server"  onchange="Autosearch();" ></asp:TextBox>
            </td>
            <td>
              
                <asp:Button ID="btnGo" runat="server" OnClientClick="return vaslidation();" OnClick="btnGo_Click"  />
            </td>
        </tr>
    </table>
        </div>
  
            <div class="content-row2">
                 <br />
              
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
             
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
                                                    <img class="img-left" title="QC Check" alt="QC Check" src="/App_Themes/<%=Page.Theme %>/images/topic-list-icon.png">
                                                    <h2>
                                                        QC Check</h2>
                                                </div>
                                            </th>
                                        </tr>
                                           
                                        <tr>
                                            <td>
                                                 <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
        <tr class="altrow">
            <td>
                <asp:Label ID="lblmsg" style="align-items:center;padding-left:700px;"  ForeColor="Red" runat="server" Text="No Record(s) Found."></asp:Label>
                <asp:GridView ID="gvQCCheck" runat="server" AutoGenerateColumns="False" 
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" 
                                                   OnRowDataBound="gvQCCheck_RowDataBound">
                                                          <Columns>
                                                             <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                              Product Name
                                                         
                                                            </HeaderTemplate>
                                                            <ItemTemplate >
                                                                 <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                                                <asp:Label  ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left"  VerticalAlign="Top"/>
                                                        </asp:TemplateField>
                                                              <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                              SKU
                                                         
                                                            </HeaderTemplate>
                                                            <ItemTemplate >
                                                                
                                                                <asp:Label  ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left"  VerticalAlign="Top"/>
                                                        </asp:TemplateField>
                                                              <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                              UPC
                                                         
                                                            </HeaderTemplate>
                                                            <ItemTemplate >
                                                               
                                                                <asp:Label  ID="lblUPC" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem,"UPC") %>'></asp:Label>
                                                            
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left"  VerticalAlign="Top"/>
                                                        </asp:TemplateField>
                                                            <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                              Image
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgName"  runat="server" />
                                                                   <asp:HiddenField ID="hdnImageName" Value='<%# Bind("ImageName") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                    </Columns>
                   
                     <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                </asp:GridView>

                     <asp:GridView ID="gvOrderDetails" runat="server" AutoGenerateColumns="False" 
                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" BorderStyle="Solid"
                                                    BorderColor="#E7E7E7" BorderWidth="1px" CellPadding="2" CellSpacing="1" GridLines="None"
                                                    Width="100%" 
                                                   OnRowDataBound="gvOrderDetails_RowDataBound">
                                                          <Columns>
                                                             <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                              Product Name
                                                         
                                                            </HeaderTemplate>
                                                            <ItemTemplate >
                                                                 <asp:HiddenField ID="hdnProductid" runat="server" Value='<%#Eval("ProductID") %>' />
                                                                <asp:Label  ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left"  VerticalAlign="Top"/>
                                                        </asp:TemplateField>
                                                              <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                              SKU
                                                         
                                                            </HeaderTemplate>
                                                            <ItemTemplate >
                                                                
                                                                <asp:Label  ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SKU") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left"  VerticalAlign="Top"/>
                                                        </asp:TemplateField>
                                                              <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                              UPC
                                                         
                                                            </HeaderTemplate>
                                                            <ItemTemplate >
                                                               
                                                                <asp:Label  ID="lblUPC" Text='<%# DataBinder.Eval(Container.DataItem,"UPC") %>' runat="server" ></asp:Label>
                                                            
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left"  VerticalAlign="Top"/>
                                                        </asp:TemplateField>
                                                                <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="center" />
                                                            <HeaderTemplate>
                                                             Ordered Quantity
                                                         
                                                            </HeaderTemplate>
                                                            <ItemTemplate >
                                                               
                                                                <asp:Label  ID="lblQuantity" Text='<%# DataBinder.Eval(Container.DataItem,"Quantity") %>'  runat="server" ></asp:Label>
                                                            
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center"  VerticalAlign="Top"/>
                                                        </asp:TemplateField>
                                                            <asp:TemplateField>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <HeaderTemplate>
                                                              Image
                                                               
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgName"  runat="server" />
                                                                <asp:HiddenField ID="hdnImageName" Value='<%# Bind("ImageName") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                    </Columns>
                   
                     <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                </asp:GridView>
                  <asp:HiddenField ID="hdnUPC"  runat="server" />
             
                
            </td>
        </tr>
    </table>
                                            </td>

                                        </tr>
                                        </table></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>

   
         </div>
         </div>

</asp:Content>
