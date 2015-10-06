using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace hemSida.Models
{
    public class ProductModel : ModelBass
    {
        public bool hitad = false;

        public long ID;
        public string DisplayName;
        public string ProductID;
        public string ProductDesc;
        public double ProductPrice;

        public List<string> results;

        public ProductModel(string name)
        {
            using (SqlConnection con = getCon())
            {
                using (SqlCommand comad = new SqlCommand("", con))
                {
                    comad.CommandText = "SELECT ID, DisplayName, ProductDesc, ProductPrice FROM [dbo].[Product] WHERE DisplayName = @name ;";
                    comad.Parameters.AddWithValue("name", name);
                    comad.Connection.Open();
                    using (SqlDataReader sqlr = comad.ExecuteReader())
                    {
                        if (sqlr.Read())
                        {
                            ID = sqlr.GetInt64(0);
                            DisplayName = sqlr.GetString(1);
                            ProductDesc = sqlr.GetString(2);
                            ProductPrice = sqlr.GetDouble(3);
                            hitad = true;
                        }
                    }
                }
                if (hitad)
                    using (SqlCommand comad = new SqlCommand(
                            "SELECT j2.DisplayName FROM [dbo].[Product_has_TagTyp] AS j1 " +
                            "RIGHT OUTER JOIN [dbo].[TagTyp] AS j2 ON j1.[TagTyp_ID] = j2.[ID] " +
                            "WHERE [Product_ID] = @TID ;", con))
                    {
                        comad.Parameters.AddWithValue("TID", ID);
                        results = new List<string>();
                        using (SqlDataReader sqlr = comad.ExecuteReader())
                        {
                            while (sqlr.Read())
                            {
                                results.Add(sqlr.GetString(0));
                            }
                        }
                    }
            }
        }

    }
}