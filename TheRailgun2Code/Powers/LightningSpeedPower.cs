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

public class LightningSpeedPower : TheRailgun2Power
{
    public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer,
        CardModel cardSource)
    {
        if (dealer != this.Owner || !props.IsPoweredAttack())
            return 1M;
        return 1.25M;
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
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}