<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configurepopup.aspx.cs" Inherits="Solution.UI.Web.REPLENISHMENTMANAGEMENT.Configurepopup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/REPLENISHMENTMANAGEMENT/css/bootstrap.min.css?343" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/js/jquery-ui/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/css/bootstrap-reset.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/css/table-responsive.css" rel="stylesheet" />
    <!-- Custom styles for this template -->
    <link href="/REPLENISHMENTMANAGEMENT/css/style.css?343" rel="stylesheet" />
    <link href="/REPLENISHMENTMANAGEMENT/css/style-responsive.css" rel="stylesheet" />
    <link rel="shortcut icon" href="images/favicon.ico">
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery-ui/jquery-ui-1.10.1.custom.min.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/bootstrap.min.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.dcjqaccordion.2.7.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.scrollTo.min.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jQuery-slimScroll-1.3.0/jquery.slimscroll.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.nicescroll.js"></script>
    <!--[if lte IE 8]><script language="javascript" type="text/javascript" src="js/flot-chart/excanvas.min.js"></script><![endif]-->
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.scrollTo/jquery.scrollTo.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.easing.min.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/dashboard.js"></script>
    <script src="/REPLENISHMENTMANAGEMENT/js/jquery.customSelect.min.js"></script>
    <!--common script init for all pages-->
    <script src="/REPLENISHMENTMANAGEMENT/js/scripts.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 300px;">
            <div id="allhtmlfile" class="form-horizontal">
                <div id="div_0" class="panel fa-border p-top-20" style="padding-top: 0px !important; margin-top: 0px !important; width: 25%; float: left;">
                    <div class="form-group" style="width: 100% !important;">
                        <label class="control-label col-md-3">Database value</label><div class="col-md-3" style="padding-top: 6px;">True</div>
                    </div>
                    <div class="form-group" style="width: 100% !important;">
                        <label class="control-label col-md-3">&nbsp;</label><div class="col-md-3">False</div>
                    </div>

                    <div class="form-group" style="width: 100% !important;"></div>
                </div>
                <div id="div_1" class="panel fa-border p-top-20" style="padding-top: 0px !important; margin-top: 0px !important; width: 49%; float: left;">
                    <div class="form-group" style="width: 100% !important;">
                        <label class="control-label col-md-3">Assigned Value</label><div class="col-md-3" style="padding-top: 6px;">
                            <asp:TextBox ID="txtyes" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" style="width: 100% !important;">
                        <label class="control-label col-md-3">&nbsp;</label><div class="col-md-3">
                            <asp:TextBox ID="txtno" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group" style="width: 50% !important;"></div>
                </div>
                <div id="div_2" class="panel fa-border p-top-20" style="padding-top: 0px !important; margin-top: 0px !important; width: 49%; float: left;">
                    <div class="form-group">
                        <label class="control-label col-md-3">&nbsp;</label>
                        <div class="col-md-9">
                            <div class="form-control-static" style="text-align:center"><asp:Button ID="mapping_btn_next" runat="server" CssClass="btn btn-orang" Text="Save" OnClick="mapping_btn_next_Click" />
                            </div>
                        </div>
                    </div>
                </div>
        </div>
        </div>
    </form>
</body>
</html>
