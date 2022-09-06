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
        private const string STYLE_PATH = "OverviewGraph/OverviewGraphView.uss";
        public LGWindow onwer { get; }
        private CreateLGSearchWindow _createLGSearch;
        private List<OverviewGroup> _groups = new List<OverviewGroup>();
        public OverviewGraphView(LGWindow window)
        {
            onwer = window;
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, STYLE_PATH)));
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
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            foreach (var item in _groups)
            {
                item.Show();
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            foreach (var item in _groups)
            {
                item.Hide();
            }
        }

        /// <summary>
        /// 添加分组
        /// </summary>
        private void m_addGroup()
        {
            foreach (var item in LogicProvider.LGEditorList)
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
                _createLGSearch = ScriptableObject.CreateInstance<CreateLGSearchWindow>();
                _createLGSearch.onSelectHandler += m_onCreateLGSelectEntry;
            }

            Vector2 screenPos = onwer.GetScreenPosition(obj.eventInfo.mousePosition);
            SearchWindow.Open(new SearchWindowContext(screenPos), _createLGSearch);
        }

        private bool m_onCreateLGSelectEntry(SearchTreeEntry arg1, SearchWindowContext arg2)
        {
            LGEditorCache configData = arg1.userData as LGEditorCache;
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
            string file = Path.GetFileNameWithoutExtension(path);
            BaseLogicGraph graph = ScriptableObject.CreateInstance(configData.GraphType) as BaseLogicGraph;
            LogicGraphView graphView = Activator.CreateInstance(configData.ViewType) as LogicGraphView;
            GraphEditorData graphEditor = new GraphEditorData();
            graph.name = file;
            if (graphView.DefaultVars != null)
            {
                foreach (var item in graphView.DefaultVars)
                {
                    graph.Variables.Add(item);
                }
            }
            graphView = null;
            path = path.Replace(Application.dataPath, "Assets");
            graphEditor.LogicName = file;
            AssetDatabase.CreateAsset(graph, path);
            AssetDatabase.Refresh();
            graph.SetEditorData(graphEditor);
            //Todo: 打开当前创建的逻辑图
            return true;
        }
    }
}
