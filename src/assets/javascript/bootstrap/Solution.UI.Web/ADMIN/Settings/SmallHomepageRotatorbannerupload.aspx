﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="SmallHomepageRotatorbannerupload.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Settings.SmallHomepageRotatorbannerupload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender-custom.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-calender.js"></script>
    <link rel="stylesheet" type="text/css" href="/css/plugins-1.2.css" />
    <script type="text/javascript">
        $(function () {

            $('#ContentPlaceHolder1_txtStartDate').datetimepicker({
                showButtonPanel: true, ampm: false, showHour: false, showMinute: false, showSecond: false, showTime: false

            });
            $('#ContentPlaceHolder1_txtEndDate').datetimepicker({
                showButtonPanel: true, ampm: false, showHour: false, showMinute: false, showSecond: false, showTime: false

            });
        });

        function chkDate() {

            if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '' && document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {
                if (document.getElementById('<%=txtgroupdisplayorder.ClientID %>') != null && document.getElementById('<%=txtgroupdisplayorder.ClientID %>').value == '') {

                    jAlert('Please enter Display Order.', 'Required', '<%=txtgroupdisplayorder.ClientID %>');
                    return false;
                }
            }
            else {


                if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '') {

                    jAlert('Please enter start date.', 'Required', '<%=txtStartDate.ClientID %>');
                    return false;
                }
                if (document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {

                    jAlert('Please enter End date.', 'Required', '<%=txtEndDate.ClientID %>');
                    return false;
                }

                var date1 = new Date(document.getElementById('<%=txtStartDate.ClientID %>').value);
                var date2 = new Date(document.getElementById('<%=txtEndDate.ClientID %>').value);

                if (date1 > date2) {
                    jAlert('Start date must be less than or equal to end date.', 'Required', '<%=txtStartDate.ClientID %>');
                    return false;
                }
                if (document.getElementById('<%=txtgroupdisplayorder.ClientID %>') != null && document.getElementById('<%=txtgroupdisplayorder.ClientID %>').value == '') {

                    jAlert('Please enter Display Order.', 'Required', '<%=txtgroupdisplayorder.ClientID %>');
                    return false;
                }

            }
            return true;
        }

        function Checkfields() {
            // document.getElementById('').value = '';
            var fup = document.getElementById('<%=FileUploadBanner.ClientID %>');
            var fileName = fup.value;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            ext = ext.toLowerCase();
            if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '' && document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {

                if (document.getElementById('<%=TxtbannerTitle.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle.ClientID %>').value == '') {
                    jAlert('Please enter banner title.', 'Required', '<%=TxtbannerTitle.ClientID %>');
                    return false;
                }
                    //            else if (document.getElementById('<%=TxtBannerURL.ClientID %>') != null && document.getElementById('<%=TxtBannerURL.ClientID %>').value == '') {
                    //                jAlert('Please select banner url.', 'Required', '<%=TxtBannerURL.ClientID %>');
                    //                return false;
                    //            }

                else if (fileName == '' && document.getElementById('<%=imghdn.ClientID %>').value == '') {
                    jAlert('Please select image file.', 'Required', '<%=FileUploadBanner.ClientID %>');
                    return false;
                }
                else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn.ClientID %>').value == '') {
                    jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner.ClientID %>');
                    return false;
                }
            }
            else {


                if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '') {

                    jAlert('Please enter start date.', 'Required', '<%=txtStartDate.ClientID %>');
                    return false;
                }
                if (document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {

                    jAlert('Please enter End date.', 'Required', '<%=txtEndDate.ClientID %>');
                    return false;
                }

                var date1 = new Date(document.getElementById('<%=txtStartDate.ClientID %>').value);
                var date2 = new Date(document.getElementById('<%=txtEndDate.ClientID %>').value);

                if (date1 > date2) {
                    jAlert('Start date must be less than or equal to end date.', 'Required', '<%=txtStartDate.ClientID %>');
                    return false;
                }

                if (document.getElementById('<%=TxtbannerTitle.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle.ClientID %>').value == '') {
                    jAlert('Please enter banner title.', 'Required', '<%=TxtbannerTitle.ClientID %>');
                    return false;
                }
                    //            else if (document.getElementById('<%=TxtBannerURL.ClientID %>') != null && document.getElementById('<%=TxtBannerURL.ClientID %>').value == '') {
                    //                jAlert('Please select banner url.', 'Required', '<%=TxtBannerURL.ClientID %>');
                    //                return false;
                    //            }

                else if (fileName == '' && document.getElementById('<%=imghdn.ClientID %>').value == '') {
                    jAlert('Please select image file.', 'Required', '<%=FileUploadBanner.ClientID %>');
                    return false;
                }
                else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn.ClientID %>').value == '') {
                    jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner.ClientID %>');
                    return false;
                }
            }

    return true;
}
function clearfield() {



    if (document.getElementById('<%=imghdn.ClientID %>') != null) {
        document.getElementById('<%=imghdn.ClientID %>').value = '';

    }
    if (document.getElementById('<%=hdnid.ClientID %>') != null) {
        document.getElementById('<%=hdnid.ClientID %>').value = '0';

            }
            if (document.getElementById('<%=TxtBannerURL.ClientID %>') != null) {
        document.getElementById('<%=TxtBannerURL.ClientID %>').value = ''
            }
            if (document.getElementById('<%=TxtbannerTitle.ClientID %>') != null) {
        document.getElementById('<%=TxtbannerTitle.ClientID %>').value = ''
            }
            if (document.getElementById('<%=TxtDisplayOrder.ClientID %>') != null) {
        document.getElementById('<%=TxtDisplayOrder.ClientID %>').value = ''
            }
            if (document.getElementById('<%=chkActive.ClientID %>') != null) {
        document.getElementById('<%=chkActive.ClientID %>').checked = false;
            }
            if (document.getElementById('<%=FileUploadBanner.ClientID %>') != null) {
        document.getElementById('<%=FileUploadBanner.ClientID %>').value = '';
            }
            if (document.getElementById('<%=txtLeftBannerText.ClientID %>') != null) {
        document.getElementById('<%=txtLeftBannerText.ClientID %>').value = '';
            }
            if (document.getElementById('<%=ddlTarget.ClientID %>') != null) {
        document.getElementById('<%=ddlTarget.ClientID %>').selectedIndex = 0;
            }
            if (document.getElementById('<%=ddlPosition.ClientID %>') != null) {
        document.getElementById('<%=ddlPosition.ClientID %>').selectedIndex = 0;
            }
            if (document.getElementById('<%=imgBanner.ClientID %>') != null) {
        document.getElementById('<%=imgBanner.ClientID %>').src = "/App_Themes/<%=Page.Theme %>/images/spacer.png";
            }

            return false;
        }
        //        function clearfield1() {

        //            if (document.getElementById('<%=imghdn1.ClientID %>') != null && document.getElementById('<%=imghdn1.ClientID %>').value == '') {
        //                document.getElementById('<%=imghdn1.ClientID %>').value = '0';

        //            }
        //            if (document.getElementById('<%=hdnid1.ClientID %>') != null && document.getElementById('<%=hdnid1.ClientID %>').value == '') {
        //                document.getElementById('<%=hdnid1.ClientID %>').value = '0';

        //            }
        //            if (document.getElementById('<%=TxtBannerURL1.ClientID %>') != null) {
        //                document.getElementById('<%=TxtBannerURL1.ClientID %>').value == ''
        //            }
        //            
        //            if (document.getElementById('<%=TxtDisplayOrder1.ClientID %>') != null) {
        //                document.getElementById('<%=TxtDisplayOrder1.ClientID %>').value == ''
        //            }
        //            if (document.getElementById('<%=chkActive1.ClientID %>') != null) {
        //                document.getElementById('<%=chkActive1.ClientID %>').checked = false;
        //            }
        //            if (document.getElementById('<%=FileUploadBanner1.ClientID %>') != null) {
        //                document.getElementById('<%=FileUploadBanner1.ClientID %>').value = '';
        //            }

        //            return false;
        //        }
        //        function clearfield2() {

        //            if (document.getElementById('<%=imghdn2.ClientID %>') != null && document.getElementById('<%=imghdn2.ClientID %>').value == '') {
        //                document.getElementById('<%=imghdn2.ClientID %>').value = '0';

        //            }
        //            if (document.getElementById('<%=hdnid2.ClientID %>') != null && document.getElementById('<%=hdnid2.ClientID %>').value == '') {
        //                document.getElementById('<%=hdnid2.ClientID %>').value = '0';

        //            }
        //            if (document.getElementById('<%=TxtBannerURL2.ClientID %>') != null) {
        //                document.getElementById('<%=TxtBannerURL2.ClientID %>').value == ''
        //            }

        //            if (document.getElementById('<%=TxtDisplayOrder2.ClientID %>') != null) {
        //                document.getElementById('<%=TxtDisplayOrder2.ClientID %>').value == ''
        //            }
        //            if (document.getElementById('<%=chkActive2.ClientID %>') != null) {
        //                document.getElementById('<%=chkActive2.ClientID %>').checked = false;
        //            }
        //            if (document.getElementById('<%=FileUploadBanner2.ClientID %>') != null) {
        //                document.getElementById('<%=FileUploadBanner2.ClientID %>').value = '';
        //            }

        //            return false;
        //        }
        //        function clearfield2() {

        //            if (document.getElementById('<%=imghdn2.ClientID %>') != null && document.getElementById('<%=imghdn2.ClientID %>').value == '') {
        //                document.getElementById('<%=imghdn2.ClientID %>').value = '0';

        //            }
        //            if (document.getElementById('<%=hdnid2.ClientID %>') != null && document.getElementById('<%=hdnid2.ClientID %>').value == '') {
        //                document.getElementById('<%=hdnid2.ClientID %>').value = '0';

        //            }
        //            if (document.getElementById('<%=TxtBannerURL2.ClientID %>') != null) {
        //                document.getElementById('<%=TxtBannerURL2.ClientID %>').value == ''
        //            }

        //            if (document.getElementById('<%=TxtDisplayOrder2.ClientID %>') != null) {
        //                document.getElementById('<%=TxtDisplayOrder2.ClientID %>').value == ''
        //            }
        //            if (document.getElementById('<%=chkActive2.ClientID %>') != null) {
        //                document.getElementById('<%=chkActive2.ClientID %>').checked = false;
        //            }
        //            if (document.getElementById('<%=FileUploadBanner2.ClientID %>') != null) {
        //                document.getElementById('<%=FileUploadBanner2.ClientID %>').value = '';
        //            }

        //            return false;
        //        }
        function Checkfields1() {
            // document.getElementById('').value = '';
            var fup = document.getElementById('<%=FileUploadBanner1.ClientID %>');
            var fileName = fup.value;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            ext = ext.toLowerCase();
            if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '' && document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {
                if (document.getElementById('<%=TxtbannerTitle1.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle1.ClientID %>').value == '') {
                    jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle1.ClientID %>');
                     return false;
                 }
                 else if (fileName == '' && document.getElementById('<%=imghdn1.ClientID %>').value == '') {
                     jAlert('Please select image file.', 'Required', '<%=FileUploadBanner1.ClientID %>');
                    return false;
                }
                else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn1.ClientID %>').value == '') {
                    jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner1.ClientID %>');
                    return false;
                }
            }
            else {


                if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '') {

                    jAlert('Please enter start date.', 'Required', '<%=txtStartDate.ClientID %>');
                    return false;
                }
                if (document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {

                    jAlert('Please enter End date.', 'Required', '<%=txtEndDate.ClientID %>');
                    return false;
                }

                var date1 = new Date(document.getElementById('<%=txtStartDate.ClientID %>').value);
                var date2 = new Date(document.getElementById('<%=txtEndDate.ClientID %>').value);

                if (date1 > date2) {
                    jAlert('Start date must be less than or equal to end date.', 'Required', '<%=txtStartDate.ClientID %>');
                    return false;
                }

                if (document.getElementById('<%=TxtbannerTitle1.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle1.ClientID %>').value == '') {
                    jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle1.ClientID %>');
                    return false;
                }
                else if (fileName == '' && document.getElementById('<%=imghdn1.ClientID %>').value == '') {
                    jAlert('Please select image file.', 'Required', '<%=FileUploadBanner1.ClientID %>');
                    return false;
                }
                else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn1.ClientID %>').value == '') {
                    jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner1.ClientID %>');
                    return false;
                }
            }

    return true;
}
function Checkfields2() {
    // document.getElementById('').value = '';
    var fup = document.getElementById('<%=FileUploadBanner2.ClientID %>');
    var fileName = fup.value;
    var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
    ext = ext.toLowerCase();
    if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '' && document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {

        if (document.getElementById('<%=TxtbannerTitle2.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle2.ClientID %>').value == '') {
            jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle2.ClientID %>');
            return false;
        }
        else if (fileName == '' && document.getElementById('<%=imghdn2.ClientID %>').value == '') {
            jAlert('Please select image file.', 'Required', '<%=FileUploadBanner2.ClientID %>');
            return false;
        }
        else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn2.ClientID %>').value == '') {
            jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner2.ClientID %>');
            return false;
        }
    }
    else {


        if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '') {

            jAlert('Please enter start date.', 'Required', '<%=txtStartDate.ClientID %>');
            return false;
        }
        if (document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {

            jAlert('Please enter End date.', 'Required', '<%=txtEndDate.ClientID %>');
            return false;
        }

        var date1 = new Date(document.getElementById('<%=txtStartDate.ClientID %>').value);
        var date2 = new Date(document.getElementById('<%=txtEndDate.ClientID %>').value);

        if (date1 > date2) {
            jAlert('Start date must be less than or equal to end date.', 'Required', '<%=txtStartDate.ClientID %>');
            return false;
        }

        if (document.getElementById('<%=TxtbannerTitle2.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle2.ClientID %>').value == '') {
            jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle2.ClientID %>');
            return false;
        }
        else if (fileName == '' && document.getElementById('<%=imghdn2.ClientID %>').value == '') {
            jAlert('Please select image file.', 'Required', '<%=FileUploadBanner2.ClientID %>');
            return false;
        }
        else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn2.ClientID %>').value == '') {
            jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner2.ClientID %>');
            return false;
        }
    }

    return true;
}
function Checkfields3() {
    // document.getElementById('').value = '';
    var fup = document.getElementById('<%=FileUploadBanner3.ClientID %>');
    var fileName = fup.value;
    var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
    ext = ext.toLowerCase();
    if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '' && document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {
        if (document.getElementById('<%=TxtbannerTitle3.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle3.ClientID %>').value == '') {
            jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle3.ClientID %>');
             return false;
         }
         else if (fileName == '' && document.getElementById('<%=imghdn3.ClientID %>').value == '') {
             jAlert('Please select image file.', 'Required', '<%=FileUploadBanner3.ClientID %>');
            return false;
        }
        else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn3.ClientID %>').value == '') {
            jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner3.ClientID %>');
            return false;
        }

    }
    else {


        if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '') {

            jAlert('Please enter start date.', 'Required', '<%=txtStartDate.ClientID %>');
            return false;
        }
        if (document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {

            jAlert('Please enter End date.', 'Required', '<%=txtEndDate.ClientID %>');
            return false;
        }

        var date1 = new Date(document.getElementById('<%=txtStartDate.ClientID %>').value);
        var date2 = new Date(document.getElementById('<%=txtEndDate.ClientID %>').value);

        if (date1 > date2) {
            jAlert('Start date must be less than or equal to end date.', 'Required', '<%=txtStartDate.ClientID %>');
            return false;
        }

        if (document.getElementById('<%=TxtbannerTitle3.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle3.ClientID %>').value == '') {
            jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle3.ClientID %>');
            return false;
        }
        else if (fileName == '' && document.getElementById('<%=imghdn3.ClientID %>').value == '') {
            jAlert('Please select image file.', 'Required', '<%=FileUploadBanner3.ClientID %>');
            return false;
        }
        else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn3.ClientID %>').value == '') {
            jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner3.ClientID %>');
            return false;
        }
    }
    return true;
}

