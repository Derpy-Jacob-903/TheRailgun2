using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2.TheRailgun2Code.Character;

public abstract class SpendCard(int cost, CardType type, CardRarity rarity, TargetType target) : TheRailgun2Card(cost, type, rarity, target)
{
    protected override Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        re
    }
    
    public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    {
        if ()
        if (card == this && autoPlayType == AutoPlayType.None &&
            card.Owner.PlayerCombatState != null &&
            !(card.Owner.PlayerCombatState.OrbQueue.Orbs.Count(c => c is LightningOrb) >= card))
            return false;
        return base.ShouldPlay(card, autoPlayType);
    }

    protected override void OnUpgrade()
    {

    }
}


