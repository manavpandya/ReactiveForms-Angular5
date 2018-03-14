<%@ Page Title="" Language="C#" MasterPageFile="~/REPLENISHMENTMANAGEMENT/AdminReplnishment.Master" AutoEventWireup="true" CodeBehind="ImportReplenishmentData.aspx.cs" Inherits="Solution.UI.Web.REPLENISHMENTMANAGEMENT.ImportReplenishmentData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function chkHeight() {
            var windowHeight = 0;
            windowHeight = $(document).height(); //window.innerHeight;

            document.getElementById('ContentPlaceHolder1_prepage').style.height = windowHeight + 'px';
            document.getElementById('ContentPlaceHolder1_prepage').style.display = '';
        }
        function VerifyInfo() {

            $('#selectfile-pro').hide();
            $('#upload-pro').hide();
            $('#verify-pro').fadeIn('slow');


            $('#selectfile-wizard').removeClass();
            $('#selectfile-wizard').addClass('currentpro active-trail');
            $('#verify-wizard').removeClass();
            $('#verify-wizard').addClass('current');
            $('#upload-wizard').removeClass();
            $('#upload-wizard').addClass('active-trail');


            $('#verify-wizard').parent().addClass('active');
            $('#selectfile-wizard').parent().removeClass('active');
            return false;

        }

        function verifyback() {

            $('#selectfile-pro').fadeIn('slow');
            $('#upload-pro').hide();
            $('#verify-pro').hide();

            $('#selectfile-wizard').removeClass();
            $('#selectfile-wizard').addClass('current');
            $('#verify-wizard').removeClass();
            $('#verify-wizard').addClass('active-trail');
            $('#upload-wizard').removeClass();
            $('#upload-wizard').addClass('active-trail');

            $('#selectfile-wizard').parent().addClass('active');
            $('#verify-wizard').parent().removeClass('active');
            return false;
        }

        function verifyuploadnext() {

            $('#selectfile-pro').hide();
            $('#verify-pro').hide();
            $('#upload-pro').fadeIn('slow');

            $('#selectfile-wizard').removeClass();
            $('#selectfile-wizard').addClass('currentpro active-trail');
            $('#verify-wizard').removeClass();
            $('#verify-wizard').addClass('currentpro active-trail');
            $('#upload-wizard').removeClass();
            $('#upload-wizard').addClass('current active-trail');

            $('#upload-wizard').parent().addClass('active');
            $('#verify-wizard').parent().removeClass('active');
            return false;
        }

        function verifylastback() {
            $('#selectfile-pro').hide();
            $('#upload-pro').hide();
            $('#verify-pro').fadeIn('slow');


            $('#selectfile-wizard').removeClass();
            $('#selectfile-wizard').addClass('currentpro active-trail');
            $('#verify-wizard').removeClass();
            $('#verify-wizard').addClass('current');
            $('#upload-wizard').removeClass();
            $('#upload-wizard').addClass('active-trail');

            $('#verify-wizard').parent().addClass('active');
            $('#upload-wizard').parent().removeClass('active');
            return false;
        }

    </script>
    

     <script type="text/javascript">
         function validateFilevalidation() {
             var uploadcontrol = document.getElementById('<%=Uploadcsv.ClientID%>').value;
            
    //Regular Expression for fileupload control.
             var reg = /(.*?)\.(csv)$/;
             
    if (uploadcontrol.length > 0) {
        //Checks with the control value.
        if (reg.test(uploadcontrol)) {
            chkHeight();
            return true;
        }
        else {
            //If the condition not satisfied shows error message.
            alert("Select csv File Only (Ex.: .csv)");
            return false;
        }
    }
    else
    {
        alert("Please Choose File!");
        return false;
    }
   // alert("Your File has been uploaded successfully.");
} //End of function validate.
</script>
    <%--  <script type="text/javascript" >
        $(document).ready(function () {
            
            $('#verify-pro-btn').click(function () {
                $('#selectfile-pro').hide();
                $('#upload-pro').hide();
                $('#verify-pro').fadeIn('slow');


                $('#selectfile-wizard').removeClass();
                $('#selectfile-wizard').addClass('currentpro active-trail');
                $('#verify-wizard').removeClass();
                $('#verify-wizard').addClass('current');
                $('#upload-wizard').removeClass();
                $('#upload-wizard').addClass('active-trail');


                $('#verify-wizard').parent().addClass('active');
                $('#selectfile-wizard').parent().removeClass('active');


            });

            $('#selectfile-back').click(function () {
                $('#selectfile-pro').fadeIn('slow');
                $('#upload-pro').hide();
                $('#verify-pro').hide();

                $('#selectfile-wizard').removeClass();
                $('#selectfile-wizard').addClass('current');
                $('#verify-wizard').removeClass();
                $('#verify-wizard').addClass('active-trail');
                $('#upload-wizard').removeClass();
                $('#upload-wizard').addClass('active-trail');

                $('#selectfile-wizard').parent().addClass('active');
                $('#verify-wizard').parent().removeClass('active');
            });

            $('#upload-btn').click(function () {
                $('#selectfile-pro').hide();
                $('#verify-pro').hide();
                $('#upload-pro').fadeIn('slow');

                $('#selectfile-wizard').removeClass();
                $('#selectfile-wizard').addClass('currentpro active-trail');
                $('#verify-wizard').removeClass();
                $('#verify-wizard').addClass('currentpro active-trail');
                $('#upload-wizard').removeClass();
                $('#upload-wizard').addClass('current active-trail');

                $('#upload-wizard').parent().addClass('active');
                $('#verify-wizard').parent().removeClass('active');

            });
            $('#verify-back').click(function () {
                $('#selectfile-pro').hide();
                $('#upload-pro').hide();
                $('#verify-pro').fadeIn('slow');


                $('#selectfile-wizard').removeClass();
                $('#selectfile-wizard').addClass('currentpro active-trail');
                $('#verify-wizard').removeClass();
                $('#verify-wizard').addClass('current');
                $('#upload-wizard').removeClass();
                $('#upload-wizard').addClass('active-trail');

                $('#verify-wizard').parent().addClass('active');
                $('#upload-wizard').parent().removeClass('active');

            });



        });

