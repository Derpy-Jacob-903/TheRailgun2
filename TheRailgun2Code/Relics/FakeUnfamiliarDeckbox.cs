using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Relics;

[Pool(typeof(EventRelicPool))]
public class FakeUnfamiliarDeckbox() : TheRailgun2Relic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool HasUponPickupEffect => true;
    public static bool DoesCharacterHaveDeck(CharacterModel character)
    {
        return character is Character.TheRailgun2;
    }
    
    public static IEnumerable<RelicModel> GetValidRelics(Player owner)
    {
        return ModelDb.Event<Neow>().AllPossibleOptions.Where(o => o.Relic != null && o.Relic.IsAllowedAtNeow(owner) && !(o.Relic is FakeUnfamiliarDeckbox)).Select((o => o.Relic)).OfType<RelicModel>();
    }
    
    public override async Task AfterObtained()
    {
        List<CardModel> cardsToReturn = new List<CardModel>();
        foreach (CardModel card in (IEnumerable<CardModel>)PileType.Deck.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if (card.Rarity == CardRarity.Basic)
            {
                await CardPileCmd.RemoveFromDeck(card);
            }
            else
            {
                cardsToReturn.Add(card);
                await CardPileCmd.RemoveFromDeck(card);
            }
        }
        List<CardPileAddResult> results = new List<CardPileAddResult>();
        for (int i = 0; i < 4; ++i)
        {
            CardModel card = Owner.RunState.CreateCard(ModelDb.Card<StrikeRailgun>(), Owner);
            results.Add(await CardPileCmd.Add(card, PileType.Deck));
        }
        for (int i = 0; i < 4; ++i)
        {
            CardModel card = Owner.RunState.CreateCard(ModelDb.Card<DefendRailgun>(), Owner);
            results.Add(await CardPileCmd.Add(card, PileType.Deck));
        }
        CardModel zup = Owner.RunState.CreateCard(ModelDb.Card<Zup>(), this.Owner);
        results.Add(await CardPileCmd.Add(zup, PileType.Deck));
        CardModel jolt = Owner.RunState.CreateCard(ModelDb.Card<Jolt>(), this.Owner);
        results.Add(await CardPileCmd.Add(jolt, PileType.Deck));
        CardModel discharge = Owner.RunState.CreateCard(ModelDb.Card<Discharge>(), this.Owner);
        results.Add(await CardPileCmd.Add(discharge, PileType.Deck));
        foreach (CardModel card in cardsToReturn)
        {
            CardModel newCard = Owner.RunState.CreateCard(ModelDb.GetById<CardModel>(card.Id), Owner);
            results.Add(await CardPileCmd.Add(newCard, PileType.Deck));
        }
        foreach(CardPileAddResult toPreview in results)
        {
            CardCmd.PreviewCardPileAdd(toPreview, style: CardPreviewStyle.MessyLayout);
            await Cmd.CustomScaledWait(0.1f, 0.2f);
        }
        List<RelicModel> list1 = GetValidRelics(Owner).ToList();
        Owner.PlayerRng.Rewards.Shuffle(list1);
        List<Reward> list2 = list1.Take<RelicModel>(1).Select(relic => new RelicReward(relic, Owner)).ToList<Reward>();
        await new RewardsSet(Owner).WithCustomRewards(list2).Offer();
    }
}