using Godot;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections;
using System.Reflection;
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2.TheRailgun2Code.Compat;

internal static class TheHeroExpansionCompat
{
    private const string CustomBrokenBladeExtensionTypeName =
        "TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomBrokenBladeExtension";

    private const string CustomMysteriousFlashlightExtensionTypeName =
        "TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomMysteriousFlashlightExtension";

    private const string CustomBladeCardsFieldName =
        "CustomBladeCards";

    private const string CustomFlashlightCardsFieldName =
        "CustomFlashlightCards";

    private static bool _bladeRegistered;
    private static bool _flashlightRegistered;

    public static bool IsLoaded =>
        FindType(CustomBrokenBladeExtensionTypeName) != null
        || FindType(CustomMysteriousFlashlightExtensionTypeName) != null;

    public static bool RegisterAllForCoco()
    {
        //GD.Print("[Kiryu Coco] TheHeroExpansion compat call reached.");

        bool bladeRegistered = RegisterBladeCardForCoco();
        bool flashlightRegistered = RegisterFlashlightCardForCoco();

        /*GD.Print(
            "[Kiryu Coco] TheHeroExpansion compat result: " +
            $"Blade={bladeRegistered}, Flashlight={flashlightRegistered}"
        );*/

        return bladeRegistered || flashlightRegistered;
    }

    public static bool RegisterBladeCardForCoco()
    {
        if (_bladeRegistered)
            return true;

        bool registered = RegisterCharacterCardReplacement(
            CustomBrokenBladeExtensionTypeName,
            CustomBladeCardsFieldName,
            ModelDb.GetId<IonizedBlade>(),
            "Blade"
        );

        if (!registered)
            return false;

        _bladeRegistered = true;

        return true;
    }

    public static bool RegisterFlashlightCardForCoco()
    {
        if (_flashlightRegistered)
            return true;

        bool registered = RegisterCharacterCardReplacement(
            CustomMysteriousFlashlightExtensionTypeName,
            CustomFlashlightCardsFieldName,
            ModelDb.GetId<Backtrack>(),
            "Flashlight"
        );

        if (!registered)
            return false;

        _flashlightRegistered = true;

        return true;
    }

    private static bool RegisterCharacterCardReplacement(
        string extensionTypeName,
        string dictionaryFieldName,
        ModelId cardId,
        string label)
    {
        Type? extensionType = FindType(extensionTypeName);

        if (extensionType == null)
        {
            GD.Print(
                "[Kiryu Coco] TheHeroExpansion compat failed: " +
                $"could not find type {extensionTypeName}"
            );

            return false;
        }

        FieldInfo? dictionaryField = extensionType.GetField(
            dictionaryFieldName,
            BindingFlags.NonPublic
            | BindingFlags.Public
            | BindingFlags.Static
        );

        if (dictionaryField == null)
        {
            GD.Print(
                "[Kiryu Coco] TheHeroExpansion compat failed: " +
                $"could not find field {dictionaryFieldName} on {extensionType.FullName}"
            );

            return false;
        }

        if (dictionaryField.GetValue(null) is not IDictionary dictionary)
        {
            GD.Print(
                "[Kiryu Coco] TheHeroExpansion compat failed: " +
                $"{dictionaryFieldName} was not an IDictionary"
            );

            return false;
        }

        ModelId characterId = ModelDb.GetId<TheRailgun2.TheRailgun2Code.Character.TheRailgun2>();

        if (dictionary.Contains(characterId))
            dictionary.Remove(characterId);

        dictionary[characterId] = cardId;

        GD.Print(
            "[Kiryu Coco] TheHeroExpansion compat registered " +
            $"{label}: character={characterId}, card={cardId}"
        );

        return true;
    }

    private static Type? FindType(
        string fullTypeName)
    {
        Type? type = Type.GetType(
            $"{fullTypeName}, TheHeroExpansion",
            throwOnError: false
        );

        if (type != null)
            return type;

        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = assembly.GetType(
                fullTypeName,
                throwOnError: false
            );

            if (type != null)
                return type;
        }

        return null;
    }
}