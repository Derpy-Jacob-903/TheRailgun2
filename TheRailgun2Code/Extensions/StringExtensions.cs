using Godot;

namespace TheRailgun2.TheRailgun2Code.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions
{
    public static string ImagePath(this string path)
    {
        return Path.Join(RailgunTwoMainFile.ModId, "images", path);
    }

    public static string CardImagePath(this string path)
    {
        path = Path.Join(RailgunTwoMainFile.ModId, "images", "card_portraits", path, "_p");
        if (ResourceLoader.Exists(path)) return path;

        RailgunTwoMainFile.Logger.Info("Could not find card image path: " + path);
        return Path.Join(RailgunTwoMainFile.ModId, "images", "card_portraits", "card_p.png");
    }

    public static string BigCardImagePath(this string path)
    {
        path = Path.Join(RailgunTwoMainFile.ModId, "images", "card_portraits", path, "_p");
        if (ResourceLoader.Exists(path)) return path;

        RailgunTwoMainFile.Logger.Info("Could not find big card image path: " + path);
        return Path.Join(RailgunTwoMainFile.ModId, "images", "card_portraits", "card_p.png");
    }

    public static string PowerImagePath(this string path)
    {
        path = Path.Join(RailgunTwoMainFile.ModId, "images", "powers", path);
        if (ResourceLoader.Exists(path)) return path;

        RailgunTwoMainFile.Logger.Info("Could not find power image path: " + path);
        return Path.Join(RailgunTwoMainFile.ModId, "images", "powers", "power.png");
    }

    public static string BigPowerImagePath(this string path)
    {
        path = Path.Join(RailgunTwoMainFile.ModId, "images", "powers", path);
        if (ResourceLoader.Exists(path)) return path;

        RailgunTwoMainFile.Logger.Info("Could not find big power image path: " + path);
        return Path.Join(RailgunTwoMainFile.ModId, "images", "powers", "power.png");
    }

    public static string RelicImagePath(this string path)
    {
        path = Path.Join(RailgunTwoMainFile.ModId, "images", "relics", path);
        if (ResourceLoader.Exists(path)) return path;

        RailgunTwoMainFile.Logger.Info("Could not find relic image path: " + path);
        return Path.Join(RailgunTwoMainFile.ModId, "images", "relics", "relic.png");
    }

    public static string BigRelicImagePath(this string path)
    {
        path = Path.Join(RailgunTwoMainFile.ModId, "images", "relics", path);
        if (ResourceLoader.Exists(path)) return path;

        RailgunTwoMainFile.Logger.Info("Could not find relic image path: " + path);
        return Path.Join(RailgunTwoMainFile.ModId, "images", "relics", "relic.png");
    }

    public static string CharacterUiPath(this string path)
    {
        return Path.Join(RailgunTwoMainFile.ModId, "images", "charui", path);
    }
}