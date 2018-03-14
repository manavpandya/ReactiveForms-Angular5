<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="CategoryPublish.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.CategoryPublish" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <style type="text/css">

        #ContentPlaceHolder1_chklcategory input
        {
            margin: 0 7px 0 0;
            float: left;
        }
        .mycheckbox input[type="checkbox"] 
{ 
    margin-right: 15px; 
}
        .btn{
            padding:5px;
            font-weight:bold;
        }
    </style>
    <script type="text/javascript" >
        function checkCount() {
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (myform.elements[i].checked == true) {
                        count += 1;
                    }
                }
            }
            if (count == 0) {
                $(document).ready(function () { jAlert('Please select at least one Category...', 'Message', ''); });
                return false;
            }
        }

        function validatepage() {
            var name = document.getElementById('ContentPlaceHolder1_txtcategory');
            if (name != null) {
                if (name.value == '') {
                    //jAlert('Please enter category name.', 'Message');
                    jAlert('Please enter category name.', 'Message', '<%=txtcategory.ClientID %>');
                    name.focus();
                    return false;
                }
                else { return true; }
            }
        
            return false;
        }
        function allcheckAll()
        {
            var myform = document.forms[0];
            var len = myform.elements.length;
            var count = 0;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox' && myform.elements[i].id.toString() != 'chkall') {
                    if (myform.elements[i].checked == true) {
                        myform.elements[i].checked = false;
                    }
                    else if (myform.elements[i].checked == false && myform.elements[i].id.toString() != 'chkall') {
                        myform.elements[i].checked = true;
                    }
                }
            }
             
        }
       </script>

    <link href="../App_Themes/<%=Page.Theme %>/css/jquery.alerts.css" type="text/css" rel="stylesheet" />
    <link href="../App_Themes/<%=Page.Theme %>/css/jquery-ui-1.8.custom.css" type="text/css" rel="stylesheet" />
    <link href="../App_Themes/<%=Page.Theme %>/css/jqx.base.css" type="text/css" rel="stylesheet" />
    <link href="../App_Themes/<%=Page.Theme %>/css/jqx.summer.css" type="text/css" rel="stylesheet" />
    <link href="../App_Themes/<%=Page.Theme %>/css/popup.css" type="text/css" rel="stylesheet" />
    <link href="../App_Themes/<%=Page.Theme %>/css/style.css" type="text/css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;</div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td class="border-td">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th colspan="4">
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Administrator Roles" alt="Administrator Roles" src="/App_Themes/<%=Page.Theme %>/Images/admin-rights-icon.png">
                                                    <h2>
                                                       Category Publish</h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="altrow">
                                         
                                            <td align="left" style="padding: 10px;width:325px" id="tdAddtemplatelist" runat="server" visible="true">
                                             
                                              <span style="padding-right: 5px;">Category Name:</span>
                                                  <asp:TextBox ID="txtcategory" runat="server" CssClass="order-textfield"></asp:TextBox>
                                              
                                            </td>
                                            <td style="padding: 10px;">
                                                  <div >
                                                <asp:ImageButton ID="btngo" runat="server" AlternateText="Go" ToolTip="Go" OnClick="btngo_Click" OnClientClick="return validatepage();"  />
                                                <asp:ImageButton ID="btnshowall" runat="server" AlternateText="show all" ToolTip="show all" OnClick="btnshowall_Click"  />
                                                      
                                                    </div>
                                                <div style="float:right;"> <asp:Button ID="btnhomepagebnnaer" runat="server" OnClick="btnhomepagebnnaer_Click" Text="Home Page publish" /></div>
                                            </td>
                                           <%-- 
                                            <td align="left" style="padding: 10px;" id="tdtemplatelist" runat="server" visible="false">
                                             <span style="padding-right: 5px;">Template List :</span>   
                                                <asp:DropDownList ID="ddltemplatename" runat="server" Width="300px" CssClass="order-list"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddltemplatename_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                 
                                            </td>--%>
                                            <td  style="padding: 10px;" id="tdbtngo" runat="server" >
                                                  
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="4">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border: 1px solid #DADADA;
                                                    border-collapse: collapse;">
                                                    <tr style="height: 27px;">
                                                        <td colspan="4" align="left" style="border: 1px solid #DADADA; background: #e7e7e7;
                                                            font-weight: bold;">
                                                         Category List
                                                        </td>
                                                      <%--  <td colspan="2" align="center" style="border: 1px solid #DADADA; background: #e7e7e7;
                                                            font-weight: bold;">
                                                          
                                                        </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="left">
                                                            Check ALL: <input type="checkbox" id="chkall" onchange="allcheckAll();" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                       <%-- <td align="left" style="vertical-align: top; padding: 10px; width: 7%;">
                                                        </td>--%>
                                                        <td align="left" style="padding-left: 100px; width: 20%; vertical-align: top; border-right: solid 1px #DADADA;" >
                                                            <div style="overflow-y:scroll;HEIGHT:330px;width:450px" id="chkdiv" runat="server">
                                                            <asp:CheckBoxList ID="chklcategory" runat="server" >
                                                            </asp:CheckBoxList>
                                                                </div>
                                                            <br />
                                                            <asp:Label ID="lblnoresult" Text="No Records Found!" ForeColor="Red" runat="server" Visible="false"></asp:Label>
                                                        </td>
                                                     

                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="padding-left: 100px; width: 20%; vertical-align: top; border-right: solid 1px #DADADA;padding-bottom:20px" >   <asp:ImageButton ID="btnUpdate" runat="server" AlternateText="Publish " Visible="true"
                                                                ToolTip="publish" CssClass="btn" OnClientClick="return checkCount();" ForeColor="White" weight="20px"  BackColor="#B92127" Height="20px" Width="20px" OnClick="btnUpdate_Click"  /></td>
                                                    </tr>
                                                   <%-- <tr>
                                                        <td>
                                                        </td>
                                                        <td align="left" style="padding-left: 15px; border-right: 1px solid #DADADA;">
                                                         
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td align="left" style="padding-left: 15px;">
                                                            <asp:Button ID="btnUpdatePageRight" runat="server" AlternateText="Update" ToolTip="Update"
                                                                OnClientClick="return updatevalidate();"/>
                                                        </td>
                                                    </tr>--%>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div>
        <asp:HiddenField ID="hdnaddnew" runat="server" value="0"/>
    </div>
</asp:Content>