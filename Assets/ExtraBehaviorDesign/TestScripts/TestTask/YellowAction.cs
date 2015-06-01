using ExtraBehaviorDesign.Runtime.Tasks;
using ProtoBuf;
using UnityEngine;
namespace ExtraBehaviorDesign.TestScripts
{
    [ProtoContract]
    public class YellowAction : Action
    {
        public override void OnStart()
        {
            Debug.Log("The Server Random the MiddleBTYellow behavior~");
            this.gameObject.renderer.material.color = Color.yellow;
        }
    }
}
