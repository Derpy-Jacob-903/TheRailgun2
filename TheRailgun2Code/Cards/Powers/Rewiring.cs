using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Rewiring() : TheRailgun2Card(2,
    CardType.Power, CardRarity.Uncommon,
    CustomTargetType.Anyone)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Power", 0)
        //Todo: should display the changes
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target == null) return;
        var str_evil = Owner.Creature.Powers.Where(model => model is StrengthPower).Sum(pm => pm.Amount);
        if (play.Target.Side != Owner.Creature.Side) str_evil -= CurrentUpgradeLevel;
        await PowerCmd.Remove<StrengthPower>(Owner.Creature);
        var str_your = play.Target.Powers.Where(model => model is StrengthPower).Sum(pm => pm.Amount);
        str_your += CurrentUpgradeLevel;
        await PowerCmd.Remove<StrengthPower>(play.Target);
        await PowerCmd.Apply<StrengthPower>(context, Owner.Creature, str_your, Owner.Creature, this);
        await PowerCmd.Apply<StrengthPower>(context, play.Target, str_evil, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars["Power"].UpgradeValueBy(1M);
}