using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;

namespace Solution.Bussines.Components
{
  public  class SubjectStatusComponent
    {/// <summary>
        /// <summary>
        /// Update Method for SubjectStatus
        /// </summary>
        /// <param name="SubjectStatus">tb_SubjectStatus SubjectStatus</param>
        /// <returns>Returns SubjectStatusID of currently updated record</returns>
        public Int32 UpdateSubjectStatus(tb_ContactEmail SubjectStatus)
        {
            Int32 isUpdated = 0;
            try
            {
                SubjectStatusDAC objSubjectStatus = new SubjectStatusDAC();
                if (objSubjectStatus.CheckDuplicate(SubjectStatus) == 0)
                {
                    UpdateCountries(SubjectStatus);
                    isUpdated = SubjectStatus.ID;
                }
                else
                {
                    isUpdated = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }
        /// <summary>
        /// Update Method for SubjectStatus
        /// </summary>
        /// <param name="SubjectStatus">tb_SubjectStatus SubjectStatus</param>
        /// <returns>Returns SubjectStatusID of currently updated record</returns>
        private tb_ContactEmail UpdateCountries(tb_ContactEmail SubjectStatus)
        {
            SubjectStatusDAC objSubjectStatus = new SubjectStatusDAC();
            objSubjectStatus.Update(SubjectStatus);
            return SubjectStatus;
        }
      /// <summary>
      ///  Get SubjectStatus Data By ID for Edit functionality
      /// </summary>
      /// <param name="Id">int Id</param>
      /// <returns>Return a SubjectStatus record for edit in table form</returns>
      public tb_ContactEmail getSubjectStatus(int Id)
      {
          SubjectStatusDAC objSubjectStatus = new SubjectStatusDAC();
          return objSubjectStatus.getSubjectStatus(Id);
          
      }
      /// <summary>
      /// To Create New SubjectStatus 
      /// </summary>
      /// <param name="SubjectStatus">tb_SubjectStatus SubjectStatus</param>
      /// <returns>Returns value of current inserted record</returns>
      public Int32 CreateSubjectStatus(tb_ContactEmail SubjectStatus)
      {
          Int32 isAdded = 0;
          try
          {
              SubjectStatusDAC objSubjectStatus = new SubjectStatusDAC();
              if (objSubjectStatus.CheckDuplicate(SubjectStatus) == 0)
              {
                  objSubjectStatus.Create(SubjectStatus);
                  isAdded = SubjectStatus.ID;
              }
              else
              {
                  isAdded = -1;
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }

          return isAdded;
      }
    }
  public class SubjectStatusEntity
  {
      private int _ID;
      private int _Deleted;
      private string _Subject;
      private string _EmailID;

      private int _Active;



      public int ID
      {
          get { return _ID; }
          set { _ID = value; }
      }
      public int Deleted
      {
          get { return _Deleted; }
          set { _Deleted = value; }
      }
      public string Subject
      {
          get { return _Subject; }
          set { _Subject = value; }
      }
      public int Active
      {
          get { return _Active; }
          set { _Active = value; }
      }
      public string EmailID
      {
          get { return _EmailID; }
          set { _EmailID = value; }
      }

  }
}
