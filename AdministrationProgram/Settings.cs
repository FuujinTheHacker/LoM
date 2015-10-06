using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AdministrationProgram
{
    [Serializable]
    public class Settings
    {
        public bool firstStart { get; set; }

        public string SqlConString { get; set; }

        public string myEmailAdress { get; set; }
        public string myEmailPassword { get; set; }
        public string mySMTPAdress { get; set; }
        public string myDnsName { get; set; }

        public Settings()
        {
            firstStart = true;
            SqlConString = "";

            myEmailAdress = "ljusochmiljo@gmail.com";
            myEmailPassword = "projectlight";
            mySMTPAdress = "smtp.gmail.com";
            myDnsName = "localhost";
        }

        public Settings clone ()
        {
            Settings back = new Settings();

            back.firstStart = firstStart;
            back.SqlConString = SqlConString;

            return back;
        }

        public static void Save(string file ,Settings s)
        {
            FileStream fs = new FileStream(file, FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, s);
                fs.Flush();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        public static Settings load(string file)
        {
            if (!File.Exists(file))
                return null;

            Settings back;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(file, FileMode.Open);
            try
            {
                back = (Settings)formatter.Deserialize(fs);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                fs.Close();
            }
            return back;
        }

    }
}
