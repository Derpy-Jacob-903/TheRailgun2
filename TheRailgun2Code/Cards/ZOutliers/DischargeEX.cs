using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class DischargeEx() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
         new PowerVar<DischargePower>(4),
         new PowerVar<DexterityPower>(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await OrbCmd.EvokeNext(choiceContext, Owner);
        await PowerCmd.Apply<DischargePower>(choiceContext, Owner.Creature, DynamicVars["DexterityPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<DischargePower>(choiceContext, Owner.Creature, DynamicVars["DischargePower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars["DischargePower"].UpgradeValueBy(2M);
}