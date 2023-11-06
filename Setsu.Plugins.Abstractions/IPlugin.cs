using Microsoft.Extensions.DependencyInjection;
using Setsu.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Setsu.Plugins.Abstractions
{
    public interface IPlugin
    {
        public string Name { get; }
        public string Description { get; }
        public Result RegisterServices(IServiceCollection collection);
        public ValueTask<Result> InitializeAsync(IServiceProvider provider, CancellationToken cts);
    }
}
