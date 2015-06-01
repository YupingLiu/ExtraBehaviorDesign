using System;

namespace ExtraBehaviorDesign.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TaskDescriptionAttribute : Attribute
    {
        public readonly string mDescription;

        public TaskDescriptionAttribute(string description){}

        public string Description { get{return mDescription;} }
    }
}
