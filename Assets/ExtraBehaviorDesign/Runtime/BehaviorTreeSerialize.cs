namespace ExtraBehaviorDesign.Runtime
{
    public class BehaviorTreeSerialize
    {
        public static string Save(BehaviorManager.BehaviorTree behaviorTree)
        {
            return RootTaskSerializer.SerliazeRootTask(behaviorTree);    
        }
    }
}
