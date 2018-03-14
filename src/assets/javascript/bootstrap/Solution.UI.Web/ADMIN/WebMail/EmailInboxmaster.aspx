<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="EmailInboxmaster.aspx.cs"
    Inherits="Solution.UI.Web.ADMIN.WebMail.EmailInboxmaster" EnableViewState="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../../ckeditor/ckeditor.js"></script>
    <script src="../../ckeditor/_samples/sample.js" type="text/javascript"></script>
    <link href="../../ckeditor/_samples/sample.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var testresults
        function checkemail1(str) {

            var filter = /^[A-Z0-9\._%-]+@[A-Z0-9\.-]+\.[A-Z]{2,4}(?:(?:[,;][A-Z0-9\._%-]+@[A-Z0-9\.-]+))?$/i;
            if (filter.test(str))
                testresults = true
            else {

                testresults = false
            }
            return (testresults)
        }
        function CheckEmail(address) {

            var reg = /^[A-Z0-9\._%-]+@[A-Z0-9\.-]+\.[A-Z]{2,4}(?:(?:[,;][A-Z0-9\._%-]+@[A-Z0-9\.-]+))?$/i;

            if (reg.test(address) == false) {
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function checkValidation() {

            if (document.getElementById('txtrefwdto') != null) {
                if (document.getElementById('txtrefwdto').value == '' && !CheckEmail(document.getElementById('txtrefwdto').value)) {
                    jAlert('Please Enter Email Address.', 'Message', 'txtrefwdto');
                    return false;
                }
                else if (document.getElementById('txtrefwdto') != null && document.getElementById('txtrefwdto').value != '') {
                    if (!CheckEmail(document.getElementById('txtrefwdto').value)) {
                        jAlert('Please Enter valid Email Address.', 'Message', 'txtrefwdto');
                        return false;
                    }
                }
                else {
                    return false;
                }
            }

            if (document.getElementById('tdcc').style.display == '') {
                if (document.getElementById('txtrefwdcc') != null) {
                    if (document.getElementById('txtrefwdcc') != null && document.getElementById('txtrefwdcc').value != '') {
                        if (!CheckEmail(document.getElementById('txtrefwdcc').value)) {
                            jAlert('Please Enter valid Cc Email Address.', 'Message', 'txtrefwdcc');
                            return false;
                        }
                    }
                    else {
                        return true;
                    }
                }
            }
            if (document.getElementById('tdbcc').style.display == '') {
                if (document.getElementById('txtrefwdbcc') != null) {
                    if (document.getElementById('txtrefwdbcc') != null && document.getElementById('txtrefwdbcc').value != '') {
                        if (!CheckEmail(document.getElementById('txtrefwdbcc').value)) {
                            jAlert('Please Enter valid Bcc Email Address.', 'Message', 'txtrefwdbcc');
                            return false;
                        }
                    }
                    else {
                        return true;
                    }
                }
            }
            return true;
        }

        function selectAll(on) {
            var allElts = document.getElementById("gvMailLog").getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    elt.checked = on;
                }
            }
        }


        function selectAllCheck(id) {
            var allElts = document.getElementById("gvMailLog").getElementsByTagName('INPUT');
            var i;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (document.getElementById(id) != null) {
                        elt.checked = document.getElementById(id).checked;
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">

        function ConfirmDelete() {
            //            if (confirm('Are you sure want to delete ?')) {
            //                document.getElementById('btnDeletemail').click();

            //            }

            jConfirm('Are you sure want to delete this email ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById('btnDeletemail').click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;

        }
        function ConfirmSpam() {
            //            if (confirm('Are you sure want to move this email in spam ?')) {
            //                document.getElementById('btnSpamemail').click();

            //            }


            jConfirm('Are you sure want to move this email in spam ?', 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById('btnSpamemail').click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }

        function printAllCheck() {
            if (document.getElementById('getprint')) {
                document.getElementById('divbody').style.overflow = 'hidden';
                document.getElementById('divbody').style.height = 'auto';
                var content = document.getElementById('getprint');
                var pri = document.getElementById("ifmcontentstoprint").contentWindow;
                pri.document.open();
                var contentAll = content.innerHTML;
                document.getElementById('divbody').style.overflow = 'auto';
                document.getElementById('divbody').style.height = '400px';
                pri.document.write(contentAll);
                pri.document.close();
                pri.focus();
                pri.print();

            }
            return false;
        }


        function chkselect() {
            //     var frame = document.getElementById('ctl00_ContentPlaceHolder1_ifrmcontent');

            //        var checkboxes = frame.contentWindow.document.getElementsByName('checkbox');

            //        for (var i = 0; i < checkboxes.length; i++)
            //        {
            //        var idss = chkSelect[i].id;
            //        idss = idss.replace('chkSelect','lblMailIDhdn');
            //        
            //            checkboxes[i].checked = true;
            //            
            //        }
            var istrue = false;
            var allElts = document.getElementById("gvMailLog").getElementsByTagName('INPUT');
            var i;
            var Chktrue;
            Chktrue = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        Chktrue = Chktrue + 1;

                    }
                }
            }
            if (Chktrue < 1) {
                jAlert('Please select atleast one record.', 'Message');
                return false;
            }
            else {
                if (Chktrue > 0) {

                    // return confirm('Are you sure to delete selected record?');

                    return checkaa();
                }
            }
            return true;
        }

        function checkaa() {
            jConfirm('Are you sure to delete selected record?', 'Confirmation', function (r) {
                if (r == true) {

                    document.getElementById('btndeletetemp').click();
                    return true;
                }
                else {

                    return false;
                }
            });
            return false;
        }

        function BackToList() {
            document.getElementById("trEmailDetails").style.display = 'none';
            document.getElementById("trEmailList").style.display = '';


        }

        function replymsg(acttype) {
            document.getElementById("trforwardedAttachments").style.display = '';
            if (acttype == 'ReplyAll') {
                // alert('Reply');
                document.getElementById("trReFwd").style.display = '';
                Getbtnvalue();

                //alert(document.getElementById("hdnfwdattach").value);
                if (document.getElementById("tdmailCc") != null && document.getElementById("tdmailCc").innerHTML != '') {
                    document.getElementById("tdcc").style.display = '';
                    document.getElementById("showcc").style.display = 'none';
                    document.getElementById("txtrefwdcc").value = document.getElementById("tdmailCc").innerHTML.replace("&lt;", "<").replace("&gt;", ">");
                }
                if (document.getElementById("tdmailBcc") != null && document.getElementById("tdmailBcc").innerHTML != '') {
                    document.getElementById("tdbcc").style.display = '';
                    document.getElementById("showbcc").style.display = 'none';
                    document.getElementById("txtrefwdbcc").value = document.getElementById("tdmailBcc").innerHTML.replace("&lt;", "<").replace("&gt;", ">");
                }
                document.getElementById("hdnrefwdmailid").value = document.getElementById("tdmailid").innerHTML;
                document.getElementById("txtrefwdto").value = document.getElementById("spafrom").innerHTML.replace("From: ", "").replace("&lt;", "<").replace("&gt;", ">");
                document.getElementById("txtrefwdsubject").value = "Re: " + document.getElementById("tdsubject").innerHTML;
                document.getElementById("hdnrefwdfrom").value = document.getElementById("spafrom").innerHTML;
                //document.getElementById("txtrefwdtobody").setValue(document.getElementById("divbody").innerHTML);
                var Tempsent = "------------------------------------------------------------------------------------------------------";
                Tempsent += "<br/><strong>From :</strong>  " + (document.getElementById("spafrom").innerHTML) + "<br/>";
                Tempsent += "<br/><strong>Date :</strong>  " + document.getElementById("spaSentOn").innerHTML + "<br/>";
                Tempsent += "<br/><strong>To :</strong>  " + document.getElementById("tdto").innerHTML + "<br/>";
                if (document.getElementById("tdmailCc") != null && document.getElementById("tdmailCc").innerHTML != '') {
                    Tempsent += "<br/><strong>Cc :</strong>  " + document.getElementById("tdmailCc").innerHTML.replace("&lt;", "<").replace("&gt;", ">") + "<br/>";
                }
                if (document.getElementById("tdmailBcc") != null && document.getElementById("tdmailBcc").innerHTML != '') {
                    Tempsent += "<br/><strong>Bcc :</strong>  " + document.getElementById("tdmailBcc").innerHTML.replace("&lt;", "<").replace("&gt;", ">") + "<br/>";
                }
                Tempsent += "<br/><strong>Subject :</strong>  " + document.getElementById("tdsubject").innerHTML + "<br/>";

                CKEDITOR.instances.txtrefwdtobody.setData("<br/><br/><br/><br/><br/><br/><br/><br/>" + Tempsent + "<br/>" + document.getElementById("divbody").innerHTML);
                CKEDITOR.instances.txtrefwdtobody.focus();
                document.getElementById("trEmailDetails").style.display = 'none';


            }
            else if (acttype == 'Reply') {
                // alert('Reply');
                document.getElementById("trReFwd").style.display = '';
                Getbtnvalue();
                document.getElementById("hdnrefwdmailid").value = document.getElementById("tdmailid").innerHTML;
                document.getElementById("txtrefwdto").value = document.getElementById("spafrom").innerHTML.replace('From:', '').replace("&lt;", "<").replace("&gt;", ">").replace(' ', '');
                document.getElementById("txtrefwdsubject").value = "Re: " + document.getElementById("tdsubject").innerHTML;
                document.getElementById("hdnrefwdfrom").value = document.getElementById("spafrom").innerHTML;
                //document.getElementById("txtrefwdtobody").setValue(document.getElementById("divbody").innerHTML);
                var Tempsent = "------------------------------------------------------------------------------------------------------";
                Tempsent += "<br/><strong>From :</strong>  " + (document.getElementById("spafrom").innerHTML.replace('From:', '')) + "<br/>";
                Tempsent += "<br/><strong>Date :</strong>  " + document.getElementById("spaSentOn").innerHTML.replace('Date:', '') + "<br/>";
                Tempsent += "<br/><strong>To :</strong>  " + document.getElementById("tdto").innerHTML + "<br/>";
                if (document.getElementById("tdmailCc") != null && document.getElementById("tdmailCc").innerHTML != '') {
                    Tempsent += "<br/><strong>Cc :</strong>  " + document.getElementById("tdmailCc").innerHTML.replace("&lt;", "<").replace("&gt;", ">") + "<br/>";
                }
                if (document.getElementById("tdmailBcc") != null && document.getElementById("tdmailBcc").innerHTML != '') {
                    Tempsent += "<br/><strong>Bcc :</strong>  " + document.getElementById("tdmailBcc").innerHTML.replace("&lt;", "<").replace("&gt;", ">") + "<br/>";
                }
                Tempsent += "<br/><strong>Subject :</strong>  " + document.getElementById("tdsubject").innerHTML + "<br/>";

                CKEDITOR.instances.txtrefwdtobody.setData("<br/><br/><br/><br/><br/><br/><br/><br/>" + Tempsent + "<br/>" + document.getElementById("divbody").innerHTML);
                CKEDITOR.instances.txtrefwdtobody.focus();
                document.getElementById("trEmailDetails").style.display = 'none';

            }
            else if (acttype == 'Forward') {
                // alert('Forward');
                document.getElementById("trReFwd").style.display = '';
                Getbtnvalue();
                //         if(document.getElementById("tdcc").style.display='')
                //         {
                document.getElementById("txtrefwdcc").value = ''
                document.getElementById("tdcc").style.display = 'none';
                document.getElementById("showcc").style.display = ''
                //         }
                //         if(document.getElementById("tdbcc").style.display='')
                //         {
                document.getElementById("txtrefwdbcc").value = ''
                document.getElementById("tdbcc").style.display = 'none';
                document.getElementById("showbcc").style.display = ''
                // }
                document.getElementById("txtrefwdto").value = '';
                document.getElementById("hdnrefwdmailid").value = document.getElementById("tdmailid").innerHTML;
                document.getElementById("txtrefwdsubject").value = "Fwd: " + document.getElementById("tdsubject").innerHTML;
                document.getElementById("hdnrefwdfrom").value = document.getElementById("spafrom").innerHTML.replace("&lt;", "<").replace("&gt;", ">");
                //document.getElementById("txtrefwdtobody").innerTEXT=document.getElementById("divbody").innerHTML;
                var Tempsent = "------------------------------------------------------------------------------------------------------";
                Tempsent += "<br/><strong>From :</strong>  " + (document.getElementById("spafrom").innerHTML.replace('From:', '')) + "<br/>";
                Tempsent += "<br/><strong>Date</strong> :  " + document.getElementById("spaSentOn").innerHTML.replace('Date:', '') + "<br/>";
                Tempsent += "<br/><strong>To</strong> :  " + document.getElementById("tdto").innerHTML + "<br/>";
                Tempsent += "<br/><strong>Subject</strong> :  " + document.getElementById("tdsubject").innerHTML + "<br/>";

                CKEDITOR.instances.txtrefwdtobody.setData("<br/><br/><br/><br/><br/><br/><br/><br/>" + Tempsent + "<br/>" + document.getElementById("divbody").innerHTML);
                CKEDITOR.instances.txtrefwdtobody.focus();
                document.getElementById("trEmailDetails").style.display = 'none';

            }
            else if (acttype == 'ShowCc') {
                document.getElementById("trforwardedAttachments").style.display = 'none';
                document.getElementById("tdcc").style.display = '';
                document.getElementById("showcc").style.display = 'none';
            }
            else if (acttype == 'ShowBcc') {
                document.getElementById("trforwardedAttachments").style.display = 'none';
                document.getElementById("showbcc").style.display = 'none';
                document.getElementById("tdbcc").style.display = '';
            }
            else if (acttype == 'AddAttachments') {
                document.getElementById("trforwardedAttachments").style.display = 'none';
                document.getElementById("addattachments").style.display = 'none';
                document.getElementById("tdAttachments").style.display = '';
                window.parent.document.getElementById("ctl00_ContentPlaceHolder1_ifrmcontent").height = "850px";

            }

        }

        function Getbtnvalue() {
            var allElts = document.getElementById("dvFwdAttach").getElementsByTagName('INPUT');
            var i;
            document.getElementById("hdnfwdattach").value = '';
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    if (elt.checked == true) {
                        var text = elt.id.replace('chkfwdattach-', '');
                        var btnval = document.getElementById("btnDownload-mainattach-" + text).value;
                        if (i == 0) {
                            document.getElementById("hdnfwdattach").value = btnval;
                        }
                        else {
                            document.getElementById("hdnfwdattach").value = document.getElementById("hdnfwdattach").value + ';' + btnval;
                        }
                    }
                }
            }
            //alert(document.getElementById("hdnfwdattach").value);
        }


        function SelectIndividuals() {
            var allElts = document.getElementById("gvMailLog").getElementsByTagName('INPUT');
            var i;
            var cnt = 0;
            var chkcnt = 0;
            for (i = 0; i < allElts.length; i++) {
                var elt = allElts[i];
                if (elt.type == "checkbox") {
                    chkcnt++;
                    if (elt.id == "gvMailLog_chkalll") {

                    }
                    else {
                        if (elt.checked == true) {
                            cnt++;
                        }
                        else {
                            cnt--;
                        }
                    }
                }
            }
            chkcnt--;
            //alert(cnt);
            if (cnt == chkcnt) {
                document.getElementById("gvMailLog_chkalll").checked = true;
            }
            else {
                document.getElementById("gvMailLog_chkalll").checked = false;
            }
        }

    
    </script>
    <script type="text/javascript">

        var oldcolor;
        function Highlight(row) {
            oldcolor = row.style.backgroundColor;
            row.style.backgroundColor = '#cfcfcf';
            //row.setAttribute("style", "background:#cfcfcf !important;");

        }
        function UnHighlight(row) {
            row.style.backgroundColor = oldcolor;
            //row.setAttribute("style", "background:" + oldcolor + " !important;");
        }
    </script>
    <style type="text/css">
        #divpage a
        {
            font-size: 12px;
            font-weight: bold;
            vertical-align: top;
            color: #a2a3cb;
            text-decoration: none;
        }
        #divpage a:hover
        {
            font-size: 12px;
            font-weight: bold;
            vertical-align: top;
            color: #ff8000;
            text-decoration: underline;
        }
        
        #divpage a:active
        {
            font-size: 12px;
            font-weight: bold;
            vertical-align: top;
            color: #ff8000;
            text-decoration: none;
        }
        .content-table td
        {
            background: none !important;
        }
        .content-table tr
        {
            background: none repeat scroll 0 0 #FFFFFF;
        }
        .altrow td
        {
            background: none !important;
        }
        .altrow tr
        {
            background: none repeat scroll 0 0 #FBFBFB;
        }
    </style>
