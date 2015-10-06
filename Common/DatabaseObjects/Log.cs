using System;
using System.Net;

namespace Common.DatabaseObjects
{
    public class Log 
    {
        public long ID { get; private set; }
        //public User User_ID { get; set; }
        public IPAddress IP { get; set; }
        public DateTime TS;
    }
}
