using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setsu.Plugins.Abstractions
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed class PluginAttribute(params Type[] types) : Attribute
    {
        public Type[] Types { get; set; } = types;
    }
}
