using System;
using System.Linq;

namespace ProjectSolamnia
{
    public class WisdomService
    {
        private readonly ProjectSolamniaDbContext _db;
        public WisdomService(ProjectSolamniaDbContext db) => _db = db;

        public void CaptureBaselineIfMissing(Character c)
        {
            if (c.WisdomSeed == null)
                c.WisdomSeed = (c.Id != 0 ? c.Id : Guid.NewGuid().GetHashCode());

            bool missingBaseline =
                c.WisdomBaselineDip == 0 &&
                c.WisdomBaselineMar == 0 &&
                c.WisdomBaselineSte == 0 &&
                c.WisdomBaselineInt == 0 &&
                c.WisdomBaselineLea == 0 &&
                c.WisdomBaselinePro == 0;

            if (!missingBaseline) return;

            int bDip = c.BaseDiplomacy   + c.CharacterTraits.Sum(ct => ct.Trait.BonusDiplomacy);
            int bMar = c.BaseMartial     + c.CharacterTraits.Sum(ct => ct.Trait.BonusMartial);
            int bSte = c.BaseStewardship + c.CharacterTraits.Sum(ct => ct.Trait.BonusStewardship);
            int bInt = c.BaseIntrigue    + c.CharacterTraits.Sum(ct => ct.Trait.BonusIntrigue);
            int bLea = c.BaseLearning    + c.CharacterTraits.Sum(ct => ct.Trait.BonusLearning);
            int bPro = c.BaseProwess     + c.CharacterTraits.Sum(ct => ct.Trait.BonusProwess);

            c.WisdomBaselineDip = bDip;
            c.WisdomBaselineMar = bMar;
            c.WisdomBaselineSte = bSte;
            c.WisdomBaselineInt = bInt;
            c.WisdomBaselineLea = bLea;
            c.WisdomBaselinePro = bPro;

            _db.SaveChanges();
        }

        public void TopUpWisdom(Character c)
        {
            CaptureBaselineIfMissing(c);

            int earned = WisdomMath.EarnedWisdomPoints(c.Age);
            int remaining = earned - c.WisdomPointsApplied;
            if (remaining <= 0) return;

            var rng = new Random(c.WisdomSeed!.Value);

            for (int i = 0; i < remaining; i++)
            {
                // 3 random +1s per wisdom point (with replacement; any stat can be hit multiple times)
                for (int j = 0; j < 3; j++)
                {
                    int r = rng.Next(6); // 0..5 → Dip, Mar, Ste, Int, Lea, Pro
                    switch (r)
                    {
                        case 0: c.WisDip++; break;
                        case 1: c.WisMar++; break;
                        case 2: c.WisSte++; break;
                        case 3: c.WisInt++; break;
                        case 4: c.WisLea++; break;
                        case 5: c.WisPro++; break;
                    }
                }

                c.WisdomPointsApplied++;
            }

            _db.SaveChanges();
        }
    }
}
