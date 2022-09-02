using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public sealed class GraphViewInspector : VisualElement
    {
        private const string STYLE_PATH = "GraphViewInspector.uss";

        public float width { get => (float)this.style.width.value.value; set { this.style.width = value; } }

        private Label titleElement { get; }
        public GraphViewInspector()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            this.titleElement = new Label("Inspector");
            this.titleElement.name = "title-label";
            this.Add(titleElement);
            var separator = new VisualElement();
            separator.name = "separator";
            separator.AddToClassList("inspector-separator");
            this.Add(separator);
        }
    }
}
