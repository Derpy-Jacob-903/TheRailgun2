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
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Relics;

public class CoinRelicNew() : TheRailgun2Relic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Lightning", 1M),
        new RepeatVar(1),
        new DynamicVar("EvokeDown", 4)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.ForEnergy(this),
        HoverTipFactory.FromOrb<VoltOrb>()
    ];

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (Owner.PlayerCombatState == null || side != CombatSide.Player)
            return base.BeforeSideTurnStart(choiceContext, side, participants, combatState);
        if (Owner.PlayerCombatState.TurnNumber == 3)
        {
            DynamicVars.Repeat.BaseValue += 1;
        }
        if (Owner.PlayerCombatState.TurnNumber == 5)
        {
            DynamicVars.Repeat.BaseValue += 1;
        }
        return base.BeforeSideTurnStart(choiceContext, side, participants, combatState);
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (Owner.PlayerCombatState != null && side == CombatSide.Player)
        {
            for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
            {
                await OrbCmd.Channel<VoltOrb>(choiceContext, Owner);
            }
        }
    }
    
    public override Task AfterCombatEnd(CombatRoom room)
    {
        DynamicVars.Repeat.BaseValue = 1;
        return base.AfterCombatEnd(room);
    }
    public override RelicModel GetUpgradeReplacement() => ModelDb.Relic<CoinRelicNewPro>();
}