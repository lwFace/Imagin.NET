namespace Imagin.Common
{
    /// <summary>
    /// Specifies an <see cref="object"/> that implements <see cref="ICheckable"/> (inherits <see cref="CheckableObject"/>).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CheckableObject<T> : CheckableObject
    {
        T value;
        public T Value
        {
            get => value;
            set => this.Change(ref this.value, value);
        }

        public override string ToString() => value.ToString();

        public CheckableObject() : this(false) { }

        public CheckableObject(T value, bool isChecked = false) : this(isChecked) => Value = value;

        public CheckableObject(bool isChecked = false) : base(isChecked) { }
    }
}