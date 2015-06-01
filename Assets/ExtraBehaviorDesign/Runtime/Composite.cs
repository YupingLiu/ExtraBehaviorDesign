using ProtoBuf;
using ProtoBuf.Meta;
using System;
using UnityEngine;
namespace ExtraBehaviorDesign.Runtime.Tasks
{
    [ProtoContract]
    [ProtoInclude(1, typeof(Selector))]
    [ProtoInclude(2, typeof(Sequence))]
    [ProtoInclude(3, typeof(Parallel))]
    [ProtoInclude(4, typeof(ParallelSelector))]
    [Serializable]
    public abstract class Composite : ParentTask
    {
        //[SerializeField]
        [Tooltip("Specifies the type of conditional abort.")]
        protected AbortTpye abortType;

        protected AbortTpye AbortType
        {
          get { return abortType; }
        }

        protected Composite()
        {
        }
        
    }
}
