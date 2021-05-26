using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Storage;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    public class Layouts : ItemCollection
    {
        static readonly XmlSerializer Serializer = new XmlSerializer(typeof(Layout), new XmlAttributeOverrides(), Array<Type>.New(typeof(DoubleSize), typeof(LayoutLength), typeof(LayoutUnitType), typeof(Layout), typeof(LayoutDocumentGroup), typeof(LayoutElement), typeof(LayoutGroup), typeof(LayoutPanel), typeof(LayoutPanelGroup), typeof(LayoutWindow), typeof(Point2D)), new XmlRootAttribute(nameof(Layout)), $"{nameof(Imagin)}.{nameof(Common)}.{nameof(Controls)}");

        /// ..................................................................................................................

        public Layouts() : base(new Filter(ItemType.File, "xml")) { }

        /// ..................................................................................................................

        Result<Layout> Apply(string filePath)
        {
            try
            {
                var layout = (Layout)Serializer.Deserialize(new StringReader(Storage.File.Long.ReadAllText(filePath, System.Text.Encoding.UTF8)));
                return new Success<Layout>(layout);
            }
            catch (Exception e)
            {
                return new Error<Layout>(e);
            }
        }

        Result<Layout> Apply(Stream stream)
        {
            Result<Layout> result = null;

            try
            {
                var layout = (Layout)Serializer.Deserialize(stream);
                result = new Success<Layout>(layout);
            }
            catch (Exception e)
            {
                result = new Error<Layout>(e);
            }
            finally
            {
                stream?.Close();
                stream?.Dispose();
            }

            return result;
        }

        /// ..................................................................................................................

        public async Task<Layout> Apply(string layout, Uri defaultLayout)
        {
            Console.WriteLine("Layouts.Apply: Start");
            Result<Layout> result = null;
            await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                Console.WriteLine("await Application.Current.Dispatcher.BeginInvoke...");
                result = Apply(layout);
                Console.WriteLine("result = Apply(layout);");
                result = result ? result : Apply(Resources.Stream(defaultLayout));
                Console.WriteLine("result = result ? result : Apply(Resources.Stream(defaultLayout));");
            });
            Console.WriteLine("Layouts.Apply: End");
            return result.Data;
        }

        /// ..................................................................................................................

        public async Task<Result> Save(string nameWithoutExtension)
        {
            return await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                var filePath = $@"{Path}\{nameWithoutExtension}.xml";

                var result = default(Result);
                var directoryName = System.IO.Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName))
                {
                    try
                    {
                        Directory.CreateDirectory(directoryName);
                        result = new Success();
                    }
                    catch (Exception e)
                    {
                        result = new Error(e);
                    }

                    if (!result)
                        return result;
                }

                try
                {
                    Serializer.Serialize(new StreamWriter(filePath), Get.Current<DockView>().Convert());
                    result = new Success();
                }
                catch (Exception e)
                {
                    result = new Error(e.Message);
                }
                return result;
            });
        }
    }
}