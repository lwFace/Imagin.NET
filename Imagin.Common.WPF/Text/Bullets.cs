using System;

namespace Imagin.Common.Text
{
    [Serializable]
    public enum Bullets
    {
        Square,
        SquareOutline,
        Circle,
        CircleOutline,
        /// <summary>
        /// A., B., C.
        /// </summary>
        LetterUpperPeriod,
        /// <summary>
        /// A), B), C)
        /// </summary>
        LetterUpperParenthesis,
        /// <summary>
        /// a., b., c.
        /// </summary>
        LetterLowerPeriod,
        /// <summary>
        /// a), b), c)
        /// </summary>
        LetterLowerParenthesis,
        /// <summary>
        /// 1., 2., 3.
        /// </summary>
        NumberPeriod,
        /// <summary>
        /// 1), 2), 3)
        /// </summary>
        NumberParenthesis,
        /// <summary>
        /// I., II., III
        /// </summary>
        RomanNumberUpperPeriod,
        /// <summary>
        /// I), II), III)
        /// </summary>
        RomanNumberUpperParenthesis,
        /// <summary>
        /// i., ii., iii.
        /// </summary>
        RomanNumberLowerPeriod,
        /// <summary>
        /// i), ii), iii)
        /// </summary>
        RomanNumberLowerParenthesis
    }
}