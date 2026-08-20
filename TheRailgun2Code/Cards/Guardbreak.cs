using System.Diagnostics;
using System.Reflection;
using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public static class Enums
{
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)] [Obsolete]
    public static CardKeyword Conduit;
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Discharge;
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)] [Obsolete]
    public static CardKeyword Guardbreak;
    [CustomEnum]
    public static CardTag Ferrous;
    [CustomEnum]
    public static ValueProp Orb;
    public static bool IsOrbCaller()
    {
	    return new StackTrace().GetFrames()?.Any(f =>
	    {
		    Type type = f.GetMethod()?.DeclaringType;

		    if (type == null)
			    return false;

		    return typeof(OrbModel).IsAssignableFrom(type)
		           || (type.DeclaringType != null &&
		               typeof(OrbModel).IsAssignableFrom(type.DeclaringType));
	    }) ?? false;
    }
}

[HarmonyPatch(
	typeof(CreatureCmd),
	nameof(CreatureCmd.Damage),
	new Type[]
	{
		typeof(PlayerChoiceContext),
		typeof(IEnumerable<Creature>),
		typeof(decimal),
		typeof(ValueProp),
		typeof(Creature)
	})]
public static class OrbDamagePatchMulti
{
	[HarmonyPrefix]
	public static void Prefix(ref ValueProp props)
	{
		if (!props.HasFlag(Enums.Orb) && Enums.IsOrbCaller())
		{
			props |= Enums.Orb;
		}
	}
}
[HarmonyPatch(
	typeof(CreatureCmd),
	nameof(CreatureCmd.Damage),
	new Type[]
	{
		typeof(PlayerChoiceContext),
		typeof(Creature),
		typeof(decimal),
		typeof(ValueProp),
		typeof(Creature)
	})]
public static class OrbDamagePatchSingle
{
	[HarmonyPrefix]
	public static void Prefix(ref ValueProp props)
	{
		if (!props.HasFlag(Enums.Orb) && Enums.IsOrbCaller())
		{
			props |= Enums.Orb;
		}
	}
}
    /*[HarmonyPatch(typeof(LightningOrb), nameof(LightningOrb.BeforeTurnEndOrbTrigger))]
    public static class IsReleaseGamePatch
    {
        static void Prefix(ref bool __result)
        {
            LightningOrb lightningOrb = this;
            List<Creature> list = lightningOrb.CombatState.GetOpponentsOf(lightningOrb.Owner.Creature).Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList<Creature>();
            if (list.Count == 0)
                return (IEnumerable<Creature>) Array.Empty<Creature>();
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: object of a compiler-generated type is created
                IReadOnlyList<Creature> targets = target == null ? (IReadOnlyList<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(lightningOrb.Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) list)) : (IReadOnlyList<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(target);
            foreach (Creature target1 in (IEnumerable<Creature>) targets)
                VfxCmd.PlayOnCreature(target1, "vfx/vfx_attack_lightning");
            lightningOrb.PlayEvokeSfx();
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) targets, value, ValueProp.Unpowered, lightningOrb.Owner.Creature);
            return (IEnumerable<Creature>) targets;
            
        }
    }*/
    public class RailgunKeywordSingleton() : CustomSingletonModel(HookType.Combat)
    {
#pragma warning disable CS0612 // Type or member is obsolete 
	    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
	    {
		    if (card.Keywords.Contains(Enums.Conduit))
		    {
			    await OrbCmd.Channel<LightningOrb>(choiceContext, card.Owner);
		    }
		    if (card.Keywords.Contains(Enums.Discharge))
		    {
			    await CardCmd.AutoPlay(choiceContext, card, null, AutoPlayType.SlyDiscard, skipCardPileVisuals: true);
			    await CardPileCmd.Add(card, PileType.Exhaust, skipVisuals: true);
		    }
	    }
	    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
	    {
		    if (card.Keywords.Contains(Enums.Discharge))
		    {
			    await CardCmd.AutoPlay(choiceContext, card, null, AutoPlayType.SlyDiscard, skipCardPileVisuals: true);
			    await CardPileCmd.Add(card, PileType.Exhaust, skipVisuals: true);
		    }
	    }

	    public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props,
		    Creature dealer, CardModel cardSource)
	    {
		    if (cardSource != null && cardSource.Keywords.Contains(Enums.Guardbreak))
		    {
			    await CreatureCmd.LoseBlock(choiceContext, target, target.Block, dealer);
		    }
	    }
    }
#pragma warning restore CS0612 // Type or member is obsolete
