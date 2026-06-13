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

public class ParticleWallRailgun() : TheRailgun2Card(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Spend", 1).WithTooltip("SPEND"),
        new BlockVar(9m, ValueProp.Move)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromOrb<LightningOrb>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var osty = false;
        if (!cardPlay.IsAutoPlay) await EchoOrb.RemoveFirstOf<LightningOrb>(choiceContext, Owner);
        else osty = true;
        if (osty) await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        //await Cmd.Wait(0.25f);
    }
    
    protected override PileType GetResultPileTypeForCardPlay()
    {
        PileType pileTypeForCardPlay = base.GetResultPileTypeForCardPlay();
        return pileTypeForCardPlay != PileType.Discard ? pileTypeForCardPlay : PileType.Hand;
    }
    
    public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
    {
        if (card is ParticleWallRailgun && autoPlayType == AutoPlayType.None &&
            card.Owner.PlayerCombatState != null &&
            !card.Owner.PlayerCombatState.OrbQueue.Orbs.Any(c => c is LightningOrb))
            return false;
        return base.ShouldPlay(card, autoPlayType);
    }
    
    protected override bool ShouldGlowRedInternal => Owner.PlayerCombatState != null && !Owner.PlayerCombatState.OrbQueue.Orbs.Any(c => c is LightningOrb);

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}