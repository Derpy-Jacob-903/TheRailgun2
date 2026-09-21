using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Extensions;
using TheRailgun2.TheRailgun2Code.Powers;

namespace TheRailgun2.TheRailgun2Code.Cards;

[Pool(typeof(TokenCardPool))]
public class Needle() : CustomCardModel(0,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    public override TargetType TargetType => !this.HasFanOfKnives ? TargetType.AnyEnemy : TargetType.AllEnemies;
    public bool HasFanOfKnives => (Owner?.Creature?.HasPower<PiercingNeedlesPower>() ?? false);

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        Enums.Discharge,
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6M, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
            if (HasFanOfKnives && CombatState != null)
            {
                Creature lastEnemy = CombatState.HittableEnemies.LastOrDefault<Creature>();
                var attackCommand = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(cardPlay.Card, cardPlay).TargetingAllOpponents(CombatState).WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NShivThrowVfx.Create(Owner.Creature, lastEnemy, Colors.DarkCyan)));
                await attackCommand.Execute(choiceContext);
            }
            else
            {
                if (cardPlay.Target != null || (HasFanOfKnives && CombatState != null))
                {
                    var attackCommand = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .FromCard(cardPlay.Card, cardPlay).Targeting(cardPlay.Target)
                        .WithHitVfxNode((t) => NShivThrowVfx.Create(base.Owner.Creature, t, Colors.DarkCyan));
                    await attackCommand.Execute(choiceContext);
                }
            }
    }
    
    public static async Task<IEnumerable<CardModel>> CreateInHand(
        Player owner,
        int count,
        ICombatState combatState)
    {
        if (count == 0)
            return Array.Empty<CardModel>();
        if (CombatManager.Instance.IsOverOrEnding)
            return Array.Empty<CardModel>();
        List<CardModel> shivs = new List<CardModel>();
        for (int index = 0; index < count; ++index)
        {
            shivs.Add(combatState.CreateCard<Needle>(owner));
            //shivs[index].UpgradeInternal();
        }
        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat(shivs, PileType.Hand, owner);
        return shivs;
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
    
    private bool HasPortrait2 => ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}_p.png".CardImagePath());
    public override string CustomPortraitPath => HasPortrait2 ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_p.png".CardImagePath() : $"card_p.png".CardImagePath();
    public override string PortraitPath => this.HasPortrait ? $"{Id.Entry.ToLowerInvariant().RemovePrefix()}.png".CardImagePath() : BetaPortraitPath;
    public override string BetaPortraitPath => $"card_p.png".CardImagePath();
}