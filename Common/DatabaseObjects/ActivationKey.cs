using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace Common.DatabaseObjects
{
    public class ActivationKey
    {
        public long ID { get; private set; }
        public ActivationKeyTyp Typ { get; set; }
        public long User_ID { get; set; }
        public string Key { get; set; }
        public DateTime TS { get; set; }

        public ActivationKey(long aUserID)
        {
            Key = GenerateKey();
            Typ = 0;
            User_ID = aUserID;

            //AddKeyToDatabase(aConnection, aTransaction);
        }

        private ActivationKey(IDataRecord someData)
        {
            ID = someData.GetInt64(0);
            Typ = (ActivationKeyTyp)someData.GetInt32(1);
            User_ID = someData.GetInt64(2);
            Key = someData.GetString(3);
            TS = someData.GetDateTime(4);
        }

        public string GenerateKey()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, 100).Select(s => s[random.Next(s.Length)]).ToArray());

            return(result);
        }

        private void AddKeyToDatabase(SqlConnection aConnection, SqlTransaction aTransaction)
        {
            if (aConnection.State != ConnectionState.Open)
            {
                aConnection.Open();
            }

            SqlCommand command = new SqlCommand("INSERT INTO [dbo].[ActivationKey](Typ,User_ID,Key) VALUES (@Typ,@User_ID,@Key);", aConnection, aTransaction);

            //command.Parameters.Add(new SqlParameter("@Typ", Typ));
            //command.Parameters.Add(new SqlParameter("@User_ID", User_ID));
            //command.Parameters.Add(new SqlParameter("@Key", Key));

            command.Parameters.AddWithValue("Typ", Typ);
            command.Parameters.AddWithValue("User_ID", User_ID);
            command.Parameters.AddWithValue("Key", Key);

          //  command.ExecuteNonQuery();

            //aTransaction.Commit();
        }

        public static ActivationKey create(SqlConnection aConnection, Char[] aKey)
        {
            SqlCommand command = new SqlCommand("INSERT INTO [dbo].[ActivationKey](Key) OUTPUT INSERTED.Id VALUES(@placeholderKey)", aConnection);
            command.Parameters.Add(new SqlParameter("@placeholderKey", aKey));

            if (aConnection.State != ConnectionState.Open)
            {
                aConnection.Open();
            }

            long id = (long)command.ExecuteScalar();

            return(getActivationKey(aConnection, id));
        }

        public static ActivationKey getActivationKey(SqlConnection aConnection, long anID)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[ActivationKey] WHERE Id = @ID;", aConnection);
            command.Parameters.Add(new SqlParameter("@ID", anID));

            if (aConnection.State != ConnectionState.Open)
            {
                aConnection.Open();
            }

            SqlDataReader sqlDataReader = command.ExecuteReader();

            if (sqlDataReader.Read())
            { 
                return(new ActivationKey(sqlDataReader));
            }

            return (null);
        }

    }

    public enum ActivationKeyTyp : int
    {
        UserActivation,
        PasswordRecovery
    }

}
