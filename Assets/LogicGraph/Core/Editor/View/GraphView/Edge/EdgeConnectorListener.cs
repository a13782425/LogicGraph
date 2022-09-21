﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game.Logic.Editor
{
    public sealed class EdgeConnectorListener : IEdgeConnectorListener
    {
        public readonly BaseGraphView graphView;
        public EdgeConnectorListener(BaseGraphView graphView)
        {
            this.graphView = graphView;
        }
        public void OnDrop(GraphView graphView, Edge edge)
        {
            var edgeView = edge as EdgeView;

            if (edgeView?.input == null || edgeView?.output == null)
                return;
            //bool wasOnTheSamePort = false;
            graphView.AddElement(edgeView);
            Port output = edgeView.output;
            Port input = edgeView.input;
            input.Connect(edgeView);
            output.Connect(edgeView);
        }

        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
        }
    }
}
