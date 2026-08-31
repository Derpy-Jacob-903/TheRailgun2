using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class ProtonShower() : SpendCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5M, ValueProp.Move),
        ..MakeCalculatedVar("CalculatedHits", 1, (Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.PlayerCombatState!.AllCards.Count<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains(Enums.Spend)))))
    ]; 
    public override int canonicalSpendCost => 1;

    protected override async Task MyOnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (CombatState != null)
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .WithHitCount((int)((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(null))
                    .FromCard(play.Card, play).TargetingRandomOpponents(CombatState)
                    .WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}