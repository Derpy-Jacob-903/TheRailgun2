using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Supercharge() : TheRailgun2Card(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        //new PowerVar<BorrowedTimePower>(1),
        new EnergyVar(0)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var ownerPlayerCombatState = this.Owner.PlayerCombatState;
        if (ownerPlayerCombatState != null)
            foreach (CardModel allCard in ownerPlayerCombatState.AllCards)
            {
                if (allCard != this && allCard.IsUpgradable)
                    CardCmd.Upgrade(allCard);
            }

        /*if (CombatState != null)
        {
            var card = await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Wound>(Owner), PileType.Discard, Owner);
            //CardCmd.Enchant<Steady>(card.cardAdded, 1M);
        }*/

        if (this.IsUpgraded)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Energy.UpgradeValueBy(1M);
}