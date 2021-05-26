using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Storage
{
    public static class Path
    {
        public const string DefaultCloneFormat = "{0} [{1}]";

        /// <summary>
        /// See <see cref="System.IO.Path.AltDirectorySeparatorChar"/>.
        /// </summary>
        public static readonly char AltDirectorySeparatorChar = System.IO.Path.AltDirectorySeparatorChar;

        /// <summary>
        /// See <see cref="System.IO.Path.DirectorySeparatorChar"/>.
        /// </summary>
        public static readonly char DirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;

        /// <summary>
        /// See <see cref="System.IO.Path.ChangeExtension(string, string)"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string ChangeExtension(string path, string extension) => System.IO.Path.ChangeExtension(path, extension);

        /// <summary>
        /// Gets a clone of the given path using the given name format.
        /// </summary>
        public static string Clone(string path, string nameFormat, Predicate<string> exists)
        {
            var parent = GetDirectoryName(path);

            var extension = GetExtension(path);
            var name = GetFileNameWithoutExtension(path);

            var n = name;
            string result() => $@"{parent}\{n}{extension}".Replace(@"\\", @"\");

            var i = 0;
            while (exists(result()))
            {
                n = nameFormat.F(name, i);
                i++;
            }

            return result();
        }

        /// <summary>
        /// See <see cref="System.IO.Path.Combine(string[])"/>.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths) => System.IO.Path.Combine(paths);

        /// <summary>
        /// See <see cref="System.IO.Path.GetDirectoryName(string)"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string path) => System.IO.Path.GetDirectoryName(path);

        /// <summary>
        /// See <see cref="System.IO.Path.GetExtension(string)"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetExtension(string path) => System.IO.Path.GetExtension(path);

        /// <summary>
        /// Enumerates the given <see cref="string"/> starting with the last <see cref="char"/>; gets everything after first period, if one is found.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetFirstExtension(string input)
        {
            if (input.NullOrEmpty())
                return null;

            var f = string.Empty;
            for (var i = input.Length - 1; i >= 0; i--)
            {
                if (input[i] == '.')
                    return f.Empty() ? null : f;

                f = $"{input[i]}{f}";
            }
            return null;
        }

        /// <summary>
        /// See <see cref="System.IO.Path.GetFileName(string)"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName(string path) => System.IO.Path.GetFileName(path);

        /// <summary>
        /// See <see cref="System.IO.Path.GetFileNameWithoutExtension(string)"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string path) => System.IO.Path.GetFileNameWithoutExtension(path);

        /// <summary>
        /// See <see cref="System.IO.Path.GetFullPath(string)"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFullPath(string path) => System.IO.Path.GetFullPath(path);

        /// <summary>
        /// See <see cref="System.IO.Path.GetInvalidFileNameChars"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static char[] GetInvalidFileNameChars() => System.IO.Path.GetInvalidFileNameChars();

        /// <summary>
        /// See <see cref="System.IO.Path.GetInvalidPathChars"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static char[] GetInvalidPathChars() => System.IO.Path.GetInvalidPathChars();

        /// <summary>
        /// See <see cref="System.IO.Path.GetPathRoot(string)"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPathRoot(string path) => System.IO.Path.GetPathRoot(path);

        /// <summary>
        /// See <see cref="System.IO.Path.GetRandomFileName"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRandomFileName() => System.IO.Path.GetRandomFileName();

        /// <summary>
        /// See <see cref="System.IO.Path.GetTempFileName"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetTempFileName() => System.IO.Path.GetTempFileName();

        /// <summary>
        /// See <see cref="System.IO.Path.GetTempPath"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetTempPath() => System.IO.Path.GetTempPath();

        /// <summary>
        /// See <see cref="System.IO.Path.HasExtension(string)"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool HasExtension(string path) => System.IO.Path.HasExtension(path);

        /// <summary>
        /// See <see cref="System.IO.Path.IsPathRooted(string)"/>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPathRooted(string path) => System.IO.Path.IsPathRooted(path);
    }
}