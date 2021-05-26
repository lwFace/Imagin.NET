using Imagin.Common.Analytics;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Common.Configuration
{
    public class Plugins : ItemCollection
    {
        #region Properties

        public event EventHandler<EventArgs<IPlugin>> Enabled;

        protected readonly Dictionary<string, IPlugin> Source = new Dictionary<string, IPlugin>();

        #endregion

        #region Plugins

        public Plugins(string path) : base(path, new Filter(ItemType.File, "dll")) { }

        #endregion

        #region Methods

        /// <summary>
        /// Search for and return plugin type defined in specified assembly.
        /// </summary>
        static Type GetType(Assembly Assembly)
        {
            foreach (var i in Assembly.GetTypes())
            {
                if (i.Implements<IPlugin>())
                    return i;
            }
            throw new DllNotFoundException("Couldn't find plugin type in target assembly.");
        }

        /// <summary>
        /// Gets a new instance of <see cref="IPlugin"/> interface based on given assembly context.
        /// </summary>
        static IPlugin New(AssemblyContext assemblyContext)
        {
            return (IPlugin)Activator.CreateInstance(GetType(assemblyContext.Assembly));
        }

        protected override void OnAdded(Item Item)
        {
            base.OnAdded(Item);

            var result = TryInstall(Item.Path, out IPlugin i);
            if (result != null)
            {
                Remove(Item);
            }
            else
            {
                i.Enabled += OnEnabled;
                Source[Item.Path] = i;
            }
        }

        protected override void OnRemoved(Item Item)
        {
            base.OnAdded(Item);
            if (Source.ContainsKey(Item.Path))
            {
                var i = Source[Item.Path];
                i.Enabled -= OnEnabled;
                Uninstall(i);
                Source.Remove(Item.Path);
            }
        }

        protected virtual void OnEnabled(object sender, EventArgs e)
        {
            Enabled?.Invoke(this, new EventArgs<IPlugin>(sender as IPlugin));
        }

        /// <summary>
        /// Load a plugin from a file path.
        /// </summary>
        public static void Install(string filePath)
        {
            Install(filePath, out IPlugin plugin);
        }

        /// <summary>
        /// Load a plugin using file at given path.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Plugin"></param>
        public static void Install(string Path, out IPlugin Plugin)
        {
            Plugin = default(IPlugin);
            var g = Guid.NewGuid();
            var a = new AssemblyContext(g, Assembly.Load(System.IO.File.ReadAllBytes(Path)), AppDomain.CreateDomain(g.ToString()));
            Plugin = New(a);
        }

        /// <summary>
        /// Attempt to load a plugin from a file path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Result TryInstall(string filePath)
        {
            return TryInstall(filePath, out IPlugin Plugin);
        }

        /// <summary>
        /// Attempt to load a plugin from a file path
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static Result TryInstall(string Path, out IPlugin Plugin)
        {
            Plugin = default(IPlugin);
            try
            {
                Install(Path, out Plugin);
                return new Success();
            }
            catch (Exception e)
            {
                return new Error(e);
            }
        }

        /// <summary>
        /// Remove, and dispose of, given <see cref="Plugin"/> instance.
        /// </summary>
        public static Result Uninstall(IPlugin Plugin)
        {
            if (Plugin.IsEnabled)
                Plugin.IsEnabled = false;

            //Unload AppDomain that hosts plugin

            return null;
        }

        #endregion
    }
}