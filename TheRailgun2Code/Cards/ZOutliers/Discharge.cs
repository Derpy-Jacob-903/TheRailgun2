using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Discharge() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self), ITranscendenceCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
         new PowerVar<DischargePower>(2)
    ];
    
    public override Material CreateCustomBannerMaterial => ShaderUtils.GenerateHsv(15.2f / 360f, .373f, 0.75f);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await OrbCmd.EvokeNext(choiceContext, Owner);
        await PowerCmd.Apply<DischargePower>(choiceContext, Owner.Creature, DynamicVars["DischargePower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars["DischargePower"].UpgradeValueBy(1M);
    public CardModel GetTranscendenceTransformedCard()
    {
        return ModelDb.Card<DischargeEx>();
    }
}