using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Potions;
using TheRailgun2.TheRailgun2Code.Cards;

namespace TheRailgun2.TheRailgun2Code.Potions;

public class NeedlePotion : TheRailgun2Potion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    public override string CustomPackedOutlinePath => ImageHelper.GetImagePath($"atlases/potion_outline_atlas.sprites/star_potion.tres");
    public override string CustomPackedImagePath => ImageHelper.GetImagePath($"atlases/potion_atlas.sprites/star_potion.tres");
    protected override IEnumerable<DynamicVar> CanonicalVars => [ new CardsVar(2) ];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature target)
    {
        foreach (CardModel card in await Needle.CreateInHand(Owner, DynamicVars.Cards.IntValue, Owner.Creature.CombatState))
            CardCmd.Upgrade(card);
    }
}