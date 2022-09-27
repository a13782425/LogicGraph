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
    /// 方法参数(逻辑图:BaseLogicGraph,路径:string)
    /// 返回类型(是否导出成功:bool)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class GraphFormatAttribute : Attribute
    {
        /// <summary>
        /// 对应那个逻辑图
        /// </summary>
        public Type GraphType { get; private set; }
        /// <summary>
        /// 逻辑图名字
        /// </summary>
        public string LogicName { get; private set; }
        /// <summary>
        /// 后缀
        /// </summary>
        public string Extension { get; private set; }

        public GraphFormatAttribute(string str, Type graphType, string extension = "txt")
        {
            LogicName = str;
            GraphType = graphType;
            Extension = extension;
        }
    }
    /// <summary>
    /// 逻辑图节点
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]

    public sealed class LogicNodeAttribute : Attribute
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        public Type NodeType { get; private set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string MenuText { get; private set; }
        /// <summary>
        /// 拥有什么端口
        /// </summary>
        public PortDirEnum PortType = PortDirEnum.All;
        ///// <summary>
        ///// 包含的逻辑图
        ///// </summary>
        //public Type[] IncludeGraphs = new Type[0];

        ///// <summary>
        ///// 排除的逻辑图
        ///// 优先判断排除的
        ///// </summary>
        //public Type[] ExcludeGraphs = new Type[0];

        /// <summary>
        /// 当前节点是否启用
        /// </summary>
        public bool IsEnable = true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeType">节点类型</param>
        /// <param name="menuText">菜单名</param>
        public LogicNodeAttribute(Type nodeType, string menuText)
        {
            NodeType = nodeType;
            MenuText = menuText;
        }

        public bool HasType(Type type)
        {
            bool result = true;
            //if (ExcludeGraphs.Length > 0)
            //{
            //    result = !ExcludeGraphs.Contains(type);
            //}
            //if (result && IncludeGraphs.Length > 0)
            //{
            //    result = IncludeGraphs.Contains(type);
            //}
            return result;
        }
    }

    /// <summary>
    /// 输入所需要的 类型和组件的映射
    /// 返回类型(Dic<Type:字段类型,Type:组件类型>)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class InputElementMappingAttribute : Attribute
    {
    }
}
