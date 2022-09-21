using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Editor
{
    [Serializable]
    internal sealed class VarEditorData
    {
        public IVariable target { get; set; }

        /// <summary>
        /// 变量名
        /// </summary>
        [SerializeField]
        private string a = null;
        public string Name { get => a; set => a = value; }

        /// <summary>
        /// 是否导出
        /// </summary>
        [SerializeField]
        private bool b = false;
        /// <summary>
        /// 是否导出
        /// </summary>
        public bool Export { get => b; set => b = value; }

        /// <summary>
        /// 可以修改变量名
        /// </summary>
        [SerializeField]
        private bool c = true;
        /// <summary>
        /// 可以修改变量名
        /// </summary>
        public bool CanRename { get => c; set => c = value; }

        /// <summary>
        /// 可以删除
        /// </summary>
        [SerializeField]
        private bool d = true;
        /// <summary>
        /// 可以删除
        /// </summary>
        public bool CanDelete { get => d; set => d = value; }
    }
}
