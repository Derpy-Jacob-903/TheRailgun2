using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class IronDustVial() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2)
    ];
    protected override HashSet<CardTag> CanonicalTags => [EchoOrb.Ferrous];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await OrbCmd.EvokeNext(choiceContext, Owner);
    }

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}