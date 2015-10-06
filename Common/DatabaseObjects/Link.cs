using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DatabaseObjects
{
    public class Link
    {
        public long ID { get; private set; }
        public long Product_ID { get; set; }
        public long ProductAssociatedFile_ID { get; set; }
        public string Key { get; set; }
        public DateTime ST;
    }
}
