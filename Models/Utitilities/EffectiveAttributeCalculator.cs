using System.Linq;

namespace ProjectSolamnia
{
    public static class EffectiveAttributeCalculator
    {
        public static int EffectiveDiplomacy(Character c)
            => c.BaseDiplomacy + c.CharacterTraits.Sum(ct => ct.Trait.BonusDiplomacy) + c.WisDip;

        public static int EffectiveMartial(Character c)
            => c.BaseMartial + c.CharacterTraits.Sum(ct => ct.Trait.BonusMartial) + c.WisMar;

        public static int EffectiveStewardship(Character c)
            => c.BaseStewardship + c.CharacterTraits.Sum(ct => ct.Trait.BonusStewardship) + c.WisSte;

        public static int EffectiveIntrigue(Character c)
            => c.BaseIntrigue + c.CharacterTraits.Sum(ct => ct.Trait.BonusIntrigue) + c.WisInt;

        public static int EffectiveLearning(Character c)
            => c.BaseLearning + c.CharacterTraits.Sum(ct => ct.Trait.BonusLearning) + c.WisLea;

        public static int EffectiveProwess(Character c)
            => c.BaseProwess
               + c.CharacterTraits.Sum(ct => ct.Trait.BonusProwess)
               + c.Level
               + c.WisPro
               + WisdomMath.ProwessAgeDelta(c.Age);
    }
}
