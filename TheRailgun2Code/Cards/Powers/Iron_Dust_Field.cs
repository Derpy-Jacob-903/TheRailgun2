using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class IronDustField() : TheRailgun2Card(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<PlatingPower>(3),
        new PowerVar<ThornsPower>(0)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<PlatingPower>(context, Owner.Creature, DynamicVars["PlatingPower"].BaseValue, Owner.Creature, this);
        if (this.IsUpgraded)
            await PowerCmd.Apply<ThornsPower>(context, Owner.Creature, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["PlatingPower"].UpgradeValueBy(1M);
        this.DynamicVars["ThornsPower"].UpgradeValueBy(2M);
    }
}