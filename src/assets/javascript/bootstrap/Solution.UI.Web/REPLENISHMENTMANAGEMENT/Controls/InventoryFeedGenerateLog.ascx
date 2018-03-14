<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InventoryFeedGenerateLog.ascx.cs" Inherits="Solution.UI.Web.REPLENISHMENTMANAGEMENT.Controls.InventoryFeedGenerateLog" %>
<script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery-1.8.2.js"></script>
<script type="text/javascript">
    $.noConflict();
        jQuery(document).ready(function ($) {
                $('a.fancybox').fancybox();
    });
</script>
 
         <link type="text/css" href="/REPLENISHMENTMANAGEMENT/css/jQuery.fancybox.css?5767" rel="stylesheet" />   
 <script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery.elevatezoom.min.js"></script>
            <script type="text/javascript" src="/REPLENISHMENTMANAGEMENT/js/jquery.fancybox.pack.js"></script>        
    <div class="row">
        <div class="col-md-12">
          <div class="panel">
            <header class="panel-heading"> Inventory Feeds for Channel Partners <!--<span class="tools pull-right"> <a class="fa fa-chevron-down" href="javascript:;"></a> <a class="fa fa-cog" href="javascript:;"></a> <a class="fa fa-times" href="javascript:;"></a> </span>--> </header>
            <div class="panel-body">
              <div id="no-more-tables">

                  <asp:GridView ID="grdInventoryFeedLog" runat="server" CssClass="table table-bordered table-striped table-condensed cf" CellPadding="0"
                        Width="100%" AutoGenerateColumns="false" BorderWidth="1" CellSpacing="1" 
                         GridLines="None" EmptyDataText="No Item(s) Found." EditRowStyle-HorizontalAlign="Center"
                         EmptyDataRowStyle-ForeColor="Red" style="margin-bottom:0 !important;" OnRowDataBound="grdInventoryFeedLog_RowDataBound">
                     
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
                                <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle" />
                            </asp:TemplateField>
                           <asp:TemplateField HeaderStyle-CssClass="nt8">
                                <HeaderTemplate>
                                   Download
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnstoreid" runat="server" Value='<%#Eval("Storeid") %>' />
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
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle"  />
                            </asp:TemplateField>
                           <asp:TemplateField HeaderStyle-CssClass="nt7">
                                <HeaderTemplate>
                                 History
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <a runat="server"   class="fancybox fancybox.iframe"  id="aviewhistory" >
                                        <button class="btn btn-white btn-xs" data-toggle="button">
                                        <i class="fa fa-eye"></i> View History</button></a>
                                  
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                                <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle"  />
                            </asp:TemplateField>
                      </Columns>


                  </asp:GridView>





               <%-- <table class="table table-bordered table-striped table-condensed cf">
                  <thead class="cf">
                    <tr>
                      <th class="nt1">Channel Partners</th>
                      <th class="nt3">Date & Time Generated</th>
                      <th class="nt5">Status</th>
                      <th class="nt8">Download</th>
                      <th class="nt2">Feed ID</th>
                      <th class="nt6">Requested By</th>
                      <th class="nt7">History</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td data-title="Channel Partners">Amazon</td>
                      <td data-title="Date & Time Generated">12/13/14 11:14:28 PM PST</td>
                      <td data-title="Status">Ready</td>
                      <td data-title="Download"><button type="button" class="btn btn-orang"><i class="fa fa-download"></i> Download</button></td>
                      <td data-title="Feed ID">54768016428</td>
                      <td data-title="Requested By">Asutosh Vyas</td>
                      <td data-title="History"><button data-toggle="button" class="btn btn-white btn-xs"><i class="fa fa-eye"></i> View History</button></td>
                    </tr>
                    <tr>
                      <td data-title="Channel Partners">ATG</td>
                      <td data-title="Date & Time Generated">12/13/14 11:14:28 PM PST</td>
                      <td data-title="Status">Ready</td>
                      <td data-title="Download"><button type="button" class="btn btn-orang "><i class="fa fa-download"></i> Download</button></td>
                      <td data-title="Feed ID">54768016428</td>
                      <td data-title="Requested By">Asutosh Vyas</td>
                      <td data-title="History"><button type="button" class="btn btn-white  btn-xs"><i class="fa fa-eye"></i> View History </button></td>
                    </tr>
                    <tr>
                      <td data-title="Channel Partners">Bellacor</td>
                      <td data-title="Date & Time Generated">12/13/14 11:14:28 PM PST</td>
                      <td data-title="Status">Ready</td>
                      <td data-title="Download"><button type="button" class="btn btn-orang "><i class="fa fa-download"></i> Download</button></td>
                      <td data-title="Feed ID">54768016428</td>
                      <td data-title="Requested By">Asutosh Vyas</td>
                      <td data-title="History"><button type="button" class="btn btn-white  btn-xs"><i class="fa fa-eye"></i> View History </button></td>
                    </tr>
                    <tr>
                      <td data-title="Channel Partners">Houzz</td>
                      <td data-title="Date & Time Generated">12/13/14 11:14:28 PM PST</td>
                      <td data-title="Status">Ready</td>
                      <td data-title="Download"><button type="button" class="btn btn-orang "><i class="fa fa-download"></i> Download</button></td>
                      <td data-title="Feed ID">54768016428</td>
                      <td data-title="Requested By">Asutosh Vyas</td>
                      <td data-title="History"><button type="button" class="btn btn-white  btn-xs"><i class="fa fa-eye"></i> View History </button></td>
                    </tr>
                    <tr>
                      <td data-title="Channel Partners">Kohls</td>
                      <td data-title="Date & Time Generated">12/13/14 11:14:28 PM PST</td>
                      <td data-title="Status">Ready</td>
                      <td data-title="Download"><button type="button" class="btn btn-orang "><i class="fa fa-download"></i> Download</button></td>
                      <td data-title="Feed ID">54768016428</td>
                      <td data-title="Requested By">Asutosh Vyas</td>
                      <td data-title="History"><button type="button" class="btn btn-white  btn-xs"><i class="fa fa-eye"></i> View History </button></td>
                    </tr>
                    <tr>
                      <td data-title="Channel Partners">LNT</td>
                      <td data-title="Date & Time Generated">12/13/14 11:14:28 PM PST</td>
                      <td data-title="Status">Ready</td>
                      <td data-title="Download"><button type="button" class="btn btn-orang "><i class="fa fa-download"></i> Download</button></td>
                      <td data-title="Feed ID">54768016428</td>
                      <td data-title="Requested By">Asutosh Vyas</td>
                      <td data-title="History"><button type="button" class="btn btn-white  btn-xs"><i class="fa fa-eye"></i> View History </button></td>
                    </tr>
                    <tr>
                      <td data-title="Channel Partners">Overstock</td>
                      <td data-title="Date & Time Generated">12/13/14 11:14:28 PM PST</td>
                      <td data-title="Status">Ready</td>
                      <td data-title="Download"><button type="button" class="btn btn-orang "><i class="fa fa-download"></i> Download</button></td>
                      <td data-title="Feed ID">54768016428</td>
                      <td data-title="Requested By">Asutosh Vyas</td>
                      <td data-title="History"><button type="button" class="btn btn-white  btn-xs"><i class="fa fa-eye"></i> View History </button></td>
                    </tr>
                    <tr>
                      <td data-title="Channel Partners">Wayfair</td>
                      <td data-title="Date & Time Generated">12/13/14 11:14:28 PM PST</td>
                      <td data-title="Status">Ready</td>
                      <td data-title="Download"><button type="button" class="btn btn-orang "><i class="fa fa-download"></i> Download</button></td>
                      <td data-title="Feed ID">54768016428</td>
                      <td data-title="Requested By">Asutosh Vyas</td>
                      <td data-title="History"><button type="button" class="btn btn-white  btn-xs"><i class="fa fa-eye"></i> View History </button></td>
                    </tr>
                  </tbody>
                </table>--%>
              </div>
            </div>
          </div>
        </div>
      </div>