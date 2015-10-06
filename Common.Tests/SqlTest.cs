using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace Common.Tests
{
    [TestClass]
    public class SqlTest
    {
        public static string constr = "Server=tcp:ljus-och-miljo-ab-sql-server.database.windows.net,1433;Database=Ljus_och_Miljo_DB;User ID=LaAdmin@ljus-och-miljo-ab-sql-server;Password=1mgtA45m4pkKK34HHIwfAW;Trusted_Connection=False;Encrypt=True;";

        public static SqlConnection getCon() {
            return new SqlConnection(constr);
        }

        [TestMethod]
        public void ConTest()
        {
            SqlConnection con = getCon();
            con.Open();
            con.Close();
            con.Dispose();
        }
    }
}
