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
        ///// <summary>
        ///// 逻辑图唯一Id
        ///// </summary>
        //private string _graphId = null;
        ///// <summary>
        ///// 当前显示逻辑图的唯一ID
        ///// </summary>
        //public string graphId
        //{
        //    get => _graphId;
        //    internal set
        //    {
        //        _graphId = value;
        //        if (string.IsNullOrWhiteSpace(value))
        //        {
        //            _summaryInfo = null;
        //            _categoryInfo = null;
        //            LogicUtils.UnloadObject(_logicGraph);
        //            _logicGraph = null;
        //            _editorData = null;
        //        }
        //        else
        //        {
        //            if (_logicGraph != null)
        //            {
        //                LogicUtils.UnloadObject(_logicGraph);
        //                _logicGraph = null;
        //            }
        //            _summaryInfo = LogicProvider.GetSummaryInfo(value);
        //            _categoryInfo = LogicProvider.GetCategoryInfo(_summaryInfo.GraphClassName);
        //            var res = LogicUtils.GetLogicGraph(_summaryInfo.AssetPath);
        //            _logicGraph = res.graph;
        //            _editorData = res.editorData;
        //        }
        //    }
        //}
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
            }
        }

 
    }

    ///// <summary>
    ///// 序列化Vector2
    ///// </summary>
    //[Serializable]
    //public class SVector2
    //{
    //    [SerializeField]
    //    public float x;
    //    [SerializeField]
    //    public float y;
    //    public SVector2() : this(0, 0) { }
    //    public SVector2(float x, float y)
    //    {
    //        this.x = x;
    //        this.y = y;
    //    }


    //    #region 其他类型->本类

    //    //隐式转换
    //    public static implicit operator SVector2(Vector2 vec)
    //    {
    //        return new SVector2(vec.x, vec.y);
    //    }
    //    //隐式转换
    //    public static implicit operator SVector2(Vector3 vec)
    //    {
    //        return new SVector2(vec.x, vec.y);
    //    }

    //    #endregion

    //    #region 本类->其他类型

    //    //隐式转换
    //    public static implicit operator Vector2(SVector2 svec)
    //    {
    //        return new Vector2(svec.x, svec.y);
    //    }

    //    //隐式转换
    //    public static implicit operator Vector3(SVector2 svec)
    //    {
    //        return new Vector3(svec.x, svec.y, 0);
    //    }

    //    #endregion
    //}
    ///// <summary>
    ///// 序列化Vector3
    ///// </summary>
    //[Serializable]
    //public class SVector3
    //{
    //    [SerializeField]
    //    public float x;
    //    [SerializeField]
    //    public float y;
    //    [SerializeField]
    //    public float z;
    //    public SVector3() : this(0, 0) { }
    //    public SVector3(float x, float y) : this(x, y, 0) { }
    //    public SVector3(float x, float y, float z)
    //    {
    //        this.x = x;
    //        this.y = y;
    //        this.z = z;
    //    }

    //    #region 其他类型->本类

    //    //隐式转换
    //    public static implicit operator SVector3(Vector2 vec)
    //    {
    //        return new SVector3(vec.x, vec.y);
    //    }
    //    //隐式转换
    //    public static implicit operator SVector3(Vector3 vec)
    //    {
    //        return new SVector3(vec.x, vec.y, vec.z);
    //    }

    //    #endregion

    //    #region 本类->其他类型

    //    //隐式转换
    //    public static implicit operator Vector2(SVector3 svec)
    //    {
    //        return new Vector2(svec.x, svec.y);
    //    }

    //    //隐式转换
    //    public static implicit operator Vector3(SVector3 svec)
    //    {
    //        return new Vector3(svec.x, svec.y, svec.z);
    //    }

    //    #endregion
    //}


    ///// <summary>
    ///// 序列化Vector3
    ///// </summary>
    //[Serializable]
    //public class SColor
    //{
    //    [SerializeField]
    //    public float r;
    //    [SerializeField]
    //    public float g;
    //    [SerializeField]
    //    public float b;
    //    [SerializeField]
    //    public float a;
    //    public SColor() : this(0, 0, 0) { }
    //    public SColor(float r, float g, float b) : this(r, g, b, 1) { }
    //    public SColor(float r, float g, float b, float a)
    //    {
    //        this.r = r;
    //        this.g = g;
    //        this.b = b;
    //        this.a = a;
    //    }

    //    #region 其他类型->本类

    //    //隐式转换
    //    public static implicit operator SColor(Color color)
    //    {
    //        return new SColor(color.r, color.g, color.b, color.a);
    //    }

    //    #endregion

    //    #region 本类->其他类型

    //    //隐式转换
    //    public static implicit operator Color(SColor color)
    //    {
    //        return new Color(color.r, color.g, color.b, color.a);
    //    }

    //    #endregion
    //}
}
