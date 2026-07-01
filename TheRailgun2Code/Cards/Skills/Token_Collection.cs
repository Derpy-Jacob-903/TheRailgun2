using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class TokenCollection() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(5, ValueProp.Move),
        new CardsVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
        
        int maxCount = Math.Min(DynamicVars.Cards.IntValue, CardPile.MaxCardsInHand - PileType.Hand.GetPile(Owner).Cards.Count);
        if (maxCount <= 0)
            return;
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, maxCount);
        var card = (await CardSelectCmd.FromSimpleGrid(choiceContext, PileType.Discard.GetPile(Owner).Cards, Owner, prefs));
        if (card == null)
            return;
        var cardPileAddResult = await CardPileCmd.Add(card, PileType.Hand);
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
}