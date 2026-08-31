using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class CoinFlip() : TheRailgun2Card(3,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [Enums.Discharge];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await OrbCmd.Channel<VoltOrb>(choiceContext, Owner);
        await CommonActions.Draw(this, choiceContext);
    }
    
    protected override CardLocation GetResultLocationForCardPlay()
    {
        var pileType = base.GetResultLocationForCardPlay();
        return pileType.pileType == PileType.Exhaust ? new CardLocation(Owner, PileType.Discard, CardPilePosition.Bottom) : new CardLocation(Owner, pileType.pileType, pileType.position);
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel clonedBy)
    {
        if (card.Pile != null && card.Pile.Type == PileType.Exhaust && card == this)
            await CardPileCmd.Add(card, PileType.Discard);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Cards.UpgradeValueBy(1M);
        EnergyCost.UpgradeBy(-1);
    }
}