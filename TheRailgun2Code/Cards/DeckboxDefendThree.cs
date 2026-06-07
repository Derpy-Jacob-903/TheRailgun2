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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class DeckboxDefendThree() : TheRailgun2Card(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(8m, ValueProp.Move),
        new BlockVar("Block2", 2m, ValueProp.Move),
        new PowerVar<HotfixPower>(2),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        //new CalculatedBlockVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.PlayerCombatState != null ? card.Owner.PlayerCombatState.OrbQueue.Orbs.Count : 0)),
        new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.PlayerCombatState != null ? card.Owner.PlayerCombatState.OrbQueue.Orbs.Count : 0))
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        for (int i = 0; i < DynamicVars["CalculatedHits"].BaseValue; i++)
        {
            await CommonActions.CardBlock(this, DynamicVars["Block2"], cardPlay);
        }
        await PowerCmd.Apply<FocusedStrikePower>(choiceContext, Owner.Creature, DynamicVars["HotfixPower"].IntValue * -1, null, null);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(1M);
        this.DynamicVars["Block2"].UpgradeValueBy(1M);
    }
}
    
    [HarmonyPatch(typeof(CardModel), "get_BannerMaterialPath")]
    public static class FramePathPatch
    {
        static void Postfix(CardModel __instance, ref string __result)
        {
            if (__instance is DeckboxDefendThree or Jolt && __instance.Rarity != CardRarity.Ancient)
            {
                __result = "res://materials/cards/banners/card_banner_quest_mat.tres";
            }
        }
    }