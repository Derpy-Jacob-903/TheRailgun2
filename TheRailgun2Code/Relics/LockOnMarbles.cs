using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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

public class LockOnMarbles() : TheRailgun2Relic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<LockOnPower>(1M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<LockOnPower>()
    ];
    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        var bagOfMarbles = this;
        if (!participants.Contains<Creature>(bagOfMarbles.Owner.Creature) || bagOfMarbles.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        bagOfMarbles.Flash();
        IReadOnlyList<LockOnPower> vulnerablePowerList = await PowerCmd.Apply<LockOnPower>(choiceContext, (IEnumerable<Creature>) combatState.HittableEnemies, bagOfMarbles.DynamicVars["LockOnPower"].BaseValue, bagOfMarbles.Owner.Creature, (CardModel) null);
    }
}