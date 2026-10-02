using BaseLib.Extensions;
using BaseLib.Utils;
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
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class NegativeFieldNew() : SpendCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Spend", canonicalSpendCost).WithTooltip("THERAILGUN2-SPEND"),
        new BlockVar(7m, ValueProp.Move),
        new CardsVar(1)
    ];

    public override int canonicalSpendCost => 1;
    
    protected override async Task MyOnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target != null)
        {
            await CreatureCmd.GainBlock(play.Target, DynamicVars.Block, play);
            if (play.Target.Player != null)
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, play.Target.Player);
        }
    }
    
    protected override bool ShouldGlowRedInternal => Owner.PlayerCombatState != null && !Owner.PlayerCombatState.OrbQueue.Orbs.Any(c => c is LightningOrb);

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}