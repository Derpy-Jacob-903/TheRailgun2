using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Thundervolt() : TheRailgun2Card(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(21M, ValueProp.Move),
        //new RepeatVar(2),
        new DynamicVar("Spend", 2).WithTooltip("THERAILGUN2-SPEND")
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<LightningOrb>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var osty = cardPlay.IsAutoPlay;
        if (!cardPlay.IsAutoPlay && Owner.PlayerCombatState.OrbQueue.Orbs.Where(c => c is LightningOrb).Count() >= 2)
        {
            await EchoOrb.RemoveFirstOf<LightningOrb>(choiceContext, Owner);
            await EchoOrb.RemoveFirstOf<LightningOrb>(choiceContext, Owner);
            osty = true;
        }
        if (osty && CombatState != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(cardPlay.Card, cardPlay).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
        }
    }
    
    public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    {
        if (card is Thundervolt && autoPlayType == AutoPlayType.None &&
            card.Owner.PlayerCombatState != null &&
            !(card.Owner.PlayerCombatState.OrbQueue.Orbs.Count(c => c is LightningOrb) >= 2))
            return false;
        return base.ShouldPlay(card, autoPlayType);
    }
    
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(9M);
}