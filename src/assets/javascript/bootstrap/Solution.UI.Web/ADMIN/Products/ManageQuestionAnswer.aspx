<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ManageQuestionAnswer.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.ManageQuestionAnswer" %>

<%@ Register Assembly="Castle.Web.Controls.Rater" Namespace="Castle.Web.Controls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript" src="/js/popup.js"></script>
    <script language="javascript" type="text/javascript">
        function selectAll(on) {
            var myform = document.forms[0];
            var len = myform.elements.length;
            for (var i = 0; i < len; i++) {
                if (myform.elements[i].type == 'checkbox') {
                    if (on.toString() == 'false') {
                        if (myform.elements[i].checked) {
                            myform.elements[i].checked = false;
                        }
                    }
                    else {
                        myform.elements[i].checked = true;
                    }
                }
            }

        }
    </script>
    <script type="text/javascript">
        function checkondelete(id, name) {
            var a;
            if (name == "Approve") {
                a = 'Are you sure want to Approve?';
            }
            else
                a = 'Are you sure want to Disapprove?';
            jConfirm(a, 'Confirmation', function (r) {
                if (r == true) {
                    document.getElementById("ContentPlaceHolder1_hdnDelete").value = id;
                    document.getElementById("ContentPlaceHolder1_hdnCommandName").value = name;
                    document.getElementById("ContentPlaceHolder1_hdnCommandNameQAR").value = id;
                    document.getElementById("ContentPlaceHolder1_btnhdnDelete").click();
                    return true;
                }
                else {
                    return false;
                }
            });
            return false;
        }

        //function DeleteConfirmation(imgname, imagename) {
        //    $('#ContentPlaceHolder1_RatingImageID').val(imgname);
        //    $('#ContentPlaceHolder1_RatingImageName').val(imagename);
        //    var message = 'Are you sure want to Delete this image ?';
        //    if (confirm(message)) {

        //        $.ajax({
        //            type: "POST",
        //            url: "ProductRatingNew.aspx/DeleteCurrentImage",
        //            data: '{RatingImageID: "' + imgname + '",RatingImageName: "' + imagename + '" }',
        //            contentType: "application/json; charset=utf-8",
        //            dataType: "json",
        //            success: function (response) {
        //                var elementid = 'innerpnl_' + imgname;
        //                var elem = document.getElementById(elementid);
        //                elem.parentNode.removeChild(elem);
        //                $('#' + elementid).remove();
        //                jAlert(response.d, 'Message');
        //                //alert(response.d);
        //            },
        //            failure: function (response) {
        //                jAlert(response.d, 'Message');
        //                //alert(response.d);
        //            }
        //        });
        //    }
        //    return false;
        //}
    </script>
    <script language="javascript" type="text/javascript">
        function checkCount() {
            debugger;
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
                $(document).ready(function () { jAlert('Check at least One Question or Answer or Reply !', 'Message'); });
                return false;
            }
            else {
                var message = "Do you want to Approve all selected Question or Answer or Reply ?";
                return confirm(message);
            }
        }
        function checkCountfordis() {
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
                $(document).ready(function () { jAlert('Check at least One Question or Answer or Reply !', 'Message'); });
                return false;
            }
            else {
                var message = "Do you want to DisApprove all selected Question or Answer or Reply ?";
                return confirm(message);
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="content-row1">
        <div class="create-new-order" style="width: 100%;">
            <span style="vertical-align: middle; margin-right: 3px; float: left;">
                <table style="margin-top: 4px; float: left">
                    <tr>
                        <td align="right">Store :
                        </td>
                        <td>&nbsp;&nbsp;
                        <asp:DropDownList ID="drpstore" runat="server" Width="170px" AutoPostBack="true"
                            CssClass="order-list" OnSelectedIndexChanged="drpstore_SelectedIndexChanged">
                        </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </span>
        </div>
    </div>
    <div class="content-row2">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td height="5" align="left" valign="top">
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="5" />
                </td>
            </tr>
            <tr>
                <td class="border-td">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF"
                        class="content-table">
                        <%--<tr>
                            <span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                                href="/Admin/Products/AddProductRating.aspx">
                                <img alt="Add Review" title="Add Review" src="/App_Themes/<%=Page.Theme %>/images/add-product-review.png" /></a></span>
                        </tr>--%>
                        <tr>
                            <td class="border-td-sub">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="add-product">
                                    <tr>
                                        <th>
                                            <div class="main-title-left">
                                                <img class="img-left" title="Question/Answer/Reply" alt="Question/Answer/Reply" src="/App_Themes/gray/Images/product-rating-icon.png" />
                                                <h2>Question/Answer/Reply</h2>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>

                                                             <tr>
                                                                <td align="right">Question/Answer/Reply Status :
                                                                </td>
                                                                <td>&nbsp;&nbsp;
                                                                    <asp:DropDownList ID="ddlQAstatus" runat="server" Width="125px" AutoPostBack="true"
                                                                        CssClass="order-list" OnSelectedIndexChanged="ddlQAstatus_SelectedIndexChanged">
                                                                        <asp:ListItem Value="0" Selected="true">Pending</asp:ListItem>
                                                                        <asp:ListItem Value="1">Approved</asp:ListItem>
                                                                        <asp:ListItem Value="-1">Disapproved</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:ObjectDataSource ID="_gridObjectDataSource" runat="server" EnablePaging="True"
                                                StartRowIndexParameterName="startIndex" MaximumRowsParameterName="pageSize" SortParameterName="sortBy"
                                                SelectCountMethod="GetCount">
                                                <%--<SelectParameters>
                                                    <asp:ControlParameter ControlID="hdnrating" DbType="String" DefaultValue="RatingID"
                                                        Name="CName" />
                                                    <asp:ControlParameter ControlID="drpstore" DbType="Int32" Name="pStoreId" DefaultValue="" />
                                                    <asp:ControlParameter ControlID="ddlQAStatus" DbType="String" Name="pSearchBy" />
                                                </SelectParameters>--%>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hdnrating" runat="server" />
                                        </td>

                                    </tr>
                                    <tr class="altrow">
                                        <td>
                                            <table width="100%">
                                                <tr id="trBottom" runat="server">
                                                    <td class="style1">
                                                        <span><a href="javascript:selectAll(true);">Check All</a> &nbsp;|&nbsp; <a href="javascript:selectAll(false);">Clear All</a></span>
                                                    </td>
                                                    <td align="right" style="width: 40%; float: right;">
                                                        <asp:ImageButton ID="btnapproverating" runat="server" ToolTip="Approve" OnClientClick='return checkCount();'
                                                            OnClick="btnapproverating_Click" />
                                                        <asp:ImageButton ID="btnDisapproverating" runat="server" ToolTip="DisApprove" OnClientClick='return checkCountfordis();'
                                                            OnClick="btnDisapproverating_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="even-row">
                                        <td>
                                            <asp:GridView ID="grdQA" runat="server" AutoGenerateColumns="False" EmptyDataText="No Record(s) Found."
                                                AllowSorting="True" EmptyDataRowStyle-ForeColor="Red" EmptyDataRowStyle-HorizontalAlign="Center"
                                                ViewStateMode="Enabled" Width="100%" GridLines="None" AllowPaging="True" PageSize="<%$ appSettings:GridPageSize %>"
                                                OnPageIndexChanging="OnPageIndexChanging"
                                                PagerSettings-Mode="NumericFirstLast" CellPadding="2"
                                                BorderStyle="Solid" BorderColor="#E7E7E7" BorderWidth="1" OnRowDataBound="grdQA_RowDataBound"
                                                CellSpacing="1" OnRowCommand="grdQA_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Select
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkselect" runat="server" />
                                                            <asp:HiddenField ID="hdnQARId" runat="server" Value='<%#Eval("QARId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="19%">
                                                        <HeaderTemplate>
                                                            Product Name
                                                            <%--<asp:ImageButton ID="lbName" runat="server" CommandArgument="DESC" CommandName="ProductName"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />--%>
                                                        </HeaderTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <a style="color: #212121; text-decoration: underline" href='<%# "AddQAR.aspx?Mode=edit&ID=" + DataBinder.Eval(Container.DataItem,"ProductID")+"&StoreID=" + DataBinder.Eval(Container.DataItem,"StoreID") %>'>
                                                                <asp:Label ID="lbproductname" runat="server" Text='<%# bind("ProductName") %>'></asp:Label></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Question" ItemStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbQuestion" runat="server" Text='<%# bind("Question") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Answer" ItemStyle-Width="20%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbAnswer" runat="server" Text='<%# bind("Answer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Reply" ItemStyle-Width="15%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbReply" runat="server" Text='<%# bind("Reply") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%-- <asp:TemplateField HeaderText="Comments" ItemStyle-Width="40%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbcomments" runat="server" Text='<%# bind("Comments") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <%-- <asp:TemplateField HeaderText="Q/A/R" ItemStyle-Width="40%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbqar" runat="server" Text='<%# bind("QAR") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdnQARvalue" runat="server" Value='<%#Eval("QAR") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>

                                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbcustomername" runat="server" Text='<%# bind("CreatedBy") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="10%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <HeaderTemplate>
                                                            Date
                                                           <%-- <asp:ImageButton ID="lbDate" runat="server" CommandArgument="DESC" CommandName="CreatedOn"
                                                                AlternateText="Ascending Order" OnClick="Sorting" />--%>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbcreatedon" runat="server" Text='<%# bind("CreatedOn") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approve" ItemStyle-Width="3%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="btnApprove" ImageUrl='<%# bind("ApproveImage") %>'
                                                               Visible='<%# Eval("ApproveImage") != ""?true:false %>' ToolTip="Approve" CommandName="Approve" CommandArgument='<%# Eval("QAR") %>'
                                                                message='<%# Eval("QARId") + "," + Eval("QAR") %>'
                                                                OnClientClick='return checkondelete(this.getAttribute("message"),"Approve");'></asp:ImageButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Disapprove" ItemStyle-Width="3%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" ID="btnUnApprove" ImageUrl='<%# bind("DisapproveImage") %>'
                                                               Visible='<%# Eval("DisapproveImage") != ""?true:false %>' ToolTip="Disapprove" CommandName="Disapprove" CommandArgument='<%# Eval("QAR") %>'
                                                                message='<%# Eval("QARId") + "," + Eval("QAR") %>' OnClientClick='return checkondelete(this.getAttribute("message"),"Disapprove");'></asp:ImageButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%-- <asp:TemplateField HeaderText="Image" ItemStyle-Width="30%">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkviewImage" runat="server" Visible="false" Text="View Image"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" Position="TopAndBottom"></PagerSettings>
                                                <PagerStyle HorizontalAlign="Right" CssClass="grid_paging" />
                                                <RowStyle HorizontalAlign="Center" CssClass="oddrow" />
                                                <AlternatingRowStyle CssClass="altrow" />
                                                <EmptyDataRowStyle HorizontalAlign="Center" ForeColor="Red"></EmptyDataRowStyle>
                                                <HeaderStyle ForeColor="White" Font-Bold="false" />
                                            </asp:GridView>
                                            <asp:Button ID="btnhdnDelete" runat="server" Text="Button" Style="display: none"
                                                OnClick="btnhdnDelete_Click" />
                                            <asp:HiddenField ID="hdnDelete" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnCommandName" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnCommandNameQAR" runat="server" Value="0" />
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
                    <img alt="" src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" width="1" height="10" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr>
        </table>
    </div>

    <div id="overlay" class="web_dialog_overlay"></div>

    <%--    <div id="dialog" class="web_dialog">
        <table style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
            <tr>
                <td class="web_dialog_title">Review Iamges</td>
                <td class="web_dialog_title align_right">
                    <a href="#" id="btnClose">Close</a>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="padding-left: 15px;">
                    <div id="popupContactInner"></div>
                </td>
            </tr>

            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>

        </table>
    </div>
    <div style="display: none;">
        <input type="hidden" id="RatingImageID" runat="server" />
        <input type="hidden" id="RatingImageName" runat="server" />
        <asp:Button ID="btndeletetempp" runat="server" />
    </div>--%>
    <style type="text/css">
        .innerpnl {
            border: 2px solid #cecece;
            float: left;
            margin: 2px;
            width: 22%;
        }

        .btndelete {
            float: left;
            clear: both;
        }

        .web_dialog_overlay {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
            background: #000000;
            opacity: .15;
            filter: alpha(opacity=15);
            -moz-opacity: .15;
            z-index: 101;
            display: none;
        }

        .web_dialog {
            display: none;
            position: fixed;
            height: 50%;
            width: 50%;
            top: 25%;
            left: 40%;
            margin-left: -190px;
            margin-top: -100px;
            background-color: #ffffff;
            border: 2px solid #cecece;
            padding: 0px;
            z-index: 102;
            font-family: Verdana;
            font-size: 10pt;
        }

        .web_dialog_title {
            border-bottom: solid 2px #cecece;
            background-color: #444;
            padding: 4px;
            color: White;
            font-weight: bold;
        }

            .web_dialog_title a {
                color: White;
                text-decoration: none;
            }

        .align_right {
            text-align: right;
        }
    </style>
    <script type="text/javascript">

        function ShowImageModel(ImageID) {
            document.getElementById(ImageID).height = '400px';
            document.getElementById(ImageID).width = '750px';
            var innerHtml = document.getElementById(ImageID).innerHTML;
            document.getElementById('popupContactInner').innerHTML = '';
            document.getElementById('popupContactInner').innerHTML = innerHtml;
            ShowDialog(true);
            return false;
        }

        $(document).ready(function () {

            $("#btnShowModal").click(function (e) {
                ShowDialog(true);
                e.preventDefault();
            });

            $("#btnClose").click(function (e) {
                HideDialog();
                e.preventDefault();
            });

        });

        function ShowDialog(modal) {
            $("#overlay").show();
            $("#dialog").fadeIn(300);

            if (modal) {
                $("#overlay").unbind("click");
            }
            else {
                $("#overlay").click(function (e) {
                    HideDialog();
                });
            }
        }

        function HideDialog() {
            $("#overlay").hide();
            $("#dialog").fadeOut(300);
        }

    </script>
</asp:Content>
