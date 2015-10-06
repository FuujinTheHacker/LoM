using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace hemSida.Models
{
    public abstract class ModelBass
    {
        public static SqlConnection getCon()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Ljus_och_Miljo_AB_con_Str"].ConnectionString;
            return new SqlConnection(conStr);
        }
    }
}