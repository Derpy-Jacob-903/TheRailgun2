using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Powers;

public abstract class TheRailgun2Power : CustomPowerModel
{
    //Loads from TheRailgun2/images/powers/your_power.png
    public override string CustomPackedIconPath => ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath()) ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath() : $"power.png".PowerImagePath();
    public override string CustomBigIconPath => ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath()) ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath() : $"power.png".BigPowerImagePath();
}