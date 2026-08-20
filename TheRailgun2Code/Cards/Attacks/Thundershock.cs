using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Thundershock() : TheRailgun2Card(2,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(15M, ValueProp.Move)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.Static(StaticHoverTip.Block)
        ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [Enums.Guardbreak];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            await CreatureCmd.LoseBlock(choiceContext, cardPlay.Target, cardPlay.Target.Block, Owner.Creature);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(cardPlay.Card, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}