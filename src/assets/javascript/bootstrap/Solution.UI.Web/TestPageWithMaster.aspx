<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="TestPageWithMaster.aspx.cs" Inherits="Solution.UI.Web.TestPageWithMaster" %>
    <%@ Register Assembly="Solution.Bussines.Components.PDFViewer" Namespace="Solution.Bussines.Components.PDFViewer" TagPrefix="viewer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function PrintBarcode() {
            if (document.getElementById('divBarcodePrint')) {
                var pri = document.getElementById("ifmcontentstoprint").contentWindow;
                pri.document.open();
                var contentAll = document.getElementById("divBarcodePrint").innerHTML;
                pri.document.write(contentAll);
                pri.document.close();
                pri.focus();
                pri.print();
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <a style="padding-left: 174px" href="javascript:void(0);" onclick="return PrintBarcode();">
        Print Barcode</a>
    <br />
    <div id="divBarcodePrint">
        <img id="ContentPlaceHolder1_imgOrderBarcode" src="/Resources/Barcodes/UPC-977775480947.png" />
        <asp:TextBox ID="txtId" runat="server"></asp:TextBox>
        <asp:Button ID="btnUpcupdate" runat="server" Text="Submit" 
            onclick="btnUpcupdate_Click" />
    </div>
    <div style="visibility: hidden">
        <iframe id="ifmcontentstoprint"></iframe>
    </div>

    <div>
<viewer:PDFViewer ID="pdfControl" FilePath="UPS-Packge1_1Z58F3E30396172082_126863_17793@201122614730-1.pdf"
FrameWidth="800" FrameHeight="500"
runat="server" BorderStyle="Inset" BorderWidth="2px" />
</div>

</asp:Content>
