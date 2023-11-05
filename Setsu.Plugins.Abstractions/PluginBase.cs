using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setsu.Plugins.Abstractions
{
    public abstract class PluginBase : IPlugin
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public virtual void RegisterServices(IServiceCollection collection)
        {

        }

        public abstract void Initialize(IServiceProvider provider);
    }
}
