using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    internal sealed class GraphVariablePropView : VisualElement
    {
        private BaseGraphView graphView;

        public VarEditorData varData { get; private set; }
        public GraphVariablePropView(BaseGraphView graphView, VarEditorData varData)
        {
            this.graphView = graphView;
            this.varData = varData;
            IVariable defaultVar = graphView.DefaultVars.FirstOrDefault(a => a.Name == varData.target.Name);
            //if (param.HasDefaultValue && defaultVar == null)
            //{
            //    Add(new Label("导出:"));
            //    Toggle toggle = new Toggle();
            //    toggle.style.marginLeft = 24;
            //    toggle.value = param.Export;
            //    toggle.RegisterCallback<ChangeEvent<bool>>(a => param.Export = a.newValue);
            //    Add(toggle);
            //}
            Label label = new Label("默认值:");
            Add(label);
            if (defaultVar == null)
            {
                //VisualElement uiElement = param.GetUI();
                //uiElement.style.marginLeft = 24;
                //Add(uiElement);
            }
            else
            {
                Label defaultLabel = new Label("默认参数不能赋初始值");
                defaultLabel.style.marginLeft = 24;
                Add(defaultLabel);
            }
        }
    }
}
