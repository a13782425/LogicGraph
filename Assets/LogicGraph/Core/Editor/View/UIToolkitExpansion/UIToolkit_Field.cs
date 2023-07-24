using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Game.Logic.Editor
{
    public static partial class UIToolkit
    {
        private static VisualElement Field<T>(T value)
        {
            switch (value)
            {
                case int @int:
                    return new IntegerField() as BaseField<T>;
                case string @string:
                    return new TextField() as BaseField<T>;
                case float @float:
                    return new FloatField() as BaseField<T>;
                case double @double:
                    return new DoubleField() as BaseField<T>;
                case uint @uint:
                    return new UnsignedIntegerField() as BaseField<T>;
                case long @long:
                    return new LongField() as BaseField<T>;
                case Rect @rect:
                    return new RectField() as BaseField<T>;
                case RectInt @rectInt:
                    return new RectIntField() as BaseField<T>;
                case Color @color:
                case Color32 @color32:
                    return new ColorField() as BaseField<T>;
                case Enum @enum:
                    return new EnumField() as BaseField<T>;
                case AnimationCurve @canvas:
                    return new CurveField() as BaseField<T>;
                case Vector2 @vector2:
                    return new Vector2Field() as BaseField<T>;
                case Vector3 @vector3:
                    return new Vector3Field() as BaseField<T>;
                case Vector4 @vector4:
                    return new Vector4Field() as BaseField<T>;
                case Vector2Int @vector2Int:
                    return new Vector2IntField() as BaseField<T>;
                case Vector3Int @vector3Int:
                    return new Vector3IntField() as BaseField<T>;
                case Bounds @bounds:
                    return new BoundsField() as BaseField<T>;
                case Gradient @gradient:
                    return new GradientField() as BaseField<T>;
                case BoundsInt boundsInt:
                    return new BoundsIntField() as BaseField<T>;
                case bool @bool:
                    return new Toggle() as BaseField<T>;
                case Object @object:
                    return new ObjectField();
                default: return null;
            }
        }

        public static VisualElement Field<T>(T value, in bool delayInput = false) => Field("", value, delayInput);
        public static VisualElement Field<T>(string label, T value, in bool delayInput = false) => Field(label, value, null, delayInput);
        public static VisualElement Field<T>(string label, T value, Action<T> onValueChanged, in bool delayInput = false)
        {
            VisualElement element = Field<T>(value);
            if (element == null)
                return null;
            element.styleSheets.Add(LogicUtils.Load<StyleSheet>(STYLE_PATH));
            if (element is ObjectField objField)
            {
                objField.objectType = typeof(T);
                if (!string.IsNullOrWhiteSpace(label))
                    objField.label = label;
                if (onValueChanged != null)
                    objField.RegisterValueChangedCallback<Object>((a) => onValueChanged.Invoke((T)Convert.ChangeType(a.newValue, typeof(T))));
            }
            if (element is BaseField<T> baseField)
            {
                if (!string.IsNullOrWhiteSpace(label))
                    baseField.label = label;
                if (onValueChanged != null)
                    baseField.RegisterValueChangedCallback<T>((a) => onValueChanged.Invoke(a.newValue));
                if (element is TextInputBaseField<T> textInputBaseField)
                {
                    textInputBaseField.isDelayed = delayInput;
                }
            }
            return element;
        }
    }
}
