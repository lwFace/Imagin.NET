namespace Imagin.Common.Linq
{
    public static class UDoubleExtensions
    {
        public static UDouble Coerce(this UDouble value, UDouble maximum, UDouble minimum) => value > maximum ? maximum : (value < minimum ? minimum : value);

        public static bool Within(this UDouble input, UDouble minimum, UDouble maximum) => input >= minimum && input <= maximum;
    }
}