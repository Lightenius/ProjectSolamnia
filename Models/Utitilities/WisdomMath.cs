namespace ProjectSolamnia
{
    public static class WisdomMath
    {
        //
        public static int EarnedWisdomPoints(int age)
        {
            if (age <= 15) return 0;

            const double B = 15;            // scale for "how fast" early growth happens decrease for increased early growth
            const double A = (B + 185.0) / 50.0;       // scale for "how much" total wisdom is earned, tweak to adjust total points (DONT TWEAK)

            double x = age - 15;
            double total = A * Math.Log(1.0 + x / B);

            return (int)Math.Floor(total + 1e-9);
        }

        // Prowess decline (Option A): ap=30, p0=4, c=0.01
        public static int ProwessAgeDelta(int age)
        {
            return (int)Math.Round(4.0 - 0.01 * Math.Pow(age - 30, 2));
        }
    }
}
