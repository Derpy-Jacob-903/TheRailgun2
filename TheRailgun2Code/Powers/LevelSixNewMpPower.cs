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
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class IonDelugePower : TheRailgun2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Evoke), HoverTipFactory.FromOrb<LightningOrb>()];
    
    private bool _channelingFromLevelSix;

    public override async Task AfterOrbChanneled(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel orb)
    {
        if (_channelingFromLevelSix || player.Creature != Applier || orb is not VoltOrb || Owner.Player == null) return;
        await MyAfterOrbChanneled(choiceContext, Owner.Player, orb);
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
                await OrbCmd.Channel<VoltOrb>(choiceContext, player);
            }
        }
        finally
        {
            _channelingFromLevelSix = false;
        }
    }
}