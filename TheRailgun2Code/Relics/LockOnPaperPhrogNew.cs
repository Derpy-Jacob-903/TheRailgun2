using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Powers;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Relics;


public class LockOnPaperPhrogNew() : TheRailgun2Relic
{
    protected override string BigIconPath => ImageHelper.GetImagePath($"atlases/relic_atlas.sprites/paper_phrog.tres");
    protected override string PackedIconOutlinePath => ImageHelper.GetImagePath($"atlases/relic_outline_atlas.sprites/paper_phrog.tres");
    public override RelicRarity Rarity =>
        RelicRarity.Shop;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<LockOnPower>()
    ];

    public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer,
        CardModel cardSource, CardPlay cardPlay)
    {
        if (dealer != null && dealer.HasPower<LockOnPower>())
        {
            return 0.75m;
        }
        return 1m;
    }
}