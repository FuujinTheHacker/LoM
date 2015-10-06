using System;


namespace Common.DatabaseObjects
{
    public class Product_has_TagTyp
    {
        public long ID { get; private set; }
        public long Product_ID { get; set; }
        public long TagTyp_ID { get; set; }
        public DateTime TS { get; set; }
    }
}
