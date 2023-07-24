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
    /// <summary>
    /// 可编辑的文本框
    /// </summary>
    public class EditorLabelElement : VisualElement
    {
        private const string STYLE_PATH = "Uss/Element/EditorLabelElement.uss";
        private Label title_label { get; }
        private TextField input_field { get; }

        private int _maxLength = -1;
        /// <summary>
        /// 显示的最长长度
        /// 小于0不限制长度
        /// </summary>
        public int maxLength { get => _maxLength; set => _maxLength = value; }

        private string _originText;

        /// <summary>
        /// 内容
        /// </summary>
        public string text
        {
            get { return _originText; }
            set
            {
                _originText = value ?? "";
                string temp = "";
                if (maxLength < 0)
                {
                    temp = _originText;
                }
                else
                {
                    if (_originText.Length > maxLength)
                    {
                        temp = _originText.Substring(0, maxLength);
                        temp += "...";
                    }
                    else
                    {
                        temp = _originText;
                    }
                }
                title_label.text = temp;
            }
        }

        /// <summary>
        /// 重命名回调
        /// Action<oldName,newName>
        /// </summary>
        public event Action<string, string> onRename;

        private bool m_editTitleCancelled = false;
        public EditorLabelElement() : this("") { }

        public EditorLabelElement(string title)
        {
            this.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            title_label = new Label(title);
            title_label.name = "title_label";
            this.Add(title_label);
            input_field = new TextField();
            input_field.value = title;
            input_field.name = "input_field";
            this.Add(input_field);
            VisualElement visualElement2 = input_field.Q(TextInputBaseField<string>.textInputUssName);
            visualElement2.RegisterCallback<FocusOutEvent>(delegate
            {
                m_onEditTitleFinished();
            });
            visualElement2.RegisterCallback<KeyDownEvent>(m_titleEditorOnKeyDown);
            RegisterCallback<MouseDownEvent>(m_onMouseDownEvent);
        }

        private void m_onMouseDownEvent(MouseDownEvent evt)
        {
            if (evt.clickCount == 2)
            {
                input_field.SetValueWithoutNotify(text);
                input_field.style.display = DisplayStyle.Flex;
                title_label.style.display = DisplayStyle.None;
                input_field.SelectAll();
                input_field.Q(TextInputBaseField<string>.textInputUssName).Focus();
                evt.StopImmediatePropagation();
            }
        }
        private void m_titleEditorOnKeyDown(KeyDownEvent evt)
        {
            switch (evt.keyCode)
            {
                case KeyCode.Escape:
                    m_editTitleCancelled = true;
                    input_field.Q(TextInputBaseField<string>.textInputUssName).Blur();
                    evt.StopImmediatePropagation();
                    break;
                case KeyCode.Return:
                    input_field.Q(TextInputBaseField<string>.textInputUssName).Blur();
                    evt.StopImmediatePropagation();
                    break;
            }
        }

        private void m_onEditTitleFinished()
        {
            title_label.style.display = DisplayStyle.Flex;
            input_field.style.display = DisplayStyle.None;
            if (!m_editTitleCancelled)
            {
                string oldName = text;
                text = input_field.text;
                onRename?.Invoke(oldName, text);
            }

            m_editTitleCancelled = false;
        }
    }
}
