using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 事件ID
    /// 内部ID从-10000开始向下递增
    /// </summary>
    public class LogicEventId
    {
        /// <summary>
        /// 逻辑图资源变化
        /// args : LogicAssetsChangedEventArgs
        /// </summary>
        public const int LOGIC_ASSETS_CHANGED = -10000;

        /// <summary>
        /// 变量变化
        /// args : VarModifyEventArgs
        /// </summary>
        public const int VAR_MODIFY = -10100;
        /// <summary>
        /// 增加一个变量
        /// args : VarAddEventArgs
        /// </summary>
        public const int VAR_ADD = -10200;
        /// <summary>
        /// 删除一个变量
        /// args : VarDelEventArgs
        /// </summary>
        public const int VAR_DEL = -10300;

    }
}
