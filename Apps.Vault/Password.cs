using Imagin.Common;
using Imagin.Common.Data;
using System;
using System.Xml.Serialization;

namespace Vault
{
    [DisplayName(nameof(Password))]
    [Serializable]
    public class Password : NamedObject
    {
        Category category = null;
        [Featured]
        public Category Category
        {
            get => category;
            set
            {
                this.Change(ref category, value);
                this.Changed(() => CategoryName);
            }
        }

        [Hidden]
        [XmlIgnore]
        public string CategoryName => category?.Name ?? "General";

        string text = string.Empty;
        [DisplayName("Password")]
        [StringFormat(StringFormat.Password)]
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        public Password() : base() { }
    }
}