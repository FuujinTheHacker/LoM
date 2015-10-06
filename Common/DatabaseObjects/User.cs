using System;
using System.Text;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;

namespace Common.DatabaseObjects
{
    class User
    {
        public long Id;
        public string Username;
        public bool ActivatedFlag;
        public byte[] Password;
        public int Salt;

        public User(IDataRecord data)
        {
            Id = data.GetInt64(0);
            Username = data.GetString(1);
            ActivatedFlag = data.GetBoolean(2);

            Password = new byte[64];
            data.GetBytes(3, 0, Password, 0, 64);

            Salt = data.GetInt32(4);
        }

        public static User getUser(SqlConnection con, string aUsername)
        {
            SqlCommand comad = new SqlCommand("SELECT ID, UserName, Activated, Pw, Salt  FROM [dbo].[User] WHERE UserName = @nameParam;", con);
            comad.Parameters.Add(new SqlParameter("nameParam", aUsername));
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            SqlDataReader sqlr = comad.ExecuteReader();

            if (sqlr.Read())
            {
                return(new User(sqlr));
            }

            return(null);
        }
    }

}
