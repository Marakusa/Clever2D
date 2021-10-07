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
    }
}
