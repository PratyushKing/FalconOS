namespace system
{
    class random
    {
        private static System.Random rnd = new System.Random();

        public static void init(int seed)
        {
            rnd = new System.Random(seed);
        }
        public static int next(int min, int max)
        {
            return rnd.Next(min, max);
        }
        public static double nextFloat()
        {
            return rnd.NextDouble();
        }
    }
}
