using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic.Runtime
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class InputAttribute : Attribute
    {
        public string name;

        public InputAttribute(string name = null)
        {
            this.name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class OutputAttribute : Attribute
    {
        public string name;

        public OutputAttribute(string name = null, bool allowMultiple = true)
        {
            this.name = name;
        }
    }
}
