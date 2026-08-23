using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class ElectricalStorm() : TheRailgun2Card(-1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(0, ValueProp.Move),
        /*new CalculationBaseVar(0M),
        new ExtraDamageVar(1M),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => Owner.PlayerCombatState?.Energy ?? 0)),*/
        //new RepeatVar(2)
        new PowerVar<LockOnPower>(0)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Energy)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        /*if (IsUpgraded)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue + ResolveEnergyXValue())
                .FromCard(cardPlay.Card, cardPlay).TargetingAllOpponents(CombatState)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
        }
        else
        {*/
        if (CombatState != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue + ResolveEnergyXValue())
                .FromCard(cardPlay.Card, cardPlay).TargetingAllOpponents(CombatState)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
            await PowerCmd.Apply<LockOnPower>(choiceContext, CombatState.Enemies,
                DynamicVars["LockOnPower"].BaseValue + ResolveEnergyXValue(), Owner.Creature, this);
        }

        //}
        //await PlayerCmd.GainEnergy( ResolveEnergyXValue(), Owner);
    }
    
    protected override void OnUpgrade() => this.DynamicVars["LockOnPower"].UpgradeValueBy(1M);
    
    protected override bool ShouldGlowRedInternal => Owner.PlayerCombatState != null && this.CombatState != null && (DynamicVars.Damage.BaseValue + Hook.ModifyXValue(this.CombatState, this, 0) + Owner.PlayerCombatState.Energy ) == 0 && (Hook.ModifyXValue(this.CombatState, this, 0) + Owner.PlayerCombatState.Energy) == 0;

    public override TargetType TargetType => TargetType.AllEnemies;
}