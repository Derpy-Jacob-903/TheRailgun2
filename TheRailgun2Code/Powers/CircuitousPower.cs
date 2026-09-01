using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class CircuitousPower : TheRailgun2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => true;
    private bool _balls => Owner.Player?.PlayerCombatState != null && Owner.Player.PlayerCombatState.Phase == PlayerTurnPhase.Play;

    public override Decimal ModifyOrbValue(OrbModel orb, Decimal value)
    {
        if (orb is VoltOrb && !_balls) return value;
        return this.Owner.Player != orb.Owner ? value : Math.Max(value + (Decimal) this.Amount, 0M);
    }

    /*
    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Side)
            return;
        return base.BeforeSideTurnStart(choiceContext, side, participants, combatState);
    }

    public override Task BeforeSideTurnEndVeryEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
            return;
        return base.BeforeSideTurnEndVeryEarly(choiceContext, side, participants);
    }
    */
}