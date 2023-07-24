using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public static partial class UIToolkit
    {
        public static Tabs Tabs(params (string label, VisualElement element)[] tabs) => Tabs(tabs.AsEnumerable());

        public static Tabs Tabs(IEnumerable<(string label, VisualElement element)> tabs)
        {
            Tabs element = new Tabs();
            element.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            foreach (var item in tabs)
            {
                element.AddTab(new Label(item.label), item.element);
            }
            return element;
        }
    }
}
