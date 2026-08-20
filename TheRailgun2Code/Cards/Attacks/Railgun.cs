using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Railgun() : TheRailgun2Card(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
         //CardKeyword.Retain
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(15M, ValueProp.Move),
        new PowerVar<VulnerablePower>(2M),
        new PowerVar<WeakPower>(2M)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //await EchoOrb.EvokeFirstOf<LightningOrb>(choiceContext, Owner);
        //await EchoOrb.EvokeFirstOf<LightningOrb>(choiceContext, Owner);
        if (cardPlay.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(cardPlay.Card, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars.Weak.BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this); 
        }
    }

    /*public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    {
        if (card is Railgun && autoPlayType == AutoPlayType.None &&
            card.Owner.PlayerCombatState != null &&
            card.Owner.PlayerCombatState.OrbQueue.Orbs.Count(c => c is LightningOrb) < 2)
            return false;
        return base.ShouldPlay(card, autoPlayType);
    }*/

    protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Retain);
    //this.DynamicVars.Damage.UpgradeValueBy(5M);
}