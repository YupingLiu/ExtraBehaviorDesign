using ProtoBuf;
using UnityEngine;
namespace ExtraBehaviorDesign.Runtime.Tasks
{
    //[ProtoContract]
    class RotateByY : Action
    {
        public float m_rotateSpeed = 1.0f;
        public float m_rotateAngle = 0.0f;
        private Transform m_trans;
        private Vector3 m_forward;

        public override void OnStart()
        {
            m_trans = this.transform;
            m_forward = m_trans.forward;
        }

        public override TaskStatus OnUpdate()
        {
            m_trans.Rotate(new Vector3(0, m_rotateSpeed, 0));
            m_rotateAngle = Vector3.Angle(m_forward, m_trans.forward);
            if (m_rotateAngle > 45)
	        {
		        return TaskStatus.Success;
	        }
            return TaskStatus.Running;
        }
    }
}
