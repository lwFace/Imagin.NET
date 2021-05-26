﻿using System;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public abstract class LayoutElement : Base
    {
        double minHeight = double.NaN;
        [XmlAttribute]
        public double MinHeight
        {
            get => minHeight;
            set => this.Change(ref minHeight, value);
        }

        LayoutLength height = LayoutLength.Star;
        [XmlAttribute]
        public string Height
        {
            get => height;
            set => this.Change(ref height, value);
        }

        double maxHeight = double.NaN;
        [XmlAttribute]
        public double MaxHeight
        {
            get => maxHeight;
            set => this.Change(ref maxHeight, value);
        }

        double minWidth = double.NaN;
        [XmlAttribute]
        public double MinWidth
        {
            get => minWidth;
            set => this.Change(ref minWidth, value);
        }

        LayoutLength width = LayoutLength.Star;
        [XmlAttribute]
        public string Width
        {
            get => width;
            set => this.Change(ref width, value);
        }

        double maxWidth = double.NaN;
        [XmlAttribute]
        public double MaxWidth
        {
            get => maxWidth;
            set => this.Change(ref maxWidth, value);
        }

        public LayoutElement() : base() { }
    }
}