<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeoTagPage.aspx.cs" Inherits="Solution.UI.Web.SeoTagPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/js/hilitor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <script>

        function grvMailReportallcheckbox(id)
        {
             
            var count = 0;
            var allElts = document.getElementById("grvMailReport").getElementsByTagName('input');
            for (var i = 0; i < allElts.length; i++) {
                if (allElts[i].type == 'checkbox' && allElts[i].id.toString().toLowerCase() != id.toString().toLowerCase()) {
                    
                    allElts[i].checked = false;
                }
            }
            //if (count == 0) {
            //    alert('Select at least row !', 'Message');
            //    return false;
            //}
            //return true;

        }
        function grvMailReportapply(id) {

            var count = 0;
            var allElts = document.getElementById(id.replace('_btnApply_', '_chkSelect_'));
             
            if (allElts.checked == false) {
                alert('Please select row !');
                return false;
            }
             return true;

        }
        function checkvalidation()
        {
            if(document.getElementById("txtSearch").value == '')
            {
                alert('Please enter search keywords.');
                document.getElementById("txtSearch").focus();
                return false;
            }
            return true;
        }
        function checkapply(id) {
            if (document.getElementById(id).checked) {
                alert('Please select number of row.');
                return false;
            }
            return true;
        }
        function highlight(text) {
            inputText = document.getElementById("txtdescription")
            var innerHTML = inputText.innerHTML
            if (innerHTML.toLowerCase().indexOf(text.toLowerCase()) > -1)
            {
                var index = innerHTML.toLowerCase().indexOf(text.toLowerCase());
                var words = innerHTML.substring(index, index + text.length);

                var reg = new RegExp(words, "g");
                innerHTML = innerHTML.replace(reg, "<span style='background-color:yellow;'>" + words + "</span>");
                inputText.innerHTML = innerHTML;
            }
             
           
            //var searchedkeyword=  innerHTML.substring(index, index + text.length) = 

            //if (index >= 0) {
            //    innerHTML = innerHTML.substring(0, index) + "<span style='background-color:yellow;'>" + innerHTML.substring(index, index + text.length) + "</span>" + innerHTML.substring(index + text.length);
            //    inputText.innerHTML = innerHTML
            //}
            //var myHilitor; // global variable 
            //document.addEventListener("DOMContentLoaded", function () { myHilitor = new Hilitor(document.getElementById("txtdescription").innerHTML); myHilitor.apply(text); }, false);

        }
</script>
    <style>
.highlight
{
background-color:yellow;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Category:&nbsp;<asp:DropDownList ID="ddlcategory" runat="server" OnSelectedIndexChanged="ddlcategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList><br /><br />
            <asp:TextBox TextMode="multiLine" CssClass="description-textarea" ID="TextBox1" TabIndex="27" Rows="10" Columns="80" runat="server" Visible="false" Style="border: solid 1px #e7e7e7; background: none repeat scroll 0 0 #E7E7E7;"></asp:TextBox>
        </div>
        <div>Description: </div>
        <div id="txtdescription" runat="server" style="border:solid 1px #d7d7d7;">

             
                                                                                                                                                           
             
        </div>
        <div><br /><br /><asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="return checkvalidation();" /> </div>

        <div>
             <br /><br /><asp:GridView ID="grvMailReport" runat="server" AutoGenerateColumns="False" BorderStyle="Solid"
                                                        CellPadding="2" CellSpacing="1" GridLines="None" Width="50%" PageSize="50" BorderColor="#E7E7E7"
                                                        BorderWidth="1px" AllowPaging="false" EmptyDataText="No Record(s) Found." EmptyDataRowStyle-ForeColor="Red"
                                                        EmptyDataRowStyle-HorizontalAlign="Center" PagerSettings-Mode="NumericFirstLast" OnRowEditing="grvMailReport_RowEditing" OnRowDataBound="grvMailReport_RowDataBound">
                                                        <Columns>
                                                             <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Sr. #
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    
                                                                    <asp:Label ID="lblNo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"index") %>' ></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Select
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" onclick="grvMailReportallcheckbox(this.id);" />
                                                                    <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"index") %>' Visible="false" ></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Keywords
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtSearch" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Keywords") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Tag
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlTag" runat="server">
                                                                        <asp:ListItem Value="h1">H1</asp:ListItem>
                                                                        <asp:ListItem Value="h2">H2</asp:ListItem>
                                                                        <asp:ListItem Value="h3">H3</asp:ListItem>
                                                                        <asp:ListItem Value="h4">H4</asp:ListItem>
                                                                        <asp:ListItem Value="h5">H5</asp:ListItem>
                                                                        <asp:ListItem Value="h6">H6</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" CssClass="border" />
                                                            </asp:TemplateField>
                                                             
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Apply
                                                                </HeaderTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                     <asp:Button ID="btnApply" runat="server" Text="Apply" CommandName="Edit" OnClientClick="return grvMailReportapply(this.id);" />
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="border" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                        <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                        <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                        <AlternatingRowStyle CssClass="altrow" />
                                                        <HeaderStyle Font-Bold="false" />
                                                        <FooterStyle ForeColor="White" Font-Bold="true" BackColor="#545454" />
                                                    </asp:GridView>

        </div>
    </form>
</body>
</html>
