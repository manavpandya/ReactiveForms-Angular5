using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Solution.Data;
using System.Data;


namespace Solution.Bussines.Components
{
    public class LockProductComponent
    {
        public int AddLockProduct(Int32 ProductID, Int32 Quantity, Int32 OrderNumber, string MarryProducts, bool IsCompleted, Int32 MarkQuantity, Int32 UpgradeQuantity, Int32 OrderedCustomCartID, decimal UpgradePrice)
        {
            LockProductDAC objProduct = new LockProductDAC();
            objProduct.ProductID = ProductID;
            objProduct.Quantity = Quantity;
            objProduct.OrderNumber = OrderNumber;
            objProduct.MarryProducts = MarryProducts;
            objProduct.IsCompleted = IsCompleted;
            objProduct.MarkQuantity = MarkQuantity;
            objProduct.UpgradeQuantity = UpgradeQuantity;
            objProduct.OrderedCustomCartID = OrderedCustomCartID;
            objProduct.UpgradePrice = UpgradePrice;
            return Convert.ToInt32(objProduct.AddLockProduct());

        }
        public int DeleteLockProduct(Int32 ProductID, Int32 OrderNumber)
        {
            LockProductDAC objProduct = new LockProductDAC();
            objProduct.ProductID = ProductID;
            objProduct.OrderNumber = OrderNumber;
            return Convert.ToInt32(objProduct.DeleteLockProduct());
        }
        public DataTable GetLockProductDetails(Int32 Pid, Int32 Ono)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return objProduct.GetLockProductDetails(Pid, Ono);
        }
        public DataTable GetmarryProductDetails(Int32 Pid)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return objProduct.GetmarryProductDetails(Pid);
        }
        public Int32 SetMarkQuantityforLockProducts(Int32 Pid, Int32 Ono, Int32 Qty)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return Convert.ToInt32(objProduct.SetMarkQuantityforLockProducts(Pid, Ono, Qty));
        }
        public Int32 SetMarkUPgradeQuantityforLockProducts(Int32 Pid, Int32 Ono, Int32 Qty)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return Convert.ToInt32(objProduct.SetMarkUPgradeQuantityforLockProducts(Pid, Ono, Qty));
        }
        public DataTable GetStatusForLockProducts(Int32 Ono)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return objProduct.GetStatusForLockProducts(Ono);
        }
        public DataTable GetStatusForLockProductsByONo(Int32 Ono)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return objProduct.GetStatusForLockProductsByONo(Ono);
        }
        public DataTable GetStatusForLockProductsNew(Int32 Ono)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return objProduct.GetStatusForLockProductsNew(Ono);
        }
        public DataSet GetMarryProductByUpccode(string Upccode)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return objProduct.GetMarryProductByUpccode(Upccode);
        }
        public DataSet GetProductByUpccode(string Upccode)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return objProduct.GetProductByUpccode(Upccode);
        }
        public DataSet GetProductByUpccodeNew(string Upccode)
        {
            LockProductDAC objProduct = new LockProductDAC();
            return objProduct.GetProductByUpccodeNew(Upccode);
        }
        public Boolean CheckProcessQuantityforLockProducts(Int32 Ono)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return Convert.ToBoolean(objProduct.CheckProcessQuantityforLockProducts(Ono));
        }
        public DataTable GetStatusForLockProducts(Int32 Ono, Int32 Pid)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return objProduct.GetStatusForLockProducts(Ono, Pid);
        }
        public DataSet GetprocessQuantity(Int32 OrderNo)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return objProduct.GetprocessQuantity(OrderNo);
        }
        public Int32 AddQuantity(Int32 ProductID, Int32 Quantity)
        {
            //LockProductDAC objProduct = new LockProductDAC();
            //return Convert.ToInt32(objProduct.AddQuantity(ProductID, Quantity));
            return 1;
        }
        public Int32 AddQuantitywithSKU(String SKU, Int32 Quantity, Int32 storeid)
        {
            //LockProductDAC objProduct = new LockProductDAC();
            //return Convert.ToInt32(objProduct.AddQuantitywithSKU(SKU, Quantity, storeid));
            return 1;
        }
        public Int32 AddQuantitywithorderedShopping(String SKU, Int32 Quantity, Int32 productId, Int32 orderId)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return Convert.ToInt32(objProduct.AddQuantitywithorderedShopping(SKU, Quantity, productId, orderId));
        }
        public Int32 AddQuantitywithorderedShoppingDelete(String SKU, Int32 Quantity, Int32 productId, Int32 orderId)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return Convert.ToInt32(objProduct.AddQuantitywithorderedShoppingDelete(SKU, Quantity, productId, orderId));
        }
        public Int32 GetInventoryforProduct(Int32 Pid, Int32 storeid)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return Convert.ToInt32(objProduct.GetInventoryforProduct(Pid, storeid));
        }
        public Int32 GetInventoryforProduct(String sku, Int32 storeid)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return Convert.ToInt32(objProduct.GetInventoryforProduct(sku, storeid));
        }
        public Int32 SetApproveOrder(Int32 Ono, Decimal dAdjustments, Decimal customdis)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return Convert.ToInt32(objProduct.SetApproveOrder(Ono, dAdjustments, customdis));
        }
        public DataSet GetOrderCartNew(Int32 OrderNumber)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return objProduct.GetOrderCartNew(OrderNumber);
        }
        public DataSet GetOrderCartNewafterApprove(Int32 OrderNumber)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return objProduct.GetOrderCartNewafterApprove(OrderNumber);
        }
        public int ApproveOrder(String OrderNo)
        {
            LockProductDAC objProduct = new LockProductDAC();

            return Convert.ToInt32(objProduct.ApproveOrder(OrderNo));
        }

    }
}
