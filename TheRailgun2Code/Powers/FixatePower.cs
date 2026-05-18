using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class FixatePower : TemporaryFocusPower, ICustomPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override AbstractModel OriginModel => ModelDb.Card<Charge>();
    public string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();

    public override async Task AfterRemoved(Creature oldOwner)
    {
        if (oldOwner.Player != null) await OrbCmd.AddSlots(oldOwner.Player, Amount);
    }
}