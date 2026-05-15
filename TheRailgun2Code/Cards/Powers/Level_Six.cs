using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards.Powers;

public class LevelSix() : TheRailgun2Card(4,
    CardType.Power, CardRarity.Ancient,
    TargetType.AnyEnemy), ITomeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(4),
        new EnergyVar(3),
        new PowerVar<StrengthPower>(3),
        new PowerVar<FocusPower>(3),
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        for (int i = 0; i < DynamicVars.Energy.BaseValue; i++)
        {
            await OrbCmd.Channel<PlasmaOrb>(context, Owner);
        }
        //await CreatureCmd.Damage(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,  this);
        //await PlayerCmd.GainEnergy(DynamicVars["Power"].BaseValue, Owner);
        //await PowerCmd.Apply<StrengthPower>(context, Owner.Creature, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<FocusPower>(context, Owner.Creature, DynamicVars["FocusPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<LevelSixPower>(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
        this.DynamicVars.HpLoss.UpgradeValueBy(-1M);
    } 
}