using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace TheRailgun2.TheRailgun2Code.Cards;

[Pool(typeof(TheRailgun2CardPool))]
public abstract class TheRailgun2Card(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    private bool HasPortrait2 => ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}_p.png".CardImagePath());
    public override string CustomPortraitPath => HasPortrait2 ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_p.png".CardImagePath() : $"card_p.png".CardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => this.HasPortrait ? $"{Id.Entry.ToLowerInvariant().RemovePrefix()}.png".CardImagePath() : BetaPortraitPath;
    public override string BetaPortraitPath => $"card_p.png".CardImagePath();
}