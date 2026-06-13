using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Electrokinesis() : TheRailgun2Card(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(15m, ValueProp.Move)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromOrb<LightningOrb>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var osty = false;
        if (!cardPlay.IsAutoPlay) await EchoOrb.RemoveFirstOf<LightningOrb>(choiceContext, Owner);
        else osty = true;
        if (osty && cardPlay.Target != null) await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
    }
    
    public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    {
        if (card is Electrokinesis && autoPlayType == AutoPlayType.None &&
            card.Owner.PlayerCombatState != null &&
            !card.Owner.PlayerCombatState.OrbQueue.Orbs.Any(c => c is LightningOrb))
            return false;
        return base.ShouldPlay(card, autoPlayType);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}