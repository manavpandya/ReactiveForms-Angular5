<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Country.aspx.cs" Inherits="Solution.UI.Web.Country" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="breadcrumbs">
        <a href="/Index.aspx" title="Home">Home </a>> <span>Country</span></div>
    <div class="content-main">
        <div class="static-title">
            <span>Country</span>
        </div>
        <div class="static-main">
            <div class="static-main-box">
                <div class="country">
                    <p>
                        You are currently shopping on our International website, which shows product costings
                        in USD.</p>
                    <p>
                        To change currency, please select your country from one of the dropdown lists below.
                        This will give you the correct pricing, delivery times and shipping costs for your
                        destination. Please be aware that the price of an item may vary based on your shipping
                        destination to reflect local market pricing.
                    </p>
                    <p style="width: 100%; text-align: center;">
                        <strong>Please note : Prices shown are for reference only. All Transactions will be
                            processed in USD.</strong>
                    </p>
                    <p>
                        <span><strong>United States Site / International Site</strong> </span><span>• Serving
                            : US, Canada and Latin America</span> <span>• Billed in US Dollars</span> <span><strong>
                                Please select your country</strong></span> <span>
                                    <asp:DropDownList ID="ddlS_Country" CssClass="select-box" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                </span>
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
