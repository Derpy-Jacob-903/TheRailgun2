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

public class ParticleWallRailgun() : SpendCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Spend", canonicalSpendCost).WithTooltip("THERAILGUN2-SPEND"),
        new BlockVar(9m, ValueProp.Move)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromOrb<LightningOrb>()
    ];

    public override int canonicalSpendCost => 1;

    protected override CardLocation GetResultLocationForCardPlay()
    {
        var pileType = base.GetResultLocationForCardPlay();
        return pileType.pileType == PileType.Discard ? new CardLocation(Owner, PileType.Hand, CardPilePosition.Bottom) : new CardLocation(Owner, pileType.pileType, pileType.position);
    }
    
    protected override async Task MyOnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, play);
    }
    
    protected override bool ShouldGlowRedInternal => Owner.PlayerCombatState != null && !Owner.PlayerCombatState.OrbQueue.Orbs.Any(c => c is LightningOrb);

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}