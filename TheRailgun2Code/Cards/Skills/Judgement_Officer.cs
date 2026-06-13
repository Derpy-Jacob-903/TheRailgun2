using BaseLib.Utils;
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

public class JudgementOfficer() : TheRailgun2Card(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(6),
        new DynamicVar("MaxUpgrades", 0)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.FromCard<Needle>(IsUpgraded)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
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