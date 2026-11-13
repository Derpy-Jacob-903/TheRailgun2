using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Relics;

public class MetronomeRailgun() : TheRailgun2Relic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FocusPower>(2M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<FocusPower>()
    ];

    public override async Task AfterOrbEvoked(PlayerChoiceContext choiceContext, OrbModel orb, IEnumerable<Creature> targets)
    {
        MetronomeRailgun centennialPuzzle = this;
        if (orb.Owner != this.Owner || centennialPuzzle.UsedThisCombat)
            return;
        centennialPuzzle.Flash();
        centennialPuzzle.UsedThisCombat = true;
        await PowerCmd.Apply<FocusPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["FocusPower"].BaseValue, Owner.Creature, null);
    }
    
    public bool _usedThisCombat;
    
    public bool UsedThisCombat
    {
        get => this._usedThisCombat;
        set
        {
            if (this._usedThisCombat == value)
                return;
            this.AssertMutable();
            this._usedThisCombat = value;
        }
    }
    
    public override Task AfterCombatEnd(CombatRoom _)
    {
        this.UsedThisCombat = false;
        return Task.CompletedTask;
    }
}