using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;

namespace TheRailgun2.TheRailgun2Code.Character;

public static class EchoOrb
{
    //evil and fucked up this wasn't added
    //note to self: pr this to baselib
    // ReSharper disable once MemberCanBePrivate.Global
    public static async Task EvokeSpecific(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel evokedOrb,
        bool dequeue = true)
    {
        if (player.PlayerCombatState == null || evokedOrb == null)
            return;
        var orbQueue = player.PlayerCombatState.OrbQueue;
        if (!orbQueue.Orbs.Contains(evokedOrb))
            return;
        choiceContext.PushModel(evokedOrb);
        await Evoke2(choiceContext, player, evokedOrb, dequeue);
        choiceContext.PopModel(evokedOrb);
    }

    public static async Task EvokeFirstOf<OrbType>(
        PlayerChoiceContext choiceContext,
        Player player,
        bool dequeue = true)
        where OrbType : OrbModel
    {
        if (player.PlayerCombatState == null)
            return;
        var orbQueue = player.PlayerCombatState.OrbQueue;
        OrbModel orb = orbQueue.Orbs.OfType<OrbType>().FirstOrDefault();
        if (orb == null)
            return;
        choiceContext.PushModel(orb);
        await Evoke2(choiceContext, player, orb, dequeue);
        choiceContext.PopModel(orb);
    }

    public static async Task RemoveFirstOf<OrbType>(
        PlayerChoiceContext choiceContext,
        Player player,
        bool dequeue = true)
        where OrbType : OrbModel
    {
        if (player.PlayerCombatState == null)
            return;
        var orbQueue = player.PlayerCombatState.OrbQueue;
        OrbModel orb = orbQueue.Orbs.OfType<OrbType>().FirstOrDefault();
        if (orb == null)
            return;
        choiceContext.PushModel(orb);
        await RemoveOrb(choiceContext, player, orb, dequeue);
        choiceContext.PopModel(orb);
    }
    
    public static async Task<int> EvokeAllOf<OrbType>(
        PlayerChoiceContext choiceContext, 
        Player player,
        Func<Task> task = null,
        bool dequeue = true)
        where OrbType : OrbModel
    {
        var count = 0;
        if (player.PlayerCombatState == null) return 0;
        var orbs = player.PlayerCombatState.OrbQueue.Orbs.ToList();
        foreach (var orb in orbs.OfType<OrbType>())
        {
            await RemoveOrb(choiceContext, player, orb, dequeue);
            count++;
            if (task != null) await task();
        }
        return count;
    }
    
    public static async Task<int> RemoveAllOf<OrbType>(
        PlayerChoiceContext choiceContext, 
        Player player,
        Func<Task> task = null,
        bool dequeue = true)
        where OrbType : OrbModel
    {
        var count = 0;
        if (player.PlayerCombatState == null) return 0;
        var orbs = player.PlayerCombatState.OrbQueue.Orbs.ToList();
        foreach (var orb in orbs.OfType<OrbType>())
        {
            await EvokeSpecific(choiceContext, player, orb, dequeue);
            count++;
            if (task != null) await task();
        }
        return count;
    }

    private static async Task Evoke2(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel evokedOrb,
        bool dequeue = true)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
        if (player.PlayerCombatState == null || player.Creature.CombatState == null || orbQueue.Orbs.Count <= 0)
            return;
        bool removed = false;
        if (dequeue)
        {
            removed = orbQueue.Remove(evokedOrb);
            NCombatRoom.Instance?.GetCreatureNode(player.Creature)?.OrbManager?.EvokeOrbAnim(evokedOrb);
        }
        choiceContext.PushModel((AbstractModel) evokedOrb);
        IEnumerable<Creature> targets = await evokedOrb.Evoke(choiceContext);
        choiceContext.PopModel((AbstractModel) evokedOrb);
        await Hook.AfterOrbEvoked(choiceContext, player.Creature.CombatState, evokedOrb, targets);
        if (!removed)
            return;
        evokedOrb.RemoveInternal();
    }
    
    private static async Task RemoveOrb(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel evokedOrb,
        bool dequeue = true)
    {
        if (CombatManager.Instance.IsOverOrEnding || player.PlayerCombatState == null)
            return;
        var orbQueue = player.PlayerCombatState.OrbQueue;
        if (player.PlayerCombatState == null || player.Creature.CombatState == null || orbQueue.Orbs.Count <= 0)
            return;
        var removed = false;
        if (dequeue)
        {
            removed = orbQueue.Remove(evokedOrb);
        }
        choiceContext.PushModel(evokedOrb);
        choiceContext.PopModel(evokedOrb);
        if (!removed)
            return;
        evokedOrb.RemoveInternal();
    }
}