function Checkfields4() {
    var fup = document.getElementById('<%=FileUploadBanner4.ClientID %>');
    var fileName = fup.value;
    var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
    ext = ext.toLowerCase();
    if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '' && document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '')
    {
        if (document.getElementById('<%=TxtbannerTitle4.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle4.ClientID %>').value == '') {
            jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle4.ClientID %>');
            return false;
        }
        else if (fileName == '' && document.getElementById('<%=imghdn4.ClientID %>').value == '') {
            jAlert('Please select image file.', 'Required', '<%=FileUploadBanner4.ClientID %>');
             return false;
         }
         else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn4.ClientID %>').value == '') {
             jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner4.ClientID %>');
            return false;
        }

}
else
    {

        if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '') {

            jAlert('Please enter start date.', 'Required', '<%=txtStartDate.ClientID %>');
            return false;
        }
        if (document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {

            jAlert('Please enter End date.', 'Required', '<%=txtEndDate.ClientID %>');
            return false;
        }

        var date1 = new Date(document.getElementById('<%=txtStartDate.ClientID %>').value);
        var date2 = new Date(document.getElementById('<%=txtEndDate.ClientID %>').value);

        if (date1 > date2) {
            jAlert('Start date must be less than or equal to end date.', 'Required', '<%=txtStartDate.ClientID %>');
            return false;
        }

        if (document.getElementById('<%=TxtbannerTitle4.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle4.ClientID %>').value == '') {
            jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle4.ClientID %>');
            return false;
        }
        else if (fileName == '' && document.getElementById('<%=imghdn4.ClientID %>').value == '') {
            jAlert('Please select image file.', 'Required', '<%=FileUploadBanner4.ClientID %>');
            return false;
        }
        else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn4.ClientID %>').value == '') {
            jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner4.ClientID %>');
            return false;
        }
}
    return true;
}
        function Checkfields5()
        {
            var fup = document.getElementById('<%=FileUploadBanner5.ClientID %>');
            var fileName = fup.value;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            ext = ext.toLowerCase();
            if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '' && document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {
        if (document.getElementById('<%=TxtbannerTitle5.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle5.ClientID %>').value == '') {
            jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle5.ClientID %>');
            return false;
        }
        else if (fileName == '' && document.getElementById('<%=imghdn5.ClientID %>').value == '') {
            jAlert('Please select image file.', 'Required', '<%=FileUploadBanner5.ClientID %>');
            return false;
        }
        else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn5.ClientID %>').value == '') {
            jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner5.ClientID %>');
             return false;
         }

}
else {

    if (document.getElementById('<%=txtStartDate.ClientID %>') != null && document.getElementById('<%=txtStartDate.ClientID %>').value == '') {

            jAlert('Please enter start date.', 'Required', '<%=txtStartDate.ClientID %>');
            return false;
        }
        if (document.getElementById('<%=txtEndDate.ClientID %>') != null && document.getElementById('<%=txtEndDate.ClientID %>').value == '') {

            jAlert('Please enter End date.', 'Required', '<%=txtEndDate.ClientID %>');
            return false;
        }

        var date1 = new Date(document.getElementById('<%=txtStartDate.ClientID %>').value);
        var date2 = new Date(document.getElementById('<%=txtEndDate.ClientID %>').value);

        if (date1 > date2) {
            jAlert('Start date must be less than or equal to end date.', 'Required', '<%=txtStartDate.ClientID %>');
            return false;
        }

        if (document.getElementById('<%=TxtbannerTitle5.ClientID %>') != null && document.getElementById('<%=TxtbannerTitle5.ClientID %>').value == '') {
            jAlert('Please enter title.', 'Required', '<%=TxtbannerTitle5.ClientID %>');
            return false;
        }
        else if (fileName == '' && document.getElementById('<%=imghdn5.ClientID %>').value == '') {
            jAlert('Please select image file.', 'Required', '<%=FileUploadBanner5.ClientID %>');
            return false;
        }
        else if ((ext != "gif" && ext != "jpeg" && ext != "jpg" && ext != "png" && ext != "bmp") && document.getElementById('<%=imghdn5.ClientID %>').value == '') {
            jAlert('Please select only image file.', 'Required', '<%=FileUploadBanner5.ClientID %>');
            return false;
        }
}
    return true;
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

    </script>
    <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order" style="width: 100%; padding-top: 5px;">
                <table>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%= Page.Theme %>/images/spacer.gif" width="1" height="5" />
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
                                            <th align="left" colspan="2">
                                                <div class="main-title-left">
                                                    <h2>
                                                        <asp:Label ID="lblTitle" runat="server" Text="Add Banner Detail">
                                                        </asp:Label>
                                                    </h2>
                                                </div>
                                                <div style="float: right;">
                                                    <h2>
                                                        <asp:ImageButton ID="imgback" runat="server" OnClick="imgback_Click" />
                                                    </h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <span style="color: Red;">*</span><asp:Label ID="Label2" runat="server"> is Required Field</asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="even-row">
                                            <td colspan="100%" colspan="2" valign="top">
                                                <table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#ffffff"
                                                    class="content-table">
                                                    <tbody>
                                                        <tr>
                                                            <td class="border-td-sub" colspan="2" width="100%" valign="top">
                                                                <fieldset class="fldset" style="width: 98%;">
                                                                    <legend>Banner Schedule</legend>
                                                                    <table align="left" class="add-product" border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <span style="color: Red;">&nbsp;</span>Start Date
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox runat="server" ID="txtStartDate" CssClass="order-textfield" MaxLength="200"
                                                                                    Style="width: 90px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="altrow">
                                                                            <td align="left">
                                                                                <span style="color: Red;">&nbsp;</span> End Date
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox runat="server" ID="txtEndDate" CssClass="order-textfield" MaxLength="200"
                                                                                    Style="width: 90px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="left">
                                                                            <td align="left">
                                                                                 <span style="color: Red;">*</span> Display Order
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtgroupdisplayorder" onkeypress="return keyRestrict(event,'0123456789');" runat="server" CssClass="order-textfield" Style="width: 90px"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgDateSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                                    OnClick="imgDateSave_Click" OnClientClick="return chkDate();" Visible="false" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="border-td-sub" width="25%" valign="top">
                                                                <fieldset class="fldset" style="width: 95%;">
                                                                    <legend>Banner1</legend>
                                                                    <table align="left" class="add-product" border="0" cellpadding="0" cellspacing="0"
                                                                        width="100%">
                                                                        <tr>
                                                                            <td colspan="2" align="center">
                                                                                <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" valign="top">
                                                                                <span style="color: Red;">*</span> Banner Title :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="TxtbannerTitle" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="altrow">
                                                                            <td align="left" valign="top">
                                                                                <span style="color: Red;">&nbsp;</span> Banner URL :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="TxtBannerURL" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" valign="top">
                                                                                <span style="color: Red;">*</span> Target :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlTarget" runat="server" CssClass="order-list">
                                                                                    <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                                                    <asp:ListItem Value="_self">Self</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="altrow" style="display:none;">
                                                                            <td align="left" valign="top">
                                                                                <span style="color: Red;">*</span> Pagination Position :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:DropDownList ID="ddlPosition" runat="server" CssClass="order-list">
                                                                                    <asp:ListItem Value="Left">Left</asp:ListItem>
                                                                                    <asp:ListItem Value="Top">Top</asp:ListItem>
                                                                                    <asp:ListItem Value="Right">Right</asp:ListItem>
                                                                                    <asp:ListItem Value="Bottom">Bottom</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>

                                                                        <tr style="display:none;">
                                                                            <td align="left" valign="top">
                                                                                    &nbsp;&nbsp;Left Banner Text
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtLeftBannerText" Width="350px" runat="server"  TextMode="MultiLine"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="table_bg">
                                                                            <td align="left" class="font-black01" valign="top">
                                                                                <span style="color: Red;">*</span> Banner Image :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:FileUpload ID="FileUploadBanner" runat="server" />
                                                                                <asp:Label ID="lblImgSize" runat="server" Font-Bold="true" ForeColor="Red" Text="Size should be 695 x 305"></asp:Label><%--Width=960px,Height=245--%>
                                                                                <%--  <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="FileUploadBanner"
                                                ErrorMessage="Select File" CssClass="rferror" SetFocusOnError="True" ValidationGroup="AppConfig"></asp:RequiredFieldValidator>--%>
                                                                                <div>
                                                                                    <br />
                                                                                    <img id="imgBanner" runat="server" style="height: 100%" /></div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="altrow" style="display:none;">
                                                                            <td align="left" valign="top">
                                                                                &nbsp;&nbsp; Display Order :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="TxtDisplayOrder" Width="40px" Style="text-align: center" MaxLength="2" Text="1"
                                                                                    onkeypress="return keyRestrict(event,'0123456789');" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="display:none;">
                                                                            <td align="left" valign="top">
                                                                                &nbsp;&nbsp;&nbsp;Active :
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" height="30" valign="top" style="width: 137px">
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                                                    OnClick="imgSave_Click" OnClientClick="return Checkfields();" />
                                                                               
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="display:none;">
                                                                                <asp:GridView ID="grdbannerlist" runat="server" AutoGenerateColumns="False" DataKeyNames="BannerID" 
                                                                                    EmptyDataText="No Record(s) Found." AllowSorting="True" EmptyDataRowStyle-ForeColor="Red"
                                                                                    EmptyDataRowStyle-HorizontalAlign="Center" ViewStateMode="Enabled" Width="100%"
                                                                                    GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                                                    PagerSettings-Mode="NumericFirstLast" OnRowDataBound="grdbannerlist_RowDataBound"
                                                                                    OnRowDeleting="grdbannerlist_RowDeleting" OnRowEditing="grdbannerlist_RowEditing">
                                                                                    <Columns>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderTemplate>
                                                                                                &nbsp;Banner Title
                                                                                            </HeaderTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                                                            <ItemTemplate>
                                                                                                &nbsp;<asp:Label ID="lblTitle" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderTemplate>
                                                                                                &nbsp;Banner URL
                                                                                            </HeaderTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                                                            <ItemTemplate>
                                                                                                &nbsp;<asp:Label ID="lblUrl" runat="server" Text='<%# Bind("BannerURL") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderTemplate>
                                                                                                &nbsp;Display Order
                                                                                            </HeaderTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                                                            <ItemTemplate>
                                                                                                &nbsp;<asp:Label ID="lblorder" runat="server" Text='<%# Bind("DisplayOrder") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderTemplate>
                                                                                                &nbsp;Active
                                                                                            </HeaderTemplate>
                                                                                            <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                                                                            <ItemTemplate>
                                                                                                <input type="hidden" id="hdnActive" runat="server" value='<%# Bind("Active") %>' />
                                                                                                <asp:Literal ID="ltStatus" runat="server"></asp:Literal>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Operations">
                                                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:ImageButton runat="server" ID="_editLinkButton" ToolTip="Edit" CommandName="edit"
                                                                                                    CommandArgument='<%# Eval("BannerID") %>'></asp:ImageButton>
                                                                                                <asp:ImageButton runat="server" ID="_deleteLinkButton" ToolTip="Delete" CommandName="Delete"
                                                                                                    CommandArgument='<%# Eval("BannerID") %>' message="Are you sure want to delete this record?"
                                                                                                    OnClientClick='return confirm(this.getAttribute("message"))'></asp:ImageButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                                                    <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                                                    <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                                                    <AlternatingRowStyle CssClass="altrow" />
                                                                                    <HeaderStyle ForeColor="White" Font-Bold="false" />
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                            <td align="left" width="25%" valign="top" id="tdSmallbanner" runat="server" >
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td align="left">
                                                                            <fieldset class="fldset" style="width: 95%;">
                                                                                <legend>Banner2</legend>
                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2" align="Center">
                                                                                            <asp:Label ID="lblMsg1" runat="server" CssClass="error"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Banner Title :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtbannerTitle1" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow">
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">&nbsp;</span> Banner URL :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtBannerURL1" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Target :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:DropDownList ID="ddlTarget1" runat="server" CssClass="order-list">
                                                                                                <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                                                                <asp:ListItem Value="_self">Self</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="table_bg">
                                                                                        <td align="left" class="font-black01" valign="top">
                                                                                            <span style="color: Red;">*</span> Banner Image :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:FileUpload ID="FileUploadBanner1" runat="server" />
                                                                                            <asp:Label ID="lblImgSize1" Font-Bold="true" ForeColor="Red" runat="server" Text="Size should be 318 x 400"></asp:Label><%--Width=960px,Height=245--%>
                                                                                            <%--  <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="FileUploadBanner"
                                                ErrorMessage="Select File" CssClass="rferror" SetFocusOnError="True" ValidationGroup="AppConfig"></asp:RequiredFieldValidator>--%>
                                                                                            <div>
                                                                                                <br />
                                                                                                <img id="imgBanner1" runat="server" style="height: 100%" /></div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow" style="display:none;">
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp; Display Order :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtDisplayOrder1" Width="40px" Style="text-align: center" MaxLength="2" Text="1"
                                                                                                onkeypress="return keyRestrict(event,'0123456789');" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="display:none;">
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp;&nbsp;Active :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:CheckBox ID="chkActive1" Checked="true" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" height="30" valign="top" style="width: 137px">
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:ImageButton ID="imgSave1" runat="server" AlternateText="Save" ToolTip="Save"
                                                                                                OnClick="imgSave1_Click" OnClientClick="return Checkfields1();" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="display:none;">
                                                                        <td align="left" valign="top">
                                                                           
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="display:none;" >
                                                                        <td align="left" valign="top">
                                                                            <fieldset class="fldset" style="width: 95%;">
                                                                                <legend>Small Banner3</legend>
                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2" align="Center">
                                                                                            <asp:Label ID="lblMsg3" runat="server" CssClass="error"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Banner Title :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtbannerTitle3" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">&nbsp;</span> Banner URL :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtBannerURL3" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow">
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Target :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:DropDownList ID="ddlTarget3" runat="server" CssClass="order-list">
                                                                                                <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                                                                <asp:ListItem Value="_self">Self</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="table_bg">
                                                                                        <td align="left" class="font-black01" valign="top">
                                                                                            <span style="color: Red;">*</span>Banner Image :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:FileUpload ID="FileUploadBanner3" runat="server" />
                                                                                            <asp:Label ID="lblImgSize3" runat="server" Text="Size should be 318 x 400"></asp:Label><%--Width=960px,Height=245--%>
                                                                                            <%--  <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="FileUploadBanner"
                                                ErrorMessage="Select File" CssClass="rferror" SetFocusOnError="True" ValidationGroup="AppConfig"></asp:RequiredFieldValidator>--%>
                                                                                            <div>
                                                                                                <br />
                                                                                                <img id="imgBanner3" runat="server" style="width: 100%" /></div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow">
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp; Display Order :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtDisplayOrder3" Width="40px" Style="text-align: center" MaxLength="2"
                                                                                                onkeypress="return keyRestrict(event,'0123456789');" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp;&nbsp;Active :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:CheckBox ID="chkActive3" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" height="30" valign="top" style="width: 137px">
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:ImageButton ID="imgSave3" runat="server" AlternateText="Save" ToolTip="Save"
                                                                                                OnClick="imgSave3_Click" OnClientClick="return Checkfields3();" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                </table>
                                                            </td>
                                                             <td align="left" width="25%" valign="top" id="tdSmallbanner2" runat="server">
                                                                 <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td align="left">
                                                                             <fieldset class="fldset" style="width: 95%;">
                                                                                <legend>Banner3</legend>
                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2" align="Center">
                                                                                            <asp:Label ID="lblMsg2" runat="server" CssClass="error"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Banner Title :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtbannerTitle2" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow">
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">&nbsp;</span> Banner URL :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtBannerURL2" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Target :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:DropDownList ID="ddlTarget2" runat="server" CssClass="order-list">
                                                                                                <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                                                                <asp:ListItem Value="_self">Self</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="table_bg">
                                                                                        <td align="left" class="font-black01" valign="top">
                                                                                            <span style="color: Red;">*</span>Banner Image :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:FileUpload ID="FileUploadBanner2" runat="server" />
                                                                                            <asp:Label ID="lblImgSize2" Font-Bold="true" ForeColor="Red" runat="server" Text="Size should be 318 x 400"></asp:Label><%--Width=960px,Height=245--%>
                                                                                            <%--  <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="FileUploadBanner"
                                                ErrorMessage="Select File" CssClass="rferror" SetFocusOnError="True" ValidationGroup="AppConfig"></asp:RequiredFieldValidator>--%>
                                                                                            <div>
                                                                                                <br />
                                                                                                <img id="imgBanner2" runat="server" style="height: 100%" /></div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow" style="display:none;">
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp; Display Order :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtDisplayOrder2" Width="40px" Style="text-align: center" MaxLength="2" Text="2"
                                                                                                onkeypress="return keyRestrict(event,'0123456789');" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="display:none;">
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp;&nbsp;Active :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:CheckBox ID="chkActive2" runat="server" Checked="true"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" height="30" valign="top" style="width: 137px">
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:ImageButton ID="imgSave2" runat="server" AlternateText="Save" ToolTip="Save"
                                                                                                OnClick="imgSave2_Click" OnClientClick="return Checkfields2();" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                            </td>
                                                                        </tr>
                                                                        </table>
                                                                 </td>
                                                             <td align="left" width="25%" valign="top" id="tdSmallbanner3" runat="server">
                                                                 <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>

                                                            <td align="left" valign="top">
                                                                            <fieldset class="fldset" style="width: 95%;">
                                                                                <legend>Banner4</legend>
                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2" align="Center">
                                                                                            <asp:Label ID="lblMsg4" runat="server" CssClass="error"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Banner Title :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtbannerTitle4" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow">
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">&nbsp;</span> Banner URL :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtBannerURL4" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Target :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:DropDownList ID="ddlTarget4" runat="server" CssClass="order-list">
                                                                                                <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                                                                <asp:ListItem Value="_self">Self</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="table_bg">
                                                                                        <td align="left" class="font-black01" valign="top">
                                                                                            <span style="color: Red;">*</span>Banner Image :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:FileUpload ID="FileUploadBanner4" runat="server" />
                                                                                            <asp:Label ID="lblImgSize4" runat="server" Font-Bold="true" ForeColor="Red" Text="Size should be 378 x 352"></asp:Label><%--Width=960px,Height=445--%>
                                                                                            <%--  <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="FileUploadBanner"
                                                ErrorMessage="Select File" CssClass="rferror" SetFocusOnError="True" ValidationGroup="AppConfig"></asp:RequiredFieldValidator>--%>
                                                                                            <div>
                                                                                                <br />
                                                                                                <img id="imgBanner4" runat="server" style="width: 100%" /></div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow" style="display:none;">
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp; Display Order :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtDisplayOrder4" Width="40px" Style="text-align: center" MaxLength="4"
                                                                                                onkeypress="return keyRestrict(event,'0143456789');" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="display:none;">
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp;&nbsp;Active :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:CheckBox ID="chkActive4" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" height="30" valign="top" style="width: 137px">
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:ImageButton ID="imgSave4" runat="server" AlternateText="Save" ToolTip="Save"
                                                                                                OnClick="imgSave4_Click" OnClientClick="return Checkfields4();" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                        </tr>
                                                                     <tr style="display:none;">

                                                            <td align="left" valign="top">
                                                                            <fieldset class="fldset" style="width: 95%;">
                                                                                <legend>Small Banner</legend>
                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2" align="Center">
                                                                                            <asp:Label ID="lblMsg5" runat="server" CssClass="error"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Banner Title :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtbannerTitle5" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow">
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">&nbsp;</span> Banner URL :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtBannerURL5" Width="350px" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" valign="top">
                                                                                            <span style="color: Red;">*</span> Target :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:DropDownList ID="ddlTarget5" runat="server" CssClass="order-list">
                                                                                                <asp:ListItem Value="_blank">Blank</asp:ListItem>
                                                                                                <asp:ListItem Value="_self">Self</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="table_bg">
                                                                                        <td align="left" class="font-black01" valign="top">
                                                                                            <span style="color: Red;">*</span>Banner Image :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:FileUpload ID="FileUploadBanner5" runat="server" />
                                                                                            <asp:Label ID="lblImgSize5" runat="server" Font-Bold="true" ForeColor="Red" Text="Size should be 780 x 90"></asp:Label><%--Width=960px,Height=445--%>
                                                                                            <%--  <asp:RequiredFieldValidator ID="RFVName" runat="server" ControlToValidate="FileUploadBanner"
                                                ErrorMessage="Select File" CssClass="rferror" SetFocusOnError="True" ValidationGroup="AppConfig"></asp:RequiredFieldValidator>--%>
                                                                                            <div>
                                                                                                <br />
                                                                                                <img id="imgBanner5" runat="server" style="width: 100%" /></div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr class="altrow" style="display:none;">
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp; Display Order :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="TxtDisplayOrder5" Width="40px" Style="text-align: center" MaxLength="4"
                                                                                                onkeypress="return keyRestrict(event,'0143456789');" runat="server" CssClass="order-textfield"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="display:none;">
                                                                                        <td align="left" valign="top">
                                                                                            &nbsp;&nbsp;&nbsp;Active :
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:CheckBox ID="chkActive5" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" height="30" valign="top" style="width: 137px">
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:ImageButton ID="imgSave5" runat="server" AlternateText="Save" ToolTip="Save"
                                                                                                OnClick="imgSave5_Click" OnClientClick="return Checkfields5();" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                        </tr>
                                                                     </table>
                                                                 </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">

                                                            
                                                            <table align="left" class="add-product" border="0" cellpadding="0" cellspacing="0"
                                                                        width="100%">
                                                                        <tr>
                                                            <td align="center">
                                                                 <asp:ImageButton ID="imgCancle" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                                    OnClick="imgCancle_Click" />
                                                            </td>
                                                                            </tr>
                                                                </table>
                                                                </td>
                                                        </tr>
                                                    </tbody>
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
    <div style="display: none;">
        <input type="hidden" id="imghdn" runat="server" value="" />
        <input type="hidden" id="imghdn1" runat="server" value="" />
        <input type="hidden" id="imghdn2" runat="server" value="" />
        <input type="hidden" id="imghdn3" runat="server" value="" />
        <input type="hidden" id="imghdn4" runat="server" value="" />
         <input type="hidden" id="imghdn5" runat="server" value="" />
        <input type="hidden" id="hdnid" runat="server" value="0" />
        <input type="hidden" id="hdnid1" runat="server" value="0" />
        <input type="hidden" id="hdnid2" runat="server" value="0" />
        <input type="hidden" id="hdnid3" runat="server" value="0" />
        <input type="hidden" id="hdnid4" runat="server" value="0" />
         <input type="hidden" id="hdnid5" runat="server" value="0" />

    </div>
</asp:Content>
