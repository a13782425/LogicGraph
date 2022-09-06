using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public sealed partial class LGWindow : EditorWindow
    {
        private VisualElement _leftContent;
        private VisualElement _rightContent;
        //private VisualElement _bottomContent;

        private FlyoutMenuView _menuView;

        private VisualElement contentContainer;

        public FlyoutButton overviewButton { get; private set; }
        public FlyoutButton loadButton { get; private set; }
        public FlyoutButton graphButton { get; private set; }
        public FlyoutButton saveButton { get; private set; }

        internal OverviewGraphView overviewGraph { get; private set; }

        public static void ShowLogic()
        {
            LGWindow panel = CreateWindow<LGWindow>();
            panel.titleContent = new GUIContent("逻辑图");
            panel.minSize = LogicUtils.MIN_SIZE;
            panel.Focus();
        }

        /// <summary>
        /// 相当于构造函数
        /// 但会在每次编译后执行
        /// </summary>
        private void OnEnable()
        {
            m_createUI();
        }

        private void m_createUI()
        {
            titleContent = new GUIContent("逻辑图");
            contentContainer = new VisualElement();
            contentContainer.name = "contentContainer";
            this.rootVisualElement.Add(contentContainer);
            rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, "LGWindow.uss")));
            _leftContent = new VisualElement();
            _leftContent.name = "left";
            _rightContent = new VisualElement();
            _rightContent.name = "right";
            //_bottomContent = new VisualElement();
            //_bottomContent.name = "bottom";


            this.contentContainer.Add(_leftContent);
            this.contentContainer.Add(_rightContent);
            //this.rootVisualElement.Add(_bottomContent);
            _menuView = new FlyoutMenuView(this);
            this._leftContent.Add(_menuView);

            //初始化侧边栏的按钮
            m_initMenuBtns();
            //初始化总览图
            m_initOverviewGraph();
        }

        /// <summary>
        /// 初始化总览图
        /// </summary>
        private void m_initOverviewGraph()
        {
            overviewGraph = new OverviewGraphView(this);
            _rightContent.Add(overviewGraph);
            overviewGraph.Show();
        }
        /// <summary>
        /// 初始化侧边栏的按钮
        /// </summary>
        private void m_initMenuBtns()
        {
            overviewButton = _menuView.AddButton("总览");
            overviewButton.icon = LogicUtils.GetTexture("flyout_all");

            loadButton = _menuView.AddButton("打开");
            loadButton.icon = LogicUtils.GetTexture("flyout_load");

            graphButton = _menuView.AddButton("图");
            graphButton.icon = LogicUtils.GetTexture("flyout_graph");

            saveButton = _menuView.AddButton("保存");
            saveButton.icon = LogicUtils.GetTexture("flyout_save");

            overviewButton.onClick += onOverviewClick;

            overviewButton.Select();
            graphButton.Hide();
            saveButton.Hide();

            void onOverviewClick(ClickEvent evt)
            {
                overviewButton.Select();
                graphButton.UnSelect();
            }

        }
    }
}