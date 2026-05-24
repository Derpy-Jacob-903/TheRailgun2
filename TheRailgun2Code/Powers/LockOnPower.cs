using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class LockOnPower : TheRailgun2Power
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("DamageIncrease", 1.5M)];

    /*public override Decimal ModifyOrbValue(OrbModel orb, Decimal value)
    {
        if (orb is FrostOrb) return value;
        return this.Owner.Player != orb.Owner ? value : Math.Max(value * DynamicVars["DamageIncrease"].BaseValue, 0M);
    }*/
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