using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public sealed class FlyoutToggleGroup
    {
        private List<FlyoutToggleButton> _toggleButtons = new List<FlyoutToggleButton>();

        /// <summary>
        /// 
        /// </summary>
        public List<FlyoutToggleButton> ToggleButtons => _toggleButtons;
        public void AddToggle(FlyoutToggleButton button)
        {
            ToggleButtons.Add(button);
        }
    }
    public class FlyoutToggleButton : VisualElement
    {

        private const string STYLE_PATH = "FlyoutToggleButton.uss";

        public Image tabIcon { get; }
        public Label contentLabel { get; }
        public Button delButton { get; }

        public string text
        {
            get
            {
                return contentLabel.text;
            }
            set
            {
                contentLabel.text = value;
            }
        }
        public FlyoutToggleButton()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
            tabIcon = new Image();
            tabIcon.name = "tabIcon";
            this.Add(tabIcon);
            contentLabel = new Label();
            contentLabel.name = "contentLabel";
            this.Add(contentLabel);
            this.RegisterCallback<ClickEvent>(onToggleClick);
            this.RegisterCallback<PointerEnterEvent>(onPointerEnter);
            this.RegisterCallback<PointerLeaveEvent>(onPointerLeave);
        }

        private void onPointerLeave(PointerLeaveEvent evt)
        {
            this.RemoveFromClassList("pointEnter");
        }

        private void onPointerEnter(PointerEnterEvent evt)
        {
            this.AddToClassList("pointEnter");
        }

        private void onToggleClick(ClickEvent evt)
        {
            UnityEngine.Debug.LogError("onToggleClick");
        }

        public void Show()
        {
            this.RemoveFromClassList("hide");
            contentLabel.RemoveFromClassList("hide");
        }
        public void Hide()
        {
            if (!this.ClassListContains("hide"))
            {
                this.AddToClassList("hide");
                contentLabel.AddToClassList("hide");
            }
        }
    }
}
