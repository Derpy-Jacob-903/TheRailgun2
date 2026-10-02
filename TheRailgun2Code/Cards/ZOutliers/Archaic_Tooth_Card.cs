using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Migrations.SerializableRuns;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Compat;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class AbsorbEx : TheRailgun2Card
{
    public AbsorbEx() : base(1,
        CardType.Skill, CardRarity.Ancient,
        TargetType.Self)
    {
        AncientsAwakenedCompat.RegisterExperimentalSerumCardForCoco(this);
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        //CardKeyword.Retain
        //Enums.Conduit
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(2),
        new PowerVar<PlatingPower>(2)
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        //bool canEvoke = Owner.PlayerCombatState?.OrbQueue is not null && Owner.PlayerCombatState?.OrbQueue?.Orbs.Count > 0;
        await OrbCmd.EvokeNext(choiceContext, Owner);
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, this);
        await CommonActions.ApplySelf<PlatingPower>(choiceContext, this);
    }
    
    protected override void OnUpgrade() {
        this.DynamicVars.Strength.UpgradeValueBy(1M);
        this.DynamicVars["PlatingPower"].UpgradeValueBy(1M);
    }
}