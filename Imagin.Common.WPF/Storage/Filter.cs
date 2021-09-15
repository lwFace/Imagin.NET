using Imagin.Common.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Imagin.Common.Storage
{
    public sealed class Filter
    {
        public readonly IEnumerable<string> Extensions;

        public readonly ItemType Types;

        public Filter(ItemType types = ItemType.Drive | ItemType.File | ItemType.Folder | ItemType.Shortcut, params string[] extensions)
        {
            Types = types;
            Extensions = extensions?.Length > 0 ? extensions.Select(i => i.ToLower()) : null;
        }

        public bool Evaluate(string path, ItemType itemType)
        {
            return itemType == ItemType.File ? (Extensions == null || Extensions.Contains(Path.GetExtension(path).TrimStart('.').ToLower())) && Types.HasFlag(ItemType.File) : Types.HasFlag(itemType);
        }
    }
}