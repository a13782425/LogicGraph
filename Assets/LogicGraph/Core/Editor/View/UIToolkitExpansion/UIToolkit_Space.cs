using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public static partial class UIToolkit
    {
        public static VisualElement Space()
        {
            VisualElement element = new VisualElement();
            element.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            element.AddToClassList("logicgraph-space");
            return element;
        }
    }
}
