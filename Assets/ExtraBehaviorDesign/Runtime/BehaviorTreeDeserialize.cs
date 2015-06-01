using ExtraBehaviorDesign.Runtime.Serialize;
using ExtraBehaviorDesign.Runtime.Tasks;
using System;
using UnityEngine;

namespace ExtraBehaviorDesign.Runtime
{
     public class BehaviorTreeDeserialize
     {
         /// <summary>
         /// 传入带命名空间的类名，反序列化出该类
         /// </summary>
         /// <param name="behaviorName"></param>
         /// <param name="behaviorTree"></param>
         public static void Load(string behaviorName, ref BehaviorManager.BehaviorTree behaviorTree)
         {
             if (null == behaviorTree.rootTask)
             {
                 // 读取BTName-RTName配置信息
                 string ConfigDicFilePath = ProtoSerializer.SERIALIZATION_CACHE_DATA_PATH + ProtoSerializer.CONFIG_FILE_NAME + ProtoSerializer.DATA_POSTFIX;
                 BTNameAndRTNameDic instance = new BTNameAndRTNameDic();
                 ProtoSerializer.DeserializeFromFile(ConfigDicFilePath, typeof(BTNameAndRTNameDic),ref instance);
                 BehaviorTreeNamewithRootTaskNameMap.BTNameMapRTNameDic = instance.BTNamRTNameDic;

                 // 反序列化根结点
                 string filePath = ProtoSerializer.SERIALIZATION_CACHE_DATA_PATH + behaviorName + ProtoSerializer.DATA_POSTFIX;
                 string rootTaskName = "";

                 if (BehaviorTreeNamewithRootTaskNameMap.BTNameMapRTNameDic.ContainsKey(behaviorName))
                 {
                     rootTaskName = BehaviorTreeNamewithRootTaskNameMap.BTNameMapRTNameDic[behaviorName];
                 }
                 Type type = Type.GetType(rootTaskName);
                 ProtoSerializer.DeserializeFromFile(filePath, type, ref behaviorTree.rootTask);

                 // 构建行为树结构
                 behaviorTree.InitalBTStructureData(behaviorTree.rootTask);
             }

             if (BehaviorManager.instance.isEnableLog)
             {
                 LogChildren(behaviorTree.rootTask);
             }
         }
         
         private static void LogChildren(Task task)
         {
            Debug.Log("task:" + task); 

            if(task is ParentTask)
            {
                 ParentTask pt = task as ParentTask;
                 for (int i = 0; i < pt.Children.Count; i++)
                 {
                     LogChildren(pt.Children[i]);
                 }
             }
         }
    }
}
