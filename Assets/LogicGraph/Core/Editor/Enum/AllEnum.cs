using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 元素的排列方向
    /// </summary>
    public enum ElementDirection
    {
        /// <summary>
        /// 垂直
        /// </summary>
        Vertical,
        /// <summary>
        /// 水平
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// 端口类型枚举
    /// </summary>
    [Flags]
    public enum PortDirEnum : byte
    {
        None = 0,
        /// <summary>
        /// 只有进
        /// </summary>
        In = 1,
        /// <summary>
        /// 只有出
        /// </summary>
        Out = 2,
        /// <summary>
        /// 二者皆有
        /// </summary>
        All = In | Out
    }
    /// <summary>
    /// 端口类型
    /// </summary>
    public enum PortTypeEnum : byte
    {

    }
}
