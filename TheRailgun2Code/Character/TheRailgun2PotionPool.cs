using BaseLib.Abstracts;
using Godot;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Character;

public class TheRailgun2PotionPool : CustomPotionPoolModel
{
    public override string BigEnergyIconPath => "card_default_gray_orb.png".CharacterUiPath();
    public override string TextEnergyIconPath => "card_small_orb.png".CharacterUiPath();
    public override Color LabOutlineColor => TheRailgun2.Color;
}