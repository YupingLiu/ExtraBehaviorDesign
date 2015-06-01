using ExtraBehaviorDesign.Runtime;
using ExtraBehaviorDesign.Runtime.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ExtraBehaviorDesign.Editor.Tools
{
    /// <summary>
    /// 静态序列化行为树结构类型（BehaviorManager.BehaviorTree）
    /// </summary>
    class SerializeBehaviorTreeWizard : ScriptableWizard
    {
        public List<TextAsset> m_behaviorTreesList = new List<TextAsset>();
        private string[] searchInFolders = new string[] { "Assets" };
        private string className;

        [MenuItem("Edit/Extra Behavior Design/SerializeBehaviorTree...")]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard("Serialize BehaviorTree", typeof(SerializeBehaviorTreeWizard), "SerializeAll", "Serialize");
        }

        // Function called when selection changes in scene
        void OnWizardUpdate()
        {
            UpdateSelectionHelper();
        }

        // Update selection counter
        void UpdateSelectionHelper()
        {
            helpString = "";

            if (0 != m_behaviorTreesList.Count)
            {
                helpString = "Number of behaviorTrees to be serialized is : " + m_behaviorTreesList.Count;
            }
        }

        // Serialize all BehaviorTrees in the project
        void OnWizardCreate()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                             .Where(assembly => assembly.FullName != null &&
                                                                assembly.FullName.StartsWith("Assembly-CSharp"));

            foreach(var assembly in assemblies)
            {
#if UNITY_EDITOR
                Debug.Log("Assembly " + ": " + assembly.ToString());
#endif
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    SerlalizeBT(types[i]);
                }
            }
        }

        // Just Serialize the listed BehaviorTree
        void OnWizardOtherButton()
        {
            // If list is empty, then exit
            if (0 == m_behaviorTreesList.Count)
            {
#if UNITY_EDITOR
                Debug.LogWarning("NO BehaviorTree IS SELECTED");
                helpString = "NO BehaviorTree IS SELECTED";
#endif
                return;
            }
          
            // Cycle and  Serlialize
            for (int i = 0; i < m_behaviorTreesList.Count; i++)
            {
                if (null == m_behaviorTreesList[i])
                {
                    continue;
                }
                TextAsset asset = m_behaviorTreesList[i];

                className = BehaviorTreeUtility.GetClassNameFromTextAsset(asset);
                Type type = BehaviorTreeUtility.GetAssemblyQualifiedType(className);
                SerlalizeBT(type);
            }
        }

        private void SerlalizeBT(Type type)
        {
            if (typeof(BehaviorManager.BehaviorTree) == type.BaseType)
            {
                BehaviorManager.BehaviorTree instance = Activator.CreateInstance(type) as BehaviorManager.BehaviorTree;

                // the type is invalide
                if (null == instance)
                {
#if UNITY_EDITOR
                    Debug.Log("instance is null");
#endif
                    BehaviorManager.BehaviorTree instanceWithAQN = Activator.CreateInstance(BehaviorTreeUtility.GetAssemblyQualifiedType(type.FullName)) as BehaviorManager.BehaviorTree;
                    if (null != instanceWithAQN)
                    {
#if UNITY_EDITOR
                        Debug.Log("Serilize the BehaviorTree : " + type.FullName);
                        BehaviorTreeSerialize.Save(instanceWithAQN);
#endif
                    }
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("Serilize the BehaviorTree : " + type.FullName);
                    BehaviorTreeSerialize.Save(instance);
#endif
                }
            }
        }
    }
}
