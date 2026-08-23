using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class EnchancingStrike() : TheRailgun2Card(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10M, ValueProp.Move),
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public override bool CanBeGeneratedInCombat => false;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var shouldTriggerFatal = cardPlay.Target.Powers.All(p => p.ShouldOwnerDeathTriggerFatal());
        var attackCommand = await CommonActions.CardAttack(this, cardPlay).WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        if (!shouldTriggerFatal || !attackCommand.Results.Any(r => r.Any(g => g is { WasTargetKilled: true, Receiver.IsSecondaryEnemy: false })))
            return;
        var upgradableCards = PileType.Deck.GetPile(Owner).Cards.Where(c => c.IsUpgradable).ToList();
        if (upgradableCards.Count > 0)
        {
            await Cmd.Wait(0.5f);
            var cardModel = Owner.RunState.Rng.Niche.NextItem(upgradableCards);
            if (cardModel == null) return;
            Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(Owner.NetId).UpgradedCards.Add(cardModel.Id);
            cardModel.UpgradeInternal();
            cardModel.FinalizeUpgradeInternal();
            if (LocalContext.IsMe(Owner))
                NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(NCardSmithVfx.Create([
                    cardModel
                ])!);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}