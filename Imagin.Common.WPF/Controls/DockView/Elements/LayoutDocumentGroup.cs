using Imagin.Common.Collections;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class LayoutDocumentGroup : LayoutElement
    {
        [XmlIgnore]
        public readonly Document[] Documents;

        bool @default = false;
        [XmlAttribute]
        public bool Default
        {
            get => @default;
            set => this.Change(ref @default, value);
        }

        public LayoutDocumentGroup() : base() { }

        public LayoutDocumentGroup(params Document[] documents) : base() => Documents = documents;

        public LayoutDocumentGroup(IEnumerable<Document> documents) : this(documents.ToArray()) { }

        public LayoutDocumentGroup(ICollect input) : this(input.ToArray<Document>()) { }
    }
}