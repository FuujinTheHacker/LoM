using System;

namespace Common.DatabaseObjects
{
    public class ProductAssociatedFile_has_TagTyp
    {
        public long ID { get; private set; }
        public long ProductAssociatedFile_ID { get; set; }
        public long TagTyp_ID { get; set; }
        public DateTime TS { get; set; }
    }
}
