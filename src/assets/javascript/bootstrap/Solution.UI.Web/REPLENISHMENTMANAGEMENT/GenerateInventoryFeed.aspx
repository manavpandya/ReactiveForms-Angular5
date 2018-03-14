<%@ Page Title="" Language="C#" MasterPageFile="~/REPLENISHMENTMANAGEMENT/AdminReplnishment.Master" AutoEventWireup="true" CodeBehind="GenerateInventoryFeed.aspx.cs" Inherits="Solution.UI.Web.REPLENISHMENTMANAGEMENT.GenerateInventoryFeed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/minimal.css" rel="stylesheet"/>
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/red.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/green.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/blue.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/yellow.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/minimal/purple.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/square.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/red.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/green.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/blue.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/yellow.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/square/purple.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/grey.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/red.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/green.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/blue.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/yellow.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/iCheck/skins/flat/purple.css" rel="stylesheet" />
    <script src="/REPLENISHMENTMANAGEMENT/js/iCheck/jquery.icheck.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/icheck-init.js"></script>
    <script type="text/javascript">
        function Validation() {
            if (document.getElementById('ContentPlaceHolder1_ddlstore') != null && document.getElementById('ContentPlaceHolder1_ddlstore').selectedIndex == 0) {
                alert("Please Select Channel Partner.");
                document.getElementById('ContentPlaceHolder1_ddlstore').focus();
                return false;
            }
            if (document.getElementById('ContentPlaceHolder1_radiocsv') != null && document.getElementById('ContentPlaceHolder1_radioExcel') != null) {
                if (document.getElementById('ContentPlaceHolder1_radiocsv').checked == false && document.getElementById('ContentPlaceHolder1_radioExcel').checked == false) {
                    alert("Please Select Inventory Feed File Format");
                    return false;
                }
            }
            document.getElementById('divloader').style.display = '';
            if (document.getElementById('ContentPlaceHolder1_btndownloadnow') != null)
            {
                document.getElementById('ContentPlaceHolder1_btndownloadnow').style.display = 'none';
            }
            return true;
        }

        function HideDownloadNow()
        {
           
            if (document.getElementById('ContentPlaceHolder1_btndownloadnow') != null) {
               
                document.getElementById('ContentPlaceHolder1_btndownloadnow').style.display = 'none';
            }
           
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <!--breadcrumbs start -->
                <ul class="breadcrumb">
                    <li>
                        <p class="hd-title">Generate Inventory Feed for Channel Partner (On Demand)</p>
                    </li>
                </ul>
                <!--breadcrumbs end -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel">
                    <div class="panel-heading">Generate Feed</div>
                    <div class="panel-body" id="selectfile-pro">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-md-3 control-label">Channel Partner</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ddlstore" runat="server" CssClass="form-control m-bot15" AutoPostBack="true" OnSelectedIndexChanged="ddlstore_SelectedIndexChanged"></asp:DropDownList>

                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3 control-label">File Name</label>
                                 <div class="col-md-4">
                                            <div class="form-control-static">
                                                <asp:TextBox ID="txtstorefile" runat="server" CssClass="form-control" ></asp:TextBox>
                                            </div>
                                        </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3 control-label">Inventory Feed File Format</label>
                                <div class="col-md-6 icheck minimal">
                                    <div class="row">
                                        <label class="checkbox-inline">
                                            <div class="row">
                                                <div class="radio single-row">
                                                    <asp:RadioButton ID="radiocsv" runat="server" TabIndex="3" Text="CSV"  Checked="true" AutoPostBack="false" GroupName="radiovalidation"  onchange="javascript:HideDownloadNow();" />
                                                    
                                                </div>
                                            </div>
                                        </label>
                                        <label class="checkbox-inline">
                                            <div class="row">
                                                <div class="radio single-row">
                                                    <asp:RadioButton ID="radioExcel" runat="server" TabIndex="3" Text="Excel" GroupName="radiovalidation" AutoPostBack="false"  onchange="javascript:HideDownloadNow();" />

                                                </div>
                                            </div>
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3 control-label">&nbsp;</label>
                                <div class="col-md-6">
                                    <asp:Button runat="server" CssClass="btn btn-orang" ID="btngeneratefeed" Text="Generate Feed" OnClientClick="return Validation();" OnClick="btngeneratefeed_Click" />
                                      <asp:Button runat="server" CssClass="btn btn-orang" ID="btndownloadnow" Visible="false" Text="Download Now" OnClick="btndownloadnow_Click" />

                                </div>
                                
                                </div>
                            <div class="form-group">
                                
                                <div class="col-md-6" style="text-align:center;display:none;" id="divloader">
                                     <label class="col-md-3 control-label">&nbsp;</label>
                                    <img src="images/downloadloader.png" border="0" />
                                </div>
                                </div>
                          
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
