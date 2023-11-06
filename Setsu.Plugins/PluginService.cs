using Setsu.Results;
using Setsu.Results.Errors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Setsu.Plugins
{
    public sealed class PluginService
    {
        public (PluginTree, IError[]) LoadTree(string folder, SearchMethod method)
        {
            return method switch
            {
                SearchMethod.SingleFolder => SearchFiles(folder),
                SearchMethod.NamedFolders => SearchFolders(folder),
                _ => throw new ArgumentOutOfRangeException(nameof(folder)),
            };
        }

        private (PluginTree, IError[]) SearchFiles(string folder)
        {
            var tree = new PluginTree();
            var errors = new List<IError>();
            foreach (var assembly in Directory.EnumerateFiles(folder, "*.dll"))
            {
                try
                {
                    var asm = Assembly.LoadFrom(assembly);
                    tree.AddFromAssembly(asm);
                }
                catch (Exception ex)
                {
                    errors.Add(new ExceptionError(ex));
                }
            }
            return (tree, errors.ToArray());
        }

        private (PluginTree, IError[]) SearchFolders(string folder)
        {
            var tree = new PluginTree();
            var errors = new List<IError>();
            foreach (var dir in Directory.EnumerateDirectories(folder))
            {
                foreach (var file in Directory.EnumerateFiles(dir, "*.dll"))
                {
                    try
                    {
                        var asm = Assembly.LoadFrom(file);
                        tree.AddFromAssembly(asm);
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new ExceptionError(ex));
                    }
                }
            }
            return (tree, errors.ToArray());
        }
    }
}
