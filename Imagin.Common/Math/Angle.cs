namespace Imagin.Common.Math
{
    public static class Angle
    {
        public static double GetDegree(double radian) => radian * (180.0 / Numbers.PI);

        public static double GetRadian(double degree) => (Numbers.PI / 180.0) * degree;

        public static double NormalizeDegree(double degree)
        {
            var result = degree % 360.0;
            return result >= 0 ? result : (result + 360.0);
        }
    }
}