using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Relics;

public class CoinRelic2() : TheRailgun2Relic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Lightning", 1M),
        new EnergyVar(1),
        new RepeatVar(5)
    ];

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (Owner.PlayerCombatState != null && side == CombatSide.Player)
            for (int i = 0; i < Math.Min(Owner.PlayerCombatState.Energy, DynamicVars.Repeat.BaseValue); i++)
            {
                //await PlayerCmd.LoseEnergy(1, Owner);
                await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
            }
    }
    public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
    {
        return player != this.Owner ? amount : amount + (Decimal) this.DynamicVars.Energy.IntValue;
    }
    /*public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<CoinRelic2>();
    }*/
}