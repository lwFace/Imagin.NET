using Imagin.Common.Analytics;
using Imagin.Common.Serialization;

namespace Imagin.Common.Collections.Serialization
{
    public class BinaryWriter<T> : Writer<T>
    {
        public BinaryWriter(string filePath, Limit limit = default) : base(filePath, limit) { }

        public override Result Deserialize(string filePath, out object data) => BinarySerializer.Deserialize(filePath, out data);

        public override Result Serialize(string filePath, object data) => BinarySerializer.Serialize(filePath, data);
    }
}