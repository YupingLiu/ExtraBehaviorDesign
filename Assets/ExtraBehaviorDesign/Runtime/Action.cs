using ExtraBehaviorDesign.TestScripts;
using ProtoBuf;
using System;

namespace ExtraBehaviorDesign.Runtime.Tasks
{
    [ProtoContract]
    [ProtoInclude(1, "ExtraBehaviorDesign.Runtime.Tasks.RotateByY, Assembly-CSharp")]
    [ProtoInclude(2, typeof(CanISeePlayer))]
    [ProtoInclude(3, typeof(TranslatetoARandomPosition))]
    [ProtoInclude(4, typeof(RedAction))]
    [ProtoInclude(5, typeof(BlueAction))]
    [ProtoInclude(6, typeof(YellowAction))]
    public abstract class Action : Task
    {
    }
}
