﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public static class NodeElementUtils
    {

        public static Dictionary<Type, Type> ElementMapping = new Dictionary<Type, Type>()
        {
            { typeof(string), typeof(TextField) },
            { typeof(int), typeof(IntegerField)},
            { typeof(bool), typeof(Toggle)},
            { typeof(double), typeof(DoubleField)},
            { typeof(float), typeof(FloatField)},
            { typeof(Vector2), typeof(Vector2Field)},
            { typeof(Vector3), typeof(Vector3Field)},
            { typeof(Vector4), typeof(Vector4Field)},
            { typeof(Color), typeof(ColorField)},
            { typeof(Gradient), typeof(GradientField)},
            { typeof(AnimationCurve),typeof(CurveField)},
            { typeof(Bounds),typeof(BoundsField)},
        };

        static NodeElementUtils()
        {
            var methodInfos = TypeCache.GetMethodsWithAttribute<NodeElementMappingAttribute>().ToList();
            var retType = typeof(Dictionary<Type, Type>);
            foreach (var methodInfo in methodInfos)
            {
                if (methodInfo.GetParameters().Count() > 0)
                {
                    Debug.LogError("节点字段和组件映射的方法不需要参数");
                    continue;
                }
                if (methodInfo.ReturnType != retType)
                {
                    Debug.LogError("节点字段和组件映射的方法的返回类型必须是:" + retType);
                    continue;
                }
                Dictionary<Type, Type> dic = methodInfo.Invoke(null, null) as Dictionary<Type, Type>;
                foreach (var item in dic)
                {
                    if (ElementMapping.ContainsKey(item.Key))
                    {
                        ElementMapping[item.Key] = item.Value;
                    }
                    else
                    {
                        ElementMapping.Add(item.Key, item.Value);
                    }
                }
            }
            Debug.LogError(methodInfos.Count);
        }
        /// <summary>
        /// 设置字段组件的默认样式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        public static void SetBaseFieldStyle<T>(BaseField<T> element)
        {
            element.style.minHeight = 24;
            element.style.marginTop = 2;
            element.style.marginRight = 2;
            element.style.marginLeft = 2;
            element.style.marginBottom = 2;
            element.style.unityTextAlign = TextAnchor.MiddleLeft;
            element.labelElement.style.minWidth = 50;
            element.labelElement.style.fontSize = 12;
        }

        public static void Show(this VisualElement visual)
        {
            visual.style.display = DisplayStyle.Flex;
        }
        public static void Hide(this VisualElement visual)
        {
            visual.style.display = DisplayStyle.None;
        }
    }
}
