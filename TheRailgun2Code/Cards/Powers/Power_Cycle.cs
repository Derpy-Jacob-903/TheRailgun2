using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;


public class PowerCycle() : TheRailgun2Card(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<PowerCyclePower>(2)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [Enums.Discharge];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<PowerCyclePower>(context, Owner.Creature, DynamicVars["PowerCyclePower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars["PowerCyclePower"].UpgradeValueBy(1M);
}