using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;
[Pool(typeof(DeprecatedCardPool))]
public class DeckboxDefend() : TheRailgun2Card(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(3m, ValueProp.Move),
        new BlockVar("Block2", 2m, ValueProp.Move),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        //new CalculatedBlockVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.PlayerCombatState != null ? card.Owner.PlayerCombatState.OrbQueue.Orbs.Count : 0)),
        new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.PlayerCombatState != null ? card.Owner.PlayerCombatState.OrbQueue.Orbs.Count : 0))
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        for (int i = 0; i < DynamicVars["CalculatedHits"].BaseValue; i++)
        {
            await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(1M);
        this.DynamicVars["Block2"].UpgradeValueBy(1M);
    }
}