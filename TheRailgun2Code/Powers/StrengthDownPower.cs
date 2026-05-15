using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Powers;

[Obsolete]
public class StrengthDownPower : TheRailgun2Power
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public string CustomPackedIconPath => "charge_power.png".PowerImagePath();
    public string CustomBigIconPath => "charge_power.png".BigPowerImagePath();
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        var power = this;
        if (side != power.Owner.Side)
            return;
        power.Flash();
        await PowerCmd.Remove((PowerModel) power);
        StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, power.Owner, -power.Amount, power.Owner, null);
    }
}