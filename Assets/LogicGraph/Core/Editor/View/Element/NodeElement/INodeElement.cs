using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 有输入功能的基础组件
    /// </summary>
    public interface IInputElement
    {
        object value { get; set; }

        event Action<object> onValueChanged;
    }
}
