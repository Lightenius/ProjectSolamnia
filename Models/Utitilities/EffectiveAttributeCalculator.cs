using System.Linq;

namespace ProjectSolamnia
{

    /// This class calculates the effective attributes for a character based on their base attributes,
    public static class EffectiveAttributeCalculator
    {
        private static int Clamp0(int v) => v < 0 ? 0 : v;
        public static int EffectiveDiplomacy(Character c)
            => Clamp0(
                c.BaseDiplomacy
                + c.CharacterTraits.Sum(ct => ct.Trait.BonusDiplomacy)
                + c.WisDip);

        public static int EffectiveMartial(Character c)
            => Clamp0(
                c.BaseMartial
                + c.CharacterTraits.Sum(ct => ct.Trait.BonusMartial)
                + c.WisMar);

        public static int EffectiveStewardship(Character c)
            => Clamp0(
                c.BaseStewardship
                + c.CharacterTraits.Sum(ct => ct.Trait.BonusStewardship)
                + c.WisSte);

        public static int EffectiveIntrigue(Character c)
            => Clamp0(
                c.BaseIntrigue
                + c.CharacterTraits.Sum(ct => ct.Trait.BonusIntrigue)
                + c.WisInt);

        public static int EffectiveLearning(Character c)
            => Clamp0(
                c.BaseLearning
                + c.CharacterTraits.Sum(ct => ct.Trait.BonusLearning)
                + c.WisLea);

        public static int EffectiveProwess(Character c)
            => Clamp0(
                c.BaseProwess
               + c.CharacterTraits.Sum(ct => ct.Trait.BonusProwess)
               + c.Level
               + c.WisPro
               + WisdomMath.ProwessAgeDelta(c.Age));
    }
}
