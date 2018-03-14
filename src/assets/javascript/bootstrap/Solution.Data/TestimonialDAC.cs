using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Solution.Bussines.Entities;

namespace Solution.Data
{
    /// <summary>
    /// Testimonial Component Class Contains Testimonial related Data Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 
    public class TestimonialDAC
    {
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

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
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Rating_Testimonials";
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@City", City);
            cmd.Parameters.AddWithValue("@Country", Country);
            cmd.Parameters.AddWithValue("@Comments", Comments);
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            cmd.Parameters.AddWithValue("@Rating", Rating);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@IPaddress", IPaddress);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

       
        /// <summary>
        /// Get Testimonials
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the Testimonials List as a Dataset</returns>
        public DataSet GetTestimonials(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT [TestimonialsID],[StoreID],[Name],[Message],[Approved],[CreatedOn],Isnull(Rating,0) as Rating ,[City]," +
                              "[Country],Isnull(YesCount,0) as YesCount ,Isnull(NoCount,0) as NoCount  FROM tb_Testimonials where isnull(Approved,0)=1 and StoreID=" + StoreID + " order by CreatedOn desc";
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Update Yes Count
        /// </summary>
        /// <param name="TestimonialID">Int32 TestimonialID</param>
        public void UpdateYesCount(Int32 TestimonialID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "update tb_Testimonials set YesCount=isnull(YesCount,0)+1 where TestimonialsID=" + TestimonialID;
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Update No Count
        /// </summary>
        /// <param name="TestimonialID">Int32 TestimonialID</param>
        public void UpdateNoCount(Int32 TestimonialID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "update tb_Testimonials set NoCount=isnull(NoCount,0)+1 where TestimonialsID=" + TestimonialID;
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Get the Testimonials Details
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns the Testimonial table object which contains Retrieved record</returns>
        public tb_Testimonials GetTestimonialDetail(int Id)
        {
            tb_Testimonials rating = null;
            {
                rating = ctx.tb_Testimonials.First(e => e.TestimonialsID == Id);
            }
            return rating;
        }

        /// <summary>
        /// Update Testimonial Review
        /// </summary>
        /// <param name="tb_Testimonial">tb_Testimonials tb_Testimonial</param>
        public void UpdateTestimonialReview(tb_Testimonials tb_Testimonial)
        {
            ctx.SaveChanges();
        }
    }
}
