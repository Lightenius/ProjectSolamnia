using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectSolamnia.Migrations
{
    /// <inheritdoc />
    public partial class AddTraits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Traits",
                columns: new[] { "Id", "BonusDiplomacy", "BonusIntrigue", "BonusLearning", "BonusMartial", "BonusProwess", "BonusStewardship", "Description", "ImageUrl", "Name", "Type" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 1, 1, 1, "This character burns with a relentless drive to rise above all others. They crave greatness and will stop at nothing to carve their name into history—even if it means stepping on friends, family, or tradition.", null, "Ambitious", 0 },
                    { 2, 0, 3, 0, 0, 0, 0, "Justice and reason are strangers to this character. Their whims govern their rule, their moods define their verdicts. To serve them is to live in constant uncertainty.", null, "Arbitrary", 0 },
                    { 3, 1, 0, 0, 0, 0, 0, "Convinced of their own superiority, this character dismisses the opinions of others with a smirk. Humility is a word they’ve heard but never needed.", null, "Arrogant", 0 },
                    { 4, 0, 0, 0, 2, 3, 0, "Fear holds no sway over this character. They stand tall in the face of danger, charging into the fray with sword in hand and heart unyielding—even if wisdom says otherwise.", null, "Brave", 0 },
                    { 5, 0, 2, 0, 0, 0, 0, "Empathy is weakness in this character’s eyes. They view pain and loss as tools, not tragedies, and rarely pause to consider the emotional toll of their actions.", null, "Callous", 0 },
                    { 6, 1, 1, 0, 0, 0, 0, "Unshaken by chaos and unbothered by insult, this character is a pillar of serenity. Whether in war or council, their steady hand guides with grace.", null, "Calm", 0 },
                    { 7, 0, 0, 2, 0, 0, 0, "This character resists earthly pleasures, placing purity and discipline above passion. Their restraint is both a virtue and, at times, a wedge between them and others.", null, "Chaste", 0 },
                    { 8, 2, -2, 0, 0, 0, 0, "A heart that aches for the suffering of others beats within this character. Their kindness may inspire loyalty, but it also exposes them to exploitation and sorrow.", null, "Compassionate", 0 },
                    { 9, 0, -1, 2, 0, 0, 0, "This character desires little beyond what they already possess. Their peaceful nature brings them happiness—but ambitionless calm can be a dangerous flaw in a turbulent world.", null, "Content", 0 },
                    { 10, 0, 2, 0, -2, -3, 0, "Self-preservation guides every step of this character. They shrink from conflict and avoid risk, preferring to survive in the shadows rather than die in the light.", null, "Craven", 0 },
                    { 11, 0, 4, 0, 0, 0, 0, "This character weaves lies like a spider spins webs. Deception is their tool, and truth is a burden they rarely carry. To trust them is to invite betrayal.", null, "Deceitful", 0 },
                    { 12, 1, 1, 1, 1, 1, 1, "Industrious and relentless, this character pours their soul into every task. Their work ethic inspires others, but it can also lead them to exhaustion.", null, "Diligent", 0 },
                    { 13, 0, 0, 0, 0, 0, 0, "Unbound by norms, this character marches to a beat only they can hear. Their odd behavior confounds others, sometimes charming, often confusing.", null, "Eccentric", 0 },
                    { 14, 1, 0, 0, 0, 0, 0, "Inconstant and changeable, this character’s loyalties and moods shift like the wind. Their unpredictability makes alliances both risky and fleeting.", null, "Fickle", 0 },
                    { 15, 2, -2, 1, 0, 0, 0, "This character’s heart is quick to pardon, even when justice might demand otherwise. Their mercy heals wounds, but it can also enable wrongdoing.", null, "Forgiving", 0 },
                    { 16, 3, 0, 0, 0, 0, 0, "Gold flows from this character’s hands like water. Whether out of kindness or a desire to be loved, they give freely, sometimes to their own detriment.", null, "Generous", 0 },
                    { 17, 0, 0, 0, 0, 0, 0, "Pleasure and indulgence are this character’s constant companions. They feast, drink, and consume without thought for moderation or judgment.", null, "Gluttonous", 0 },
                    { 18, 0, 0, 0, 0, 0, 0, "This character hungers for wealth above all else. Coins are their closest companions, and they are rarely seen giving without expecting more in return.", null, "Greedy", 0 },
                    { 19, 2, 0, 0, 0, 0, 0, "Charming and outgoing, this character thrives in the company of others. Their laughter is infectious, their presence magnetic.", null, "Gregarious", 0 },
                    { 20, 2, -4, 0, 0, 0, 0, "This character values truth as a sacred duty. Their candor is refreshing to some and dangerous to others, especially in a world ruled by secrets.", null, "Honest", 0 },
                    { 21, 0, 0, 0, 0, 0, 0, "This character carries little pride, preferring to downplay their own achievements. Their humility is admirable—but it can be mistaken for weakness.", null, "Humble", 0 },
                    { 22, 0, 0, 0, 0, 0, 0, "This character has little tolerance for delay or indecision. They act quickly—sometimes recklessly—and despise idle waiting or long debates.", null, "Impatient", 0 },
                    { 23, 0, -3, 1, 0, 0, 2, "Guided by law and fairness, this character strives to do what is right—even when the cost is high. Justice is their compass, and compromise is not in their nature.", null, "Just", 0 },
                    { 24, 0, 2, 0, 0, 0, 0, "Driven by carnal desires, this character embraces passion without restraint. Whether through charm or indulgence, they rarely deny themselves pleasure.", null, "Lustful", 0 },
                    { 25, 0, 0, 0, 0, 0, 0, "This character avoids effort whenever possible, preferring rest over toil. Duties are often neglected in favor of comfort and ease.", null, "Lazy", 0 },
                    { 26, 0, 2, 0, 3, 0, 0, "Suspicious of all, this character sees plots and betrayal around every corner. Trust is a foreign concept—and fear is their only constant ally.", null, "Paranoid", 0 },
                    { 27, 0, 0, 2, 0, 0, 0, "Slow to anger and deliberate in action, this character believes that good things come to those who wait—and they are always willing to wait.", null, "Patient", 0 },
                    { 28, 0, 2, 0, 2, 1, 0, "This character finds satisfaction in the suffering of others. Whether through cruelty or vengeance, they revel in power over pain.", null, "Sadistic", 0 },
                    { 29, 0, 0, 0, 0, 0, 0, "Quiet and reserved, this character avoids the spotlight. Crowds are discomforting, and small talk is a source of anxiety.", null, "Shy", 0 },
                    { 30, 0, 0, 0, 0, 0, 3, "Once this character has made up their mind, it is nearly impossible to sway them. Their resolve is ironclad, for better or worse.", null, "Stubborn", 0 },
                    { 31, 0, 0, 0, 0, 0, 2, "Moderation defines this character’s habits. They avoid indulgence and temptation, living a life of balance and restraint.", null, "Temperate", 0 },
                    { 32, 2, -2, 0, 0, 0, 0, "Naive or idealistic, this character gives others the benefit of the doubt. Unfortunately, this often leads them into dangerous traps.", null, "Trusting", 0 },
                    { 33, 0, 2, 0, 0, 0, 0, "This character never forgets a slight. Grudges are cherished like treasures, and revenge is a promise they always intend to keep.", null, "Vengeful", 0 },
                    { 34, -1, -1, 0, 2, 0, 0, "Quick-tempered and easily provoked, this character is a storm waiting to explode. Their fury inspires fear—and often regret.", null, "Wrathful", 0 },
                    { 35, 0, 0, 0, 2, 0, 0, "Fanatical in their beliefs, this character serves a higher power with unwavering devotion. To them, heresy is not a difference of opinion—it's a crime.", null, "Zealous", 0 },
                    { 36, 2, 0, 0, 0, 0, 0, "Though trained in diplomacy, this character’s clumsy attempts at negotiation often result in embarrassment. Their intentions are good, but the execution falls painfully short.", null, "Naive Appeaser", 1 },
                    { 37, 4, 0, 0, 0, 0, 0, "This character has found some success in the diplomatic arena, though it’s unclear whether it’s through talent or sheer luck. Still, they hold their own in courtly matters.", null, "Adequate Bargainer", 1 },
                    { 38, 6, 0, 0, 0, 0, 0, "Gifted with silvered words and natural charm, this character navigates diplomacy with confidence. Courtiers hang on their every word, and few can deny their appeals.", null, "Charismatic Negotiator", 1 },
                    { 39, 8, 0, 0, 0, 0, 0, "This master of diplomacy commands influence behind the scenes with unmatched skill. Subtle and refined, they bend even the most stubborn wills without lifting a sword.", null, "Grey Eminence", 1 },
                    { 40, 10, 0, 0, 3, 0, 0, "With eloquence honed to a fine edge, this character can untangle the most volatile disputes and charm the bitterest rivals. Diplomacy isn’t just a skill—it’s their art.", null, "Virtuoso Arbitrator", 1 },
                    { 41, 0, 0, 0, 2, 0, 0, "Armed with theory but lacking in practice, this character's martial education is more of a liability than an asset. They often do more harm to themselves than their foes.", null, "Misguided Warrior", 1 },
                    { 42, 0, 0, 0, 4, 0, 0, "This character has a working grasp of martial matters and some experience in the field, though they still have much to learn before mastering the art of war.", null, "Tough Soldier", 1 },
                    { 43, 0, 0, 0, 6, 0, 0, "A proven battlefield leader, this character commands with confidence and knows how to turn the tide of war through calculated tactics.", null, "Skilled Tactician", 1 },
                    { 44, 0, 0, 0, 8, 0, 0, "War is a game, and this character is a grandmaster. No martial detail escapes their attention, and every campaign is a well-orchestrated symphony of steel.", null, "Brilliant Strategist", 1 },
                    { 45, 0, 0, 0, 10, 0, 3, "Renowned across kingdoms, this character leads armies with unmatched brilliance. Their feats in battle are the stuff of legend, and entire wars bend to their will.", null, "Exalted Warlord", 1 },
                    { 46, 0, 0, 0, 0, 0, 2, "Educated in stewardship but devoid of restraint, this character is more likely to spend wealth than manage it. Prosperity slips through their fingers like sand.", null, "Indulgent Wastrel", 1 },
                    { 47, 0, 0, 0, 0, 0, 4, "Competent and careful, this character can manage a budget and oversee estates, even if their talents occasionally fall short.", null, "Thrifty Clerk", 1 },
                    { 48, 0, 0, 0, 0, 0, 6, "Savvy and detail-oriented, this character excels in managing lands and treasuries, turning modest ventures into wealth-generating enterprises.", null, "Fortune Builder", 1 },
                    { 49, 0, 0, 0, 0, 0, 8, "Everything this character touches turns to gold. A paragon of stewardship, they manage resources with unrivaled efficiency and vision.", null, "Midas Touched", 1 },
                    { 50, 0, 0, 3, 0, 0, 10, "An economic mastermind, this character elevates even failing realms into beacons of prosperity. Their brilliance shines brightest in coin and crown.", null, "Golden Sovereign", 1 },
                    { 51, 0, 2, 0, 0, 0, 0, "While trained in subterfuge, this character often blunders their schemes. Their plots are usually transparent and ineffective, but at least they try.", null, "Amateurish Plotter", 1 },
                    { 52, 0, 4, 0, 0, 0, 0, "Dramatic and cunning, this character knows their way around intrigue but often takes flashy risks that may backfire.", null, "Flamboyant Trickster", 1 },
                    { 53, 0, 6, 0, 0, 0, 0, "This character has a mind for manipulation and secrets, spinning lies as easily as others breathe. Their reach is long and subtle.", null, "Intricate Webweaver", 1 },
                    { 54, 0, 8, 0, 0, 0, 0, "Few have seen their true face. This master of secrecy and deception leaves little trace, ruling through whispers and threats.", null, "Elusive Shadow", 1 },
                    { 55, 3, 10, 0, 0, 0, 0, "Every move this character makes pulls on a hidden string. Their mastery of intrigue rivals kings—rulers may sit on the throne, but this one pulls the strings.", null, "Conniving Puppetmaster", 1 },
                    { 56, 0, 0, 2, 0, 0, 0, "Their grasp of letters is solid, but beyond that lies confusion. This character has studied learning, yet true insight continues to elude them.", null, "Conscientious Scribe", 1 },
                    { 57, 0, 0, 4, 0, 0, 0, "This character has cultivated a respectable understanding of faith, reason, and the written word. They are a thoughtful student of the world’s mysteries.", null, "Insightful Thinker", 1 },
                    { 58, 0, 0, 6, 0, 0, 0, "Possessing both curiosity and discipline, this character dives deep into philosophical and theological debates, often leading scholars in discussion.", null, "Astute Intellectual", 1 },
                    { 59, 0, 0, 8, 0, 0, 0, "This brilliant mind sees connections others miss. Their insights shape doctrine and reform, often setting the course of intellectual history.", null, "Mastermind Philosopher", 1 },
                    { 60, 0, 3, 10, 0, 0, 0, "Revered in scholarly and spiritual circles alike, this character’s wisdom guides kings and clergy. They are a living library of divine and secular knowledge.", null, "Erudite Oracle", 1 },
                    { 61, 0, 0, 0, 0, 1, 0, "Though technically trained, this character struggles with the basics of combat. Their clumsy swordplay is more likely to amuse than intimidate.", null, "Bumbling Squire", 1 },
                    { 62, 0, 0, 0, 0, 2, 0, "Having earned their spurs, this character is a competent warrior who knows their way around a battlefield and commands respect in melee.", null, "Confident Knight", 1 },
                    { 63, 0, 0, 0, 0, 3, 0, "Tested in raids and skirmishes, this character leads with grit and strikes with power. Their presence inspires their comrades to press on.", null, "Formidable Banneret", 1 },
                    { 64, 0, 0, 0, 0, 4, 0, "A legend among warriors, this character’s name echoes across arenas and battlefields. Tales of their feats inspire awe—and fear.", null, "Famous Champion", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Traits",
                keyColumn: "Id",
                keyValue: 64);
        }
    }
}
