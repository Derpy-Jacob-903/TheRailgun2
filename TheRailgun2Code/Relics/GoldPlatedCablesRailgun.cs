using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Relics;

public class GoldPlatedCablesRailgun() : TheRailgun2Relic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override int ModifyOrbPassiveTriggerCounts(OrbModel orb, int triggerCount)
    {
        var ownerPlayerCombatState = Owner.PlayerCombatState;
        return ownerPlayerCombatState != null && (orb.Owner != Owner || orb != ownerPlayerCombatState.OrbQueue.Orbs[0]) ? triggerCount : triggerCount + 1;
    }

    public override Task AfterModifyingOrbPassiveTriggerCount(OrbModel orb)
    {
        Flash();
        return Task.CompletedTask;
    }
}