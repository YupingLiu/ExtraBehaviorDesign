using ExtraBehaviorDesign.Runtime.Tasks;
using ExtraBehaviorDesign.Runtime.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ExtraBehaviorDesign.Runtime
{
    public class BehaviorManager : MonoBehaviour
    {
        public static BehaviorManager instance;

        public BehaviorManager() { }

        public List<BehaviorTree> BehaviorTrees = new List<BehaviorTree>();

        public void Awake()
        {
            BehaviorManager.instance = this;
        }
        public void DisableBehavior(Behavior behavior)
        { }
        public void DisableBehavior(Behavior behavior, bool paused)
        { }

        public bool isAutoSerialize = false;
        public bool isEnableLog = true;

        public void EnableBehavior(Behavior behavior)
        {
            BehaviorManager.BehaviorTree CurrentBT = behavior.BehaviorTree;

            for (int i = 0; i < CurrentBT.taskList.Count; i++)
            {
                CurrentBT.taskList[i].GameObject = behavior.gameObject;
                CurrentBT.taskList[i].Transform = behavior.transform;
                CurrentBT.taskList[i].Collider = behavior.collider;
                CurrentBT.taskList[i].Animation = behavior.animation;
                CurrentBT.taskList[i].Audio = behavior.audio;
                CurrentBT.taskList[i].Camera = behavior.camera;
                CurrentBT.taskList[i].Collider = behavior.collider;
                //TestBT.taskList[i].Collider2D = collider2D;
                //TestBT.taskList[i].ConstantForce = constantForce;
                CurrentBT.taskList[i].GameObject = behavior.gameObject;
                //TestBT.taskList[i].GUIText = guiText;
                //TestBT.taskList[i].GUITexture = guiTexture;
                //TestBT.taskList[i].HingeJoint = hingeJoint;
                //TestBT.taskList[i].Light = light;
                //TestBT.taskList[i].ParticleEmitter = particleEmitter;
                //TestBT.taskList[i].ParticleSystem = particleSystem;
                //TestBT.taskList[i].Renderer = renderer;
                CurrentBT.taskList[i].Rigidbody = behavior.rigidbody;
                CurrentBT.taskList[i].Rigidbody2D = behavior.rigidbody2D;
                CurrentBT.taskList[i].Transform = behavior.transform;
                //TestBT.taskList[i].Owner = behaviorTree.behavior;
                CurrentBT.taskList[i].OnAwake();
            }
            //behaviorTreeMap.Add(behavior, TestBT);
            BehaviorTrees.Add(CurrentBT);

            // Push the index of RootTask to activeStack
            PushTask(CurrentBT, 0, 0);

            if (isEnableLog && behavior.LogTree)
            {
                CurrentBT.LogTree(CurrentBT.taskList[0]);
            }
        }

        public List<Task> GetActiveTasks(Behavior behavior)
        { return null; }
        public List<Task> GetTaskList(Behavior behavior)
        { return null; }

        public void OnDestory()
        {
        }
        public void RestartBehavior(Behavior behavior)
        {

        }
        public void Tick()
        {
            for (int i = 0; i < this.BehaviorTrees.Count; i++)
            {
                this.Tick(this.BehaviorTrees[i]);
            }
        }

        private void Tick(BehaviorTree behaviorTree)
        {
            for (int i = behaviorTree.activeStackList.Count - 1; i >  -1; i--)
            {
                if (0 == behaviorTree.activeStackList[i].Count)
                {
                    continue;
                }
                TaskStatus taskStatus = TaskStatus.Inactive;
                while (TaskStatus.Running != taskStatus)
                {
                    if (0 != behaviorTree.activeStackList.Count && i < behaviorTree.activeStackList.Count && 0 != behaviorTree.activeStackList[i].Count)
                    {
                        int index = behaviorTree.activeStackList[i].Peek();
                        taskStatus = RunTask(behaviorTree, index, i);
                    }
                    else
                    {
                        break;
                    }
                    
                }
            }
            if (0 == behaviorTree.activeStackList.Count && behaviorTree.behavior.RestartWhenComplete)
            {
                PushTask(behaviorTree, 0, 0);
            }
        }

        public void Update()
        {
            this.Tick();
        }

        public class BehaviorTree
        {
            private static Dictionary<string, List<OnExtendBehaviorTreeHandler>> extensionBehaviorDic = new Dictionary<string, List<OnExtendBehaviorTreeHandler>>();
            public List<Task> taskList;
            public List<int> parentIndexList;
            public Task rootTask;
            public List<Stack<int>> activeStackList;
            
            public List<int> parentIndexMap;
          
            public Behavior behavior;
            
            public List<string> logMessageList = new List<string>();
            // 关于Decorator的特殊逻辑
            public List<int> decoratorIndexList;
            public Dictionary<int, TaskStatus> decoratorIndexStatusDic;

            /// <summary>
            /// 根据根结点，组件taskList和parentIndexList
            /// </summary>
            /// <param name="rootTask"></param>
            /// <returns></returns>
            public bool InitalBTStructureData(Task rootTask)
            {
                if (null == rootTask)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("BehaviorManager.BehaviorTree : InitalBTStructureData rootTask " + rootTask + " is NULL");
#endif
                    return false;
                }
                this.rootTask = rootTask;
                // rootTask没有父节点
                this.parentIndexMap.Add(-1);
                return AddToTaskList(rootTask);
            }

            private bool AddToTaskList(Task rootTask)
            {
                bool result = true;

                taskList.Add(rootTask);
               
                if (rootTask is ParentTask)
                {
                    int parentIndex = taskList.Count - 1;
                    parentIndexList.Add(parentIndex);
                    ParentTask parentTask = rootTask as ParentTask;
                    for (int i = 0; i < parentTask.Children.Count; i++)
                    {
                        // parentIndexMap中index为节点在task在taskList中的index，parentIndexMap[index]为taskList[index]节点的父节点下标
                        parentIndexMap.Add(parentIndex);
                        result = AddToTaskList(parentTask.Children[i]);
                        if (false == result)
                        {
                            break;
                        }
                    }
                }
                return result;
            }

            public void LogTree(Task rootTask)
            {
                List<Task> nextTaskList = new List<Task>();
                List<string> currentParentMessage = new List<string>();
                nextTaskList.Add(rootTask);
                currentParentMessage.Add("0.");
                LogTree(currentParentMessage, nextTaskList);

                Debug.Log("The Behavior Tree of " + behavior.gameObject + " is like:");
                Debug.Log("0." + ParseMessage(rootTask.ToString()));
                for (int i = logMessageList.Count - 1; i >= 0; i--)
                {
                    Debug.Log(logMessageList[i]);
                }
            }

            private string ParseMessage(string originalMessage)
            {
                string[] separators = { ".",")"};
                string[] splitArray = originalMessage.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
                originalMessage = splitArray[splitArray.Length - 1];
                return originalMessage;
            }

            // 请重命名重构，并且加上注释
            private void LogTree(List<string> currentParentMessage, List<Task> taskList)
            {
                string message = "";
                List<string> pMessage = new List<string>();
                int childCount = 0;
                List<Task> nextTaskList = new List<Task>();

                for (int i = 0; i < taskList.Count; i++)
                {
                    Task task = taskList[i];
                    if (taskList[i] is ParentTask)
                    {
                        ParentTask parentTask = taskList[i] as ParentTask;
                        for (int j = 0; j < parentTask.Children.Count; j++)
                        {
                            pMessage.Add(currentParentMessage[i] + j + ".");
                            string parseMessage = ParseMessage(parentTask.Children[j].ToString());
                            message = message + pMessage[childCount + j] + parseMessage + "     ";
                        }
                        nextTaskList.AddRange(parentTask.Children);
                        childCount += parentTask.Children.Count;
                    }
                }

                for (int i = 0; i < taskList.Count; i++)
                {
                    if (taskList[i] is ParentTask)
                    {
                        ParentTask parentTask = taskList[i] as ParentTask;
                        childCount = parentTask.Children.Count;
                        for (int k = 0; k < childCount; k++)
                        {
                            Task nextTask = parentTask.Children[k];
                            LogTree(pMessage, nextTaskList);
                        }
                    }
                }
                

                if ("" != message && !logMessageList.Contains(message))
                {
                    logMessageList.Add(message);
                }
            }

            public BehaviorTree()
            {
                this.taskList = new List<Task>();
                this.parentIndexList = new List<int>();
                this.parentIndexMap = new List<int>();
                this.activeStackList = new List<Stack<int>>();
                this.activeStackList.Add(new Stack<int>());
                this.decoratorIndexList = new List<int>();
                this.decoratorIndexStatusDic = new Dictionary<int, TaskStatus>();
            }

            /// <summary>
            /// 插入父节点，同时更新parentIndexList
            /// </summary>
            /// <param name="insertIndex"></param>
            /// <param name="isParentIndex"></param>
            private void AfterAddingRefreshParentIndexList(int insertIndex, int parentIndex, bool isParentIndex)
            {
                int index = 0;
                for (int i = parentIndexList.Count - 1; i > -1 ; i--)
                {
                    if (insertIndex <= parentIndexList[i])
                    {
                        for (int j = 0; j < parentIndexMap.Count; j++)
                        {
                            if (parentIndexMap[j] == parentIndexList[i])
                            {
                                parentIndexMap[j]++;
                            }
                        }
                        parentIndexList[i]++;
                    }
                    // 上面先加1了
                    if (insertIndex == (parentIndexList[i] - 1))
                    {
                        index = i;
                        break;
                    }
                }
                if (isParentIndex)
                {
                    parentIndexList.Insert(index, insertIndex);
                }

                parentIndexMap.Insert(insertIndex, parentIndex);
            }

            public bool AddTask(int parentIndex, int asChildIndex, Task task)
            {
                // 第一步：检查保护
                if (null == task)
                {
                    Debug.LogError("ADD TASK ERROR -- NULL TASK");
                    return false;
                }
                if (!this.parentIndexList.Contains(parentIndex))
                {
                    Debug.LogError("ADD TASK ERROR --HAS NO SUCH PARENT");
                    return false;
                }

                ParentTask parentTask = taskList[parentIndex] as ParentTask;
                int indexOfTaskList;

                if (asChildIndex > -1 && asChildIndex <= parentTask.Children.Count)
                {
                    // 第二步：算出插入的节点应该如何加到tasklist里（在tasklist里的位置）
                    if (0 == asChildIndex)
                    {
                        // 作为父亲的第一个孩纸
                        indexOfTaskList = parentIndex + 1;
                    }
                    else
                    {
                        if (asChildIndex < parentTask.Children.Count)
                        {
                            // 说明插入位置的后面还有兄弟
                            indexOfTaskList = taskList.IndexOf(parentTask.Children[asChildIndex]);
                        }
                        else
                        {
                            // 就是等于了asChildIndex == parentTask.Children.Count，即插在最后
                            // 获得插入位置的左兄弟节点的index加上其所有的孩子数再加1就是最后一个子节点的下标
                            Task preSblingTask = parentTask.Children[asChildIndex - 1];
                            indexOfTaskList = taskList.IndexOf(preSblingTask);
                            GetOffspringCount(preSblingTask, ref indexOfTaskList);
                        }
                    }

                    // 第三步：先把自己加进去
                    parentTask.Children.Insert(asChildIndex, task);
                    taskList.Insert(indexOfTaskList, task);

                    // 第四步：如果插入的是子树按深度优先遍历插入子树节点
                    bool result = true;
                    // 按深度优先
                    if (task is ParentTask)
                    {
                        ParentTask childAsParentTask = task as ParentTask;
                        // 是组合节点，要加到组合节点下标的list里
                        AfterAddingRefreshParentIndexList(indexOfTaskList, parentIndex, true);
                        for (int i = 0; i < parentTask.Children.Count; i++)
                        {
                            result = AddTask(indexOfTaskList, i, parentTask.Children[i]);
                            if (!result)
                            {
                                Debug.LogError("Add Task Error -- at parentTask " + parentIndex + "  and the of failded child is " + i);
                                break;
                            }
                        }
                    }
                    else
                    {
                        AfterAddingRefreshParentIndexList(indexOfTaskList, parentIndex, false);
                    }
                    return result;
                }
                return false;
            }

            /// <summary>
            /// 简单粗暴版RemoveTask，将待删除的节点或子树与parent解除父子关系，重新初始化过taskList、parentIndexList和parentIndexMap
            /// </summary>
            /// <param name="taskIndex"></param>
            /// <returns></returns>
            public bool RemoveTask(int taskIndex)
            {
                if (-1 < taskIndex && taskList.Count > taskIndex)
                {
                    Task task = taskList[taskIndex];

                    int parentIndex = parentIndexMap[taskIndex];

                    // TOTHINK : 析构自己和所有子节点
                    // TODO STH.

                    if (-1 != parentIndex)
                    {
                        ParentTask parentTask = taskList[parentIndex] as ParentTask;
                        parentTask.RemoveChild(task);
                        taskList.Clear();
                        parentIndexList.Clear();
                        parentIndexMap.Clear();
                        InitalBTStructureData(rootTask);
                    }
                    else
                    {
                        // 根结点，整棵树删掉
                        if (taskIndex == 0)
                        {
                            rootTask = null; 
                            taskList.Clear();
                            parentIndexList.Clear();
                            parentIndexMap.Clear();
                        }
                        else
                        { 
#if UNITY_EDITOR
                            Debug.LogWarning("BehaviorManager.BehaviorTree RemoveTask ERROR : the child has no parent and child is not a roottask.");
#endif
                        }
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }

            // 这个方案删除节点后有太多数据要refresh，而且很麻烦。当删除的是一棵子树的时候，读写操作过多。
            //public bool RemoveTask(int taskIndex)
            //{
            //    if(-1 < taskIndex && taskList.Count > taskIndex) 
            //    {
            //        Task task = taskList[taskIndex];
            //        // 第二步：如果是父节点，所有子节点也要删掉
            //        if(task is Composite)
            //        {    
            //            ParentTask parentTask = task as ParentTask;
            //            int index = 0;
            //            for (int i = 0; i < parentTask.Children.Count; i++)
            //            {
            //                index = GetIndexInTaskList(taskIndex, i);
            //                RemoveTask(index);
            //            }

            //            parentIndexList.Remove(taskIndex);
            //            // 移除自己
            //            parentIndexMap.RemoveAt(taskIndex);
            //            // 移除所有子节点
            //            for (int i = 0; i < parentIndexMap.Count; i++)
            //            {
            //                if (taskIndex == parentIndexMap[i])
            //                {
            //                    parentIndexMap.RemoveAt(i);
            //                }
            //            }
            //        }
            //        // 第三步：不论父子，在做完扫尾工作以后，把自己移除
            //        taskList.RemoveAt(taskIndex);
            //    }
            //    else
            //    {
            //        return false;
            //    }        
            //    return true;
            //}

            //public bool RemoveTask(List<int> deleteTaskIndexList)
            //{
            //    int taskIndex = 0;
            //    bool result = true;

            //    for (int i = 0; i < deleteTaskIndexList.Count; i++)
            //    {
            //        taskIndex = deleteTaskIndexList[i];
            //        result = RemoveTask(taskIndex);
            //        if (!result)
            //        {
            //            return result;
            //        }
            //    }
            //    return result;
            //}

            /// <summary>
            /// 获取一个节点的所有后代个数
            /// </summary>
            /// <param name="task"></param>
            /// <returns></returns>
            private void GetOffspringCount(Task task, ref int num)
            {
                if (!(task is ParentTask))
                {
                    num += 1;
                    return;
                }
                ParentTask pt = task as ParentTask;
                for (int i = 0; i < pt.Children.Count; i++)
                {
                    GetOffspringCount(pt.Children[i], ref num);
                }
                num += 1;
                return;
            }

            /// <summary>
            /// 得到index为parentindex的第childindex个孩子在tasklist里的下标
            /// </summary>
            /// <param name="parentIndex"></param>
            /// <param name="childIndex"></param>
            /// <returns></returns>
            public int GetIndexInTaskList(int parentIndex, int childIndex)
            {
                int indexInTaskList = 0;
                // 先做一些保护判断
                if (parentIndexList.Contains(parentIndex))
                {
                    ParentTask parentTask = taskList[parentIndex] as ParentTask;
                    if (childIndex > -1 && childIndex < parentTask.Children.Count)
                    {
                        for (int i = 0; i < childIndex; i++)
                        {
                            if (parentTask.Children[i] is ParentTask)
                            {
                                ParentTask pt = parentTask.Children[i] as ParentTask;
                                indexInTaskList += pt.Children.Count;
                            }
                        }
                        indexInTaskList += parentIndex + childIndex + 1;
                        return indexInTaskList;
                    }
                    else
                    {
                        Debug.LogError("GetIndexInTaskList ERROR: childIndex = " + childIndex + " IS OUT OF RANGE");
                    }
                }
                else
                {
                    Debug.LogError("GetIndexInTaskList ERROR: the parentIndex = " + parentIndex + " is not in the parentIndexList ");
                }
                return -1;
            }

            /// <summary>
            /// 得到某个task实例的在tasklist里的下标
            /// </summary>
            /// <param name="task"></param>
            /// <returns></returns>
            public int GetIndexInTaskList(Task task)
            {
                // 先做一些保护判断
                if (taskList.Contains(task))
	            {
                    return taskList.IndexOf(task);
	            }
                return -1;
            }

            /// <summary>
            /// 得到所有xxxTask类型的节点在tasklist里的下标
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public List<int> GetIndexInTaskList<T>()
            {
                // 先做一些保护判断
                List<int> indexList = new List<int>();
                for  (int i  = 0; i < taskList.Count; i++)
                {
                    if (taskList[i] is T)
                    {
                        indexList.Add(i);
                    }
                }
                return indexList;
            }
        }

        private void PushTask(BehaviorManager.BehaviorTree behaviorTree, int taskIndex, int stackIndex)
        {
            if (0 != behaviorTree.activeStackList.Count)
            {
                if (0 != behaviorTree.activeStackList[stackIndex].Count)
                {
                    if (behaviorTree.activeStackList[stackIndex].Peek() == taskIndex)
                    {
                        return;
                    }
                }
            }
            else
            { 
                behaviorTree.activeStackList.Add(new Stack<int>());
            }
            
            behaviorTree.activeStackList[stackIndex].Push(taskIndex);

            // to avoid Run Multitime
            behaviorTree.taskList[taskIndex].OnStart();
        }

        private void PopTask(BehaviorManager.BehaviorTree behaviorTree, int taskIndex, int stackIndex, bool isPopParent)
        {
            Task task = behaviorTree.taskList[taskIndex];
            // 如果pop出去的是某个父节点的子节点，特别是指Parallel属性的父节点（故父子节点在不同的栈），子节点需要写执行状态给在另一个栈的父节点
            int parentTaskIndex = behaviorTree.parentIndexMap[taskIndex];
            if (-1 != parentTaskIndex)
            {
                ParentTask parentTask = behaviorTree.taskList[parentTaskIndex] as ParentTask;
                // 作为父节点的第几个节点
                int childIndex = parentTask.GetChildIndex(task);
                // 父节点如果是并行节点是存储孩子的执行状态而顺序节点直接回写给父亲  
                if (!parentTask.CanRunParallelChildren())
                {
                    parentTask.OnChildExecuted(task.NodeData.ExecutionStatus);
                }
                else
                {
                    parentTask.OnChildExecuted(childIndex, task.NodeData.ExecutionStatus);
                }
                // 这又似乎是写的不好的地方，应该可以让这里和RunTask关于返回子节点状态返回给父节点的地方合在一起
                parentTask.NodeData.ExecutionStatus = parentTask.OverrideStatus(task.NodeData.ExecutionStatus);
            }

            if (behaviorTree.activeStackList[stackIndex].Peek() == taskIndex)
            {
                behaviorTree.activeStackList[stackIndex].Pop();
                task.OnEnd();

                // 如果弹出的是父节点，需要把它还在栈中的子节点也弹出
                // 这里有问题，如果子节点也是并行节点，那还需要把子节点的子节点栈删掉
                // 所以应该是无须其他判断就可以把该栈后面所有的栈都remove掉（待验证）
                if (isPopParent)
                {
                    for (int i = behaviorTree.activeStackList.Count - 1; i > stackIndex; i--)
                    {
                        behaviorTree.activeStackList.RemoveAt(i);
                        //Stack<int> intStack = behaviorTree.activeStackList[i];
                        
                        // 如果一个栈中的top是待查父节点的子节点，那么剩下的应该都是该父节点的子节点（待测试）
                        //int childIndex = intStack.Peek();
                        //if (behaviorTree.parentIndexMap[childIndex] == taskIndex)
                        //{ 
                        //    behaviorTree.activeStackList.RemoveAt(i);
                        //}
                    }
                }

                if (task.RemoveWhenFinish())
                {
                    behaviorTree.RemoveTask(taskIndex);
                }
            }
            else
            { 
#if UNITY_EDITOR
                Debug.LogWarning("BehaviorManager PopTask ERROR : the task to pop is not in top.");
#endif
            }

            // 如果整个栈都是空的，那就移除
            if (0 == behaviorTree.activeStackList[stackIndex].Count)
            {
                behaviorTree.activeStackList.RemoveAt(stackIndex);
            }
        }

        private TaskStatus RunTask(BehaviorManager.BehaviorTree behaviorTree, int taskIndex, int stackIndex)
        {
            Task task = behaviorTree.taskList[taskIndex];
            // 第一步：加保护
            if (null == task)
            {
                Debug.LogError("NULL TASK");
                return TaskStatus.Failure;
            }

            TaskStatus taskStatus = task.NodeData.ExecutionStatus;

            // 第二步：Push，再在Push里调用OnStart
            PushTask(behaviorTree, taskIndex, stackIndex);
            // 第三步：如果是父节点
            if (task is ParentTask)
            {
                ParentTask parentTask = task as ParentTask;
                int childIndex = -1;
                TaskStatus currentChildTaskStatus = TaskStatus.Inactive;


                // 1、父节点是否已经执行完所有孩子了
                while (parentTask.CanExecute())
                {
                    if (task is Decorator && (behaviorTree.decoratorIndexList.Contains(taskIndex)))
	                {
                        break;
	                }

                    // 2、如果可以并行地执行所有孩子就不用等当前孩子跑完
                    if (parentTask.CanRunParallelChildren() || TaskStatus.Running != currentChildTaskStatus)
                    {
                        childIndex = parentTask.CurrentChildIndex();
                        int nextRunTaskIndex = behaviorTree.GetIndexInTaskList(taskIndex, childIndex);

                        if (parentTask.CanRunParallelChildren())
                        {
                            behaviorTree.activeStackList.Add(new Stack<int>());
                            stackIndex = behaviorTree.activeStackList.Count - 1;
                            parentTask.OnChildStarted(childIndex);
                        }
                        else
                        {
                            parentTask.OnChildStarted();
                        }

                        // 3、跑下一个子节点
                        currentChildTaskStatus = RunTask(behaviorTree, nextRunTaskIndex, stackIndex);
                        taskStatus = currentChildTaskStatus;

                        if (parentTask is Decorator)
                        {
                            behaviorTree.decoratorIndexList.Add(taskIndex);
                            behaviorTree.decoratorIndexStatusDic.Add(taskIndex, parentTask.OverrideStatus(taskStatus));
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                // 关于Decorator的特殊逻辑
                if (task is Decorator && behaviorTree.decoratorIndexList.Contains(taskIndex))
                {
                    taskStatus = behaviorTree.decoratorIndexStatusDic[taskIndex];
                    behaviorTree.decoratorIndexList.Remove(taskIndex);
                    behaviorTree.decoratorIndexStatusDic.Remove(taskIndex);
                    return taskStatus;
                }
                taskStatus = parentTask.OverrideStatus(taskStatus);
            }
            // 第四步：如果是子节点，直接跑自己的OnUpdate
            else
            {
                taskStatus = task.OnUpdate();
            }

            // 保存好本帧自己的执行结果
            task.NodeData.ExecutionStatus = taskStatus;

            // 第五步：不管是正常跑完还是非正常终止的都要调下OnEnd
            if (TaskStatus.Running != taskStatus)  
            {
                PopTask(behaviorTree, taskIndex, stackIndex, (task is ParentTask));
            }
            return taskStatus;
        }

        // Runtask副本
        //private TaskStatus RunTask(BehaviorManager.BehaviorTree behaviorTree, int taskIndex)
        //{
        //    Task task = behaviorTree.taskList[taskIndex];
        //    // 第一步：加保护
        //    if (null == task)
        //    {
        //        Debug.LogError("NULL TASK");
        //        return TaskStatus.Failure;
        //    }
        //    TaskStatus taskStatus = task.NodeData.ExecutionStatus;
        //    // 第二步：已经到了这里，就说明此节点是待运行的，所以调用start函数啦
        //    task.OnStart();
        //    // 第三步：如果是父节点
        //    if (task is ParentTask)
        //    {
        //        ParentTask parentTask = task as ParentTask;
        //        int childIndex = -1;
        //        TaskStatus currentChildTaskStatus = TaskStatus.Inactive;
               
        //        // 1、父节点是否已经执行完所有孩子了
        //        while (parentTask.CanExecute())
        //        {
        //            // 为了多帧Running的特殊逻辑1
        //            if (childIndex == parentTask.CurrentChildIndex() && (TaskStatus.Running != taskStatus || TaskStatus.Inactive != taskStatus || parentTask is Selector))
        //            {
        //                taskStatus = TaskStatus.Running;
        //                break;
        //            }

        //            // 2、如果可以并行地执行所有孩子就不用等当前孩子跑完
        //            if (parentTask.CanRunParallelChildren() || TaskStatus.Running != currentChildTaskStatus)
        //            {
        //                childIndex = parentTask.CurrentChildIndex();


        //                int nextRunTaskIndex = behaviorTree.GetIndexInTaskList(taskIndex, childIndex);
        //                if (parentTask.CanRunParallelChildren())
        //                {
        //                    parentTask.OnChildStarted(childIndex);
        //                }
        //                else
        //                {
        //                    parentTask.OnChildStarted();
        //                }

        //                // 为了多帧Running的特殊逻辑2
        //                Task nextRunTask = behaviorTree.taskList[nextRunTaskIndex];
        //                if (!(parentTask is Decorator))
        //                {
        //                    if (TaskStatus.Success == nextRunTask.NodeData.ExecutionStatus || TaskStatus.Failure == nextRunTask.NodeData.ExecutionStatus)
        //                    {
        //                        continue;
        //                    }
        //                }
                        
        //                // 3、跑下一个子节点
        //                currentChildTaskStatus = RunTask(behaviorTree, nextRunTaskIndex);
        //                taskStatus = currentChildTaskStatus;
        //                // 4、并行节点是存储孩子的执行状态而顺序节点直接回写给父亲  
        //                if (!parentTask.CanRunParallelChildren())
        //                {
        //                    parentTask.OnChildExecuted(taskStatus);
        //                }
        //                else
        //                {
        //                    parentTask.OnChildExecuted(childIndex, taskStatus);
        //                }
        //            }
        //            else if (parentTask is Sequence)
        //            {
        //                // 多帧running的特殊逻辑
        //               currentChildTaskStatus = behaviorTree.taskList[childIndex].OnUpdate();
        //               taskStatus = currentChildTaskStatus;
        //               parentTask.OnChildExecuted(taskStatus);
        //            }
        //        }
        //        // 5、如果是并行节点，需要回写
        //        taskStatus = parentTask.OverrideStatus(taskStatus);
        //        // 啊啊啊啊，突然惊呆了我，这里直接用task赋值也可以？它不是传值吗，怎么变成传引用了？
        //        task.NodeData.ExecutionStatus = taskStatus;
        //        // 我就不信了，再试一个变量
        //        parentTask.ResetCurrentChildIndex();
        //    }
        //    // 第四步：如果是子节点，直接跑自己的OnUpdate
        //    else
        //    {
        //        taskStatus = task.OnUpdate();
        //        task.NodeData.ExecutionStatus = taskStatus;
        //    }
        //    // 第五步：不管是正常跑完还是非正常终止的都要调下OnEnd
        //    if (taskStatus != TaskStatus.Running)
        //    {
        //        task.OnEnd();
        //    }
        //    return taskStatus;
        //}
    }
}
