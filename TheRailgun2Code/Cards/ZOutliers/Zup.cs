using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Zup() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(1)
    ];

    public override Material CreateCustomBannerMaterial => ShaderUtils.GenerateHsv(195.2f / 360f, .373f, 0.75f);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
        {
            await OrbCmd.Channel<LightningOrb>(choiceContext, Owner);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(1M);
}