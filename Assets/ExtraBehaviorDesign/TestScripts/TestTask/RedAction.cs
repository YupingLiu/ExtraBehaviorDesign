using ExtraBehaviorDesign.Runtime.Tasks;
using ProtoBuf;
using UnityEngine;

namespace ExtraBehaviorDesign.TestScripts
{
    [ProtoContract]
    public class RedAction : Action
    {
        public override void OnStart()
        {
            Debug.Log("The Server Random the MiddleBTRed behavior~");
            this.gameObject.renderer.material.color = Color.red;
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
