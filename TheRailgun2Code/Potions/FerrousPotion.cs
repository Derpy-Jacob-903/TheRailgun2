using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Potions;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Potions;

public class FerrousPotion : TheRailgun2Potion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    public override string CustomPackedOutlinePath => ImageHelper.GetImagePath($"atlases/potion_outline_atlas.sprites/blood_potion.tres");
    public override string CustomPackedImagePath => ImageHelper.GetImagePath($"atlases/potion_atlas.sprites/blood_potion.tres");
    protected override IEnumerable<DynamicVar> CanonicalVars => [ new CardsVar(3), new EnergyVar(1) ];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        if (Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Count((Func<CardModel, bool>)(c => c.Tags.Contains(Enums.Ferrous))) == 0)
        {
            CardPileAddResult combat2 = await CardPileCmd.AddGeneratedCardToCombat(ModelDb.Card<IronWave>(), PileType.Hand, Owner);
        }
        else
        {
            CardModel card = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>) CardFactory.GetDistinctForCombat(Owner, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains(Enums.Ferrous))), 
                Math.Min(DynamicVars.Cards.IntValue, Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Count((Func<CardModel, bool>)(c => c.Tags.Contains(Enums.Ferrous)))), Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>(), Owner, true);
            if (card == null)
                return;
            CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
        foreach (CardModel cardModel in Owner.PlayerCombatState.AllCards
                     .Where<CardModel>((Func<CardModel, bool>)(c => c.Tags.Contains(Enums.Ferrous)))
                     .ToList<CardModel>())
        {
            //if (cardModel.EnergyCost. <= 0) continue;
            cardModel.EnergyCost.AddThisCombat(-1, true);
        }
    }
    
    
}