using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class ScatterRailgun() : TheRailgun2Card(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(16M, ValueProp.Move)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            if (this.IsUpgraded)
                return
                [
                    HoverTipFactory.FromPower<ArtifactPower>(),
                    HoverTipFactory.Static(StaticHoverTip.Block)
                ];
            return
            [
                HoverTipFactory.Static(StaticHoverTip.Block),
            ];
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
        {
            foreach (var Target in CombatState.HittableEnemies)
            {
                if (IsUpgraded) await PowerCmd.Remove<ArtifactPower>(Target);
                await CreatureCmd.LoseBlock(Target, Target.Block);
            }
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}