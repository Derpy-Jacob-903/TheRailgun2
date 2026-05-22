using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class ElectricFencePower : TheRailgun2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult damageResult,
        ValueProp props,
        Creature dealer,
        CardModel cardModel)
    {
        if (target != Owner || dealer == null || !props.IsPoweredAttack())
            return;
        await CreatureCmd.Damage(choiceContext, dealer, Amount, ValueProp.Unpowered, Owner, null);
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
            return;
        await PowerCmd.Remove(this);
    }
}