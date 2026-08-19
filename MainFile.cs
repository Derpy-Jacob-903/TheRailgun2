using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "TheRailgun2"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();
        
        var assembly = Assembly.GetExecutingAssembly();
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);
        
        /*var deckboxType = AccessTools.TypeByName("MoreNeow.MoreNeowCode.Relics.Complex.UnfamiliarDeckbox");
        if (deckboxType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(deckboxType, "AddCharacterDeck");
            addMethod.Invoke(null, [ModelDb.GetId<TheRailgun2Code.Character.TheRailgun2>(), ModelDb.GetId<Zap>(), ModelDb.GetId<Ax>()]);
        } */
    }
}