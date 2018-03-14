<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageBox.ascx.cs"
    Inherits="Solution.UI.Web.ADMIN.Controls.MessageBox" %>
<script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
<script type="text/javascript" language="javascript">
    function autoResize() {
        $('#themeframe').height($('#themeframe').contents().height());
    }

    function SearchValidation() {

        //        if (document.getElementById("ContentPlaceHolder1_ctl28_ddlUserlist").selectedIndex == 0) {

        //            jAlert('Please Select User.', 'Message', 'ContentPlaceHolder1_ctl28_ddlUserlist');
        //            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ctl28_ddlUserlist').offset().top }, 'slow');
        //            return false;
        //        }

        //   else if (document.getElementById("ContentPlaceHolder1_ctl28_txtmsg").value == '') {

        //            jAlert('Please enter Message.', 'Message', 'ContentPlaceHolder1_ctl28_txtmsg');
        //            $('html, body').animate({ scrollTop: $('#ContentPlaceHolder1_ctl28_txtmsg').offset().top }, 'slow');
        //            return false;
        //        }


        if (document.getElementById('<%=ddlUserlist.ClientID%>').selectedIndex == 0) {

            jAlert('Please Select User.', 'Message', '<%=ddlUserlist.ClientID%>');
            $('html, body').animate({ scrollTop: $('#<%=ddlUserlist.ClientID%>').offset().top }, 'slow');
            return false;
        }
        else if (document.getElementById('<%=txtmsg.ClientID%>').value == '') {

            jAlert('Please enter Message.', 'Message', '<%=txtmsg.ClientID%>');
            $('html, body').animate({ scrollTop: $('#<%=txtmsg.ClientID%>').offset().top }, 'slow');
            return false;
        }




        return true;
    }

    function onloadFrame() {

        document.getElementById('<%=Iframe1.ClientID%>').height = document.getElementById('<%=Iframe1.ClientID%>').contentWindow.document.documentElement.scrollHeight;

    }
</script>
<div>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="dashboard-left">
        <tbody>
            <tr>
                <th>
                    <div class="main-title-left">
                        <img src="/App_Themes/<%=Page.Theme %>/icon/item-by-sale.png" alt="Message Box" title="Message Box"
                            class="img-left" />
                        <h2>
                            Message Box</h2>
                    </div>
                    <div class="main-title-right">
                        <a title="Minimize" href="javascript:void(0);" onclick="ShowDiv('imgmsg','messagebox','msgcntrls');">
                            <img class="minimize" title="Minimize" id="imgmsg" alt="Minimize" src="/App_Themes/<%=Page.Theme %>/images/minimize.png" /></a>
                    </div>
                </th>
            </tr>
            <tr id="messagebox">
              
                        <td align="center" valign="middle">
                            <div>
                                <iframe id="Iframe1" src='/Admin/MessageBox.aspx' scrolling='no' frameborder='0'
                                    width="100%" runat="server"></iframe>
                            </div>
                        </td>
                   
            </tr>
            <tr id="msgcntrls">
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr id="tblItem">
                            <td align="left" class="even-row" style="padding: 3pt;" id="tdviewMsg" runat="server">
                                <a id="viewmsgs" runat="server" title="View List" href="/Admin/ViewallMessage.aspx"
                                    style="cursor: pointer;"><span style="padding-right: 4px">View All Messages</span></a>
                                <img class="img-right" title="View List" alt="View List" src="/App_Themes/<%=Page.Theme %>/icon/view-report.png" />
                            </td>
                        </tr>
                        <tr>
                            <td style="float: left">
                                <span><strong>Send Message </strong></span>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" style="padding-right: 0px">
                                <asp:DropDownList ID="ddlUserlist" runat="server" Style="width: 100%; background: #fff;
                                    border: 1px solid #bcc0c1; font-size: 12px;">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%">
                                <asp:TextBox ID="txtmsg" runat="server" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" width="100%" style="padding-right: 0px">
                                <asp:ImageButton ID="btnsend" runat="server" OnClick="btnsend_Click" OnClientClick="return SearchValidation();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</div>
