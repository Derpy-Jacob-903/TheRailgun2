using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class NewMagneticFlightPower : TheRailgun2Power
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("DamageDecrease", 0.5M)];

    public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer,
        CardModel cardSource, CardPlay cardPlay)
    {
        if (target != this.Owner || !props.IsPoweredAttack())
            return 1M;
        Decimal amount1 = this.DynamicVars["DamageDecrease"].BaseValue;
        return amount1;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature dealer, CardModel cardSource)
    {
        if (target != this.Owner || !props.IsPoweredAttack())
            return;
        await PowerCmd.Decrement(this);
    }

    public override Decimal GetScaledAmountForMultiplayer(
        ICombatState combatState,
        Creature? applier,
        Decimal amount,
        Creature target,
        CardModel? cardSource)
    {
        return ((combatState.Players.Count - 1) * 2 + 1) * amount * 2;
    }
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}