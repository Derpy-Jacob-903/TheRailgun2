using BaseLib.Cards.Variables;
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
        new PowerVar<StrengthPower>(-2),
        new DisplayVar<Taser>("EnemyStrengthLoss", (model) => (model.DynamicVars.Strength.IntValue * -1).ToString()),
        new PowerVar<WeakPower>(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            await CreatureCmd.LoseBlock(cardPlay.Target, cardPlay.Target.Block);
            await CommonActions.Apply<StrengthPower>(choiceContext, cardPlay.Target, this);
            await CommonActions.Apply<WeakPower>(choiceContext, cardPlay.Target, this);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Weak.UpgradeValueBy(1M);
}