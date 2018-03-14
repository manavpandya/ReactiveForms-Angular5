<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="AddQAR.aspx.cs" Inherits="Solution.UI.Web.ADMIN.Products.AddQAR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts-main.js"></script>
    <script type="text/javascript" src="/App_Themes/<%=Page.Theme %>/js/jquery-alerts.js"></script>
    <script type="text/javascript">
        var Emailresults
        function checkemail1(str) {
            var filter = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{1,6}(?:\.[a-z]{1})?)$/i
            if (filter.test(str))
                Emailresults = true
            else {
                Emailresults = false
            }
            return (Emailresults)
        }

        var storedIdsQuestion = [];
        function addtext(id, value) {
            var strIds = id + " ";
            if (storedIdsQuestion.indexOf(strIds) <= -1) {
                storedIdsQuestion.push(strIds)
                $("#ContentPlaceHolder1_hdnAllQustionIds").val(storedIdsQuestion.join('~'));
            }
        }

        var storedIdsAnswer = [];
        function addtextAnswer(id, value) {
            var strIds = id + " ";
            if (storedIdsAnswer.indexOf(strIds) <= -1) {
                storedIdsAnswer.push(strIds)
                $("#ContentPlaceHolder1_hdnAllAnswerIds").val(storedIdsAnswer.join('~'));
            }
        }

        var storedIdsReply = [];
        function addtextReply(id, value) {
            var strIds = id + " ";
            if (storedIdsReply.indexOf(strIds) <= -1) {
                storedIdsReply.push(strIds)
                $("#ContentPlaceHolder1_hdnAllReplyIds").val(storedIdsReply.join('~'));
            }
        }

        var storedIdsInnerReply = [];
        function addtextInnerReply(id, value) {
            var strIds = id + " ";
            if (storedIdsInnerReply.indexOf(strIds) <= -1) {
                storedIdsInnerReply.push(strIds)
                $("#ContentPlaceHolder1_hdnAllInnerReplyIds").val(storedIdsInnerReply.join('~'));
            }
        }

        function AdditionBox(id) {
            $("." + id).css({ "display": "block" });
        }

        function CancelBox(id) {
            $("." + id).css({ "display": "none" });
        }

        function SaveBox(id, IdwithText, type) {
            var question = $("#" + IdwithText).val();
            $.ajax(
                   {
                       type: "POST",
                       url: "/TestMail.aspx/SaveData",
                       data: "{comment: '" + escape(question) + "', Type: '" + escape(type) + "', ProductId: '" + escape(0) + "', ID:'" + escape(id) + "'}",
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       async: "true",
                       cache: "false",
                       success: function (msg) {
                           window.location.href = '/Admin/Products/ManageQuestionAnswer.aspx';
                       }
                   });



            return false;
        }

        function AddQuestionBox() {
            $(".txtAreaAddQusetion").css({ "display": "block" });
        }

        function CancelAddQuestionBox() {
            $(".txtAreaAddQusetion").css({ "display": "none" });
        }

        function SaveAddQuestionBox(ProductId) {
            var question = $("#txtAreaAddQusetion").val();
            var Type = 'Question';
            $.ajax(
                       {
                           type: "POST",
                           url: "/TestMail.aspx/SaveData",
                           data: "{comment: '" + escape(question) + "', Type:'" + escape(Type) + "', ProductId: '" + escape(ProductId) + "', ID:'" + escape(0) + "'}",
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           async: "true",
                           cache: "false",
                           success: function (msg) {
                               window.location.href = '/Admin/Products/ManageQuestionAnswer.aspx';
                           }
                       });
            return false;
        }


        function ApproveDisapproveBox(Id, Type, Action) {
            $.ajax(
                       {
                           type: "POST",
                           url: "/TestMail.aspx/ApproveDisapprove",
                           data: "{Id: '" + escape(Id) + "', Type:'" + escape(Type) + "', Action: '" + escape(Action) + "'}",
                           contentType: "application/json; charset=utf-8",
                           dataType: "json",
                           async: "true",
                           cache: "false",
                           success: function (msg) {
                               window.location.href = '/Admin/Products/ManageQuestionAnswer.aspx';
                           }
                       });
            return false;
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="content-width">
        <div class="content-row1">
            <div class="create-new-order">
                &nbsp;
            </div>
        </div>
        <div class="content-row2">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
                    </td>
                </tr>
                <tr>
                    <td height="5" align="left" valign="top">
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="5" />
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
                                            <th>
                                                <div class="main-title-left">
                                                    <img class="img-left" title="Add Question/Answer/Reply" alt="Add Question/Answer/Reply" src="/App_Themes/<%=Page.Theme %>/Images/shipping-services-icon.png" />
                                                    <h2>
                                                        <asp:Label runat="server" Text="Add Question/Answer/Reply" ID="lblTitle"></asp:Label></h2>
                                                </div>
                                            </th>
                                        </tr>
                                        <tr class="even-row">
                                            <td>
                                                <asp:Literal ID="litTable" runat="server" />
                                                <%--<table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr class="oddrow">
                                                        <td>
                                                            <span class="star">*</span>Product Name:
                                                        </td>
                                                        <td style="width: 80%">
                                                            <asp:Label ID="lblProduct" runat="server" CssClass="order-label" Width="185px">
                                                            </asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="altrow">
                                                        <td>
                                                            <span style="vertical-align: middle; margin-right: 3px; float: right;"><a
                                                                href="#">
                                                                <img alt="Add Question" title="Add Question" src="/App_Themes/<%=Page.Theme %>/images/add-product-review.png" /></a></span>
                                                        </td>
                                                    </tr>
                                                    <tr class="oddrow">
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtcomment" CssClass="order-textfield" TextMode="MultiLine" Columns="5">QUESTION1?</asp:TextBox>
                                                        </td>
                                                        <td style="text-align:left">
                                                            <asp:Label ID="Label1" runat="server" CssClass="order-label"/>on 26-10-2016 By <asp:Label ID="Label4" runat="server" CssClass="order-label" />Prakash Chasiya                                                            
                                                        </td>
                                                    </tr>                                                   
                                                </table>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hdnAllQustionIds" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hdnAllAnswerIds" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hdnAllReplyIds" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hdnAllInnerReplyIds" runat="server" />
                                            </td>
                                        </tr>
                                        <tr class="altrow">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <td style="width: 80%">
                                                        <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ToolTip="Save"
                                                            OnClick="imgSave_Click" OnClientClick="return Checkfields();" />&nbsp;<asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel" ToolTip="Cancel"
                                                                OnClick="imgCancel_Click" />
                                                    </td>
                                                </table>
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
                        <img src="/App_Themes/<%=Page.Theme %>/images/spacer.gif" alt="" width="1" height="10" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
