using System.Diagnostics;
using BaseLib.Cards.Variables;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class LevelSixNewPower : TheRailgun2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Evoke), HoverTipFactory.FromOrb<LightningOrb>()];

    public override async Task AfterOrbEvoked(PlayerChoiceContext choiceContext, OrbModel orb, IEnumerable<Creature> targets)
    {
        if (orb.Owner.Creature != Owner)
            return;
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, Owner, (Decimal) Amount, ValueProp.Unpowered & ValueProp.Unblockable, Owner);
        VfxCmd.PlayOnCreatureCenter(Owner, "vfx/vfx_attack_blunt");
    }

    private bool _channelingFromLevelSix;

    public override async Task AfterOrbChanneled(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel orb)
    {
        if (_channelingFromLevelSix)
            return;
        await MyAfterOrbChanneled(choiceContext, player, orb);
    }

    public async Task MyAfterOrbChanneled(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel orb)
    {
        _channelingFromLevelSix = true;
        try
        {
            for (int i = 0; i < Amount; i++)
            {
                await OrbCmd.Channel<LightningOrb>(choiceContext, player);
            }
        }
        finally
        {
            _channelingFromLevelSix = false;
        }
    }
    
    /*public static bool IsOrbCaller()
    {
        return new StackTrace().GetFrames()?.Any(f =>
        {
            Type type = f.GetMethod()?.DeclaringType;

            if (type == null)
                return false;

            return typeof(LevelSixNewPower).IsAssignableFrom(type)
                   || (type.DeclaringType != null &&
                       typeof(LevelSixNewPower).IsAssignableFrom(type.DeclaringType));
        }) ?? false;
    }*/
}

/*[HarmonyPatch(
    typeof(LevelSixNewPower),
    nameof(LevelSixNewPower.AfterOrbChanneled),
    new Type[]
    {
        typeof(PlayerChoiceContext),
        typeof(Player),
        typeof(OrbModel)
    })]
public static class LevelSixNewPatch
{
        [HarmonyPrefix]
        public static bool Prefix(LevelSixNewPower __instance)
        {
            return !LevelSixNewPower.IsOrbCaller();
        }
}*/