</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div class="wrapper">
        <div class="row">
            <div class="col-md-12">
                <!--breadcrumbs start -->
                <ul class="breadcrumb">
                    <li>
                        <p class="hd-title">Import Replenishment Data</p>
                    </li>
                </ul>
                <!--breadcrumbs end -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel">
                    <div class="panel-body">
                        <div class=" full-width">
                            <ul class="wizardpro-alt">
                                <li class="active"><span class="current" id="selectfile-wizard">Select File</span> </li>
                                <li><span class="active-trail" id="verify-wizard">Verify</span> </li>
                                <li><span class="active-trail" id="upload-wizard">Upload</span> </li>
                            </ul>
                        </div>
                        <div class="panel fa-border" id="selectfile-pro">
                            <div class="panel-heading">Select File </div>
                            <div class="panel-body">
                                <div class="pull-right"></div>
                                <div class="form-horizontal">
                                    
<p class="help-block m-bot20">Please upload Replenishment file in CSV format. Please ensure it follows the sample file format.</p> 
                                       
                                  
                                    <div class="form-group">

                                        <label class="col-md-3 control-label col-lg-3" for="inputSuccess">View Replenishment File Format</label>
                                        <div class="col-lg-6">
                                            <a href="/resources/halfpricedraps/product/ProductCSV/ImportReplenishemntCSV/SampleReplenishmentdata.csv" class="btn btn-orang" onclick="return true;"><i class="fa fa-download"></i>
                                                Sample File Format</a>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lbllastupdate" class="col-md-3 control-label col-lg-3" for="inputSuccess" Text="Last Updated On"></asp:Label>

                                        <div class="col-lg-6">
                                            <p class="form-control-static">
                                                <strong>
                                                    <asp:Label ID="lbllastupdatevalue" runat="server" Text=""></asp:Label></strong>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label class="col-md-3 control-label col-lg-3" ID="lbllastupdateby" for="inputSuccess" Text="Last Updated By" runat="server"></asp:Label>
                                        <div class="col-lg-6">
                                            <p class="form-control-static">
                                                <strong>
                                                    <asp:Label runat="server" ID="lbllastupdatebyvalue" Text=""></asp:Label></strong>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="control-label col-md-3">Select Replenishment File</label>
                                        <div class="col-md-6">
                                            <div class="form-control-static">

                                                <asp:UpdatePanel ID="UpdatePanelfileload" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <div class="col-md-6" id="divFileName" runat="server" visible="false" style="padding-left: 0px;">
                                                            <div class="form-control-static" style="padding-top: 0px;">
                                                                <asp:Label runat="server" ID="lblFileName" Text=""></asp:Label>

                                                            </div>
                                                        </div>
                                                      
                                                        <div class="col-md-6" id="divUpload" runat="server" style="padding-left: 0px;">
                                                            <div class="form-control-static" style="padding-top: 0px;">
                                                                <asp:FileUpload runat="server" ID="Uploadcsv" />
                                                                <asp:RegularExpressionValidator ID="revImage" ControlToValidate="Uploadcsv" ValidationExpression="(.*?)\.(csv)$"
                                                                    Text="Select csv File Only (Ex.: .csv)" runat="server"
                                                                    ValidationGroup="importfile" Display="Dynamic" ForeColor="Red" />
                                                                <asp:RequiredFieldValidator ID="rfvFileUpload" ControlToValidate="Uploadcsv" Text="Please Choose File" runat="server" ValidationGroup="importfile" Display="Dynamic"
                                                                    ForeColor="Red"></asp:RequiredFieldValidator>

                                                            </div>
                                                        </div>

                                                       
                                                    </ContentTemplate>

                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnImport" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <%-- <asp:UpdateProgress ID="UpdateProgressFabric" runat="server" AssociatedUpdatePanelID="uplfileupload">
                                                    <ProgressTemplate>
                                                        <div style="position: relative;">
                                                            <div style="position: absolute; bottom: -27px; left: 40%;">
                                                                <img alt="" src="../images/ProductLoader.gif" />
                                                                <b>Loading ... ... Please wait!</b>
                                                            </div>
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <asp:UpdatePanel ID="uplfileupload" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>--%>



                                                <%--</ContentTemplate>
                                                </asp:UpdatePanel>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-md-3">&nbsp;</label>
                                        <div class="col-md-6">
                                            <div class="form-control-static">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                         <div class="col-md-2" id="divImport" runat="server" style="padding-left: 0px;padding-right:14%;width:12% !important;">
                                                            <div class="form-control-static" style="padding-top: 0px;">
                                                                <asp:Button ID="btnImport" CssClass="btn btn-orang" Text="Upload" runat="server"  OnClick="btnimportfile_Click" ValidationGroup="importfile" CausesValidation="true" OnClientClick="validateFilevalidation();" />

                                                            </div>
                                                        </div>
                                                          <div class="col-md-2" id="divReset" runat="server" visible="false" style="padding-left: 0px;padding-right:14%;width:20% !important;">
                                                            <div class="form-control-static" style="padding-top: 0px;">
                                                                <asp:LinkButton ID="lnkbtnReset" runat="server" CssClass="btn btn-orang" Text="Re-Upload File" OnClick="lnkbtnReset_Click"></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                        <asp:Button ID="btnimportfile" runat="server" CssClass="btn btn-orang" Text="Next" OnClientClick="return VerifyInfo();" ValidationGroup="importfile" CausesValidation="true" Enabled="false" />
                                                        <asp:HiddenField ID="hdnFileName" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel fa-border" style="display: none" id="verify-pro">
                            <div class="panel-heading">Verify </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <%--<label class="control-label col-md-3"><strong>Replenishment File Details</strong></label>--%>
                                        <div class="col-md-6">
                                            <div class="form-control-static">&nbsp;</div>
                                        </div>
                                    </div>
                                
                      <p class="help-block m-bot20"> Review Replenishment file data verification results below.  Next button will be enabled once all the validations are successful. Click on the Back button to resolve any errors reported below.</p>
                                      
                                    <div class="form-group">
                                      
                                        <div class="col-md-6">
                                          
                                          <%--  <div class="form-control-static"><strong class="green-success">successful!</strong></div>--%>
                                            <div class="form-control-static"><strong class="red-error">
                                                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label></strong></div>
                                            <%--   <div class="form-control-static"><strong class="red-error">Data does not match field data format</strong></div>--%>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label ID="lblerrorskumsg" class="control-label col-md-3" runat="server" Text="Error SKU :" Visible="false"></asp:Label>

                                        <div class="col-md-6">
                                            <div class="form-control-static">
                                                <asp:Label ID="lblerrorsku" runat="server" Text="0" Visible="false"></asp:Label>

                                            </div>
                                        </div>
                                    </div>
                                   
                                    <div class="form-group">
                                        <asp:Label ID="lbltotsku" class="control-label col-md-3" runat="server" Text="Total # of SKUs:"></asp:Label>

                                        <div class="col-md-6">
                                            <div class="form-control-static">
                                                <asp:Label ID="lblskucount" runat="server" Text="0"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblskureplenished" class="control-label col-md-3" runat="server" Text="Total # of Valid SKUs:"></asp:Label>

                                        <div class="col-md-6">
                                            <div class="form-control-static">
                                                <asp:Label ID="lblreplenishedskucount" runat="server" Text="0"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                     <div class="form-group" >
                                        
                                        <label class="control-label col-md-3">SKUs with Errors</label>
                                        <div class="col-md-6">

                                            <div class="form-control-static" id="diverror" runat="server">
                                                <asp:Literal ID="ltrErrors" runat="server"></asp:Literal>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-md-3">
                                            <asp:Button ID="btnselectfileback" runat="server" CssClass="btn btn-orang" Text="Back" OnClientClick="return verifyback();" />

                                        </label>
                                        <div class="col-md-6">
                                            <div class="form-control-static">
                                                <asp:Button runat="server" Text="Next" CssClass="btn btn-orang" ID="btnuploadnext" OnClientClick="return verifyuploadnext();" />
                                                <%--<button type="button" class="btn btn-orang " id="upload-btn">Next</button>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="panel fa-border" style="display: none;" id="upload-pro">
                            <div class="panel-heading">Upload </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                   
                                          
                                         
                            <p class="help-block m-bot20" id="uploadtext" runat="server">  Click on Upload Replenishment File button to upload the replenishment data into the system. </p>
                                               
                                       
                                    <div class="form-group">
                                       <label class="control-label col-md-3">&nbsp;</label>
                                        <div class="col-md-6">
                                            <div class="form-control-static">

                                                <%--  <asp:UpdateProgress ID="UpdateProgressImport" runat="server" AssociatedUpdatePanelID="uplimportdata">
                                                    <ProgressTemplate>
                                                        <div style="position:absolute;">
                                                            <div style="position: absolute; bottom: -27px; left: 40%;">
                                                                <img alt="" src="images/loading.gif" />
                                                                <b>Loading ... ... Please wait!</b>
                                                            </div>
                                                        </div>
                                                      
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>--%>
                                                <%--<asp:UpdatePanel ID="uplimportdata" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>--%>
                                                        <asp:Button runat="server" class="btn btn-orang " Text="Upload Replenishment File" ID="btnUploadReplenishmentFile" OnClientClick="chkHeight();" OnClick="btnUploadReplenishmentFile_Click" />
                                                   <%-- </ContentTemplate>
                                                </asp:UpdatePanel>--%>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-md-3">&nbsp;</label>
                                        <div class="col-md-6">

                                            <div class="form-control-static">
                                                <strong class="green-success">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblsuccessmsg" runat="server" Text=""></asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </strong>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-md-3">&nbsp; </label>
                                        <div class="col-md-6">
                                            <div class="form-control-static">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <a id="btnverifyback" class="btn btn-orang" runat="server" onclick="return verifylastback();">Back</a>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <%--  <button type="button" class="btn btn-orang " id="verify-back">Back</button>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div id="prepage" runat="server" style="position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px; background-color: #000; opacity: 0.7; filter: alpha(opacity=70); layer-background-color: white; height: 100%; width: 100%; z-index: 1000; display: none;">
                    <table width="100%" style="padding-top: 400px;" align="center">
                        <tr>
                            <td align="center" style="color: #fff; padding-top: 500px;" valign="middle">
                                <img alt="" src="images/loading.gif" /><br />
                                <b>Loading ... ... Please wait!</b>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
