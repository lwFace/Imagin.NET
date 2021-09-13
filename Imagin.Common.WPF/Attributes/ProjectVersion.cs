using System;

namespace Imagin.Common
{
    public class ProjectVersionAttribute : Attribute
    {
        public readonly Version Value;

        public ProjectVersionAttribute(int a = 0, int b = 0, int c = 0, int d = 0) => Value = new Version(a, b, c, d);
    }
}