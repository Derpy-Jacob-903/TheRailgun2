using BaseLib.Abstracts;
using Godot;

namespace TheRailgun2.TheRailgun2Code.Character;

public class TheRailgun2RelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => TheRailgun2.CharacterId;
    public override Color LabOutlineColor => TheRailgun2.Color;
}