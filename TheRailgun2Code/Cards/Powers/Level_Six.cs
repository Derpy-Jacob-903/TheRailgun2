using BaseLib.Abstracts;
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

[Pool(typeof(DeprecatedCardPool))]
public class LevelSix() : TheRailgun2Card(5,
    CardType.Power, CardRarity.Status,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(2),
        new EnergyVar(6),
        new PowerVar<StrengthPower>(6),
        new PowerVar<FocusPower>(6),
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Energy), 
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>()
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        /*for (int i = 0; i < DynamicVars.Energy.BaseValue; i++)
        {
            await OrbCmd.Channel<PlasmaOrb>(context, Owner);
        }*/
        //await CreatureCmd.Damage(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,  this);
        await PlayerCmd.GainEnergy(DynamicVars["Power"].BaseValue, Owner);
        await PowerCmd.Apply<StrengthPower>(context, Owner.Creature, DynamicVars["StrengthPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(context, Owner.Creature, DynamicVars["DexterityPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<LevelSixPower>(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.HpLoss.UpgradeValueBy(-1M);
    } 
}