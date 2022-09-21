using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Editor
{
    /// <summary>
    /// 逻辑图操作数据
    /// 编辑器内部使用
    /// </summary>
    internal sealed class GraphOperateData
    {
        private LGSummaryInfo _summaryInfo = null;
        /// <summary>
        /// 当前展示逻辑图的简介
        /// </summary>
        public LGSummaryInfo summaryInfo => _summaryInfo;
        private LGCategoryInfo _categoryInfo = null;
        /// <summary>
        /// 逻辑图分类信息数据
        /// </summary>
        public LGCategoryInfo categoryInfo => _categoryInfo;
        private BaseLogicGraph _logicGraph = null;
        /// <summary>
        /// 逻辑图数据
        /// </summary>
        public BaseLogicGraph logicGraph => _logicGraph;

        private GraphEditorData _editorData = null;
        /// <summary>
        /// 逻辑图编辑器数据
        /// </summary>
        public GraphEditorData editorData => _editorData;

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh(string onlyId)
        {
            LogicUtils.UnloadObject(_logicGraph);
            if (string.IsNullOrWhiteSpace(onlyId))
            {
                _summaryInfo = null;
                _categoryInfo = null;
                _logicGraph = null;
                _editorData = null;
            }
            else
            {
                _summaryInfo = LogicProvider.GetSummaryInfo(onlyId);
                _categoryInfo = LogicProvider.GetCategoryInfo(_summaryInfo.GraphClassName);
                var res = LogicUtils.GetLogicGraph(_summaryInfo.AssetPath);
                _logicGraph = res.graph;
                _editorData = res.editorData;
                _editorData.target = _logicGraph;
            }
        }
    }

}
