using ProtoBuf;
using System.Collections.Generic;
namespace ExtraBehaviorDesign.Runtime.Tasks
{
    [ProtoContract]
    [ProtoInclude(1, typeof(UntilSuccess))]
    public class Decorator : ParentTask
    {
        public Decorator()
        { }

        public override int MaxChildren()
        {
            return 1;
        }
    }
}
