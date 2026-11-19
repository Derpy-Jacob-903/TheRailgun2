
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using BaseLib.Utils.ModInterop;
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2.TheRailgun2Code.Compat;

[ModInterop("AncientsAwakened", "AncientsAwakened.AncientsAwakenedCode.CrossModCompatability.CustomPerfectedCardExtension")]
public static class AncientsAwakenedInterop 
{
    public static void AddPerfectedCardForCustomCharacters(this CardModel cardModel, CardModel originalCardModel)
    {
        // Since this is a void, this method can be empty.
    }

    public static CardPoolModel PerfectedCardPool(CardPoolModel optionalFallBack)
    {
        return optionalFallBack; // Alternatively, you could call ModelDb.CardPool<MyCharacterPool>() directly and just pass null whenever this is called.
    }
}

[ModInterop("AncientsAwakened", "AncientsAwakened.AncientsAwakenedCode.CrossModCompatability.CustomExperimentalCardExtension")]
public static class AncientsAwakenedExperimentalInterop
{
    public static void AddExperimentalCardForCustomCharacters(this CardModel cardModel, CharacterModel characterModel)
    {
    }
}