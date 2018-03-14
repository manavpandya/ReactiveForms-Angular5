<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="Solution.UI.Web.TestPage" %>

<%--  <%@ Register Assembly="Solution.Bussines.Components.PDFViewer" Namespace="Solution.Bussines.Components.PDFViewer" TagPrefix="viewer" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
         
        function onKeyPressPhone(e) {


            //            var key = window.event ? window.event.keyCode : e.which;
            //            alert(key);
            //            if (key == 32 || key == 102 || key == 39 || key == 37 || key == 46 || key == 13 || key == 8 || key == 9 || key == 189 || key == 109 || key == 0 || key == 40 || key == 41 || key == 45) {
            //                return key;
            //                
            //            }
            //            var keychar = String.fromCharCode(key);

            //            var reg = /\d/;
            //            if (window.event)
            //                return event.returnValue = reg.test(keychar);
            //            else
            //                return reg.test(keychar);
        }
        //        function sdfddgr() {
        //            setInterval("alert('test');", 1000);
        //            //            setInterval("alert('test');", 1000);
        // 
        //           setTimeout("alert('test1');", 1000);
        // 
        //        }
        function checkdatee() {
            var date1 = new Date(); //small
            var date2 = new Date("05/30/2012 12:00:00 AM");
            var sec = new Date(date2.getTime() - date1.getTime());
            alert(sec);
            var second = 1000, minute = 60 * second, hour = 60 * minute, day = 24 * hour;
            var days = Math.floor(sec / day);
            sec -= days * day;
            var hours = Math.floor(sec / hour);
            sec -= hours * hour;
            var minutes = Math.floor(sec / minute);
            sec -= minutes * minute;
            var seconds = Math.floor(sec / second);
            alert(days + " day" + (days != 1 ? "s" : "") + ", " + hours + " hour" + (hours != 1 ? "s" : "") + ", " + minutes + " minute" + (minutes != 1 ? "s" : "") + ", " + seconds + " second" + (seconds != 1 ? "s" : ""));


        }

        function tttt() {

            alert(moneyConvert('123466665.00'));

        }

        function moneyConvert(value) {

            var buf = "";

            var sBuf = "";

            var j = 0;

            value = String(value);



            if (value.indexOf(".") > 0) {

                buf = value.substring(0, value.indexOf("."));

            } else {

                buf = value;

            }

            if (buf.length % 3 != 0 && (buf.length / 3 - 1) > 0) {

                sBuf = buf.substring(0, buf.length % 3) + ",";

                buf = buf.substring(buf.length % 3);

            }

            j = buf.length;

            for (var i = 0; i < (j / 3 - 1); i++) {

                sBuf = sBuf + buf.substring(0, 3) + ",";

                buf = buf.substring(3);

            }

            sBuf = sBuf + buf;

            if (value.indexOf(".") > 0) {

                value = sBuf + value.substring(value.indexOf("."));
            }

            else {

                value = sBuf;

            }

            return value;

        }
        function toUSD(number) {
            var number = number.toString(),
    dollars = number.split('.')[0],
    cents = (number.split('.')[1] || '') + '00';
            dollars = dollars.split('').reverse().join('')
        .replace(/(\d{3}(?!$))/g, '$1,')
        .split('').reverse().join('');
            return '$' + dollars + '.' + cents.slice(0, 2);
        }

        function ChkRootNode() {

            var allElts = document.getElementById('ContentPlaceHolder1_tvCategory').getElementsByTagName('input');

            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];

                if (elt.type == "checkbox") {
                    if (document.getElementById('ContentPlaceHolder1_tvCategoryn0CheckBox').checked == true && elt.id != 'ContentPlaceHolder1_tvCategoryn0CheckBox') {
                        elt.checked = false;
                        elt.disabled = true;
                    }
                    else {
                        elt.disabled = false;
                    }
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmng" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
    </div>
    <div>
        Table name:<asp:TextBox ID="txtTablename" runat="server"></asp:TextBox>
        <asp:Button ID="btnSKU" runat="server" Text="Update SKU" OnClick="btnSKU_Click" />
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblTime" runat="server" Text="01 Hrs : 17 Mins : 56 Sec"></asp:Label>
                <asp:Timer ID="Timer1" Interval="1000" runat="server">
                </asp:Timer>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:TextBox ID="txtstdproId" runat="server" CssClass="textfild" Width="245px"></asp:TextBox>
    </div>
    <table width="100%">
        
        <tr>
            <td align="left">
                LNT:
                <asp:TextBox ID="TextBox1" runat="server" Text="1"></asp:TextBox>&nbsp;<asp:Button
                    ID="lntbtn" runat="server" Text="LNT Order" OnClick="lntbtn_Click" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <strong>Data List Custom Paging</strong>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:DataList runat="server" ID="dListItems" CellPadding="2">
                    <ItemTemplate>
                        <%#Eval("title") %>
                    </ItemTemplate>
                    <AlternatingItemStyle BackColor="Silver" />
                    <ItemStyle BackColor="White" />
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" border="0">
                    <tr>
                        <td align="right">
                            <asp:LinkButton ID="lbtnFirst" runat="server" CausesValidation="false" OnClick="lbtnFirst_Click">First</asp:LinkButton>
                            &nbsp;
                        </td>
                        <td align="right">
                            <asp:LinkButton ID="lbtnPrevious" runat="server" CausesValidation="false" OnClick="lbtnPrevious_Click">Previous</asp:LinkButton>&nbsp;&nbsp;
                        </td>
                        <td align="center" valign="middle">
                            <asp:DataList ID="dlPaging" runat="server" RepeatDirection="Horizontal" OnItemCommand="dlPaging_ItemCommand"
                                OnItemDataBound="dlPaging_ItemDataBound">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="Paging" Text='<%# Eval("PageText") %>'></asp:LinkButton>&nbsp;
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                        <td align="left">
                            &nbsp;&nbsp;<asp:LinkButton ID="lbtnNext" runat="server" CausesValidation="false"
                                OnClick="lbtnNext_Click">Next</asp:LinkButton>
                        </td>
                        <td align="left">
                            &nbsp;
                            <asp:LinkButton ID="lbtnLast" runat="server" CausesValidation="false" OnClick="lbtnLast_Click">Last</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center" style="height: 30px" valign="middle">
                            <asp:Label ID="lblPageInfo" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div>
        Category Listing
    </div>
    <div>
        <asp:Literal ID="ltrCategory" runat="server"></asp:Literal></div>
    <%--<viewer:PDFViewer ID="pdfControl" FilePath="UPS-Packge1_1Z58F3E30396172082_126863_17793@201122614730-1.pdf"
FrameWidth="800" FrameHeight="500"
runat="server" BorderStyle="Inset" BorderWidth="2px" />--%>
    <%--<iframe src="http://localhost/UPS-Packge1_1Z58F3E30396172082_126863_17793@201122614730-1.pdf" height="400px" width="300px" >
</iframe>--%>
    <script type="text/javascript" src="js/jquery-1.3.1.min.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#driver").click(function (event) {
                $('#stage').load('Yahoo-Post-Old.htm');
            });
        });
    </script>
    <p>
        Click on the button to load Yahoo-Post-Old.html file:</p>
    <div id="stage" style="background-color: blue;">
        STAGE
    </div>
    <input type="button" id="driver" value="Load Data" />
    ---------------------------- New --------------
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#driver").click(function (event) {
                var name = $("#name").val();
                $("#stage").load('/jquery/result.php', { "name": name });
            });
        });
    </script>
    <p>
        Enter your name and click on the button:</p>
    <input type="input" id="name" size="40" /><br />
    <div id="Div1" style="background-color: blue;">
        STAGE
    </div>
    <input type="button" id="Button1" value="Show Result" />
    <div id="divappnen">
    </div>
    </form>
</body>
</html>
