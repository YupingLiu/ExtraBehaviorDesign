using ExtraBehaviorDesign.Runtime;
using ExtraBehaviorDesign.Runtime.Tasks;
using ExtraBehaviorDesign.TestScripts;
using UnityEngine;
/// <summary>
/// 供服务器随机选择的BT，与原始BT的不同是让挂载此棵BT的GO变红色
/// </summary>
public class MiddleBTRandom : BehaviorManager.BehaviorTree
{
    public MiddleBTRandom()
    {
        Selector selector = ScriptableObject.CreateInstance<Selector>();
        Sequence sequence = ScriptableObject.CreateInstance<Sequence>();
        ParallelSelector ps = ScriptableObject.CreateInstance<ParallelSelector>();
        CanISeePlayer c1 = ScriptableObject.CreateInstance<CanISeePlayer>();
        CanISeePlayer c2 = ScriptableObject.CreateInstance<CanISeePlayer>();
        TranslatetoARandomPosition t1 = ScriptableObject.CreateInstance<TranslatetoARandomPosition>();
        UntilSuccess untilSuccess = ScriptableObject.CreateInstance<UntilSuccess>();
        RotateByY r1 = ScriptableObject.CreateInstance<RotateByY>();
            
        // 包含0，不包含3
        int randomChoice = Random.Range(0, 3);
        string color = "";
        switch (randomChoice)
        {
            case 0:
                RedAction redTask = ScriptableObject.CreateInstance<RedAction>();
                color = "red";
                sequence.AddChild(redTask, 0);
                break;
            case 1:
                BlueAction blueTask = ScriptableObject.CreateInstance<BlueAction>();
                color = "blue";
                sequence.AddChild(blueTask, 0);
                break;
            case 2:
                YellowAction yellowTask = ScriptableObject.CreateInstance<YellowAction>();
                color = "yellow";
                sequence.AddChild(yellowTask, 0);
                break;
            default:
                break;
        }

        if (null == UIBTInformationNotifier.instance)
        {
            UIBTInformationNotifier notifier = new UIBTInformationNotifier();
        }
        UIBTInformationNotifier.instance.NotifyColorInfoChangeEvent(color);


        selector.AddChild(sequence, 0);
        selector.AddChild(ps, 1);
        sequence.AddChild(c1, 1);
        sequence.AddChild(t1, 2);
        ps.AddChild(untilSuccess, 0);
        ps.AddChild(r1, 1);
        untilSuccess.AddChild(c2, 0);

        InitalBTStructureData(selector);
    }
}

