using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class LightningSpeedNew() : TheRailgun2Card(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12M, ValueProp.Move),
    ];

    public override bool CanBeGeneratedInCombat => false; //can copy 

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(cardPlay.Card, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars["Exhaust"].IntValue);
        var cards = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, (Func<CardModel, bool>)null, this));//.FirstOrDefault<CardModel>();
        foreach (var card in cards)
        {
            if (card != null)
            {
                CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), PileType.Draw, Owner));
                for (int i = 0; i < this.CurrentUpgradeLevel; i++)
                {
                    CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), PileType.Discard, Owner), 2.2f);
                }
                await CardCmd.Exhaust(choiceContext, card);
            }
        }
    }

    //protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}