using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 分割线
    /// </summary>
    public sealed class SeparatorElement : VisualElement
    {
        private ElementDirection m_direction = ElementDirection.Vertical;

        /// <summary>
        /// 分割线方向
        /// </summary>
        public ElementDirection direction
        {
            get
            {
                return m_direction;
            }
            set
            {
                float temp = thickness;
                m_direction = value;
                thickness = temp;
            }
        }
        /// <summary>
        /// 分割线厚度
        /// </summary>
        public float thickness
        {
            get
            {
                if (direction == ElementDirection.Vertical)
                {
                    return this.style.width.value.value;
                }
                return this.style.height.value.value;
            }
            set
            {
                if (direction == ElementDirection.Vertical)
                {
                    this.style.width = value;
                    this.style.height = Length.Percent(100);
                }
                else
                {
                    this.style.height = value;
                    this.style.width = Length.Percent(100);
                }
            }
        }
        /// <summary>
        /// 分割线背景色
        /// </summary>
        public Color color { get => this.style.backgroundColor.value; set => this.style.backgroundColor = value; }

        public SeparatorElement() : this(ElementDirection.Vertical) { }

        public SeparatorElement(ElementDirection vertical)
        {
            this.direction = vertical;
            this.thickness = 2;
            this.color = Color.black;
        }
    }
}
