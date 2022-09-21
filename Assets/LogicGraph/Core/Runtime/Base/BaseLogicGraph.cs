using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Logic.Runtime
{
    /// <summary>
    /// 逻辑图基类
    /// </summary>
    [Serializable]
    public class BaseLogicGraph : ScriptableObject
    {
        [SerializeField]
        private string _onlyId = "";
        public string OnlyId => _onlyId;

        #region 节点定义

        [SerializeReference]
        private List<BaseLogicNode> _nodes = new List<BaseLogicNode>();
        /// <summary>
        /// 当前逻辑图的所有节点
        /// </summary>
        public List<BaseLogicNode> Nodes => _nodes;
        /// <summary>
        /// 当前逻辑图的所有节点的另一种缓存
        /// </summary>
        private Dictionary<int, BaseLogicNode> _nodeDic = new Dictionary<int, BaseLogicNode>();

        [SerializeReference]
        private List<int> _startNodes = new List<int>();
        /// <summary>
        /// 逻辑图开始节点
        /// 一切罪恶的开始
        /// </summary>       
        public List<int> StartNodes => _startNodes;

        #endregion

        #region 变量定义

        [SerializeReference]
        private List<IVariable> _variables = new List<IVariable>();
        /// <summary>
        /// 当前逻辑图的所有变量
        /// </summary>
        public List<IVariable> Variables => _variables;
        /// <summary>
        /// 当前逻辑图的所有变量的另一种缓存
        /// </summary>
        private Dictionary<string, IVariable> _varDic = new Dictionary<string, IVariable>();

#if UNITY_EDITOR
        [SerializeField]
        [Multiline]
        //[HideInInspector]
        private string __editorData = "";
#endif

        #endregion

        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="onlyId">节点唯一Id</param>
        /// <returns></returns>
        public BaseLogicNode GetNode(int onlyId) => _nodeDic.ContainsKey(onlyId) ? _nodeDic[onlyId] : null;

        /// <summary>
        /// 获取变量
        /// </summary>
        /// <param name="varName">变量名</param>
        /// <returns></returns>
        public IVariable GetVar(string varName) => _varDic.ContainsKey(varName) ? _varDic[varName] : null;

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            foreach (var item in _variables)
            {
                _varDic.Add(item.Name, item);
            }
            foreach (var item in Nodes)
            {
                _nodeDic.Add(item.OnlyId, item);
            }
            Nodes.ForEach(n => n.Initialize(this));
        }
    }
}
