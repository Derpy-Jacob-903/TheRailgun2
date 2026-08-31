using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Extensions;
using TheRailgun2.TheRailgun2Code.Powers;
using TheRailgun2.TheRailgun2Code.Relics;

namespace TheRailgun2.TheRailgun2Code.Character;

public static class EchoOrb
{
    //evil and fucked up this wasn't added
    //note to self: pr this to baselib
    // ReSharper disable once MemberCanBePrivate.Global
    public static async Task EvokeSpecific(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel evokedOrb,
        bool dequeue = true)
    {
        if (player.PlayerCombatState == null || evokedOrb == null)
            return;
        var orbQueue = player.PlayerCombatState.OrbQueue;
        if (!orbQueue.Orbs.Contains(evokedOrb))
            return;
        choiceContext.PushModel(evokedOrb);
        await Evoke2(choiceContext, player, evokedOrb, dequeue);
        choiceContext.PopModel(evokedOrb);
    }

    public static async Task EvokeFirstOf<OrbType>(
        PlayerChoiceContext choiceContext,
        Player player,
        bool dequeue = true)
        where OrbType : OrbModel
    {
        if (player.PlayerCombatState == null)
            return;
        var orbQueue = player.PlayerCombatState.OrbQueue;
        OrbModel orb = orbQueue.Orbs.OfType<OrbType>().FirstOrDefault();
        if (orb == null)
            return;
        choiceContext.PushModel(orb);
        await Evoke2(choiceContext, player, orb, dequeue);
        choiceContext.PopModel(orb);
    }

    public static async Task<bool> RemoveFirstOf<OrbType>(
        PlayerChoiceContext choiceContext,
        Player player)
        where OrbType : OrbModel
    {
        if (player.PlayerCombatState == null)
            return false;
        var orbQueue = player.PlayerCombatState.OrbQueue;
        OrbModel orb = orbQueue.Orbs.OfType<OrbType>().FirstOrDefault();
        if (orb == null)
            return false;
        choiceContext.PushModel(orb);
        await RemoveOrb(choiceContext, player, orb);
        choiceContext.PopModel(orb);
        return true;
    }
    
    public static async Task<int> EvokeAllOf<OrbType>(
        PlayerChoiceContext choiceContext, 
        Player player,
        Func<Task> task = null,
        bool dequeue = true)
        where OrbType : OrbModel
    {
        var count = 0;
        if (player.PlayerCombatState == null) return 0;
        var orbs = player.PlayerCombatState.OrbQueue.Orbs.ToList();
        foreach (var orb in orbs.OfType<OrbType>())
        {
            await RemoveOrb(choiceContext, player, orb);
            count++;
            if (task != null) await task();
        }
        return count;
    }
    
    public static async Task<int> RemoveAllOf<OrbType>(
        PlayerChoiceContext choiceContext, 
        Player player,
        Func<Task> task = null)
        where OrbType : OrbModel
    {
        var count = 0;
        if (player.PlayerCombatState == null) return 0;
        var orbs = player.PlayerCombatState.OrbQueue.Orbs.ToList();
        foreach (var orb in orbs.OfType<OrbType>())
        {
            await EvokeSpecific(choiceContext, player, orb);
            count++;
            if (task != null) await task();
        }
        return count;
    }

    private static async Task Evoke2(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel evokedOrb,
        bool dequeue = true)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
        if (player.PlayerCombatState == null || player.Creature.CombatState == null || orbQueue.Orbs.Count <= 0)
            return;
        bool removed = false;
        if (dequeue)
        {
            removed = orbQueue.Remove(evokedOrb);
            NCombatRoom.Instance?.GetCreatureNode(player.Creature)?.OrbManager?.EvokeOrbAnim(evokedOrb);
        }
        choiceContext.PushModel(evokedOrb);
        IEnumerable<Creature> targets = await evokedOrb.Evoke(choiceContext);
        choiceContext.PopModel(evokedOrb);
        await Hook.AfterOrbEvoked(choiceContext, player.Creature.CombatState, evokedOrb, targets);
        if (!removed)
            return;
        evokedOrb.RemoveInternal();
    }
    
    public static async Task RemoveOrb(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel evokedOrb)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
        if (player.PlayerCombatState == null || player.Creature.CombatState == null || orbQueue.Orbs.Count <= 0)
            return;
        var removed = orbQueue.Remove(evokedOrb);
        var nOrbManager = NCombatRoom.Instance?.GetCreatureNode(player.Creature)?.OrbManager;
        nOrbManager?.EvokeOrbAnim(evokedOrb);
        if (!removed)
            return;
        evokedOrb.RemoveInternal();
    }
}

