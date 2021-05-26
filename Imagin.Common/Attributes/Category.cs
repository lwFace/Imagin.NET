﻿using System;

namespace Imagin.Common
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CategoryAttribute : Attribute
    {
        public readonly string Category;

        public CategoryAttribute(string category = "") : base() => Category = category;

        public CategoryAttribute(object category) : this($"{category}") { }
    }
}