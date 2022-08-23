using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Logic.Runtime
{
    public class BaseLogicGraph : ScriptableObject
    {
        [SerializeField]
        private string _onlyId = "";
        public string OnlyId => _onlyId;

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


        /// <summary>
        /// 获取一个节点
        /// </summary>
        /// <param name="onlyId"></param>
        /// <returns></returns>
        public BaseLogicNode GetNodeById(int onlyId) => _nodeDic.ContainsKey(onlyId) ? _nodeDic[onlyId] : null;

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            foreach (var item in Nodes)
            {
                _nodeDic.Add(item.onlyId, item);
            }
            Nodes.ForEach(n => n.Initialize(this));
        }
    }
}
