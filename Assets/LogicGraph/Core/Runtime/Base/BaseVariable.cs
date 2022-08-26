using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Runtime
{
    [Serializable]
    public abstract class BaseVariable
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

        /// <summary>
        /// 当前变量的值
        /// </summary>
        public virtual object Value { get; set; }
    }
}
