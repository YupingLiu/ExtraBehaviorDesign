using UnityEngine;
using System.Collections;
using ProtoBuf;
using ProtoBuf.Meta;
using System;

namespace ExtraBehaviorDesign.Runtime.Tasks
{
    [ProtoContract]
    [ProtoInclude(1, "ExtraBehaviorDesign.Runtime.Tasks.ParentTask, Assembly-CSharp")]
    [ProtoInclude(2, "ExtraBehaviorDesign.Runtime.Tasks.Action, Assembly-CSharp")]
    [Serializable]
    public abstract class Task : ScriptableObject
    {
        protected Task() {
            NodeData = new NodeData();
        }

        public enum RemoveType
        { 
            ONLYWHENSUCCESS = 0,
            WHENNOTRUNNING = 1,
            NEVER = 2,
        }

        protected GameObject gameObject;
        protected Transform transform;

        public Animation Animation { set{}}
        public AudioSource Audio { set { } }
        public Camera Camera { set { } }
        public Collider Collider { set { } }
        public GameObject GameObject { set { this.gameObject = value; } }

        public NodeData NodeData { get; set; }
        public Behavior Owner { get; set; }

        public Rigidbody Rigidbody { set { } }
        public Rigidbody2D Rigidbody2D { set { } }
        public Transform Transform { set { this.transform = value; } }

        public virtual float GetPriority() { return 0; }
        public virtual void OnAwake() { }
        public virtual void OnBehaviorComplete() { }
        public virtual void OnEnd() { }
        public virtual void OnPause(bool paused) { }
        public virtual void OnReset() { }
        public virtual void OnStart() { }
        public virtual TaskStatus OnUpdate() { return TaskStatus.Success; }

        public RemoveType removeType = RemoveType.NEVER;

        /// <summary>
        /// 节点在Pop的时候（执行成功或失败），判断是否要从行为树中移除自己
        /// </summary>
        /// <returns></returns>
        public virtual bool RemoveWhenFinish() 
        {
            bool result = false;
            switch (removeType)
            {
                case RemoveType.ONLYWHENSUCCESS:
                    if (TaskStatus.Success == NodeData.ExecutionStatus)
                    {
                        result = true;
                    }
                    break;
                case RemoveType.WHENNOTRUNNING:
                    if (TaskStatus.Running != NodeData.ExecutionStatus)
                    {
                        result = true;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        // 易忘点
        protected T GetComponent<T>() where T : Component
        {
            return this.gameObject.GetComponent<T>();
        }
    }
}