</head>
<body style="background: none;">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <form id="form1" runat="server">
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="border-td" style="border: none;">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th colspan="4" valign="middle" style="padding: 0px">
                                            <div class="main-title-left" style="width: 98%">
                                                <h2 style="padding: 2px 0 0 0;">
                                                    <asp:Label ID="lblPageTitle" runat="server" Text="Folder Details"></asp:Label>
                                                    <span id="divpageitem" runat="server" style="float: right">Page per item :&nbsp;<asp:DropDownList
                                                        ID="ddlpageNo" runat="server" AutoPostBack="true" CssClass="order-list" Width="40px"
                                                        Height="20px" OnSelectedIndexChanged="ddlpageNo_SelectedIndexChanged">
                                                        <asp:ListItem Value="20" Selected="True">20</asp:ListItem>
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                        <asp:ListItem Value="40">40</asp:ListItem>
                                                        <asp:ListItem Value="50">50</asp:ListItem>
                                                        <asp:ListItem Value="60">60</asp:ListItem>
                                                        <asp:ListItem Value="70">70</asp:ListItem>
                                                        <asp:ListItem Value="80">80</asp:ListItem>
                                                    </asp:DropDownList>
                                                    </span>
                                                </h2>
                                            </div>
                                            <%-- <div id="divpageitem1" style="float: right; margin-right: 10px; color: White; font-weight: normal;"
                                                runat="server" visible="false">
                                            </div>--%>
                                        </th>
                                    </tr>
                                    <tr id="trEmailList" runat="server">
                                        <td>
                                            <%--for the Email List Grid--%>
                                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="center" colspan="4" valign="middle" style="height: 10px">
                                                        <asp:Label ID="lblMsg" runat="server" CssClass="font-red"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="top">
                                                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr id="trTop" runat="server" visible="false">
                                                                <td>
                                                                    <table align="center" height="100%" width="100%">
                                                                        <tr>
                                                                            <td style="text-align: left">
                                                                                <a id="A6" style="color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                                    font-size: 11px; font-weight: normal; line-height: normal; padding: 2px;" href="javascript:selectAll(true);">
                                                                                    Check All</a>&nbsp; | <a id="A7" style="color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                                        font-size: 11px; font-weight: normal; line-height: normal; padding: 2px;" href="javascript:selectAll(false);">
                                                                                        Clear All</a>
                                                                            </td>
                                                                            <td colspan="2" style="text-align: right">
                                                                                <asp:Button ID="Button6" runat="server" Style="background: url(/Admin/images/webmail_delete.png) no-repeat scroll 0% 0% transparent;
                                                                                    cursor: pointer; width: 50px; height: 48px; border: 0pt none;" Width="45" Height="22"
                                                                                    Text="" OnClick="btnDelete_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" valign="top">
                                                                    <div id="getIdsss" style="overflow-y: auto; height: 620px;">
                                                                        <asp:GridView ID="gvMailLog" runat="server" AutoGenerateColumns="False" CellPadding="0"
                                                                            CellSpacing="1" Width="100%" GridLines="None" AllowPaging="false" PagerSettings-Mode="Numeric" EnableViewState="true"
                                                                            OnPageIndexChanging="gvMailLog_PageIndexChanging" OnRowCreated="gvMailLog_RowCreated"
                                                                            OnRowDataBound="gvMailLog_RowDataBound" OnRowCommand="gvMailLog_RowCommand" OnSorting="gvMailLog_Sorting">
                                                                            <Columns>
                                                                                <asp:TemplateField Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblMailID" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"MailID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="">
                                                                                    <HeaderTemplate>
                                                                                        <asp:CheckBox ID="chkalll" runat="server" Text="&nbsp;Select" onchange="javascript:selectAllCheck(this.id)"
                                                                                            onclick="javascript:selectAllCheck(this.id)" /></HeaderTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkSelect" runat="server" onchange="SelectIndividuals();" onclick="SelectIndividuals();" />
                                                                                        <input type="hidden" id="lblMailIDhdn" runat="server" value='<%#DataBinder.Eval(Container.DataItem,"MailID") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="">
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <img src="" id="imgmsg" runat="server" />
                                                                                        <asp:Label ID="lblReadIdD" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"isRead") %>'></asp:Label>
                                                                                        <asp:Label ID="lblIsDeleted" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"IsDeleted") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="3%" />
                                                                                    <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                                                    <HeaderTemplate>
                                                                                        <table>
                                                                                            <tr style="background-color: transparent;">
                                                                                                <td>
                                                                                                    <img src="/Admin/Images/attachment1.png" alt="Attachment" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        &nbsp;<asp:Image ID="imgattach" runat="server" Width="16px" Height="16px" />
                                                                                        <asp:HiddenField ID="hdnattch" Value='<%#DataBinder.Eval(Container.DataItem,"IsAttachment") %>'
                                                                                            runat="server" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        From
                                                                                        <asp:ImageButton ID="btnFrom" runat="server" CommandArgument="DESC" CommandName="From"
                                                                                            AlternateText="Ascending Order" OnClick="Sorting" />
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lblFromEmail" runat="server" Style="color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                                            font-size: 11px; line-height: normal; padding: 2px;" Text='<%# DataBinder.Eval(Container.DataItem,"From") %>'
                                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"MailID") %>' CommandName="ShowSubject"
                                                                                            CausesValidation="false"></asp:LinkButton>
                                                                                        <%--<asp:Label ID="" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"From") %>'></asp:Label>--%>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="false">
                                                                                    <HeaderTemplate>
                                                                                        To
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblToEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"To") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Cc" Visible="false">
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCcEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Cc") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Bcc" Visible="false">
                                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblBccEmail" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Bcc") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderTemplate>
                                                                                        Subject
                                                                                        <asp:ImageButton ID="btnSubject" runat="server" CommandArgument="DESC" CommandName="Subject"
                                                                                            AlternateText="Ascending Order" OnClick="Sorting" />
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnksubject" runat="server" Style="color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                                            font-size: 11px; line-height: normal; padding: 2px;" Text='<%# DataBinder.Eval(Container.DataItem,"Subject") %>'
                                                                                            CommandArgument='<%# DataBinder.Eval(Container.DataItem,"MailID") %>' CommandName="ShowSubject"
                                                                                            CausesValidation="false"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderStyle HorizontalAlign="Center" Width="14%" />
                                                                                    <HeaderTemplate>
                                                                                        Date
                                                                                        <asp:ImageButton ID="btnDate" runat="server" CommandArgument="DESC" CommandName="SentOn"
                                                                                            AlternateText="Ascending Order" OnClick="Sorting" />
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSentOn" runat="server" Style="color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                                            font-size: 11px; line-height: normal; padding: 2px;" Text='<%# DataBinder.Eval(Container.DataItem,"SentOn") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <AlternatingRowStyle CssClass="altrow" />
                                                                            <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                                            <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td height="10">
                                                                </td>
                                                            </tr>
                                                            <tr id="trBottom" runat="server" visible="false">
                                                                <td>
                                                                    <table align="center" height="100%" width="100%">
                                                                        <tr>
                                                                            <td style="text-align: left">
                                                                                <a id="lkbAllowAll" style="color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                                    font-size: 11px; font-weight: normal; line-height: normal; padding: 2px;" href="javascript:selectAll(true);">
                                                                                    Check All</a>&nbsp; | <a id="lkbClearAll" style="color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                                        font-size: 11px; font-weight: normal; line-height: normal; padding: 2px;" href="javascript:selectAll(false);">
                                                                                        Clear All</a>
                                                                            </td>
                                                                            <td style="text-align: center;">
                                                                                <div id="divpage" style="color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                                    font-size: 12px; font-weight: normal; line-height: normal; padding: 2px">
                                                                                    <asp:Panel ID="pnlPaging" runat="server" EnableViewState="false">
                                                                                    </asp:Panel>
                                                                                    <asp:PlaceHolder ID="plcPaging" runat="server"></asp:PlaceHolder>
                                                                                    <asp:Literal ID="litPagingSummary" runat="server"></asp:Literal>
                                                                                </div>
                                                                            </td>
                                                                            <td style="text-align: right">
                                                                                <div style="display: none;">
                                                                                    <asp:Button ID="btnDeletemail" runat="server" Style="background: url(/Admin/images/webmail_delete.png) no-repeat scroll 0% 0% transparent;
                                                                                        cursor: pointer; width: 50px; height: 48px; border: 0pt none;" Width="45" Height="22"
                                                                                        Text="" OnClick="btnDeletemail_Click" />
                                                                                    <asp:Button ID="btnSpamemail" runat="server" Style="background: url(/Admin/images/webmail_spam.png) no-repeat scroll 0% 0% transparent;
                                                                                        cursor: pointer; width: 50px; height: 48px; border: 0pt none;" Width="45" Height="22"
                                                                                        Text="" OnClick="btnSpamemail_Click" />
                                                                                    <asp:Button ID="btnMarkUnread" runat="server" Style="background: url(/Admin/images/webmail_mark.png) no-repeat scroll 0% 0% transparent;
                                                                                        cursor: pointer; width: 92px; height: 48px; border: 0pt none;" Width="92" Height="48"
                                                                                        Text="" OnClick="btnMarkUnread_Click" />
                                                                                    <input type="hidden" id="hdmMailIdss" runat="server" value="0" />
                                                                                </div>
                                                                                <asp:Button ID="btnDelete" runat="server" Style="background: url(/Admin/images/webmail_delete.png) no-repeat scroll 0% 0% transparent;
                                                                                    cursor: pointer; width: 50px; height: 48px; border: 0pt none;" Width="45" Height="22"
                                                                                    Text="" OnClick="btnDelete_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trEmailDetails" runat="server" style="display: none">
                                        <td style="padding-left: 0px;">
                                            <span style='font-size: 14px; float: left'></span>
                                            <%--For the Showing Email contents--%>
                                            <table cellpadding="3" cellspacing="0" width="100%" border="0">
                                                <tr>
                                                    <td style="background-color: #f2f2f2; border: solid 1px #ddd;">
                                                        <%-- <asp:ImageButton ID="btnreply" runat="server" AlternateText="Reply" ImageUrl="" Width="30px"
                                        Height="30px" ToolTip="Reply" OnClientClick="replymsg('Reply');" />
                                    <asp:ImageButton ID="btnforward" runat="server" AlternateText="Forward" ImageUrl=""
                                        Width="30px" Height="30px" ToolTip="Forward" OnClientClick="replymsg('Forward');" />--%>
                                                        <a href="javascript:void(0);" id="A2" onclick="javascript:if('<%=Request["ShowType"]%>' != null && '<%=Request["ShowType"]%>' == 'ShowBody'){window.close();}else{window.location.href = window.location.href;}"
                                                            style="color: #fff; text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                            font-size: 11px; font-weight: normal; line-height: normal; padding: 2px;">
                                                            <input type="button" id="Button2" onclick="return false;" style="background: url(/Admin/images/webmail_back.png) no-repeat scroll 0% 0% transparent;
                                                                cursor: pointer; width: 50px; height: 48px; border: 0pt none;" value="" title="Back" /></a>
                                                        <a href="javascript:void(0);" id="btnreply" onclick="replymsg('Reply');" style="color: #fff;
                                                            text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px;
                                                            font-weight: normal; line-height: normal; padding: 2px;">
                                                            <input type="button" id="r1" style="background: url(/Admin/images/webmail_reply.png) no-repeat scroll 0% 0% transparent;
                                                                width: 50px; cursor: pointer; height: 48px; border: 0pt none;" value="" title="Reply" /></a>
                                                        <a href="javascript:void(0);" id="btnreplyall" onclick="replymsg('ReplyAll');" style="color: #fff;
                                                            text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px;
                                                            font-weight: normal; line-height: normal; padding: 2px;">
                                                            <input type="button" id="ra1" style="background: url(/Admin/images/webmail_reply-all.png) no-repeat scroll 0% 0% transparent;
                                                                cursor: pointer; width: 50px; height: 48px; border: 0pt none;" value="" title="Reply To All" /></a>
                                                        <a href="javascript:void(0);" id="btnforward" onclick="replymsg('Forward');" style="color: #fff;
                                                            text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px;
                                                            font-weight: normal; line-height: normal; padding: 2px;">
                                                            <input type="button" id="f1" style="background: url(/Admin/images/webmail_forward.png) no-repeat scroll 0% 0% transparent;
                                                                cursor: pointer; width: 50px; height: 48px; border: 0pt none;" value="" title="Forward" /></a>
                                                        <a href="javascript:void(0);" id="A4" onclick="javascript:document.getElementById('btnMarkUnread').click();"
                                                            style="color: #fff; text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                            font-size: 11px; font-weight: normal; line-height: normal; padding: 2px;">&nbsp;
                                                            <input type="button" id="Button4" onclick="return false;" style="background: url(/Admin/images/webmail_mark.png) no-repeat scroll 0% 0% transparent;
                                                                cursor: pointer; width: 92px; height: 48px; border: 0pt none;" value="" title="Mark As Unread" /></a>
                                                        <a href="javascript:void(0);" id="A5" onclick="ConfirmSpam();" style="font-family: Verdana,Arial,Helvetica,sans-serif;
                                                            font-size: 11px; font-weight: normal; line-height: normal; padding: 2px; text-decoration: none;
                                                            color: #fff;">
                                                            <input type="button" id="Button5" onclick="return false;" style="background: url(/Admin/images/webmail_spam.png) no-repeat scroll 0% 0% transparent;
                                                                cursor: pointer; width: 50px; height: 48px; border: 0pt none;" value="" title="Spam" /></a>
                                                        <a href="javascript:void(0);" id="A3" onclick="ConfirmDelete();" style="color: #fff;
                                                            text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px;
                                                            font-weight: normal; line-height: normal; padding: 2px;">
                                                            <input type="button" id="Button3" onclick="return false;" style="background: url(/Admin/images/webmail_delete.png) no-repeat scroll 0% 0% transparent;
                                                                cursor: pointer; width: 50px; height: 48px; border: 0pt none;" value="" title="Delete" /></a>
                                                        <a href="javascript:void(0);" id="A1" onclick="return printAllCheck();" style="color: #fff;
                                                            text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px;
                                                            font-weight: normal; line-height: normal; padding: 2px;">
                                                            <input type="button" id="Button1" onclick="return false;" style="background: url(/Admin/images/webmail_print.png) no-repeat scroll 0% 0% transparent;
                                                                cursor: pointer; width: 50px; height: 48px; border: 0pt none;" value="" title="Print" /></a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Literal ID="ltrsubjectshow" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr id="trReFwd" runat="server" style="display: none">
                                        <td style="padding-left: 0px;">
                                            <table cellpadding="3" cellspacing="0" width="100%" border="0">
                                                <tr style='background-color: #F2F2F2; border: 1px solid White; height: 15px;'>
                                                    <td>
                                                        <span style="float: left; margin-left: 15px; margin-top: 2px">
                                                            <%--<asp:ImageButton ID="btnsend" runat="server" AlternateText="Send" ImageUrl="" Width="30px"
                                            Height="30px" ToolTip="Send" OnClick="btnsend_Click" />
                                        <asp:ImageButton ID="btndiscard" runat="server" AlternateText="Discard" ImageUrl=""
                                            Width="30px" Height="30px" ToolTip="Discard" OnClick="btndiscard_Click" />--%>
                                                            <asp:LinkButton ID="btnsend" runat="server" ToolTip="Send" OnClick="btnsend_Click"
                                                                Style="background: url(/Admin/images/webmail_send.png) no-repeat scroll 0% 0% transparent;
                                                                width: 50px; height: 48px; border: 0pt none;" Width="38" Height="22" OnClientClick="return checkValidation();"></asp:LinkButton>
                                                            <asp:LinkButton ID="btndiscard" runat="server" ToolTip="Discard" OnClientClick="if(document.getElementById('trEmailDetails')){document.getElementById('trEmailDetails').style.display='';}if(document.getElementById('trReFwd')){document.getElementById('trReFwd').style.display='none';} return false;"
                                                                Style="background: url(/Admin/images/webmail_Discard.png) no-repeat scroll 0% 0% transparent;
                                                                width: 50px; height: 48px; border: 0pt none;" Width="50" Height="22"></asp:LinkButton>
                                                            <asp:LinkButton ID="btnCancel" Visible="false" runat="server" ToolTip="Cancel" Style="background: url(/Admin/images/webmail_Discard.png) no-repeat scroll 0% 0% transparent;
                                                                width: 50px; height: 48px; border: 0pt none;" Width="50" Height="22"></asp:LinkButton>
                                                        </span><span style="float: right; margin-right: 35px" id="sparflinks" runat="server">
                                                            <%--  <asp:ImageButton ID="btnreplyr" runat="server" AlternateText="Reply" ImageUrl=""
                                            Width="30px" Height="30px" ToolTip="Reply" OnClientClick="replymsg('Reply');" />
                                        <asp:ImageButton ID="btnforwardf" runat="server" AlternateText="Forward" ImageUrl=""
                                            Width="30px" Height="30px" ToolTip="Forward" OnClientClick="replymsg('Forward');" />--%>
                                                            <a href="javascript:void(0);" id="btnreplyr" onclick="replymsg('Reply');return false;"
                                                                style="color: #fff; text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                font-size: 11px; font-weight: normal; line-height: normal; padding: 2px;">
                                                                <input type="button" id="r11" style="background: url(/Admin/images/webmail_reply.png) no-repeat scroll 0% 0% transparent;
                                                                    cursor: pointer; width: 50px; height: 48px; border: 0pt none;" value="" title="Reply" /></a>
                                                            <a href="javascript:void(0);" id="btnreplyallr" onclick="replymsg('ReplyAll');" style="color: #fff;
                                                                text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px;
                                                                font-weight: normal; line-height: normal; padding: 2px;">
                                                                <input type="button" id="ra11" style="background: url(/Admin/images/webmail_reply-all.png) no-repeat scroll 0% 0% transparent;
                                                                    cursor: pointer; width: 50px; height: 48px; border: 0pt none;" value="" title="Reply To All" /></a><a
                                                                        href="javascript:void(0);" id="btnforwardf" onclick="replymsg('Forward');return false;"
                                                                        style="color: #fff; text-decoration: none; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                                        font-size: 11px; font-weight: normal; line-height: normal; padding: 2px;"><input
                                                                            type="button" id="f11" style="background: url(/Admin/images/webmail_forward.png) no-repeat scroll 0% 0% transparent;
                                                                            cursor: pointer; width: 50px; height: 48px; border: 0pt none;" value="" title="Forward" /></a>
                                                        </span>
                                                        <%--<asp:ImageButton ID="btndelete" runat="server" AlternateText="Reply" ImageUrl="~/Admin/images/Reply.png"
                                        Width="30px" Height="30px" ToolTip="Forward" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                            &nbsp;
                                            <table cellpadding="3" cellspacing="0" width="100%" border="0">
                                                <tr style="display: none;">
                                                    <td>
                                                        <asp:HiddenField ID="hdnrefwdfrom" runat="server" />
                                                        <asp:HiddenField ID="hdnrefwdmailid" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr style='background-color: white; border: 1px solid White;'>
                                                    <td style="width: 19%">
                                                        &nbsp;&nbsp; To :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtrefwdto" CssClass="textfield_small" Width="400px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="tdcc" style='background-color: #fff; border: 1px solid White; display: none'>
                                                    <td>
                                                        &nbsp;&nbsp; Cc :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtrefwdcc" CssClass="textfield_small" Width="400px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="tdbcc" style='background-color: white; border: 1px solid White; display: none'>
                                                    <td>
                                                        &nbsp;&nbsp; Bcc :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtrefwdbcc" CssClass="textfield_small" Width="400px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style='background-color: #fff; border: 1px solid White;'>
                                                    <td>
                                                        &nbsp;&nbsp; Subject :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtrefwdsubject" CssClass="textfield_small" Width="400px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr style='background-color: white; border: 1px solid White;'>
                                                    <td>
                                                        &nbsp;&nbsp; Email Type :
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkishtml" runat="server" Text=" Is HTML Message" Checked="true" />
                                                    </td>
                                                </tr>
                                                <tr id="tdAttachments" style='background-color: white; border: 1px solid White; display: none'>
                                                    <td>
                                                        &nbsp;&nbsp; Attachments :
                                                    </td>
                                                    <td>
                                                        File&nbsp;1&nbsp;:&nbsp;<asp:FileUpload ID="FileUpload1" runat="server" CssClass="textfield_small"
                                                            Style="width: 200px; height: 23px;" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        File&nbsp;2&nbsp;:&nbsp;<asp:FileUpload ID="FileUpload2" runat="server" CssClass="textfield_small"
                                                            Style="width: 200px; height: 23px;" /><br />
                                                        <br />
                                                        File&nbsp;3&nbsp;:&nbsp;<asp:FileUpload ID="FileUpload3" runat="server" CssClass="textfield_small"
                                                            Style="width: 200px; height: 23px;" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        File&nbsp;4&nbsp;:&nbsp;<asp:FileUpload ID="FileUpload4" runat="server" CssClass="textfield_small"
                                                            Style="width: 200px; height: 23px;" />
                                                        <input type="hidden" id="hdnfl1" />
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trforwardedAttachments" style='background-color: #fff; border: 1px solid White;'>
                                                    <td>
                                                        &nbsp;&nbsp;Previous Attachments :
                                                    </td>
                                                    <td>
                                                        <div id="dvFwdAttach" runat="server">
                                                            <asp:Literal ID="ltrFwdAttach" runat="server"></asp:Literal>
                                                            <asp:HiddenField ID="hdnfwdattach" runat="server" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr style='background-color: white; border: 1px solid White;'>
                                                    <td>
                                                    </td>
                                                    <td align="left">
                                                        <a href="javascript:void(0);" id="showcc" style="color: #696A6A; font-family: Verdana,Arial,Helvetica,sans-serif;
                                                            font-size: 11px; font-weight: normal; line-height: normal; padding: 2px;" onclick="replymsg('ShowCc');return false;">
                                                            Show Cc</a>&nbsp; <a href="javascript:void(0);" id="showbcc" style="color: #696A6A;
                                                                font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px; font-weight: normal;
                                                                line-height: normal; padding: 2px;" onclick="replymsg('ShowBcc');return false;">
                                                                Show Bcc</a>&nbsp; <a href="javascript:void(0);" id="addattachments" style="color: #696A6A;
                                                                    font-family: Verdana,Arial,Helvetica,sans-serif; font-size: 11px; font-weight: normal;
                                                                    line-height: normal; padding: 2px;" onclick="replymsg('AddAttachments');return false;">
                                                                    Add Attachments</a>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="ckeditor-table">
                                                        <asp:TextBox ID="txtrefwdtobody" runat="server" Columns="80" Rows="10" TextMode="multiLine"></asp:TextBox>
                                                        <script type="text/javascript">
                                                            CKEDITOR.replace('<%= txtrefwdtobody.ClientID %>', { baseHref: '<%= ResolveUrl("~/ckeditor/") %>', height: 400 });
                                                            CKEDITOR.on('dialogDefinition', function (ev) {
                                                                if (ev.data.name == 'image') {
                                                                    var btn = ev.data.definition.getContents('info').get('browse');
                                                                    btn.hidden = false;
                                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'ImageBrowser.aspx', 'popuppage', 'scrollbars=no,width=780,height=630,left=' + ((screen.width - 780) / 2) + ',top=' + ((screen.height - 630) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                }
                                                                if (ev.data.name == 'link') {
                                                                    var btn = ev.data.definition.getContents('info').get('browse');
                                                                    btn.hidden = false;
                                                                    btn.onClick = function () { window.open(CKEDITOR.basePath + 'LinkBrowser.aspx', 'popuppage', 'scrollbars=no,width=520,height=580,left=' + ((screen.width - 520) / 2) + ',top=' + ((screen.height - 580) / 2) + ',resizable=no,toolbar=no,titlebar=no'); };
                                                                }
                                                            });
                                                        </script>
                                                    </td>
                                                </tr>
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
    <div style="visibility: hidden">
        <iframe id="ifmcontentstoprint"></iframe>
    </div>
    <div style="display: none;">
        <asp:Button ID="btndeletetemp" runat="server" Style="background: url(/Admin/images/webmail_delete.png) no-repeat scroll 0% 0% transparent;
            cursor: pointer; width: 50px; height: 48px; border: 0pt none;" Width="45" Height="22"
            Text="" OnClick="btnDelete_Click" />
    </div>
    </form>
</body>
</html>
