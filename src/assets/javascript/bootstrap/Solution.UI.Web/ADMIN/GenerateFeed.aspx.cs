using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;



namespace Solution.UI.Web
{
    public partial class GenerateFeed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, ImageClickEventArgs e)
        {
            GetOrderInfo();
            GetCancelOrders();
        }
        /// <summary>
        /// Function for Remove comma From String
        /// </summary>
        /// <param name="sFieldValueToEscape">String sFieldValueToEscape</param>
        /// <returns>return String</returns>
        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            if (sFieldValueToEscape.Contains(","))
            {
                if (sFieldValueToEscape.Contains("\""))
                {
                    return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "\"" + sFieldValueToEscape + "\"";
                }
            }
            else
            {
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";//sFieldValueToEscape;
            }
        }

        /// <summary>
        /// Write data in the file
        /// </summary>
        /// <param name="Text">string Text</param>
        /// <param name="FileName">string FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            try
            {
                FileInfo info = new FileInfo(FileName);
                writer = info.CreateText();
                writer.Write(Text);
            }
            catch { }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        private void GetCancelOrders()
        {
            DataSet dsCancellorders = CommonComponent.GetCommonDataSet("select Distinct tb_Order.OrderNumber as [merchant order id],'MerchantCanceled' as reason from tb_Order where isnull(tb_Order.Deleted,0)=0 and (isnull(OrderStatus,'')='Canceled' or isnull(TransactionStatus,'')='Canceled' ) and StoreID=1");
            if(dsCancellorders!=null && dsCancellorders.Tables.Count>0 && dsCancellorders.Tables[0].Rows.Count>0)
            {
                CustomizableAmazonCSV(dsCancellorders, "Canceled-GTS");
            }
        }


        private void GetOrderInfo()
        {
            DataSet dsOrders = CommonComponent.GetCommonDataSet("select Distinct tb_OrderShippedItems.OrderNumber as [merchant order id],TrackingNumber as [tracking number],tb_OrderShippedItems.ShippedVia as [carrier code],Replace(convert(char(10),tb_OrderShippedItems.ShippedOn,111),'/','-') as  [ship date] 	 from tb_OrderShippedItems inner join tb_Order on tb_OrderShippedItems.OrderNumber=tb_Order.OrderNumber where isnull(tb_Order.Deleted,0)=0 and isnull(OrderStatus,'')='Shipped' and tb_order.storeid=1");
            if(dsOrders!=null && dsOrders.Tables.Count>0 && dsOrders.Tables[0].Rows.Count>0)
            {
                CustomizableAmazonCSV(dsOrders, "Shipping-GTS");
            }

        }

        private void CustomizableAmazonCSV(DataSet Ds,string filename)
        {
            try
            {
                //if (ddlCriteria.SelectedValue != "1")
                //{
                //    if (txtStartDate.Text.ToString().Trim() != "" && txtEndDate.Text.ToString().Trim() != "")
                //    {
                //        if (Convert.ToDateTime(txtEndDate.Text) >= Convert.ToDateTime(txtStartDate.Text))
                //            SetValues();
                //        else
                //        {
                //            lblMsg.Text = "Start Date should be smaller than End Date.";
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        lblMsg.Text = "Please select Date.";
                //        return;
                //    }
                //}
                //else
                //    SetValues();

                Int32 cnt = 0;
                String strfields = "", strFieldsNames = "";
                if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                {
                    for (int iCol = 0; iCol < Ds.Tables[0].Columns.Count; iCol++)
                    {
                        strfields += "{" + cnt + "}\t";
                        strFieldsNames += Ds.Tables[0].Columns[iCol].ToString() + "\t";
                        cnt++;
                    }
                }
                if (strfields.Length > 1)
                {
                    strfields = strfields.Substring(0, strfields.Length - 1);
                    strFieldsNames = strFieldsNames.Substring(0, strFieldsNames.Length - 1);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                {
                    object[] args = new object[cnt];
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        Int32 j = 0;

                        for (int iCol = 0; iCol < Ds.Tables[0].Columns.Count; iCol++)
                        {
                            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[i][Ds.Tables[0].Columns[iCol].ToString()].ToString()))
                            {
                                string strResponse = Ds.Tables[0].Rows[i][Ds.Tables[0].Columns[iCol].ToString()].ToString();
                                strResponse = strResponse.Replace("\t", "");
                                strResponse = strResponse.Replace("\" />", "\"/>");
                                strResponse = strResponse.Replace("\" >", "\">");
                                strResponse = strResponse.Replace("©", "&copy;");
                                strResponse = strResponse.Replace("®", "&reg;");
                                args[iCol] = strResponse.ToString().Replace("\r\n", "");
                            }
                            else
                            {
                                args[iCol] = "";
                            }
                        }
                        j++;
                        sb.AppendLine(string.Format(strfields, args));
                    }

                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    //if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("canada") > -1)
                    //{
                   // sb.AppendLine("TemplateType=ConsumerElectronics\tVersion=1.7/1.2.9\tThis row for Amazon.com use only.  Do not modify or delete.");
                    //}
                    sb.AppendLine(strFieldsNames);
                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    String FileName = filename+".txt";
                   // String CSVPath = Convert.ToString(AppLogic.AppConfigs("YahooProductCSV"));
                    if (!Directory.Exists(Server.MapPath("/Admin/Files/")))
                        Directory.CreateDirectory(Server.MapPath("/Admin/Files/"));

                    //try
                    //{
                    //Response.Clear();
                    //Response.ClearContent();
                    String FilePath = Server.MapPath("/Admin/Files/" + FileName);
                    WriteFile(sb.ToString(), FilePath);
                    //Response.ContentType = "text/plain";
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    //Response.TransmitFile(FilePath);
                    //Response.End();
                    //}
                    //catch { }
                }
            }
            catch
            { }
        }
    }
}