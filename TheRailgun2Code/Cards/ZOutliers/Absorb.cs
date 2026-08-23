using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Absorb() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)//, ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(1),
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        bool canEvoke = Owner.PlayerCombatState?.OrbQueue is not null && Owner.PlayerCombatState?.OrbQueue?.Orbs.Count > 0;
        await OrbCmd.EvokeNext(choiceContext, Owner);
        if (canEvoke || IsUpgraded)
        {
            await CommonActions.ApplySelf<StrengthPower>(choiceContext, this);
        }
    }

    //protected override void OnUpgrade() => this.DynamicVars.Strength.UpgradeValueBy(1M);
    //public CardModel GetTranscendenceTransformedCard()
    //{
        //return ModelDb.Card<AbsorbEx>();
    //}
}