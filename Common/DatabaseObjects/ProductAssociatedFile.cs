using System;


namespace Common.DatabaseObjects
{
    public class ProductAssociatedFile
    {
        public long ID { get; private set; }
        public string DisplayName { get; set; }
        public long Product_ID { get; set; }
        public string FilePath { get; set; }
        public DateTime TS { get; set; }
    }
}
