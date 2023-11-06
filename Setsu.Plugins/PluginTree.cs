using Microsoft.Extensions.DependencyInjection;
using Setsu.Plugins.Abstractions;
using Setsu.Plugins.Abstractions.Attributes;
using Setsu.Results;
using Setsu.Results.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Setsu.Plugins
{
    public class PluginTree
    {
        private readonly Dictionary<string, PluginTreeNode> nodes = [];
        private readonly Dictionary<Type, string> typeDict = [];

        public PluginTreeNode? GetFromId(string id)
        {
            nodes.TryGetValue(id, out var node);
            return node;
        }
        public PluginTreeNode? GetFromType(Type type)
        {
            if (!typeDict.TryGetValue(type, out var id))
                return null;
            nodes.TryGetValue(id, out var node);
            return node;
        }

        public Result Construct()
        {
            if (nodes.Count == 0)
                return Result.Ok();
            var res = new List<IError>();
            foreach (var node in nodes.Values)
            {
                if (node.Construct(this) is { IsSuccess: false } result)
                    res.Add(result.Error);
            }
            return res.Count == 0 ? Result.Ok() : Result.Fail(new AggregateError([.. res]));
        }

        public Result RegisterServices(IServiceCollection collection)
        {
            if (nodes.Count == 0)
                return Result.Ok();
            var res = new List<IError>();
            foreach (var node in nodes.Values)
            {
                if (node.RegisterServices(collection) is { IsSuccess: false } result)
                    res.Add(result.Error);
            }
            return res.Count == 0 ? Result.Ok() : Result.Fail(new AggregateError([..res]));
        }
        public async ValueTask<Result> InitializeAsync(IServiceProvider provider, CancellationToken cts)
        {
            if (nodes.Count == 0)
                return Result.Ok();
            var length = nodes.Count;
            var tasks = new ValueTask<Result>[length];
            var list = (IList<KeyValuePair<string, PluginTreeNode>>)nodes;
            for (int i = 0; i < length; i++)
            {
                tasks[i] = list[i].Value.InitializeAsync(provider, cts);
            }
            var res = new List<IError>();
            foreach (var node in tasks)
            {
                if (await node is { IsSuccess: false } result)
                    res.Add(result.Error);
            }
            return res.Count == 0 ? Result.Ok() : Result.Fail(new AggregateError([.. res]));
        }

        public void AddFromAssembly(Assembly assembly)
        {
            foreach (var attr in assembly.GetCustomAttributes(typeof(PluginAttribute<>)).Cast<IPluginAttribute>())
            {
                var type = attr.GetType().GetGenericArguments()[0];
                var node = PluginTreeNode.FromType(type, attr);
                typeDict.Add(type, attr.Id);
                nodes.Add(attr.Id, node);
            }
        }
    }
}
