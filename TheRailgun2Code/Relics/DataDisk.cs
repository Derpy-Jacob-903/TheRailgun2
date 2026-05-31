using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Relics;

public class DataDiskRailgun() : TheRailgun2Relic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FocusPower>(1M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<FocusPower>()
        Marbles
    ];
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return;
        Flash();
        FocusPower focusPower = await PowerCmd.Apply<FocusPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["FocusPower"].BaseValue, Owner.Creature, null);
    }
}