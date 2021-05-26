﻿using System.IO;

namespace Imagin.Common.Linq
{
    public static class StreamExtensions
    {
        public static byte[] Array(this Stream input)
        {
            using (var result = new MemoryStream())
            {
                input.CopyTo(result);
                return result.ToArray();
            }
        }
    }
}