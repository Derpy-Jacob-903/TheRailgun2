using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheRailgun2.TheRailgun2Code.Cards;
using TheRailgun2.TheRailgun2Code.Extensions;

namespace TheRailgun2.TheRailgun2Code.Powers;

public class PowerShotPower : TheRailgun2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

  protected override object InitInternalData() => (object) new Data();

  public override async Task AfterCardDrawnEarly(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    Data data;
    if (card.Owner.Creature != Owner)
      data = (Data) null;
    else if (!card.Tags.Contains<CardTag>(Enums.Spend))
    {
      data = (Data) null;
    }
    else
    {
      data = GetInternalData<Data>();
      bool flag = true;
      if (Owner.CombatState.HittableEnemies.All<Creature>((Func<Creature, bool>) (c => c.HpDisplay.IsInfinite())))
      {
        if (data.infiniteAutoPlaysThisTurn >= 9)
        {
          flag = false;
          if (!data.showedCapReachedMessage)
          {
            ThinkCmd.Play(new LocString("powers", "HELLRAISER_POWER.infiniteAutoPlayCapReached"), Owner);
            data.showedCapReachedMessage = true;
          }
        }
        ++data.infiniteAutoPlaysThisTurn;
      }
      else
        ResetInfiniteAutoPlayData();
      if (!flag)
      {
        data = (Data) null;
      }
      else
      {
        data.autoPlayingCards.Add(card);
        await CardCmd.AutoPlay(choiceContext, card, (Creature) null);
        data.autoPlayingCards.Remove(card);
        data = (Data) null;
      }
    }
  }

  public override Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.ResetInfiniteAutoPlayData();
    return Task.CompletedTask;
  }

  public override Task BeforeAttack(AttackCommand command)
  {
    if (!(GetInternalData<Data>().autoPlayingCards).Contains<AbstractModel>(command.ModelSource))
      return Task.CompletedTask;
    command.WithHitFx("vfx/hellraiser_attack_vfx", command.HitSfx, command.TmpHitSfx).WithAttackerAnim("Cast", command.Attacker.Player.Character.CastAnimDelay).SpawningHitVfxOnEachCreature().WithHitVfxSpawnedAtBase();
    return Task.CompletedTask;
  }

  public void ResetInfiniteAutoPlayData()
  {
    Data internalData = this.GetInternalData<Data>();
    internalData.infiniteAutoPlaysThisTurn = 0;
    internalData.showedCapReachedMessage = false;
  }

  public class Data
  {
    public readonly HashSet<CardModel> autoPlayingCards = new HashSet<CardModel>();
    public int infiniteAutoPlaysThisTurn;
    public bool showedCapReachedMessage;
  }
}