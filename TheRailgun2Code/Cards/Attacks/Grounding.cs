using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;
public class GroundingNew() : TheRailgun2Card(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10m, ValueProp.Move),
        ..MakeCalculatedVar("Calculated", 0, (model, creature) =>
        {
            return model.CombatState != null ? model.CombatState.Enemies.Sum(c => c.GetPowerAmount<LockOnPower>()) : 0;
        }, 2)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<LockOnPower>()
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await OrbCmd.AddSlots(Owner, DynamicVars.Repeat.IntValue);
        if (CombatState != null && play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash").Execute(context);
            var _LockOn = ((CalculatedVar)DynamicVars["Calculated"]).Calculate(play.Target);
            foreach (var c in CombatState.Enemies)
            {
                await PowerCmd.Remove<LockOnPower>(c);
            }
            await PowerCmd.Apply<LockOnPower>(context, play.Target, _LockOn, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(1M);
}