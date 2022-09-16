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
    internal sealed class EdgeView : Edge
    {
        private const string STYLE_PATH = "GraphView/EdgeView.uss";
        public bool isConnected = false;
        //private BaseGraphView owner => ((input ?? output) as PortView).owner.owner;

        public EdgeView() : base()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
        }

        //public override void OnPortChanged(bool isInput)
        //{
        //	base.OnPortChanged(isInput);
        //	UpdateEdgeSize();
        //}

        //public void UpdateEdgeSize()
        //{
        //	if (input == null && output == null)
        //		return;

        //	PortData inputPortData = (input as PortView)?.portData;
        //	PortData outputPortData = (output as PortView)?.portData;

        //	for (int i = 1; i < 20; i++)
        //		RemoveFromClassList($"edge_{i}");
        //	int maxPortSize = Mathf.Max(inputPortData?.sizeInPixel ?? 0, outputPortData?.sizeInPixel ?? 0);
        //	if (maxPortSize > 0)
        //		AddToClassList($"edge_{Mathf.Max(1, maxPortSize - 6)}");
        //}

        //protected override void OnCustomStyleResolved(ICustomStyle styles)
        //{
        //	base.OnCustomStyleResolved(styles);

        //	UpdateEdgeControl();
        //}
    }
}