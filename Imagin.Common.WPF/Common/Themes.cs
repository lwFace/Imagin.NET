using Imagin.Common.Configuration;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Imagin.Common
{
    public sealed class Themes : ResourceDictionary
    {
        [Serializable]
        public enum Types { Blaze, Chocolate, Dark, Jungle, Light, Midnight, Violet }

        Collection<Theme> Resources { get; set; } = new Collection<Theme>();

        class Theme : Dictionary<Types, Uri>
        {
            public Theme(string assemblyName) => Enum.GetValues(typeof(Types)).ForEach(i => Add((Types)i, Common.Resources.Uri(assemblyName, $"Theme/{(Types)i}.xaml")));
        }

        public Themes() : base()
        {
            if (Get.Exists<Themes>())
                throw new SingleInstanceException();

            Get.New<Themes>(this);
            Resources.Add(new Theme(AssemblyData.Name));
        }

        public void Change(Types theme)
        {
            if (Resources == null)
                return;

            BeginInit();
            MergedDictionaries.Clear();

            foreach (var i in Resources)
            {
                var result = new ResourceDictionary();
                result.Source = i[theme];
                MergedDictionaries.Add(result);
            }

            EndInit();

            /*
            If coming from a file path...

            var theme = default(ResourceDictionary);
            var result = Configuration.Resources.TryGetResourceDictionary(path, out theme);

            if (theme != null)
            {
                provider.MergeResources(theme);
                Theme = path;
                System.Diagnostics.Debug.WriteLine("Theme is not null => {0}".F(path));
            }

            return result;
            */
        }
    }
}