using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2.TheRailgun2Code.Character;

public abstract class SpendCard(int cost, CardType type, CardRarity rarity, TargetType target) : TheRailgun2Card(cost, type, rarity, target)
{
    public int canonicalSpendCost => 0;
    /// <summary>
    /// Whst kinda Orb this card Spends?
    /// Use `typeof(OrbModel)`
    /// </summary>
    public Type orbToSpend => typeof(LightningOrb);
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        bool osty = play.IsAutoPlay;
        if (!osty && Owner.PlayerCombatState.OrbQueue.Orbs.Where(c => c is LightningOrb).Count() >= canonicalSpendCost)
        {
            await BeforeSpend(choiceContext, play);
            for (int i = 0; i < canonicalSpendCost; i++)
            {
                await EchoOrb.RemoveFirstOf<LightningOrb>(choiceContext, Owner);
            }
            osty = true;
            if (!play.IsAutoPlay)
            {
                await AfterSpend(choiceContext, play);
            }
        }
        if (osty)
        {
            await MyOnPlay(choiceContext, play);
        }
    }

    public bool TryModifySpendCost(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        modifiedCost = originalCost;
        return false;
    }

    protected Task BeforeSpend(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        return Task.CompletedTask;
    }

    protected Task AfterSpend(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Use as you use the normal OnPlay
    /// </summary>
    /// <param name="choiceContext">PlayerChoiceContext</param>
    /// <param name="play">CardPlay</param>
    /// <returns></returns>
    protected abstract Task MyOnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play);
    
    public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    {
        if (card is SpendCard spendCard && autoPlayType == AutoPlayType.None &&
            card.Owner.PlayerCombatState != null &&
            !(card.Owner.PlayerCombatState.OrbQueue.Orbs.Count(c => c is LightningOrb) >= spendCard.canonicalSpendCost))
            return false;
        return base.ShouldPlay(card, autoPlayType);
    }
}


