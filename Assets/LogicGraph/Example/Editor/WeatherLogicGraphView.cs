using Game.Logic.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Example.Editor
{
    [LogicGraph("天气逻辑图", typeof(WeatherLogicGraph))]
    public class WeatherLogicGraphView : LogicGraphView
    {
    }

}
