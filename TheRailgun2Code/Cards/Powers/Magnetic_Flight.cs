using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class MagneticFlight() : TheRailgun2Card(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<MagneticFlightPower>(2)
    ];
    protected override HashSet<CardTag> CanonicalTags => [EchoOrb.Ferrous];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<MagneticFlightPower>(context, Owner.Creature, DynamicVars["MagneticFlightPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars["MagneticFlightPower"].UpgradeValueBy(2M);
}