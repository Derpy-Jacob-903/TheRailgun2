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
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class TemporaryThornsPower : TheRailgun2Power, ITemporaryPower
{
    private bool _shouldIgnoreNextInstance = false;
    protected virtual bool IsPositive => true;
    private int Sign => !IsPositive ? -1 : 1;
    protected virtual bool RemovedAfterOwnTurn => false;
    public void IgnoreNextInstance()
    {
        _shouldIgnoreNextInstance = true;
    }
    public override async Task BeforeApplied(
        Creature target,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (this._shouldIgnoreNextInstance)
        {
            this._shouldIgnoreNextInstance = false;
        }
        else
        {
            ThornsPower strengthPower = await PowerCmd.Apply<ThornsPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), target, (Decimal) this.Sign * amount, applier, cardSource, true);
        }
    }

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        Decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (amount == (Decimal) Amount || power != this)
            return;
        if (_shouldIgnoreNextInstance)
        {
            _shouldIgnoreNextInstance = false;
        }
        else
        {
            ThornsPower strengthPower = await PowerCmd.Apply<ThornsPower>(choiceContext, Owner, (Decimal) Sign * amount, applier, cardSource, true);
        }
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (participants.Contains<Creature>(Owner))
            return;
        Flash();
        await PowerCmd.Remove((PowerModel) this);
        ThornsPower strengthPower = await PowerCmd.Apply<ThornsPower>(choiceContext, Owner, (Decimal) (-Sign * Amount), Owner, (CardModel) null);
    }
    public AbstractModel OriginModel => ModelDb.Power<ThornsPower>();
    public PowerModel InternallyAppliedPower => ModelDb.Power<ThornsPower>();
    public override PowerType Type => !this.IsPositive ? PowerType.Debuff : PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}