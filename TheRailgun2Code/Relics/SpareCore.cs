using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
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
using TheRailgun2.TheRailgun2Code.Powers;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Relics;

public class SpareCore() : TheRailgun2Relic
{
    protected override string BigIconPath => ImageHelper.GetImagePath($"relics/cracked_core.png");
    public override string PackedIconPath => ImageHelper.GetImagePath($"atlases/relic_atlas.sprites/cracked_core.tres");
    protected override string PackedIconOutlinePath => ImageHelper.GetImagePath($"atlases/relic_outline_atlas.sprites/cracked_core.tres");
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromOrb<LightningOrb>()
    ];
    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        var crackedCore = this;
        if (crackedCore.Owner.PlayerCombatState != null || !participants.Contains(crackedCore.Owner.Creature) || crackedCore.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        for (int i = 0; i < crackedCore.DynamicVars.Repeat.BaseValue; ++i)
            await OrbCmd.Channel<LightningOrb>((PlayerChoiceContext) new BlockingPlayerChoiceContext(), crackedCore.Owner);
    }
}