﻿using System;

namespace Imagin.Common.Linq
{
    public static class VersionExtensions
    {
        public static Version Coerce(this Version input, Version maximum, Version minimum) => input > maximum ? maximum : (input < minimum ? minimum : input);
    }
}