using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class ZapRailgun() : TheRailgun2Card(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)//, ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(1),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
        {
            await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        }
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
    //public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<Fulminate>();
}