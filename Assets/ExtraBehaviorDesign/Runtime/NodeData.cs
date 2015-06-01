using ExtraBehaviorDesign.Runtime.Tasks;
using System;
using UnityEngine;
namespace ExtraBehaviorDesign.Runtime
{
    [Serializable]
    public class NodeData
    {
        [SerializeField]
        private Vector2 offset;

        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        public NodeData(){
            ExecutionStatus = TaskStatus.Inactive;
        }
        public TaskStatus ExecutionStatus { get; set; }
    }
}
