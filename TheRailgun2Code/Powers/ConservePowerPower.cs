using BaseLib.Abstracts;
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
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class ConservePowerPower : TheRailgun2Power, IAfterOrbSpent
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override int DisplayAmount => this.DynamicVars["CardsLeft"].IntValue;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("CardsLeft", 2)];
    public async Task AfterOrbSpent(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.Owner.Creature != this.Owner) return;
        if (DynamicVars["CardsLeft"].BaseValue < 1 || Owner.Player == null) return;
        DynamicVars["CardsLeft"].BaseValue--;
        InvokeDisplayAmountChanged();
        await OrbCmd.Channel<VoltOrb>(new ThrowingPlayerChoiceContext(), Owner.Player);
    }
    protected override object InitInternalData() => (object) new PanachePower.Data();
    public override Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return Task.CompletedTask;
        this.DynamicVars["CardsLeft"].BaseValue = Amount;
        this.InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }
}