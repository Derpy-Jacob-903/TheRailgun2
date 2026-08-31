using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Orbs;
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2.TheRailgun2Code.Character;

public abstract class SpendCard(int cost, CardType type, CardRarity rarity, TargetType target) : TheRailgun2Card(cost, type, rarity, target)
{
    public abstract int canonicalSpendCost { get; }
    /// <summary>
    /// Can this card Spend this Orb?
    /// </summary>
    public virtual bool CanSpendOrb(OrbModel orb)
    {
        return true; //orb is LightningOrb or VoltOrb;
    }

    public override bool CanBeGeneratedInCombat => MyCanBeGeneratedByModifiers();

    public override bool CanBeGeneratedByModifiers => MyCanBeGeneratedByModifiers();

    public bool MyCanBeGeneratedInCombat()
    {
        if (Owner.PlayerCombatState == null || Owner.Character.BaseOrbSlotCount >= 2)
            return true;
        var balls = Math.Max(Math.Max(Owner.PlayerCombatState.OrbQueue.Capacity, Owner.Character.BaseOrbSlotCount), 1);
        if (Owner.Character is Defect) // is this hardcoded?
            balls = Math.Max(Owner.PlayerCombatState.OrbQueue.Capacity, Owner.Character.BaseOrbSlotCount);
        return balls >= canonicalSpendCost;
    }
    public bool MyCanBeGeneratedByModifiers()
    {
        if (Owner.Character is Defect) // is this hardcoded?
            return Owner.Character.BaseOrbSlotCount >= canonicalSpendCost;
        return Math.Max(Owner.Character.BaseOrbSlotCount, 1) >= canonicalSpendCost;
    }
    public override IEnumerable<CardTag> Tags => [Enums.Spend];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        bool osty = play.IsAutoPlay;
        if (!osty && Owner.PlayerCombatState != null && Owner.PlayerCombatState.OrbQueue.Orbs.Count(CanSpendOrb) >= canonicalSpendCost)
        {
            await BeforeSpend(choiceContext, play);
            for (int i = 0; i < canonicalSpendCost; i++)
            {
                await EchoOrb.RemoveOrb(choiceContext, Owner, Owner.PlayerCombatState.OrbQueue.Orbs.First(CanSpendOrb));
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
            !(card.Owner.PlayerCombatState.OrbQueue.Orbs.Count(c => spendCard.CanSpendOrb(c)) >= spendCard.canonicalSpendCost))
            return false;
        return base.ShouldPlay(card, autoPlayType);
    }
}


