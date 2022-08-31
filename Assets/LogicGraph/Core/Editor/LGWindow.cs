using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public class LGWindow : EditorWindow
    {
        private VisualElement _leftContent;
        private VisualElement _rightContent;
        private VisualElement _bottomContent;

        private GraphListPanel _graphListPanel;

        private VisualElement contentContainer;

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
            contentContainer = new VisualElement();
            contentContainer.name = "contentContainer";
            this.rootVisualElement.Add(contentContainer);
            rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(LogicUtils.EDITOR_STYLE_PATH, "LGWindow.uss")));
            _leftContent = new VisualElement();
            _leftContent.name = "left";
            _rightContent = new VisualElement();
            _rightContent.name = "right";
            _bottomContent = new VisualElement();
            _bottomContent.name = "bottom";
            //_bottomContent.style.height = 24;
            //_bottomContent.style.backgroundColor = new Color(32 / 255f, 32 / 255f, 32 / 255f);


            this.contentContainer.Add(_leftContent);
            this.contentContainer.Add(_rightContent);
            this.rootVisualElement.Add(_bottomContent);
            //var _topToolbar = new ToolbarView();
            //_topToolbar.onDrawLeft += m_onDrawTopLeft;
            //_topContent.Add(_topToolbar);
            //_bottomToolbar = new ToolbarView();
            //_bottomToolbar.onDrawLeft += m_onDrawBottomLeft;
            //_bottomToolbar.onDrawRight += m_onDrawBottomRight;
            //_bottomContent.Add(_bottomToolbar);
            _graphListPanel = new GraphListPanel(this);
            this._leftContent.Add(new FlyoutMenuView());
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