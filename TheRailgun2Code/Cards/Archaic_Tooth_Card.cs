using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;
[Pool(typeof(DeprecatedCardPool))]
public class Fulminate2() : TheRailgun2Card(0,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(20M, ValueProp.Move),
        new RepeatVar(2),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Fulminate2 quadcast = this;
        if (cardPlay.Target != null)
            await DamageCmd.Attack(quadcast.DynamicVars.Damage.BaseValue).FromCard(quadcast, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        if (quadcast.Owner.PlayerCombatState != null && quadcast.Owner.PlayerCombatState.OrbQueue.Orbs.Count <= 0)
            return;
        await CreatureCmd.TriggerAnim(quadcast.Owner.Creature, "Cast", quadcast.Owner.Character.CastAnimDelay);
        for (int i = 0; i < quadcast.DynamicVars.Repeat.IntValue; ++i)
        {
            await OrbCmd.EvokeNext(choiceContext, quadcast.Owner, i == quadcast.DynamicVars.Repeat.IntValue - 1);
            if (i != quadcast.DynamicVars.Repeat.IntValue - 1)
                await Cmd.CustomScaledWait(0.15f, 0.25f);
        }
    }

    protected override void OnUpgrade() {
        this.DynamicVars.Damage.UpgradeValueBy(10M);
        this.DynamicVars.Repeat.UpgradeValueBy(1M);
    }
}