using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 总览图
    /// </summary>
    internal class OverviewGraphView : GraphView
    {
        private const string STYLE_PATH = "Uss/OverviewGraph/OverviewGraphView.uss";
        public LGWindow onwer { get; }
        private CreateGraphSearchWindow _createLGSearch;
        private List<OverviewGroup> _groups = new List<OverviewGroup>();
        public OverviewGraphView(LGWindow window)
        {
            Input.imeCompositionMode = IMECompositionMode.On;
            onwer = window;
            this.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new ClickSelector());
            ContentZoomer contentZoomer = new ContentZoomer();
            contentZoomer.minScale = 0.5f;
            contentZoomer.maxScale = 2f;
            contentZoomer.scaleStep = 0.05f;
            this.AddManipulator(contentZoomer);
            m_addGroup();
            Vector2 pos = new Vector2(100, 120);
            this.UpdateViewTransform(pos, Vector3.one);
            this.RegisterCallback<KeyUpEvent>(m_onKeyDown);
            onwer.toolbar.Add(new OverviewGraphToolBar());
            this.StretchToParentSize();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            this.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            this.style.display = DisplayStyle.None;
        }
        /// <summary>
        /// 刷新总览视图
        /// </summary>
        /// <param name="eventArg"></param>
        internal void Refresh(LogicAssetsChangedEventArgs eventArg)
        {
            foreach (var item in _groups)
            {
                item.Refresh(eventArg);
            }
        }
        /// <summary>
        /// 添加分组
        /// </summary>
        private void m_addGroup()
        {
            foreach (var item in LogicProvider.LGCategoryList)
            {
                var group = new OverviewGroup(this);
                group.Initialize(item);
                this.AddElement(group);
                _groups.Add(group);
            }
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("创建逻辑图", onCreateLogicClick, DropdownMenuAction.AlwaysEnabled);
        }
        private void m_onKeyDown(KeyUpEvent evt)
        {
            switch (evt.keyCode)
            {
                case KeyCode.Escape:
                    ClearSelection();
                    evt.StopImmediatePropagation();
                    break;
                case KeyCode.Delete:
                    var temp = selection.OfType<OverviewNode>().ToList();
                    foreach (var item in temp)
                    {
                        if (EditorUtility.DisplayDialog("警告", $"正在删除《{item.data.LogicName}》逻辑图", "确定", "取消"))
                        {
                            LogicUtils.RemoveGraph(item.data.AssetPath);
                        }
                    }
                    evt.StopImmediatePropagation();
                    break;
            }
        }

        private void onCreateLogicClick(DropdownMenuAction obj)
        {
            if (_createLGSearch == null)
            {
                _createLGSearch = ScriptableObject.CreateInstance<CreateGraphSearchWindow>();
                _createLGSearch.onSelectHandler += m_onCreateLGSelectEntry;
            }

            Vector2 screenPos = onwer.GetScreenPosition(obj.eventInfo.mousePosition);
            SearchWindow.Open(new SearchWindowContext(screenPos), _createLGSearch);
        }

        private bool m_onCreateLGSelectEntry(SearchTreeEntry arg1, SearchWindowContext arg2)
        {
            LGCategoryInfo configData = arg1.userData as LGCategoryInfo;
            string path = EditorUtility.SaveFilePanel("创建逻辑图", Application.dataPath, "LogicGraph", "asset");
            if (string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayDialog("错误", "路径为空", "确定");
                return false;
            }
            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog("错误", "创建文件已存在", "确定");
                return false;
            }
            path = FileUtil.GetProjectRelativePath(path);
            BaseLogicGraph graph = LogicUtils.CreateLogicGraph(configData.GraphType, path);
            //Todo: 打开当前创建的逻辑图
            return true;
        }


    }
}
