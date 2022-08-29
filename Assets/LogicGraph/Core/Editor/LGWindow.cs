using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public class LGWindow : EditorWindow
    {
        private VisualElement _topContent;
        private VisualElement _centerContent;
        private VisualElement _bottomContent;

        private GraphListPanel _graphListPanel;

        public static void ShowLogic()
        {
            LGWindow panel = CreateWindow<LGWindow>();
            panel.titleContent = new GUIContent("逻辑图");
            panel.minSize = LogicUtils.MIN_SIZE;
            panel.Focus();
        }


        private void OnEnable()
        {
            m_createUI();

        }

        private void m_createUI()
        {
            titleContent = new GUIContent("逻辑图");
            _topContent = new VisualElement();
            _topContent.name = "top";
            _topContent.style.height = 21;
            _topContent.style.flexGrow = 0;
            _centerContent = new VisualElement();
            _centerContent.name = "center";
            _centerContent.style.flexGrow = 1;
            _bottomContent = new VisualElement();
            _bottomContent.name = "bottom";
            _bottomContent.style.height = 21;
            _bottomContent.style.flexGrow = 0;

            this.rootVisualElement.Add(_topContent);
            this.rootVisualElement.Add(_centerContent);
            this.rootVisualElement.Add(_bottomContent);
            var _topToolbar = new ToolbarView();
            _topToolbar.onDrawLeft += m_onDrawTopLeft;
            _topContent.Add(_topToolbar);
            //_bottomToolbar = new ToolbarView();
            //_bottomToolbar.onDrawLeft += m_onDrawBottomLeft;
            //_bottomToolbar.onDrawRight += m_onDrawBottomRight;
            //_bottomContent.Add(_bottomToolbar);
            _graphListPanel = new GraphListPanel(this);
            this.rootVisualElement.Add(_graphListPanel);
            _graphListPanel.Hide();

        }

        private void m_onDrawTopLeft()
        {
            if (GUILayout.Button("菜单", EditorStyles.toolbarButton))
            {
                _graphListPanel.Show();
            }
            //onDrawTopLeft?.Invoke();
        }

    }
}