using System.Reflection;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Compat;
using TheRailgun2.TheRailgun2Code.Data;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2;

[ModInitializer(nameof(Initialize))]
public partial class RailgunTwoMainFile : Node
{
    public const string ModId = "TheRailgun2"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);
        
        ModManager.OnMetricsUpload += DownfallMetrics.OnMetricsUpload;
        harmony.PatchAll();
        
        var assembly = Assembly.GetExecutingAssembly();
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);
        
        TheHeroExpansionCompat.RegisterAllForCoco();
        
        /*var deckboxType = AccessTools.TypeByName("MoreNeow.MoreNeowCode.Relics.Complex.UnfamiliarDeckbox");
        if (deckboxType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(deckboxType, "AddCharacterDeck");
            addMethod.Invoke(null, [ModelDb.GetId<TheRailgun2Code.Character.TheRailgun2>(), ModelDb.GetId<Zap>(), ModelDb.GetId<Ax>()]);
        } */
    }
    
    public static Task AfterOrbSpent(PlayerChoiceContext ctx, CardPlay play)
    {
        return HookUtils.Dispatch<IAfterOrbSpent>(play.Card.Owner.Creature.CombatState, ctx, m => m.AfterOrbSpent(ctx, play));
    }
}