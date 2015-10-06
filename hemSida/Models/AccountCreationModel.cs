using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace hemSida.Models
{
    public class AccountCreationModel : ModelBass
    {
        public string myUsername { get; set; }
        public long myUserID { get; set; }
        public string myOrgName { get; set; }
        public string myPhoneNumber { get; set; }
        public string myEmail { get; set; }
        public string myActivationKey { get; set; }
        public int mySalt { get; set; }

        public ActivationResult myResult { get; set; }

        public AccountCreationModel(string anAccountActivationKey)
        {
            myActivationKey = anAccountActivationKey;

            using (SqlConnection con = getCon())
            {
                if (anAccountActivationKey != null && FindUser(con, anAccountActivationKey) && LoadUser(con) )
                {
                    myResult = ActivationResult.FOUND;
                }
                else
                {
                    myResult = ActivationResult.NOTFOUND;
                }
            }
        }

        public AccountCreationModel(string anActivationKey, string aName, string aPassword, string aPassword2)
        {
            myActivationKey = anActivationKey;

            using (SqlConnection con = getCon())
            {
                if (FindUser(con, anActivationKey) && LoadUser(con) && UpdateUser(con, aName, aPassword, aPassword2))
                {
                    myResult = ActivationResult.ACTIVATIONSUCCESS;
                }
                else
                {
                    myResult = ActivationResult.ACTIVATIONFAILED;
                }
            }
        }

        private bool FindUser(SqlConnection aConnection, string anAccountActivationKey)
        {
            bool returnFalg = false;

            using (SqlCommand sqlCommand = new SqlCommand("", aConnection))
            {
                sqlCommand.CommandText = "SELECT [Key], [User_ID] FROM [dbo].[ActivationKey] WHERE [Key] = @anAccountActivationKey ;";
                sqlCommand.Parameters.AddWithValue("anAccountActivationKey", anAccountActivationKey);

                if (sqlCommand.Connection.State != ConnectionState.Open)
                {
                    sqlCommand.Connection.Open();
                }

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (returnFalg = sqlDataReader.Read())
                    {
                        myUserID = sqlDataReader.GetInt64(1);
                    }
                }
            }
            return(returnFalg);
        }

        private bool LoadUser(SqlConnection aConnection)
        {
            bool returnFlag = false;

            using (SqlCommand sqlCommand = new SqlCommand("", aConnection))
            {
                sqlCommand.CommandText = "SELECT ID, OrgName, Tele, Epost, Salt FROM [dbo].[User] WHERE ID = @myUserID ;";
                sqlCommand.Parameters.AddWithValue("myUserID", myUserID);

                if (sqlCommand.Connection.State != ConnectionState.Open)
                {
                    sqlCommand.Connection.Open();
                }

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (returnFlag = sqlDataReader.Read())
                    {
                        myOrgName = sqlDataReader.GetString(1);
                        myPhoneNumber = sqlDataReader.GetString(2);
                        myEmail = sqlDataReader.GetString(3);
                        mySalt = sqlDataReader.GetInt32(4);
                    }
                }
            }
            return(returnFlag);
        }

        private bool UpdateUser(SqlConnection aConnection, string aUsername, string aPassword, string aPassword2)
        {
            bool returnFlag = false;

            if (aPassword == aPassword2 && aPassword != "")
            {
                using (SqlCommand sqlCommand = new SqlCommand("", aConnection))
                {
                    sqlCommand.CommandText = "UPDATE [dbo].[User] SET UserName = @aUsername, Pw = @aPassword, Activated = @ActivatedFlag WHERE ID = @ID ;";
                    sqlCommand.Parameters.AddWithValue("ID", myUserID);
                    sqlCommand.Parameters.AddWithValue("aUsername", aUsername);

                    byte[] hashedPassword = Common.UserLoginHelper.ComputeHash(aPassword, mySalt);
                    sqlCommand.Parameters.AddWithValue("aPassword", hashedPassword);

                    sqlCommand.Parameters.AddWithValue("ActivatedFlag", 1);

                    if (sqlCommand.Connection.State != ConnectionState.Open)
                    {
                        sqlCommand.Connection.Open();
                    }

                    int result = sqlCommand.ExecuteNonQuery();

                    returnFlag = (result == 1);
                }
            }
            else
            {
                returnFlag = false;
            }

            return (returnFlag);
        }
    }

    public enum ActivationResult
    { 
        FOUND,
        NOTFOUND,
        ACTIVATIONSUCCESS,
        ACTIVATIONFAILED
    }
}