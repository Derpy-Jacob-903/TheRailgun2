using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class GekotaKeychain() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        //Todo: Curse Tooltip like hermit
        //HoverTipFactory.(StaticHoverTip.)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState.AllCards
            .OrderBy<CardModel, CardRarity>((Func<CardModel, CardRarity>)(c => c.Rarity))
            .ThenBy<CardModel, ModelId>((Func<CardModel, ModelId>)(c => c.Id))
            .Count(u => u.Type == CardType.Curse) == 0) return;
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars.Cards.IntValue);
        var cards = (await CardSelectCmd.FromSimpleGrid(choiceContext,
                Owner.PlayerCombatState.AllCards
                    .OrderBy<CardModel, CardRarity>((Func<CardModel, CardRarity>)(c => c.Rarity))
                    .ThenBy<CardModel, ModelId>((Func<CardModel, ModelId>)(c => c.Id))
                    .Where(u => u.Type == CardType.Curse).ToList<CardModel>(), Owner, prefs)
            );
        if (cards == null)
            return;
        foreach (var cardModel in cards) await CardCmd.Exhaust(choiceContext, cardModel);
    }
    
    protected override bool ShouldGlowRedInternal => Owner.PlayerCombatState != null && Owner.PlayerCombatState.AllCards.Any(u => u.Type == CardType.Curse);

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}