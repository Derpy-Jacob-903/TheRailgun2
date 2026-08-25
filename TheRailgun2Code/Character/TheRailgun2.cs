using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using TheRailgun2.TheRailgun2Code.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Character;

public class TheRailgun2 : PlaceholderCharacterModel
{
    public const string CharacterId = "TheRailgun2";

    public static readonly Color Color = new(51/255f, 173/255f, 1);

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;
    public override int BaseOrbSlotCount => 2;

    public override Color MapDrawingColor => new Color("0D8C66");
    public override Color DialogueColor => new Color("136B51");
    public override Color RemoteTargetingLineColor => new Color("6FEDC7FF");
    public override Color RemoteTargetingLineOutline => new Color("16634CFF");
    public override VfxColor SpeechBubbleColor => VfxColor.Cyan;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeRailgun>(),
        ModelDb.Card<StrikeRailgun>(),
        ModelDb.Card<StrikeRailgun>(),
        ModelDb.Card<StrikeRailgun>(),
        ModelDb.Card<Tap>(),
        ModelDb.Card<DefendRailgun>(),
        ModelDb.Card<DefendRailgun>(),
        ModelDb.Card<DefendRailgun>(),
        ModelDb.Card<DefendRailgun>(),
        ModelDb.Card<ElectronBurst>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<CoinRelicNew>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<TheRailgun2CardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheRailgun2RelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheRailgun2PotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    
    public override NCreatureVisuals CreateCustomVisuals()
    {
        return NodeFactory<NCreatureVisuals>.CreateFromScene("res://TheRailgun2/images/char/railgun.tscn");
    }
    //public override string CustomCharacterSelectBg => "res://TheRailgun2/images/char/selection_screen.tscn";
    public override string CustomMerchantAnimPath => "res://TheRailgun2/images/char/railgun.tscn";
}