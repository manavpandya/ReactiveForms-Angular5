using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Memo Component Class Contains Memo related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class MemoComponent
    {

        #region Key Functions

        /// <summary>
        /// Add new Memo
        /// </summary>
        /// <param name="Memo">tb_Memo Memo</param>
        /// <returns>Returns Id of new inserted Memo</returns>
        public Int32 CreateMemo(tb_Memo Memo)
        {
            Int32 isAdded = 0;
            try
            {
                MemoDAC dac = new MemoDAC();
                dac.Create(Memo);
                isAdded = Memo.MemoID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Insert Memo Reply
        /// </summary>
        /// <param name="Memo">tb_MemoReply MemoReply</param>
        /// <returns>Returns the tb_MemoReply Object</returns>
        public Int32 CreateMemoReply(tb_MemoReply MemoReply)
        {
            Int32 isAdded = 0;
            try
            {
                MemoDAC dac = new MemoDAC();
                dac.CreateMemoReply(MemoReply);
                isAdded = MemoReply.ReplyID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }


        //Admin Side Functions
        /// <summary>
        /// Get Data of All Memo
        /// </summary>
        /// <returns>Returns All Memo Details</returns>
        public DataSet GetMemoByID(int MemoID)
        {
            MemoDAC objMemo = new MemoDAC();
            DataSet DSMemo = new DataSet();
            DSMemo = objMemo.GetMemoByID(MemoID);
            return DSMemo;
        }

        //Admin Side Functions
        /// <summary>
        /// Get Data of All Memo
        /// </summary>
        /// <returns>Returns All Memo Details</returns>
        public DataSet GetMemoByUserID(int UserID)
        {
            MemoDAC objMemo = new MemoDAC();
            DataSet DSMemo = new DataSet();
            DSMemo = objMemo.GetMemoByuserID(UserID);
            return DSMemo;
        }




        //Admin Side Functions
        /// <summary>
        /// Get Data of All MemoReply
        /// </summary>
        /// <returns>Returns All Memo Details</returns>
        public DataSet GetMemoReply(int MemoID)
        {
            MemoDAC objMemo = new MemoDAC();
            DataSet DSMemo = new DataSet();
            DSMemo = objMemo.GetMemoReply(MemoID);
            return DSMemo;
        }


        #endregion
    }
    
}
