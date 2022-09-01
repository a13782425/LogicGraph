using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Game.Logic.Editor.LogicUtils;

namespace Game.Logic.Editor
{
    //抄FluidSideMenu
    public sealed class FlyoutMenuView : VisualElement
    {
        private const string STYLE_PATH = "FlyoutMenuView.uss";
        #region MenuState

        private enum MenuState
        {
            Expanded,
            IsExpanding,
            Collapsed,
            IsCollapsing
        }

        /// <summary>
        /// 当前界面属于哪个窗口
        /// </summary>
        public LGWindow owner { get; }
        public VisualElement layoutContainer { get; }
        public VisualElement headerContainer { get; }
        public ScrollView buttonsScrollViewContainer { get; }
        public VisualElement buttonsContainer { get; }
        public FlyoutButton overviewButton { get; }
        public FlyoutButton loadButton { get; }
        public FlyoutButton graphButton { get; }
        public FlyoutButton saveButton { get; }
        /// <summary>
        /// 自定义按钮
        /// </summary>
        private List<FlyoutButton> buttons { get; }

        private Image headerIcon { get; }
        private Label headerLabel { get; }

        #endregion

        public FlyoutMenuView(LGWindow lgwindow)
        {
            owner = lgwindow;
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(EDITOR_STYLE_PATH, STYLE_PATH)));
            buttons = new List<FlyoutButton>();
            layoutContainer = new VisualElement();
            layoutContainer.AddToClassList("FlyoutMenuView");
            layoutContainer.name = "layoutContainer";
            this.Add(layoutContainer);

            headerContainer = new VisualElement();
            headerContainer.name = "headerContainer";
            headerContainer.AddToClassList("FlyoutMenuView");
            headerContainer.AddToClassList("OptionalContainer");
            layoutContainer.Add(headerContainer);

            headerIcon = new Image();
            headerIcon.name = "headerIcon";
            headerIcon.AddToClassList("headerIcon");
            headerContainer.Add(headerIcon);

            headerLabel = new Label("逻辑图");
            headerLabel.name = "headerLabel";
            headerLabel.AddToClassList("headerLabel");
            headerContainer.Add(headerLabel);

            buttonsContainer = new VisualElement();
            buttonsContainer.name = "buttonsContainer";
            buttonsContainer.AddToClassList("FlyoutMenuView");
            layoutContainer.Add(buttonsContainer);

            headerIcon.RegisterCallback<ClickEvent>(onHeaderClick);
        }

        public FlyoutButton AddButton(string text)
        {
            FlyoutButton tabButton = new FlyoutButton();
            tabButton.text = text;
            buttons.Add(tabButton);
            buttonsContainer.Add(tabButton);
            var spaceBlock = new VisualElement();
            spaceBlock.name = "spaceBlock";
            spaceBlock.style.height = 8;
            spaceBlock.style.alignSelf = Align.Center;
            spaceBlock.style.flexShrink = 0;
            buttonsContainer.Add(spaceBlock);
            return tabButton;
        }
        private void onHeaderClick(ClickEvent evt)
        {
            if (this.ClassListContains("hide"))
            {
                this.RemoveFromClassList("hide");
                headerContainer.RemoveFromClassList("hide");
                buttonsContainer.RemoveFromClassList("hide");
                headerIcon.RemoveFromClassList("hide");
                headerLabel.RemoveFromClassList("hide");
                foreach (var item in buttons)
                {
                    item.ZoomIn();
                }
            }
            else
            {
                this.AddToClassList("hide");
                headerContainer.AddToClassList("hide");
                buttonsContainer.AddToClassList("hide");
                headerIcon.AddToClassList("hide");
                headerLabel.AddToClassList("hide");
                foreach (var item in buttons)
                {
                    item.ZoomOut();
                }
            }
        }
    }
}
