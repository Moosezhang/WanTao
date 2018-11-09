using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEntity.Common
{
    [Serializable]
    public class DictionaryInfo
    {
        public string ItemId { get; set; }
        public string ItemKey { get; set; }
        public string ItemValue { get; set; }

        public string DictionaryKey { get; set; }

        public int DictionaryLevel { get; set; }
        public IList<DictionaryInfo> SubDic { get; set; }
    }
}
