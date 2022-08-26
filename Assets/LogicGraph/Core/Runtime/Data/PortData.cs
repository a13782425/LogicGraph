using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Runtime
{
    [Serializable]
    public class PortData
    {
    }
    public abstract class EdgeData
    {
        /// <summary>
        /// 是否是输入
        /// </summary>
        public bool isInput = true;
    }
    /// <summary>
    /// 变量的连线数据
    /// </summary>
    [Serializable]
    public class VarEdgeData : EdgeData
    {


        /// <summary>
        /// 当前线对应的变量名
        /// </summary>
        public string varName = "";
    }
}
