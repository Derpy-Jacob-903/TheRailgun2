using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards.Powers;

public class Rewiring() : TheRailgun2Card(2,
    CardType.Power, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(0M),
        new ExtraDamageVar(1M),
        new DynamicVar("Power", 0)
        //Todo: should display the changes
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target == null) return;
        var str_evil = 0;
        var str_your = 0;
        foreach (var pm in Owner.Creature.Powers.Where(model => model is StrengthPower))
        {
            str_evil += pm.Amount;
            str_evil -= CurrentUpgradeLevel;
            await PowerCmd.Remove<StrengthPower>(Owner.Creature);
        }
        foreach (var pm in play.Target.Powers.Where(model => model is StrengthPower))
        {
            str_your += pm.Amount;
            str_your += CurrentUpgradeLevel;
            await PowerCmd.Remove<StrengthPower>(play.Target);
        }
        await PowerCmd.Apply<StrengthPower>(context, Owner.Creature, str_your, Owner.Creature, this);
        await PowerCmd.Apply<StrengthPower>(context, play.Target, str_evil, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars["Power"].UpgradeValueBy(1M);
}