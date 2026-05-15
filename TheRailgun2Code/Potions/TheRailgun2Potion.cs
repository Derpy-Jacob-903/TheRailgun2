using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Potions;

[Pool(typeof(TheRailgun2PotionPool))]
public abstract class TheRailgun2Potion : CustomPotionModel
{
    public static string PotionImagePath(string path)
    {
        return Path.Join(MainFile.ModId, "images", "potion", path);
    }
    public override string CustomPackedImagePath => PotionImagePath($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png");
}
