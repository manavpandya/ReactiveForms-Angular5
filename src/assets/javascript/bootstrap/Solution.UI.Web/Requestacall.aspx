<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Requestacall.aspx.cs" Inherits="Solution.UI.Web.Requestacall" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
  <form method="post" >
        <fieldset>
          <legend>Contact Us</legend>
          <div>
            <label for="Name">Name:</label>
            <input type="text" name="Name" value="" />
          </div>
          <div>
            <label for="Email">Email:</label>
            <input type="text" name="Email" value="" />
          </div>
          <div>
            <label for="City">City:</label>
            <input type="text" name="City" value="" />
          </div>
          <div>
            <label for="Address">Address:</label>
            <input type="text" name="Address" value="" />
          </div>
          <div>
            <label for="Country">Country:</label>
            <input type="text" name="Country" value="" />
          </div>
          <div>
            <label for="STORENAME">STORENAME:</label>
            <input type="hidden" name="STORENAME" value="" />
          </div>
          
          <div>
            <label>&nbsp;</label>
            <input type="submit" value="Submit" class="submit" />
          </div>
        </fieldset>
      </form>
</body>
</html>
