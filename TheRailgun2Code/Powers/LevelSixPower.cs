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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class LevelSixPower : TheRailgun2Power
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        [HoverTipFactory.Static(StaticHoverTip.Evoke)];

    public override async Task AfterOrbEvoked(PlayerChoiceContext choiceContext, OrbModel orb, IEnumerable<Creature> targets)
    {
        if (orb.Owner.Creature != Owner)
            return;
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, Owner, (Decimal) Amount, ValueProp.Unpowered & ValueProp.Unblockable, Owner, (CardModel) null);
        VfxCmd.PlayOnCreatureCenter(Owner, "vfx/vfx_attack_blunt");
    }
}