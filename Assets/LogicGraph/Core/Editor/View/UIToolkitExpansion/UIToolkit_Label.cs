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
        public static Label Label(string value)
        {
            Label element = new Label(value);
            element.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            return element;
        }
    }
}
