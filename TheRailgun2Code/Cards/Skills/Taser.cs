using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Taser() : TheRailgun2Card(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(2),
        new PowerVar<WeakPower>(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            await CreatureCmd.LoseBlock(cardPlay.Target, cardPlay.Target.Block);
            await BetaMainCompatibility.PowerCmd_.Apply.InvokeGeneric<Task<StrengthPower>, StrengthPower>((object) null, (object) choiceContext, (object) cardPlay.Target, (DynamicVars.Power<StrengthPower>().BaseValue * -1), Owner.Creature, this, false);
            //await CommonActions.Apply<StrengthPower>(choiceContext, cardPlay.Target, this);
            await CommonActions.Apply<WeakPower>(choiceContext, cardPlay.Target, this);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Weak.UpgradeValueBy(1M);
}