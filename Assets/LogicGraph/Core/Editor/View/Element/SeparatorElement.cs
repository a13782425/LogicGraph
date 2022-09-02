using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 分割线
    /// </summary>
    public sealed class SeparatorElement : VisualElement
    {
        public float thickness
        {
            get => (float)this.style.width.value.value;
            set
            {
                this.style.width = value;
            }
        }

    }
}
