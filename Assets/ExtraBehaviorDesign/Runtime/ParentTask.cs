using ProtoBuf;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace ExtraBehaviorDesign.Runtime.Tasks
{
    [ProtoContract]
    [ProtoInclude(1, "ExtraBehaviorDesign.Runtime.Tasks.Composite, Assembly-CSharp")]
    [ProtoInclude(2, typeof(Decorator))]
    public abstract class ParentTask : Task
    {
        [ProtoMember(3)]
        protected List<Task> children;

        protected ParentTask(){}

        public List<Task> Children { get { return this.children; } }

        public void AddChild(Task child, int index) 
        {
            if (null == this.children)
            {
                this.children = new List<Task>();
                if (child is Decorator)
                {
                    hasDecoratorChild = true;
                }
            }
            this.children.Insert(index, child);
        }

        public void RemoveChild(int index)
        {
            if (-1 < index && index < this.children.Count )
            {
                this.children.RemoveAt(index);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("ParentTask RemoveChild ERROR :Invalid Index.");
#endif
            }
        }

        public void RemoveChild(Task child)
        {
            if (null != child)
            {
                int index = GetChildIndex(child);
                if (-1 != index)
                {
                    this.children.RemoveAt(index);
                }
                else
                { 
#if UNITY_EDITOR
                    Debug.LogWarning("ParentTask RemoveChild ERROR : The parent does not have the child.");
#endif
                }
            }
            else
            { 
#if UNITY_EDITOR
                Debug.LogWarning("ParentTask RemoveChild ERROR : The child to remove is NULL.");
#endif
            }
        }

        public virtual bool CanExecute() { return true; }
        public virtual bool CanReveluate() { return true; }
        public virtual bool CanRunParallelChildren() { return false; }
        public virtual int CurrentChildIndex() { return 0; }
        public virtual TaskStatus Decorate(TaskStatus status) { return TaskStatus.Success; }
        public virtual int MaxChildren() { return 0; }
        public virtual void OnChildExecuted(TaskStatus childStatus) { }
        public virtual void OnChildExecuted(int childIndex, TaskStatus childStatus) { }
        public virtual void OnChildStarted() { }
        public virtual void OnChildStarted(int childIndex) { }
        public virtual void OnConditionalAbort(int childIndex) { }
        public virtual TaskStatus OverrideStatus() { return TaskStatus.Running; }
        public virtual TaskStatus OverrideStatus(TaskStatus taskStutas) { return taskStutas; }

        public void ReplaceAddChild(Task child, int index)
        {
            if (null != this.children && index < this.children.Count)
            {
                this.children[index] = child;
                return;
            }
            this.AddChild(child, index);
        }

        // 这个方法写得很不好，应该有更好的解决方法
        public int GetChildIndex(Task task)
        {
            if (null == task)
            {
#if UNITY_EDITOR
                Debug.LogWarning("ParentTask GetChildIndex ERROR : The task is null.");
#endif
            }
            if (Children.Contains(task))
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    if (task == Children[i])
                    {
                        return i;
                    }
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("ParentTask GetChildIndex ERROR : The parentTask does not have the child.");
#endif
            }
            return -1;
        }

        public bool hasDecoratorChild = false;
    }
}
