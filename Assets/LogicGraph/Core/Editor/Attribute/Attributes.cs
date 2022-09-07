using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class LogicGraphAttribute : Attribute
    {
        /// <summary>
        /// 逻辑图名字
        /// </summary>
        public string LogicName { get; private set; }
        /// <summary>
        /// 对应那个逻辑图
        /// </summary>
        public Type GraphType { get; private set; }
        /// <summary>
        /// 当前逻辑图的颜色
        /// </summary>
        public Color? Color { get; set; }

        public LogicGraphAttribute(string str, Type graphType)
        {
            LogicName = str;
            GraphType = graphType;
        }
    }

    /// <summary>
    /// 格式化逻辑图方法
    /// 方法参数(BaseLogicGraph,path)
    /// </summary>
    public sealed class GraphFormatAttribute : Attribute
    {

    }
}
