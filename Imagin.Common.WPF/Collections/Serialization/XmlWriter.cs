using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Imagin.Common.Collections.Serialization
{
    public class XmlWriter<T> : Writer<T>
    {
        readonly string Root;

        //..............................................................................................

        XmlSerializer serializer = null;
        XmlSerializer Serializer => serializer ?? (serializer = new XmlSerializer(typeof(List<T>), new XmlAttributeOverrides(), AllTypes, new XmlRootAttribute(Root), string.Empty));

        Type[] allTypes = null;
        Type[] AllTypes => allTypes ?? (allTypes = GetTypes().Distinct().ToArray());

        readonly Type[] Types;

        //..............................................................................................

        Encoding encoding = Encoding.ASCII;
        public Encoding Encoding
        {
            get => encoding;
            set => this.Change(ref encoding, value);
        }

        bool encrypt = false;
        public bool Encrypt
        {
            get => encrypt;
            set => this.Change(ref encrypt, value);
        }

        Encryption encryption = new Encryption();
        public Encryption Encryption
        {
            get => encryption;
            set => this.Change(ref encryption, value);
        }

        //..............................................................................................

        public XmlWriter(string root, string filePath, Limit limit = default, params Type[] types) : base(filePath, limit)
        {
            Root = root;
            Types = types;
        }

        //..............................................................................................

        IEnumerable<Type> GetTypes()
        {
            foreach (var i in GetTypes(typeof(T)))
                yield return i;

            foreach (var i in Types)
            {
                foreach (var j in GetTypes(i))
                    yield return j;
            }
        }

        IEnumerable<Type> GetTypes(Type input)
        {
            yield return input;
            foreach (var i in input.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                if (i.HasAttribute<XmlIgnoreAttribute>() || i.PropertyType.IsInterface || i.PropertyType.IsValueType || i.PropertyType.Equals(typeof(string)))
                    continue;

                foreach (var j in GetTypes(i.PropertyType))
                    yield return j;
            }
        }

        //..............................................................................................

        public override Result Deserialize(string filePath, out object result)
        {
            StringReader reader = null;
            try
            {
                //1. Get an XML string from the file
                string text = Storage.File.Long.ReadAllText(filePath, encoding.Convert());

                if (encrypt)
                {
                    if (!encryption.Password.NullOrEmpty())
                    {
                        //2a. Decrypt the XML string
                        text = encryption.Decrypt(text);
                    }
                }

                reader = new StringReader(text);

                //2b. Deserialize the XML string
                result = Serializer.Deserialize(reader);
                reader.Close();
                return new Success();
            }
            catch (Exception e)
            {
                result = Enumerable.Empty<T>();
                reader?.Close();

                Console.WriteLine($"{nameof(XmlWriter<T>)}.{nameof(Deserialize)}: {e.Message} {e.InnerException?.Message}");
                return new Error(e);
            }
        }

        public override Result Serialize(string filePath, object input)
        {
            Result result = null;

            var data = new List<T>(input as IEnumerable<T>);
            StringWriter writer = new StringWriter();

            try
            {
                Storage.Folder.Long.Create(Path.GetDirectoryName(filePath));

                //1. Serialize to an XML string
                Serializer.Serialize(writer, data);

                string text = writer.ToString();
                if (encrypt)
                {
                    if (!encryption.Password.NullOrEmpty())
                    {
                        //2a. Encrypt the XML string
                        text = encryption.Encrypt(text);
                    }
                }

                //2b. Write the XML string to the file
                Storage.File.Long.WriteAllText(filePath, text, encoding.Convert());
                result = new Success();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{nameof(XmlWriter<T>)}.{nameof(Serialize)}: {e.Message} {e.InnerException?.Message}");
                result = new Error(e);
            }
            finally { writer.Close(); }
            return result;
        }
    }
}