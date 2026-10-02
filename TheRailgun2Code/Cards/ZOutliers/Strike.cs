using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Compat;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class StrikeRailgunPerfect : TheRailgun2Card
{
    public StrikeRailgunPerfect() : base(1,
        CardType.Attack, CardRarity.Token,
        TargetType.AnyEnemy)
    {
        AncientsAwakenedCompat.RegisterPerfectedStrikeForCoco(this);
    }
    
    public override Material CreateCustomBannerMaterial => ShaderUtils.GenerateHsv(355f / 360f, 1f, 1f);

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [Enums.Discharge];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9M, ValueProp.Move),
        new PowerVar<LockOnPower>(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(cardPlay.Card, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
            await PowerCmd.Apply<LockOnPower>(choiceContext, cardPlay.Target, DynamicVars["LockOnPower"].BaseValue,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(3M);
        this.DynamicVars.Damage.UpgradeValueBy(3M);
    }
}