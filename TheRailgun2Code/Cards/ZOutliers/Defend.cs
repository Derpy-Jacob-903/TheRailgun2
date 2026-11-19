using BaseLib.Patches.Content;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheRailgun2.TheRailgun2Code.Character;
using TheRailgun2.TheRailgun2Code.Compat;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class DefendRailgunPerfect : TheRailgun2Card
{
    public DefendRailgunPerfect() : base(1,
        CardType.Skill, CardRarity.Token,
        TargetType.Self)
    {
        var ancientsAwakenedType =
            AccessTools.TypeByName("AncientsAwakened.AncientsAwakenedCode.Pools.Mithrix.PerfectedPool");
        if (ancientsAwakenedType == null) return;
        CustomContentDictionary.AddModel(GetType());
        this.AddPerfectedCardForCustomCharacters(ModelDb.Card<DefendRailgun>());
    }
    
    public override CardPoolModel VisualCardPool => AncientsAwakenedInterop.PerfectedCardPool(Pool);
    
    public override Material CreateCustomBannerMaterial => ShaderUtils.GenerateHsv(0.5f, 1f, 1f);

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(8m, ValueProp.Move),
        new RepeatVar(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, DynamicVars.Block, cardPlay);
        for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
        {
            await OrbCmd.Channel<VoltOrb>(choiceContext, Owner);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}