using Imagin.Common.Analytics;
using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace Imagin.Common
{
    public static class Resources
    {
        public static ResourceDictionary Load(string filePath)
        {
            var result = default(ResourceDictionary);
            using (var fileStream = File.OpenRead(filePath))
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                result = (ResourceDictionary)XamlReader.Load(fileStream);
            }
            return result;
        }

        public static Result TryLoad(string filePath, out ResourceDictionary result)
        {
            try
            {
                result = Load(filePath);
                return new Success();
            }
            catch (Exception e)
            {
                result = default;
                return new Error(e);
            }
        }

        //..................................................................................................

        public static ResourceDictionary Load(Uri fileUri)
        {
            using (var stream = Application.GetResourceStream(fileUri).Stream)
                return (ResourceDictionary)XamlReader.Load(stream);
        }

        //..................................................................................................

        public static void Save(string assemblyName, string resourcePath, string destinationPath)
        {
            using (var fileStream = File.Create(destinationPath))
            using (var stream = Application.GetResourceStream(Uri(assemblyName, resourcePath)).Stream)
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
        }

        //..................................................................................................

        public static Stream Stream(Uri uri) => Application.GetResourceStream(uri).Stream;

        //..................................................................................................

        public static Uri Uri(string assemblyName, string relativePath) => new Uri($"pack://application:,,,/{assemblyName};component/{relativePath}", UriKind.Absolute);
    }
}