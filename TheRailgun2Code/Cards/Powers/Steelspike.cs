using BaseLib.Patches.Features;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Steelspike() : TheRailgun2Card(0,
    CardType.Power, CardRarity.Uncommon,
    TargetType.AllAllies)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        
        if (CombatState != null && card == this)
        {
            modifiedCost = CombatState.PlayerCreatures.Count(c => c.IsAlive && c != card.Owner.Creature) + originalCost;
            return true;
        }
        modifiedCost = originalCost;
        return false;
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<ThornsPower>(3),
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (CombatState != null)
            foreach (var pp in CombatState.PlayerCreatures)
            {
               await PowerCmd.Apply<ThornsPower>(context, pp, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, this);
            }

    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["ThornsPower"].UpgradeValueBy(2M);
    }
}