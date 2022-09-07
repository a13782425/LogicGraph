using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public class FlyoutButton : VisualElement
    {
        private const string STYLE_PATH = "FlyoutButton.uss";

        public event Action<ClickEvent> onClick;

        public Texture icon { get { return _tabIcon.image; } set { _tabIcon.image = value; } }
        public string text { get { return _contentLabel.text; } set { _contentLabel.text = value; } }

        private Image _tabIcon;

        private Label _contentLabel;

        public FlyoutButton()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            _tabIcon = new Image();
            _tabIcon.name = "tabIcon";
            this.Add(_tabIcon);
            _contentLabel = new Label();
            _contentLabel.name = "contentLabel";
            this.Add(_contentLabel);
            this.RegisterCallback<ClickEvent>(onToggleClick);
            this.RegisterCallback<MouseLeaveEvent>(this.ExecuteDefaultAction);
            this.RegisterCallback<MouseEnterEvent>(this.ExecuteDefaultAction);
            //_tabIcon.tintColor = Color.red;
        }

        private void onToggleClick(ClickEvent evt)
        {
            onClick?.Invoke(evt);
            evt.StopImmediatePropagation();
        }

        /// <summary>
        /// 选中当前按钮
        /// </summary>
        public void Select()
        {
            if (!this.ClassListContains("select"))
            {
                this.AddToClassList("select");
            }
        }
        /// <summary>
        /// 选中当前按钮
        /// </summary>
        public void UnSelect()
        {
            this.RemoveFromClassList("select");
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void ZoomIn()
        {
            this.RemoveFromClassList("hide");
            _contentLabel.RemoveFromClassList("hide");
        }
        /// <summary>
        /// 缩小
        /// </summary>
        public void ZoomOut()
        {
            if (!this.ClassListContains("hide"))
            {
                this.AddToClassList("hide");
                _contentLabel.AddToClassList("hide");
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            this.style.display = DisplayStyle.None;
        }
        /// <summary>
        /// 缩小
        /// </summary>
        public void Show()
        {
            this.style.display = DisplayStyle.Flex;
        }
    }
}
