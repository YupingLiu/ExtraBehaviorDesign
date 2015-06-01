using ExtraBehaviorDesign.Runtime.Tasks;
using ProtoBuf;
using UnityEngine;
namespace ExtraBehaviorDesign.TestScripts
{
    [ProtoContract]
    class BlueAction : Action
    {
        //public override void OnAwake()
        //{
        //    Debug.Log("OnAwake: This is SimpleAction2");
        //    base.OnAwake();
        //}

        public override void OnStart()
        {
            //Debug.Log("OnStart: This is SimpleAction2");
            //base.OnStart();
            Debug.Log("The Server Random the MiddleBTBlue behavior~");
            this.gameObject.renderer.material.color = Color.blue;
        }

        //public override TaskStatus OnUpdate()
        //{
        //    Debug.Log("OnUpdate: This is SimpleAction2");
        //    return TaskStatus.Success;
        //}

        //public override void OnEnd()
        //{
        //    Debug.Log("OnEnd: This is SimpleAction2");
        //    base.OnEnd();
        //}
    }
}
