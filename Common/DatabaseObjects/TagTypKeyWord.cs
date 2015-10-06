using System;

namespace Common.DatabaseObjects
{
    public class TagTypKeyWord
    {
        public long ID { get; private set; }
        public long TagTyp_ID { get; set; }
        public string Key { get; set; }
        public DateTime TS { get; set; }
    }
}
