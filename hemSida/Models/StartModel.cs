using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace hemSida.Models
{
    public class StartModel : ModelBass
    {
        public List<string> results;

        public StartModel ()
        {
            results = new List<string>();
            using (SqlConnection con = getCon())
            {
                using (SqlCommand comad = new SqlCommand("SELECT DisplayName FROM [dbo].[TagTyp];", con))
                {
                    con.Open();
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