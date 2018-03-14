using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
namespace Solution.Data
{
    public class LockProductDAC
    {
        #region Variables
        private  SQLAccess objSql = null;
        private Int32 _LockProductID;
        private Int32 _ProductID;
        private Int32 _Quantity;
        private Int32 _OrderNumber;
        private String _MarryProducts;
        private Boolean _IsCompleted;
        private Int32 _MarkQuantity;
        private Int32 _UpgradeQuantity;
        private Decimal _UpgradePrice;
        private Int32 _CustomCartID;
        #endregion

        #region Property


        public Int32 UpgradeQuantity
        {
            get { return _UpgradeQuantity; }
            set { _UpgradeQuantity = value; }
        }
        public Decimal UpgradePrice
        {
            get { return _UpgradePrice; }
            set { _UpgradePrice = value; }
        }
        public Int32 MarkQuantity
        {
            get { return _MarkQuantity; }
            set { _MarkQuantity = value; }
        }

        public Int32 LockProductID
        {
            get { return _LockProductID; }
            set { _LockProductID = value; }
        }
        public Int32 ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }
        public Int32 Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }
        public Int32 OrderNumber
        {
            get { return _OrderNumber; }
            set { _OrderNumber = value; }
        }
        public String MarryProducts
        {
            get { return _MarryProducts; }
            set { _MarryProducts = value; }
        }
        public Boolean IsCompleted
        {
            get { return _IsCompleted; }
            set { _IsCompleted = value; }
        }

        public Int32 OrderedCustomCartID
        {
            get { return _CustomCartID; }
            set { _CustomCartID = value; }
        }
        #endregion
        public enum Modes
        {
            Add = 1,
            Update = 2,
            Delete = 3
        }
        #region Add  Lock Products
        public int AddLockProduct()
        {
            objSql = new SQLAccess();
            SqlCommand myCommand = new SqlCommand("usp_LockProducts_New");
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand = AddParamater(myCommand);
            myCommand.Parameters.Add("@Mode", SqlDbType.Int).Value = Modes.Add;
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(paramReturnval);
            object f = objSql.ExecuteNonQuery(myCommand);
            return Convert.ToInt32(paramReturnval.Value);
        }
        #endregion

        #region AddParameter
        private SqlCommand AddParamater(SqlCommand myCommand)
        {
            
            myCommand.Parameters.Add("@ProductID", SqlDbType.Int).Value = this.ProductID;
            myCommand.Parameters.Add("@Quantity", SqlDbType.Int).Value = this.Quantity;
            myCommand.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = this.OrderNumber;
            myCommand.Parameters.Add("@MarryProducts", SqlDbType.NVarChar, 100).Value = this.MarryProducts;
            myCommand.Parameters.Add("@IsCompleted", SqlDbType.Bit).Value = this.IsCompleted;
            myCommand.Parameters.Add("@MarkQuantity", SqlDbType.Int).Value = this.MarkQuantity;
            myCommand.Parameters.Add("@UpgradeQuantity", SqlDbType.Int).Value = this.UpgradeQuantity;
            myCommand.Parameters.Add("@OrderedCustomcartID", SqlDbType.Int).Value = this.OrderedCustomCartID;
            myCommand.Parameters.Add("@UpgradePrice", SqlDbType.Money).Value = this.UpgradePrice;
            return myCommand;
        }
        #endregion

        #region Delete  Lock Products
        public int DeleteLockProduct()
        {
            objSql = new SQLAccess();
            SqlCommand myCommand = new SqlCommand("USP_LockProducts_New");
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add("@ProductID", SqlDbType.Int).Value = this.ProductID;
            myCommand.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = this.OrderNumber;
            myCommand.Parameters.Add("@Mode", SqlDbType.Int).Value = 3;

            return Convert.ToInt32(objSql.ExecuteNonQuery(myCommand));
        }
        #endregion

        public DataTable GetLockProductDetails(Int32 Pid, Int32 Ono)
        {
            objSql = new SQLAccess();
            string sql = "select * from tb_lockproducts where productid=" + Pid + " and ordernumber=" + Ono + " ";
            return objSql.GetDs(sql).Tables[0]; ;
        }
        public DataTable GetmarryProductDetails(Int32 Pid)
        {
            objSql = new SQLAccess();
            string sql = "select MarryProducts from tb_product where productid=" + Pid + "";
            return objSql.GetDs(sql).Tables[0]; ;
        }

        public Int32 SetMarkQuantityforLockProducts(Int32 Pid, Int32 Ono, Int32 Qty)
        {
            objSql = new SQLAccess();
            string sql = "update tb_lockproducts set MarkQuantity=isnull(MarkQuantity,0)+" + Qty + " where productid=" + Pid + " and ordernumber=" + Ono + " and isnull(iscompleted,0)=0";
            return Convert.ToInt32(objSql.ExecuteNonQuery(sql));
        }
        public Int32 SetMarkUPgradeQuantityforLockProducts(Int32 Pid, Int32 Ono, Int32 Qty)
        {
            objSql = new SQLAccess();
            string sql = "update tb_lockproducts set MarkUpgradeQuantity=isnull(MarkUpgradeQuantity,0)+" + Qty + " where marryproducts=(select SKU from tb_product where productid in (" + Pid + ")) and ordernumber=" + Ono + " and isnull(iscompleted,0)=0";
            return Convert.ToInt32(objSql.ExecuteNonQuery(sql));
        }
        public DataTable GetStatusForLockProducts(Int32 Ono)
        {
            objSql = new SQLAccess();
            string sql = " select (isnull(sum(quantity),0) + isnull(sum(UpgradeQuantity),0)) as Quantity,isnull(sum(isnull(MarkQuantity,0)),0) as ProcessQuantity from tb_LockProducts where  ordernumber=" + Ono + " and isnull(iscompleted,0)=0";
            return objSql.GetDs(sql).Tables[0];
        }
        public DataTable GetStatusForLockProductsByONo(Int32 Ono)
        {
            objSql = new SQLAccess();
            string sql = " select ordernumber from tb_LockProducts where  ordernumber=" + Ono + " and isnull(iscompleted,0)=0";
            return objSql.GetDs(sql).Tables[0];
        }

        public DataTable GetStatusForLockProductsNew(Int32 Ono)
        {
            objSql = new SQLAccess();
            string sql = " select isnull(sum(quantity),0) as Quantity, isnull(sum(UpgradeQuantity),0) as UpQuantity,(isnull(sum(isnull(MarkQuantity,0)),0)+isnull(sum(isnull(MarkUpgradeQuantity,0)),0)) as ProcessQuantity from tb_LockProducts where  ordernumber=" + Ono + " and isnull(iscompleted,0)=0";
            return objSql.GetDs(sql).Tables[0];
        }

        public DataSet GetMarryProductByUpccode(string Upccode)
        {
            objSql = new SQLAccess();
            DataSet dsCode = new DataSet();
            dsCode = objSql.GetDs("SELECT p.* FROM tb_Product p inner join tb_LockProducts lp on (select productid from tb_product WHERE SKU in (lp.marryproducts))=p.productid WHERE p.UPC='" + Upccode + "' and (lp.MarkUpgradeQuantity)<lp.UpgradeQuantity");
            return dsCode;
        }

        public DataSet GetProductByUpccode(string Upccode)
        {
            objSql = new SQLAccess();
            DataSet dsCode = new DataSet();
            dsCode = objSql.GetDs("SELECT p.* FROM tb_Product p inner join tb_LockProducts lp on lp.productid=p.productid WHERE p.UPC='" + Upccode + "' and lp.markquantity<lp.quantity + lp.UpgradeQuantity");
            return dsCode;
        }

        public DataSet GetProductByUpccodeNew(string Upccode)
        {
            objSql = new SQLAccess();
            DataSet dsCode = new DataSet();
            dsCode = objSql.GetDs("SELECT p.* FROM tb_Product p inner join tb_LockProducts lp on lp.productid=p.productid WHERE p.UPC='" + Upccode + "' and lp.markquantity<lp.quantity");
            return dsCode;
        }

        public Boolean CheckProcessQuantityforLockProducts(Int32 Ono)
        {
            objSql = new SQLAccess();
            string sql = "select isnull(1,0) from tb_LockProducts where ordernumber=" + Ono + " and (markquantity + markUpgradequantity)<(quantity + UpgradeQuantity)";
            return Convert.ToBoolean(objSql.ExecuteScalarQuery(sql));
        }

        public DataTable GetStatusForLockProducts(Int32 Ono, Int32 Pid)
        {
            objSql = new SQLAccess();
            string sql = " select isnull(sum(quantity),0) as Quantity,isnull(sum(isnull(MarkQuantity,0)),0) as ProcessQuantity from tb_LockProducts where  ordernumber=" + Ono + " and ProductID=" + Pid;
            return objSql.GetDs(sql).Tables[0];
        }
        public DataSet GetprocessQuantity(Int32 OrderNo)
        {
            objSql = new SQLAccess();
            string sql = "SELECT dbo.tb_Product.Name as ProductName, dbo.tb_Product.SKU, " +
                       "dbo.tb_LockProducts.MarkQuantity as Quantity,'Main' as Type " +
                     "FROM         dbo.tb_LockProducts INNER JOIN" +
                     " dbo.tb_Product ON dbo.tb_LockProducts.ProductID = dbo.tb_Product.ProductID " +
                     "WHERE     (dbo.tb_LockProducts.IsCompleted = 0) AND (dbo.tb_LockProducts.MarkQuantity > 0) AND dbo.tb_LockProducts.OrderNumber=" + OrderNo + "" +
                     " UNION ALL SELECT dbo.tb_Product.Name as ProductName, dbo.tb_Product.SKU, " +
                       "dbo.tb_LockProducts.MarkUpgradeQuantity as Quantity,'Upgrade' as Type " +
                     "FROM         dbo.tb_LockProducts INNER JOIN" +
                     " dbo.tb_Product ON dbo.tb_LockProducts.marryproducts = dbo.tb_Product.SKU " +
                     "WHERE     (dbo.tb_LockProducts.IsCompleted = 0) AND (dbo.tb_LockProducts.MarkUpgradeQuantity > 0) AND dbo.tb_LockProducts.OrderNumber=" + OrderNo + "";

            DataSet ds = objSql.GetDs(sql);

            return ds;
        }

        public Int32 AddQuantity(Int32 ProductID, Int32 Quantity)
        {
            objSql = new SQLAccess();
            string sql = " update tb_product set inventory=inventory+(" + Quantity + ") " +
                          "  where productid=" + ProductID + "  ";
            return Convert.ToInt32(objSql.ExecuteNonQuery(sql));
        }
        public Int32 AddQuantitywithSKU(String SKU, Int32 Quantity, Int32 storeid)
        {
            objSql = new SQLAccess();
            string sql = " update tb_product set inventory=inventory+(" + Quantity + ") " +
                          "  where SKU='" + SKU + "'  AND storeid=" + storeid + "";
            return Convert.ToInt32(objSql.ExecuteNonQuery(sql));
        }
        public Int32 AddQuantitywithorderedShopping(String SKU, Int32 Quantity, Int32 productId, Int32 orderId)
        {
            objSql = new SQLAccess();
            string sql = " update tb_OrderedShoppingCartItems set MarryProducts='" + SKU + "',MarryproductQuantity=isnull(MarryproductQuantity,0)+(" + Quantity + "),MarryWithtotalQuantity=(Quantity)-(" + Quantity + ")" +
                          "  where RefproductId=" + productId + " AND OrderedShoppingCartID in(SELECT ShoppingCardID FROM tb_order WHERE OrderNumber=" + orderId + ")  ";
            return Convert.ToInt32(objSql.ExecuteNonQuery(sql));
        }
        public Int32 AddQuantitywithorderedShoppingDelete(String SKU, Int32 Quantity, Int32 productId, Int32 orderId)
        {
            objSql = new SQLAccess();
            string sql = " update tb_OrderedShoppingCartItems set MarryProducts='" + SKU + "',MarryproductQuantity=isnull(MarryproductQuantity,0)+(" + Quantity + "),MarryWithtotalQuantity=(MarryWithtotalQuantity)-(" + Quantity + ")" +
                          "  where RefproductId=" + productId + " AND OrderedShoppingCartID in(SELECT ShoppingCardID FROM tb_order WHERE OrderNumber=" + orderId + ")  ";
            return Convert.ToInt32(objSql.ExecuteNonQuery(sql));
        }

        public Int32 GetInventoryforProduct(Int32 Pid, Int32 storeid)
        {
            objSql = new SQLAccess();
            string sql = "select top 1 isnull(inventory,0) from tb_product where productid=" + Pid + " and Storeid=" + storeid + " ";
            return Convert.ToInt32(objSql.ExecuteScalarQuery(sql));
        }
        public Int32 GetInventoryforProduct(String sku, Int32 storeid)
        {
            objSql = new SQLAccess();
            string sql = "select top 1 isnull(inventory,0) from tb_product where SKU='" + sku + "' and Storeid=" + storeid + " ";
            return Convert.ToInt32(objSql.ExecuteScalarQuery(sql));
        }

        public Int32 SetApproveOrder(Int32 Ono, Decimal dAdjustments, Decimal customdis)
        {
            objSql = new SQLAccess();
            string sql = "update tb_order set isapproved=1  where ordernumber=" + Ono;
            return Convert.ToInt32(objSql.ExecuteNonQuery(sql));
        }
        public DataSet GetOrderCartNew(Int32 OrderNumber)
        {
            objSql = new SQLAccess();
            SqlCommand myCommand = new SqlCommand("USP_LockProducts_New");
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = OrderNumber;
            myCommand.Parameters.Add("@Mode", SqlDbType.Int).Value = 4;
            return objSql.GetDs(myCommand);

        }
        public DataSet GetOrderCartNewafterApprove(Int32 OrderNumber)
        {
            objSql = new SQLAccess();
            SqlCommand myCommand = new SqlCommand("USP_LockProducts_New");
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = OrderNumber;
            myCommand.Parameters.Add("@Mode", SqlDbType.Int).Value = 5;
            return objSql.GetDs(myCommand);

        }
        
        public int ApproveOrder(String OrderNo)
        {
            objSql = new SQLAccess();
            SqlCommand myCommand = new SqlCommand("USP_ApproveOrder");
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrderNo;
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(paramReturnval);
            object f = objSql.ExecuteNonQuery(myCommand);
            return Convert.ToInt32(paramReturnval.Value);

        }
    }
}
