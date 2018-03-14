<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DropDowntttt.aspx.cs" Inherits="Solution.UI.Web.DropDowntttt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <script type="text/javascript" src="/js/jquery.min-dropdown.js"></script>
    
     <script type="text/javascript" src="/js/dropit.js"></script>
      <link rel="stylesheet" href="/css/dropit.css" type="text/css" />
        <script type="text/javascript">
            var $u = jQuery.noConflict();
            $u(document).ready(function () {
                $u('.dropit').dropit();
            });
</script>
</head>
<body>
    <form id="form1" runat="server">
     <div>
      <ul class="dropit">
    <li class="dropit-trigger dropit-open">
        <a href="#">Dropdown</a>
        <ul class="dropit-submenu">
            <li><a href="#">Some Action 1</a></li>
            <li><a href="#">Some Action 2</a></li>
            <li><a href="#">Some Action 3</a></li>
            <li><a href="#">Some Action 4</a></li>
        </ul>
    </li>
</ul>          
     </div>
    <div>
    
    </div>
    </form>
</body>
</html>
