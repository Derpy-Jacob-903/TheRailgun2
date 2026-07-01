using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;
[Pool(typeof(DeprecatedCardPool))]
public class HangRailgun() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(1M),
        new CalculationExtraVar(1M),
        new CalculatedVar("Evokes").WithMultiplier(
            (Func<CardModel, Creature, decimal>) ((card, _) => (decimal)(Math.Pow(2, CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().Count(
                (Func<CardPlayFinishedEntry, bool>) (e => e.Actor.Player == card.Owner && e.CardPlay.Card is HangRailgun))))))
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //int orbCount = Owner.PlayerCombatState.OrbQueue.Orbs.Count;
        //for (int i = 0; i < orbCount; ++i)
        //{
            for (int j = 0; j < DynamicVars["Evokes"].BaseValue - 1; ++j) await OrbCmd.EvokeNext(choiceContext, Owner, false);
            await OrbCmd.EvokeNext(choiceContext, Owner);
        //}
    }

    protected override void OnUpgrade() => DynamicVars.CalculationBase.UpgradeValueBy(1);
}