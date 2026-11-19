using BaseLib.Extensions;
using BaseLib.Patches.Content;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Compat;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

[Pool(typeof(EventCardPool))]
public class IonizedBlade : TheRailgun2Card
{
    public IonizedBlade() : base(3,
        CardType.Attack, CardRarity.Event,
        TargetType.AnyEnemy)
    {
        var mysteriousFlashlightType =
            AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomMysteriousFlashlightExtension");
        if (mysteriousFlashlightType != null)
            CustomContentDictionary.AddModel(GetType());
        var brokenBladeType =
            AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomBrokenBladeExtension");
        if (brokenBladeType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(brokenBladeType, "AddBladeCardForCustomCharacter");
            addMethod.Invoke(null, [this, ModelDb.Character<Character.TheRailgun2>()]);
        }
    }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8M, ValueProp.Move)
    ];

    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TheRailgun2CardPool>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal am =
            (Decimal)(await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay)
                .Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext)).Results
            .SelectMany<List<DamageResult>, DamageResult>(
                (Func<List<DamageResult>, IEnumerable<DamageResult>>)(r => (IEnumerable<DamageResult>)r))
            .Sum<DamageResult>((Func<DamageResult, int>)(r => r.TotalDamage));
        for (int i = 0; i < am; i++)
        {
            await OrbCmd.Channel<VoltOrb>(choiceContext, Owner);
        }
        await PowerCmd.Apply<LockOnPower>(choiceContext, cardPlay.Target, am, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}