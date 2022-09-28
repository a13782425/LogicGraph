using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Runtime
{
    #region 端口特性

    /// <summary>
    /// 端口连接的基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public abstract class PortAttribute : Attribute
    {
        /// <summary>
        /// 端口显示的名字
        /// </summary>
        public string name;
        /// <summary>
        /// 是否允许多条链接
        /// 
        /// 节点端口默认允许多条链接
        /// 参数入端口默认允许一个链接
        /// 参数出端口默认允许多条链接
        /// </summary>
        public abstract bool isMultiple { get; }
        public PortAttribute(string name = null)
        {
            this.name = name;
        }
    }
    /// <summary>
    /// 入端口
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class InputAttribute : Attribute
    {
        /// <summary>
        /// 端口显示的名字
        /// </summary>
        public string name;
        public InputAttribute(string name = null)
        {
            this.name = name;
        }
    }
    /// <summary>
    /// 出端口
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class OutputAttribute : Attribute
    {
        /// <summary>
        /// 端口显示的名字
        /// </summary>
        public string name;
        public OutputAttribute(string name = null)
        {
            this.name = name;
        }
    }

    /// <summary>
    /// 参数入端口
    /// </summary>
    public sealed class VarInputAttribute : PortAttribute
    {
        public override bool isMultiple => false;
        public VarInputAttribute(string name = null) : base(name) { }
    }

    /// <summary>
    /// 参数出端口
    /// </summary>
    public class VarOutputAttribute : PortAttribute
    {
        public override bool isMultiple => true;
        public VarOutputAttribute(string name = null) : base(name) { }
    }

    /// <summary>
    /// 节点入端口
    /// </summary>
    public sealed class NodeInputAttribute : PortAttribute
    {
        public override bool isMultiple => true;
        public NodeInputAttribute(string name = null) : base(name) { }
    }

    /// <summary>
    /// 节点出端口
    /// </summary>
    public class NodeOutputAttribute : PortAttribute
    {
        public override bool isMultiple => true;
        public NodeOutputAttribute(string name = null) : base(name) { }
    }
    #endregion
}
