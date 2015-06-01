using ExtraBehaviorDesign.Runtime.Serialize;
using ExtraBehaviorDesign.Runtime.Utility;
using UnityEngine;
namespace ExtraBehaviorDesign.Runtime
{
    class RootTaskSerializer
    {
        public static string SerliazeRootTask(BehaviorManager.BehaviorTree behaviorTree)
        {
            string serializationFilePath = "";
            if (null == behaviorTree)
            {
#if UNITY_EDITOR
                Debug.LogWarning("RootTaskSerializer.SerliazeRootTask ERROR : behaviorTree is NULL");
#endif              
                return serializationFilePath;
            }
            string behaviorTreeName = BehaviorTreeUtility.GetClassNameFromInstance(behaviorTree);
            string rootTaskName = BehaviorTreeUtility.GetClassNameFromInstance(behaviorTree.rootTask);

            // 将行为树-根结点键值对存进map
            BehaviorTreeNamewithRootTaskNameMap.AddPairValue(behaviorTreeName, rootTaskName);

            // 将map刷新到外存
            BTNameAndRTNameDic instance = new BTNameAndRTNameDic();
            instance.BTNamRTNameDic = BehaviorTreeNamewithRootTaskNameMap.BTNameMapRTNameDic;
            ProtoSerializer.SerializeToFile(instance, ProtoSerializer.CONFIG_FILE_NAME);

            // TODO: 其实序列化的时候不需要泛型，只有反序列化才需要知道
            serializationFilePath = ProtoSerializer.SerializeToFile(behaviorTree.rootTask, behaviorTreeName);
            return serializationFilePath;
        }
    }
}
