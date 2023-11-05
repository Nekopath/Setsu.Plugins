using Microsoft.Extensions.DependencyInjection;

namespace Setsu.Plugins.Abstractions
{
    public interface IPlugin
    {
        public string Name { get; }
        public string Description { get; }
        public void RegisterServices(IServiceCollection collection);
        public void Initialize(IServiceProvider provider);
    }
}
