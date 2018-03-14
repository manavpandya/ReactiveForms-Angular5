<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="AddDynamicPageProperty.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.AddDynamicPageProperty" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>


    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <script src="/admin/js/jquery-1.4.1.min.js"></script>
   
  
    <script type="text/javascript">
        function MutExChkList(chk) {
            var chkList = chk.parentNode.parentNode.parentNode;
            var chks = chkList.getElementsByTagName("input");
            for (var i = 0; i < chks.length; i++) {
                if (chks[i] != chk && chk.checked) {
                    chks[i].checked = false;
                }
            }
        }

        function ValidatePage() {
            var ddlpage = document.getElementById("ContentPlaceHolder1_ddlpagename");
            if (ddlpage.value == "" || ddlpage.value == "0" || ddlpage.value == 0) {
                jAlert('Please Enter Page Name.', 'Message');
                document.getElementById("ContentPlaceHolder1_ddlpagename").focus();
                return false;
            }
            return true;
        }
       

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                            class="content-table">
                            <tr>
                                <td class="border-td-sub">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                        <tr>
                                            <th>
                                                <div class="main-title-left">

                                                    <h2>
                                                        <asp:Label ID="lblTitle" runat="server" Text="Add New Page Property"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td class="border-td-sub" width="50%" valign="top">
                                                <fieldset class="fldset" style="width: 98%;">
                                                    <table align="left" class="add-product" border="0" cellpadding="0" cellspacing="0"
                                                        width="100%">
                                                        <tr>
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Page Name :
                                                            </td>
                                                            <td align="left" valign="top">
                                                               <asp:DropDownList ID="ddlpagename" runat="server" CssClass="order-list">
                                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                    <asp:ListItem Value="Buy1Get1">Buy 1 Get 1</asp:ListItem>
                                                                    <asp:ListItem Value="OnSale">On Sale</asp:ListItem>
                                                                    <asp:ListItem Value="KidsCollection">Kids Collection</asp:ListItem>
                                                                    <asp:ListItem Value="GrommetCurtains">Grommet Curtains</asp:ListItem>
                                                                    <asp:ListItem Value="NewArrivalCurtains">New Arrival Curtains</asp:ListItem>
                                                                    <asp:ListItem Value="CustomCurtains">Custom Curtains</asp:ListItem>
                                                                    <asp:ListItem Value="FreeSwatch">Free Swatch</asp:ListItem>
                                                                    <asp:ListItem Value="CustomItemPage">Custom Item Page</asp:ListItem>
                                                                   <%-- <asp:ListItem Value="SalesEventpage">Sales Event page</asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Title :
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="txtpagetitle" runat="server"></asp:TextBox>

                                                            </td>

                                                        </tr>
                                                        <tr class="even-row">

                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Description :
                                                            </td>
                                                            <td align="left" valign="top">
                                                              <CKEditor:CKEditorControl ID="txtDescription" runat="server" BasePath="~/ckeditor/"
                                                                Width="800px" Height="400px"></CKEditor:CKEditorControl>
                                                            </td>

                                                        </tr>
                                                        <tr class="even-row">
                                                            <td align="left" valign="top" width="20%;">
                                                                <span style="color: Red;">&nbsp;</span> Description Position :
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:DropDownList ID="ddldescposition" runat="server" CssClass="order-list">
                                                                    <asp:ListItem Value="top">Top</asp:ListItem>
                                                                    <asp:ListItem Value="bottom">Bottom</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr class="even-row">
                                                             <td colspan="2">
                                                            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="border-td content-table">
                                                                <tbody>
                                                                    <tr>
                                                                        <th>
                                                                            <div class="main-title-left">
                                                                                <img class="img-left" title="SEO" alt="SEO" src="/App_Themes/<%=Page.Theme %>/icon/add-product.png">
                                                                                <h2>SEO</h2>
                                                                            </div>

                                                                        </th>
                                                                    </tr>

                                                                    <tr>
                                                                        <td id="tdSEO">
                                                                            <div id="tab-container" class="slidingDivSEO">
                                                                                <ul class="menu">
                                                                                    <li class="active" id="ordernotes1" onclick='$("#ordernotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#giftnotes1").removeClass("active");$("div.order-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.gift-notes").css("display", "none");'>Page Title</li>
                                                                                    <li id="privatenotes1" onclick='$("#ordernotes1").removeClass("active");$("#privatenotes1").addClass("active"); $("#giftnotes1").removeClass("active");$("div.private-notes").fadeIn();$("div.order-notes").css("display", "none");$("div.gift-notes").css("display", "none");'>Keywords</li>
                                                                                    <li id="giftnotes1" onclick='$("#giftnotes1").addClass("active");$("#privatenotes1").removeClass("active");$("#ordernotes1").removeClass("active");$("div.gift-notes").fadeIn();$("div.private-notes").css("display", "none");$("div.order-notes").css("display", "none");'>Description</li>

                                                                                </ul>
                                                                                <span class="clear"></span>
                                                                                <div class="tab-content-2 order-notes">
                                                                                    <div class="tab-content-3">
                                                                                        <asp:TextBox ID="txtSETitle" runat="server" CssClass="order-textfield" Width="100%"
                                                                                            Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="29"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tab-content-2 private-notes">
                                                                                    <div class="tab-content-3">
                                                                                        <asp:TextBox ID="txtSEKeyword" runat="server" CssClass="order-textfield" Width="100%"
                                                                                            Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="30"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="tab-content-2 gift-notes">
                                                                                    <div class="tab-content-3">
                                                                                        <asp:TextBox ID="txtSEDescription" runat="server" CssClass="order-textfield" Width="100%"
                                                                                            Columns="6" Height="90px" BorderStyle="None" Rows="6" TextMode="MultiLine" TabIndex="31"></asp:TextBox>
                                                                                    </div>
                                                                                </div>

                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>

                                        <tr class="even-row" align="center">
                                            <td align="center">

                                                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return ValidatePage();" /> <%--OnClientClick="return validation();"--%> 
                                                <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cencel" ToolTip="Cancel" OnClick="imgCancle_Click" />
                                            </td>
                                        </tr>


                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="10" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10">
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none;">
            <iframe id="idsset"></iframe>
        </div>
    </div>

</asp:Content>
