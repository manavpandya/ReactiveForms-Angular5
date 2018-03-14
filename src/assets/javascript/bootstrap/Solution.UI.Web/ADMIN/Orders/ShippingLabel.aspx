<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShippingLabel.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.Orders.ShippingLabel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../App_Themes/Gray/css/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function showLabel(msg) {

            document.getElementById('lblWaitMsg').innerHTML = msg;
            document.getElementById('lblWaitMsg').style.display = '';
        }
    </script>
    <script language="javascript" type="text/javascript">

        function chkboxChecked(inv, id) {

            if (inv <= 0) {
                alert("Cannot be shipped due to low inventory");
                document.getElementById(id).checked = false;
            }
           else if (inv <= 0) {
                alert("Cannot be shipped due to alredy shipped");
                document.getElementById(id).checked = false;
            }
        }
        function chklblgen(msg, id) {
            if (msg.toLowerCase() == 'yes') {
                alert("Shipping Label Already Generated");
                document.getElementById(id).checked = false;
            }
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
            if (key == null || key == 0 || key == 8 || key == 9 || key == 13 || key == 27)
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
        var allchecked = 1;
        function ToggleShipment(chkname) {
            if (allchecked == 0)
                allchecked = 1;
            else
                allchecked = 0;

            var items = document.getElementsByName(chkname);
            for (var i = 0; i < items.length; i++) {
                if (allchecked == 0)
                    items[i].childNodes[0].checked = false;
                else
                    items[i].childNodes[0].checked = true;
            }
        }
        function SetTotalWeight() {
            var totalWeight = 0;
            for (var i = 0; i < 20; i++) {

                        //if (document.getElementById('grdShipping_lblavailQuantity_' + i).innerHTML <= 0) {
                        //alert("Individual Shipment is not allow for : " + strUser);
                        //document.getElementById('grdShipping_chkAllowShip_' + i).checked = false;
                        //}

                if (document.getElementById('grdShipping_txtProWeight_' + i)) {
                    //alert(document.getElementById('grdShipping_chkAllowShip_' + i));lblQuantity
                    if ((!document.getElementById('grdShipping_chkAllowShip_' + i).checked) && document.getElementById('grdShipping_lblShipping_' + i).innerHTML.toLowerCase()=="no" )
                        totalWeight += parseFloat(document.getElementById('grdShipping_txtProWeight_' + i).value);
                }
            }
            document.getElementById('txtWeight').value = totalWeight.toFixed(2);
            document.getElementById('hfWeight').value = totalWeight.toFixed(2);
        }

        function CheckValues() {
    
            if (document.getElementById("ddlWareHouse") != null && document.getElementById("ddlWareHouse").selectedIndex == 0) {
                alert('Please Select Warehouse');
                document.getElementById('ddlWareHouse').focus();
                return false;
            }
           
            var totalWeight = parseFloat(document.getElementById('txtWeight').value);
            if (totalWeight > 0) {
                var tVariable = parseFloat(document.getElementById('txtHeight').value)
                if (tVariable == 0) {
                    alert('Please provide height for shipment');
                    document.getElementById('txtHeight').focus();
                    return false;
                }
                tVariable = parseFloat(document.getElementById('txtWidth').value)
                if (tVariable == 0) {
                    alert('Please provide Width for shipment');
                    document.getElementById('txtWidth').focus();
                    return false;
                }
                tVariable = parseFloat(document.getElementById('txtLength').value)
                if (tVariable == 0) {
                    alert('Please provide Length for shipment');
                    document.getElementById('txtLength').focus();
                    return false;
                }


                for (var i = 0; i < 20; i++) {
                    if (document.getElementById('grdShipping_txtProWeight_' + i)) {

                        if (document.getElementById('grdShipping_chkAllowShip_' + i).checked) {
                            tVariable = parseFloat(document.getElementById('grdShipping_txtProWeight_' + i).value)

                            if (tVariable == 0) {
                                alert('Please provide Weight for shipment');
                                document.getElementById('grdShipping_txtProWeight_' + i).focus();
                                return false;
                            }
                            tVariable = parseFloat(document.getElementById('grdShipping_txtHeightgrid_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Height for shipment');
                                document.getElementById('grdShipping_txtHeightgrid_' + i).focus();
                                return false;
                            }
                            tVariable = parseFloat(document.getElementById('grdShipping_txtWidthgrid_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Width for shipment');
                                document.getElementById('grdShipping_txtWidthgrid' + i).focus();
                                return false;
                            }
                            tVariable = parseFloat(document.getElementById('grdShipping_txtLengthgrid_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Length for shipment');
                                document.getElementById('grdShipping_txtLengthgrid_' + i).focus();
                                return false;
                            }
                        }
                    }
                }
            }
            else if (totalWeight == 0) {
                for (var i = 0; i < 20; i++) {
                    if (document.getElementById('grdShipping_txtProWeight_' + i)) {

                        if (document.getElementById('grdShipping_chkAllowShip_' + i).checked) {
                            tVariable = parseFloat(document.getElementById('grdShipping_txtProWeight_' + i).value)

                            if (tVariable == 0) {
                                alert('Please provide Weight for shipment');
                                document.getElementById('grdShipping_txtProWeight_' + i).focus();
                                return false;
                            }
                            tVariable = parseFloat(document.getElementById('grdShipping_txtHeightgrid_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Height for shipment');
                                document.getElementById('grdShipping_txtHeightgrid_' + i).focus();
                                return false;
                            }
                            tVariable = parseFloat(document.getElementById('grdShipping_txtWidthgrid_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Width for shipment' + i);
                                document.getElementById('grdShipping_txtWidthgrid_' + i).focus();
                                return false;
                            }
                            tVariable = parseFloat(document.getElementById('grdShipping_txtLengthgrid_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Length for shipment');
                                document.getElementById('grdShipping_txtLengthgrid_' + i).focus();
                                return false;
                            }
                        }
                    }
                }
            }


            showLabel('Please wait while getting Shipping Details from USPS, UPS and FEDEX Sites...');
        }

        function leadingZeros(num, totalChars, padWith) {
            num = num + "";
            padWith = (padWith) ? padWith : "0";
            if (num.length < totalChars) {
                while (num.length < totalChars) {
                    num = padWith + num;
                }
            } else { }

            if (num.length > totalChars) { //if padWith was a multiple character string and num was overpadded
                num = num.substring((num.length - totalChars), totalChars);
            } else { }
            return num;
        }
    </script>
    <style type="text/css">
        .confirm-dialog
        {
            width: 887px;
            position: relative;
            top: 0;
        }
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        #popUpPanel
        {
            top: 10% !important;
        }
    </style>
    <script type="text/javascript">
        function CheckValuesPop() {
  
            if (document.getElementById("ddlWareHouse") != null && document.getElementById("ddlWareHouse").selectedIndex == 0) {
                alert('Please Select Warehouse');
                document.getElementById('ddlWareHouse').focus();
                return false;
            }
            var ddlwarecount = document.getElementById("ddlWareHouse");
            
            if (ddlwarecount.length <= 2) {
                alert('Other Warehouse not available to compare rates');
            
            }

            var totalWeight = parseFloat(document.getElementById('txtWeight').value);
            if (totalWeight > 0) {
                var tVariable = parseFloat(document.getElementById('txtHeight').value)
                if (tVariable == 0) {
                    alert('Please provide height for shipment');
                    document.getElementById('txtHeight').focus();
                    return false;
                }
                tVariable = parseFloat(document.getElementById('txtWidth').value)
                if (tVariable == 0) {
                    alert('Please provide Width for shipment');
                    document.getElementById('txtWidth').focus();
                    return false;
                }
                tVariable = parseFloat(document.getElementById('txtLength').value)
                if (tVariable == 0) {
                    alert('Please provide Length for shipment');
                    document.getElementById('txtLength').focus();
                    return false;
                }
                for (var i = 0; i < 20; i++) {
                    if (document.getElementById('grdShipping_txtProWeight_' + i)) {
                        if (document.getElementById('grdShipping_chkAllowShip_' + i).checked) {
                            tVariable = parseFloat(document.getElementById('grdShipping_txtProWeight_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Weight for shipment');
                                document.getElementById('grdShipping_txtProWeight_' + i).focus();
                                return false;
                            }
                            tVariable = parseFloat(document.getElementById('grdShipping_txtHeightgrid_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Height for shipment');
                                document.getElementById('grdShipping_txtHeightgrid_' + i).focus();
                                return false;
                            }
                            tVariable = parseFloat(document.getElementById('grdShipping_txtWidthgrid_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Width for shipment');
                                document.getElementById('grdShipping_txtWidthgrid' + i).focus();
                                return false;
                            }
                            tVariable = parseFloat(document.getElementById('grdShipping_txtLengthgrid_' + i).value)
                            if (tVariable == 0) {
                                alert('Please provide Length for shipment');
                                document.getElementById('grdShipping_txtLengthgrid_' + i).focus();
                                return false;
                            }
                        }
                    }
                }
            }
            document.getElementById("btnHdn").click();
        }


    </script>
</head>
<body style="background: none;">
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <table cellspacing="0" cellpadding="0" border="0" width="100%">
        <tbody>
            <tr>
                <td class="border-td-sub">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="content-table">
                        <tbody>
                            <tr>
                                <th>
                                    <div class="main-title-left">
                                        <h2 style="padding-left: 0px;">
                                            Shipping Label</h2>
                                    </div>
                                </th>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="content_box_border">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td align="center" class="border">
                                    <table width="100%" cellpadding="0" cellspacing="0" class="product_table">
                                        <tr>
                                            <td>
                                                <table width="99%" cellpadding="0" cellspacing="0" align="center">
                                                    <tr valign="top">
                                                        <td style="width: 50%; text-align: left;">
                                                            <span style="color: #9B2414">Warehouse</span>
                                                            <asp:DropDownList ID="ddlWareHouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlWareHouse_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 50%; text-align: left;">
                                                        </td>
                                                    </tr>
                                                    <tr valign="top">
                                                        <td style="width: 50%; text-align: left;">
                                                            <asp:Literal ID="ltShippingFrom" runat="server"></asp:Literal>
                                                        </td>
                                                        <td style="width: 50%; text-align: left;">
                                                            <br />
                                                            <asp:Literal ID="ltShippingTo" runat="server"></asp:Literal>
                                                        </td>
                                                        <td style="width: 20%; padding-top: 5px;">
                                                            <a href="javascript:void(0);" onclick="return CheckValuesPop();" visible="true">
                                                                <div style="display: none">
                                                                    <%--   <asp:Button ID="btnHdn" runat="server" Text="Compare Warehouse" OnClick="btnHdn_Click" />--%>
                                                                    <asp:ImageButton ID="btnHdn" ImageUrl="/App_Themes/<%=Page.Theme %>/button/compare-rates.png"
                                                                        runat="server" OnClick="btnHdn_Click" /></div>
                                                                <img src="/App_Themes/<%=Page.Theme %>/button/compare-rates.png" title="Compare Warehouse rates" />
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                                            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup"
                                                                PopupControlID="popUpPanel" CancelControlID="btnclose" BackgroundCssClass="modalBackground">
                                                            </cc1:ModalPopupExtender>
                                                            <asp:Panel ID="popUpPanel" CssClass="confirm-dialog" runat="server" Style="display: none;
                                                                overflow: auto; top: 0;">
                                                                <table width="887px" border="0" cellspacing="0" cellpadding="3" style="border: 1px solid #444;
                                                                    font-size: 12px; font-family: Arial,Helvetica,sans-serif; height: 320px;">
                                                                    <tr style="background-color: #696969; height: 25px;">
                                                                        <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                                                                            font-weight: bold;">
                                                                            &nbsp;Warehouses Rates
                                                                        </td>
                                                                        <td align="right" valign="top">
                                                                            <asp:ImageButton ID="btnclose" Style="position: relative;" ImageUrl="/App_Themes/Gray/images/cancel-icon.png"
                                                                                runat="server" ToolTip="Close"></asp:ImageButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="background-color: White">
                                                                        <td colspan="2" valign="top">
                                                                            <%--<table width="100%">
                                                                                <tr>
                                                                                    <td style="width: 50%; vertical-align: top;">
                                                                                        <table width="100%">
                                                                                            <tr style="background-color: #696969; height: 25px;">
                                                                                                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                                                                                                    font-weight: bold;">
                                                                                                    &nbsp;Warehouse 1
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Repeater runat="server" ID="rdRadioForShipping1">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label runat="server" ID="lblShippingMethod" Text='<%#Eval("ShippingMethodName") %>'></asp:Label><br />
                                                                                                        </ItemTemplate>
                                                                                                    </asp:Repeater>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td style="width: 50%; vertical-align: top;">
                                                                                        <table width="100%">
                                                                                            <tr style="background-color: #696969; height: 25px;">
                                                                                                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                                                                                                    font-weight: bold;">
                                                                                                    &nbsp;Warehouse 2
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 50%; vertical-align: top;">
                                                                                        <table width="100%">
                                                                                            <tr style="background-color: #696969; height: 25px;">
                                                                                                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                                                                                                    font-weight: bold;">
                                                                                                    &nbsp;Warehouse 3
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td style="width: 50%; vertical-align: top;">
                                                                                        <table width="100%">
                                                                                            <tr style="background-color: #696969; height: 25px;">
                                                                                                <td valign="middle" align="left" style="color: #fff; font-family: Arial,Helvetica,sans-serif;
                                                                                                    font-weight: bold;">
                                                                                                    &nbsp;Warehouse 4
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>--%>
                                                                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="100%" align="center" style="font-size: 13px">
                                                                            <asp:Label ID="invMsg" runat="server" Visible="false"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 15px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px;">
                                                <input type="hidden" id="hdnproductid" runat="server" value="-1" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%" align="center" style="font-size: 13px">
                                                <asp:Label ID="warehouseinventoryMsg" runat="server" Visible="false" ForeColor="#FF3300"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="grdShipping" runat="server" Width="100%" AutoGenerateColumns="false"
                                                    OnRowDataBound="grdShipping_RowDataBound" ShowFooter="false" OnRowCommand="grdShipping_RowCommand"
                                                    CssClass="table-noneforOrder" EmptyDataText="No Record Found!" CellSpacing="0"
                                                    CellPadding="5" border="1" Style="width: 100%; border-collapse: collapse;">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Package Id">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPackageId" runat="server" Text='<%#Eval("PackageId") %>'></asp:Label>
                                                                <asp:Label ID="lblSCartID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippingCartID") %>'
                                                                    Visible="false"> </asp:Label>
                                                                <asp:Label ID="lblProductID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProductId") %>'
                                                                    Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:ImageButton ID="btnAddProduct" runat="server" CommandName="AddProduct" ValidationGroup="AddProduct"
                                                                    ImageUrl="../images/add.jpg" />
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="center" />
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <HeaderStyle Width="50px" HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                SKU
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblShippingCartID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ShippingCartID") %>'></asp:Label>
                                                                <asp:Label ID="lblSKU" runat="server" Text='<%#Eval("SKU")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAddSKU" runat="server" Style="border: 1px solid gray; padding-left: 5px;
                                                                    width: 62px;"></asp:TextBox><asp:RequiredFieldValidator ID="reqSKU" runat="server"
                                                                        Display="Dynamic" ControlToValidate="txtAddSKU" ForeColor="Red" ErrorMessage="*"
                                                                        ValidationGroup="AddProduct"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <HeaderStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Product Name
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductName" runat="server" Text='<%#Eval("Name").ToString()%>'></asp:Label>
                                                                <asp:Label ID="lblVstore" Visible="false" runat="server" Text=''></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAddProdName" runat="server" Columns="33" Style="border: 1px solid gray;
                                                                    padding-left: 5px;"></asp:TextBox><asp:RequiredFieldValidator ID="reqProdName" runat="server"
                                                                        Display="Dynamic" ControlToValidate="txtAddProdName" ForeColor="Red" ErrorMessage="*"
                                                                        ValidationGroup="AddProduct"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Qty
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAddQuantity" runat="server" onKeyPress="return keyRestrict(event,'0123456789.')"
                                                                    Style="text-align: center; border: 1px solid gray; padding-left: 5px; width: 15px"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqQuantity" runat="server" ControlToValidate="txtAddQuantity" ForeColor="Red"
                                                                        Display="Dynamic" ErrorMessage="*" ValidationGroup="AddProduct"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <FooterStyle HorizontalAlign="center" />
                                                            <HeaderStyle Width="30px" HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Available Qty
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblavailQuantity" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <FooterStyle HorizontalAlign="center" />
                                                            <HeaderStyle Width="30px" HorizontalAlign="center" />
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Shipping Generated
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblShipping" runat="server" Text='<%#Eval("LabelGenerated") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <FooterStyle HorizontalAlign="center" />
                                                            <HeaderStyle Width="30px" HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtHeightgrid" onKeyPress="return keyRestrict(event,'0123456789.')"
                                                                    MaxLength="5" runat="server" Style="border: 1px solid gray; padding-left: 5px;"
                                                                    Text='<%#Eval("Height").ToString().Length>0?Eval("Height"):"0" %>' Columns="3"></asp:TextBox>
                                                                X
                                                                <asp:TextBox ID="txtWidthgrid" onKeyPress="return keyRestrict(event,'0123456789.')"
                                                                    MaxLength="5" runat="server" Style="border: 1px solid gray; padding-left: 5px;"
                                                                    Columns="3" Text='<%#Eval("Width").ToString().Length>0?Eval("Width"):"0" %>'></asp:TextBox>
                                                                X
                                                                <asp:TextBox ID="txtLengthgrid" onKeyPress="return keyRestrict(event,'0123456789.')"
                                                                    MaxLength="5" runat="server" Style="border: 1px solid gray; padding-left: 5px;"
                                                                    Text='<%#Eval("Length").ToString().Length>0?Eval("Length"):"0" %>' Columns="3"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                Dimensions<br />
                                                                (H x W x L)
                                                            </HeaderTemplate>
                                                            <FooterStyle HorizontalAlign="center" />
                                                            <HeaderStyle HorizontalAlign="center" Width="197px" />
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAddHeight" runat="server" onkeypress="return keyRestrict(event,'0123456789.')"
                                                                    Style="border: 1px solid gray; padding-left: 5px;" Columns="3"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqHeight" runat="server" ControlToValidate="txtAddHeight" ForeColor="Red"
                                                                        ErrorMessage="*" Display="Dynamic" ValidationGroup="AddProduct"></asp:RequiredFieldValidator>
                                                                X
                                                                <asp:TextBox ID="txtAddWidth" runat="server" onkeypress="return keyRestrict(event,'0123456789.')"
                                                                    Style="border: 1px solid gray; padding-left: 5px;" Columns="3"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqWidth" runat="server" ControlToValidate="txtAddWidth" ErrorMessage="*"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="AddProduct"></asp:RequiredFieldValidator>
                                                                X
                                                                <asp:TextBox ID="txtAddLength" runat="server" onkeypress="return keyRestrict(event,'0123456789.')"
                                                                    Style="border: 1px solid gray; padding-left: 5px;" Columns="3"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqLength" runat="server" ControlToValidate="txtAddLength" ErrorMessage="*"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="AddProduct"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Weight">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtProWeight" autocomplete="off" onKeyPress="var ret=keyRestrict(event,'0123456789.');SetTotalWeight();return ret;"
                                                                    onblur="SetTotalWeight();" MaxLength="5" runat="server" Style="border: 1px solid gray;
                                                                    padding-left: 5px; width: 38px;" Text='<%# (Convert.ToDecimal(Eval("Weight").ToString().Length>0?Eval("Weight"):"0")*Convert.ToInt32(Eval("Quantity").ToString().Length>0?Eval("Quantity"):"0")).ToString("f2")%>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAddWeight" runat="server" onkeypress="return keyRestrict(event,'0123456789.')"
                                                                    Style="border: 1px solid gray; padding-left: 5px; width: 38px;"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqWeight" runat="server" ControlToValidate="txtAddWeight" ErrorMessage="*"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="AddProduct"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <HeaderStyle HorizontalAlign="center" Width="7%" />
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Price">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtProductPrice" runat="server" Text='<%# Convert.ToDecimal(Eval("SalePrice").ToString().Length>0?Eval("SalePrice"):"0").ToString("f2") %>'
                                                                    Style="border: 1px solid gray; padding-left: 5px; width: 50px;" Columns="5"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAddProduct" runat="server" onkeypress="return keyRestrict(event,'0123456789.')"
                                                                    Style="border: 1px solid gray; padding-left: 5px; width: 38px;" Columns="5"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqPrice" runat="server" ControlToValidate="txtAddProduct" ForeColor="Red"
                                                                        Display="Dynamic" ErrorMessage="*" ValidationGroup="AddProduct"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <HeaderStyle HorizontalAlign="center" Width="7%" />
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <a style="text-decoration: underline; color: Black;" href="javascript:ToggleShipment('Insured')">
                                                                    Insured</a>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkInsured" runat="server" name="Insured" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="center" Width="3%" />
                                                            <ItemStyle HorizontalAlign="center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="10%">
                                                            <HeaderTemplate>
                                                                <a style="text-decoration: underline; color: Black;" href="javascript:ToggleShipment('AllowShipment')">
                                                                    Individual Shipment</a>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkAllowShip" runat="server" name="AllowShipment" onclick="SetTotalWeight();" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="center" Width="3%" />
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle CssClass="oddrow" Height="30px" HorizontalAlign="Left" />
                                                    <EditRowStyle CssClass="altrow" />
                                                    <PagerStyle CssClass="altrow" HorizontalAlign="Right" />
                                                    <HeaderStyle Height="20px" HorizontalAlign="Left" Wrap="True" VerticalAlign="middle"
                                                        BackColor="gray" />
                                                    <AlternatingRowStyle CssClass="altrow" />
                                                    <PagerSettings Position="TopAndBottom" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 15px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 7px; background-color: Gray;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left; padding-left: 10px;">
                                                <div style="float: left;">
                                                    Package Id:
                                                    <asp:Label ID="lblPackageIDFP" runat="server"></asp:Label><br />
                                                    <br />
                                                    Total Weight:
                                                    <asp:TextBox ID="txtWeight" runat="server" onKeyPress="return keyRestrict(event,'0123456789.')"
                                                        Style="border: 1px solid gray; padding-left: 5px; width: 60px; background-color: #DFECFF;"></asp:TextBox>
                                                    <asp:HiddenField ID="hfWeight" runat="server" />
                                                    LBS &nbsp;&nbsp;&nbsp; Extra Charge:
                                                    <asp:TextBox ID="txtExtra" runat="server" onKeyPress="return keyRestrict(event,'0123456789.')"
                                                        Style="border: 1px solid gray; padding-left: 5px; width: 60px;" AutoPostBack="true"
                                                        Text="0" OnTextChanged="txtExtra_TextChanged"></asp:TextBox>
                                                    $&nbsp;&nbsp;&nbsp; (H x W x L):
                                                    <asp:TextBox ID="txtHeight" runat="server" onKeyPress="return keyRestrict(event,'0123456789.')"
                                                        Style="border: 1px solid gray; padding-left: 5px; width: 60px;" Text="0" MaxLength="6"></asp:TextBox>
                                                    x
                                                    <asp:TextBox ID="txtWidth" runat="server" onKeyPress="return keyRestrict(event,'0123456789.')"
                                                        Style="border: 1px solid gray; padding-left: 5px; width: 60px;" Text="0" MaxLength="6"></asp:TextBox>
                                                    x
                                                    <asp:TextBox ID="txtLength" runat="server" onKeyPress="return keyRestrict(event,'0123456789.')"
                                                        Style="border: 1px solid gray; padding-left: 5px; width: 60px;" Text="0" MaxLength="6"></asp:TextBox>
                                                    <br />
                                                    <br />
                                                    Need Delivery Confirmation with Signature:
                                                    <asp:CheckBox ID="chkMail" runat="server" ToolTip="Send a mail notification when the UPS shipping label generate." />
                                                </div>
                                                <div style="float: right; padding-right: 10px;">
                                                    <asp:Button ID="btnshipmethod" Style="background: url(../images/Get-Available-Shipping-Methods.jpg);
                                                        width: 199px; height: 23px; border: 0" runat="server" OnClientClick="return CheckValues();"
                                                        OnClick="btnshipmethod_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 5px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 7px; background-color: Gray;">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 8px;">
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblshiperror" runat="server" Text="" ForeColor="red"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblWaitMsg" runat="server" ForeColor="green"></asp:Label>
                                </td>
                            </tr>
                            <asp:Panel ID="panSelectMethod" runat="server" Visible="false">
                                <tbody>
                                    <tr>
                                        <td style="height: 5px;">
                                        </td>
                                    </tr>
                                    <tr id="MsgShippingMethods" runat="server">
                                        <td style="padding: 10px;">
                                            <table>
                                                <tr style="font-weight: bold;">
                                                    <td style="width: 470px">
                                                        Shipping Methods:
                                                    </td>
                                                    <td style="width: 25px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" colspan="3" style="padding-right: 10px;">
                                                        <asp:RadioButtonList ID="rdRadioForShipping" runat="server" Width="100%" RepeatColumns="3"
                                                            CssClass="rdRadioForShipping">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 1px; background-color: Gray;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; padding-left: 10px;">
                                            <div id="panweight" runat="server" style="float: left">
                                                <asp:ImageButton ID="btnGetLabel" runat="server" AlternateText="Get Label" ImageUrl="~/Admin/images/generate-label.jpg"
                                                    OnClientClick="if(document.getElementById('txtWeight').value=='' || document.getElementById('txtWeight').value=='0'){alert('Please Enter Weight.'); return false;} else {showLabel('Please wait while getting Shipping Label from USPS, UPS and FEDEX Sites...');} "
                                                    OnClick="btnGetLabel_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 5px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 1px; background-color: Gray;">
                                        </td>
                                    </tr>
                            </asp:Panel>
                            <tr>
                                <td style="height: 18px; text-align: center;">
                                    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="red"></asp:Label></br>
                                    <asp:Label ID="ltUSPSShippingLabel" runat="server" Text="" ForeColor="red"></asp:Label>
                                </td>
                            </tr>
                            <asp:Panel ID="panUSPS" runat="server" Visible="false">
                                <tr>
                                    <td>
                                        <span style="color: #9B2414; font-weight: bold;">USPS Shipping Label</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdUSPS" runat="server" Width="100%" AutoGenerateColumns="false"
                                            OnRowCommand="grdUSPS_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        PackageId
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPackageID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "PackageID").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Label Url
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbluspsimgurl" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "ImgUrl").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Tracking#
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrackingNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "TrackingNo").ToString()%>'
                                                            Visible="false"></asp:Label>
                                                        <%#  DataBinder.Eval(Container.DataItem, "TrackingNo").ToString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Create Date
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%#  DataBinder.Eval(Container.DataItem, "CreateDate").ToString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Download">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="bntDownUSPS" runat="server" CommandName="Download" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ImgUrl") %>'>
                                                             <img src="../images/download.png" alt="Download" style="border:0px;" height="20px" title="Download Label" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mail">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSendMail" runat="server" CommandName="SendMail" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"TrackingNo") + "-" + DataBinder.Eval(Container.DataItem,"ShippingCartID")+"-" + DataBinder.Eval(Container.DataItem,"PackageID") %>'
                                                            OnClientClick="if(!confirm('This action will send informtion of the USPS Shipping to Customer.')) return false;">
                                            <img src="../images/send_mail.png" alt="SendMail" style="border: 0px;" height="20px" title="Send Mail To Customer" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDelUSPS" runat="server" CommandName="Dellabel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ImgUrl") %>'
                                                            OnClientClick="if(!confirm('This action will Delete the Shipping-Label.')) return false;">
                                            <img src="../images/delete-icon.gif" alt="Delete" style="border: 0px;" height="20px" title="Delete" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="oddrow" Height="30px" HorizontalAlign="Left" />
                                            <EditRowStyle CssClass="altrow" />
                                            <PagerStyle CssClass="altrow" HorizontalAlign="Right" />
                                            <HeaderStyle Height="28px" HorizontalAlign="Left" Wrap="True" VerticalAlign="middle"
                                                BackColor="gray" Font-Size="12px" />
                                            <AlternatingRowStyle CssClass="altrow" />
                                            <PagerSettings Position="TopAndBottom" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr runat="server" id="trUSPSAll">
                                    <td style="padding-right: 5px; padding-top: 4px; text-align: right;">
                                        <asp:LinkButton ID="btnlnkDelUSPS" OnClick="btnlnkDelUSPS_Click" runat="server" Text="Delete All USPS Labels"
                                            ToolTip="Delete All USPS Labels" OnClientClick="if(!confirm('This action will delete all USPS shipping labels of this order.')) return false;"> 
                                        </asp:LinkButton>
                                        <asp:LinkButton Visible="false" ID="btnSendMailAllUSPS" OnClick="btnSendMailAllUSPS_Click"
                                            runat="server" Text="Send All USPS Mail to Customer" ToolTip="Send All USPS Mail to Customer"
                                            OnClientClick="if(!confirm('This action will send informtion of all USPS Shipping to Customer.')) return false;">
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="panUPS" runat="server" Visible="false">
                                <tr>
                                    <td>
                                        <span style="color: #9B2414; font-weight: bold;">UPS Shipping Label</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdUPS" runat="server" Width="100%" AutoGenerateColumns="false"
                                            OnRowCommand="grdUPS_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        PackageId
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPackageID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "PackageID").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Label Url
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbluspsimgurl" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "ImgUrl").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Tracking#
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrackingNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "TrackingNo").ToString()%>'
                                                            Visible="false"></asp:Label>
                                                        <%#  DataBinder.Eval(Container.DataItem, "TrackingNo").ToString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Create Date
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%#  DataBinder.Eval(Container.DataItem, "CreateDate").ToString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Download">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="bntDownUSPS" runat="server" CommandName="Download" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ImgUrl") %>'>
                                                             <img src="../images/download.png" alt="Download" style="border:0px;" height="20px" title="Download Label" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mail">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSendMail" runat="server" CommandName="SendMail" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"TrackingNo") + "-" + DataBinder.Eval(Container.DataItem,"ShippingCartID") %>'
                                                            OnClientClick="if(!confirm('This action will send informtion of the UPS Shipping to Customer.')) return false;">
                                            <img src="../images/send_mail.png" alt="SendMail" style="border: 0px;" height="20px" title="Send Mail To Customer" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDelUSPS" runat="server" CommandName="Dellabel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ImgUrl") %>'
                                                            OnClientClick="if(!confirm('This action will Delete the Shipping-Label.')) return false;">
                                            <img src="../images/delete-icon.gif" alt="Delete" style="border: 0px;" height="20px" title="Delete" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="oddrow" Height="30px" HorizontalAlign="Left" />
                                            <EditRowStyle CssClass="altrow" />
                                            <PagerStyle CssClass="altrow" HorizontalAlign="Right" />
                                            <HeaderStyle Height="28px" HorizontalAlign="Left" Wrap="True" VerticalAlign="middle"
                                                BackColor="gray" Font-Size="12px" />
                                            <AlternatingRowStyle CssClass="altrow" />
                                            <PagerSettings Position="TopAndBottom" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr runat="server" id="tr1">
                                    <td style="padding-right: 5px; padding-top: 4px; text-align: right;">
                                        <asp:LinkButton ID="btnlnkDelUPS" OnClick="btnlnkDelUPS_Click" runat="server" Text="Delete All UPS Labels"
                                            ToolTip="Delete All UPS Labels" OnClientClick="if(!confirm('This action will delete all UPS shipping labels of this order.')) return false;"> 
                                        </asp:LinkButton>
                                        <asp:LinkButton Visible="false" ID="btnSendMailAllUPS" OnClick="btnSendMailAllUPS_Click"
                                            runat="server" Text="Send All UPS Mail to Customer" ToolTip="Send All UPS Mail to Customer"
                                            OnClientClick="if(!confirm('This action will send informtion of all UPS Shipping to Customer.')) return false;">
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="panFEDEX" runat="server" Visible="false">
                                <tr>
                                    <td>
                                        <span style="color: #9B2414; font-weight: bold;">FEDEX Shipping Label</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="grdFEDEX" runat="server" Width="100%" AutoGenerateColumns="false"
                                            OnRowCommand="grdFEDEX_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        PackageId
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPackageID" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "PackageID").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Label Url
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbluspsimgurl" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "ImgUrl").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Tracking#
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrackingNo" runat="server" Text='<%#  DataBinder.Eval(Container.DataItem, "TrackingNo").ToString()%>'
                                                            Visible="false"></asp:Label>
                                                        <%#  DataBinder.Eval(Container.DataItem, "TrackingNo").ToString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Create Date
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%#  DataBinder.Eval(Container.DataItem, "CreateDate").ToString()%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Download">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="bntDownFEDEX" runat="server" CommandName="Download" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ImgUrl") %>'>
                                                             <img src="../images/download.png" alt="Download" style="border:0px;" height="20px" title="Download Label" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mail">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSendMail" runat="server" CommandName="SendMail" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"TrackingNo") + "-" + DataBinder.Eval(Container.DataItem,"ShippingCartID") %>'
                                                            OnClientClick="if(!confirm('This action will send informtion of the FEDEX Shipping to Customer.')) return false;">
                                            <img src="../images/send_mail.png" alt="SendMail" style="border: 0px;" height="20px" title="Send Mail To Customer" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDelFEDEX" runat="server" CommandName="Dellabel" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ImgUrl") %>'
                                                            OnClientClick="if(!confirm('This action will Delete the Shipping-Label.')) return false;">
                                            <img src="../images/delete-icon.gif" alt="Delete" style="border: 0px;" height="20px" title="Delete" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="oddrow" Height="30px" HorizontalAlign="Left" />
                                            <EditRowStyle CssClass="altrow" />
                                            <PagerStyle CssClass="altrow" HorizontalAlign="Right" />
                                            <HeaderStyle Height="28px" HorizontalAlign="Left" Wrap="True" VerticalAlign="middle"
                                                BackColor="gray" Font-Size="12px" />
                                            <AlternatingRowStyle CssClass="altrow" />
                                            <PagerSettings Position="TopAndBottom" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr runat="server" id="tr2">
                                    <td style="padding-right: 5px; padding-top: 4px; text-align: right;">
                                        <asp:LinkButton ID="btnlnkDelFEDEX" OnClick="btnlnkDelFEDEX_Click" runat="server" Text="Delete All FEDEX Labels"
                                            ToolTip="Delete All FEDEX Labels" OnClientClick="if(!confirm('This action will delete all FEDEX shipping labels of this order.')) return false;"> 
                                        </asp:LinkButton>
                                        <asp:LinkButton Visible="false" ID="btnSendMailAllFEDEX" OnClick="btnSendMailAllUPS_Click"
                                            runat="server" Text="Send All FEDEX Mail to Customer" ToolTip="Send All FEDEX Mail to Customer"
                                            OnClientClick="if(!confirm('This action will send informtion of all FEDEX Shipping to Customer.')) return false;">
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </asp:Panel>

                        </table>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
