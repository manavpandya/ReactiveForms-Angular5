<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateInventoryFeedHistoryLog.aspx.cs" Inherits="Solution.UI.Web.REPLENISHMENTMANAGEMENT.GenerateInventoryFeedHistoryLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <link href="/REPLENISHMENTMANAGEMENT/css/bootstrap.min.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/js/jquery-ui/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/css/bootstrap-reset.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/font-awesome/css/font-awesome.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/css/table-responsive.css" rel="stylesheet" />
<!-- Custom styles for this template -->
<link href="/REPLENISHMENTMANAGEMENT/css/style.css" rel="stylesheet" />
<link href="/REPLENISHMENTMANAGEMENT/css/style-responsive.css" rel="stylesheet"/>
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

<script src="/REPLENISHMENTMANAGEMENT/js/dashboard.js"></script> 
<script src="/REPLENISHMENTMANAGEMENT/js/jquery.customSelect.min.js" ></script> 
<!--common script init for all pages--> 
<script src="/REPLENISHMENTMANAGEMENT/js/scripts.js"></script> 
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="row">
        <div class="col-md-12">
          <div class="panel">
            <header class="panel-heading"> History of Inventory Feeds for <%=Storename%>  <!--<span class="tools pull-right"> <a class="fa fa-chevron-down" href="javascript:;"></a> <a class="fa fa-cog" href="javascript:;"></a> <a class="fa fa-times" href="javascript:;"></a> </span>--> </header>
            <div class="panel-body">
              <div id="no-more-tables" >
                   
                  <asp:GridView ID="grdInventoryFeedLogHistory" runat="server" CssClass="table table-bordered table-striped table-condensed cf" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" HeaderStyle-CssClass="cf" 
                         GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                         EmptyDataRowStyle-ForeColor="Red" style="margin-bottom:0 !important;" OnRowDataBound="grdInventoryFeedLogHistory_RowDataBound">
                    
                      <Columns>
                          <asp:TemplateField HeaderStyle-CssClass="nt1">
                                <HeaderTemplate>
                                   Channel Partners
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("StoreName")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"  />
                            </asp:TemplateField>
                          <asp:TemplateField HeaderStyle-CssClass="nt3">
                                <HeaderTemplate>
                                   Date & Time Generated
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblgeneratedon" runat="server" Text='<%# Eval("GeneratedOn") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" />
                                <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"  />
                            </asp:TemplateField>
                          <asp:TemplateField HeaderStyle-CssClass="nt5">
                                <HeaderTemplate>
                                   Status
                                </HeaderTemplate>
                                <ItemTemplate>
                                   Ready
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle"  />
                            </asp:TemplateField>
                           <asp:TemplateField HeaderStyle-CssClass="nt8">
                                <HeaderTemplate>
                                   Download
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <a id="adownloadfile" class="btn btn-orang" runat="server"><i class="fa fa-download"></i>Download</a>
                                     <asp:HiddenField ID="hdnfilename" runat="server" Value='<%#Eval("[FileName]") %>' />
                                  
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                           <asp:TemplateField HeaderStyle-CssClass="nt2">
                                <HeaderTemplate>
                                   Feed ID
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("FeedID")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle"  />
                            </asp:TemplateField>
                          <asp:TemplateField HeaderStyle-CssClass="nt6">
                                <HeaderTemplate>
                                  Requested By
                                </HeaderTemplate>
                                <ItemTemplate>
                                  <%# Eval("Name")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle"  />
                            </asp:TemplateField>
                           
                      </Columns>
                      
                  </asp:GridView>
                      

              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    </form>
</body>
</html>
