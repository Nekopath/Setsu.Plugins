using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setsu.Plugins.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class DependentOnAttribute<T> : Attribute where T: IPlugin
    {
        public Type Type { get; } = typeof(T);
    }
}
