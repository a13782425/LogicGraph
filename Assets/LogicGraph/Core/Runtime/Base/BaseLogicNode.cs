using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Runtime
{
    public abstract class BaseLogicNode
    {

        [SerializeField]
        private int _onlyId;
        /// <summary>
        /// 数据唯一Id
        /// </summary>
        public int onlyId => _onlyId;
        [SerializeField]
        private List<int> _childs = new List<int>();
        public List<int> Childs => _childs;

        [SerializeField]
        private List<VarEdgeData> _varEdges = new List<VarEdgeData>();
        public List<VarEdgeData> VarEdges => _varEdges;

        /// <summary>
        /// 当前节点所在的逻辑图
        /// </summary>
        [NonSerialized]
        protected BaseLogicGraph logicGraph;

        /// <summary>
        /// 缓存的需要端口的变量
        /// </summary>
        private readonly Dictionary<string, FieldInfo> _cacheVarField = new Dictionary<string, FieldInfo>();

        private bool _isComplete = false;
        /// <summary>
        /// 是否执行完毕
        /// </summary>
        public bool IsComplete { get => _isComplete; protected set => _isComplete = value; }

        private bool _isSkip = false;
        /// <summary>
        /// 是否跳过子节点
        /// </summary>
        public bool IsSkip { get => _isSkip; protected set => _isSkip = value; }

        public bool Initialize(BaseLogicGraph graph)
        {
            this.logicGraph = graph;
            m_initVarFieldDatas();
            OnEnable();
            return true;
        }



        /// <summary>
        /// 拉去数据
        /// </summary>
        public virtual void PullData()
        {

            foreach (var item in VarEdges)
            {
                if (item.isInput)
                {

                }
            }
        }
        /// <summary>
        /// 推送数据
        /// </summary>
        public virtual void PushData() { }

        /// <summary>
        /// 节点初始化的时候调用
        /// </summary>
        protected virtual bool OnEnable() => true;
        /// <summary>
        /// 节点执行调用的方法
        /// </summary>
        /// <returns></returns>
        public virtual bool OnExecute() { IsComplete = true; return true; }

        /// <summary>
        /// 节点停止调用
        /// 只有正在执行的节点才会被调用
        /// 如果逻辑图正在执行时退出，会先执行正在运行节点的OnStop方法再执行OnExit
        /// </summary>
        /// <returns></returns>
        public virtual bool OnStop() => true;

        /// <summary>
        /// 节点退出
        /// 当逻辑图完成时候
        /// 无论逻辑图是否正常退出全部节点都会执行
        /// 如果逻辑图正在执行时退出，会先执行正在运行节点的OnStop方法再执行OnExit
        /// </summary>
        /// <returns></returns>
        public virtual bool OnExit() => true;

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <returns></returns>
        public virtual List<int> GetChild() => Childs;


        #region 公共方法

        /// <summary>
        /// 获取当前图变量的值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="varName">变量名</param>
        /// <returns></returns>
        public T GetVarValue<T>(string varName)
        {
            return (T)GetVarValue(varName, typeof(T));
        }
        /// <summary>
        /// 获取当前图变量的值
        /// </summary>
        /// <param name="varName">变量名</param>
        /// <param name="type">值的类型</param>
        /// <returns></returns>
        public object GetVarValue(string varName, Type type)
        {
            var curVar = GetVar(varName);
            if (curVar == null)
                return Activator.CreateInstance(type);
            return curVar.Value;
        }

        /// <summary>
        /// 获取当前图的变量
        /// </summary>
        /// <param name="varName">变量名</param>
        /// <returns></returns>
        public BaseVariable GetVar(string varName) => logicGraph == null ? null : logicGraph.GetVar(varName);

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化所有含端口的变量
        /// </summary>
        private void m_initVarFieldDatas()
        {
            var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var serializeAttribute = field.GetCustomAttribute<SerializeField>();
                var inputAttribute = field.GetCustomAttribute<VarInputAttribute>();
                var outputAttribute = field.GetCustomAttribute<VarOutputAttribute>();
                if (!field.IsPublic && serializeAttribute == null)
                    continue;
                if (inputAttribute == null && outputAttribute == null)
                    continue;
                _cacheVarField.Add(field.Name, field);
            }
        }

        #endregion
    }
}
