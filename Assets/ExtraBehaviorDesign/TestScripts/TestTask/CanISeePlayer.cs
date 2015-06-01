using ProtoBuf;
using UnityEngine;
namespace ExtraBehaviorDesign.Runtime.Tasks
{
    [ProtoContract]
    public class CanISeePlayer : Action
    {
        private Transform m_playerTrans;
        private Transform m_trans;
        private bool ifCanSee = false;

        public override void OnAwake()
        {
            m_trans = this.transform;
            m_playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void OnStart()
        {
            var direction = m_playerTrans.position - m_trans.position;
            var ray = new Ray(m_trans.position, m_playerTrans.position);
            float viewAngle = Vector3.Angle(m_trans.forward, direction);
            //Debug.Log("ViewAngle is : " + viewAngle);
            bool IsInView = viewAngle < 45;
            //Debug.LogWarning("IsInView : " + IsInView);
            Debug.DrawLine(m_trans.position, m_playerTrans.position, Color.yellow);
            //int layerMask = 1 << 8;
            if (IsInView)
            {
                RaycastHit hit;
                Debug.DrawRay(m_trans.position, direction, Color.green);
                if (Physics.Raycast(m_trans.position, direction, out hit, 1000))
                {
                    //Debug.LogWarning("hit.collider.transform" + hit.collider.transform);
                    ifCanSee = (hit.collider.transform == m_playerTrans);
                    hit.collider.transform.renderer.material.color = Color.green;
                    //Debug.LogError("ifCanSee = " + ifCanSee);
                }
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (ifCanSee)
            {
                ifCanSee = false;
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
