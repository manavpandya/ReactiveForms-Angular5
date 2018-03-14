<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchControl.aspx.cs" Inherits="Solution.UI.Web.SearchControl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,700" rel="stylesheet" type="text/css" />
     
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/gallery.js"></script>
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jquery.jcarousel.pack.js"></script>
     <script type="text/javascript" src="/js/script.js"></script>
     <script type="text/javascript">
         function IndexValidation() {
             if (document.getElementById("txtFrom") != null && document.getElementById("txtFrom").value == "") {
                 alert("Please enter from price.");
                 document.getElementById("txtFrom").focus();
                 return false;
             }
             else if (document.getElementById("txtTo") != null && document.getElementById("txtTo").value == "") {
                 alert("Please enter to price.");
                 document.getElementById("txtTo").focus();
                 return false;
             }
             else {
                 var fromPrice = parseFloat(document.getElementById("txtFrom").value);
                 var toPrice = parseFloat(document.getElementById("txtTo").value);
                 if (parseFloat(fromPrice) > parseFloat(toPrice)) {
                     alert("Please enter valid price range.");
                     document.getElementById("txtFrom").focus();
                     return false;
                 }
                 else {
                     return true;
                 }
             }
             return true;
         }




         function CheckSelection() {
           //  document.getElementById("btnIndexPriceGo1").click();
         }

         function ColorSelection(clrvalue) {
             document.getElementById("hdnColorSelection").value = clrvalue;
           //  document.getElementById("btnIndexPriceGo1").click();
         }

         function CheckSelectionall() {
               document.getElementById("btnIndexPriceGo1").click();
         }

         function keyRestrict(e, validchars) {
             var key = '', keychar = '';
             key = getKeyCode(e);
             if (key == null) return true;
             keychar = String.fromCharCode(key);
             keychar = keychar.toLowerCase();
             validchars = validchars.toLowerCase();
             if (validchars.indexOf(keychar) != -1)
                 return true;
             if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27 || key == 46)
                 return true;
             return false;
         }
         function getKeyCode(e) {
             if (window.event)
                 return window.event.keyCode;
             else if (e)
                 return e.which;
             else
                 return null;
         }

         function unselectcheckbox(chkelement) {
             var allElts = document.getElementById('divPrice').getElementsByTagName('INPUT');
             var i;
             for (i = 0; i < allElts.length; i++) {
                 var elt = allElts[i];
                 if (elt.type == "checkbox") {
                     if (elt.name != chkelement) {
                         elt.checked = false;
                     }
                 }
             }
             CheckSelection();
         }

         function unselectcheckboxforCustom(chkelement) {
             var allElts = document.getElementById('divCustom').getElementsByTagName('INPUT');
             var i;
             for (i = 0; i < allElts.length; i++) {
                 var elt = allElts[i];
                 if (elt.type == "checkbox") {
                     if (elt.name != chkelement) {
                         elt.checked = false;
                     }
                 }
             }
             CheckSelection();
         }

     </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <div class="option-pro-main" id="hideIndexOptionDiv" runat="server">
        <div class="colors-box">
            <div class="option-probox-title">
                <span>Colors</span></div>
            <asp:Literal ID="ltrColor" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Header</span></div>
            <asp:Literal ID="ltrHeader" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Pattern</span></div>
            <asp:Literal ID="ltrPattern" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Fabric</span></div>
            <asp:Literal ID="ltrFabric" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Style</span></div>
            <asp:Literal ID="ltrStyle" runat="server"></asp:Literal>
        </div>
        <div class="pattern-box">
            <div class="option-probox-title">
                <span>Custom</span></div>
            <div class="toggle1" id="divCustom">
                <ul id="mycarousel6" class="jcarousel-skin-tango2">
                    <li>
                        <ul class="option-pro">
                            <li class="pattern-pro-box">
                                <input type="checkbox" class="checkbox" name="chkCustom_Yes" value="Yes" onchange="unselectcheckboxforCustom('chkCustom_Yes');"
                                    onclick="unselectcheckboxforCustom('chkCustom_Yes');" />
                                <span>Yes</span></li>
                            <li class="pattern-pro-box">
                                <input type="checkbox" class="checkbox" name="chkCustom_No" value="No" onchange="unselectcheckboxforCustom('chkCustom_No');"
                                    onclick="unselectcheckboxforCustom('chkCustom_No');" />
                                <span>No</span> </li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div class="price-box-main">
            <div class="option-probox-title">
                <span>Price</span>
            </div>
            <div class="toggle1" id="divPrice" style="height: 238px;">
                <div class="price-box-bg">
                    <ul>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_0" value="< 10" onchange="unselectcheckbox('chkPrice_0');" />
                            <span>Less than $10</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_1" value=">= 10 ~ <= 20" onchange="unselectcheckbox('chkPrice_1');"
                                onclick="unselectcheckbox('chkPrice_1');" />
                            <span>$10 to $20</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_2" value=">= 20 ~ <= 40" onchange="unselectcheckbox('chkPrice_2');"
                                onclick="unselectcheckbox('chkPrice_2');" />
                            <span>$20 to $40</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_3" value=">= 40 ~ <= 60" onchange="unselectcheckbox('chkPrice_3');"
                                onclick="unselectcheckbox('chkPrice_3');" />
                            <span>$40 to $60</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_4" value=">= 60 ~ <= 80" onchange="unselectcheckbox('chkPrice_4');"
                                onclick="unselectcheckbox('chkPrice_4');" />
                            <span>$60 to $80</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_5" value=">= 80 ~ <= 100"
                                onchange="unselectcheckbox('chkPrice_5');" onclick="unselectcheckbox('chkPrice_5');" />
                            <span>$80 to $100</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_6" value=">= 100 ~ <= 200"
                                onchange="unselectcheckbox('chkPrice_6');" onclick="unselectcheckbox('chkPrice_6');" />
                            <span>$100 to $200</span></li>
                        <li>
                            <input type="checkbox" class="checkbox" name="chkPrice_7" value="> 200" onchange="unselectcheckbox('chkPrice_7');"
                                onclick="unselectcheckbox('chkPrice_7');" />
                            <span>Greater then $200</span></li>
                    </ul>
                    <div class="qty-box">
                        <p>
                            $<span>
                                <asp:TextBox ID="txtFrom" runat="server" MaxLength="3" Width="28px" CssClass="qty-input"
                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                            </span>to $<span>
                                <asp:TextBox ID="txtTo" runat="server" MaxLength="3" Width="28px" CssClass="qty-input"
                                    onkeypress="return keyRestrict(event,'0123456789.');"></asp:TextBox>
                            </span>
                        </p>
                        <asp:ImageButton ID="btnIndexPriceGo" runat="server" ImageUrl="/images/go-button.jpg"
                            OnClientClick="return IndexValidation();" ToolTip="Go" OnClick="btnIndexPriceGo_Click" style="margin-top:10px !important;" />
                    </div>
                </div>
            </div>
        </div>
         
        <a href="#" id="toggle1" style="display:none;">
            <img src="/images/option-arrow-up.png" height="31" width="28" alt="arrow" id="option-arrow"></a>
    </div>
        <div style=" width:98%; float:left; margin:0; padding:0 1%; background:#E8E8E8; text-align:right; cursor:pointer;" onclick="CheckSelectionall();"><asp:ImageButton ID="btnIndexPriceGo1" runat="server" ImageUrl="/images/go.png" ToolTip="Go" OnClick="btnIndexPriceGo1_Click" /></div>
         <input type="hidden" id="hdnColorSelection" runat="server" value="" />
    </div>
    </form>
</body>
</html>
