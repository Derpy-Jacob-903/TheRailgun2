using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class AbsorbEx() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain
        //Enums.Conduit
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(2),
        new PowerVar<DexterityPower>(1)
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        bool canEvoke = Owner.PlayerCombatState?.OrbQueue is not null && Owner.PlayerCombatState?.OrbQueue?.Orbs.Count > 0;
        await OrbCmd.EvokeNext(choiceContext, Owner);
        if (canEvoke || IsUpgraded)
        {
            await CommonActions.ApplySelf<StrengthPower>(choiceContext, this);
            await CommonActions.ApplySelf<DexterityPower>(choiceContext, this);
        }
    }
    
    protected override void OnUpgrade() {
        this.DynamicVars.Strength.UpgradeValueBy(1M);
        this.DynamicVars.Dexterity.UpgradeValueBy(1M);
    }
}