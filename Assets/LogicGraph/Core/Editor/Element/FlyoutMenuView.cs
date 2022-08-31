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

        private bool isShow = true;
        private MenuState currentMenuState { get; set; }
        private bool areButtonLabelsHidden { get; set; }
        public VisualElement layoutContainer { get; }
        public VisualElement headerContainer { get; }
        public ScrollView buttonsScrollViewContainer { get; }
        public VisualElement buttonsContainer { get; }
        public FlyoutToggleButton overviewButton { get; }
        public FlyoutToggleButton loadButton { get; }
        public FlyoutToggleButton graphButton { get; }
        public FlyoutToggleButton saveButton { get; }

        //public VisualElement footerContainer { get; }

        private Image headerIcon { get; }
        private Label headerLabel { get; }

        #endregion

        public FlyoutMenuView()
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(EDITOR_STYLE_PATH, STYLE_PATH)));

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


            //buttonsScrollViewContainer = new ScrollView();
            //buttonsScrollViewContainer.verticalScrollerVisibility = ScrollerVisibility.Hidden;
            //buttonsScrollViewContainer.name = "buttonsScrollViewContainer";
            //buttonsScrollViewContainer.AddToClassList("FlyoutMenuView");
            //layoutContainer.Add(buttonsScrollViewContainer);

            buttonsContainer = new VisualElement();
            buttonsContainer.name = "buttonsContainer";
            buttonsContainer.AddToClassList("FlyoutMenuView");
            layoutContainer.Add(buttonsContainer);

            overviewButton = AddButton("总览");
            overviewButton.tabIcon.AddToClassList("overview");

            loadButton = AddButton("打开");
            loadButton.tabIcon.AddToClassList("load");

            graphButton = AddButton("图");
            graphButton.tabIcon.AddToClassList("graph");

            saveButton = AddButton("保存");
            saveButton.tabIcon.AddToClassList("save");

            headerIcon.RegisterCallback<ClickEvent>(onHeaderClick);
        }

        public FlyoutToggleButton AddButton(string text)
        {
            FlyoutToggleButton tabButton = new FlyoutToggleButton();
            tabButton.text = text;
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
            if (headerIcon.ClassListContains("hide") && !isShow)
            {
                isShow = false;
            }
            isShow = !isShow;
            if (isShow)
            {
                this.RemoveFromClassList("hide");
                headerContainer.RemoveFromClassList("hide");
                buttonsContainer.RemoveFromClassList("hide");
                headerIcon.RemoveFromClassList("hide");
                headerLabel.RemoveFromClassList("hide");
                overviewButton.Show();
                loadButton.Show();
                graphButton.Show();
                saveButton.Show();
            }
            else
            {
                this.AddToClassList("hide");
                headerContainer.AddToClassList("hide");
                buttonsContainer.AddToClassList("hide");
                headerIcon.AddToClassList("hide");
                headerLabel.AddToClassList("hide");
                overviewButton.Hide();
                loadButton.Hide();
                graphButton.Hide();
                saveButton.Hide();
            }

        }
    }
}
