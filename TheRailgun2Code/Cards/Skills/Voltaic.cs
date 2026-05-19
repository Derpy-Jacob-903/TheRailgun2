using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class VoltaicRailgun() : TheRailgun2Card(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedVar("CalculatedChannels").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) CombatManager.Instance.History.Entries.OfType<OrbChanneledEntry>().Count<OrbChanneledEntry>((Func<OrbChanneledEntry, bool>) (e => e.Actor.Player == card.Owner && e.Orb is LightningOrb))))
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<LightningOrb>()
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var voltaic = this;
        await CreatureCmd.TriggerAnim(voltaic.Owner.Creature, "Cast", voltaic.Owner.Character.CastAnimDelay);
        int lightningChanneledCount = (int) ((CalculatedVar) voltaic.DynamicVars["CalculatedChannels"]).Calculate(cardPlay.Target);
        for (int i = 0; i < lightningChanneledCount; ++i)
            await OrbCmd.Channel<LightningOrb>(choiceContext, voltaic.Owner);
    }
    protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Exhaust);
}