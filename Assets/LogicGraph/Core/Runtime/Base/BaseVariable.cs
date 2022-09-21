using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Runtime
{
    public interface IVariable
    {
        string Name { get; set; }
        object Value { get; set; }
        Type GetValueType();
    }

    [Serializable]
    public abstract class BaseVariable<T> : IVariable
    {
        [SerializeField]
        private string _name;

        /// <summary>
        /// 变量名
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [SerializeField]
        private T val = default;

        public T Value { get => val; set => val = value; }
        /// <summary>
        /// 当前变量的值
        /// </summary>
        object IVariable.Value { get => val; set => val = (T)value; }

        /// <summary>
        /// 获取值的类型
        /// </summary>
        /// <returns></returns>
        public Type GetValueType() => typeof(T);
    }

    [Serializable]
    public class ColorVariable : BaseVariable<Color>
    {

    }
    [Serializable]
    public class FloatVariable : BaseVariable<float>
    {
    }
    [Serializable]
    public class IntVariable : BaseVariable<int>
    {
        //    [SerializeField]
        //    private int val = default;
        //    public override object Value { get => val; set => val = (int)value; }
        //    public override Type GetValueType() => typeof(int);
    }
    [Serializable]
    public class StringVariable : BaseVariable<string>
    {
        //    [SerializeField]
        //    private string val = "";
        //    public override object Value { get => val; set => val = (string)value; }
        //    public override Type GetValueType() => typeof(string);
    }
    [Serializable]
    public class Vector2Variable : BaseVariable<Vector2>
    {
        //    [SerializeField]
        //    private Vector2 val = default;
        //    public override object Value { get => val; set => val = (Vector2)value; }
        //    public override Type GetValueType() => typeof(Vector2);
    }

    [Serializable]
    public class Vector3Variable : BaseVariable<Vector3>
    {
        //    [SerializeField]
        //    private Vector3 val = default;
        //    public override object Value { get => val; set => val = (Vector3)value; }
        //    public override Type GetValueType() => typeof(Vector3);
    }
    [Serializable]
    public class BoolVariable : BaseVariable<bool>
    {
        //    [SerializeField]
        //    private bool val = default;
        //    public override object Value { get => val; set => val = (bool)value; }
        //    public override Type GetValueType() => typeof(bool);
    }
}
