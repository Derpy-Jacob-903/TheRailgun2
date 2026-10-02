
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2.TheRailgun2Code.Compat;

internal static class AncientsAwakenedCompat
{
    private const string PerfectedPoolTypeName =
        "AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix.PerfectedPool";

    private const string CustomPerfectedCardExtensionTypeName =
        "AncientsAwakened.AncientsAwakenedCode.Extensions.CustomPerfectedCardExtension";

    private const string CustomExperimentalCardExtensionTypeName =
        "AncientsAwakened.AncientsAwakenedCode.Extensions.CustomExperimentalCardExtension";

    private const string AncientScepterTypeName =
        "AncientsAwakened.AncientsAwakenedCode.Relics.Mithrix.AncientScepter";

    private const string AddExperimentalCardMethodName =
        "AddExperimentalCardForCustomCharacters";

    private const string CustomPerfectedStrikeCardsFieldName =
        "CustomPerfectedStrikeCards";

    private const string CustomPerfectedDefendCardsFieldName =
        "CustomPerfectedDefendCards";

    private const string PerfectedStrikeCacheFieldName =
        "_perfectedStrikeUpgrades";

    private const string PerfectedDefendCacheFieldName =
        "_perfectedDefendUpgrades"; 

    private static bool _hasLookedForPerfectedPool;
    private static CardPoolModel? _perfectedPool;

    private static bool _perfectedStrikeRegistered;
    private static bool _perfectedDefendRegistered;
    private static bool _experimentalSerumCardRegistered;

    public static bool IsLoaded =>
        FindType(PerfectedPoolTypeName) != null
        || FindType(CustomPerfectedCardExtensionTypeName) != null
        || FindType(CustomExperimentalCardExtensionTypeName) != null;

    public static CardPoolModel GetPerfectedPoolOrFallback(
        CardPoolModel fallback)
    {
        if (_hasLookedForPerfectedPool)
            return _perfectedPool ?? fallback;

        _hasLookedForPerfectedPool = true;

        Type? perfectedPoolType = FindType(PerfectedPoolTypeName);

        if (perfectedPoolType == null)
            return fallback;

        MethodInfo? cardPoolMethod = typeof(ModelDb)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(method =>
                method.Name == "CardPool"
                && method.IsGenericMethodDefinition
                && method.GetGenericArguments().Length == 1
                && method.GetParameters().Length == 0
            );

        if (cardPoolMethod == null)
            return fallback;

        object? pool = cardPoolMethod
            .MakeGenericMethod(perfectedPoolType)
            .Invoke(null, null);

        _perfectedPool = pool as CardPoolModel;

        return _perfectedPool ?? fallback;
    }

    public static bool RegisterPerfectedStrikeForCoco(
        CardModel perfectedStrikeCard)
    {
        if (_perfectedStrikeRegistered)
            return true;

        bool registered = RegisterPerfectedReplacement(
            CustomPerfectedStrikeCardsFieldName,
            ModelDb.Card<StrikeRailgun>().Id,
            perfectedStrikeCard.Id
        );

        if (!registered)
            return false;

        _perfectedStrikeRegistered = true;

        InvalidateAncientScepterCaches();

        return true;
    }

    public static bool RegisterPerfectedDefendForCoco(
        CardModel perfectedDefendCard)
    {
        if (_perfectedDefendRegistered)
            return true;

        bool registered = RegisterPerfectedReplacement(
            CustomPerfectedDefendCardsFieldName,
            ModelDb.Card<DefendRailgun>().Id,
            perfectedDefendCard.Id
        );

        if (!registered)
            return false;

        _perfectedDefendRegistered = true;

        InvalidateAncientScepterCaches();

        return true;
    }

    public static bool RegisterExperimentalSerumCardForCoco(
        CardModel cardModel)
    {
        if (_experimentalSerumCardRegistered)
            return true;

        Type? extensionType = FindType(CustomExperimentalCardExtensionTypeName);

        if (extensionType == null)
            return false;

        MethodInfo? addExperimentalCardMethod = extensionType
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(method =>
                method.Name == AddExperimentalCardMethodName
                && method.GetParameters().Length == 2
            );

        if (addExperimentalCardMethod == null)
            return false;

        try
        {
            addExperimentalCardMethod.Invoke(
                null,
                new object?[]
                {
                    cardModel,
                    ModelDb.Character<TheRailgun2.TheRailgun2Code.Character.TheRailgun2>()
                }
            );
        }
        catch
        {
            return false;
        }

        _experimentalSerumCardRegistered = true;

        return true;
    }

    private static bool RegisterPerfectedReplacement(
        string dictionaryFieldName,
        ModelId starterCardId,
        ModelId perfectedCardId)
    {
        Type? extensionType = FindType(CustomPerfectedCardExtensionTypeName);

        if (extensionType == null)
            return false;

        FieldInfo? dictionaryField = extensionType.GetField(
            dictionaryFieldName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static
        );

        if (dictionaryField == null)
            return false;

        if (dictionaryField.GetValue(null) is not IDictionary dictionary)
            return false;

        ModelId characterId = ModelDb.Character<TheRailgun2.TheRailgun2Code.Character.TheRailgun2>().Id;

        if (dictionary.Contains(characterId))
            dictionary.Remove(characterId);

        dictionary[starterCardId] = perfectedCardId;

        return true;
    }

    private static void InvalidateAncientScepterCaches()
    {
        Type? ancientScepterType = FindType(AncientScepterTypeName);

        if (ancientScepterType == null)
            return;

        FieldInfo? strikeCacheField = ancientScepterType.GetField(
            PerfectedStrikeCacheFieldName,
            BindingFlags.NonPublic | BindingFlags.Static
        );

        FieldInfo? defendCacheField = ancientScepterType.GetField(
            PerfectedDefendCacheFieldName,
            BindingFlags.NonPublic | BindingFlags.Static
        );

        strikeCacheField?.SetValue(null, null);
        defendCacheField?.SetValue(null, null);
    }

    private static Type? FindType(string fullTypeName)
    {
        Type? type = Type.GetType(
            $"{fullTypeName}, AncientsAwakened",
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