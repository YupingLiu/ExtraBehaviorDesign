using ProtoBuf;
using System.Collections.Generic;

namespace ExtraBehaviorDesign.Runtime.Serialize
{  
    [ProtoContract]
    public class BTNameAndRTNameDic
    {
        public BTNameAndRTNameDic() { 
            this.bTNamRTNameDic = new Dictionary<string, string>();
        }

        [ProtoMember(1)]
        private Dictionary<string, string> bTNamRTNameDic;

        public Dictionary<string, string> BTNamRTNameDic
        {
            get { return bTNamRTNameDic; }
            set { bTNamRTNameDic = value; }
        }
    }
}
