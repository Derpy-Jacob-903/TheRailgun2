using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class RewiringNew() : TheRailgun2Card(2,
    CardType.Power, CardRarity.Rare,
    TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar("Slots", 1),
        new RepeatVar("Orbs", 2)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            Player pp = play.Target.Player;
            if (pp == null) return;
            await OrbCmd.AddSlots(pp, DynamicVars["Slots"].IntValue);
            for (int i = 0; i < DynamicVars["Orbs"].BaseValue; i++)
            {
                await OrbCmd.Channel<VoltOrb>(context, pp);
            }
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["Slots"].UpgradeValueBy(1M);
        this.DynamicVars["Orbs"].UpgradeValueBy(1M);
    }
}