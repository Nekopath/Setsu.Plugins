using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setsu.Plugins.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class PluginAttribute<T>(string uniqueId) : Attribute where T : IPlugin
    {
        public Type Type { get; } = typeof(T);
        public string Id { get; } = uniqueId;
    }
}
