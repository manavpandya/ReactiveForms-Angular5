using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;

namespace Solution.Bussines.Components
{
  public  class HeaderComponent
    {
        /// <summary>
        /// Add new Topic
        /// </summary>
        /// <param name="topic">tb_Topic topic</param>
        /// <returns>Returns Id of new inserted Topic</returns>
        public Int32 CreateHeader(tb_HeaderText HeaderText)
        {
            Int32 isAdded = 0;
            try
            {
                HeaderDAC dac = new HeaderDAC();
                dac.Create(HeaderText);
                isAdded = HeaderText.HeaderID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }


        /// <summary>
        /// Get Topic  detail for Edit mode
        /// </summary>
        /// <param name="Id">Get Topic  detail for selected Id</param>
        /// <returns>Returns tb_Topic object</returns>
        public tb_HeaderText getHeaderByID(int Id)
        {
           HeaderDAC dac = new HeaderDAC();
           return dac.GetHeaderByID(Id);
        }
        /// <summary>
        /// Update Topic
        /// </summary>
        /// <param name="topic">tb_Topic topic</param>
        /// <returns>Returns Id of updated Topic</returns>
        public Int32 Update(tb_HeaderText topic)
        {
            int RowsAffected = 0;
            try
            {
                HeaderDAC dac = new HeaderDAC();
                RowsAffected = dac.Update(topic);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RowsAffected;
        }

        

        /// <summary>
        /// Check Topic name is already inserted or not
        /// </summary>
        /// <param name="custLevel">tb_Topic topic</param>
        /// <returns>returns true if found else false</returns>
        public bool CheckDuplicate(tb_Topic topic)
        {
            TopicDAC dac = new TopicDAC();
            if (dac.CheckDuplicate(topic) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }

  public class HeaderEntity
  {
      private int _HeaderID;
      private int _DisplayOrder;
      private string _HeaderText;
      private int _Active;



      public int HeaderID
      {
          get { return _HeaderID; }
          set { _HeaderID = value; }
      }
      public int DisplayOrder
      {
          get { return _DisplayOrder; }
          set { _DisplayOrder = value; }
      }
      public string HeaderText
      {
          get { return _HeaderText; }
          set { _HeaderText = value; }
      }
      public int Active
      {
          get { return _Active; }
          set { _Active = value; }
      }
    
  }
}
