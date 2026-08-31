using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class TempestRailgun() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        TempestRailgun tempest = this;
        await CreatureCmd.TriggerAnim(tempest.Owner.Creature, "Cast", tempest.Owner.Character.CastAnimDelay);
        int numOfOrbs = tempest.ResolveEnergyXValue();
        if (tempest.IsUpgraded)
            numOfOrbs += 2 * CurrentUpgradeLevel;
        for (int i = 0; i < numOfOrbs; ++i)
            await OrbCmd.Channel<VoltOrb>(choiceContext, tempest.Owner);
    }
}