interface ISpendHooks
{
    
}

public class VoltOrb : CustomOrbModel
{
    public override decimal PassiveVal => ModifyOrbValue(2m);
    public override decimal EvokeVal => ModifyOrbValue(3m);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<LockOnPower>()
    ];
    
    //how'd I make a character use a recolor of an existing character?

    public override Node2D CreateCustomSprite()
    {
        var container = new Node2D();
        string darkPath = SceneHelper.GetScenePath("orbs/orb_visuals/lightning_orb");
        Node2D dark = PreloadManager.Cache.GetScene(darkPath)
            .Instantiate<Node2D>();
        var sprite = new MegaSprite(dark.GetNode("SpineSkeleton"));
        ApplyTextureSkin(sprite, ImageTexture.CreateFromImage(Image.LoadFromFile("char/railgun_orbs".ImagePath())));
        
        sprite.GetAnimationState().SetAnimation("idle_loop");
        //dark.Modulate = new Color(0f, 1f, 4f, 1.0f);
        container.AddChild(dark);
        return container;
    }
    
    //bullshit from the CustomSkins mod
    static Shader _skinShader = null;
    static Shader GetSkinShader() => _skinShader ??= MakeSkinShader();

    static Shader MakeSkinShader()
    {
        _skinShader = new Shader();
        _skinShader.Code = """
                           shader_type canvas_item;
                           uniform sampler2D skin_texture;
                           varying vec4 modulate_color;
                           void vertex() { modulate_color = COLOR; }
                           void fragment() { COLOR = texture(skin_texture, UV) * modulate_color; }
                           """;
        return _skinShader;
    }
    
    static void ApplyTextureSkin(MegaSprite spineBody, Texture2D texture)
    {
        var shader = GetSkinShader();
        var mat = new ShaderMaterial();
        mat.Shader = shader;
        mat.SetShaderParameter("skin_texture", texture);
        spineBody.SetNormalMaterial(mat);

        // TODO UNDERSTAND HOW DOES THIS KIND OF FIX IT???
        // Tried using an llm to solve the shader issue it added this
        /*var addMat = new ShaderMaterial();
        addMat.Shader = shader;
        addMat.SetShaderParameter("skin_texture", texture);
        spineBody.BoundObject.Call("set_additive_material", addMat);*/
    }
    //bullshit ends
protected override string PassiveSfx => "event:/sfx/characters/defect/defect_lightning_passive";

  protected override string EvokeSfx => "event:/sfx/characters/defect/defect_lightning_evoke";

  protected override string ChannelSfx => "event:/sfx/characters/defect/defect_lightning_channel";

  public override Color DarkenedColor => new Color("7860a7");

  public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
  {
    await this.TriggerPassive(choiceContext, (Creature) null);
  }

  public override async Task Passive(PlayerChoiceContext choiceContext, Creature target)
  {
    ActivatePassive();
    await ApplyLightningDamage(PassiveVal, target, choiceContext, false);
  }

  public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
  {
    return await ApplyLightningDamage(EvokeVal, (Creature) null, playerChoiceContext, true);
  }

  public override decimal ModifyOrbValue(OrbModel orb, decimal value)
  {
      return orb == this && CombatState.GetOpponentsOf(Owner.Creature).Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList().Any(c => c.HasPower<LockOnPower>()) ? value + 1 : value;
  }

  private async Task<IEnumerable<Creature>> ApplyLightningDamage(
    Decimal value,
    Creature? target,
    PlayerChoiceContext choiceContext,
    bool isEvoke)
  {
    var list = CombatState.GetOpponentsOf(Owner.Creature).Where<Creature>((Func<Creature, bool>) (e => e.IsHittable)).ToList();
    if (!Owner.Creature.HasPower<ElectrodynamicsPower>() && list.Any(c => c.HasPower<LockOnPower>()))
    {
        list = list.Where(c => c.HasPower<LockOnPower>()).ToList();
    }
    if (list.Count == 0)
      return [];
    IReadOnlyList<Creature> targets = this.Owner.Creature.HasPower<ElectrodynamicsPower>() ? list : [target ?? Owner.RunState.Rng.CombatTargets.NextItem(list)] ;
    if (isEvoke)
      ActivateEvoke(targets.ToArray());
    foreach (Creature target1 in  targets)
      VfxCmd.PlayOnCreature(target1, "vfx/vfx_attack_lightning");
    PlayEvokeSfx();
     await CreatureCmd.Damage(choiceContext, targets, value, ValueProp.Unpowered & Enums.VoltOrb, Owner.Creature);
    return targets;
  }
}