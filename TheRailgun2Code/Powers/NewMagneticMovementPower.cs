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
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class NewMagneticMovementPower : TheRailgun2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public Decimal ModifyLockOnMultiplier(
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature dealer,
        CardModel cardSource) 
    {
        return target == this.Owner || !props.HasFlag(Enums.Orb) ? amount : amount + 0.01M * Amount;
    }
}