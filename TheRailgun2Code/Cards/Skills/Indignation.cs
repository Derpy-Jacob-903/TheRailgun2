using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class IndignationRailgun() : TheRailgun2Card(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(2),
        new PowerVar<LockOnPower>(3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //await CreatureCmd.TriggerAnim(this, "cast");
        if (Owner.PlayerCombatState!.OrbQueue.Orbs.Count == Owner.PlayerCombatState.OrbQueue.Capacity)
        {
            var dynVarSource = Owner.Creature.CombatState!.Enemies;
            await CommonActions.Apply<LockOnPower>(choiceContext, dynVarSource, this, false);
        }
        else
        {
            for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
            {
                await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
            }
        }
    }

    protected override void OnUpgrade() => this.DynamicVars["LockOnPower"].UpgradeValueBy(2);
}