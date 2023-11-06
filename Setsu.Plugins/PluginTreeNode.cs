using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Setsu.Plugins.Abstractions;
using Setsu.Plugins.Abstractions.Attributes;
using Setsu.Results;
using Setsu.Results.Errors;

namespace Setsu.Plugins
{
    public sealed class PluginTreeNode
    {
        public string Id { get; private set; }
        public IPlugin? Instance { get; private set; } = null;
        public Type PluginType { get; private set; }
        public Type[] Dependencies { get; private set; } = [];

        public static PluginTreeNode FromType(Type type, IPluginAttribute attribute)
        {
            var deps = type.GetCustomAttributes(typeof(DependentOnAttribute<>), true).Select(x => x.GetType().GetGenericArguments().First()).ToArray();
            return new(attribute.Id, type, deps);
        }

        private PluginTreeNode(string id, Type type, Type[] dependencies)
        {
            Id = id;
            PluginType = type;
            Dependencies = dependencies;
        }

        public Result Construct(PluginTree tree)
        {
            foreach (var dep in Dependencies)
            {
                if (tree.GetFromType(dep) is not { } node)
                    continue;
                if (node.Construct(tree) is { IsSuccess: false } res)
                    return res;
            }
            try
            {
                Instance = (IPlugin)Activator.CreateInstance(PluginType);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new ExceptionError(ex));
            }
        }

        public Result RegisterServices(IServiceCollection collection)
        {
            if (Instance == null)
                throw new InvalidOperationException("The plugin needs to be constructed before calling RegisterServices.");
            return Instance.RegisterServices(collection);
        }

        public ValueTask<Result> InitializeAsync(IServiceProvider provider, CancellationToken cts)
        {
            if (Instance == null)
                throw new InvalidOperationException("The plugin needs to be constructed before calling InitializeAsync.");
            return Instance.InitializeAsync(provider, cts);
        }
    }
}
