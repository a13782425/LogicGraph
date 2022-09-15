using Game.Logic.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Game.Logic.Editor
{
    public sealed partial class LGWindow : EditorWindow
    {
        private GraphOperateData _operateData = new GraphOperateData();
        internal GraphOperateData operateData => _operateData;

        /// <summary>
        /// 逻辑图唯一Id
        /// </summary>
        private string _graphId = null;
        /// <summary>
        /// 当前显示逻辑图的唯一ID
        /// </summary>
        public string graphId
        {
            get => _graphId;
            private set
            {
                _graphId = value;
                operateData.Refresh(value);
            }
        }

        private VisualElement _leftContent;
        private VisualElement _rightContent;

        private FlyoutMenuView _menuView;

        private VisualElement contentContainer;
        public ToolbarView toolbar { get; private set; }
        public FlyoutButton overviewButton { get; private set; }
        public FlyoutButton loadButton { get; private set; }
        public FlyoutButton graphButton { get; private set; }
        public FlyoutButton saveButton { get; private set; }

        private LogicMessage _logicMessage;
        /// <summary>
        /// 总览的视图
        /// </summary>
        internal OverviewGraphView overviewView { get; private set; }

        private LogicGraphView graphView { get; set; }


        /// <summary>
        /// 打开窗口
        /// </summary>
        public static LGWindow OpenWindow()
        {
            LGWindow panel = CreateWindow<LGWindow>();
            panel.titleContent = new GUIContent("逻辑图");
            panel.minSize = LogicUtils.MIN_SIZE;
            panel.Focus();
            return panel;
        }
        /// <summary>
        /// 打开窗口
        /// </summary>
        public static LGWindow OpenWindow(string onlyId)
        {
            if (string.IsNullOrWhiteSpace(onlyId))
            {
                return OpenWindow();
            }
            LGWindow window = m_getWindow(onlyId);
            if (window == null)
            {
                window = OpenWindow();

                window.ShowLogicGraph(onlyId);
            }
            else
            {
                window.m_focus();
            }
            return window;
        }



        /// <summary>
        /// 显示单个逻辑图
        /// </summary>
        /// <param name="onlyId">逻辑图的唯一ID</param>
        public void ShowLogicGraph(string onlyId)
        {
            LGWindow window = m_getWindow(onlyId);
            if (window != null)
            {
                if (window == this)
                {
                    window.m_onGraphClick(null);
                }
                else
                {
                    window.m_focus();
                }
                return;
            }
            this.graphId = onlyId;
            m_showLogicGraph();
        }
        /// <summary>
        /// 关闭当前正在显示的逻辑图
        /// </summary>
        public void CloseLogicGraph()
        {
            if (graphView != null)
            {
                this.graphId = null;
                graphView.RemoveFromHierarchy();
                graphView = null;
                graphButton.Hide();
                saveButton.Hide();
                m_onOverviewClick(null);

            }
        }
        /// <summary>
        /// 注册当前窗口的事件
        /// 同一个事件回调在一个事件ID中只能注册一次
        /// </summary>
        /// <param name="messageId">事件ID</param>
        /// <param name="callback">事件回调</param>
        public void AddListener(int messageId, MessageEventHandler callback) => _logicMessage.AddListener(messageId, callback);
        /// <summary>
        /// 移除当前窗口的事件
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="callback"></param>
        public void RemoveListener(int messageId, MessageEventHandler callback) => _logicMessage.RemoveListener(messageId, callback);
        /// <summary>
        /// 派发当前窗口的事件
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="args"></param>
        public void OnEvent(int messageId, object args = null) => _logicMessage.OnEvent(messageId, args);

    }

    #region 私有方法及事件回调

    //私有方法及事件回调
    partial class LGWindow
    {
        private static LGWindow m_getWindow(string onlyId)
        {
            if (string.IsNullOrWhiteSpace(onlyId))
                return null;
            Object[] panels = Resources.FindObjectsOfTypeAll(typeof(LGWindow));
            LGWindow panel = null;
            foreach (var item in panels)
            {
                if (item is LGWindow p)
                {
                    if (p.graphId == onlyId)
                    {
                        panel = p;
                        break;
                    }
                }
            }
            return panel;
        }
        private void m_focus()
        {
            this.Focus();
            m_onGraphClick(null);
        }
        private void m_showLogicGraph()
        {
            if (!string.IsNullOrWhiteSpace(this.graphId))
            {
                if (this.operateData.summaryInfo == null)
                {
                    this.Close();
                }
                else
                {
                    //删除没有的节点
                    this.operateData.logicGraph.Nodes.RemoveAll(n => n == null);
                    this.operateData.logicGraph.Init();
                    graphView = Activator.CreateInstance(this.operateData.categoryInfo.ViewType) as LogicGraphView;
                    graphView.Initialize(this, this.operateData.logicGraph);
                    graphButton.Show();
                    saveButton.Show();
                    _rightContent.Add(graphView);
                    m_onGraphClick(null);
                }
            }
        }

        private bool m_onLogicAssetsChanged(object args)
        {
            LogicAssetsChangedEventArgs eventArg = args as LogicAssetsChangedEventArgs;
            overviewView.Refresh(eventArg);

            if (!string.IsNullOrWhiteSpace(this.graphId))
            {
                foreach (var item in eventArg.deletedGraphs)
                {
                    if (item == this.operateData.summaryInfo.AssetPath)
                    {
                        CloseLogicGraph();
                        break;
                    }
                }
            }

            return true;
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
            toolbar = new ToolbarView();
            _rightContent.Add(toolbar);
            this.contentContainer.Add(_leftContent);
            this.contentContainer.Add(_rightContent);
            _menuView = new FlyoutMenuView(this);
            this._leftContent.Add(_menuView);
            //初始化侧边栏的按钮
            m_initMenuBtns();
            //初始化总览图
            overviewView = new OverviewGraphView(this);
            _rightContent.Add(overviewView);
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

            overviewButton.onClick += m_onOverviewClick;
            loadButton.onClick += m_onLoadClick;
            graphButton.onClick += m_onGraphClick;
            saveButton.onClick += m_onSaveClick;
            graphButton.Hide();
            saveButton.Hide();
            m_onOverviewClick(null);
        }
        private void m_onOverviewClick(ClickEvent evt)
        {
            overviewView?.Show();
            graphView?.Hide();
            overviewButton.Select();
            graphButton.UnSelect();
        }
        private void m_onLoadClick(ClickEvent obj)
        {
            string path = EditorUtility.OpenFilePanelWithFilters("打开逻辑图", Application.dataPath, new[] { "BaseLogicGraph", "asset" });
            var graph = LogicUtils.LoadGraph<BaseLogicGraph>(FileUtil.GetProjectRelativePath(path));
            if (graph != null)
            {
                this.graphId = graph.OnlyId;
                m_showLogicGraph();
            }
        }
        private void m_onGraphClick(ClickEvent evt)
        {
            overviewView?.Hide();
            graphView?.Show();
            overviewButton.UnSelect();
            graphButton.Select();
        }
        private void m_onSaveClick(ClickEvent obj)
        {
            if (!string.IsNullOrWhiteSpace(this.graphId))
            {
                graphView?.Save();
            }
        }
    }

    #endregion

    #region Unity内部方法

    /// <summary>
    /// Unity内部方法
    /// </summary>
    partial class LGWindow
    {
        /// <summary>
        /// 相当于构造函数
        /// 但会在每次编译后执行
        /// </summary>
        private void OnEnable()
        {
            _logicMessage = new LogicMessage();
            m_createUI();
            if (!string.IsNullOrWhiteSpace(_graphId))
            {
                this.graphId = _graphId;
                m_showLogicGraph();
            }
            LogicUtils.AddListener(LogicEventId.LOGIC_ASSETS_CHANGED, m_onLogicAssetsChanged);
        }

        private void OnDisable()
        {
            LogicUtils.RemoveListener(LogicEventId.LOGIC_ASSETS_CHANGED, m_onLogicAssetsChanged);

        }
        private void OnDestroy()
        {
            if (graphView != null)
            {
                this.graphId = null;
            }
        }
    }

    #endregion
}