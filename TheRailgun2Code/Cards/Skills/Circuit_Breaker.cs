using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class CircuitBreaker() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var p = Owner.Creature;
        var dex = 0;
        var str = 0;
        foreach (var pm in Owner.Creature.Powers.Where(model => model is StrengthPower))
        {
            dex += pm.Amount;
            str -= pm.Amount;
        }
        foreach (var pm in Owner.Creature.Powers.Where(model => model is DexterityPower))
        {
            dex -= pm.Amount;
            str += pm.Amount;
        }
        await PowerCmd.Apply<StrengthPower>(choiceContext, p, str, null, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, p, dex, null, this);
    }
    
    protected override bool ShouldGlowRedInternal => Owner.Creature.GetPowerAmount<StrengthPower>() == Owner.Creature.GetPowerAmount<DexterityPower>();

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}