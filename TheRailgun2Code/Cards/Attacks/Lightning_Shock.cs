using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class LightningShock() : TheRailgun2Card(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    private const string _increaseKey = "Increase";
    private Decimal _extraDamageFromClawPlays;
    private Decimal ExtraDamageFromClawPlays
    {
        get => this._extraDamageFromClawPlays;
        set
        {
            this.AssertMutable();
            this._extraDamageFromClawPlays = value;
        }
    }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        (DynamicVar) new DamageVar(3M, ValueProp.Move),
        new DynamicVar("Increase", 2M)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(cardPlay.Card, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
        if (Owner.PlayerCombatState != null)
        {
            var claws = Owner.PlayerCombatState.AllCards.OfType<LightningShock>();
            var baseValue = DynamicVars["Increase"].BaseValue;
            foreach (LightningShock claw in claws)
                claw.BuffFromClawPlay(baseValue);
        }
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(3M);
    }

    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue += this.ExtraDamageFromClawPlays;
    }

    private void BuffFromClawPlay(Decimal extraDamage)
    {
        DamageVar damage = DynamicVars.Damage;
        damage.BaseValue += extraDamage;
        this.ExtraDamageFromClawPlays += extraDamage;
    }
}