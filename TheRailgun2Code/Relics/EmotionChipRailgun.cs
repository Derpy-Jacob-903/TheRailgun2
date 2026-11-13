using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Relics;

public class EmotionChipRailgun() : TheRailgun2Relic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public bool LostHpInPreviousTurn
    {
        get
        {
            return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Any<DamageReceivedEntry>((Func<DamageReceivedEntry, bool>) (e => e.Receiver == this.Owner.Creature && !e.Result.WasFullyBlocked && e.HappenedLastPlayerTurn(this.Owner)));
        }
    }

    public override Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature dealer,
        CardModel cardSource)
    {
        if (!CombatManager.Instance.IsInProgress || target != this.Owner.Creature || result.UnblockedDamage <= 0)
            return Task.CompletedTask;
        this.Status = RelicStatus.Active;
        this.Flash();
        return Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        EmotionChipRailgun emotionChip = this;
        if (player != emotionChip.Owner)
            return;
        emotionChip.Status = RelicStatus.Normal;
        if (!emotionChip.LostHpInPreviousTurn)
            return;
        emotionChip.Flash();
        if (emotionChip.Owner.PlayerCombatState != null)
            foreach (OrbModel orb in emotionChip.Owner.PlayerCombatState.OrbQueue.Orbs)
            {
                await OrbCmd.Passive(choiceContext, orb, (Creature)null, true);
                await Cmd.Wait(0.25f);
            }
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        this.Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }
}