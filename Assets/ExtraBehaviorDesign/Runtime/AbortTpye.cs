using System;

namespace ExtraBehaviorDesign.Runtime.Tasks
{
    public enum AbortTpye
    {
        None = 0,
        Self = 1, 
        LowerPriority = 2,
        Both = 3,
    }
}
