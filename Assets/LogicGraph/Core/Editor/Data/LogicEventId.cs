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
    }
}
