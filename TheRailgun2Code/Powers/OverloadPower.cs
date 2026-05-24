using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class OverloadPower : TheRailgun2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.ForEnergy(this)];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DisplayVar<PowerModel>("HpLoss", (model) => (model.Amount * 5).ToString())
    ];

    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != this.Owner || originalCost <= 0M)
            return false;
        modifiedCost = originalCost - Amount;
        if (modifiedCost < 0M)
            modifiedCost = 0M;
        return true;
    }
    
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains<Creature>(Owner))
            return;
            //hailstormPower.Flash();
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, Owner, (Decimal) Amount * 5, ValueProp.Unpowered, Owner, (CardModel) null);
            VfxCmd.PlayOnCreatureCenter(Owner, "vfx/vfx_attack_blunt");
    }
}