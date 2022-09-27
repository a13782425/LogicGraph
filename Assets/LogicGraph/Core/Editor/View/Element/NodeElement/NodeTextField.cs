using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.Logic.Editor
{
    public class NodeTextField : TextField, IInputElement
    {
        object IInputElement.value { get => value; set => this.value = (string)value; }

        public event Action<object> onValueChanged;

        public NodeTextField()
        {
            this.SetBaseFieldStyle();
            this.RegisterCallback<ChangeEvent<string>>((e) => OnValueChange(e.newValue));
        }
        private void OnValueChange(string newValue)
        {
            this.onValueChanged?.Invoke(newValue);
        }
    }
}
