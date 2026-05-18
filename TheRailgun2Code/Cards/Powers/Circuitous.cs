using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Circuitous() : TheRailgun2Card(2,
    CardType.Power, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<DexterityPower>(2)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<DexterityPower>(context, Owner.Creature, DynamicVars.Dexterity.BaseValue, Owner.Creature, this);
        if (play.Target != null)
            await PowerCmd.Apply<DexterityPower>(context, play.Target, -DynamicVars.Dexterity.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Dexterity.UpgradeValueBy(1M);
}