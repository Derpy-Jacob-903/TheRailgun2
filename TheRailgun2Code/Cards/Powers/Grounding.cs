using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Grounding() : TheRailgun2Card(0,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(1),
        new PowerVar<ConsumingShadowPower>(1)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<ConsumingShadowPower>()
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await OrbCmd.AddSlots(Owner, DynamicVars.Repeat.IntValue);
        await CommonActions.ApplySelf<ConsumingShadowPower>(context, this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
}