using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class ElectronBurst() : TheRailgun2Card(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self), ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(2)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [Enums.Discharge];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.PlayerCombatState != null)
            foreach (OrbModel orb in Owner.PlayerCombatState.OrbQueue.Orbs)
            {
                if (orb is LightningOrb or VoltOrb)
                {
                    for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
                    {
                        await OrbCmd.Passive(choiceContext, orb, null);
                    }
                }
            }
    }
    protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
    protected override bool ShouldGlowRedInternal => Owner.PlayerCombatState != null && Owner.PlayerCombatState.OrbQueue.Orbs.Any(orb => orb is LightningOrb or VoltOrb);
    public CardModel GetTranscendenceTransformedCard()
    {
        return ModelDb.Card<ElectronBurstEx>();
    }
}