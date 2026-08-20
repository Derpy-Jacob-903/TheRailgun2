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
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Taser() : SpendCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    public override int canonicalSpendCost => 1;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(2),
        new PowerVar<WeakPower>(2),
        new DynamicVar("Spend", 1).WithTooltip("THERAILGUN2-SPEND")
    ];
    
    protected override async Task MyOnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            await CreatureCmd.LoseBlock(choiceContext, cardPlay.Target, cardPlay.Target.Block, Owner.Creature);
            await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target,
                (DynamicVars.Power<StrengthPower>().BaseValue * -1), Owner.Creature, this);
            await CommonActions.Apply<WeakPower>(choiceContext, cardPlay.Target, this);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Weak.UpgradeValueBy(1M);
}