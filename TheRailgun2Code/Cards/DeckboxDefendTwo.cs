using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;
[Pool(typeof(DeprecatedCardPool))]
public class DeckboxDefendTwo() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    //protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(3m, ValueProp.Move),
        new BlockVar("Block2", 2m, ValueProp.Move),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedVar("Shit").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => CombatManager.Instance.History.Entries.OfType<CardPlayFinishedEntry>().Any(p => p.HappenedThisTurn(CombatState) && p.CardPlay.Card is DeckboxDefendTwo) ? 1 : 0))
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        for (int i = 0; i < DynamicVars["CalculatedHits"].BaseValue; i++)
        {
            await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        }
        else
        {
            if (Owner.PlayerCombatState?.OrbQueue?.Orbs?[0] != null)
                await OrbCmd.Passive(choiceContext, Owner.PlayerCombatState.OrbQueue.Orbs[0], null);
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(1M);
        this.DynamicVars["Block2"].UpgradeValueBy(1M);
    }
    
    [HarmonyPatch(typeof(CardModel), "get_BannerMaterialPath")]
    public static class FramePathPatch
    {
        static void Postfix(CardModel __instance, ref string __result)
        {
            if (__instance is DeckboxDefendTwo or Jolt && __instance.Rarity != CardRarity.Ancient)
            {
                __result = "res://materials/cards/banners/card_banner_quest_mat.tres";
            }
        }
    } 
}