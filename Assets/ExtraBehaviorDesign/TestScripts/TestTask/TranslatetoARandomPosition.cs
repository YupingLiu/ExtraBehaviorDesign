using ProtoBuf;
using UnityEngine;
namespace ExtraBehaviorDesign.Runtime.Tasks
{
    [ProtoContract]
    class TranslatetoARandomPosition : Action
    {
        public NavMeshAgent m_agent;
        private Transform m_trans;
        private Vector3 m_randomPos;
        //private TestGetComponent m_tgp;

        public override void OnAwake()
        {
            m_agent = this.GetComponent<NavMeshAgent>();
            //m_tgp = this.GetComponent<TestGetComponent>();
            m_trans = this.transform;
        }

        public override void OnStart()
        {
            //Debug.LogError("m_agent " + m_agent);
            //Debug.LogError("m_agent.destination " + m_agent.destination);
            //Debug.LogError("m_agent.enabled " + m_agent.enabled);
            m_agent.enabled = true;

            m_randomPos = m_trans.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));
            //Debug.LogError("m_randomPos = " + m_randomPos);
            m_agent.destination = m_randomPos;
        }

        public override TaskStatus OnUpdate()
        {
            if (m_agent.remainingDistance <= 1.0f)
            {
                m_agent.Stop();
                return TaskStatus.Success;
            }
            //if (m_tgp.PrintTest())
            //{
            //    return TaskStatus.Success;
            //}
            return TaskStatus.Running;
        }
    }
}
