using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class CustomerAvailabilityList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                Session["namesearch"] = "";
                Session["nameemail"] = "";
                Session["namesku"] = "";
                Session["namepname"] = "";

                bindstore();
                AppConfig.StoreID = 1;

                ddlStore.SelectedIndex = AppConfig.StoreID;

                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");

                Binddata();
            }

        }



        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            //Store is selected dynamically from menu
            if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]))
                ddlStore.SelectedValue = Request.QueryString["StoreID"].ToString();
            else
                ddlStore.SelectedIndex = 0;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdcustomernotify.PageIndex = 0;
            grdcustomernotify.DataBind();
            DataSet dsgrid = new DataSet();
            string searchtext = "";
            if (txtMailFrom.Text.ToString() != "" && txtMailTo.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtMailTo.Text.ToString()) >= Convert.ToDateTime(txtMailFrom.Text.ToString()))
                {

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            else
            {
                if (txtMailFrom.Text.ToString() == "" && txtMailTo.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
                else if (txtMailTo.Text.ToString() == "" && txtMailFrom.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }

            }

            if (ddlSearch.SelectedValue.ToString().ToLower() == "name")
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    searchtext = txtSearch.Text;
                    dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku, a.mailsenddate, a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate, a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where isnull(a.MailID,0) >= 0 and a.FirstName like '%" + searchtext + "%' or a.LastName like '%" + searchtext + "%' order by a.AvailabilityID desc");
                    grdcustomernotify.DataSource = dsgrid;
                    grdcustomernotify.DataBind();
                    lbltotala.Text = dsgrid.Tables[0].Rows.Count.ToString();
                    Session["namesearch"] = searchtext;
                }
            }
            else if (ddlSearch.SelectedValue.ToString().ToLower() == "email")
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    searchtext = txtSearch.Text;
                    //dsgrid = CommonComponent.GetCommonDataSet("select a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.Email like '%" + searchtext + "%' order by a.AvailabilityID desc");
                    if (txtMailFrom.Text.ToString() != "")
                        dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku, a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.Email like '%" + searchtext + "%'   AND Cast(a.mailsenddate as date) >= Cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)  order by a.AvailabilityID desc");
                    else if (txtMailTo.Text.ToString() != "")
                        dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku, a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.Email like '%" + searchtext + "%'  AND Cast(a.mailsenddate as date) <= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date) order by a.AvailabilityID desc");
                    else
                        dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku, a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.Email like '%" + searchtext + "%' order by a.AvailabilityID desc");
                    grdcustomernotify.DataSource = dsgrid;
                    grdcustomernotify.DataBind();
                    lbltotala.Text = dsgrid.Tables[0].Rows.Count.ToString();
                    Session["nameemail"] = searchtext;
                }
            }
            else if (ddlSearch.SelectedValue.ToString().ToLower() == "sku")
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    searchtext = txtSearch.Text;
                    //dsgrid = CommonComponent.GetCommonDataSet("select a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where SKU like '%" + searchtext + "%' and ProductID=a.ProductID) order by a.AvailabilityID desc");
                    if (txtMailFrom.Text.ToString() != "")
                        dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku,a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where SKU like '%" + searchtext + "%' and ProductID=a.ProductID)   AND Cast(a.mailsenddate as date) >= Cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date) order by a.AvailabilityID desc");
                    else if (txtMailTo.Text.ToString() != "")
                        dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku,a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where SKU like '%" + searchtext + "%' and ProductID=a.ProductID)  AND Cast(a.mailsenddate as date) <= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date) order by a.AvailabilityID desc");
                    else
                        dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku,a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where SKU like '%" + searchtext + "%' and ProductID=a.ProductID) order by a.AvailabilityID desc");
                    grdcustomernotify.DataSource = dsgrid;
                    grdcustomernotify.DataBind();
                    lbltotala.Text = dsgrid.Tables[0].Rows.Count.ToString();
                    Session["namesku"] = searchtext;
                }
            }
            else if (ddlSearch.SelectedValue.ToString().ToLower() == "productname")
            {
                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    searchtext = txtSearch.Text;
                    //DataSet dsgrid = CommonComponent.GetCommonDataSet("select a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where Name like '%" + searchtext + "%' and ProductID=a.ProductID) order by a.AvailabilityID desc");
                    if (txtMailFrom.Text.ToString() != "")
                    {
                        dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku,a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where Name like '%" + searchtext + "%' and ProductID=a.ProductID) AND Cast(a.mailsenddate as date) >= Cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date) order by a.AvailabilityID desc");
                    }
                    else if (txtMailTo.Text.ToString() != "")
                    {
                        dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku,a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where Name like '%" + searchtext + "%' and ProductID=a.ProductID)  AND Cast(a.mailsenddate as date) <= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date) order by a.AvailabilityID desc");
                    }
                    else
                        dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku,a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,(select MailDate from tb_MailLog m where a.mailid = m.MailID) MailDate,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where Name like '%" + searchtext + "%' and ProductID=a.ProductID) order by a.AvailabilityID desc");

                    grdcustomernotify.DataSource = dsgrid;
                    grdcustomernotify.DataBind();
                    lbltotala.Text = dsgrid.Tables[0].Rows.Count.ToString();
                    Session["namepname"] = searchtext;
                }
            }
            else
            {
                Binddata();
                Session["namesearch"] = "";
                Session["nameemail"] = "";
                Session["namesku"] = "";
                Session["namepname"] = "";
            }
            SetSession();
        }

        private void SetSession()
        {
            Session["SIDSearch"] = ddlStore.SelectedValue.ToString();
            // Session["StockSearch"] = ddlProductTypeDelivery.SelectedValue.ToString();
            // Session["ProdctTypeSearch"] = ddlProductType.SelectedValue.ToString();
            // Session["CategorySearch"] = ddlCategory.SelectedValue.ToString();
            // Session["StatusSearch"] = ddlStatus.SelectedValue.ToString();
            Session["SearchBy"] = ddlSearch.SelectedValue.ToString();
            Session["SearchText"] = txtSearch.Text.ToString();
            Session["SearchGridpage"] = grdcustomernotify.PageIndex.ToString();

        }
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            Binddata();
            Session["namesearch"] = "";
            Session["nameemail"] = "";
            Session["namesku"] = "";
            Session["namepname"] = "";
            txtSearch.Text = "";
            SetSession();
            ddlSearch.SelectedValue = "SKU";
        }

        protected void grdProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdcustomernotify.Rows.Count > 0)
            {
            }
            else
            {
                trBottom.Visible = false;
                ViewState["ExportProductIds"] = null;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Literal ltMailSent = (Literal)e.Row.FindControl("ltMailSent");
                if (ltMailSent.Text.ToString() == "1" || ltMailSent.Text.ToString().ToLower() == "true" || ltMailSent.Text.ToString() == "True")
                {
                    ltMailSent.Text = "<span style=\"color:Green;font-weight: bold;\">Yes</span>";
                }
                else
                {
                    ltMailSent.Text = "<span style=\"color:Red;font-weight: bold;\">No</span>";
                }
                System.Web.UI.HtmlControls.HtmlGenericControl aviewlink = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("aviewlink");
                Literal ltviewmail = (Literal)e.Row.FindControl("ltviewmail");
                if (!string.IsNullOrEmpty(ltviewmail.Text.ToString()) && ltviewmail.Text != "0")
                {
                    aviewlink.Visible = true;
                    //ltviewmail.Text = "<a href=\"javascript:void(0);\" style=\"color: #212121  ;\" onclick=\"OpenCenterWindow('../Reports/Maildetail.aspx?MID=<%# DataBinder.Eval(Container.DataItem,\"MailID\") %>',800,800);\"><img src=\"/App_Themes/<%=Page.Theme %>/images/view-details.png\" border=\"0\" /></a>";
                }
                else
                {
                    aviewlink.Visible = false;
                }
                trBottom.Visible = true;


            }

        }
        protected void grdProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource.GetType() != typeof(GridView))
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);
                Label lblSalePrice = (Label)row.FindControl("lblSalePrice");
                Label lblPrice = (Label)row.FindControl("lblPrice");
                Label lblInventory = row.FindControl("lblInventory") as Label;
                TextBox tbSalePrice = (TextBox)row.FindControl("txtSalePrice");
                TextBox tbPrice = (TextBox)row.FindControl("txtPrice");
                TextBox tbInventory = row.FindControl("txtInventory") as TextBox;
                ImageButton btnSave = row.FindControl("btnSave") as ImageButton;
                ImageButton btnCancel = row.FindControl("btnCancel") as ImageButton;
                ImageButton btnEditPrice = row.FindControl("btnEditPrice") as ImageButton;
                HiddenField hdnProductID = row.FindControl("hdnProductid") as HiddenField;
                CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                if (e.CommandName == "EditPrice")
                {
                    tbSalePrice.Visible = true;
                    tbPrice.Visible = true;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    lblSalePrice.Visible = false;
                    lblPrice.Visible = false;
                    btnEditPrice.Visible = false;
                }
                else if (e.CommandName == "Cancel")
                {
                    btnEditPrice.Visible = false;

                    btnSave.Visible = true;


                    tbSalePrice.Visible = false;
                    tbPrice.Visible = false;
                    chkSelect.Checked = false;
                }
            }
        }


        protected void ddlStore_SelectedIndexchange(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
        }

        protected void ddlMailStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataSet dsmailsend = new DataSet();
                if (ddlMailStatus.SelectedItem.Value != "")
                {

                    if (ddlMailStatus.SelectedItem.Value != "" && ddlMailStatus.SelectedItem.Value == "1")
                    {
                        dsmailsend = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku, a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID,a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU, a.MailSendDate ,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a WHERE  isnull(a.MailID,0) >= 0 and isnull(a.mailsent,0)  = 1  order by a.AvailabilityID desc");
                        grdcustomernotify.DataSource = dsmailsend;
                        grdcustomernotify.DataBind();
                    }
                    else if (ddlMailStatus.SelectedItem.Value != "" && ddlMailStatus.SelectedItem.Value == "0")
                    {
                        dsmailsend = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku, a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID,a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU, a.MailSendDate ,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a WHERE  isnull(a.MailID,0) >= 0 and isnull(a.mailsent,0)  = 0  order by a.AvailabilityID desc");
                        grdcustomernotify.DataSource = dsmailsend;
                        grdcustomernotify.DataBind();
                    }
                    else
                    {
                        DataSet dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku, a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID,a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU, a.MailSendDate ,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a WHERE  isnull(a.MailID,0) >= 0  order by a.AvailabilityID desc");
                        grdcustomernotify.DataSource = dsgrid;
                        grdcustomernotify.DataBind();
                    }

                }
            }
            catch
            {

            }
        }
        private void Binddata()
        {
            try
            {
                DataSet dsgrid = CommonComponent.GetCommonDataSet("select ISNULL(a.productsku,'') as childsku, a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID,a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU, a.MailSendDate ,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a WHERE  isnull(a.MailID,0) >= 0  order by a.AvailabilityID desc");
                grdcustomernotify.DataSource = dsgrid;
                grdcustomernotify.DataBind();

                lbltotala.Text = dsgrid.Tables[0].Rows.Count.ToString();
            }
            catch
            {
                grdcustomernotify.DataSource = null;
                grdcustomernotify.DataBind();
                lbltotala.Text = "0";
            }

        }

        protected void grdcustomernotify_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string strSql = "";
            grdcustomernotify.PageIndex = e.NewPageIndex;
            try
            {

                if (Session["namesearch"] != null && Session["namesearch"].ToString() != "")
                {
                    strSql = "select  ISNULL(a.productsku,'') as childsku,a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU,  a.MailSendDate , a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.FirstName like '%" + Session["namesearch"].ToString() + "%' or a.LastName like '%" + Session["namesearch"].ToString() + "%'  ";

                }
                else if (Session["nameemail"] != null && Session["nameemail"].ToString() != "")
                {
                    strSql = "select  ISNULL(a.productsku,'') as childsku,a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID,a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU, a.MailSendDate ,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.Email like '%" + Session["nameemail"].ToString() + "%' order by a.AvailabilityID desc";

                }
                else if (Session["namesku"] != null && Session["namesku"].ToString() != "")
                {
                    strSql = "select  ISNULL(a.productsku,'') as childsku,a.mailsenddate,a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU, a.MailSendDate ,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where SKU like '%" + Session["namesku"].ToString() + "%' and ProductID=a.ProductID)";

                }
                else if (Session["namepname"] != null && Session["namepname"].ToString() != "")
                {
                    strSql = "select  ISNULL(a.productsku,'') as childsku,a.mailsenddate, a.MailSent,isnull(a.MailID,0) as MailID, a.AvailabilityID, a.ProductID,(select Name from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductName,(select SKU from tb_Product p where p.ProductID=a.ProductID and Active=1 and ISNULL(Deleted,0)=0) ProductSKU, a.MailSendDate ,a.FirstName+' '+a.LastName as Name,a.Email from tb_AvailabilityNotification a  where  isnull(a.MailID,0) >= 0 and a.ProductID in (select ProductID from tb_Product where Name like '%" + Session["namepname"].ToString() + "%' and ProductID=a.ProductID) ";

                }
                if (txtMailFrom.Text.ToString() != "" && txtMailTo.Text.ToString() != "")
                {
                    if (Convert.ToDateTime(txtMailTo.Text.ToString()) >= Convert.ToDateTime(txtMailFrom.Text.ToString()))
                    {

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }
                }
                else
                {
                    if (txtMailFrom.Text.ToString() == "" && txtMailTo.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }
                    else if (txtMailTo.Text.ToString() == "" && txtMailFrom.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                        return;
                    }

                }


                if (txtMailFrom.Text.ToString() != "")
                {
                    strSql += " AND Cast( a.mailsenddate as date) >= Cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
                }
                if (txtMailTo.Text.ToString() != "")
                {

                    strSql += " AND Cast( a.mailsenddate as date) <= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
                }
                strSql = strSql + " order by a.AvailabilityID desc";
            }
            catch
            {

            }

            DataSet dsgrid = CommonComponent.GetCommonDataSet(strSql);
            if (dsgrid != null && dsgrid.Tables.Count > 0 && dsgrid.Tables[0].Rows.Count > 0)
            {
                grdcustomernotify.DataSource = grdcustomernotify;
                grdcustomernotify.DataBind();


            }
            else
            {
                grdcustomernotify.DataSource = null;
                grdcustomernotify.DataBind();
                Session["namesearch"] = "";
                Session["nameemail"] = "";
                Session["namesku"] = "";
                Session["namepname"] = "";
                Binddata();
            }
            grdcustomernotify.ShowHeader = true;
        }
    }
}