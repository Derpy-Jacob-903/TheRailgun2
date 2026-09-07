using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class NeedleStorm() : TheRailgun2Card(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState != null)
        {
            IEnumerable<CardModel> list = Owner.PlayerCombatState.AllCards.Where(c => c is Needle);
            bool flag = true;
            foreach (CardModel card in list)
            {
                await CardCmd.Exhaust(choiceContext, card, false, !flag);
                flag = false;
            }
        }
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        Enums.Discharge,
        CardKeyword.Exhaust
    ];

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}