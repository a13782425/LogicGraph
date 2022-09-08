using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 总览图的工具栏
    /// </summary>
    public sealed class OverviewGraphToolBar : VisualElement
    {
        private string _curValue = "All";
        public OverviewGraphToolBar()
        {
            var abc = new ToolbarPopupSearchField();
            //VisualElement visualElement = abc.Q(className: TextInputBaseField<string>.ussClassName);
            abc.RegisterCallback<ChangeEvent<string>>(abcd);
            abc.menu.AppendAction("All", test1, test);
            //Debug.LogError(visualElement);
            abc.menu.AppendAction("测试", test1, test);
            abc.menu.AppendAction("测试2", test1, test);
            //abc.menu.PrepareForDisplay(null);
            //(abc.menu.MenuItems()[0] as DropdownMenuAction).UpdateActionStatus((abc.menu.MenuItems()[0] as DropdownMenuAction).eventInfo);
            this.Add(abc);
        }

        private void test1(DropdownMenuAction obj)
        {
            _curValue = obj.name;
            Debug.LogError("11111" + obj.name);
            //obj.UpdateActionStatus(obj.eventInfo);
        }

        private void abcd(ChangeEvent<string> evt)
        {
            Debug.LogError(evt.newValue);
        }

        private DropdownMenuAction.Status test(DropdownMenuAction arg)
        {
            if (_curValue == "All")
            {
                return DropdownMenuAction.Status.Checked;
            }
            else
            {
                if (arg.name == _curValue)
                {
                    return DropdownMenuAction.Status.Checked;
                }
                else
                    return DropdownMenuAction.Status.Normal;
            }
        }
    }
}
