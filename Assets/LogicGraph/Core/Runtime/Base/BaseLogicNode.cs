using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// 当前节点所在的逻辑图
        /// </summary>
        [NonSerialized]
        protected BaseLogicGraph logicGraph;

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
            OnEnable();
            return true;
        }

        /// <summary>
        /// 拉去数据
        /// </summary>
        public virtual void PullData() { }
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
    }
}
