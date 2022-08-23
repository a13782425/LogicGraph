using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Game.Logic.Editor
{
    [CustomEditor(typeof(LogicGraphImporter))]
    [CanEditMultipleObjects]
    internal class LogicGraphImporterInspector : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            UnityEditor.EditorGUI.EndDisabledGroup();
        }
   
    }
}
