using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Potions;

public class LightningPotion : TheRailgun2Potion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    public override string CustomPackedOutlinePath => ImageHelper.GetImagePath($"atlases/potion_outline_atlas.sprites/star_potion.tres");
    public override string CustomPackedImagePath => ImageHelper.GetImagePath($"atlases/potion_atlas.sprites/star_potion.tres");
    protected override IEnumerable<DynamicVar> CanonicalVars => [ new RepeatVar(2) ];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
        {
            await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        }
    }
}