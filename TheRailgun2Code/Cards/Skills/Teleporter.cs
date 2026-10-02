using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class SistersProject() : TheRailgun2Card(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2),
        new DynamicVar("MaxUpgrades", -1)
    ];
    
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.FromCard<Needle>(DynamicVars["MaxUpgrades"].BaseValue > 0)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
        {
            foreach (var VARIABLE in CombatState.Players)
            {
                foreach (CardModel card in await Needle.CreateInHand(VARIABLE, DynamicVars.Cards.IntValue, CombatState))
                {
                    for (int i = 0; i < DynamicVars["MaxUpgrades"].IntValue && card.IsUpgradable; i++)
                    {
                        CardCmd.Upgrade(card);
                    }
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Cards.UpgradeValueBy(1M);
    }
}