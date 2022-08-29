using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Runtime
{
    /// <summary>
    /// 变量的连线数据
    /// </summary>
    [Serializable]
    public class VarEdgeData
    {
        /// <summary>
        /// 是否是输入
        /// </summary>
        public bool isInput = true;

        /// <summary>
        /// 当前线对应的图内变量名
        /// </summary>
        public string varName = "";
        /// <summary>
        /// 当前线对应的节点内变量名
        /// </summary>
        public string fieldName = "";
    }
}
