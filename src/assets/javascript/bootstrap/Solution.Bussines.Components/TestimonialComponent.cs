using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Data;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Testimonial Component Class Contains Testimonial related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class TestimonialComponent
    {
        TestimonialDAC Testimonialdac = new TestimonialDAC();
        List<TestimonialEntity> lstproduct = new List<TestimonialEntity>();
        public static int _count;

        /// <summary>
        /// Add Or Update Testimonials Rating
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <param name="City">String City</param>
        /// <param name="Country">String Country</param>
        /// <param name="Comments">String Comments</param>
        /// <param name="EmailID">String EmailID</param>
        /// <param name="Rating">Int32 Rating</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="IPaddress">String IPaddress</param>
        /// <returns></returns>
        public string AddOrUpdateTestimonialsRating(String Name, String City, String Country, String Comments, String EmailID, Int32 Rating, Int32 StoreID, String IPaddress)
        {
            TestimonialDAC Testimonialdac = new TestimonialDAC();
            return Testimonialdac.AddOrUpdateTestimonialsRating(Name, City, Country, Comments, EmailID, Rating, StoreID, IPaddress);
        }

        /// <summary>
        /// Retrieves the Testimonials List for displaying into grid
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="CName"string CName></param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchBy">string pSearchBy</param>
        /// <returns>Returns the Testimonials List for displaying into grid</returns>
        public IQueryable<TestimonialEntity> GetDataByFilterForTestimonials(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchBy)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            IQueryable<TestimonialEntity> results = from a in ctx.tb_Testimonials
                                                    select new TestimonialEntity
                                                                         {
                                                                             TestimonialID = a.TestimonialsID,
                                                                             CreatedOn = a.CreatedOn.Value,
                                                                             Comments = a.Message,
                                                                             IsApproved = a.Approved.Value,
                                                                             Rating = (a.Rating ?? 0),
                                                                             Name = a.Name,
                                                                             City = (a.City ?? ""),
                                                                             StoreID = a.StoreID.Value,
                                                                             Country = (a.Country ?? ""),
                                                                             Email = a.EmailID
                                                                         };


            if (pStoreId != 0)
            {
                if (pSearchBy == "1")
                {
                    results = results.Where(a => a.IsApproved == 1 && a.StoreID == pStoreId).AsQueryable();
                }
                else if (pSearchBy == "0")
                {
                    results = results.Where(a => a.IsApproved == 0 && a.StoreID == pStoreId).AsQueryable();
                }
                else if (pSearchBy == "-1")
                {
                    results = results.Where(a => a.IsApproved == -1 && a.StoreID == pStoreId).AsQueryable();
                }
                else
                {
                    results = results.Where(a => a.IsApproved == 1 && a.StoreID == pStoreId).AsQueryable();
                }
            }
            else
            {
                if (pSearchBy == "1")
                {
                    results = results.Where(a => a.IsApproved == 1).AsQueryable();
                }
                else if (pSearchBy == "0")
                {
                    results = results.Where(a => a.IsApproved == 0).AsQueryable();
                }
                else if (pSearchBy == "-1")
                {
                    results = results.Where(a => a.IsApproved == -1).AsQueryable();
                }
                else
                {
                    results = results.AsQueryable();
                }
            }
            _count = results.Count();
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = lstproduct.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    results = results.OrderByField(SortingOption[0].ToString(), true);
                }
                else if (SortingOption.Length == 2)
                {
                    results = results.OrderByField(SortingOption[0].ToString(), false);
                }
            }
            else
            {
                results = results.OrderBy(o => o.Name);
            }
            results = results.Skip(startIndex).Take(pageSize);
            return results;
        }

        /// <summary>
        /// Get Count of Records
        /// </summary>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchBy">string pSearchBy</param>
        /// <returns>Returns the count of record</returns>
        public static int GetCount(string CName, int pStoreId, string pSearchBy)
        {
            return _count;
        }

        /// <summary>
        /// Get the Testimonials Details
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns the Testimonial table object which contains Retrieved record</returns>
        public tb_Testimonials GetTestimonialDetail(int Id)
        {
            return Testimonialdac.GetTestimonialDetail(Id);
        }

        /// <summary>
        /// Update Testimonial Review
        /// </summary>
        /// <param name="tb_Testimonial">tb_Testimonials tb_Testimonial</param>
        public void UpdateTestimonialReview(tb_Testimonials tb_Testimonial)
        {
            Testimonialdac.UpdateTestimonialReview(tb_Testimonial);
        }

        /// <summary>
        /// Get Testimonials
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the Testimonials List as a Dataset</returns>
        public static DataSet GetTestimonials(Int32 StoreId)
        {
            TestimonialDAC Testimonialdac = new TestimonialDAC();
            return Testimonialdac.GetTestimonials(StoreId);
        }

        /// <summary>
        /// Update Yes Count
        /// </summary>
        /// <param name="TestimonialID">Int32 TestimonialID</param>
        public void UpdateYesCount(Int32 TestimonialID)
        {
            TestimonialDAC Testimonialdac = new TestimonialDAC();
            Testimonialdac.UpdateYesCount(TestimonialID);
        }

        /// <summary>
        /// Update No Count
        /// </summary>
        /// <param name="TestimonialID">Int32 TestimonialID</param>
        public void UpdateNoCount(Int32 TestimonialID)
        {
            TestimonialDAC Testimonialdac = new TestimonialDAC();
            Testimonialdac.UpdateNoCount(TestimonialID);
        }
    }

    public class TestimonialEntity
    {
        private int _TestimonialID;

        public int TestimonialID
        {
            get { return _TestimonialID; }
            set { _TestimonialID = value; }
        }

        private int _StoreID;

        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }

        private int _Rating;

        public int Rating
        {
            get { return _Rating; }
            set { _Rating = value; }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _StoreName;

        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }

        private string _City;

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        private string _Country;

        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private Int32 _IsApproved;

        public Int32 IsApproved
        {
            get { return _IsApproved; }
            set { _IsApproved = value; }
        }

        private DateTime _ApprovedDate;

        public DateTime ApprovedDate
        {
            get { return _ApprovedDate; }
            set { _ApprovedDate = value; }
        }

        private DateTime _CreatedOn;

        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
    }
}
