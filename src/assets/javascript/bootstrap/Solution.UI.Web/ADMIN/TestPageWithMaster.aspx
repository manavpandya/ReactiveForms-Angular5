<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true"
    CodeBehind="TestPageWithMaster.aspx.cs" Inherits="Solution.UI.Web.ADMIN.TestPageWithMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function PrintBarcode() {
            if (document.getElementById('divBarcodePrint')) {
                var BrowserName = navigator.appName.toString();

                if (BrowserName.toString().toLowerCase().indexOf('internet explorer') > -1) {
                    w = window.open('', 'msg', 'directories=no, location=no, menubar=no, status=yes,titlebar=no,toolbar=no,scrollbars=no,resizable=no,Height=200,Width=300,left=0,top=0,visible=false,alwaysLowered=yes');
                    w.document.write(document.getElementById("divBarcodePrint").innerHTML);
                    w.document.close();
                    w.print();
                    w.close();
                }
                else {
                    var pri = document.getElementById("ifmcontentstoprint").contentWindow;
                    pri.document.open();
                    var contentAll = document.getElementById("divBarcodePrint").innerHTML;
                    pri.document.write(contentAll);
                    pri.document.close();
                    pri.focus();
                    pri.print();
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <a style="padding-left: 174px" href="javascript:void(0);" onclick="return PrintBarcode();">
        Print Barcode</a>
    <br />
    <div id="divBarcodePrint">
        <img id="ContentPlaceHolder1_imgOrderBarcode" src="/Resources/Barcodes/UPC-977775480947.png" />
    </div>
    <div style="visibility: hidden">
        <iframe id="ifmcontentstoprint"></iframe>
    </div>
</asp:Content>
