using ExtraBehaviorDesign.Runtime;
using ExtraBehaviorDesign.Runtime.Tasks;
using UnityEngine;
namespace ExtraBehaviorDesign.TestScripts
{
    public class MiddleBT : BehaviorManager.BehaviorTree
    {
        public MiddleBT()
        {
            Selector selector = 
                ScriptableObject.CreateInstance<Selector>();
            Sequence sequence = 
                ScriptableObject.CreateInstance<Sequence>();
            ParallelSelector ps = 
                ScriptableObject.CreateInstance<ParallelSelector>();
            CanISeePlayer c1 = 
                ScriptableObject.CreateInstance<CanISeePlayer>();
            CanISeePlayer c2 = 
                ScriptableObject.CreateInstance<CanISeePlayer>();
            TranslatetoARandomPosition t1 = 
                ScriptableObject.CreateInstance<TranslatetoARandomPosition>();
            UntilSuccess untilSuccess = 
                ScriptableObject.CreateInstance<UntilSuccess>();
            RotateByY r1 = ScriptableObject.CreateInstance<RotateByY>();

            selector.AddChild(sequence, 0);
            selector.AddChild(ps, 1);
            sequence.AddChild(c1, 0);
            sequence.AddChild(t1, 1);
            ps.AddChild(untilSuccess, 0);
            ps.AddChild(r1, 1);
            untilSuccess.AddChild(c2, 0);

            InitalBTStructureData(selector);
        }
    }
}
