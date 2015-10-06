using System;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Text;
using Common.DatabaseObjects;
using System.Linq;

namespace Common
{
    public static class UserLoginHelper
    {
        public static byte[] ComputeHash(string pw, int salt)
        {
            pw = salt + pw + salt + pw + salt;

            SHA512 sha = SHA512.Create();
            byte[] back = sha.ComputeHash(ASCIIEncoding.UTF8.GetBytes(pw));

            back = sha.ComputeHash(back);

            back = sha.ComputeHash(back);

            back = sha.ComputeHash(back);
            sha.Dispose();

            return back;
        }

        public static bool Login(SqlConnection con, string aUsername, string aPassword, ref long Id)
        {
            if (aUsername == null || aPassword == null || aUsername == "" || aPassword == "")
                return false;

            User user = User.getUser(con, aUsername);

            if (user != null && user.ActivatedFlag)
            {
                byte[] testPw = ComputeHash(aPassword, user.Salt);
                if (testPw.SequenceEqual(user.Password))
                {
                    Id = user.Id;
                    return true;
                }
            }
            return false;
        }
    }
}
