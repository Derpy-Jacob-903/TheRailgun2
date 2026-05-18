using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Enums
{
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Conduit;
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Guardbreak;
    
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
}

