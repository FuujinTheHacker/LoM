using System;
using System.Data;
using System.Data.SqlClient;

namespace Common.DatabaseObjects
{
    public class Product
    {
        public long ID { get; private set; }
        public String DisplayName { get; set; }       
        public DateTime TS;

        public static Product getProduct(SqlConnection con, long aId)
        {
            SqlCommand comad = new SqlCommand("SELECT * FROM [dbo].[Product] WHERE Id = @ID;", con);
            comad.Parameters.Add(new SqlParameter("@ID", aId));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            SqlDataReader sqlr = comad.ExecuteReader();

            if (sqlr.Read())
            {
                return new Product(sqlr);
            }

            return(null);
        }

        private Product(IDataRecord data)
        {
            ID = data.GetInt64(0);
            DisplayName = data.GetString(1);
            TS = data.GetDateTime(2);
        }

        public static Product create(SqlConnection con, string aName)
        {
            SqlCommand comad = new SqlCommand("INSERT INTO [dbo].[Product](DisplayName) OUTPUT INSERTED.Id VALUES(@placeHolderName)", con);
            comad.Parameters.Add(new SqlParameter("@placeHolderName", aName));

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            long id = (long)comad.ExecuteScalar();

            return(getProduct(con, id));
        }

    }
}
