using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Relics;

public class CoinRelic() : TheRailgun2Relic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Lightning", 1M),
        new RepeatVar(3)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.ForEnergy(this),
        HoverTipFactory.FromOrb<LightningOrb>()
    ];

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (Owner.PlayerCombatState != null && side == CombatSide.Player)
            for (int i = 0; i < Math.Min(Owner.PlayerCombatState.Energy, DynamicVars.Repeat.BaseValue); i++)
            {
                await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
            }
    }
    public override RelicModel GetUpgradeReplacement() => ModelDb.Relic<CoinRelicNewPro>();
}