namespace Imagin.Common.Converters
{
    public sealed class Value<T>
    {
        public readonly object ActualValue;

        public Value(T input) => ActualValue = input;

        public Value(Nothing input) => ActualValue = input;

        public static implicit operator Value<T>(T input) => new Value<T>(input);

        public static implicit operator Value<T>(Nothing input) => new Value<T>(input);
    }
}