using System.Collections.Generic;
using UnityEngine;

namespace ExtraBehaviorDesign.Runtime.Serialize
{
    public class BehaviorTreeNamewithRootTaskNameMap
    {
        public static Dictionary<string, string> BTNameMapRTNameDic = new Dictionary<string, string>();

        /// <summary>
        /// 添加行为树类名-根结点类名键值对
        /// </summary>
        /// <param name="behaviorTreeName"></param>
        /// <param name="rootTaskName"></param>
        /// <returns></returns>
        public static bool AddPairValue(string behaviorTreeName, string rootTaskName)
        {
            if ("" == behaviorTreeName || "" == rootTaskName)
            {
#if UNITY_EDITOR
                Debug.LogWarning("BehaviorTreeNameMapRootTaskName.AddPairValue : behaviorTreeName or rootTaskName is empty");
#endif
                return false;
            }

            if (BTNameMapRTNameDic.ContainsKey(behaviorTreeName))
            {
                // 默认修改覆盖
                BTNameMapRTNameDic[behaviorTreeName] = rootTaskName;
#if UNITY_EDITOR
                Debug.LogWarning("BehaviorTreeNameMapRootTaskName.AddPairValue : the behaviorTree already exesit, it'll be overwrite");
#endif
            }
            else
            {
                BTNameMapRTNameDic.Add(behaviorTreeName, rootTaskName);
            }

            return true;
        }

        /// <summary>
        /// 根据行为树类名获得其根结点类名
        /// </summary>
        /// <param name="behaviorTreeName"></param>
        /// <returns></returns>
        public static string GetRootTaskName(string behaviorTreeName)
        {
            string rootTaskName = "";

            if ("" == behaviorTreeName)
            {
#if UNITY_EDITOR
                Debug.LogWarning("BehaviorTreeNameMapRootTaskName.GetRootTaskName : behaviorTreeName is empty");
#endif
                return rootTaskName;
            }
            if (BTNameMapRTNameDic.ContainsKey(behaviorTreeName))
            {
                rootTaskName = BTNameMapRTNameDic[behaviorTreeName];
            }
            return rootTaskName;
        }
    }
}
