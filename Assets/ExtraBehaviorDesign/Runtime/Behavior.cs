using ExtraBehaviorDesign.Runtime.Tasks;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ExtraBehaviorDesign.Runtime
{
    public delegate void OnExtendBehaviorTreeHandler(BehaviorManager.BehaviorTree behaviorTree);
    public delegate void OnDeleteBehaviorTaskHandler(BehaviorManager.BehaviorTree behaviorTree);
    [Serializable]
    public abstract class Behavior : MonoBehaviour, IBehavior
    {
        [SerializeField]
        private TextAsset aiAsset;
        public TextAsset AIAsset
        {
            get { return aiAsset; }
            set { aiAsset = value; }
        }

        [SerializeField]
        private bool logTree = true;
        public bool LogTree
        {
            get { return logTree; }
            set { logTree = value; }
        }

        [SerializeField]
        private bool restartWhenComplete;
        public bool RestartWhenComplete
        {
            get { return restartWhenComplete; }
            set { restartWhenComplete = value; }
        }

        [SerializeField]
        private bool startWhenEnabled;
        public bool StartWhenEnabled
        {
            get { return startWhenEnabled; }
            set { startWhenEnabled = value; }
        }

        private bool mInitialized = false;

        public Behavior() 
        {
            m_behaviorTree = new BehaviorManager.BehaviorTree();
        }

        public TaskStatus ExecutionStatus { get; set; }

        public static BehaviorManager CreateBehaviorManager() 
        {
            if (null == BehaviorManager.instance)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = "Behavior Manager";
                return gameObject.AddComponent<BehaviorManager>();
            }
            return null; 
        }

        public void DisableBehavior() { }
        public void DisableBehavior(bool pause) { }

        // 一添加该Componenet,就会调用到OnEnable的方法，但不会去到Start
        public void OnEnable()
        {
            //if (null != BehaviorManager.instance)
            //{
            //    BehaviorManager.instance.EnableBehavior(this);
            //    return;
            //}
            if (this.StartWhenEnabled && this.mInitialized)
            {
                this.EnableBehavior();
            }
        }
        public void EnableBehavior() 
        {
            Behavior.CreateBehaviorManager();
            // 反序列化行为树资源
            DeserilizeBehaviorAsset();
            // 动态扩展行为树
            NotifyExtend();
            // 运行行为树
            BehaviorManager.instance.EnableBehavior(this); 
        }

        public void Start()
        {
            if (this.startWhenEnabled)
            {
                this.EnableBehavior();
            }
            this.mInitialized = true;
        }

        private BehaviorManager.BehaviorTree m_behaviorTree;

        public BehaviorManager.BehaviorTree BehaviorTree
        {
            get { return m_behaviorTree; }
            set { m_behaviorTree = value; }
        }

        private bool DeserilizeBehaviorAsset()
        {
            bool result = true;
            if (null != AIAsset)
            {
                // 读取Asset
                TextAsset aiAsset = AIAsset;
                // 序列化资源路径
                string path = AssetDatabase.GetAssetPath(aiAsset);
                // 序列化资源名字
                string behaviorName = Path.GetFileNameWithoutExtension(path);
                // 反序列化
                BehaviorTreeDeserialize.Load(behaviorName, ref m_behaviorTree);
                // 这句话有存在的必要吗，循环引用，目前真是有用到，最好去掉
                m_behaviorTree.behavior = this;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("Behavior DeserilizeBehaviorAsset Warning : the asset of deserialization is null.");
#endif
                result = false;
            }
            return result;
        }
        private OnExtendBehaviorTreeHandler OnExtendHandler;
        private OnDeleteBehaviorTaskHandler OnDeleteHandler;

        public void AddExtendBehaviorTreeHandler(OnExtendBehaviorTreeHandler handler)
        {
            OnExtendHandler += handler;
        }

        public void RemoveExtendBehaviorTreeHandler(OnExtendBehaviorTreeHandler handler)
        {
            OnExtendHandler -= handler;
        }

        public void AddDeleteBehaviorTaskHandler(OnDeleteBehaviorTaskHandler handler)
        {
            OnDeleteHandler += handler;
        }

        public void RemoveDeleteBehaviorTaskHandler(OnDeleteBehaviorTaskHandler handler)
        {
            OnDeleteHandler -= handler;
        }

        private void NotifyExtend()
        {
            if (null != OnExtendHandler)
            {
                OnExtendHandler(m_behaviorTree);
            }
        }

        private void NotifyDelete()
        {
            if (null != OnDeleteHandler)
            {
                OnDeleteHandler(m_behaviorTree);
            }
        }

    }
}
