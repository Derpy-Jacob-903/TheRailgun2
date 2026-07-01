using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class ShatterRailgun() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int orbCount = Owner.PlayerCombatState.OrbQueue.Orbs.Count;
        for (int i = 0; i < orbCount; ++i)
        {
            for (int j = 0; i < DynamicVars.Repeat.BaseValue - 1; ++j) await OrbCmd.EvokeNext(choiceContext, Owner, false);
            await OrbCmd.EvokeNext(choiceContext, Owner);
        }
    }
    
    protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
}