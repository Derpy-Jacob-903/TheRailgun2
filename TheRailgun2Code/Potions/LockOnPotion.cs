using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Orbs;
using TheRailgun2.TheRailgun2Code.Extensions;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Potions;

public class LockOnPotion : TheRailgun2Potion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    public override string CustomPackedOutlinePath => ImageHelper.GetImagePath($"atlases/potion_outline_atlas.sprites/bone_brew.tres");
    public override string CustomPackedImagePath => ImageHelper.GetImagePath($"atlases/potion_atlas.sprites/bone_brew.tres");
    protected override IEnumerable<DynamicVar> CanonicalVars => [ new PowerVar<LockOnPower>(3) ];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        if (target != null)
            await PowerCmd.Apply<LockOnPower>(choiceContext, target, DynamicVars["LockOnPower"].IntValue,
                Owner.Creature,
                null);
    }
}