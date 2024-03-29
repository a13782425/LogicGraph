﻿using Game.Logic.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Logic.Editor
{
    [Serializable]
    internal sealed class GraphEditorData
    {
        private readonly static DateTime MINI_TIME = new DateTime(2022, 1, 1);

        public BaseLogicGraph target { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [SerializeField]
        private string a = "";
        /// <summary>
        /// 标题
        /// </summary>
        public string LogicName { get => a; set => a = value; }

        /// <summary>
        /// 当前图的节点编辑器数据
        /// </summary>
        [SerializeField]
        private List<NodeEditorData> b = new List<NodeEditorData>();
        /// <summary>
        /// 当前图的节点编辑器数据
        /// </summary>
        public List<NodeEditorData> NodeDatas => b;

        /// <summary>
        /// 当前图的分组编辑器数据
        /// </summary>
        [SerializeField]
        private List<GroupEditorData> c = new List<GroupEditorData>();
        /// <summary>
        /// 当前图的分组编辑器数据
        /// </summary>
        public List<GroupEditorData> GroupDatas => c;

        /// <summary>
        /// 最后一次Format位置
        /// </summary>
        [SerializeField]
        private string d = "";
        /// <summary>
        /// 最后一次Format位置
        /// </summary>
        [SerializeField]
        public string LastFormatPath { get => d; set => d = value; }

        /// <summary>
        /// 当前图坐标
        /// </summary>
        [SerializeField]
        private Vector3 e = Vector3.zero;
        /// <summary>
        /// 当前图坐标
        /// </summary>
        [SerializeField]
        public Vector3 Pos { get => e; set => e = value; }

        /// <summary>
        /// 当前图的缩放
        /// </summary>
        [SerializeField]
        private Vector3 f = Vector3.one;
        /// <summary>
        /// 当前图的缩放
        /// </summary>
        public Vector3 Scale { get => f; set => f = value; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SerializeField]
        private int g = 0;
        /// <summary>
        /// 创建时间DateTime.Now.ToString("yyyy.MM.dd");
        /// </summary>
        public DateTime CreateTime { get => MINI_TIME.AddMinutes(g); set => g = (int)(value - MINI_TIME).TotalMinutes; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [SerializeField]
        private int h = 0;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get => MINI_TIME.AddMinutes(h); set => h = (int)(value - MINI_TIME).TotalMinutes; }

        /// <summary>
        /// 描述
        /// </summary>
        [SerializeField]
        private string i = "";
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get => i; set => i = value; }

        /// <summary>
        /// 当前图的变量编辑器数据
        /// </summary>
        [SerializeField]
        private List<VarEditorData> j = new List<VarEditorData>();
        /// <summary>
        /// 当前图的变量编辑器数据
        /// </summary>
        public List<VarEditorData> VarDatas => j;

        /// <summary>
        /// 当前图的变量节点数据
        /// </summary>
        [SerializeField]
        private List<VarNodeEditorData> k = new List<VarNodeEditorData>();
        /// <summary>
        /// 当前图的变量节点数据
        /// </summary>
        public List<VarNodeEditorData> VarNodeDatas => k;



        /// <summary>
        /// 获取变量节点缓存
        /// </summary>
        /// <param name="onlyId"></param>
        /// <returns></returns>
        internal VarNodeEditorData GetVarNodeEditorData(int onlyId) => VarNodeDatas.FirstOrDefault(a => a.OnlyId == onlyId);
        /// <summary>
        /// 获取节点缓存
        /// </summary>
        /// <param name="onlyId"></param>
        /// <returns></returns>
        internal NodeEditorData GetNodeEditorData(int onlyId) => NodeDatas.FirstOrDefault(a => a.OnlyId == onlyId);
    }
}
