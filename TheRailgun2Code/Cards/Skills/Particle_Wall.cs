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
        new BlockVar(9m, ValueProp.Move)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromOrb<LightningOrb>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await EchoOrb.RemoveFirstOf<LightningOrb>(choiceContext, Owner);
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        await Cmd.Wait(0.25f);
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

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}