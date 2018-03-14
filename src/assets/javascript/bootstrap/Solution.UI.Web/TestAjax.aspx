<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestAjax.aspx.cs" Inherits="Solution.UI.Web.TestAjax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   	<script type="text/javascript" src="/js/jquery-1.3.1.min.js"></script>

    <script type="text/javascript">

        $(function () {
			    // Get a reference to the content div (into which we will load content).
			    var jContent = $("#content");

			    // Hook up link click events to load content.
			    $("#atest").click(
					function (objEvent) {
					    var jLink = $(this);

					    // Clear status list.
					    $("#ajax-status").empty();

					    // Launch AJAX request.
					    $.ajax(
							{
							    // The link we are accessing.
							    url: "/Index.aspx",

							    // The type of request.
							    type: "get",

							    // The type of data that is getting returned.
							    dataType: "html",

							    error: function () {
							        ShowStatus("AJAX - error()");

							        // Load the content in to the page.
							        jContent.html("<p>Page Not Found!!</p>");
							    },

							    beforeSend: function () {
							        ShowStatus("AJAX - beforeSend()");
							    },

							    complete: function () {
							        ShowStatus("AJAX - complete()");
							    },

							    success: function (strData) {
							        ShowStatus("AJAX - success()");

							        // Load the content in to the page.
							        jContent.html(strData);
							    }
							}
							);

					    // Prevent default click.
					    return (false);
					}
					);

			}
			);


        // I show the status on the screen.
        function ShowStatus(strStatus) {
            var jStatusList = $("#ajax-status");

            // Prepend the paragraph.
            jStatusList.prepend("<p>" + strStatus + "</p>");
        }
		
	</script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <a href="javascript:void(0);" id="atest">dsf</a>
    </div>
    <div id="content">
    
    </div>
    <div id="ajax-status">
    
    </div>
    
    </form>
</body>
</html>
