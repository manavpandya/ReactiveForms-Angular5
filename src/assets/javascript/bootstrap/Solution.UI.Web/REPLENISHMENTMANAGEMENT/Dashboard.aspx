<%@ Page Title="" Language="C#" MasterPageFile="~/REPLENISHMENTMANAGEMENT/AdminReplnishment.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Solution.UI.Web.REPLENISHMENTMANAGEMENT.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapper">
      <div class="row">
        <div class="col-md-12"> 
          <!--breadcrumbs start -->
          <ul class="breadcrumb">
            <li>
              <p class="hd-title">Dashboard</p>
            </li>
          </ul>
          <!--breadcrumbs end --> 
        </div>
      </div>
      <div class="row">
        <div class="col-md-12">
             <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div id="divleftControls" runat="server">
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
        </div>
      </div>
    </div>
</asp:Content>
