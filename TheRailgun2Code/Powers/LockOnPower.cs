using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class LockOnPower : TheRailgun2Power
{
    
    /*public override Decimal ModifyOrbValue(OrbModel orb, Decimal value)
    {
        if (orb is FrostOrb) return value;
        return this.Owner.Player != orb.Owner ? value : Math.Max(value * DynamicVars["DamageIncrease"].BaseValue, 0M);
    }*/
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("DamageIncrease", 1.5M)];
    public override Decimal ModifyDamageMultiplicative(
        Creature target,
        decimal amount,
        ValueProp props,
        Creature dealer,
        CardModel cardSource)
    {
        if (target != this.Owner || !props.HasFlag(Enums.Orb))
            return 1M;
        var amount1 = this.DynamicVars["DamageIncrease"].BaseValue;
        var relic = target.Player?.GetRelic<LockOnPaperPhrog>();
        if (relic != null)
            amount1 = relic.ModifyLockOnMultiplier(target, amount1, props, dealer, cardSource);
        return amount1;
    }
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
            return;
        await PowerCmd.TickDownDuration(this);
    }
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
}