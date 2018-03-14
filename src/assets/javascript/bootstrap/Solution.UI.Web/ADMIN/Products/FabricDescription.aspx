<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FabricDescription.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.FabricDescription" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/gray/js/jquery-alerts.js"></script>
    <script src="../JS/jquery-1.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closeWin() {
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">


        <div>

            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="content-table border-td"
                style="padding: 2px;">
                <tbody>

                    <tr>
                        <th>
                            <div class="main-title-left" style="color: #fff; text-align: left; padding-top: 8px;">
                                Fabric Description : <asp:Literal ID="fabricname" runat="server"></asp:Literal>
                            </div>
                            <div class="main-title-right">
                                <a href="javascript:void(0);" class="show_hideMainDiv" runat="server" id="btnClose"
                                    onclick="window.close();">
                                    <img id="imgMainDiv" class="close" title="close" alt="close" src="/App_Themes/<%=Page.Theme %>/images/cancel-icon.png" /></a>
                            </div>
                        </th>
                    </tr>
                    <tr>

                    </tr>
                    
                    <tr class="even-row">
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="padding-top:5px;">

                                <tr class="altrow">
                                    <td valign="top">
                                        <span class="star">&nbsp;&nbsp;</span>Description:
                                    </td>
                                    <td>
                                        <CKEditor:CKEditorControl ID="ckeditordescription" runat="server" BasePath="~/ckeditor/"
                                            Width="600px" Height="150px"></CKEditor:CKEditorControl>
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr class="altrow" >
                        <td align="center" style="padding-top:5px;">
                            <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ToolTip="Save"
                                OnClick="btnSave_Click" />

                        </td>
                    </tr>
            </table>


        </div>

    </form>
</body>
</html>
