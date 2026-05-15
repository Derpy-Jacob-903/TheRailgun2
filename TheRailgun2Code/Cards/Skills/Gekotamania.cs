using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheRailgun2.TheRailgun2Code.Cards;

public class Gekotamania() : TheRailgun2Card(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    private const string _increaseKey = "Increase";
    private int _currentBlock = 1;
    private int _increasedBlock;
    [SavedProperty]
    public int CurrentBlock
    {
        get => this._currentBlock;
        set
        {
            this.AssertMutable();
            this._currentBlock = value;
            this.DynamicVars.Block.BaseValue = (Decimal) this._currentBlock;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar((Decimal)this.CurrentBlock, ValueProp.Move),
        (DynamicVar)new IntVar("Increase", 1M)
    ];

    [SavedProperty]
    public int IncreasedBlock
    {
        get => this._increasedBlock;
        set
        {
            this.AssertMutable();
            this._increasedBlock = value;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        GeneticAlgorithm geneticAlgorithm = this;
        Decimal num = await CreatureCmd.GainBlock(geneticAlgorithm.Owner.Creature, geneticAlgorithm.DynamicVars.Block, cardPlay);
        int intValue = geneticAlgorithm.DynamicVars["Increase"].IntValue;
        geneticAlgorithm.BuffFromPlay(intValue);
        if (!(geneticAlgorithm.DeckVersion is GeneticAlgorithm deckVersion))
            return;
        deckVersion.BuffFromPlay(intValue);
    }

    protected override void OnUpgrade() => this.DynamicVars["Increase"].UpgradeValueBy(1M);

    protected override void AfterDowngraded() => this.UpdateBlock();

    private void BuffFromPlay(int extraBlock)
    {
        this.IncreasedBlock += extraBlock;
        this.UpdateBlock();
    }
    
    public override

    private void UpdateBlock() => this.CurrentBlock = 1 + this.IncreasedBlock;
}