using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Setsu.Plugins.Abstractions
{
    public interface IPluginAttribute
    {
        public Type Type { get; }
        public string Id { get; }
    }
}
