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
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class TeleporterTwo() : TheRailgun2Card(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar("Discards", 3),
        new CardsVar(2),
        new DynamicVar("MaxUpgrades", 0)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.FromCard<Needle>(IsUpgraded)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, DynamicVars["Discards"].IntValue), null, this));
        foreach (CardModel card in await Needle.CreateInHand(Owner, DynamicVars.Cards.IntValue, CombatState))
        {
            for (int i = 0; i < DynamicVars["MaxUpgrades"].IntValue && card.IsUpgradable; i++)
            {
                CardCmd.Upgrade(card);
            }
        }
    }

    protected override void OnUpgrade() => this.DynamicVars["MaxUpgrades"].UpgradeValueBy(1M);
}