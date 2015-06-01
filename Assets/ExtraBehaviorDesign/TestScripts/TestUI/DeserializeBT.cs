using ExtraBehaviorDesign.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ExtraBehaviorDesign.TestScripts
{
    public class DeserializeBT : MonoBehaviour
    {
        public List<GameObject> m_enemyObjList;
        private List<Behavior> m_enemyBehaviorList = new List<Behavior>();

        public void Start()
        {
            // Get Behavior Component List
            for (int i = 0; i < m_enemyObjList.Count; i++)
            {
                m_enemyBehaviorList.Add(m_enemyObjList[i].GetComponent<BehaviorTree>());
            }
        }

        // add delegate —— Extend Button
        public void OnExtendBT()
        {
            for (int i = 0; i < m_enemyBehaviorList.Count; i++)
            {
                m_enemyBehaviorList[i].AddExtendBehaviorTreeHandler(ExtendBT);
            }
        }

        // event handler
        private void ExtendBT(BehaviorManager.BehaviorTree behaviorTree)
        {
            ExtendEffect ef = ScriptableObject.CreateInstance<ExtendEffect>();
            if (behaviorTree.AddTask(1, 3, ef))
            {
                if (null == UIBTInformationNotifier.instance)
                {
                    UIBTInformationNotifier notifier = new UIBTInformationNotifier();
                }
                UIBTInformationNotifier.instance.NotifyExtensionInfoChangeEvent(ef.ToString());
                Debug.Log("DeserializeBT.OnExtendBT : Extend Success");
            }
        }

        /// <summary>
        /// deserialize bt asset —— Deserialize Button
        /// <para>The Server must tell the client the path of AIAsset</para>
        /// </summary>
        /// <param name="serializationFilePath"></param>
        public void OnDeserializeAssetToBehavior(string serializationFilePath)
        {
            if ("" == serializationFilePath)
	        {
#if UNITY_EDITOR
                Debug.LogWarning("DeserializeBT OnDeserializeBSToBehavior ERROR ： the Serialization File Path is empty");
                return ;
#endif
	        }

            // Get BT Asset
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(serializationFilePath, typeof(TextAsset)) as TextAsset;
            
            // Attach BT Asset to Behavior Component (if has no, AddComponent)
            for (int i = 0; i < m_enemyBehaviorList.Count; i++)
            {
                if (null == m_enemyBehaviorList[i])
                {
                    m_enemyBehaviorList[i] = m_enemyObjList[i].AddComponent<BehaviorTree>();
                    m_enemyBehaviorList[i].StartWhenEnabled = false;
                }
                else
                {
                    continue;
                }
                m_enemyBehaviorList[i].AIAsset = textAsset;
            }    
        }

        // start to run —— Run Button
        public void OnStartRun()
        {
            for (int i = 0; i < m_enemyBehaviorList.Count; i++)
            {
                m_enemyBehaviorList[i].StartWhenEnabled = true;
                m_enemyBehaviorList[i].RestartWhenComplete = true;
                if (!m_enemyObjList[i].activeSelf)
                {
                    m_enemyObjList[i].SetActive(true);
                }
                else 
                {
                    m_enemyBehaviorList[i].OnEnable();
                }
            }
        }

        // remove delegate
        public void OnDestroy()
        {
            for (int i = 0; i < m_enemyBehaviorList.Count; i++)
            {
                m_enemyBehaviorList[i].RemoveExtendBehaviorTreeHandler(ExtendBT);
            }
        }
    }
}
