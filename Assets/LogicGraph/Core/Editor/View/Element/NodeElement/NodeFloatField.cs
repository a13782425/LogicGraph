using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public class NodeFloatField : FloatField, IInputElement
    {
        object IInputElement.value { get => value; set => this.value = (float)value; }

        public event Action<object> onValueChanged;

        public NodeFloatField()
        {
            this.SetBaseFieldStyle();
            this.RegisterCallback<ChangeEvent<float>>((e) => OnValueChange(e.newValue));
        }
        private void OnValueChange(float newValue)
        {
            this.onValueChanged?.Invoke(newValue);
        }
    }
}
