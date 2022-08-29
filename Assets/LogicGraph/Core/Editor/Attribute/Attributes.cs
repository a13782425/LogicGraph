using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public LogicGraphAttribute(string str, Type graphType)
        {
            LogicName = str;
            GraphType = graphType;
        }
    }
}
