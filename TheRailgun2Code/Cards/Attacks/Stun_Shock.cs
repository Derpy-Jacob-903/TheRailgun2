using BaseLib.Extensions;
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

namespace TheRailgun2.TheRailgun2Code.Cards;

public class StunShock() : TheRailgun2Card(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Spend", 1).WithTooltip("THERAILGUN2-SPEND"),
        new DamageVar(8M, ValueProp.Move),
        new PowerVar<VulnerablePower>(1M),
        new PowerVar<WeakPower>(1M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromOrb<LightningOrb>(),
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>()
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var osty = false;
        if (!cardPlay.IsAutoPlay && Owner.PlayerCombatState.OrbQueue.Orbs.Any(c => c is LightningOrb))
        {
            await EchoOrb.RemoveFirstOf<LightningOrb>(choiceContext, Owner);
            osty = true;
        }
        else osty = cardPlay.IsAutoPlay;
        if (osty && cardPlay.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(cardPlay.Card, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars.Weak.BaseValue, 
                Owner.Creature, this);
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue,
                    Owner.Creature, this);
        }
        
    }

    public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    {
        if (card is StunShock && autoPlayType == AutoPlayType.None &&
            card.Owner.PlayerCombatState != null &&
            !card.Owner.PlayerCombatState.OrbQueue.Orbs.Any(c => c is LightningOrb))
            return false;
        return base.ShouldPlay(card, autoPlayType);
    }
    
    protected override bool ShouldGlowRedInternal => Owner.PlayerCombatState != null && !Owner.PlayerCombatState.OrbQueue.Orbs.Any(c => c is LightningOrb);

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}