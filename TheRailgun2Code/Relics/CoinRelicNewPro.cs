using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
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

public class CoinRelicNewPro() : TheRailgun2Relic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(2)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.ForEnergy(this),
        HoverTipFactory.FromOrb<VoltOrb>()
    ];
    
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (Owner.PlayerCombatState != null && side == CombatSide.Player)
        {
            if (Owner.PlayerCombatState.TurnNumber == 1) { DynamicVars.Repeat.BaseValue = 1; }
            else
            {
                if (DynamicVars.Repeat.BaseValue < 6) { DynamicVars.Repeat.BaseValue += 1; }
            }
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
}