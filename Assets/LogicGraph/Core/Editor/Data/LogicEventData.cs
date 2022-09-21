using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 逻辑图资源变化
    /// </summary>
    public sealed class LogicAssetsChangedEventArgs
    {
        /// <summary>
        /// 移动了的逻辑图
        /// </summary>
        public List<string> moveGraphs = new List<string>();
        /// <summary>
        /// 删除了的逻辑图
        /// 里面存的不一定是逻辑图的assets
        /// </summary>
        public List<string> deletedGraphs = new List<string>();
        /// <summary>
        /// 新增的逻辑图
        /// </summary>
        public List<string> addGraphs = new List<string>();

    }

    /// <summary>
    /// 变量变化
    /// </summary>
    public sealed class VarModifyEventArgs
    {
        /// <summary>
        /// 变化的变量
        /// </summary>
        public IVariable var;
    }
}
