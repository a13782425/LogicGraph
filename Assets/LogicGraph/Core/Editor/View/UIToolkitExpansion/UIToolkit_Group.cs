using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public static partial class UIToolkit
    {
        public static VisualElement Row(params VisualElement[] elements) => Row(elements.AsEnumerable());
        public static VisualElement Row(IEnumerable<VisualElement> elements)
        {
            VisualElement element = new VisualElement();
            element.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            element.AddToClassList("logicgraph-row");
            foreach (var item in elements)
            {
                element.Add(item);
            }
            return element;
        }

        public static VisualElement Column(params VisualElement[] elements) => Column(elements.AsEnumerable());
        public static VisualElement Column(IEnumerable<VisualElement> elements)
        {
            VisualElement element = new VisualElement();
            element.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            element.AddToClassList("logicgraph-column");
            foreach (var item in elements)
            {
                element.Add(item);
            }
            return element;
        }

        public static VisualElement Box(params VisualElement[] elements) => Box(elements.AsEnumerable());
        public static VisualElement Box(IEnumerable<VisualElement> elements)
        {
            VisualElement element = new Box();
            element.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            foreach (var item in elements)
            {
                element.Add(item);
            }
            return element;
        }
    }
}
