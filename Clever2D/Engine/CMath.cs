namespace Clever2D.Engine
{
    /// <summary>
    /// CMath contains additional math calculations.
    /// </summary>
    public static class CMath
    {
        /// <summary>
        /// Greatest common divisor.
        /// </summary>
        public static ulong Gcd(ulong a, ulong b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        /// <summary>
        /// Returns the clamped value to the given minimum and maximum values.
        /// </summary>
        /// <param name="original">Value to clamp.</param>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        public static float Clamp(float original, float min, float max)
        {
            if (original > max)
                return max;
            else if (original < min)
                return min;
            else
                return original;
        }
    }
}
