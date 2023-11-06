using Microsoft.Extensions.DependencyInjection;
using Setsu.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Setsu.Plugins.Abstractions
{
    public abstract class PluginBase : IPlugin
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public virtual Result RegisterServices(IServiceCollection collection)
        {
            return Result.Ok();
        }

        public abstract ValueTask<Result> InitializeAsync(IServiceProvider provider, CancellationToken cts);
    }
}
