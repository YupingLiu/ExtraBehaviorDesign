using ExtraBehaviorDesign.Runtime;
using ExtraBehaviorDesign.Runtime.Tasks;

namespace ExtraBehaviorDesign.TestScripts
{
    class SimpleBT : BehaviorManager.BehaviorTree
    {
        public SimpleBT()
        {
            Sequence s1 = new Sequence();
            Parallel p1 = new Parallel();
            Selector selector1 = new Selector();
            ParallelSelector ps1 = new ParallelSelector();
            RedAction a1 = new RedAction();
            //SimpleAction2 a2 = new SimpleAction2();
            YellowAction a2 = new YellowAction();
            s1.AddChild(a1, 0);
            s1.AddChild(a2, 1);
            p1.AddChild(a1, 0);
            p1.AddChild(a2, 1);
            selector1.AddChild(a1, 0);
            selector1.AddChild(a2, 1);
            ps1.AddChild(a1, 0);
            ps1.AddChild(a2, 1);
            // just test sequence
            // this.taskList.Add(s1);
            // just test parallel
            //this.taskList.Add(p1);
            // just test selector
            //this.taskList.Add(selector1);
            // just test ParallelSelecotr
            this.taskList.Add(ps1);
            this.taskList.Add(a1);
            this.taskList.Add(a2);
            this.parentIndexList.Add(0);
            // 其实组装一个action的东西是不是应该是AddTask方法封装好的呀，而用户只是传进来一个Type
            //SimpleAction2 a3 = new SimpleAction2();
            //this.AddTask(0, 0, a3);

            for (int i = 0; i < taskList.Count; i++)
            {
                NodeData nd = new NodeData();
                nd.ExecutionStatus = TaskStatus.Inactive;
                taskList[i].NodeData = nd;
                taskList[i].OnAwake();
            }
        }
    }
}
